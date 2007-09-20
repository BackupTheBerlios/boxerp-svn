namespace winFormsTestApp2
{
	partial class Form2
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
			this.button4 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this._age = new System.Windows.Forms.TextBox();
			this._description = new System.Windows.Forms.TextBox();
			this._name = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(94, 225);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(169, 23);
			this.button4.TabIndex = 17;
			this.button4.Text = "Read Data";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.OnReadData);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(94, 196);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(169, 23);
			this.button3.TabIndex = 16;
			this.button3.Text = "Change the Data";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.OnChangeData);
			// 
			// _age
			// 
			this._age.Location = new System.Drawing.Point(94, 113);
			this._age.Name = "_age";
			this._age.Size = new System.Drawing.Size(169, 20);
			this._age.TabIndex = 15;
			// 
			// _description
			// 
			this._description.Location = new System.Drawing.Point(96, 64);
			this._description.Name = "_description";
			this._description.Size = new System.Drawing.Size(169, 20);
			this._description.TabIndex = 14;
			// 
			// _name
			// 
			this._name.Location = new System.Drawing.Point(96, 18);
			this._name.Name = "_name";
			this._name.Size = new System.Drawing.Size(169, 20);
			this._name.TabIndex = 13;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(27, 113);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(29, 13);
			this.label3.TabIndex = 12;
			this.label3.Text = "Age:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(27, 67);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(63, 13);
			this.label2.TabIndex = 11;
			this.label2.Text = "Description:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(27, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 10;
			this.label1.Text = "Name:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(96, 150);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 18;
			this.button1.Text = "Undo";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.OnUndo);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(177, 150);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 19;
			this.button2.Text = "Redo";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.OnRedo);
			// 
			// Form2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this._age);
			this.Controls.Add(this._description);
			this.Controls.Add(this._name);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Name = "Form2";
			this.Text = "Form2";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.TextBox _age;
		private System.Windows.Forms.TextBox _description;
		private System.Windows.Forms.TextBox _name;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
	}
}