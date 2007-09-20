namespace winFormsTestApp2
{
	partial class Form3
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
			this._collection = new System.Windows.Forms.ListView();
			this.label1 = new System.Windows.Forms.Label();
			this._title = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// _collection
			// 
			this._collection.Location = new System.Drawing.Point(12, 64);
			this._collection.Name = "_collection";
			this._collection.Size = new System.Drawing.Size(545, 273);
			this._collection.TabIndex = 0;
			this._collection.UseCompatibleStateImageBehavior = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 28);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(30, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Title:";
			// 
			// _title
			// 
			this._title.Location = new System.Drawing.Point(48, 25);
			this._title.Name = "_title";
			this._title.Size = new System.Drawing.Size(194, 20);
			this._title.TabIndex = 2;
			// 
			// Form3
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(569, 448);
			this.Controls.Add(this._title);
			this.Controls.Add(this.label1);
			this.Controls.Add(this._collection);
			this.Name = "Form3";
			this.Text = "Form3";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListView _collection;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox _title;
	}
}