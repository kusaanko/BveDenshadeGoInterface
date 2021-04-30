using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;
using SlimDX.DirectInput;
using System.Diagnostics;

namespace Mackoy.Bvets.DenshadeGoInterface
{
    public class DenshadeGoInterface : Mackoy.Bvets.IInputDevice
    {
        public static DirectInput input;
        public static Joystick stick;

        public static Settings settings = null;
        public static ConfigForm configForm;

        public event InputEventHandler KeyDown;
        public event InputEventHandler KeyUp;
        public event InputEventHandler LeverMoved;

        private int notch;
        private int breakLevel;

        private bool[] button = new bool[5];

        private Stopwatch stopwatch = new Stopwatch();

        public static Joystick GetStick()
        {
            List<SlimDX.DirectInput.Joystick> sticks = new List<SlimDX.DirectInput.Joystick>();
            foreach (DeviceInstance device in input.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                try
                {
                    if(device.ProductName.Equals(settings.ControllerName))
                    {
                        stick = new SlimDX.DirectInput.Joystick(input, device.InstanceGuid);
                        break;
                    }
                }
                catch (DirectInputException)
                {
                }
            }
            if (stick == null)
            {
                return null;
            }
            stick.Acquire();
            foreach (DeviceObjectInstance deviceObject in stick.GetObjects())
            {
                if ((deviceObject.ObjectType & ObjectDeviceType.Axis) != 0)
                    stick.GetObjectPropertiesById((int)deviceObject.ObjectType).SetRange(-100, 100);
            }
            stick.Unacquire();
            return stick;
        }

        public void Load(string settingsPath)
        {
            input = new DirectInput();
            settings = Settings.LoadFromXml(settingsPath);

            if(GetStick() == null)
            {
                MessageBox.Show("コントローラーを検出できませんでした。", "DenshadeGoInput", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SetAxisRanges(int[][] ranges)
        {
        }

        public void Tick()
        {
            if (stick == null)
            {
                if (stopwatch.ElapsedMilliseconds > 1000)
                {
                    GetStick();
                    stopwatch.Stop();
                    stopwatch.Reset();
                    stopwatch.Start();
                }
                if (!stopwatch.IsRunning)
                {
                    stopwatch.Start();
                }
                return;
            }
            if(stick.Acquire().IsFailure)
            {
                stick = null;
                return;
            }
            JoystickState state = stick.GetCurrentState();
            bool[] buttons = state.GetButtons();
            int x = state.X;
            int y = state.Y;
            stick.Unacquire();
            int breakLevelBefore = breakLevel;
            bool[,] breakMap = new bool[,]
            {
                { true, true, false, true },
                { false, true, true, true },
                { false, true, false, true },
                { true, true, true, false },
                { true, true, false, false },
                { false, true, true, false },
                { false, true, false, false },
                { true, false, true, true },
                { true, false, false, true },
                { false, false, false, false },
            };
            for (int i = 0;i < breakMap.GetLength(0);i++)
            {
                if (buttons[settings.Button4] == breakMap[i, 0] && 
                    buttons[settings.Button5] == breakMap[i, 1] &&
                    buttons[settings.Button6] == breakMap[i, 2] &&
                    buttons[settings.Button7] == breakMap[i, 3])
                {
                    breakLevel = i;
                    break;
                }
            }
            if (breakLevelBefore != breakLevel)
            {
                onLeverMoved(3, -breakLevel);
            }
            int notchBefore = notch;
            if (notch == 1 && !buttons[settings.Button0] && x == -100)
            {
                notch = 0;
            }
            if (buttons[settings.Button0] && x == 100)
            {
                notch = 1;
            }
            if (!buttons[settings.Button0] && x == 100)
            {
                notch = 2;
            }
            if (buttons[settings.Button0] && x == -100)
            {
                notch = 3;
            }
            if ((notch == 3 || notch == 5) && !buttons[settings.Button0] && x == -100)
            {
                notch = 4;
            }
            if (buttons[settings.Button0] && x >= -10 && x <= 10)
            {
                notch = 5;
            }
            if (notchBefore != notch)
            {
                onLeverMoved(3, notch);
            }
            if (buttons[settings.Button8])
            {
                if (!button[0])
                {
                    onKeyDown(settings.ButtonS[0], settings.ButtonS[1]);
                    button[0] = true;
                }
            }
            else
            {
                if (button[0])
                {
                    onKeyUp(settings.ButtonS[0], settings.ButtonS[1]);
                    button[0] = false;
                }
            }
            if (buttons[settings.Button9])
            {
                if (!button[1])
                {
                    onKeyDown(settings.ButtonP[0], settings.ButtonP[1]);
                    button[1] = true;
                }
            }
            else
            {
                if (button[1])
                {
                    onKeyUp(settings.ButtonP[0], settings.ButtonP[1]);
                    button[1] = false;
                }
            }
            if (buttons[settings.Button3])
            {
                if (!button[2])
                {
                    onKeyDown(settings.ButtonA[0], settings.ButtonA[1]);
                    button[2] = true;
                }
            }
            else
            {
                if (button[2])
                {
                    onKeyUp(settings.ButtonA[0], settings.ButtonA[1]);
                    button[2] = false;
                }
            }
            if (buttons[settings.Button2])
            {
                if (!button[3])
                {
                    onKeyDown(settings.ButtonB[0], settings.ButtonB[1]);
                    button[3] = true;
                }
            }
            else
            {
                if (button[3])
                {
                    onKeyUp(settings.ButtonB[0], settings.ButtonB[1]);
                    button[3] = false;
                }
            }
            if (buttons[settings.Button1])
            {
                if (!button[4])
                {
                    onKeyDown(settings.ButtonC[0], settings.ButtonC[1]);
                    button[4] = true;
                }
            }
            else
            {
                if (button[4])
                {
                    onKeyUp(settings.ButtonC[0], settings.ButtonC[1]);
                    button[4] = false;
                }
            }
        }

        public void Configure(IWin32Window owner)
        {
            using (configForm= new ConfigForm())
            {
                configForm.ShowDialog(owner);
            }
        }

        public void Dispose()
        {

            if (settings != null)
            {
                settings.SaveToXml();
                settings = null;
            }
        }

        private void onLeverMoved(int axis, int notch)
        {
            if (LeverMoved != null)
            {
                LeverMoved(this, new InputEventArgs(axis, notch));
            }
        }

        private void onKeyDown(int axis, int keyCode)
        {
            if (LeverMoved != null)
            {
                KeyDown(this, new InputEventArgs(axis, keyCode));
            }
        }

        private void onKeyUp(int axis, int keyCode)
        {
            if (LeverMoved != null)
            {
                KeyUp(this, new InputEventArgs(axis, keyCode));
            }
        }
    }
}
