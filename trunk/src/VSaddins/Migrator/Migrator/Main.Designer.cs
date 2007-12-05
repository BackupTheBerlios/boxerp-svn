namespace Migrator
{
	partial class Main
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
			this._controllers = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this._sharedData = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this._testViews = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this._interfaces = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this._files = new System.Windows.Forms.ListView();
			this.File = new System.Windows.Forms.ColumnHeader();
			this.label6 = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this._projects = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this._winForms = new System.Windows.Forms.CheckBox();
			this._wpf = new System.Windows.Forms.CheckBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this._header = new System.Windows.Forms.RichTextBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this._statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _controllers
			// 
			this._controllers.FormattingEnabled = true;
			this._controllers.Location = new System.Drawing.Point(116, 76);
			this._controllers.Name = "_controllers";
			this._controllers.Size = new System.Drawing.Size(364, 21);
			this._controllers.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 79);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(91, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Controllers Folder:";
			// 
			// _sharedData
			// 
			this._sharedData.FormattingEnabled = true;
			this._sharedData.Location = new System.Drawing.Point(116, 103);
			this._sharedData.Name = "_sharedData";
			this._sharedData.Size = new System.Drawing.Size(364, 21);
			this._sharedData.TabIndex = 5;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 106);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(99, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "SharedData Folder:";
			// 
			// _testViews
			// 
			this._testViews.FormattingEnabled = true;
			this._testViews.Location = new System.Drawing.Point(116, 130);
			this._testViews.Name = "_testViews";
			this._testViews.Size = new System.Drawing.Size(364, 21);
			this._testViews.TabIndex = 7;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 133);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(94, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "Test Views Folder:";
			// 
			// _interfaces
			// 
			this._interfaces.FormattingEnabled = true;
			this._interfaces.Location = new System.Drawing.Point(116, 157);
			this._interfaces.Name = "_interfaces";
			this._interfaces.Size = new System.Drawing.Size(364, 21);
			this._interfaces.TabIndex = 9;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(9, 160);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(89, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "Interfaces Folder:";
			// 
			// _files
			// 
			this._files.CheckBoxes = true;
			this._files.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.File});
			this._files.FullRowSelect = true;
			this._files.GridLines = true;
			this._files.Location = new System.Drawing.Point(9, 333);
			this._files.Name = "_files";
			this._files.Size = new System.Drawing.Size(474, 272);
			this._files.TabIndex = 10;
			this._files.UseCompatibleStateImageBehavior = false;
			this._files.View = System.Windows.Forms.View.Details;
			// 
			// File
			// 
			this.File.Tag = "Check the box to create shared data classes";
			this.File.Text = "[Needs Shared Data] - File name";
			this.File.Width = 467;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(9, 317);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(31, 13);
			this.label6.TabIndex = 11;
			this.label6.Text = "Files:";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(374, 611);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(107, 21);
			this.button1.TabIndex = 12;
			this.button1.Text = "Close";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.OnClose);
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(261, 611);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(107, 21);
			this.button2.TabIndex = 13;
			this.button2.Text = "Migrate";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.OnMigrate);
			// 
			// _projects
			// 
			this._projects.FormattingEnabled = true;
			this._projects.Location = new System.Drawing.Point(116, 49);
			this._projects.Name = "_projects";
			this._projects.Size = new System.Drawing.Size(364, 21);
			this._projects.TabIndex = 15;
			this._projects.SelectedIndexChanged += new System.EventHandler(this.OnProjectChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 52);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(43, 13);
			this.label1.TabIndex = 14;
			this.label1.Text = "Project:";
			// 
			// _winForms
			// 
			this._winForms.AutoSize = true;
			this._winForms.Checked = true;
			this._winForms.CheckState = System.Windows.Forms.CheckState.Checked;
			this._winForms.Location = new System.Drawing.Point(116, 12);
			this._winForms.Name = "_winForms";
			this._winForms.Size = new System.Drawing.Size(73, 17);
			this._winForms.TabIndex = 16;
			this._winForms.Text = "WinForms";
			this._winForms.UseVisualStyleBackColor = true;
			// 
			// _wpf
			// 
			this._wpf.AutoSize = true;
			this._wpf.Location = new System.Drawing.Point(202, 12);
			this._wpf.Name = "_wpf";
			this._wpf.Size = new System.Drawing.Size(50, 17);
			this._wpf.TabIndex = 17;
			this._wpf.Text = "WPF";
			this._wpf.UseVisualStyleBackColor = true;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(12, 13);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(64, 13);
			this.label7.TabIndex = 18;
			this.label7.Text = "GUI Toolkit:";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(12, 195);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(69, 13);
			this.label8.TabIndex = 19;
			this.label8.Text = "Files Header:";
			// 
			// _header
			// 
			this._header.Location = new System.Drawing.Point(12, 211);
			this._header.Name = "_header";
			this._header.Size = new System.Drawing.Size(467, 96);
			this._header.TabIndex = 20;
			this._header.Text = "using System;\nusing System.Collections.Generic;\nusing System.Text;\nusing Boxerp.C" +
				"lient;";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._statusLabel});
			this.statusStrip1.Location = new System.Drawing.Point(0, 656);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(499, 22);
			this.statusStrip1.TabIndex = 21;
			this.statusStrip1.Text = "Ready";
			// 
			// _statusLabel
			// 
			this._statusLabel.Name = "_statusLabel";
			this._statusLabel.Size = new System.Drawing.Size(38, 17);
			this._statusLabel.Text = "Ready";
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(499, 678);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this._header);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label7);
			this.Controls.Add(this._wpf);
			this.Controls.Add(this._winForms);
			this.Controls.Add(this._projects);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label6);
			this.Controls.Add(this._files);
			this.Controls.Add(this._interfaces);
			this.Controls.Add(this.label5);
			this.Controls.Add(this._testViews);
			this.Controls.Add(this.label4);
			this.Controls.Add(this._sharedData);
			this.Controls.Add(this.label3);
			this.Controls.Add(this._controllers);
			this.Controls.Add(this.label2);
			this.Name = "Main";
			this.Text = "Boxerp Client Migrator";
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox _controllers;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox _sharedData;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox _testViews;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox _interfaces;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ListView _files;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.ComboBox _projects;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ColumnHeader File;
		private System.Windows.Forms.CheckBox _winForms;
		private System.Windows.Forms.CheckBox _wpf;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.RichTextBox _header;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel _statusLabel;
	}
}