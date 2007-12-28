using System;
using System.Collections.Generic;
using System.Text;
using Boxerp.Client;
using Boxerp.Client.GtkSharp;
using System.Threading;

namespace  gtkResponsivenessTest
{
	public class Controller : AbstractController<ISampleView, Controller>
	{
		Random _random = new Random();

		public Controller(IResponsiveClient helper, ISampleView view)
			: base (helper, view)
		{

		}

		public void DoAsyncOperation()
		{
			ResponsiveHelper.StartAsyncCall(doAsyncOperation);
		}

		private void doAsyncOperation()
		{
			try
			{
				System.Threading.Thread.Sleep(_random.Next(1000));
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

		protected override void OnAsyncOperationFinish(object sender, ThreadEventArgs args)
		{
			Console.Out.WriteLine("Operation has finished");
		}
	}
}
