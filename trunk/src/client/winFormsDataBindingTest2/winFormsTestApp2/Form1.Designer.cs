namespace winFormsTestApp2
{
	partial class Form1
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
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this._name = new System.Windows.Forms.TextBox();
			this._description = new System.Windows.Forms.TextBox();
			this._age = new System.Windows.Forms.TextBox();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(18, 14);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(58, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(18, 85);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(100, 20);
			this.label2.TabIndex = 1;
			this.label2.Text = "Description:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(18, 155);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(43, 20);
			this.label3.TabIndex = 2;
			this.label3.Text = "Age:";
			// 
			// _name
			// 
			this._name.Location = new System.Drawing.Point(122, 9);
			this._name.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this._name.Name = "_name";
			this._name.Size = new System.Drawing.Size(252, 26);
			this._name.TabIndex = 3;
			// 
			// _description
			// 
			this._description.Location = new System.Drawing.Point(122, 80);
			this._description.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this._description.Name = "_description";
			this._description.Size = new System.Drawing.Size(252, 26);
			this._description.TabIndex = 4;
			// 
			// _age
			// 
			this._age.Location = new System.Drawing.Point(118, 155);
			this._age.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this._age.Name = "_age";
			this._age.Size = new System.Drawing.Size(252, 26);
			this._age.TabIndex = 5;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(118, 283);
			this.button3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(254, 35);
			this.button3.TabIndex = 8;
			this.button3.Text = "Change the Data";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.OnChangeData);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(118, 328);
			this.button4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(254, 35);
			this.button4.TabIndex = 9;
			this.button4.Text = "Read Data";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.OnReadData);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(579, 472);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this._age);
			this.Controls.Add(this._description);
			this.Controls.Add(this._name);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox _name;
		private System.Windows.Forms.TextBox _description;
		private System.Windows.Forms.TextBox _age;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
	}
}

