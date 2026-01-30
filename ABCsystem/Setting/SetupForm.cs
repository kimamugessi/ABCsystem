using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ABCsystem.Setting;

namespace ABCsystem4.Setting
{
    public enum SettingType
    {
        SettingPath = 0,
        SettingCamera
    }

    public partial class SetupForm: Form
    {
        private CameraSetting _cameraSetting;
        private PathSetting _pathSetting;
        private CommunicatorSetting _commSetting;
        public SetupForm(bool resetOnOpen = false)
        {
            InitializeComponent();

            if (resetOnOpen)
            {
                // 기존 Setting 초기화 (SettingXml에 ResetToBlank 구현되어 있어야 함)
                SettingXml.ResetToBlank(deleteFile: true);
            }

            InitTabControl();
        }
        private void InitTabControl()
        {
            tabSetting.TabPages.Clear();
            //카메라 설정 페이지 추가
            _cameraSetting = new CameraSetting();
            AddTabControl(_cameraSetting, "Camera");
            //경로 설정 페이지 추가
            _pathSetting = new PathSetting();
            AddTabControl(_pathSetting, "Path");

            _commSetting = new CommunicatorSetting();
            AddTabControl(_commSetting, "Communicator");
            //기본값으로 카메라 설정 페이지 보이도록 설정
            tabSetting.SelectTab(0);
        }
        

        //탭 추가 함수
        private void AddTabControl(UserControl control, string tabName)
        {
            // 새 탭 추가
            TabPage newTab = new TabPage(tabName)
            {
                Dock = DockStyle.Fill
            };
            control.Dock = DockStyle.Fill;
            newTab.Controls.Add(control);
            tabSetting.TabPages.Add(newTab);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
