using System;
using System.Collections.Generic;
using System.Text;
using Boxerp.Client;
using System.Threading;

namespace winformsResponsivenessTest
{
	public class Controller : AbstractController<ISampleView, Controller>
	{
		Random _random = new Random();
		int _randomMax;

		public Controller(IResponsiveClient helper, ISampleView view)
			: base (helper, view)
		{

		}

		public void DoAsyncOperation(int randonMax)
		{
			_randomMax = randonMax;
			ResponsiveHelper.StartAsyncCall(doAsyncOperation);
		}

		private void doAsyncOperation()
		{
			try
			{
				System.Threading.Thread.Sleep(_random.Next(_randomMax));
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
				Console.Out.WriteLine("doAsyncOperation finishing on thread:" + Thread.CurrentThread.ManagedThreadId);
				StopAsyncCall();
			}
		}

		protected override void OnAsyncOperationFinish(object sender, ThreadEventArgs args)
		{
			Console.Out.WriteLine("Operation has finished");
		}
	}
}
