//
// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
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

using Boxerp.Client;

namespace Boxerp.Client.WPF
{

	public class WpfResponsiveHelper : WpfResponsiveHelper<WaitDialog> 
	{
		public WpfResponsiveHelper(ConcurrencyMode mode) : this(mode, true) { }

		public WpfResponsiveHelper(ConcurrencyMode mode, bool displayExceptions)
			: base(mode)
		{
			_displayExceptions = displayExceptions;
		}
	}

	public class WpfResponsiveHelper<T> : AbstractResponsiveHelper
		where T : IWpfWaitControl, new()
	{
		T _waitDialog;
		
		protected bool _displayExceptions = true;
		Queue<T> _dialogs = new Queue<T>();
		Queue<QuestionWindow> _questionWindows = new Queue<QuestionWindow>();
				
		public WpfResponsiveHelper(ConcurrencyMode mode) : this (mode, true){ }

		public WpfResponsiveHelper(ConcurrencyMode mode, bool displayExceptions)
			: base(mode)
		{
			_displayExceptions = displayExceptions;
		}

		public override void StartAsyncCallList(ResponsiveEnum transferType, IController controller)
		{
			StartAsyncCallList(transferType, controller, true);
		}

		/// <summary>
		/// Create or manage a wait dialog and and invoke the base class. See more doc there
		/// </summary>
		/// <param name="transferType"></param>
		/// <param name="controller"></param>
		public override void StartAsyncCallList(ResponsiveEnum transferType, IController controller, bool showWaitControl)
		{
			if ((_concurrencyMode == ConcurrencyMode.Modal) || (_concurrencyMode == ConcurrencyMode.Parallel)
				|| (RunningThreads == 0))
			{
				_waitDialog = new T();
				_waitDialog.CancelEvent += OnCancel;
				_dialogs.Enqueue(_waitDialog);
			}
			
			base.StartAsyncCallList(transferType, controller);

			if (showWaitControl)
			{
				try
				{
					_waitDialog.IsModal = _concurrencyMode == ConcurrencyMode.Modal;
					_waitDialog.ShowControl();
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
		}

		public override void StartAsyncCall(SimpleDelegate method)
		{
			StartAsyncCall(method, true);
		}

		public override void StartAsyncCall(SimpleDelegate method, bool showWaitControl)
		{
			if ((_concurrencyMode == ConcurrencyMode.Modal) || (_concurrencyMode == ConcurrencyMode.Parallel)
				|| (RunningThreads == 0))
			{
					_waitDialog = new T();
					_waitDialog.CancelEvent += OnCancel;
					_dialogs.Enqueue(_waitDialog);
			}

			base.StartAsyncCall(method);

			if (showWaitControl)
			{
				try
				{
					_waitDialog.IsModal = _concurrencyMode == ConcurrencyMode.Modal;
					_waitDialog.ShowControl();
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
		}

		public override void OnCancel(object sender, EventArgs e)
		{
			CancelRequested = true;

			QuestionWindow win = new QuestionWindow();
			win.Msg = "Operation is being cancelled";
			win.YesButtonLabel = "Force abortion right now";
			win.NoButtonLabel = "Wait for the process to finish correctly";
			_questionWindows.Enqueue(win);
			if (RunningThreads > 0)
			{
				win.ShowDialog();
				if ((win.Yes) && (RunningThreads > 0))
				{
					ForceAbort();
				}
				if (_questionWindows.Count > 0)
				{
					_questionWindows.Dequeue();
				}
			}
		}

		private void TransferCompleted(object sender, ThreadEventArgs e)
		{
			ResponsiveEnum operationType = e.OperationType;
			T wDialog = _dialogs.Dequeue();
			wDialog.CloseControl();
			if (_questionWindows.Count > 0)
			{
				_questionWindows.Dequeue().Close();
			}

			if ((_displayExceptions) && (!e.Success))
			{
				string msg = "Operation Aborted \n";
				if ((e.ExceptionMsg != null) && (e.ExceptionMsg.Length > 0))
				{
					ErrorWindow win = new ErrorWindow();
					win.Msg = string.Format("{0}{1}", msg, e.ExceptionMsg);
					win.ShowDialog();
				}
				else
				{
					MessageBox.Show(msg);
				}
			}
			if (this.transferCompleteEventHandler != null)
			{
				transferCompleteEventHandler(sender, e);
			}
		}

		public override void OnTransferCompleted(object sender, ThreadEventArgs e)
		{
			_waitDialog.Dispatcher.BeginInvoke(
				System.Windows.Threading.DispatcherPriority.Normal, 
				new ThreadEventHandler(TransferCompleted),
				sender,
				e
			);
		}
	}
}
