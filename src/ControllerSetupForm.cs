using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SlimDX.DirectInput;

namespace Mackoy.Bvets.DenshadeGoInterface
{
    public partial class ControllerSetupForm : Form
    {
        private Joystick stick;
        private int step;
        private int wait;
        private List<int> button4;
        private List<int> button5;
        private List<int> button6;
        private List<int> button7;
        private int notchButton = -1;
        private int selectButton = -1;
        private int startButton = -1;
        private int aButton = -1;
        private int bButton = -1;
        private int cButton = -1;
        public ControllerSetupForm(string controller)
        {
            InitializeComponent();
            List<SlimDX.DirectInput.Joystick> sticks = new List<SlimDX.DirectInput.Joystick>();
            foreach (DeviceInstance device in DenshadeGoInterface.input.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                try
                {
                    if (device.ProductName.Equals(controller))
                    {
                        stick = new SlimDX.DirectInput.Joystick(DenshadeGoInterface.input, device.InstanceGuid);
                        break;
                    }
                }
                catch (DirectInputException)
                {
                }
            }
            if (stick == null)
            {
                return;
            }
            stick.Acquire();
            foreach (DeviceObjectInstance deviceObject in stick.GetObjects())
            {
                if ((deviceObject.ObjectType & ObjectDeviceType.Axis) != 0)
                    stick.GetObjectPropertiesById((int)deviceObject.ObjectType).SetRange(-100, 100);
            }
            stick.Unacquire();
            timer1.Start();
            countLabel.Text = "";
            button4 = new List<int>();
            button5 = new List<int>();
            button6 = new List<int>();
            button7 = new List<int>();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool[] buttons = GetButtons();
            if (stick.Acquire().IsFailure) return;
            if (step == 0)
            {
                bool isComplete = true;
                if (stick.GetCurrentState().X > -90) isComplete = false;
                for (int i = 0;i < buttons.Length;i++)
                {
                    if (buttons[i])
                    {
                        isComplete = false;
                        break;
                    }
                }
                if (isComplete)
                {
                    step++;
                    SetText(0, 8);
                }
            }
            else if (step == 1)
            {
                int count = 0;
                List<int> onList = new List<int>();
                for (int i = 0;i < buttons.Length;i++)
                {
                    if(buttons[i])
                    {
                        count++;
                        onList.Add(i);
                    }
                }
                if (count == 2)
                {
                    if(wait % 10 == 0)
                    {
                        countLabel.Text = (2 - wait / 10) + "";
                    }
                    if (wait == 20)
                    {
                        step++;
                        SetText(0, 7);
                        button4.AddRange(onList);
                        button7.AddRange(onList);
                    }
                    wait++;
                }
                else
                {
                    if (wait != 0)
                    {
                        countLabel.Text = "";
                    }
                    wait = 0;
                }
            }
            else if (step == 2)
            {
                int count = 0;
                List<int> onList = new List<int>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (buttons[i])
                    {
                        count++;
                        onList.Add(i);
                    }
                }
                if (count == 3)
                {
                    if (wait % 10 == 0)
                    {
                        countLabel.Text = (2 - wait / 10) + "";
                    }
                    if (wait == 20)
                    {
                        step++;
                        SetText(0, 6);
                        foreach (int i in onList)
                        {
                            if (!button4.Contains(i))
                            {
                                button6.Add(i);
                                break;
                            }
                        }
                    }
                    wait++;
                }
                else
                {
                    if (wait != 0)
                    {
                        countLabel.Text = "";
                    }
                    wait = 0;
                }
            }
            else if (step == 3)
            {
                int count = 0;
                List<int> onList = new List<int>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (buttons[i])
                    {
                        count++;
                        onList.Add(i);
                    }
                }
                if (count == 1)
                {
                    if (wait % 10 == 0)
                    {
                        countLabel.Text = (2 - wait / 10) + "";
                    }
                    if (wait == 20)
                    {
                        step++;
                        SetText(0, 4);
                        foreach (int i in onList)
                        {
                            if (!button4.Contains(i) && !button6.Contains(i))
                            {
                                button5.Add(i);
                                break;
                            }
                        }
                    }
                    wait++;
                }
                else
                {
                    if (wait != 0)
                    {
                        countLabel.Text = "";
                    }
                    wait = 0;
                }
            }
            else if (step == 4)
            {
                int count = 0;
                List<int> onList = new List<int>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (buttons[i])
                    {
                        count++;
                        onList.Add(i);
                    }
                }
                bool check = false;
                foreach (int i in onList)
                {
                    if (button4.Contains(i))
                    {
                        check = true;
                        break;
                    }
                }
                if (count == 2 && check)
                {
                    if (wait % 10 == 0)
                    {
                        countLabel.Text = (2 - wait / 10) + "";
                    }
                    if (wait == 20)
                    {
                        step++;
                        SetText(1, -1);
                        foreach (int i in onList)
                        {
                            if (button4.Contains(i) && !button6.Contains(i))
                            {
                                button4.Clear();
                                button4.Add(i);
                                button7.Remove(i);
                                break;
                            }
                        }
                    }
                    wait++;
                }
                else
                {
                    if (wait != 0)
                    {
                        countLabel.Text = "";
                    }
                    wait = 0;
                }
            }
            else if (step == 5)
            {
                int count = 0;
                List<int> onList = new List<int>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (buttons[i] && !button4.Contains(i) && !button5.Contains(i) && !button6.Contains(i) && !button7.Contains(i))
                    {
                        count++;
                        onList.Add(i);
                    }
                }
                if (count == 1 && stick.GetCurrentState().X > 90)
                {
                    if (wait % 10 == 0)
                    {
                        countLabel.Text = (2 - wait / 10) + "";
                    }
                    if (wait == 20)
                    {
                        step++;
                        infoLabel.Text = "SELECTボタンを押して下さい";
                        notchButton = onList[0];
                    }
                    wait++;
                }
                else
                {
                    if (wait != 0)
                    {
                        countLabel.Text = "";
                    }
                    wait = 0;
                }
            }
            else if (step == 6)
            {
                int count = 0;
                List<int> onList = new List<int>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (buttons[i] && !button4.Contains(i) && !button5.Contains(i) && !button6.Contains(i) && !button7.Contains(i) && notchButton != i)
                    {
                        count++;
                        onList.Add(i);
                    }
                }
                if (count == 1)
                {
                    if (wait % 10 == 0)
                    {
                        countLabel.Text = (1 - wait / 10) + "";
                    }
                    if (wait == 10)
                    {
                        step++;
                        infoLabel.Text = "STARTボタンを押して下さい";
                        selectButton = onList[0];
                    }
                    wait++;
                }
                else
                {
                    if (wait != 0)
                    {
                        countLabel.Text = "";
                    }
                    wait = 0;
                }
            }
            else if (step == 7)
            {
                int count = 0;
                List<int> onList = new List<int>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (buttons[i] && !button4.Contains(i) && !button5.Contains(i) && !button6.Contains(i) && !button7.Contains(i) &&
                        notchButton != i && selectButton != i)
                    {
                        count++;
                        onList.Add(i);
                    }
                }
                if (count == 1)
                {
                    if (wait % 10 == 0)
                    {
                        countLabel.Text = (1 - wait / 10) + "";
                    }
                    if (wait == 10)
                    {
                        step++;
                        infoLabel.Text = "Aボタンを押して下さい";
                        startButton = onList[0];
                    }
                    wait++;
                }
                else
                {
                    if (wait != 0)
                    {
                        countLabel.Text = "";
                    }
                    wait = 0;
                }
            }
            else if (step == 8)
            {
                int count = 0;
                List<int> onList = new List<int>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (buttons[i] && !button4.Contains(i) && !button5.Contains(i) && !button6.Contains(i) && !button7.Contains(i) &&
                       notchButton != i && selectButton != i && startButton != i)
                    {
                        count++;
                        onList.Add(i);
                    }
                }
                if (count == 1)
                {
                    if (wait % 10 == 0)
                    {
                        countLabel.Text = (1 - wait / 10) + "";
                    }
                    if (wait == 10)
                    {
                        step++;
                        infoLabel.Text = "Bボタンを押して下さい";
                        aButton = onList[0];
                    }
                    wait++;
                }
                else
                {
                    if (wait != 0)
                    {
                        countLabel.Text = "";
                    }
                    wait = 0;
                }
            }
            else if (step == 9)
            {
                int count = 0;
                List<int> onList = new List<int>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (buttons[i] && !button4.Contains(i) && !button5.Contains(i) && !button6.Contains(i) && !button7.Contains(i) &&
                        notchButton != i && selectButton != i && startButton != i && aButton != i)
                    {
                        count++;
                        onList.Add(i);
                    }
                }
                if (count == 1)
                {
                    if (wait % 10 == 0)
                    {
                        countLabel.Text = (1 - wait / 10) + "";
                    }
                    if (wait == 10)
                    {
                        step++;
                        infoLabel.Text = "Cボタンを押して下さい";
                        bButton = onList[0];
                    }
                    wait++;
                }
                else
                {
                    if (wait != 0)
                    {
                        countLabel.Text = "";
                    }
                    wait = 0;
                }
            }
            else if (step == 10)
            {
                int count = 0;
                List<int> onList = new List<int>();
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (buttons[i] && !button4.Contains(i) && !button5.Contains(i) && !button6.Contains(i) && !button7.Contains(i) &&
                        notchButton != i && selectButton != i && startButton != i && aButton != i && bButton != i)
                    {
                        count++;
                        onList.Add(i);
                    }
                }
                if (count == 1)
                {
                    if (wait % 10 == 0)
                    {
                        countLabel.Text = (1 - wait / 10) + "";
                    }
                    if (wait == 10)
                    {
                        step++;
                        cButton = onList[0];
                        DenshadeGoInterface.settings.Button0 = notchButton;
                        DenshadeGoInterface.settings.Button1 = cButton;
                        DenshadeGoInterface.settings.Button2 = bButton;
                        DenshadeGoInterface.settings.Button3 = aButton;
                        DenshadeGoInterface.settings.Button4 = button4[0];
                        DenshadeGoInterface.settings.Button5 = button5[0];
                        DenshadeGoInterface.settings.Button6 = button6[0];
                        DenshadeGoInterface.settings.Button7 = button7[0];
                        DenshadeGoInterface.settings.Button8 = startButton;
                        DenshadeGoInterface.settings.Button9 = selectButton;
                        DenshadeGoInterface.configForm.loadUpDown();
                        Close();
                    }
                    wait++;
                }
                else
                {
                    if (wait != 0)
                    {
                        countLabel.Text = "";
                    }
                    wait = 0;
                }
            }
            stick.Unacquire();
        }

        private void SetText(int notch, int brea)
        {
            string bStr = "ブレーキを" + brea;
            if (brea == 9) bStr = "非常";
            if (brea == -1) bStr = "";
            string notchStr = "ノッチを" + notch;
            if (notch == 0) notchStr = "ノッチを切";
            if (notch == -1) notchStr = "";
            if (notchStr.Length > 0 && bStr.Length > 0) bStr = "、" + bStr;
            infoLabel.Text = notchStr + bStr + "にしてください";
        }

        private bool[] GetButtons()
        {
            if (stick.Acquire().IsFailure) return new bool[128];
            bool[] buttons = stick.GetCurrentState().GetButtons();
            stick.Unacquire();
            return buttons;
        }

        private void cacelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
