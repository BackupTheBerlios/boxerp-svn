
using System;
using System.Collections;
using System.Threading;
using System.Reflection;
using Gtk;

namespace Boxerp.Client.GtkSharp
{
	public class GtkResponsiveHelper : AbstractResponsiveHelper
	{
		WaitDialog _waitDialog;
		WarningDialog _warningDialog;
		protected Gtk.Window _parentWindow = null;
		
		public GtkResponsiveHelper(Gtk.Window parent)
		{
			_parentWindow = parent;
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
		    if (_parentWindow != null)
		    {
			    _waitDialog = new WaitDialog(_parentWindow);
			}
			else
			{
			    _waitDialog = new WaitDialog();
			}
			_waitDialog.CancelEvent += OnCancel;
			_transferSuccess = true;
			base.StartAsyncCallList(transferType, controller);
		}
		
		public override void StartAsyncCall(SimpleDelegate method)
		{
		    if (_parentWindow != null)
		    {
			    _waitDialog = new WaitDialog(_parentWindow);
			}
			else
			{
			    _waitDialog = new WaitDialog();
			}
			_waitDialog.CancelEvent += OnCancel;
			_transferSuccess = true;
			base.StartAsyncCall(method);
		}
		
        public override void OnCancel(object sender, EventArgs e)
		{
        	CancelRequest = true;
        	QuestionDialog qdialog = new QuestionDialog();
        	qdialog.Message = "The process is being cancelled, please wait. Do you want to force abort right now?";
            int rtype = qdialog.Run();
            if (rtype == (int)ResponseType.Yes)
            {
                ForceAbort();      
            }
            
        	//TODO: Show a dialog :" Please wait while cancelling" with
        	//a button to force cancelation by aborting threads
		}
		
		private void TransferCompleted(object sender, EventArgs e)
		{
			ResponsiveEnum transferType = (ResponsiveEnum)sender;
			_waitDialog.Stop();
			_waitDialog.Destroy();
			if (_transferSuccess) // FIXME: transferSuccess must be syncrhonized
         	{
				if (transferType == ResponsiveEnum.Read)
				{
					// todo: set up the eventargs.success
				}
				else
				{
				    // todo: set up the eventargs.success
				}
				if (_parentWindow != null)
				{
				    _parentWindow.Present();
				}
			}
			else
			{
				_warningDialog = new WarningDialog();
				string msg = "";
				foreach (string i in _exceptionsMsgPool.Values)
					msg += i + "\n";
         		_warningDialog.Message = msg;
         		_warningDialog.QuitOnOk = false;
            	_warningDialog.Present();
         	}		
         	if (this.transferCompleteEventHandler != null)
         	{
         	    transferCompleteEventHandler(sender, (ThreadEventArgs)e);
         	}
        }
		
		public override void OnTransferCompleted(object sender, ThreadEventArgs e)
		{
			Application.Invoke(sender, e, TransferCompleted);
		}

		
	}
}
