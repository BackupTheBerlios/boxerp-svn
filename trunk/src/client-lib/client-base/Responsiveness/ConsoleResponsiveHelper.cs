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

using Boxerp.Client;

namespace Boxerp.Client
{
	public class ConsoleResponsiveHelper : ConsoleResponsiveHelper<ConsoleWaitDialog>
	{
		public ConsoleResponsiveHelper(ConcurrencyMode mode) : this(mode, true) { }

		public ConsoleResponsiveHelper(ConcurrencyMode mode, bool displayExceptions)
			: base(mode)
		{
			_displayExceptions = displayExceptions;
		}
	}

	public class ConsoleResponsiveHelper<T> : AbstractResponsiveHelper
		where T : class, IWaitControl, new()
	{
		T _waitDialog;

		protected bool _userWaitDialogInstance = false;
		protected bool _displayExceptions = true;
		Dictionary<Guid, T> _dialogs = new Dictionary<Guid, T>();
		Dictionary<int, Guid> _storagePointers = new Dictionary<int, Guid>();

		public ConsoleResponsiveHelper(ConcurrencyMode mode) : this(mode, true, null) { }

		public ConsoleResponsiveHelper(ConcurrencyMode mode, bool displayExceptions)
			: this(mode, true, null)
		{ }

		/// <summary>
		/// You might want your custom waitDialog to manage the wait process. For example you might 
		/// want to have a status bar rather than a window. You can pass an instance of it in order
		/// for the application to use it
		/// </summary>
		/// <param name="mode"></param>
		/// <param name="displayExceptions">In case of exceptions during an async call, display them in a window or not</param>
		/// <param name="waitDialogInstance">An instance of your WaitDialog class</param>
		public ConsoleResponsiveHelper(ConcurrencyMode mode, bool displayExceptions, T waitDialogInstance)
			: base(mode)
		{
			_waitDialog = waitDialogInstance;
			_userWaitDialogInstance = _waitDialog == null ? false : true;
			_displayExceptions = displayExceptions;
		}

		public override List<Thread> StartAsyncCallList(ResponsiveEnum transferType, IController controller)
		{
			return StartAsyncCallList(transferType, controller, true);
		}

		/// <summary>
		/// Create or manage a wait dialog and and invoke the base class. See more doc there
		/// </summary>
		/// <param name="transferType"></param>
		/// <param name="controller"></param>
		public override List<Thread> StartAsyncCallList(ResponsiveEnum transferType, IController controller, bool showWaitControl)
		{
			Guid newDialogGuid = Guid.NewGuid();
			if ((_concurrencyMode == ConcurrencyMode.Modal) || (_concurrencyMode == ConcurrencyMode.Parallel)
				|| (RunningThreads == 0))
			{
				if (!_userWaitDialogInstance)
				{
					_waitDialog = new T();
					_dialogs[newDialogGuid] = _waitDialog;
				}
				_waitDialog.CancelEvent += OnCancel;

			}
			int threadId = -1;
			List<Thread> threads;
			// before adding the dialog to the dialogs storage, we have to make sure the thread lasts 
			// enough. if it is over, then we don't add it to the dialogs nor show the dialog
			lock (_storagePointers)
			{
				threads = base.StartAsyncCallList(transferType, controller);
				try
				{
					// if the thread finishes between this lines of code, we get an exception
					threadId = threads[0].ManagedThreadId;
					_storagePointers[threadId] = newDialogGuid;
				}
				catch
				{
					// and the dialog should be removed
					_dialogs.Remove(newDialogGuid);
				}
			}
			if (showWaitControl)
			{	
				_waitDialog.IsModal = _concurrencyMode == ConcurrencyMode.Modal;
				lock (_dialogs)
				{
					// the Transfer completed method removes the dialog from the dialogs so 
					// it is important to check it before opening the window
					if (_dialogs.ContainsKey(newDialogGuid))
					{
						_waitDialog.AssociatedThreadId = threadId;
						_waitDialog.ShowControl();
					}
				}	
			}

			return threads;
		}



		public override Thread StartAsyncCall(SimpleDelegate method)
		{
			return StartAsyncCall(method, true);
		}

		public override Thread StartAsyncCall(SimpleDelegate method, bool showWaitControl)
		{
			Guid newDialogGuid = Guid.NewGuid();
			if ((_concurrencyMode == ConcurrencyMode.Modal) || (_concurrencyMode == ConcurrencyMode.Parallel)
				|| (RunningThreads == 0))
			{
				if (!_userWaitDialogInstance)
				{
					_waitDialog = new T();
					_dialogs[newDialogGuid] = _waitDialog;
				}
				_waitDialog.CancelEvent += OnCancel;
			}

			int threadId = -1;
			Thread thread;
			// before adding the dialog to the dialogs storage, we have to make sure the thread lasts 
			// enough. if it is over, then we don't add it to the dialogs nor show the dialog
			lock (_storagePointers)
			{
				thread = base.StartAsyncCall(method);
				try
				{
					// if the thread finishes between this lines of code, we get an exception
					threadId = thread.ManagedThreadId;
					_storagePointers[threadId] = newDialogGuid;
				}
				catch
				{
					// and the dialog should be removed
					_dialogs.Remove(newDialogGuid);
				}
			}
			if (showWaitControl)
			{	
				_waitDialog.IsModal = _concurrencyMode == ConcurrencyMode.Modal;
				lock (_dialogs)
				{
					// the Transfer completed method removes the dialog from the dialogs so 
					// it is important to check it before opening the window
					if (_dialogs.ContainsKey(newDialogGuid))
					{
						_waitDialog.AssociatedThreadId = threadId;
						_waitDialog.ShowControl();
					}
				}	
			}
			return thread;
		}

		public override void OnCancel(object sender, EventArgs e)
		{
			IWaitControl senderDialog = sender as IWaitControl;
			CancelRequested = true;

			if (RunningThreads > 0)
			{
				ForceAbort(senderDialog.AssociatedThreadId);
			}
		}

		private void TransferCompleted(object sender, ThreadEventArgs e)
		{
			ResponsiveEnum operationType = e.OperationType;
			T wDialog = null;
			if (!_userWaitDialogInstance)
			{
				int id = e.ThreadId;
				if (_storagePointers.ContainsKey(id))
				{
					wDialog = _dialogs[_storagePointers[id]];
					_dialogs.Remove(_storagePointers[id]);
					_storagePointers.Remove(id);
				}
				else
				{
					// the thread died before proper initialization:
					Guid dialogGui = Guid.NewGuid();
					foreach (Guid guid in _dialogs.Keys)
					{
						bool isInStorage = false;
						foreach (Guid storedGuid in _storagePointers.Values)
						{
							if (guid == storedGuid)
							{
								isInStorage = true;
								break;
							}
						}
						if (!isInStorage)
						{
							// just take whatever dialog that wasn't initialized properly and close.
							wDialog = _dialogs[guid];
							dialogGui = guid;
							break;
						}
						else
						{
							throw new NotSupportedException("This is a bug in Boxerp responsiveness engine, this should not happend ever");
						}

					}
					_dialogs.Remove(dialogGui);
				}
			}
			else
			{
				wDialog = _waitDialog;
				wDialog.CancelEvent -= OnCancel;
			}
			wDialog.CloseControl();
			
			if ((_displayExceptions) && (!e.Success))
			{
				string msg = "Operation Aborted \n";
				if ((e.ExceptionMsg != null) && (e.ExceptionMsg.Length > 0))
				{
					Console.Out.WriteLine(string.Format("{0}{1}", msg, e.ExceptionMsg));
				}
				else
				{
					Console.Out.WriteLine(msg);
				}
			}
			if (this.transferCompleteEventHandler != null)
			{
				transferCompleteEventHandler(sender, e);
			}
		}

		public override void OnTransferCompleted(object sender, ThreadEventArgs e)
		{
			Console.Out.WriteLine("thread completed:" + Thread.CurrentThread.ManagedThreadId);
			TransferCompleted(sender, e);
		}

		public override void CallUIfromAsyncThread(SimpleDelegate anonymousMethod)
		{
			anonymousMethod.Invoke();
		}

		public override void UpdateWaitMessage(string msg)
		{
			Console.Out.WriteLine(msg);
		}
	}
}
