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
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using Boxerp.Client;

namespace Boxerp.Client.WindowsForms
{

	public class WinFormsResponsiveHelper : WinFormsResponsiveHelper<WaitDialog>
	{
		public WinFormsResponsiveHelper(ConcurrencyMode mode)
			: base(mode)
		{
		}
	}

    /// <summary>
    /// This class helps to keep winform applications responsive 
    /// </summary>
    public class WinFormsResponsiveHelper<T> : AbstractResponsiveHelper
		where T : class, IWinFormsWaitControl, new()
    {
		private T _waitDialog;

		private bool _userWaitDialogInstance = false;
        private Dictionary<int, QuestionDialog> _questionWindows = new Dictionary<int, QuestionDialog>();
        private Dictionary<Guid, T> _dialogs = new Dictionary<Guid, T>();
        private Dictionary<int, Guid> _storagePointers = new Dictionary<int, Guid>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        public WinFormsResponsiveHelper(ConcurrencyMode mode)
            : base(mode)
        {
        }


		/// <summary>
		/// You might want your custom waitDialog to manage the wait process. For example you might 
		/// want to have a status bar rather than a window. You can pass an instance of it in order
		/// for the application to use it
		/// </summary>
		/// <param name="mode"></param>
		/// <param name="waitDialogInstance">An instance of your WaitDialog class</param>
		public WinFormsResponsiveHelper(ConcurrencyMode mode, T waitDialogInstance)
			: base(mode)
		{
			_waitDialog = waitDialogInstance;
			_userWaitDialogInstance = _waitDialog == null ? false : true;
		}

		public override List<Thread> StartAsyncCallList(ResponsiveEnum trType, IController controller)
		{
			return StartAsyncCallList(trType, controller, true);
		}

        public override List<Thread> StartAsyncCallList(ResponsiveEnum trType, IController controller, bool showWaitControl)
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
            lock(_storagePointers)
            {

			    threads = base.StartAsyncCallList(trType, controller);
                try
                {
                    // if the thread finishes between these lines of code, we get an exception
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
                catch (Exception)
                {
                    throw;
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
                    Console.Out.WriteLine("Creating window: " + _waitDialog.GetHashCode());
                    _dialogs[newDialogGuid] = _waitDialog;
				}
				_waitDialog.CancelEvent += OnCancel;
				
            }

            int threadId = -1;
			Thread thread;
            // before adding the dialog to the dialogs storage, we have to make sure the thread lasts 
            // enough. if it is over, then we don't add it to the dialogs nor show the dialog
            lock(_storagePointers) {
                thread  = base.StartAsyncCall(method);
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


        public override void OnCancel(object sender, EventArgs e)
        {
            IWinFormsWaitControl senderDialog = sender as IWinFormsWaitControl;
            CancelRequested = true;

            QuestionDialog ques = new QuestionDialog(true);
            ques.Message = "The process is being cancelled, please wait.  Do you want to force abort right now?";
            _questionWindows[senderDialog.AssociatedThreadId] = ques;
            if (RunningThreads > 0)
            {
                ResponseType resp = ques.Run();
                if ((resp == ResponseType.Ok) && (RunningThreads > 0))
                {
                    ForceAbort(senderDialog.AssociatedThreadId);
                }
                if (_questionWindows.Count > 0)
                {
                    _questionWindows.Remove(senderDialog.AssociatedThreadId);
                }
            }
        }

        private void TransferCompleted(object sender, ThreadEventArgs e)
        {
            ThreadEventArgs evArgs = (ThreadEventArgs)e;
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
                    //the thread died before proper intialization
                    Guid dialogGuid = Guid.NewGuid();
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
                            //just take whatever dialog that was not intialized properly and close.
                            wDialog = _dialogs[guid];
                            dialogGuid = guid;
                            break;
                        }
                        else
                        {
                            throw new NotSupportedException("This is a bug in Boxerp responsiveness engine, this should not happend ever");
                        }
                    }
                    _dialogs.Remove(dialogGuid);
                }
			}
			else
			{
				wDialog = _waitDialog;
				wDialog.CancelEvent -= OnCancel;
			}

            Console.Out.WriteLine("closing teh stuffs");
            wDialog.Invoke(new MethodInvoker(wDialog.CloseControl));
            
			if (_questionWindows.Count > 0)
            {
                QuestionDialog qdia = _questionWindows[e.ThreadId]; //todo: check this
                qdia.Invoke(new MethodInvoker(qdia.Close));
                _questionWindows.Remove(e.ThreadId);
            }

            if (!evArgs.Success)
            {
                string msg = "Operation Aborted \n";
				string fullMsg;
				if ((evArgs.ExceptionMsg != null) && (evArgs.ExceptionMsg.Length > 0))
				{
					fullMsg = string.Format("{0}{1}", msg, evArgs.ExceptionMsg);
				}
				else
				{
					fullMsg = msg;
				}

                MessageBox.Show(fullMsg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            if (this.transferCompleteEventHandler != null)
            {
                transferCompleteEventHandler(sender, evArgs);
            }
        }

        public override void OnTransferCompleted(object sender, ThreadEventArgs e)
        {
			if (_waitDialog != null)
			{
				try
				{
					_waitDialog.BeginInvoke(new ThreadEventHandler(TransferCompleted), new object[] { sender, e });
				}
				catch (InvalidOperationException ex)
				{
					if (ex.Message.Contains("Invoke or BeginInvoke cannot be called on a control until the window handle has been created."))
					{
						Console.Out.WriteLine("The thread finished before the wait window was open. Close this dirty hack ASAP");
					}
				}
			}
		}
    }
}
