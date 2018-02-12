using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ADBTest
{
    public partial class ADBTestForm : Form
    {
        //适用屏幕分辨率 : OPPO A59S (720 x 1280)
        //适用屏幕分辨率 : HUAWEI M3 (1600 x 2560)

        Hotkey UnityHotKey;
        int SwitchHotKey;
        int _tickCount = 0;
        int TickCouct
        {
            get => _tickCount;
            set
            {
                _tickCount = value;
                this.Text = string.Format("Egg Inc. [计数器: {0}]", _tickCount);
                UnityNotifyIcon.Text = string.Format("{0} : {1}", TaskButton.Text, _tickCount);
            }
        }

        private enum AndroidDeviceModel
        {
            OPPO_A59S=0,
            HUAWEI_M3=1
        }
        private AndroidDeviceModel _deviceModel = AndroidDeviceModel.OPPO_A59S;
        private AndroidDeviceModel DeviceModel
        {
            get => _deviceModel;
            set
            {
                _deviceModel = value;

                switch (value)
                {
                    case AndroidDeviceModel.OPPO_A59S:
                        {
                            // 720 x 1280
                            // {20, 1060, 680, 200}
                            MainButtonLocation = new Point(360, 1160);
                            // (210, 766, 300, 80)
                            OpenBoxButtonLocation = new Point(360, 806);
                            // 图标间间隔26px
                            BoxLocations = new Point[] {
                                // {630, 430, 64, 64}
                                new Point(630, 430),
                                // {630, 340, 64, 64}
                                new Point(630, 340),
                                // {630, 250, 64, 64}
                                new Point(630, 250),
                                // {630, 160, 64, 64}
                                new Point(630, 160)
                            };
                            BoxSize = new Size(64, 64);
                            BoxCheckPadding = new Padding(8, 8, 8, 8);
                            break;
                        }
                    case AndroidDeviceModel.HUAWEI_M3:
                        {
                            // 1600 x 2560
                            // { 35, 2175, 1530, 350}
                            MainButtonLocation = new Point(800, 2350);
                            // {537, 1500, 525, 140}
                            OpenBoxButtonLocation = new Point(800, 1570);
                            // 图标间间隔45px
                            BoxLocations = new Point[] {
                                // {1442, 751, 112, 112}
                                new Point(1442, 751),
                                // {1442, 594, 112, 112}
                                new Point(1442, 594),
                                // {1442, 437, 112, 112}
                                new Point(1442, 437),
                                // {1442, 280, 112, 112}
                                new Point(1442, 280)
                            };
                            BoxSize = new Size(112, 112);
                            BoxCheckPadding = new Padding(16, 16, 16, 16);
                            break;
                        }
                    default:
                        break;
                }
            }
        }
        Padding BoxCheckPadding = new Padding(8, 8, 8, 8);
        Size BoxSize = new Size(64, 64);
        Point[] BoxLocations = new Point[] {
                new Point(630, 430),
                new Point(630, 340),
                new Point(630, 250),
                new Point(630, 160)
            };
        Point OpenBoxButtonLocation = new Point(360, 806);
        Point MainButtonLocation = new Point(360, 1160);

        Thread ADBThread = null;
        Process ADBProcess = new Process() { StartInfo = new ProcessStartInfo(Path.Combine(Application.StartupPath, @"adb\adb.exe")) { WindowStyle = ProcessWindowStyle.Hidden } };

        Thread ScreenShotThread = null;
        Process ScreenShotProcess = new Process() { StartInfo = new ProcessStartInfo(Path.Combine(Application.StartupPath, @"adb\adb.exe")) { WindowStyle = ProcessWindowStyle.Hidden } };

        Thread OpenBoxThread = null;
        Process OpenBoxProcess = new Process() { StartInfo = new ProcessStartInfo(Path.Combine(Application.StartupPath, @"adb\adb.exe")) { WindowStyle = ProcessWindowStyle.Hidden } };

        NotifyIcon UnityNotifyIcon = new NotifyIcon();
        Bitmap StartIcon = new Bitmap(48, 48);
        Bitmap AbortIcon = new Bitmap(48, 48);

        private bool _activated = false;
        private bool HelperActivated
        {
            get => _activated;
            set
            {
                _activated = value;

                try{
                    if (!ADBProcess.HasExited) ADBProcess.Kill();
                }catch (InvalidOperationException) { }
                try{
                    if (!ScreenShotProcess.HasExited) ScreenShotProcess.Kill();
                }catch (InvalidOperationException) { }

                try{
                    if (!OpenBoxProcess.HasExited) OpenBoxProcess.Kill();
                }catch (InvalidOperationException) { }

                if (value)
                {
                    ADBThread = new Thread(Egg) { IsBackground = true };
                    ADBThread.Start();
                    this.TopMost = true;

                    TaskButton.Text = "已开始";
                    UnityNotifyIcon.Text = string.Format("{0} : {1}", TaskButton.Text, TickCouct);
                    UnityNotifyIcon.Icon = Icon.FromHandle(StartIcon.GetHicon());
                    TaskButton.BackColor = Color.Orange;
                }
                else
                {
                    ADBThread?.Abort();
                    ADBThread = null;
                    ScreenShotThread?.Abort();
                    ScreenShotThread = null;
                    OpenBoxThread?.Abort();
                    OpenBoxThread = null;

                    TaskButton.Text = "已停止";
                    UnityNotifyIcon.Text = string.Format("{0} : {1}", TaskButton.Text, TickCouct);
                    UnityNotifyIcon.Icon = Icon.FromHandle(AbortIcon.GetHicon());
                    TaskButton.BackColor = Color.DeepSkyBlue;
                }
            }
                
        }

        private enum IconType
        {
            Unknown,
            News,
            AD,
            Box
        }

        public ADBTestForm()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();

            //热键为 ALT+Z
            UnityHotKey = new Hotkey(this.Handle);
            SwitchHotKey = UnityHotKey.RegisterHotkey(Keys.Z, Hotkey.KeyFlags.MOD_ALT);
            UnityHotKey.OnHotkey += new HotkeyEventHandler(OnHotkey);

            using (Graphics IconGraphics = Graphics.FromImage(StartIcon))
                IconGraphics.FillEllipse(Brushes.Orange, 0, 0, 48, 48);
            using (Graphics IconGraphics = Graphics.FromImage(AbortIcon))
                IconGraphics.FillEllipse(Brushes.DeepSkyBlue, 0, 0, 48, 48);

            UnityNotifyIcon.Click += delegate { HelperActivated = !HelperActivated; };
            UnityNotifyIcon.Visible = true;
            UnityNotifyIcon.Text = "点击开始";
            UnityNotifyIcon.Icon = Icon.FromHandle(AbortIcon.GetHicon());

            OPPOA59SRadioButton.CheckedChanged += delegate (object s, EventArgs e) { if(OPPOA59SRadioButton.Checked) DeviceModel = AndroidDeviceModel.OPPO_A59S; };
            HUAWEIM3RadioButton.CheckedChanged += delegate (object s, EventArgs e) { if (HUAWEIM3RadioButton.Checked) DeviceModel = AndroidDeviceModel.HUAWEI_M3; };

#if (! DEBUG)
            TestButton.Hide();
#endif
        }

        public void OnHotkey(int HotkeyID)
        {
            if (HotkeyID == SwitchHotKey)
            {
                HelperActivated = !HelperActivated;
            }
        }

        private void Egg()
        {
            while (true)
            {
                TickCouct++;

                ADBProcess.StartInfo.Arguments = string.Format("shell input swipe {0} {1} {0} {1} {2}000",MainButtonLocation.X, MainButtonLocation.Y, InputTimeout.Value);
                ADBProcess.Start();
                ADBProcess.WaitForExit();

                ADBIdle();
            }
        }

        /// <summary>
        /// ADB进入空闲状态
        /// </summary>
        private void ADBIdle()
        {
            //截图
            if (ScreenShotCheckBox.Checked && TickCouct % 24 == 0)
            {
                ScreenShot();
            }
            //打开箱子
            if (OpenBoxCheckBox.Checked && TickCouct % 5 == 0)
            {
                OpenBox();
            }

            Thread.Sleep(Convert.ToInt32(SleepTimeout.Value) * 1000);
        }

        /// <summary>
        /// 异步截屏
        /// </summary>
        private void ScreenShot()
        {
            if (ScreenShotThread == null || ScreenShotThread.ThreadState == System.Threading.ThreadState.Stopped)
            {
                ScreenShotThread = new Thread(new ThreadStart(delegate
                {
                    ScreenShotProcess.StartInfo.Arguments = string.Format("shell /system/bin/screencap -p /sdcard/screenshot-{0}.png", DateTime.Now.ToString("MM-dd+HH-mm-ss"));
                    ScreenShotProcess.Start();
                    ScreenShotProcess.WaitForExit();
                }))
                { IsBackground = true };
                ScreenShotThread.Start();
            }
        }

        /// <summary>
        /// 异步打开箱子
        /// </summary>
        private void OpenBox()
        {
            if (OpenBoxThread == null || OpenBoxThread.ThreadState == System.Threading.ThreadState.Stopped)
            {
                OpenBoxThread = new Thread(new ThreadStart(delegate 
                {
                    string ScreenShotPath = Path.Combine(Path.GetTempPath(), "screenshot-forbox.png");

                    //截屏
                    OpenBoxProcess.StartInfo.Arguments = "shell /system/bin/screencap -p /sdcard/screenshot-forbox.png";
                    OpenBoxProcess.Start();
                    OpenBoxProcess.WaitForExit();
                    //获取截图
                    OpenBoxProcess.StartInfo.Arguments = string.Format("pull /sdcard/screenshot-forbox.png {0}", ScreenShotPath);
                    OpenBoxProcess.Start();
                    OpenBoxProcess.WaitForExit();
                    //File.Copy(ScreenShotPath, @"D:\ScreenShot-ForBox.png", true);
                    //遍历箱子坐标
                    foreach (Tuple<Point, IconType> CheckResult in CheckBoxes(ScreenShotPath))
                    {
                        Debug.Print(string.Format("{0} : {1}", CheckResult.Item1.ToString(), CheckResult.Item2.ToString()));

                        switch (CheckResult.Item2)
                        {
                            case IconType.Box:
                                {
                                    Debug.Print("盒子");
                                    OpenBoxProcess.StartInfo.Arguments = string.Format("shell input tap {0} {1}", CheckResult.Item1.X, CheckResult.Item1.Y);
                                    OpenBoxProcess.Start();
                                    OpenBoxProcess.WaitForExit();
                                    OpenBoxProcess.StartInfo.Arguments = string.Format("shell input tap {0} {1}", OpenBoxButtonLocation.X, OpenBoxButtonLocation.Y);
                                    OpenBoxProcess.Start();
                                    OpenBoxProcess.WaitForExit();
                                    break;
                                }
                            case IconType.AD:
                                {
                                    Debug.Print("广告");

                                    //还没有退出广告的代码，暂不开启功能
                                    break;

                                    /* 广告类型：
                                     * 0：VPN未连接时不会进入广告 / 注意广告时间内不要息屏
                                     * 1：15秒或25秒后自动关闭
                                     * 2：25秒后广告结束，但无法使用返回键返回游戏，只能点击屏幕右上角的关闭按钮才可以关闭按钮
                                     * 3：15秒或25秒后广告结束，可以通过返回键关闭广告
                                     */

                                    OpenBoxProcess.StartInfo.Arguments = string.Format("shell input tap {0} {1}", CheckResult.Item1.X, CheckResult.Item1.Y);
                                    OpenBoxProcess.Start();
                                    OpenBoxProcess.WaitForExit();
                                    OpenBoxProcess.StartInfo.Arguments = string.Format("shell input tap {0} {1}", 473, 807);
                                    OpenBoxProcess.Start();
                                    OpenBoxProcess.WaitForExit();

                                    break;
                                }
                            case IconType.News:
                                {
                                    Debug.Print("新闻");
                                    break;
                                }
                            case IconType.Unknown:
                                {
                                    Debug.Print("未知的图标类型");
                                    break;
                                }
                            default:
                                {
                                    Debug.Print("其他的图标类型");
                                    break;
                                }
                        }
                    }

                    if (File.Exists(ScreenShotPath)) File.Delete(ScreenShotPath);
                }))
                { IsBackground = true };
                OpenBoxThread.Start();
            }
        }

        /// <summary>
        /// 检查箱子位置
        /// </summary>
        /// <param name="ScreenBoxPath">截图文件路径</param>
        /// <returns>箱子坐标，是否为广告</returns>
        private IEnumerable<Tuple<Point, IconType>> CheckBoxes(string ScreenBoxPath)
        {
            //报纸 x1
            //广告 x1
            //箱子 x2

            if (!File.Exists(ScreenBoxPath)) yield break;

            //!注意，这里需要从下向上寻找图标，并从下向上 yield return ，否则点击了上方的图标之后，下方的图标会移动到上方被点击后的图标位置
            Bitmap ScreenShotBitmap = null;
            using (FileStream ScreenShotStream = new FileStream(ScreenBoxPath, FileMode.Open))
            {
                ScreenShotBitmap = Image.FromStream(ScreenShotStream) as Bitmap;
            }

            //检查箱子位置及图标
            foreach (Point BoxLocation in BoxLocations)
            {
                if (IsWhite(ScreenShotBitmap.GetPixel(BoxLocation.X + BoxSize.Width / 2, BoxLocation.Y + BoxCheckPadding.Top)) &&
                    IsWhite(ScreenShotBitmap.GetPixel(BoxLocation.X + BoxSize.Width / 2, BoxLocation.Y + BoxSize.Height - BoxCheckPadding.Bottom)) &&
                    IsWhite(ScreenShotBitmap.GetPixel(BoxLocation.X + BoxCheckPadding.Left, BoxLocation.Y + BoxSize.Height / 2)) &&
                    IsWhite(ScreenShotBitmap.GetPixel(BoxLocation.X + BoxSize.Width - BoxCheckPadding.Right, BoxLocation.Y + BoxSize.Height / 2))
                    )
                {
                    yield return new Tuple<Point, IconType>(new Point(BoxLocation.X + BoxSize.Width / 2, BoxLocation.Y + BoxSize.Height / 2),
                        GetIconType(ScreenShotBitmap.GetPixel(BoxLocation.X + BoxSize.Width / 2, BoxLocation.Y + BoxSize.Height / 2)));
                }
            }

            ScreenShotBitmap?.Dispose();
        }

        /// <summary>
        /// 检查颜色是否为白色
        /// </summary>
        /// <returns>是否为白色</returns>
        private bool IsWhite(Color TargetColor)
        {
            return (
                TargetColor.R == 255 &&
                TargetColor.G == 255 &&
                TargetColor.B == 255
                );
        }

        /// <summary>
        /// 根据图标中心颜色获取图标类型
        /// </summary>
        /// <param name="TargetColor">图标中心颜色</param>
        /// <returns></returns>
        private IconType GetIconType(Color TargetColor)
        {
            Debug.Print(TargetColor.ToString());

            //Color[A = 255, R = 159, G = 168, B = 151]
            if (TargetColor.G > TargetColor.R && TargetColor.R > TargetColor.B)
                return IconType.News;

            //Color[A = 255, R = 255, G = 255, B = 255]
            if (TargetColor.R == TargetColor.G && TargetColor.G == TargetColor.B)
                return IconType.AD;
            
            //Color[A = 255, R = 165, G = 141, B = 70]
            if (TargetColor.R > TargetColor.G && TargetColor.G > TargetColor.B)
                return IconType.Box;

            return IconType.Unknown;
        }

        private void TaskButton_Click(object sender, EventArgs e)
        {
            HelperActivated = !HelperActivated;
        }
        
        private void ADBTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnityHotKey.UnregisterHotkeys();
            UnityNotifyIcon.Dispose();
        }

        private void OpenBoxCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            PlayADCheckBox.Enabled = OpenBoxCheckBox.Checked;
        }

        private void ScreenShotButton_Click(object sender, EventArgs e)
        {
            ScreenShot();
        }

        private void OpenBoxButton_Click(object sender, EventArgs e)
        {
            OpenBox();
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
#if (DEBUG)
            foreach (Tuple<Point, IconType> CheckResult in CheckBoxes(@"D:\CSharp\ADBTest\Game\OPPO-A59S_3.png"))
            {
                Debug.Print(string.Format("{0} : {1}", CheckResult.Item1.ToString(), CheckResult.Item2.ToString()));
            }
#endif
            }

    }
}
