//
// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
//
// Copyright (C) 2005,2006 Shidix Technologies (www.shidix.com)
//
// Redistribution and use in source and binary forms, with or
// without modification, are permitted provided that the following
// conditions are met:
// Redistributions of source code must retain the above
// copyright notice, this list of conditions and the following
// disclaimer.
// Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following
// disclaimer in the documentation and/or other materials
// provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
// PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR
// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.

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
		Queue<QuestionDialog> _questionWindows = new Queue<QuestionDialog>();

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

			try
			{
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
			catch (System.Reflection.TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
			catch (Exception ex)
			{
				throw ex;
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

			try
			{
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
			catch (System.Reflection.TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		
        public override void OnCancel(object sender, EventArgs e)
		{
        	CancelRequested = true;
        	QuestionDialog qdialog = new QuestionDialog();
			qdialog.Modal = true;
        	qdialog.Message = "The process is being cancelled, please wait. Do you want to force abort right now?";
			_questionWindows.Enqueue(qdialog);
			if (RunningThreads > 0)
			{
				int rType = qdialog.Run();
				if ((rType == (int)ResponseType.Ok) && (RunningThreads > 0))
				{
					ForceAbort();
				}
				if (_questionWindows.Count > 0)
				{
					_questionWindows.Dequeue();
				}
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
			if (_questionWindows.Count > 0)
			{
				QuestionDialog qd = _questionWindows.Dequeue();
				qd.Hide();
				qd.Destroy();
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
