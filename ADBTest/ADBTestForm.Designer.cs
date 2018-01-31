namespace ADBTest
{
    partial class ADBTestForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.TaskButton = new System.Windows.Forms.Button();
            this.SleepTimeout = new System.Windows.Forms.NumericUpDown();
            this.InputTimeout = new System.Windows.Forms.NumericUpDown();
            this.ScreenShotCheckBox = new System.Windows.Forms.CheckBox();
            this.OpenBoxCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.SleepTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InputTimeout)).BeginInit();
            this.SuspendLayout();
            // 
            // TaskButton
            // 
            this.TaskButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.TaskButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.TaskButton.FlatAppearance.BorderSize = 5;
            this.TaskButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gray;
            this.TaskButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.TaskButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.TaskButton.Location = new System.Drawing.Point(0, 0);
            this.TaskButton.Name = "TaskButton";
            this.TaskButton.Size = new System.Drawing.Size(163, 47);
            this.TaskButton.TabIndex = 0;
            this.TaskButton.Text = "开始";
            this.TaskButton.UseVisualStyleBackColor = true;
            this.TaskButton.Click += new System.EventHandler(this.TaskButton_Click);
            // 
            // SleepTimeout
            // 
            this.SleepTimeout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SleepTimeout.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.SleepTimeout.Location = new System.Drawing.Point(81, 48);
            this.SleepTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SleepTimeout.Name = "SleepTimeout";
            this.SleepTimeout.Size = new System.Drawing.Size(81, 26);
            this.SleepTimeout.TabIndex = 1;
            this.SleepTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SleepTimeout.Value = new decimal(new int[] {
            18,
            0,
            0,
            0});
            // 
            // InputTimeout
            // 
            this.InputTimeout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InputTimeout.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.InputTimeout.Location = new System.Drawing.Point(1, 48);
            this.InputTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.InputTimeout.Name = "InputTimeout";
            this.InputTimeout.Size = new System.Drawing.Size(81, 26);
            this.InputTimeout.TabIndex = 2;
            this.InputTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.InputTimeout.Value = new decimal(new int[] {
            22,
            0,
            0,
            0});
            // 
            // ScreenShotCheckBox
            // 
            this.ScreenShotCheckBox.BackColor = System.Drawing.SystemColors.Control;
            this.ScreenShotCheckBox.Location = new System.Drawing.Point(1, 75);
            this.ScreenShotCheckBox.Name = "ScreenShotCheckBox";
            this.ScreenShotCheckBox.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.ScreenShotCheckBox.Size = new System.Drawing.Size(161, 21);
            this.ScreenShotCheckBox.TabIndex = 4;
            this.ScreenShotCheckBox.Text = "自动截屏记录";
            this.ScreenShotCheckBox.UseVisualStyleBackColor = false;
            this.ScreenShotCheckBox.CheckedChanged += new System.EventHandler(this.ScreenShotCheckBox_CheckedChanged);
            // 
            // OpenBoxCheckBox
            // 
            this.OpenBoxCheckBox.BackColor = System.Drawing.SystemColors.Control;
            this.OpenBoxCheckBox.Location = new System.Drawing.Point(1, 97);
            this.OpenBoxCheckBox.Name = "OpenBoxCheckBox";
            this.OpenBoxCheckBox.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.OpenBoxCheckBox.Size = new System.Drawing.Size(161, 21);
            this.OpenBoxCheckBox.TabIndex = 5;
            this.OpenBoxCheckBox.Text = "自动拾取箱子";
            this.OpenBoxCheckBox.UseVisualStyleBackColor = false;
            this.OpenBoxCheckBox.CheckedChanged += new System.EventHandler(this.OpenBoxCheckBox_CheckedChanged);
            // 
            // ADBTestForm
            // 
            this.AcceptButton = this.TaskButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(163, 119);
            this.Controls.Add(this.OpenBoxCheckBox);
            this.Controls.Add(this.ScreenShotCheckBox);
            this.Controls.Add(this.InputTimeout);
            this.Controls.Add(this.SleepTimeout);
            this.Controls.Add(this.TaskButton);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ADBTestForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ADBTest";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ADBTestForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.SleepTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InputTimeout)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button TaskButton;
        private System.Windows.Forms.NumericUpDown SleepTimeout;
        private System.Windows.Forms.NumericUpDown InputTimeout;
        private System.Windows.Forms.CheckBox ScreenShotCheckBox;
        private System.Windows.Forms.CheckBox OpenBoxCheckBox;
    }
}

