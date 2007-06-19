using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Boxerp.Client.WPF
{
	public class WpfResponsiveHelper : AbstractResponsiveHelper
	{
		WaitDialog waitDialog;
				
		public WpfResponsiveHelper()
		{
		}

		// Just to notify clients when the transfer is completed
		private ThreadEventHandler transferCompleteEventHandler;
		public override event ThreadEventHandler TransferCompleteEvent
		{
			add
			{
				transferCompleteEventHandler += value;
			}
			remove
			{
				transferCompleteEventHandler -= value;
			}
		}

		public override void StartAsyncCallList(ResponsiveEnum transferType, IController controller)
		{
			waitDialog = new WaitDialog();
			waitDialog.CancelEvent += OnCancel;
			_transferSuccess = true;
			waitDialog.Show();
			base.StartAsyncCallList(transferType, controller);
		}

		public override void StartAsyncCall(SimpleDelegate method)
		{
			waitDialog = new WaitDialog();
			waitDialog.CancelEvent += OnCancel;
			waitDialog.Show();
			_transferSuccess = true;
			base.StartAsyncCall(method);
		}

		public override void OnCancel(object sender, EventArgs e)
		{
			CancelRequest = true;
			
			//QuestionDialog qdialog = new QuestionDialog();
			//qdialog.Message = "The process is being cancelled, please wait. Do you want to force abort right now?";
			//int rtype = qdialog.Run();
			//if (rtype == (int)ResponseType.Yes)
			//{
				ForceAbort();
			//}

			//TODO: Show a dialog :" Please wait while cancelling" with
			//a button to force cancelation by aborting threads
		}

		private void TransferCompleted(object sender, ThreadEventArgs e)
		{
			ResponsiveEnum transferType = (ResponsiveEnum)sender;
			waitDialog.Stop();
			waitDialog.Destroy();
			waitDialog.Close();
			if (_transferSuccess) // FIXME: transferSuccess must be syncrhonized
			{
				e.Success = true;
				e.TransferType = transferType;
			}
			else
			{
				string msg = "";
				foreach (string i in _exceptionsMsgPool.Values)
					msg += i + "\n";
				MessageBox.Show(msg);
				e.Success = false;
			}
			if (this.transferCompleteEventHandler != null)
			{
				transferCompleteEventHandler(sender, e);
			}
		}

		public override void OnTransferCompleted(object sender, ThreadEventArgs e)
		{
			waitDialog.Dispatcher.BeginInvoke(
				System.Windows.Threading.DispatcherPriority.Normal, 
				new ThreadEventHandler(TransferCompleted),
				sender,
				e
			);
		}

		
	}
}
