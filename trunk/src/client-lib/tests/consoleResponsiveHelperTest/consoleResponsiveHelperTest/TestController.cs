using System;
using System.Collections.Generic;
using System.Text;
using Boxerp.Client;
using System.Threading;

namespace consoleResponsiveHelperTest
{
	public class TestController : AbstractController<TestView, TestController>
	{
		public TestController(IResponsiveClient helper, TestView view)
			: base (helper, view)
		{

		}

		protected override void OnAsyncOperationFinish(object sender, ThreadEventArgs args)
		{
			Console.Out.WriteLine("done");
		}

		public void DoWork()
		{
			ResponsiveHelper.StartAsyncCall(doWork);
		}

		private void doWork()
		{
			try
			{
				for (int i = 0; i < 10; i++)
				{
					Console.Out.WriteLine("printing async");
					Thread.Sleep(500);
					ResponsiveHelper.CallUIfromAsyncThread(
					(
						delegate()
						{
							View.ShowMessage("CallUIfromAsyncThread");
						}));
					Thread.Sleep(100);
				}
			}
			catch (ThreadAbortException ex)
			{
				ResponsiveHelper.OnAbortAsyncCall(ex);
			}
			catch (Exception ex)
			{
				ResponsiveHelper.OnAsyncException(ex);
			}
			finally
			{
				StopAsyncCall();
			}
		}

	}
}
