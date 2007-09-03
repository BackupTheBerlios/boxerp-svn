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

namespace Boxerp.Client.WindowsForms
{

    /// <summary>
    /// This class helps to keep winform applications responsive 
    /// </summary>
    public abstract class WinFormsResponsiveHelper : AbstractResponsiveHelper
    {
        WaitDialog _waitDialog;
        WaitDialog _waitWindow;
        Queue<WaitDialog> _dialogs = new Queue<WaitDialog>();
        Queue<WaitDialog> _windows = new Queue<WaitDialog>();
        Queue<MessageBox> _questionWindows = new Queue<MessageBox>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mode"></param>
        public WinFormsResponsiveHelper(ConcurrencyMode mode)
            : base(mode)
        {
        }

        public override void StartAsyncCallList(ResponsiveEnum trType, IController controller)
        {
            if ((_concurrencyMode == ConcurrencyMode.Modal) || (_concurrencyMode == ConcurrencyMode.Parallel)
                || (RunningThreads == 0))
            {
                if (_concurrencyMode == ConcurrencyMode.Modal)
                {
                    _waitDialog = new WaitDialog(true);
                    _waitDialog.CancelEvent += OnCancel;
                    _dialogs.Enqueue(_waitDialog);
                }
                else
                {
                    _waitWindow = new WaitDialog(false);
                    _waitWindow.CancelEvent += OnCancel;
                    _windows.Enqueue(_waitWindow);
                }

                base.StartAsyncCallList(trType, controller);

                try
                {
                    if (_concurrencyMode == ConcurrencyMode.Modal)
                    {
                        _waitDialog.Show();
                    }
                    else
                    {
                        _waitWindow.Show();
                    }
                }

                catch (System.Reflection.TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }


        public override void OnCancel(object sender, EventArgs e)
        {
            CancelRequested = true;

        }

        public override void OnTransferCompleted(object sender, ThreadEventArgs e)
        {
            throw new Exception("The method or operation is not implemented.");

            
        }
    }
}
