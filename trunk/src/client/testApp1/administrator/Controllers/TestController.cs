// /home/carlos/boxerp_completo/trunk/src/client/administrator/Controllers/TestController.cs created with MonoDevelop
// User: carlos at 10:05 PM 6/23/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Admin.Interfaces;
using Boxerp.Client;

namespace Admin.Controllers
{
	
	
	public class TestController : AbstractController
	{
		ITestWindow _control;
		
		public TestController(IResponsiveClient helper, ITestWindow control)
			: base (helper)
		{
			_control = control;
		}
		
		public void RunMethod()
		{
			ResponsiveHelper.StartAsyncCall(runMethod);
		}

		private void runMethod()
		{
			try
			{
				System.Threading.Thread.Sleep(5000);
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
				_control.ShowSomething();
			}
			if ((args.Success) && (args.MethodBase == MethodBase(runMethodWithCancellationLogic)))
			{
				_control.ShowSomething();
			}
		}
	}
}
