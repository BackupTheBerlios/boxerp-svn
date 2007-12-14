using System;
using System.Collections.Generic;
using System.Text;
using Boxerp.Client;

namespace consoleResponsiveHelperTest
{
	class Program
	{
		static void Main(string[] args)
		{
			ResponsivenessSingleton.GetInstance().Initialize();
			TestView view = new TestView();
			TestController controller = new TestController(
						new ConsoleResponsiveHelper(ConcurrencyMode.Modal), view);
			controller.DoWork();
		}
	}
}
