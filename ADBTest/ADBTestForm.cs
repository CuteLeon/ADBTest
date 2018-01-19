using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
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
        Process ADBProcess = new Process() { StartInfo = new ProcessStartInfo(@"D:\Desktops\adb\adb.exe") { WindowStyle = ProcessWindowStyle.Hidden } };

        public ADBTestForm()
        {
            InitializeComponent();

            //热键为 ALT+Z
            UnityHotKey = new Hotkey(this.Handle);
            SwitchHotKey = UnityHotKey.RegisterHotkey(Keys.Z, Hotkey.KeyFlags.MOD_ALT);
            UnityHotKey.OnHotkey += new HotkeyEventHandler(OnHotkey);
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
                ADBProcess.StartInfo.Arguments = "shell input swipe 360 1200 360 1200 20000";
                ADBProcess.Start();
                ADBProcess.WaitForExit();
                Thread.Sleep(25000);
            }
        }

        private void Switch()
        {
            if (TestThread == null)
            {
                TestThread = new Thread(Egg) { IsBackground = true };

                TestThread.Start();
            }
            else
            {
                TestThread?.Abort();
                TestThread = null;
            }
            TaskButton.Text = TestThread == null ? "已停止" : "已开始";
            TaskButton.BackColor = TestThread == null ? Color.DeepSkyBlue : Color.OrangeRed;
        }

        private void TaskButton_Click(object sender, EventArgs e)
        {
            Switch();
        }
    }
}
