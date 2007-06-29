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
			_responsiveHelper.StartAsyncCall(runMethod);
		}
		
		private void runMethod()
		{
			try
			{
				System.Threading.Thread.Sleep(5000);
			}
			catch (System.Threading.ThreadAbortException ex)
			{
				_responsiveHelper.OnAbortAsyncCall(ex);
			}
			catch (Exception ex)
			{
				_responsiveHelper.OnAsyncException(ex);
			}
			finally
			{
				StopAsyncCall();
			}
		}
		
		
		protected override void OnAsyncOperationFinish(Object sender, ThreadEventArgs args)
		{
			if ((args.Success) && (args.MethodBase == Method(runMethod)))
			{
				_control.ShowSomething();
			}
		}
	}
}
