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
	/// <summary>
	/// Windows.Forms Responsive Helper. Using this class the Winforms WaitDialog will be used by
	/// default to let the user see the progress and cancel the operation
	/// </summary>
	public class WinFormsResponsiveHelper : WinFormsResponsiveHelper<WaitDialog>
	{
		public WinFormsResponsiveHelper(ConcurrencyMode mode)
			: base(mode)
		{
		}
	}

	/// <summary>
	/// Windows.Forms Responsive Helper. This class allows you to specify a custom IWinFormsWaitControl
	/// so that you can use a status bar, or custom dialog.
	/// </summary>
    public class WinFormsResponsiveHelper<T> : GenericResponsiveHelper<T, QuestionDialog>
		where T : class, IWinFormsWaitControl, new()
    {
		
        public WinFormsResponsiveHelper(ConcurrencyMode mode)
            : base(mode)
        {
        }

		public WinFormsResponsiveHelper(ConcurrencyMode mode, T waitDialogInstance)
			: base(mode)
		{
		}

		protected override void TransferCompleted(object sender, ThreadEventArgs e)
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
			T wDialog = GetDialog(e.ThreadId);
			if (wDialog != null)
			{
				if (wDialog.IsBeingDisplayed)
				{
					if (typeof(Form).IsAssignableFrom(typeof(T)))
					{
						Form wDialogForm = wDialog as Form;
						if (wDialogForm.IsHandleCreated)
						{
							wDialog.BeginInvoke(new ThreadEventHandler(TransferCompleted), new object[] { sender, e });
						}
					}
					else
					{
						wDialog.BeginInvoke(new ThreadEventHandler(TransferCompleted), new object[] { sender, e });
					}
				}
			}
		}

		protected override void showException(string msg)
		{
			MessageBox.Show(msg);
		}

		protected override void showMessage(string msg)
		{
			MessageBox.Show(msg);
		}

		public override void CallUIfromAsyncThread(SimpleDelegate anonymousMethod)
		{
			if (_waitDialog.IsBeingDisplayed)
			{
				_waitDialog.Invoke(anonymousMethod, new object[0]);
			}
		}
    }
}
