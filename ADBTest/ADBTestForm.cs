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
        Hotkey UnityHotKey;
        int SwitchHotKey;

        Thread TestThread = null;
        Process ADBProcess = new Process() { StartInfo = new ProcessStartInfo(Path.Combine(Application.StartupPath, @"adb\adb.exe")) { WindowStyle = ProcessWindowStyle.Hidden } };

        NotifyIcon UnityNotifyIcon = new NotifyIcon();
        Bitmap StartIcon = new Bitmap(48, 48);
        Bitmap AbortIcon = new Bitmap(48, 48);

        public ADBTestForm()
        {
            InitializeComponent();

            //热键为 ALT+Z
            UnityHotKey = new Hotkey(this.Handle);
            SwitchHotKey = UnityHotKey.RegisterHotkey(Keys.Z, Hotkey.KeyFlags.MOD_ALT);
            UnityHotKey.OnHotkey += new HotkeyEventHandler(OnHotkey);

            using (Graphics IconGraphics = Graphics.FromImage(StartIcon))
                IconGraphics.FillEllipse(Brushes.OrangeRed, 0, 0, 48, 48);
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
                ADBProcess.StartInfo.Arguments = string.Format("shell input swipe 360 1200 360 1200 {0}000", InputTimeout.Value);
                ADBProcess.Start();
                ADBProcess.WaitForExit();
                if (DateTime.Now.Minute % 15 ==0) SaveScreen();//截图
                Thread.Sleep(Convert.ToInt32(SleepTimeout.Value) * 1000);
            }
        }

        private void SaveScreen()
        {
            ADBProcess.StartInfo.Arguments = string.Format("shell /system/bin/screencap -p /sdcard/screenshot-{0}.png", DateTime.Now.ToString("MM-dd+HH-mm-ss"));
            ADBProcess.Start();
            //ADBProcess.WaitForExit();
        }

        private void Switch()
        {
            if (TestThread == null)
            {
                TestThread = new Thread(Egg) { IsBackground = true };
                TestThread.Start();
                this.TopMost = true;

            }
            else
            {
                if (!ADBProcess.HasExited) ADBProcess.Kill();
                TestThread?.Abort();
                TestThread = null;
            }
            TaskButton.Text = TestThread == null ? "已停止" : "已开始";
            UnityNotifyIcon.Text = TaskButton.Text;
            UnityNotifyIcon.Icon = Icon.FromHandle((TestThread == null ? AbortIcon : StartIcon).GetHicon());
            TaskButton.BackColor = TestThread == null ? Color.DeepSkyBlue : Color.OrangeRed;
        }

        private void TaskButton_Click(object sender, EventArgs e)
        {
            Switch();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveScreen();
        }

        private void ADBTestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnityHotKey.UnregisterHotkeys();
            UnityNotifyIcon.Dispose();
        }

    }
}
