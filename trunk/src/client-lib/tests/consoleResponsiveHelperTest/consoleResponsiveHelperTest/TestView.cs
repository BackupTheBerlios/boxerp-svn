using System;
using System.Collections.Generic;
using System.Text;
using Boxerp.Client;

namespace consoleResponsiveHelperTest
{
	public class TestView : IView<TestController>
	{
		private TestController _controller;
		
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

		public void ShowMessage(string msg)
		{
			Console.Out.WriteLine(msg);
		}
	}
}
