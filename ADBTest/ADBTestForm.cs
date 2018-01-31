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
        //TODO:使用UnityNotifyIcon弹出错误信息气泡提示

        Hotkey UnityHotKey;
        int SwitchHotKey;
        int _TickCount = 0;
        int TickCouct
        {
            get => _TickCount;
            set
            {
                _TickCount = value;
                this.Text = string.Format("Egg Inc. [计数器: {0}]", _TickCount);
            }
        }

        Thread ADBThread = null;
        Process ADBProcess = new Process() { StartInfo = new ProcessStartInfo(Path.Combine(Application.StartupPath, @"adb\adb.exe")) { WindowStyle = ProcessWindowStyle.Hidden } };

        Thread ScreenShotThread = null;
        Process ScreenShotProcess = new Process() { StartInfo = new ProcessStartInfo(Path.Combine(Application.StartupPath, @"adb\adb.exe")) { WindowStyle = ProcessWindowStyle.Hidden } };

        Thread OpenBoxThread = null;
        Process OpenBoxProcess = new Process() { StartInfo = new ProcessStartInfo(Path.Combine(Application.StartupPath, @"adb\adb.exe")) { WindowStyle = ProcessWindowStyle.Hidden } };

        NotifyIcon UnityNotifyIcon = new NotifyIcon();
        Bitmap StartIcon = new Bitmap(48, 48);
        Bitmap AbortIcon = new Bitmap(48, 48);

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

            UnityNotifyIcon.Click += delegate { Switch(); };
            UnityNotifyIcon.Visible = true;
            UnityNotifyIcon.Text = "点击开始";
            UnityNotifyIcon.Icon = Icon.FromHandle(AbortIcon.GetHicon());
        }

        public void OnHotkey(int HotkeyID)
        {
            if (HotkeyID == SwitchHotKey)
            {
                Switch();
            }
        }

        private void Egg()
        {
            while (true)
            {
                TickCouct++;

                ADBProcess.StartInfo.Arguments = string.Format("shell input swipe 360 1200 360 1200 {0}000", InputTimeout.Value);
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
            if (ScreenShotCheckBox.Checked && TickCouct % 15 == 0)
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
                                    //Button: {210, 766, 300, 80}
                                    Debug.Print("盒子");
                                    OpenBoxProcess.StartInfo.Arguments = string.Format("shell input tap {0} {1}", CheckResult.Item1.X, CheckResult.Item1.Y);
                                    OpenBoxProcess.Start();
                                    OpenBoxProcess.WaitForExit();
                                    OpenBoxProcess.StartInfo.Arguments = string.Format("shell input tap {0} {1}", 360, 806);
                                    OpenBoxProcess.Start();
                                    OpenBoxProcess.WaitForExit();
                                    break;
                                }
                            case IconType.AD:
                                {
                                    //No,Thx: {140, 767, 214, 80}
                                    Debug.Print("广告");
                                    /*
                                    OpenBoxProcess.StartInfo.Arguments = string.Format("shell input tap {0} {1}", CheckResult.Item1.X, CheckResult.Item1.Y);
                                    OpenBoxProcess.Start();
                                    OpenBoxProcess.WaitForExit();
                                    OpenBoxProcess.StartInfo.Arguments = string.Format("shell input tap {0} {1}", 247, 807);
                                    OpenBoxProcess.Start();
                                    OpenBoxProcess.WaitForExit();
                                     */
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
        /// <param name="ScreenBoxPath">棘突文件路径</param>
        /// <returns>箱子坐标，是否为广告</returns>
        private IEnumerable<Tuple<Point, IconType>> CheckBoxes(string ScreenBoxPath)
        {
            //报纸 x1
            //广告 x1
            //箱子 x2

            if (!File.Exists(ScreenBoxPath)) yield break;

            //!注意，这里需要从下向上寻找图标，并从下向上 yield return ，否则点击了上方的图标之后，下方的图标会移动到上方被点击后的图标位置
            Point[] BoxLocations = new Point[] {
                new Point(630, 430),
                new Point(630, 340),
                new Point(630, 250),
                new Point(630, 160)
            };
            Bitmap ScreenShotBitmap = null;
            using (FileStream ScreenShotStream = new FileStream(ScreenBoxPath, FileMode.Open))
            {
                ScreenShotBitmap = Image.FromStream(ScreenShotStream) as Bitmap;
            }

            //检查箱子位置及图标
            foreach (Point BoxLocation in BoxLocations)
            {
                if (IsWhite(ScreenShotBitmap.GetPixel(BoxLocation.X + 32, BoxLocation.Y + 8)) &&
                    IsWhite(ScreenShotBitmap.GetPixel(BoxLocation.X + 32, BoxLocation.Y + 56)) &&
                    IsWhite(ScreenShotBitmap.GetPixel(BoxLocation.X + 8, BoxLocation.Y + 32)) &&
                    IsWhite(ScreenShotBitmap.GetPixel(BoxLocation.X + 56, BoxLocation.Y + 32))
                    )
                {
                    yield return new Tuple<Point, IconType>(new Point(BoxLocation.X + 32, BoxLocation.Y + 32), 
                        GetIconType(ScreenShotBitmap.GetPixel(BoxLocation.X + 32, BoxLocation.Y + 32)));
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

        /// <summary>
        /// 切换开关
        /// </summary>
        private void Switch()
        {
            if (ADBThread == null)
            {
                ADBThread = new Thread(Egg) { IsBackground = true };
                ADBThread.Start();
                this.TopMost = true;
            }
            else
            {
                try
                {
                    if (!ADBProcess.HasExited) ADBProcess.Kill();
                    if (!ScreenShotProcess.HasExited) ScreenShotProcess.Kill();
                    if (!OpenBoxProcess.HasExited) OpenBoxProcess.Kill();
                }
                catch{ }

                ADBThread?.Abort();
                ADBThread = null;
                ScreenShotThread?.Abort();
                ScreenShotThread = null;
                OpenBoxThread?.Abort();
                OpenBoxThread = null;
            }

            TaskButton.Text = ADBThread == null ? "已停止" : "已开始";
            UnityNotifyIcon.Text = TaskButton.Text;
            UnityNotifyIcon.Icon = Icon.FromHandle((ADBThread == null ? AbortIcon : StartIcon).GetHicon());
            TaskButton.BackColor = ADBThread == null ? Color.DeepSkyBlue : Color.Orange;
        }

        private void TaskButton_Click(object sender, EventArgs e)
        {
            Switch();
        }
        
        private void ADBTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnityHotKey.UnregisterHotkeys();
            UnityNotifyIcon.Dispose();
        }

        private void ScreenShotCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ScreenShotCheckBox.Checked)
                ScreenShot();
        }

        private void OpenBoxCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (OpenBoxCheckBox.Checked)
                OpenBox();
        }

    }
}
