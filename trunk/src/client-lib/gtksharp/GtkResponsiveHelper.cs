
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Reflection;
using Gtk;

namespace Boxerp.Client.GtkSharp
{
	public class GtkResponsiveHelper : AbstractResponsiveHelper
	{
		WaitDialog _waitDialog;
		WaitWindow _waitWindow;
		Queue<WaitDialog> _dialogs = new Queue<WaitDialog>();
		Queue<WaitWindow> _windows = new Queue<WaitWindow>();
		//WarningDialog _warningDialog;
		protected Gtk.Window _parentWindow = null;
		
		public GtkResponsiveHelper(Gtk.Window parent, ConcurrencyMode mode)
			: base(mode)
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
			if ((_concurrencyMode == ConcurrencyMode.Modal) || (_concurrencyMode == ConcurrencyMode.Parallel)
				|| (RunningThreads == 0))
			{
				if (_concurrencyMode == ConcurrencyMode.Modal)
				{
					_waitDialog = new WaitDialog(/*_parentWindow*/);
					_waitDialog.CancelEvent += OnCancel;
					_dialogs.Enqueue(_waitDialog);
				}
				else
				{
					_waitWindow = new WaitWindow();
					_waitWindow.CancelEvent += OnCancel;
					_windows.Enqueue(_waitWindow);
				}
			}
			
			base.StartAsyncCallList(transferType, controller);

			if (_concurrencyMode == ConcurrencyMode.Modal)
			{
				_waitDialog.Show();
			}
			else
			{
				_waitWindow.Show();
				// TODO : if the window is minimized show it in the middle of the screen
			}
		}
		
		public override void StartAsyncCall(SimpleDelegate method)
		{
			if ((_concurrencyMode == ConcurrencyMode.Modal) || (_concurrencyMode == ConcurrencyMode.Parallel)
				|| (RunningThreads == 0))
			{
				if (_concurrencyMode == ConcurrencyMode.Modal)
				{
					_waitDialog = new WaitDialog(/*_parentWindow*/);
					_waitDialog.CancelEvent += OnCancel;
					_dialogs.Enqueue(_waitDialog);
				}
				else
				{
					_waitWindow = new WaitWindow();
					_waitWindow.CancelEvent += OnCancel;
					_windows.Enqueue(_waitWindow);
				}
			}
			
			base.StartAsyncCall(method);

			if (_concurrencyMode == ConcurrencyMode.Modal)
			{
				_waitDialog.Show();
			}
			else
			{
				_waitWindow.Show();
				_waitWindow.Present();
				// TODO : if the window is minimized show it in the middle of the screen
			}
		}
		
        public override void OnCancel(object sender, EventArgs e)
		{
        	CancelRequested = true;
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
			ThreadEventArgs evArgs = (ThreadEventArgs) e;
			
			ResponsiveEnum operationType = evArgs.OperationType;
			if (_concurrencyMode == ConcurrencyMode.Modal)
			{
				WaitDialog wDialog = _dialogs.Dequeue();
				wDialog.Stop();
				wDialog.Hide();
				wDialog.Destroy();
			}
			else
			{
				WaitWindow wWindow = _windows.Dequeue();
				wWindow.Stop();
				wWindow.Hide();
				wWindow.Destroy();   // Is this close ?
			}
			
			if (!evArgs.Success)
			{
				string msg = "Operation Aborted \n";
				WarningDialog warning = new WarningDialog();
				warning.Message = msg + evArgs.ExceptionMsg;
         		warning.QuitOnOk = false;
            	warning.Present();
			}
			
			if (this.transferCompleteEventHandler != null)
			{
				transferCompleteEventHandler(sender, evArgs);
			}
		}
		
		public override void OnTransferCompleted(object sender, ThreadEventArgs e)
		{
			Application.Invoke(sender, (EventArgs)e, TransferCompleted);
		}

		
	}
}
