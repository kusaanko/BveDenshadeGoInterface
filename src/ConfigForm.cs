using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SlimDX.DirectInput;

namespace Kusaanko.Bvets.DenshadeGoInterface
{
    public partial class ConfigForm : Form
    {
        private string[] reverserSwitchKeyTexts = { "リバーサー前進", "リバーサー中", "リバーサー後進" };
        private string[] cabSwitchKeyTexts = { "電笛", "空笛", "低速運転", "乗降促進" };
        private string[] gameControlKeyTexts = { "視点を上に移動", "視点を下に移動", "視点を左に移動", "視点を右に移動", 
            "視点をデフォルトに戻す", "視界をズームイン", "視界をズームアウト", "視点を切り替える", "時刻表示切り替え", "シナリオ再読み込み",
            "列車速度の変更", "早送り", "一時停止"};
        private string[] atsKeyTexts = new string[16];
        private List<int[]> keyCodeTable = new List<int[]>();
        private List<int> buttonSize = new List<int>();

        public ConfigForm()
        {
            InitializeComponent();

            for (int i = 0; i < atsKeyTexts.Length; i++)
            {
                atsKeyTexts[i] = "ATS " + i.ToString();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            for (int i = 0; i < reverserSwitchKeyTexts.Length; i++)
            {
                addKey(reverserSwitchKeyTexts[i]);
                keyCodeTable.Add(new int[2] { 0, i });
            }
            for (int i = 0; i < cabSwitchKeyTexts.Length; i++)
            {
                addKey(cabSwitchKeyTexts[i]);
                keyCodeTable.Add(new int[2] { -1, i });
            }
            for (int i = 0; i < atsKeyTexts.Length; i++)
            {
                addKey(atsKeyTexts[i]);
                keyCodeTable.Add(new int[2] { -2, i });
            }
            for (int i = 0; i < gameControlKeyTexts.Length; i++)
            {
                addKey(gameControlKeyTexts[i]);
                keyCodeTable.Add(new int[2] { -3, i });
            }

            selectDropDownList(buttonSKeyList, DenshadeGoInterface.settings.ButtonS);
            selectDropDownList(buttonPKeyList, DenshadeGoInterface.settings.ButtonP);
            selectDropDownList(buttonAKeyList, DenshadeGoInterface.settings.ButtonA);
            selectDropDownList(buttonBKeyList, DenshadeGoInterface.settings.ButtonB);
            selectDropDownList(buttonCKeyList, DenshadeGoInterface.settings.ButtonC);

            bool added = false;
            List<SlimDX.DirectInput.Joystick> sticks = new List<SlimDX.DirectInput.Joystick>();
            foreach (DeviceInstance device in DenshadeGoInterface.input.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                try
                {
                    controllerList.Items.Add(device.ProductName);
                    Joystick stick = new SlimDX.DirectInput.Joystick(DenshadeGoInterface.input, device.InstanceGuid);
                    stick.Acquire();
                    int size = stick.GetCurrentState().GetButtons().Length;
                    buttonSize.Add(size);
                    if (device.ProductName.Equals(DenshadeGoInterface.settings.ControllerName))
                    {
                        added = true;
                        controllerList.SelectedIndex = controllerList.Items.Count - 1;
                        resizeUpDown(size);
                    }
                    stick.Unacquire();
                }
                catch (DirectInputException)
                {
                }
            }
            if (!added)
            {
                buttonSize.Add(128);
                controllerList.Items.Add(DenshadeGoInterface.settings.ControllerName);
                controllerList.SelectedIndex = controllerList.Items.Count - 1;
                resizeUpDown(128);

            }
            loadUpDown();
        }

        private void resizeUpDown(int size)
        {
            triangleUpDown.Maximum = size;
            circleUpDown.Maximum = size;
            crossUpDown.Maximum = size;
            boxUpDown.Maximum = size;
            L2UpDown.Maximum = size;
            R2UpDown.Maximum = size;
            L1UpDown.Maximum = size;
            R1UpDown.Maximum = size;
            startUpDown.Maximum = size;
            selectUpDown.Maximum = size;
        }

        public void loadUpDown()
        {
            triangleUpDown.Value = DenshadeGoInterface.settings.Button0;
            circleUpDown.Value = DenshadeGoInterface.settings.Button1;
            crossUpDown.Value = DenshadeGoInterface.settings.Button2;
            boxUpDown.Value = DenshadeGoInterface.settings.Button3;
            L2UpDown.Value = DenshadeGoInterface.settings.Button4;
            R2UpDown.Value = DenshadeGoInterface.settings.Button5;
            L1UpDown.Value = DenshadeGoInterface.settings.Button6;
            R1UpDown.Value = DenshadeGoInterface.settings.Button7;
            startUpDown.Value = DenshadeGoInterface.settings.Button8;
            selectUpDown.Value = DenshadeGoInterface.settings.Button9;
        }

        private void saveUpDown()
        {
            DenshadeGoInterface.settings.Button0 = decimal.ToInt32(triangleUpDown.Value);
            DenshadeGoInterface.settings.Button1 = decimal.ToInt32(circleUpDown.Value);
            DenshadeGoInterface.settings.Button2 = decimal.ToInt32(crossUpDown.Value);
            DenshadeGoInterface.settings.Button3 = decimal.ToInt32(boxUpDown.Value);
            DenshadeGoInterface.settings.Button4 = decimal.ToInt32(L2UpDown.Value);
            DenshadeGoInterface.settings.Button5 = decimal.ToInt32(R2UpDown.Value);
            DenshadeGoInterface.settings.Button6 = decimal.ToInt32(L1UpDown.Value);
            DenshadeGoInterface.settings.Button7 = decimal.ToInt32(R1UpDown.Value);
            DenshadeGoInterface.settings.Button8 = decimal.ToInt32(startUpDown.Value);
            DenshadeGoInterface.settings.Button9 = decimal.ToInt32(selectUpDown.Value);
        }

        private void addKey(string s)
        {
            buttonSKeyList.Items.Add(s);
            buttonPKeyList.Items.Add(s);
            buttonAKeyList.Items.Add(s);
            buttonBKeyList.Items.Add(s);
            buttonCKeyList.Items.Add(s);
        }

        private void selectDropDownList(ComboBox list, int[] assign)
        {
            switch (assign[0])
            {
                case 0:
                    if (assign[1] >= 0 && assign[1] < reverserSwitchKeyTexts.Length)
                    {
                        list.SelectedIndex = assign[1];
                    }
                    break;
                case -1:
                    if (assign[1] >= 0 && assign[1] < cabSwitchKeyTexts.Length)
                    {
                        list.SelectedIndex = assign[1] + reverserSwitchKeyTexts.Length;
                    }
                    break;
                case -2:
                    if (assign[1] >= 0 && assign[1] < atsKeyTexts.Length)
                    {
                        list.SelectedIndex = assign[1] + reverserSwitchKeyTexts.Length + cabSwitchKeyTexts.Length;
                    }
                    break;
                case -3:
                    if (assign[1] >= 0 && assign[1] < gameControlKeyTexts.Length)
                    {
                        list.SelectedIndex = assign[1] + reverserSwitchKeyTexts.Length + cabSwitchKeyTexts.Length + atsKeyTexts.Length;
                    }
                    break;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (buttonSKeyList.SelectedIndex >= 0)
            {
                DenshadeGoInterface.settings.ButtonS = keyCodeTable[buttonSKeyList.SelectedIndex];
            }
            if (buttonPKeyList.SelectedIndex >= 0)
            {
                DenshadeGoInterface.settings.ButtonP = keyCodeTable[buttonPKeyList.SelectedIndex];
            }
            if (buttonAKeyList.SelectedIndex >= 0)
            {
                DenshadeGoInterface.settings.ButtonA = keyCodeTable[buttonAKeyList.SelectedIndex];
            }
            if (buttonBKeyList.SelectedIndex >= 0)
            {
                DenshadeGoInterface.settings.ButtonB = keyCodeTable[buttonBKeyList.SelectedIndex];
            }
            if (buttonCKeyList.SelectedIndex >= 0)
            {
                DenshadeGoInterface.settings.ButtonC = keyCodeTable[buttonCKeyList.SelectedIndex];
            }
            saveUpDown();
            DenshadeGoInterface.settings.ControllerName = controllerList.Text;
            DenshadeGoInterface.GetStick();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void controllerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            resizeUpDown(buttonSize[controllerList.SelectedIndex]);
        }

        private void resetMappingButton_Click(object sender, EventArgs e)
        {
            DenshadeGoInterface.settings.Button0 = 0;
            DenshadeGoInterface.settings.Button1 = 1;
            DenshadeGoInterface.settings.Button2 = 2;
            DenshadeGoInterface.settings.Button3 = 3;
            DenshadeGoInterface.settings.Button4 = 4;
            DenshadeGoInterface.settings.Button5 = 5;
            DenshadeGoInterface.settings.Button6 = 6;
            DenshadeGoInterface.settings.Button7 = 7;
            DenshadeGoInterface.settings.Button8 = 8;
            DenshadeGoInterface.settings.Button9 = 9;
            if (!controllerList.Items.Contains("JC-PS101U"))
            {
                controllerList.Items.Add("JC-PS101U");
                controllerList.SelectedIndex = controllerList.Items.Count - 1;
            }
            loadUpDown();
        }

        private void setupButton_Click(object sender, EventArgs e)
        {
            using (ControllerSetupForm form = new ControllerSetupForm(controllerList.Text))
            {
                form.ShowDialog(this);
            }
        }
    }
}
