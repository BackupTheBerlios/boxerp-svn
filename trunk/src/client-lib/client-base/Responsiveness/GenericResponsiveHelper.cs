using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Boxerp.Client
{
	public abstract class GenericResponsiveHelper<T, TQuestion> : AbstractResponsiveHelper
		where T : IWaitControl, new()
		where TQuestion : IQuestionWindow, new()
	{
		protected T _waitDialog;
		protected bool _userWaitDialogInstance = false;
		protected bool _displayExceptions = true;
		protected Dictionary<Guid, T> _dialogs = new Dictionary<Guid, T>();
		protected Dictionary<int, TQuestion> _questionWindows = new Dictionary<int, TQuestion>();
		protected Dictionary<int, Guid> _storagePointers = new Dictionary<int, Guid>();
		

		public GenericResponsiveHelper(ConcurrencyMode mode) : this (mode, true, default(T)){ }

		public GenericResponsiveHelper(ConcurrencyMode mode, bool displayExceptions)
			: this(mode, true, default(T))
		{}

		/// <summary>
		/// You might want your custom waitDialog to manage the wait process. For example you might 
		/// want to have a status bar rather than a window. You can pass an instance of it in order
		/// for the application to use it
		/// </summary>
		/// <param name="mode"></param>
		/// <param name="displayExceptions">In case of exceptions during an async call, display them in a window or not</param>
		/// <param name="waitDialogInstance">An instance of your WaitDialog class</param>
		public GenericResponsiveHelper(ConcurrencyMode mode, bool displayExceptions, T waitDialogInstance)
			: base(mode)
		{
			_waitDialog = waitDialogInstance;
			_userWaitDialogInstance = _waitDialog == null ? false : true;
			_displayExceptions = displayExceptions;
		}

		public override Thread StartAsyncCall(SimpleDelegate method)
		{
			return StartAsyncCall(method, true);
		}

		public override System.Threading.Thread StartAsyncCall(SimpleDelegate method, bool showWaitControl)
		{
			Guid newDialogGuid = Guid.NewGuid();
			if ((_concurrencyMode == ConcurrencyMode.Modal) || (_concurrencyMode == ConcurrencyMode.Parallel)
				|| (RunningThreads == 0))
			{
				if (!_userWaitDialogInstance)
				{
					_waitDialog = new T();
					Console.Out.WriteLine("Creationg window:" + _waitDialog.GetHashCode());
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
				try
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
				catch (System.Reflection.TargetInvocationException ex)
				{
					throw ex.InnerException;
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}
			return thread;
		}

		public override List<Thread> StartAsyncCallList(ResponsiveEnum transferType, IController controller)
		{
			return StartAsyncCallList(transferType, controller, true);
		}

		public override List<System.Threading.Thread> StartAsyncCallList(ResponsiveEnum trType, IController controller, bool showWaitControl)
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
				threads = base.StartAsyncCallList(trType, controller);
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
				try
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
				catch (System.Reflection.TargetInvocationException ex)
				{
					throw ex.InnerException;
				}
				catch (Exception ex)
				{
					throw ex;
				}
			}

			return threads;
		}

		public override void OnCancel(object sender, EventArgs e)
		{
			IWaitControl senderDialog = sender as IWaitControl;
			CancelRequested = true;

			TQuestion win = new TQuestion();
			win.Msg = "Operation is being cancelled";
			win.AfirmativeOption = "Force abortion right now";
			win.NegativeOption = "Wait for the process to finish correctly";
			_questionWindows[senderDialog.AssociatedThreadId] = win;
			if (RunningThreads > 0)
			{
				win.ShowDialog();
				if ((win.IsAfirmative) && (RunningThreads > 0))
				{
					ForceAbort(senderDialog.AssociatedThreadId);
				}
				if (_questionWindows.Count > 0)
				{
					_questionWindows.Remove(senderDialog.AssociatedThreadId);
				}
			}
		}

		protected virtual void TransferCompleted(object sender, ThreadEventArgs e)
		{
			ResponsiveEnum operationType = e.OperationType;
			T wDialog = default(T);
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
			if (_questionWindows.Count > 0)
			{
				_questionWindows[e.ThreadId].Close();
				_questionWindows.Remove(e.ThreadId);
			}

			if ((_displayExceptions) && (!e.Success))
			{
				string msg = "Operation Aborted \n";
				if ((e.ExceptionMsg != null) && (e.ExceptionMsg.Length > 0))
				{
					string m = string.Format("{0}{1}", msg, e.ExceptionMsg);
					showException(m);
				}
				else
				{
					showMessage(msg);
				}
			}
			if (this.transferCompleteEventHandler != null)
			{
				transferCompleteEventHandler(sender, e);
			}
		}

		protected abstract void showMessage(string msg);

		protected abstract void showException(string msg);

	}
}
