using ABCsystem.Core;
using ABCsystem.Teach;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace ABCsystem
{
    public partial class ModelTreeForm: DockContent
    {
        //개별 트리 노트에서 팝업 메뉴 보이기를 위한 메뉴
        private ContextMenuStrip _contextMenu;

        public ModelTreeForm()
        {
            InitializeComponent();

            //초기 트리 노트의 기본값은 "Root"
            tvModelTree.Nodes.Add("Root");

            // 컨텍스트 메뉴 초기화
            _contextMenu = new ContextMenuStrip();

            List<InspWindowType> windowTypeList;
            windowTypeList = new List<InspWindowType> { InspWindowType.Base, InspWindowType.Body, InspWindowType.Sub, InspWindowType.ID};
            
            foreach (InspWindowType windowType in windowTypeList)
                _contextMenu.Items.Add(new ToolStripMenuItem(windowType.ToString(), null, AddNode_Click) { Tag = windowType });

        }

        private void tvModelTree_MouseDown(object sender, MouseEventArgs e)
        {
            //Root 노드에서 마우스 오른쪽 버튼 클릭 시에, 팝업 메뉴 생성
            if (e.Button == MouseButtons.Right)
            {
                TreeNode clickedNode = tvModelTree.GetNodeAt(e.X, e.Y);
                if (clickedNode != null && clickedNode.Text == "Root")
                {
                    tvModelTree.SelectedNode = clickedNode;
                    _contextMenu.Show(tvModelTree, e.Location);
                }
            }
        }

        //팝업 메뉴에서, 메뉴 선택시 실행되는 함수
        private void AddNode_Click(object sender, EventArgs e)
        {
            if (tvModelTree.SelectedNode != null & sender is ToolStripMenuItem)
            {
                ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
                InspWindowType windowType = (InspWindowType)menuItem.Tag;
                AddNewROI(windowType);
            }
        }

        //imageViewer에 ROI 추가 기능 실행
        private void AddNewROI(InspWindowType inspWindowType)
        {
            CameraForm cameraForm = MainForm.GetDockForm<CameraForm>();
            if (cameraForm != null)
            {
                cameraForm.AddRoi(inspWindowType);
            }
        }

        //현재 모델 전체의 ROI를 트리 모델에 업데이트
        public void UpdateDiagramEntity()
        {
            tvModelTree.Nodes.Clear();
            TreeNode rootNode = tvModelTree.Nodes.Add("Root");

            Model model = Global.Inst.InspStage.CurModel;
            List<InspWindow> windowList = model.InspWindowList;
            if (windowList.Count <= 0)
                return;

            foreach (InspWindow window in model.InspWindowList)
            {
                if (window is null)
                    continue;

                string uid = window.UID;

                TreeNode node = new TreeNode(uid);
                rootNode.Nodes.Add(node);
            }

            tvModelTree.ExpandAll();
        }

        public void SelectNodeByUid(string uid) //트리뷰에서 UID로 노드 선택
        {
            if (string.IsNullOrEmpty(uid)) return;

            // 1. 트리뷰의 모든 노드 중에서 검색 (재귀 방식)
            TreeNode[] nodes = tvModelTree.Nodes.Find(uid, true); // UpdateDiagramEntity에서 노드 추가 시 name을 uid로 안했다면 아래 루프 사용

            // 만약 Find로 안 찾아진다면 (Name을 지정 안 했을 경우)
            if (nodes.Length == 0)
            {
                foreach (TreeNode root in tvModelTree.Nodes)
                {
                    foreach (TreeNode child in root.Nodes)
                    {
                        if (child.Text == uid)
                        {
                            tvModelTree.SelectedNode = child;
                            child.EnsureVisible();
                            tvModelTree.Focus(); // 이게 있어야 트리가 선택된 게 보임
                            return;
                        }
                    }
                }
            }
            else
            {
                tvModelTree.SelectedNode = nodes[0];
                nodes[0].EnsureVisible();
                tvModelTree.Focus();
            }
        }

    }
}
