using System;
using Boxerp.Client;

namespace  gtkResponsivenessTest
{		
	public interface ITestWindow : IView<TestController>
	{
		void ShowSomething();
	}
}
