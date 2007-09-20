using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace winFormsTestApp2
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
		}

		private void OnTestProxy2(object sender, EventArgs e)
		{
			Form1 f1 = new Form1();
			f1.Show();
		}

		private void OnTestDynProxy(object sender, EventArgs e)
		{
			Form2 f2 = new Form2();
			f2.Show();
		}

		private void OnTestWithCollection(object sender, EventArgs e)
		{
			Form3 f3 = new Form3();
			f3.Show();
		}
	}
}