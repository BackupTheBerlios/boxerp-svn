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
	public class GtkResponsiveHelper : GtkResponsiveHelper<WaitDialog, WaitWindow>
	{
		public GtkResponsiveHelper(ConcurrencyMode mode) : this(mode, true) { }

		public GtkResponsiveHelper(ConcurrencyMode mode, bool displayExceptions)
			: base(mode, displayExceptions)	{}
	}

	public class GtkResponsiveHelper<T, Y> : AbstractResponsiveHelper
		where T : class, IWaitControl, new()
		where Y : class, IWaitControl, new()
	{
		private T _waitDialog;
		private Y _waitWindow;
		
		private bool _userWaitDialogInstance = false;
		private bool _userWaitWindowInstance = false;
		private bool _displayExceptions = true;
		private Queue<T> _dialogs = new Queue<T>();
		private Queue<Y> _windows = new Queue<Y>();
		private Queue<QuestionDialog> _questionWindows = new Queue<QuestionDialog>();

		public GtkResponsiveHelper(ConcurrencyMode mode)
			: this(mode, true, null)
		{
		}

		public GtkResponsiveHelper(ConcurrencyMode mode, bool displayExceptions)
			: this(mode, displayExceptions, null)
		{
		}

		public GtkResponsiveHelper(ConcurrencyMode mode, bool displayExceptions, T waitDialogInstance, Y waitWindowInstance)
			: base(mode)
		{
			_waitDialog = waitDialogInstance;
			_waitWindow = waitWindowInstance;
			_userWaitDialogInstance = _waitDialog == null ? false : true;
			_userWaitWindowInstance = _waitWindow == null ? false : true;
			_displayExceptions = displayExceptions;
		}

		public override void StartAsyncCallList(ResponsiveEnum transferType, IController controller)
		{
			StartAsyncCallList(transferType, controller, true);
		}
		
		public override void StartAsyncCallList(ResponsiveEnum transferType, IController controller, bool showWaitDialog)
		{
			if ((_concurrencyMode == ConcurrencyMode.Modal) || (_concurrencyMode == ConcurrencyMode.Parallel)
				|| (RunningThreads == 0))
			{
				if (_concurrencyMode == ConcurrencyMode.Modal)
				{
					if (!_userWaitDialogInstance)
					{
						_waitDialog = new WaitDialog();
						_dialogs.Enqueue(_waitDialog);
					}
					_waitDialog.CancelEvent += OnCancel;
				}
				else
				{
					if (!_userWaitWindowInstance)
					{
						_waitWindow = new WaitWindow();
						_windows.Enqueue(_waitWindow);
					}
					_waitWindow.Modal = false;
					_waitWindow.CancelEvent += OnCancel;
				}
			}

			base.StartAsyncCallList(transferType, controller);

			try
			{
				if (showWaitDialog)
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
			StartAsyncCall(method, true);	
		}
		
		public override void StartAsyncCall(SimpleDelegate method, bool showWaitDialog)
		{
			
			if ((_concurrencyMode == ConcurrencyMode.Modal) || (_concurrencyMode == ConcurrencyMode.Parallel)
				|| (RunningThreads == 0))
			{
				if (_concurrencyMode == ConcurrencyMode.Modal)
				{
					if (!_userWaitDialogInstance)
					{
						_waitDialog = new WaitDialog();
						_dialogs.Enqueue(_waitDialog);
					}
					_waitDialog.CancelEvent += OnCancel;
				}
				else
				{
					if (!_userWaitWindowInstance)
					{
						_waitWindow = new WaitWindow();
						_windows.Enqueue(_waitWindow);
					}
					_waitWindow.Modal = false;
					_waitWindow.CancelEvent += OnCancel;
				}
			}

			base.StartAsyncCall(method);

			try
			{
				if (showWaitDialog)
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
			ThreadEventArgs evArgs = (ThreadEventArgs)e;
			if (_concurrencyMode == ConcurrencyMode.Modal)
			{
				WaitDialog wDialog;
				if (!_userWaitDialogInstance)
				{
					wDialog = _dialogs.Dequeue();
					wDialog.Stop();
					wDialog.Hide();
					wDialog.Destroy();
				}
				else
				{
					wDialog = _waitDialog;
					wDialog.Stop();
					wDialog.Hide();
				}
			}
			else
			{
				WaitWindow wWindow;
				if (!_userWaitWindowInstance)
				{
					wWindow = _windows.Dequeue();
					wWindow.Stop();
					wWindow.Hide();
					wWindow.Destroy();   
				}
				else
				{
					wWindow = _waitWindow;
					wWindow.Stop();
					wWindow.Hide();
				}
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
