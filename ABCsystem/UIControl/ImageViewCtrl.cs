using ABCsystem.Algorithm;
using ABCsystem.Core;
using ABCsystem.Teach;
using ABCsystem.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Point = System.Drawing.Point; // 추가: Point는 이제 System.Drawing.Point로 인식됨
using Size = System.Drawing.Size;   // 추가: Size는 이제 System.Drawing.Size로 인식됨
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABCsystem.UIControl
{
    //ROI 변경 작업 타입
    public enum EntityActionType    
    {
        None = 0,
        Select,
        Inspect,
        Add,
        Copy,
        Move,
        Resize,
        Delete,
        DeleteList,
        UpdateImage
    }

    //검사 양불판정 갯수를 화면에 표시하기 위한 구조체
    public struct InspectResultCount    
    {
        public int Total { get; set; }
        public int OK { get; set; }
        public int NG { get; set; }

        public InspectResultCount(int _totalCount, int _okCount, int _ngCount)
        {
            Total = _totalCount;
            OK = _okCount;
            NG = _ngCount;
        }
    }

    // 이미지 뷰어 컨트롤 클래스
    public partial class ImageViewCtrl: UserControl
    {
        //ROI를 추가,수정,삭제 등으로 변경 시, 이벤트 발생
        // ROI(관심영역) 추가, 수정, 삭제 등 상태 변화 시 외부로 알리는 이벤트
        public event EventHandler<DiagramEntityEventArgs> DiagramEntityEvent;

        // 컨트롤의 초기화 완료 여부 (중복 초기화 방지 및 렌더링 시점 제어)
        private bool _isInitialized = false;

        // 원본 이미지 객체 (이미지 프로세싱 및 렌더링의 소스)
        private Bitmap _bitmapImage = null;

        // [Double Buffering] 화면 깜빡임 방지를 위한 메모리 내 도화지(Back Buffer)
        private Bitmap Canvas = null;

        // 원본 이미지가 전체 화면(Control) 내에서 그려질 실제 영역과 위치 (Zoom/Pan 반영)
        private RectangleF ImageRect = new RectangleF(0, 0, 0, 0);

        // 현재 화면 확대/축소 배율 (1.0 = 100%)
        private float _curZoom = 1.0f;
        // 마우스 휠 등을 이용한 확대/축소 시 적용되는 가중치 (1.1 = 10%씩 변화)
        private float _zoomFactor = 1.1f;

        // 줌 배율의 최소/최대 한계값 설정
        private float MinZoom = 1.0f;
        private const float MaxZoom = 100.0f;

        // 검사 정보(검사 영역 및 결과 등)를 저장하는 리스트
        private List<DrawInspectInfo> _rectInfos = new List<DrawInspectInfo>();

        // 현재 컨트롤의 작업 상태 (예: "Idle", "Drawing", "Editing" 등)
        public string WorkingState { get; set; } = "";

        // 검사 결과(OK/NG 등)의 통계 수치를 관리하는 객체
        private InspectResultCount _inspectResultCount = new InspectResultCount();

        // [ROI 편집] 새로운 ROI를 그릴 때 시작 좌표
        private Point _roiStart = Point.Empty;
        // [ROI 편집] 현재 그려지거나 선택된 ROI의 사각형 영역
        private Rectangle _roiRect = Rectangle.Empty;

        // [상태 제어] 마우스 드래그를 통한 ROI 생성/크기조절/이동 여부 플래그
        private bool _isSelectingRoi = false;
        private bool _isResizingRoi = false;
        private bool _isMovingRoi = false;

        // [상태 제어] 크기 조절 및 이동 시작 시점의 좌표 저장
        private Point _resizeStart = Point.Empty;
        private Point _moveStart = Point.Empty;

        // [핸들러] 8방향 크기 조절 중 현재 선택된 방향 인덱스
        private int _resizeDirection = -1;
        // ROI 테두리에 표시될 크기 조절용 핸들(Square)의 크기
        private const int _ResizeHandleSize = 10;

        // 생성할 새 ROI의 타입 (Rect, Circle, Line 등)
        private InspWindowType _newRoiType = InspWindowType.None;

        // 화면상의 모든 그래픽 개체(ROI, Line 등)를 관리하는 마스터 리스트
        private List<DiagramEntity> _diagramEntityList = new List<DiagramEntity>();

        // [멀티 선택] 현재 마우스 드래그나 Ctrl+클릭으로 선택된 개체들
        private List<DiagramEntity> _multiSelectedEntities = new List<DiagramEntity>();
        // [복사/붙여넣기] 클립보드 역할을 하는 엔티티 버퍼
        private List<DiagramEntity> _copyBuffer = new List<DiagramEntity>();
        // 마우스의 현재 좌표 (실시간 렌더링 및 툴팁 표시용)
        private Point _mousePos;

        // 현재 단일 선택된 엔티티
        private DiagramEntity _selEntity;
        // 선택된 엔티티를 강조할 색상
        private Color _selColor = Color.White;

        // [드래그 선택] 마우스 드래그로 범위를 지정할 때 그려지는 사각형 영역
        private Rectangle _selectionBox = Rectangle.Empty;
        // [드래그 선택] 현재 영역 선택 중인지 여부
        private bool _isBoxSelecting = false;
        // [키 조합] 컨트롤키가 눌려있는지 여부 (다중 선택 로직용)
        private bool _isCtrlPressed = false;
        // 화면(Screen) 좌표계 기준의 선택 영역
        private Rectangle _screenSelectedRect = Rectangle.Empty;

        // 외부 여백 또는 추가 확장 사이즈 설정
        private Size _extSize = new Size(0, 0);

        // [라인 측정] 일반 라인 그리기 기능 활성화 여부
        private bool _drawLineEnabled = false;
        // [라인 측정] 수직 높이 측정을 위한 데이터 세트 리스트 (점들의 집합)
        private List<DiagramEntity[]> _heightLineList = new List<DiagramEntity[]>();
        // [라인 측정] 수직 높이 측정선 렌더링 활성화 여부
        private bool _drawVerticalEnabled = false;

        // 우클릭 시 나타날 메뉴 (삭제, 복사, 설정 등)
        private ContextMenuStrip _contextMenu;

        // 멀티스레드 환경에서 데이터(List 등) 접근 시 충돌 방지를 위한 잠금 객체
        private readonly object _lock = new object();

        private List<InspWindow> _inspWindowList = new List<InspWindow>();

        public void SetInspWindowList(List<InspWindow> inspWindowList)
        {
            // 외부(InspStage 등)에서 받은 리스트를 컨트롤 내부 변수에 저장
            this._inspWindowList = inspWindowList;

            // 화면을 다시 그리도록 명령 (OnPaint 호출)
            this.Invalidate();
        }

        // 생성자
        public ImageViewCtrl()
        {
            InitializeComponent();
            initializeCanvas();

            _contextMenu = new ContextMenuStrip();
            _contextMenu.Items.Add("Delete", null, OnDeleteClicked);
            _contextMenu.Items.Add(new ToolStripSeparator()); //구분선
            _contextMenu.Items.Add("Teaching", null, OnTeachingClicked);
            _contextMenu.Items.Add("Unlock", null, OnUnlockClicked);
            _contextMenu.Items.Add(new ToolStripSeparator()); //구분선
            _contextMenu.Items.Add("DrawHeightLine", null, OnDrawHeightLineClicked); 
            

            MouseWheel += new MouseEventHandler(ImageViewCCtrl_MouseWheel);
        }


        //캔버스 초기화 및 설정
        private void initializeCanvas() 
        {
            ResizeCanvas(); //캔버스 userControl 크기만큼 생성
            DoubleBuffered = true;  //깜빡임 방지 더블 버퍼 설정
        }

        //InspWindowType에 따른 색상 반환 함수
        public Color GetWindowColor(InspWindowType inspWindowType)
        {
            Color color = Color.LightBlue;

            switch (inspWindowType)
            {
                case InspWindowType.Base:
                    color = Color.LightBlue;
                    break;
                case InspWindowType.Body:
                    color = Color.Yellow;
                    break;
                case InspWindowType.Sub:
                    color = Color.Orange;
                    break;
                case InspWindowType.ID:
                    color = Color.Magenta;
                    break;
            }

            return color;
        }

        //새로운 ROI 추가 시작 함수
        public void NewRoi(InspWindowType inspWindowType)
        {
            _newRoiType = inspWindowType;
            _selColor = GetWindowColor(inspWindowType);
            Cursor = Cursors.Cross;
        }

        //줌에 따른 좌표 계산 기능 수정 
        private void ResizeCanvas()
        {
            if (Width <= 0 || Height <= 0 || _bitmapImage == null) return;

            Canvas = new Bitmap(Width, Height); // 캔버스를 UserControl 크기만큼 생성
            if (Canvas == null) return;

            float virtualWidth = _bitmapImage.Width * _curZoom;
            float virtualHeight = _bitmapImage.Height * _curZoom;

            float offsetX = virtualWidth < Width ? (Width - virtualWidth) / 2f : 0f;
            float offsetY = virtualHeight < Height ? (Height - virtualHeight) / 2f : 0f;

            ImageRect = new RectangleF(offsetX, offsetY, virtualWidth, virtualHeight);
        }

        // 이미지 로딩 함수
        public void LoadBitmap(Bitmap bitmap)
        {
            if (this.InvokeRequired)    //스레드에서 검사시, 멈추는 현상 방지
            {
                this.BeginInvoke(new Action<Bitmap>(LoadBitmap), bitmap); return;
            }
            if (_bitmapImage != null)   // 기존에 로드된 이미지가 있다면 해제 후 초기화, 메모리누수 방지
            {
                if (_bitmapImage.Width == bitmap.Width && _bitmapImage.Height == bitmap.Height) //이미지 크기가 같다면, 이미지 변경 후, 화면 갱신
                {
                    _bitmapImage.Dispose();   // 기존 이미지 해제 후 교체
                    _bitmapImage = bitmap;
                    Invalidate();
                    return;
                }
                _bitmapImage.Dispose(); //birmap 객체가 사요하던 메모리 리소스 해제
                _bitmapImage = null;  //객체 해제하여 GC을 수집할 수 있도록 설정
            }
            _bitmapImage = bitmap;  //새 이미지 로드;
            if (_isInitialized == false)    ////bitmap==null 예외처리도 초기화되지않은 변수들 초기화
            {
                _isInitialized = true;
                ResizeCanvas();
            }
            FitImageToScreen();
        }

        // 이미지 화면에 맞게 조정 함수
        private void FitImageToScreen()
        {
            if (_bitmapImage == null) return;

            RecalcZoomRatio();  

            float NewWidth = _bitmapImage.Width * _curZoom;
            float NewHeight = _bitmapImage.Height * _curZoom;

            ImageRect = new RectangleF( //이미지가 UserControl중앙에 배치되도록 정렬
                (Width - NewWidth) / 2,
                (Height - NewHeight) / 2,
                NewWidth,
                NewHeight
            );

            Invalidate();   //내부 함수, 화면 갱신 기능
        }

        // 줌 비율 재계산 함수
        private void RecalcZoomRatio()
        {
            if (_bitmapImage == null || Width <= 0 || Height <= 0) return;

            Size imageSize = new Size(_bitmapImage.Width, _bitmapImage.Height);

            float aspectRatio = (float)imageSize.Height / (float)imageSize.Width;
            float clientAspect = (float)Height / (float)Width;

            float ratio;

            if (aspectRatio <= clientAspect)
                ratio = (float)Width / (float)imageSize.Width;
            else
                ratio = (float)Height / (float)imageSize.Height;

            float minZoom = ratio;

            MinZoom = minZoom;

            _curZoom = Math.Max(MinZoom, Math.Min(MaxZoom, ratio)); //min, max값을 벗어나지 않게 설정

            Invalidate();   //내부 함수, 화면 갱신 기능
        }

        // Windows Forms에서 컨트롤이 다시 그려질 때 자동으로 호출되는 메서드
        // 화면새로고침(Invalidate()), 창 크기변경, 컨트롤이 숨겨졌다가 나타날때 실행
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); //base.____:부모 클래스의 것을 가져다 씀

            if (_bitmapImage != null && Canvas != null)
            {
                using (Graphics g = Graphics.FromImage(Canvas))  //캔버스 초기화, 이미지 그리기
                {
                    g.Clear(Color.Transparent); //배경을 투명하게

                    g.InterpolationMode = InterpolationMode.NearestNeighbor;    //이미지 확대or축소때 화질 최적화 방식(Interpolation Mode) 설정   
                    g.DrawImage(_bitmapImage, ImageRect);

                    DrawDiagram(g);
                    e.Graphics.DrawImage(Canvas, 0, 0); //캔버스를 UserControl 화면에 표시
                }
            }
            DrawHeightLine(e.Graphics); //새 수직선 (ROI3 기준) 
        }

        //EdgeAlgorithm에서 검출된 엣지 포인트를 가져오는 함수
        private PointF GetEdgePoint(DiagramEntity entity)
        {
            if (entity?.LinkedWindow != null)   //1. 엔티티에 연결된 InspWindow 확인
            {
                // 2. InspWindow 내의 알고리즘 목록에서 EdgeAlgorithm 추출
                // (InspWindow 클래스 정의에 따라 AlgorithmList 또는 InspAlgorithmList일 수 있습니다)
                var edgeAlgo = entity.LinkedWindow.AlgorithmList // <-- 에러 시 변수명 확인 필요
                    .OfType<EdgeAlgorithm>()
                    .FirstOrDefault();

                if (edgeAlgo != null && edgeAlgo.IsInspected)
                {
                    var pt = edgeAlgo.FoundEdgePoint; // EdgeAlgorithm.cs에 추가한 public property
                    if (pt.X >= 0 && pt.Y >= 0) return new PointF(pt.X, pt.Y);
                }
            }
            // [Fallback] 검사에 실패했거나 유효한 엣지를 찾지 못한 경우
            // 호출 측에서 이 값을 체크하여 렌더링 여부를 결정
            return new PointF(-1, -1); // 유효하지 않은 좌표
        }


        // 수직 높이 라인 그리기 함수
        public void DrawHeightLine(Graphics g)
        {
            //SLogger.Write($"[DrawHeightLine] Enabled={_drawVerticalEnabled}, Count={_heightLineList.Count}");
            // 1. 기초 검사: 그리기 비활성화 상태거나 데이터가 없으면 즉시 종료
            if (_drawVerticalEnabled == false || _heightLineList.Count == 0) return;

            // 선의 품질을 높이기 위한 안티앨리어싱 설정
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // 폰트 설정 (수치 표시용 및 상단 결과 표시용)
            Font font = new Font("Arial", 10, FontStyle.Bold);
            Font hugeFont = new Font("Arial", 50, FontStyle.Bold);

            // 전체 검사 결과 상태 저장용
            string finalStatus = "";
            Color statusColor = Color.Red;

            for (int i = 0; i < _heightLineList.Count; i++)
            {
                var lineSet = _heightLineList[i];
                // [좌표 추출] 알고리즘이 찾은 실제 이미지상의 엣지 포인트(Virtual Coord)
                PointF vP1 = GetEdgePoint(lineSet[0]);  //가로선 기준점 1
                PointF vP2 = GetEdgePoint(lineSet[1]);  //가로선 기준점 2
                PointF vP3 = GetEdgePoint(lineSet[2]);  //세로선 시작점

                // [수치 계산]
                float dx = vP2.X - vP1.X;   //가로선의 X축 길이
                float targetY = vP1.Y;  //세로선이 만나는 Y좌표 초기값

                if (Math.Abs(dx) > 0.0001f) //분모 0 방지
                {
                    targetY = vP1.Y + ((vP2.Y - vP1.Y) / dx) * (vP3.X - vP1.X);
                }

                float pixelLength = Math.Abs(targetY - vP3.Y);
                if (pixelLength < 300 || pixelLength > 700) //정상 수치가 358px 정도이므로, 이미지가 넘어갈 때 발생하는 159px 같은 엉뚱한 수치는 화면에 그리지 않도록 차단
                {
                    continue;
                }


                // [단계 1] 판정 로직 적용 (수정된 기준)
                string currentLineStatus = "NG";
                Color lineColor = Color.Red;

                if (pixelLength >= 500) //NO CAP 기준
                {
                    currentLineStatus = "NO CAP";
                    lineColor = Color.Red;
                }
                else if (pixelLength >= 350 && pixelLength <= 360)  //OK 기준
                {
                    currentLineStatus = "OK";
                    lineColor = Color.Lime;
                }
                else
                {
                    currentLineStatus = "NG";
                    lineColor = Color.Red;
                }

                // [단계 2] 전체 결과 업데이트 (우선순위: NO CAP > NG > OK)   
                if (currentLineStatus == "NO CAP")
                {
                    finalStatus = "NO CAP";
                    statusColor = Color.Red;
                }
                else if (finalStatus != "NO CAP") // 이미 NO CAP 판정이 났다면 유지
                {
                    if (currentLineStatus == "NG")
                    {
                        finalStatus = "NG";
                        statusColor = Color.Red;
                    }
                    else if (finalStatus == "") // 아무 판정도 없었을 때만 OK로 초기화
                    {
                        finalStatus = "OK";
                        statusColor = Color.Lime;
                    }
                }

                // [단계 3] 화면에 선 및 개별 수치 그리기
                PointF sP1 = VirtualToScreen(vP1);  //가로선 기준점 1
                PointF sP2 = VirtualToScreen(vP2);  //가로선 기준점 2
                PointF sStart = VirtualToScreen(vP3);   //세로선 시작점
                PointF sEnd = VirtualToScreen(new PointF(vP3.X, targetY));  //세로선 끝점

                Color drawColor = (pixelLength > 500) ? Color.White : lineColor;    //500px 초과 시 흰색으로 그리기

                using (Pen bluePen = new Pen(Color.Blue, 2f))   //가로선 그리기
                {
                    g.DrawLine(bluePen, sP1, sP2);
                }

                using (Pen p = new Pen(drawColor, 2f))  //세로선 그리기
                {
                    // 선 그리기
                    g.DrawLine(p, sStart, sEnd);

                    // 글자 그리기 (메모리 효율을 위해 브러시도 using 사용 권장)
                    using (SolidBrush textBrush = new SolidBrush(drawColor))
                    {
                        g.DrawString($"{pixelLength:F2} px", font, textBrush, sEnd.X + 5, (sStart.Y + sEnd.Y) / 2);
                    }
                }
            }

            // [최종] 상단에 전체 판정 글씨 쓰기
            if (!string.IsNullOrEmpty(finalStatus))
            {
                DrawStatusText(g, finalStatus, hugeFont, statusColor, 20, 20);
            }
        }


        // 화면에 판정 텍스트를 그리는 별도 함수 (그림자 효과 포함)
        private void DrawStatusText(Graphics g, string text, Font font, Color color, float x, float y)
        {
            // 그림자 (검은색)
            g.DrawString(text, font, Brushes.Black, x + 3, y + 3);
            // 본문 글씨
            using (SolidBrush brush = new SolidBrush(color))
            {
                g.DrawString(text, font, brush, x, y);
            }
        }

        // 다이어그램 그리기 함수
        private void DrawDiagram(Graphics g)
        {
            // ROI 그리기
            // 선택된 모든 ROI들을 포함하는 최소 사각형 영역을 초기화
            _screenSelectedRect = new Rectangle(0, 0, 0, 0);
            // 1. 관리 리스트에 있는 모든 엔티티(ROI)를 순회하며 그리기
            foreach (DiagramEntity entity in _diagramEntityList)
            {
                Rectangle screenRect = VirtualToScreen(entity.EntityROI);   // 원본 이미지 기준의 ROI 좌표를 현재 화면(Zoom/Pan 반영) 좌표로 변환
                using (Pen pen = new Pen(entity.EntityColor, 2))
                {
                    // 2. 다중 선택된 상태인지 확인
                    if (_multiSelectedEntities.Contains(entity))
                    {
                        pen.DashStyle = DashStyle.Dash; // 선택된 개체는 점선으로 표시하여 강조
                        pen.Width = 2;

                        if (_screenSelectedRect.IsEmpty)    // 선택된 모든 ROI를 감싸는 전체 영역(Union) 계산
                        {
                            _screenSelectedRect = screenRect;
                        }
                        else
                        {
                            _screenSelectedRect = Rectangle.Union(_screenSelectedRect, screenRect); // 기존 선택 영역과 현재 ROI 영역을 합쳐서 확장된 사각형 생성
                        }
                    }
                    g.DrawRectangle(pen, screenRect);// 실질적인 ROI 사각형 그리기
                }
                
                if (_multiSelectedEntities.Count <= 1 && entity == _selEntity)  //선택된 ROI가 있다면, 리사이즈 핸들 그리기
                {
                    using (Brush brush = new SolidBrush(Color.LightBlue))   // 리사이즈 핸들 그리기 (8개 포인트: 4 모서리 + 4 변 중간)
                    {
                        Point[] resizeHandles = GetResizeHandles(screenRect);
                        foreach (Point handle in resizeHandles)
                        {
                            g.FillRectangle(brush, handle.X - _ResizeHandleSize / 2, handle.Y - _ResizeHandleSize / 2, _ResizeHandleSize, _ResizeHandleSize);
                        }
                    }
                }
            }

            //선택된 개별 roi가 없고, 여러개가 선택되었다면
            if (_multiSelectedEntities.Count > 1 && !_screenSelectedRect.IsEmpty)
            {
                using (Pen pen = new Pen(Color.White, 2))
                {
                    g.DrawRectangle(pen, _screenSelectedRect);
                }

                // 리사이즈 핸들 그리기 (8개 포인트: 4 모서리 + 4 변 중간)
                using (Brush brush = new SolidBrush(Color.LightBlue))
                {
                    Point[] resizeHandles = GetResizeHandles(_screenSelectedRect);
                    foreach (Point handle in resizeHandles)
                    {
                        g.FillRectangle(brush, handle.X - _ResizeHandleSize / 2, handle.Y - _ResizeHandleSize / 2, _ResizeHandleSize, _ResizeHandleSize);
                    }
                }
            }

            //신규 ROI 추가할때, 해당 ROI 그리기
            if (_isSelectingRoi && !_roiRect.IsEmpty)
            {
                Rectangle rect = VirtualToScreen(_roiRect);
                using (Pen pen = new Pen(_selColor, 2))
                {
                    g.DrawRectangle(pen, rect);
                }
            }

            if (_multiSelectedEntities.Count <= 1 && _selEntity != null)
            {
                //#11_MATCHING#8 패턴매칭할 영역 표시
                DrawInspParam(g, _selEntity.LinkedWindow);
            }

            //선택 영역 박스 그리기
            if (_isBoxSelecting && !_selectionBox.IsEmpty)
            {
                using (Pen pen = new Pen(Color.LightSkyBlue, 3))
                {
                    pen.DashStyle = DashStyle.Dash;
                    pen.Width = 2;
                    g.DrawRectangle(pen, _selectionBox);
                }
            }

            lock (_lock)
            {
                DrawRectInfo(g);
            }

            //#17_WORKING_STATE#4 작업 상태 화면에 표시
            if (WorkingState != "")
            {
                float fontSize = 20.0f;
                Color stateColor = Color.FromArgb(255, 128, 0);
                PointF textPos = new PointF(20,Height-40);
                DrawText(g, WorkingState, textPos, fontSize, stateColor);
            }

            //#13_INSP_RESULT#5 검사 양불판정 갯수 화면에 표시
            if (_inspectResultCount.Total > 0)
            {
                string resultText = $"Total: {_inspectResultCount.Total}\r\nOK: {_inspectResultCount.OK}\r\nNG: {_inspectResultCount.NG}";

                float fontSize = 12.0f;
                Color resultColor = Color.FromArgb(255, 255, 255);
                PointF textPos = new PointF(Width -120, 10);
                DrawText(g, resultText, textPos, fontSize, resultColor);
            }
        }

        // 검사 정보(사각형 및 점 등) 그리기 함수
        private void DrawRectInfo(Graphics g)
        {
            if (_rectInfos == null || _rectInfos.Count <= 0) return;    //검사 정보가 없으면 종료

            // 이미지 좌표 → 화면 좌표 변환 후 사각형 그리기
            foreach (DrawInspectInfo rectInfo in _rectInfos)
            {
                Color lineColor = Color.LightCoral;
                if (rectInfo.decision == DecisionType.Defect)
                    lineColor = Color.Red;
                else if (rectInfo.decision == DecisionType.Good)
                    lineColor = Color.LightGreen;
                else if (rectInfo.decision == DecisionType.Info)
                    lineColor = Color.Yellow; // 엣지 윤곽선 추출

                //점 표시
                if (rectInfo.UsePoint)
                {
                    PointF screenPt = VirtualToScreen(
                        new PointF(rectInfo.point.X, rectInfo.point.Y)
                    );

                    //float r = Math.Max(4.0f, 6.0f * _curZoom);

                    float r;
                    if (rectInfo.decision == DecisionType.Info)
                    {
                        // 노란 점(윤곽용): 작게 + 줌 따라가되 너무 안 커지게
                        r = Math.Max(1.5f, 2.0f * _curZoom);
                        r = Math.Min(r, 4.0f); // 상한(너무 커지는거 방지)
                    }
                    else
                    {
                        // 대표점(초록/기타): 기존 유지
                        r = Math.Max(4.0f, 6.0f * _curZoom);
                    }

                    using (Brush b = new SolidBrush(lineColor))
                    {
                        g.FillEllipse(b, screenPt.X - r, screenPt.Y - r, r * 2, r * 2);
                    }

                    // 텍스트 - Info(노란 윤곽점)는 텍스트 표시 안 함
                    if (!string.IsNullOrEmpty(rectInfo.info) && rectInfo.decision != DecisionType.Info)
                    {
                        DrawText(g, rectInfo.info, new PointF(screenPt.X + r, screenPt.Y), 12.0f * _curZoom, lineColor);
                    }

                    continue;
                }

                Rectangle rect = new Rectangle(rectInfo.rect.X, rectInfo.rect.Y, rectInfo.rect.Width, rectInfo.rect.Height);
                Rectangle screenRect = VirtualToScreen(rect);

                using (Pen pen = new Pen(lineColor, 2))
                {
                    if (rectInfo.UseRotatedRect)
                    {
                        PointF[] screenPoints = rectInfo.rotatedPoints
                                                .Select(p => VirtualToScreen(new PointF(p.X, p.Y))) // 화면 좌표계로 변환
                                                .ToArray();

                        if (screenPoints.Length == 4)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                g.DrawLine(pen, screenPoints[i], screenPoints[(i + 1) % 4]); // 시계방향으로 선 연결
                            }
                        }
                    }
                    else
                    {
                        g.DrawRectangle(pen, screenRect);
                    }
                }

                if (rectInfo.info != "")
                {
                    float baseFontSize = 20.0f;

                    if (rectInfo.decision == DecisionType.Info)
                    {
                        baseFontSize = 3.0f;
                        lineColor = Color.LightBlue;
                    }

                    float fontSize = baseFontSize * _curZoom;

                    // 스코어 문자열 그리기 (우상단)
                    string infoText = rectInfo.info;
                    PointF textPos = new PointF(screenRect.Left, screenRect.Top); // 위로 약간 띄우기

                    if (rectInfo.inspectType == InspectType.InspBinary
                        && rectInfo.decision != DecisionType.Info)
                    {
                        textPos.Y = screenRect.Bottom - fontSize;
                    }

                    DrawText(g, infoText, textPos, fontSize, lineColor);
                }
            }
        }


        private void DrawText(Graphics g, string text, PointF position, float fontSize, Color color)
        {
            using (Font font = new Font("Arial", fontSize, FontStyle.Bold))
            using (Brush outlineBrush = new SolidBrush(Color.Black))
            using (Brush textBrush = new SolidBrush(color))
            {
                for (int dx = -1; dx <= 1; dx++)
                {
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (dx == 0 && dy == 0) continue; // 가운데는 제외
                        PointF borderPos = new PointF(position.X + dx, position.Y + dy);
                        g.DrawString(text, font, outlineBrush, borderPos);
                    }
                }

                g.DrawString(text, font, textBrush, position);
            }
        }
        public void UpdateInspParam()
        {
            _extSize.Width = _extSize.Height = 0;

            if (_selEntity == null)
                return;

            InspWindow window = _selEntity.LinkedWindow;
            if (window == null)
                return;

            MatchAlgorithm matchAlgo = (MatchAlgorithm)window.FindInspAlgorithm(InspectType.InspMatch);
            if (matchAlgo != null)
            {
                _extSize.Width = matchAlgo.ExtSize.Width;
                _extSize.Height = matchAlgo.ExtSize.Height;
            }
        }

        private void DrawInspParam(Graphics g, InspWindow window)
        {
            if (_extSize.Width > 0 || _extSize.Height > 0)
            {
                Rectangle extArea = new Rectangle(_roiRect.Left - _extSize.Width,
                    _roiRect.Top - _extSize.Height,
                    _roiRect.Width + _extSize.Width * 2,
                    _roiRect.Height + _extSize.Height * 2);
                Rectangle screenRect = VirtualToScreen(extArea);

                using (Pen pen = new Pen(Color.White, 2))
                {
                    pen.DashStyle = DashStyle.Dot;
                    pen.Width = 2;
                    g.DrawRectangle(pen, screenRect);
                }
            }
        }
        private void ImageViewCtrl_MouseDown(object sender, MouseEventArgs e)
        {
            _isCtrlPressed = (ModifierKeys & Keys.Control) == Keys.Control; //Ctrl 키 눌림 여부 판단

            if (e.Button == MouseButtons.Left) // 마우스 왼쪽 버튼이 눌렸을 때
            {
                if (_newRoiType != InspWindowType.None) //새로운 ROI 추가 모드일때
                {
                    _roiStart = e.Location; //ROI 시작점 저장
                    _isSelectingRoi = true; //ROI 선택중 상태로 변경
                    _selEntity = null; //선택된 엔티티 초기화
                }
                else
                {
                    if (!_isCtrlPressed && _multiSelectedEntities.Count > 1 && _screenSelectedRect.Contains(e.Location))
                    {
                        _selEntity = _multiSelectedEntities[0];
                        _isMovingRoi = true;
                        _moveStart = e.Location;
                        _roiRect = _selEntity.EntityROI;
                        Invalidate();
                        return;
                    }

                    if (_selEntity != null && !_selEntity.IsHold)
                    {
                        Rectangle screenRect = VirtualToScreen(_selEntity.EntityROI);
                        //마우스 클릭 위치가 ROI 크기 변경을 하기 위한 위치(모서리,엣지)인지 여부 판단
                        _resizeDirection = GetResizeHandleIndex(screenRect, e.Location);
                        if (_resizeDirection != -1)
                        {
                            _isResizingRoi = true;
                            _resizeStart = e.Location;
                            Invalidate();
                            return;
                        }
                    }

                    _selEntity = null;
                    foreach (DiagramEntity entity in _diagramEntityList)
                    {
                        Rectangle screenRect = VirtualToScreen(entity.EntityROI);
                        if (!screenRect.Contains(e.Location))
                            continue;

                        //컨트롤키를 이용해, 개별 ROI 추가/제거
                        if (_isCtrlPressed)
                        {
                            if (_multiSelectedEntities.Contains(entity))
                                _multiSelectedEntities.Remove(entity);
                            else
                                AddSelectedROI(entity);
                        }
                        else
                        {
                            _multiSelectedEntities.Clear();
                            AddSelectedROI(entity);
                        }

                        _selEntity = entity;
                        _roiRect = entity.EntityROI;
                        _isMovingRoi = true;
                        _moveStart = e.Location;

                        UpdateInspParam();
                        break;
                    }

                    if (_selEntity == null && !_isCtrlPressed)
                    {
                        _isBoxSelecting = true;
                        _roiStart = e.Location;
                        _selectionBox = new Rectangle();
                    }

                    Invalidate();
                }
            }
            else if (e.Button == MouseButtons.Right) Focus();

        }

        private void ImageViewCtrl_MouseMove(object sender, MouseEventArgs e)
        {
            _mousePos = e.Location;

            if (e.Button == MouseButtons.Left) //마우스 왼쪽 버튼이 눌린 상태에서 마우스가 움직일 때
            {
                if (_isSelectingRoi) //새로운 ROI 선택중
                {
                    int x = Math.Min(_roiStart.X, e.X);
                    int y = Math.Min(_roiStart.Y, e.Y);
                    int width = Math.Abs(e.X - _roiStart.X);
                    int height = Math.Abs(e.Y - _roiStart.Y);
                    _roiRect = ScreenToVirtual(new Rectangle(x, y, width, height));
                    Invalidate();
                }

                else if (_isResizingRoi)
                {
                    ResizeROI(e.Location);
                    if (_selEntity != null)
                        _selEntity.EntityROI = _roiRect;
                    _resizeStart = e.Location;
                    Invalidate();
                }

                else if (_isMovingRoi)
                {
                    int dx = e.X - _moveStart.X;
                    int dy = e.Y - _moveStart.Y;

                    int dxVirtual = (int)((float)dx / _curZoom + 0.5f);
                    int dyVirtual = (int)((float)dy / _curZoom + 0.5f);

                    if (_multiSelectedEntities.Count > 1)
                    {
                        foreach (var entity in _multiSelectedEntities)
                        {
                            if (entity == null || entity.IsHold)
                                continue;

                            Rectangle rect = entity.EntityROI;
                            rect.X += dxVirtual;
                            rect.Y += dyVirtual;
                            entity.EntityROI = rect;
                        }
                    }
                    else if (_selEntity != null && !_selEntity.IsHold)
                    {
                        _roiRect.X += dxVirtual;
                        _roiRect.Y += dyVirtual;
                        _selEntity.EntityROI = _roiRect;
                    }

                    _moveStart = e.Location;
                    Invalidate();
                }
                else if (_isBoxSelecting)
                {
                    int x = Math.Min(_roiStart.X, e.X);
                    int y = Math.Min(_roiStart.Y, e.Y);
                    int w = Math.Abs(e.X - _roiStart.X);
                    int h = Math.Abs(e.Y - _roiStart.Y);
                    _selectionBox = new Rectangle(x, y, w, h);
                    Invalidate();

                }
            }
    
            else
            {
                if (_selEntity != null && _newRoiType == InspWindowType.None)
                {
                    Rectangle screenRoi = VirtualToScreen(_roiRect);
                    Rectangle screenRect = VirtualToScreen(_selEntity.EntityROI);
                    int index = GetResizeHandleIndex(screenRect, e.Location);
                    if (index != -1)
                    {
                        Cursor = GetCursorForHandle(index);
                    }
                    else if (screenRoi.Contains(e.Location))
                    {
                        Cursor = Cursors.SizeAll; 
                    }
                    else
                    {
                        Cursor = Cursors.Arrow;
                    }
                }
            }
        }
        private void ImageViewCtrl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) //마우스 왼쪽 버튼이 떼어졌을 때
            {
                if (_isSelectingRoi) //새로운 ROI 선택이 완료되었을 때
                {
                    _isSelectingRoi = false;

                    if (_bitmapImage == null) return; //이미지가 없다면 리턴
                    if (_roiStart == e.Location) return; //클릭만 하고 드래그하지 않았다면 리턴

                    //ROI 크기가 10보다 작으면, 추가하지 않음
                    if (_roiRect.Width < 10 ||
                        _roiRect.Height < 10 ||
                        _roiRect.X < 0 ||
                        _roiRect.Y < 0 ||
                        _roiRect.Right > _bitmapImage.Width ||
                        _roiRect.Bottom > _bitmapImage.Height)
                        return;

                    _selEntity = new DiagramEntity(_roiRect, _selColor);

                    DiagramEntityEvent?.Invoke(this, new DiagramEntityEventArgs(EntityActionType.Add, null, _newRoiType, _roiRect, new Point()));


                }
                else if (_isResizingRoi)
                {
                    _selEntity.EntityROI = _roiRect;
                    _isResizingRoi = false;

                    //모델에 InspWindow 크기 변경 이벤트 발생
                    DiagramEntityEvent?.Invoke(this, new DiagramEntityEventArgs(EntityActionType.Resize, _selEntity.LinkedWindow, _newRoiType, _roiRect, new Point()));
                }
                else if (_isMovingRoi)
                {
                    _isMovingRoi = false;

                    if (_selEntity != null)
                    {
                        InspWindow linkedWindow = _selEntity.LinkedWindow;

                        Point offsetMove = new Point(0, 0);
                        if (linkedWindow != null)
                        {
                            offsetMove.X = _selEntity.EntityROI.X - linkedWindow.WindowArea.X;
                            offsetMove.Y = _selEntity.EntityROI.Y - linkedWindow.WindowArea.Y;
                        }

                        //모델에 InspWindow 이동 이벤트 발생
                        if (offsetMove.X != 0 || offsetMove.Y != 0)
                            DiagramEntityEvent?.Invoke(this, new DiagramEntityEventArgs(EntityActionType.Move, linkedWindow, _newRoiType, _roiRect, offsetMove));
                        else
                            //모델에 InspWindow 선택 변경 이벤트 발생
                            DiagramEntityEvent?.Invoke(this, new DiagramEntityEventArgs(EntityActionType.Select, _selEntity.LinkedWindow));

                    }
                }
                if (_isBoxSelecting)
                {
                    _isBoxSelecting = false;
                    _multiSelectedEntities.Clear();

                    Rectangle selectionVirtual = ScreenToVirtual(_selectionBox);

                    foreach (DiagramEntity entity in _diagramEntityList)
                    {
                        if (selectionVirtual.IntersectsWith(entity.EntityROI))
                        {
                            _multiSelectedEntities.Add(entity);
                        }
                    }

                    if (_multiSelectedEntities.Any())
                        _selEntity = _multiSelectedEntities[0];

                    _selectionBox = Rectangle.Empty;

                    //선택해제
                    DiagramEntityEvent?.Invoke(this, new DiagramEntityEventArgs(EntityActionType.Select, null));

                    Invalidate();

                    return;
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                if (_newRoiType != InspWindowType.None)
                {
                    //같은 타입의 ROI추가가 더이상 없다면 초기화하여, ROI가 추가되지 않도록 함
                    _newRoiType = InspWindowType.None;
                }
                else if (_selEntity != null)
                {
                    //팝업메뉴 표시
                    _contextMenu.Show(this, e.Location);
                }

                Cursor = Cursors.Arrow;
            }
        }

        private void AddSelectedROI(DiagramEntity entity)
        {
            if (entity == null) return;
            if (!_multiSelectedEntities.Contains(entity))
                _multiSelectedEntities.Add(entity);
        }

        #region ROI Handle
        //마우스 위치가 ROI 크기 변경을 위한 여부를 확인하기 위해, 4개 모서리와 사각형 라인의 중간 위치 반환
        private Point[] GetResizeHandles(Rectangle rect)    /*ROI 크기 변경 핸들 위치 반환 함수*/
        {
            return new Point[]
            {
                new Point(rect.Left, rect.Top), // 좌상
                new Point(rect.Right, rect.Top), // 우상
                new Point(rect.Left, rect.Bottom), // 좌하
                new Point(rect.Right, rect.Bottom), // 우하
                new Point(rect.Left + rect.Width / 2, rect.Top), // 상 중간
                new Point(rect.Left + rect.Width / 2, rect.Bottom), // 하 중간
                new Point(rect.Left, rect.Top + rect.Height / 2), // 좌 중간
                new Point(rect.Right, rect.Top + rect.Height / 2) // 우 중간
            };
        }

        //마우스 위치가 크기 변경 위치에 해당하는 지를, 위치 인덱스로 반환
        private int GetResizeHandleIndex(Rectangle screenRect, Point mousePos)
        {
            Point[] handles = GetResizeHandles(screenRect);
            for (int i = 0; i < handles.Length; i++)
            {
                Rectangle handleRect = new Rectangle(handles[i].X - _ResizeHandleSize / 2, handles[i].Y - _ResizeHandleSize / 2, _ResizeHandleSize, _ResizeHandleSize);
                if (handleRect.Contains(mousePos)) return i;
            }
            return -1;
        }

        //사각 모서리와 중간 지점을 인덱스로 설정하여, 해당 위치에 따른 커서 타입 반환
        private Cursor GetCursorForHandle(int handleIndex)
        {
            switch (handleIndex)
            {
                case 0: case 3: return Cursors.SizeNWSE;
                case 1: case 2: return Cursors.SizeNESW;
                case 4: case 5: return Cursors.SizeNS;
                case 6: case 7: return Cursors.SizeWE;
                default: return Cursors.Default;
            }
        }
        #endregion

        //ROI 크기 변경시, 마우스 위치를 입력받아, ROI 크기 변경
        private void ResizeROI(Point mousePos)
        {
            Rectangle roi = VirtualToScreen(_roiRect);
            switch (_resizeDirection)
            {
                case 0:
                    roi.X = mousePos.X;
                    roi.Y = mousePos.Y;
                    roi.Width -= (mousePos.X - _resizeStart.X);
                    roi.Height -= (mousePos.Y - _resizeStart.Y);
                    break;
                case 1:
                    roi.Width = mousePos.X - roi.X;
                    roi.Y = mousePos.Y;
                    roi.Height -= (mousePos.Y - _resizeStart.Y);
                    break;
                case 2:
                    roi.X = mousePos.X;
                    roi.Width -= (mousePos.X - _resizeStart.X);
                    roi.Height = mousePos.Y - roi.Y;
                    break;
                case 3:
                    roi.Width = mousePos.X - roi.X;
                    roi.Height = mousePos.Y - roi.Y;
                    break;
                case 4:
                    roi.Y = mousePos.Y;
                    roi.Height -= (mousePos.Y - _resizeStart.Y);
                    break;
                case 5:
                    roi.Height = mousePos.Y - roi.Y;
                    break;
                case 6:
                    roi.X = mousePos.X;
                    roi.Width -= (mousePos.X - _resizeStart.X);
                    break;
                case 7:
                    roi.Width = mousePos.X - roi.X;
                    break;
            }

            _roiRect = ScreenToVirtual(roi);
        }


        //#4_IMAGE_VIEWER#4 마우스휠을 이용한 확대/축소
        private void ImageViewCCtrl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
                ZoomMove(_curZoom / _zoomFactor, e.Location);
            else
                ZoomMove(_curZoom * _zoomFactor, e.Location);

            // 새로운 이미지 위치 반영 (점진적으로 초기 상태로 회귀)
            if (_bitmapImage != null)
            {
                ImageRect.Width = _bitmapImage.Width * _curZoom;
                ImageRect.Height = _bitmapImage.Height * _curZoom;
            }

            // 다시 그리기 요청
            Invalidate();
        }

        //휠에 의해, Zoom 확대/축소 값 계산
        private void ZoomMove(float zoom, Point zoomOrigin)
        {
            PointF virtualOrigin = ScreenToVirtual(new PointF(zoomOrigin.X, zoomOrigin.Y));

            _curZoom = Math.Max(MinZoom, Math.Min(MaxZoom, zoom));
            if (_curZoom <= MinZoom)
                return;

            PointF zoomedOrigin = VirtualToScreen(virtualOrigin);

            float dx = zoomedOrigin.X - zoomOrigin.X;
            float dy = zoomedOrigin.Y - zoomOrigin.Y;

            ImageRect.X -= dx;
            ImageRect.Y -= dy;
        }

        // Virtual <-> Screen 좌표계 변환
        #region 좌표계 변환
        private PointF GetScreenOffset()
        {
            return new PointF(ImageRect.X, ImageRect.Y);
        }

        private Rectangle ScreenToVirtual(Rectangle screenRect)
        {
            PointF offset = GetScreenOffset();
            return new Rectangle(
                (int)((screenRect.X - offset.X) / _curZoom + 0.5f),
                (int)((screenRect.Y - offset.Y) / _curZoom + 0.5f),
                (int)(screenRect.Width / _curZoom + 0.5f),
                (int)(screenRect.Height / _curZoom + 0.5f));
        }

        private Rectangle VirtualToScreen(Rectangle virtualRect)
        {
            PointF offset = GetScreenOffset();
            return new Rectangle(
                (int)(virtualRect.X * _curZoom + offset.X + 0.5f),
                (int)(virtualRect.Y * _curZoom + offset.Y + 0.5f),
                (int)(virtualRect.Width * _curZoom + 0.5f),
                (int)(virtualRect.Height * _curZoom + 0.5f));
        }

        private PointF ScreenToVirtual(PointF screenPos)
        {
            PointF offset = GetScreenOffset();
            return new PointF(
                (screenPos.X - offset.X) / _curZoom,
                (screenPos.Y - offset.Y) / _curZoom);
        }

        private PointF VirtualToScreen(PointF virtualPos)
        {
            PointF offset = GetScreenOffset();
            return new PointF(
                virtualPos.X * _curZoom + offset.X,
                virtualPos.Y * _curZoom + offset.Y);
        }
        #endregion

        private void ImageViewCtrl_Resize(object sender, EventArgs e)
        {
            ResizeCanvas();
            Invalidate();
        }

        private void ImageViewCtrl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            FitImageToScreen();
        }

        //#8_INSPECT_BINARY#17 화면에 보여줄 영역 정보를 표시하기 위해, 위치 입력 받는 함수
        public void AddRect(List<DrawInspectInfo> rectInfos)
        {
            //song
            if (rectInfos == null) rectInfos = new List<DrawInspectInfo>();

            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => AddRect(rectInfos)));
                return;
            }

            //song : 원하는 것만 남기기
            // 엣지 점: UsePoint == true
            // BaseROI: (예시) inspectType == InspectType.InspNone 이면서 rect가 유효한 것
            // BaseROI가 어떤 inspectType/decision으로 들어오는지에 따라 조건을 조정해야 함
            var filtered = rectInfos.Where(x =>
                x.UsePoint == true || (
                x.UsePoint == false &&
                x.inspectType == InspectType.InspBinary &&
                x.decision == DecisionType.Info &&
                string.IsNullOrEmpty(x.info) &&
                x.rect.Width > 0 && x.rect.Height > 0)
            ).ToList();

            lock (_lock)
            {
                //_rectInfos.Clear(); //song
                // song : filtered만 "교체"되게 (windowId + inspectType 단위)
                var groups = filtered.GroupBy(x => new { x.windowUid, x.inspectType });

                foreach (var g in groups)
                {
                    _rectInfos.RemoveAll(old =>
                        old.windowUid == g.Key.windowUid &&
                        old.inspectType == g.Key.inspectType);

                    _rectInfos.AddRange(g);
                }
            }

            Invalidate();   //오버레이 다시 그리기 요청
        }
        public void RemoveResultsByWindowUid(string uid)
        {
            if (string.IsNullOrEmpty(uid)) return;

            lock (_lock)
            {
                _rectInfos.RemoveAll(x => x.windowUid == uid);
            }
            Invalidate();
        }

        //DeleteList
        public void RemoveResultsByWindowUids(IEnumerable<string> uids)
        {
            if (uids == null) return;
            var set = new HashSet<string>(uids.Where(x => !string.IsNullOrEmpty(x)));

            lock (_lock)
            {
                _rectInfos.RemoveAll(x => set.Contains(x.windowUid));
            }
            Invalidate();
        }
        public void SetInspResultCount(InspectResultCount inspectResultCount)
        {
            _inspectResultCount = inspectResultCount;
        }

        //#13_INSP_RESULT#9 키보드 이벤트 받기 
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            _isCtrlPressed = keyData == Keys.Control;

            if (keyData == (Keys.Control | Keys.C))
            {
                CopySelectedROIs();
            }
            else if (keyData == (Keys.Control | Keys.V))
            {
                PasteROIsAt();
            }
            else
            {
                switch (keyData)
                {
                    case Keys.Delete:
                        {
                            if (_selEntity != null)
                            {
                                DeleteSelEntity();
                            }
                        }
                        break;
                    case Keys.Enter:
                        {
                            InspWindow selWindow = null;
                            if (_selEntity != null)
                                selWindow = _selEntity.LinkedWindow;

                            DiagramEntityEvent?.Invoke(this, new DiagramEntityEventArgs(EntityActionType.Inspect, selWindow));
                        }
                        break;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        // ─── 복사(Ctrl+C) ----------------------------------------------------------
        private void CopySelectedROIs() // #ROI COPYPASTE#
        {
            _copyBuffer.Clear();
            for (int i = 0; i < _multiSelectedEntities.Count; i++)
            {
                _copyBuffer.Add(_multiSelectedEntities[i]);
            }
        }

        // ─── 붙여넣기(Ctrl+V) ------------------------------------------------------
        private void PasteROIsAt() // #ROI COPYPASTE#
        {
            if (_copyBuffer.Count == 0)
                return;

            // ① 기준점(마우스)을 Virtual 좌표로 변환
            PointF virtBase = ScreenToVirtual(_mousePos);

            foreach (var entity in _copyBuffer)
            {
                int dx = (int)(virtBase.X - entity.EntityROI.Left + 0.5f);
                int dy = (int)(virtBase.Y - entity.EntityROI.Top + 0.5f);
                var newRect = entity.EntityROI;

                DiagramEntityEvent?.Invoke(this,
                    new DiagramEntityEventArgs(EntityActionType.Copy, entity.LinkedWindow,
                                                entity.LinkedWindow?.InspWindowType ?? InspWindowType.None,
                                                newRect, new Point(dx, dy)));
            }
            Invalidate();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Control)
                _isCtrlPressed = false;

            base.OnKeyUp(e);
        }

        public void ResetEntity(bool clearResults = true)
        {
            lock (_lock)
            {
                _diagramEntityList.Clear(); //// ROI(도형) 제거

                if (clearResults)
                    _rectInfos.Clear(); // 검사 결과(엣지 점, Rect 등) 제거 여부
                //_rectInfos.Clear();

                _selEntity = null;
                _multiSelectedEntities?.Clear();  // ROI를 리셋할 때 “선택 상태 메모리”가 남아서 UI가 꼬이는 걸 막기 위한 안전장치
            }
            Invalidate();
        }

        public bool SetDiagramEntityList(List<DiagramEntity> diagramEntityList)
        {
            _diagramEntityList = diagramEntityList
                                .OrderBy(r => r.EntityROI.Width * r.EntityROI.Height)
                                .ToList();

            _selEntity = null;
            Invalidate();
            return true;
        }

        public void SelectDiagramEntity(InspWindow window)
        {
            DiagramEntity entity = _diagramEntityList.Find(e => e.LinkedWindow == window);
            if (entity != null)
            {
                _multiSelectedEntities.Clear();
                AddSelectedROI(entity);

                _selEntity = entity;
                _roiRect = entity.EntityROI;
            }
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            DeleteSelEntity();

        }

        private void OnTeachingClicked(object sender, EventArgs e)
        {
            if (_selEntity == null) return;

            InspWindow window = _selEntity.LinkedWindow;

            if (window == null) return;

            window.IsTeach = true;
            _selEntity.IsHold = true;
        }


        private void OnUnlockClicked(object sender, EventArgs e)
        {
            if (_selEntity == null) return;

            InspWindow window = _selEntity.LinkedWindow;

            if (window == null) return;

            _selEntity.IsHold = false;
        }

        private void OnDrawHeightLineClicked(object sender, EventArgs e)
        {
            if (_multiSelectedEntities.Count == 3)
            {
                // 3개를 하나의 배열로 묶어서 리스트에 추가 (누적됨)
                _heightLineList.Add(_multiSelectedEntities.ToArray());
                _drawVerticalEnabled = true;
                Invalidate(); // 화면 갱신
            }
        }


        private void DeleteSelEntity()
        {
            List<InspWindow> selected = _multiSelectedEntities
                .Where(d => d.LinkedWindow != null)
                .Select(d => d.LinkedWindow)
                .ToList();

            if (selected.Count > 0)
            {
                // 1. 다중 선택된 항목 중 선 그리기와 관련된 ROI가 있는지 확인하고 초기화
                foreach (var entity in _multiSelectedEntities)
                {
                    CheckAndResetLineRois(entity);
                }

                DiagramEntityEvent?.Invoke(this, new DiagramEntityEventArgs(EntityActionType.DeleteList, selected));
            }
            else if (_selEntity != null)
            {
                InspWindow linkedWindow = _selEntity.LinkedWindow;
                if (linkedWindow == null) return;

                // 2. 단일 선택된 항목이 선 그리기와 관련된 ROI인지 확인하고 초기화
                CheckAndResetLineRois(_selEntity);

                DiagramEntityEvent?.Invoke(this, new DiagramEntityEventArgs(EntityActionType.Delete, linkedWindow));
            }

            // 삭제 후 화면 갱신
            Invalidate();
        }

        // 선 관련 변수를 초기화하는 헬퍼 메서드
        private void CheckAndResetLineRois(DiagramEntity target)
        {
            // 리스트를 뒤에서부터 검사하여 삭제된 ROI가 포함된 라인만 제거
            for (int i = _heightLineList.Count - 1; i >= 0; i--)
            {
                if (_heightLineList[i].Contains(target))
                {
                    _heightLineList.RemoveAt(i);
                }
            }

            if (_heightLineList.Count == 0) _drawVerticalEnabled = false;
            Invalidate();
        }
        public List<string> GetSelectedUids()
        {
            // _multiSelectedEntities에 담긴 모든 엔티티의 UID를 리스트로 추출
            return _multiSelectedEntities
                   .Where(e => e.LinkedWindow != null)
                   .Select(e => e.LinkedWindow.UID)
                   .ToList();
        }
        public List<string[]> GetHeightLineUids()
        {
            // _heightLineList에 담긴 각 DiagramEntity 배열에서 UID만 뽑아냅니다.
            return _heightLineList.Select(nodes => new string[] {
        nodes[0].LinkedWindow.UID,
        nodes[1].LinkedWindow.UID,
        nodes[2].LinkedWindow.UID
    }).ToList();
        }
        public void RestoreHeightLinesFromModel(Model model)
        {
            // 1. 저장된 데이터가 없으면 리스트를 비우고 종료
            if (model == null || model.SavedHeightLineUids == null)
            {
                _heightLineList.Clear();
                _drawVerticalEnabled = false;
                return;
            }

            _heightLineList.Clear();

            // 2. 저장된 UID 세트를 하나씩 확인
            foreach (var uids in model.SavedHeightLineUids)
            {
                // 현재 ImageViewer가 관리하는 _diagramEntityList에서 같은 UID를 가진 객체를 찾음
                var ent1 = _diagramEntityList.FirstOrDefault(e => e.LinkedWindow.UID == uids[0]);
                var ent2 = _diagramEntityList.FirstOrDefault(e => e.LinkedWindow.UID == uids[1]);
                var baseEnt = _diagramEntityList.FirstOrDefault(e => e.LinkedWindow.UID == uids[2]);

                // 세 개의 ROI가 모두 찾아졌다면 선 리스트에 추가
                if (ent1 != null && ent2 != null && baseEnt != null)
                {
                    _heightLineList.Add(new DiagramEntity[] { ent1, ent2, baseEnt });
                }
            }

            // 3. 선이 존재하면 그리기 활성화 및 화면 갱신
            _drawVerticalEnabled = (_heightLineList.Count > 0);
            this.Invalidate();
        }


    }
    #region EventArgs
    public class DiagramEntityEventArgs : EventArgs
    {
        public EntityActionType ActionType { get; private set; }
        public InspWindow InspWindow { get; private set; }
        public InspWindowType WindowType { get; private set; }
        public List<InspWindow> InspWindowList { get; private set; }
        public OpenCvSharp.Rect Rect { get; private set; }
        public OpenCvSharp.Point OffsetMove { get; private set; }
        public DiagramEntityEventArgs(EntityActionType actionType, InspWindow inspWindow)
        {
            ActionType = actionType;
            InspWindow = inspWindow;
        }

        public DiagramEntityEventArgs(EntityActionType actionType, InspWindow inspWindow, InspWindowType windowType, Rectangle rect, Point offsetMove)
        {
            ActionType = actionType;
            InspWindow = inspWindow;
            WindowType = windowType;
            Rect = new OpenCvSharp.Rect(rect.X, rect.Y, rect.Width, rect.Height);
            OffsetMove = new OpenCvSharp.Point(offsetMove.X, offsetMove.Y);
        }

        public DiagramEntityEventArgs(EntityActionType actionType, List<InspWindow> inspWindowList, InspWindowType windowType = InspWindowType.None)
        {
            ActionType = actionType;
            InspWindow = null;
            InspWindowList = inspWindowList;
            WindowType = windowType;
        }
    }

    #endregion
}
