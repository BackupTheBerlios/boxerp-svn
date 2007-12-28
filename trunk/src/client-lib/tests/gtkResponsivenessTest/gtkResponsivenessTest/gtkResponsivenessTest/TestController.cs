// /home/carlos/boxerp_completo/trunk/src/client/administrator/Controllers/TestController.cs created with MonoDevelop
// User: carlos at 10:05 PM 6/23/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Boxerp.Client;

namespace  gtkResponsivenessTest
{
	public class TestController : AbstractController<ITestWindow, TestController>
	{
		public TestController(IResponsiveClient helper, ITestWindow view)
			: base (helper, view)
		{
		}
		
		public void RunMethod()
		{
			ResponsiveHelper.StartAsyncCall(runMethod);
		}

		private void runMethod()
		{
			try
			{
				for (int i = 0; i < 10; i++)
				{
					Console.WriteLine("Iteration:" + i);
					ResponsiveHelper.UpdateWaitMessage("Iteration:" + i);
					System.Threading.Thread.Sleep(1000);
				}
			}
			catch (System.Threading.ThreadAbortException ex)
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

		public void RunMethodWithCancellationLogic()
		{
			ResponsiveHelper.StartAsyncCall(runMethodWithCancellationLogic);
		}

		private void runMethodWithCancellationLogic()
		{
			try
			{
				for (int i = 0; i < 5; i++)
				{
					if (ResponsiveHelper.CancelRequested)
					{
						break;
					}
					System.Threading.Thread.Sleep(i * 1000);
				}
			}
			catch (System.Threading.ThreadAbortException ex)
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

		
		
		
		protected override void OnAsyncOperationFinish(Object sender, ThreadEventArgs args)
		{
			if ((args.Success) && (args.MethodBase == MethodBase(runMethod)))
			{
				View.ShowSomething();
			}
			if ((args.Success) && (args.MethodBase == MethodBase(runMethodWithCancellationLogic)))
			{
				View.ShowSomething();
			}
		}
	}
}
