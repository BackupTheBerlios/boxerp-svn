using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Boxerp.Client;
using Boxerp.Client.WindowsForms;

namespace ResponsivenessTest2
{
	public partial class Form1 : Form, ITestWindow
	{
		private TestController _controller;
		public Form1()
		{
			InitializeComponent();
			_controller = new TestController(new WinFormsResponsiveHelper(ConcurrencyMode.Modal), this);
		}

		#region ITestWindow Members

		public void ShowSomething()
		{
			MessageBox.Show("Done");
		}

		#endregion

		#region IView<TestController> Members

		public TestController Controller
		{
			get
			{
				return _controller;
			}
			set
			{
				_controller = value;
			}
		}

		#endregion

		private void OnStart(object sender, EventArgs e)
		{
			Controller.RunMethod();
		}
	}
}