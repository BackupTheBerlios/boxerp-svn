
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
		
		public GtkResponsiveHelper(ConcurrencyMode mode)
			: base(mode)
		{
		}
		
		public override void StartAsyncCallList(ResponsiveEnum transferType, IController controller)
		{
			if ((_concurrencyMode == ConcurrencyMode.Modal) || (_concurrencyMode == ConcurrencyMode.Parallel)
				|| (RunningThreads == 0))
			{
				if (_concurrencyMode == ConcurrencyMode.Modal)
				{
					_waitDialog = new WaitDialog();
					_waitDialog.CancelEvent += OnCancel;
					_dialogs.Enqueue(_waitDialog);
				}
				else
				{
					_waitWindow = new WaitWindow();
					_waitWindow.Modal = false;
					_waitWindow.CancelEvent += OnCancel;
					_windows.Enqueue(_waitWindow);
				}
			}
			
			base.StartAsyncCallList(transferType, controller);

			if (_concurrencyMode == ConcurrencyMode.Modal)
			{
				_waitDialog.Run();
			}
			else
			{
				_waitWindow.ShowAll();
				_waitWindow.Present();
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
					_waitDialog = new WaitDialog();
					_waitDialog.CancelEvent += OnCancel;
					_dialogs.Enqueue(_waitDialog);
				}
				else
				{
					_waitWindow = new WaitWindow();
					_waitWindow.Modal = false;
					_waitWindow.CancelEvent += OnCancel;
					_windows.Enqueue(_waitWindow);
				}
			}
			
			base.StartAsyncCall(method);

			if (_concurrencyMode == ConcurrencyMode.Modal)
			{
				_waitDialog.Run();
			}
			else
			{
				_waitWindow.ShowAll();
				_waitWindow.Present();
				// TODO : if the window is minimized show it in the middle of the screen
			}
		}
		
        public override void OnCancel(object sender, EventArgs e)
		{
        	CancelRequested = true;
        	QuestionDialog qdialog = new QuestionDialog();
			qdialog.Modal = true;
        	qdialog.Message = "The process is being cancelled, please wait. Do you want to force abort right now?";
            int rType = qdialog.Run();
			if (rType == (int)ResponseType.Ok)
			{
			    ForceAbort();      
            }
        }
		
		private void TransferCompleted(object sender, EventArgs e)
		{
			ThreadEventArgs evArgs = (ThreadEventArgs) e;
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
