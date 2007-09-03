namespace BoxerpWinformsSampleProject
{
    partial class MainFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DownloadFileBtn = new System.Windows.Forms.Button();
            this.FileLocationTxt = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // DownloadFileBtn
            // 
            this.DownloadFileBtn.Location = new System.Drawing.Point(205, 51);
            this.DownloadFileBtn.Name = "DownloadFileBtn";
            this.DownloadFileBtn.Size = new System.Drawing.Size(75, 23);
            this.DownloadFileBtn.TabIndex = 0;
            this.DownloadFileBtn.Text = "Download File";
            this.DownloadFileBtn.UseVisualStyleBackColor = true;
            this.DownloadFileBtn.Click += new System.EventHandler(this.DownloadFileBtn_Click);
            // 
            // FileLocationTxt
            // 
            this.FileLocationTxt.Location = new System.Drawing.Point(12, 12);
            this.FileLocationTxt.Name = "FileLocationTxt";
            this.FileLocationTxt.Size = new System.Drawing.Size(268, 21);
            this.FileLocationTxt.TabIndex = 1;
            this.FileLocationTxt.Text = "http://download.microsoft.com/vwdsetup.exe";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 86);
            this.Controls.Add(this.FileLocationTxt);
            this.Controls.Add(this.DownloadFileBtn);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "MainFrm";
            this.Text = "Main Test Form";
            this.Load += new System.EventHandler(this.MainFrm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button DownloadFileBtn;
        private System.Windows.Forms.TextBox FileLocationTxt;
    }
}

