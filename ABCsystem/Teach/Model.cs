using Common.Util.Helpers;
using ABCsystem.Core;
using ABCsystem.Teach;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ABCsystem.Teach
{
    public class Model
    {
        public string ModelName { get; set; } = "";
        public string ModelInfo { get; set; } = "";
        public string ModelPath { get; set; } = "";

        public string InspectImagePath { get; set; } = "";

        [XmlElement("InspWindow")]
        public List<InspWindow> InspWindowList { get; set; }


        public List<string[]> SavedHeightLineUids { get; set; } = new List<string[]>();

        public Model()
        {
            InspWindowList = new List<InspWindow>();
        }

        public InspWindow AddInspWindow(InspWindowType windowType)
        {
            InspWindow inspWindow = InspWindowFactory.Inst.Create(windowType);
            InspWindowList.Add(inspWindow);

            return inspWindow;
        }

        public bool AddInspWindow(InspWindow inspWindow)
        {
            if (inspWindow == null)
                return false;

            InspWindowList.Add(inspWindow);

            return true;
        }

        public bool DelInspWindow(InspWindow inspWindow)
        {
            if (InspWindowList.Contains(inspWindow))
            {
                InspWindowList.Remove(inspWindow);
                return true;
            }
            return false;
        }

        public bool DelInspWindowList(List<InspWindow> inspWindowList)
        {
            int before = InspWindowList.Count;
            InspWindowList.RemoveAll(w => inspWindowList.Contains(w));
            return InspWindowList.Count < before;
        }

        public void CreateModel(string path, string modelName, string modelInfo)
        {
            ModelPath = path;
            ModelName = modelName;
            ModelInfo = modelInfo;
        }
        public Model Load(string path)
        {
            Model model = XmlHelper.LoadXml<Model>(path);
            if (model == null)
                return null;

            ModelPath = path;

            foreach (var window in model.InspWindowList)
            {
                window.LoadInspWindow(model);
            }

            return model;
        }

        //모델 저장함수
        public void Save()
        {
            if (ModelPath == "") return;

            // 1. 현재 열려있는 CameraForm의 ImageViewer를 찾습니다.
            var cameraForm = MainForm.DockPanelInstance.Contents
                                     .OfType<CameraForm>()
                                     .FirstOrDefault();

            if (cameraForm != null)
            {
                // 2. [핵심] 화면의 선 정보를 모델의 리스트 변수에 옮겨 담습니다.
                // 이제 ImageViewCtrl에 메서드를 만들었으므로 에러가 사라집니다.
                this.SavedHeightLineUids = cameraForm.ImageViewer.GetHeightLineUids();
            }

            // 3. XML 파일 저장 (SavedHeightLineUids 리스트가 함께 저장됨)
            XmlHelper.SaveXml(ModelPath, this);

            // 4. 기존 ROI 개별 저장 로직
            foreach (var window in InspWindowList)
            {
                window.SaveInspWindow(this);
            }
        }

        //모델 다른 이름으로 저장함수
        public void SaveAs(string filePath)
        {
            string fileName = Path.GetFileName(filePath);
            if (Directory.Exists(filePath) == false)
            {
                ModelPath = Path.Combine(filePath, fileName + ".xml");
                ModelName = fileName;
                Save();
            }
        }
    }
}
