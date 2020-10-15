namespace GetColor
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelX = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.textBoxX = new System.Windows.Forms.TextBox();
            this.textBoxY = new System.Windows.Forms.TextBox();
            this.panelColor = new System.Windows.Forms.Panel();
            this.textBoxColor = new System.Windows.Forms.TextBox();
            this.textBoxColor16 = new System.Windows.Forms.TextBox();
            this.buttonToggle = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(12, 51);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(36, 12);
            this.labelX.TabIndex = 0;
            this.labelX.Text = "X座標";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Location = new System.Drawing.Point(12, 76);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(36, 12);
            this.labelY.TabIndex = 1;
            this.labelY.Text = "Y座標";
            // 
            // textBoxX
            // 
            this.textBoxX.Location = new System.Drawing.Point(75, 48);
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.Size = new System.Drawing.Size(197, 19);
            this.textBoxX.TabIndex = 2;
            // 
            // textBoxY
            // 
            this.textBoxY.Location = new System.Drawing.Point(75, 73);
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.Size = new System.Drawing.Size(197, 19);
            this.textBoxY.TabIndex = 3;
            // 
            // panelColor
            // 
            this.panelColor.Location = new System.Drawing.Point(14, 98);
            this.panelColor.Name = "panelColor";
            this.panelColor.Size = new System.Drawing.Size(257, 131);
            this.panelColor.TabIndex = 4;
            // 
            // textBoxColor
            // 
            this.textBoxColor.Location = new System.Drawing.Point(12, 236);
            this.textBoxColor.Name = "textBoxColor";
            this.textBoxColor.Size = new System.Drawing.Size(260, 19);
            this.textBoxColor.TabIndex = 5;
            // 
            // textBoxColor16
            // 
            this.textBoxColor16.Location = new System.Drawing.Point(11, 261);
            this.textBoxColor16.Name = "textBoxColor16";
            this.textBoxColor16.Size = new System.Drawing.Size(260, 19);
            this.textBoxColor16.TabIndex = 6;
            // 
            // buttonToggle
            // 
            this.buttonToggle.Location = new System.Drawing.Point(27, 12);
            this.buttonToggle.Name = "buttonToggle";
            this.buttonToggle.Size = new System.Drawing.Size(228, 23);
            this.buttonToggle.TabIndex = 7;
            this.buttonToggle.Text = "スタート (Click)";
            this.buttonToggle.UseVisualStyleBackColor = true;
            this.buttonToggle.Click += new System.EventHandler(this.buttonToggle_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 287);
            this.Controls.Add(this.buttonToggle);
            this.Controls.Add(this.textBoxColor16);
            this.Controls.Add(this.textBoxColor);
            this.Controls.Add(this.panelColor);
            this.Controls.Add(this.textBoxY);
            this.Controls.Add(this.textBoxX);
            this.Controls.Add(this.labelY);
            this.Controls.Add(this.labelX);
            this.Name = "Form1";
            this.Text = "GetColor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.TextBox textBoxX;
        private System.Windows.Forms.TextBox textBoxY;
        private System.Windows.Forms.Panel panelColor;
        private System.Windows.Forms.TextBox textBoxColor;
        private System.Windows.Forms.TextBox textBoxColor16;
        private System.Windows.Forms.Button buttonToggle;
    }
}

