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
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SleepTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.InputTimeout)).BeginInit();
            this.SuspendLayout();
            // 
            // TaskButton
            // 
            this.TaskButton.Dock = System.Windows.Forms.DockStyle.Top;
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
            this.SleepTimeout.Location = new System.Drawing.Point(102, 47);
            this.SleepTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SleepTimeout.Name = "SleepTimeout";
            this.SleepTimeout.Size = new System.Drawing.Size(60, 26);
            this.SleepTimeout.TabIndex = 1;
            this.SleepTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SleepTimeout.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // InputTimeout
            // 
            this.InputTimeout.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.InputTimeout.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.InputTimeout.Location = new System.Drawing.Point(1, 47);
            this.InputTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.InputTimeout.Name = "InputTimeout";
            this.InputTimeout.Size = new System.Drawing.Size(60, 26);
            this.InputTimeout.TabIndex = 2;
            this.InputTimeout.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.InputTimeout.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(61, 47);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 26);
            this.button1.TabIndex = 3;
            this.button1.Text = "截图";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ADBTestForm
            // 
            this.AcceptButton = this.TaskButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(163, 74);
            this.Controls.Add(this.button1);
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
            ((System.ComponentModel.ISupportInitialize)(this.SleepTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.InputTimeout)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button TaskButton;
        private System.Windows.Forms.NumericUpDown SleepTimeout;
        private System.Windows.Forms.NumericUpDown InputTimeout;
        private System.Windows.Forms.Button button1;
    }
}

