using Common.Util.Helpers;
using ABCsystem.Grab;
using ABCsystem.Sequence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABCsystem.Setting
{
    public class SettingXml
    {
        //환경설정 파일 저장 경로
        private const string SETTING_DIR = "Setup";
        private const string SETTING_FILE_NAME = @"Setup\Setting.xml";

        #region Singleton Instance
        private static SettingXml _setting;

        public static SettingXml Inst
        {
            get
            {
                if (_setting == null)
                    Load();

                return _setting;
            }
        }
        #endregion

        //환경설정 로딩
        public static void Load()
        {
            if (_setting != null)
                return;

            //환경설정 경로 생성
            string settingFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, SETTING_FILE_NAME);
            if (File.Exists(settingFilePath) == true)
            {
                //환경설정 파일이 있다면 XmlHelper를 이용해 로딩
                _setting = XmlHelper.LoadXml<SettingXml>(settingFilePath);
            }

            if (_setting == null)
            {
                //환경설정 파일이 없다면 새로 생성
                _setting = CreateDefaultInstance();
            }
        }

        //환경설정 저장
        public static void Save()
        {
            string settingFilePath = Path.Combine(Environment.CurrentDirectory, SETTING_FILE_NAME);
            if (!File.Exists(settingFilePath))
            {
                //Setup 폴더가 없다면 생성
                string setupDir = Path.Combine(Environment.CurrentDirectory, SETTING_DIR);

                if (!Directory.Exists(setupDir))
                    Directory.CreateDirectory(setupDir);

                //Setting.xml 파일이 없다면 생성
                FileStream fs = File.Create(settingFilePath);
                fs.Close();
            }

            //XmlHelper를 이용해 Xml로 환경설정 정보 저장
            XmlHelper.SaveXml(settingFilePath, Inst);
        }

        //최초 환경설정 파일 생성
        private static SettingXml CreateDefaultInstance()
        {
            SettingXml setting = new SettingXml();
            setting.ModelDir = @"d:\model";
            return setting;
        }

        public SettingXml() { }

        public string MachineName { get; set; } = "VISION03";

        public string ModelDir { get; set; } = "";
        public string ImageDir { get; set; } = "";

        public CameraType CamType { get; set; } = CameraType.WebCam;

        public long ExposureTime { get; set; } = 15000; //단위: us

        public bool CycleMode { get; set; } = false;

        public CommunicatorType CommType { get; set; }

        public string CommIP { get; set; } = "127.0.0.1";

        // ##1_Setting_##2 “기존에 입력되있는 Setting” 지워버리기(Setting.xml 초기화)
        public static void ResetToBlank(bool deleteFile = true)
        {
            // 메모리 인스턴스 초기화
            _setting = new SettingXml();

            // “지워진 상태”로 만들기 (원하면 여기 기본값을 네 취향대로 바꿔도 됨)
            _setting.MachineName = "";
            _setting.ModelDir = "";
            _setting.ImageDir = "";
            _setting.CamType = ABCsystem.Grab.CameraType.None;
            _setting.ExposureTime = 15000;          // 기본 노출값은 유지(원하면 0도 가능)
            _setting.CycleMode = false;
            _setting.CommType = ABCsystem.Sequence.CommunicatorType.None;
            _setting.CommIP = "127.0.0.1";

            // 파일 자체도 삭제하고 싶으면 삭제
            if (deleteFile)
            {
                string settingFilePath = Path.Combine(Environment.CurrentDirectory, SETTING_FILE_NAME);
                if (File.Exists(settingFilePath))
                    File.Delete(settingFilePath);
            }

            // 다시 저장(= 완전히 초기화된 파일로 재생성)
            Save();
        }
    }

}
