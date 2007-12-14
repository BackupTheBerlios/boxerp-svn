using System;
using Boxerp.Client;

namespace ResponsivenessTest2
{		
	public interface ITestWindow : IView<TestController>
	{
		void ShowSomething();
	}
}
