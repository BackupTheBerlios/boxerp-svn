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

namespace Boxerp.Client.WPF
{
	public class WpfResponsiveHelper : AbstractResponsiveHelper
	{
		WaitDialog waitDialog;
		Queue<WaitDialog> _dialogs = new Queue<WaitDialog>();
				
		public WpfResponsiveHelper(ConcurrencyMode mode) : base(mode){ }


		/// <summary>
		/// Create or manage a wait dialog and and invoke the base class. See more doc there
		/// </summary>
		/// <param name="transferType"></param>
		/// <param name="controller"></param>
		public override void StartAsyncCallList(ResponsiveEnum transferType, IController controller)
		{
			if ((_concurrencyMode == ConcurrencyMode.Modal) || (_concurrencyMode == ConcurrencyMode.Parallel)
				|| (RunningThreads == 0))
			{
				waitDialog = new WaitDialog();
				waitDialog.CancelEvent += OnCancel;
				_dialogs.Enqueue(waitDialog);
			}
			
			base.StartAsyncCallList(transferType, controller);

			if (_concurrencyMode == ConcurrencyMode.Modal)
			{
				waitDialog.ShowDialog();
			}
			else
			{
				waitDialog.Show();
				waitDialog.WindowState = WindowState.Normal;
			}
		}

		public override void StartAsyncCall(SimpleDelegate method)
		{
			if ((_concurrencyMode == ConcurrencyMode.Modal) || (_concurrencyMode == ConcurrencyMode.Parallel)
				|| (RunningThreads == 0))
			{
				waitDialog = new WaitDialog();
				waitDialog.CancelEvent += OnCancel;
				_dialogs.Enqueue(waitDialog);
			}

			base.StartAsyncCall(method);

			if (_concurrencyMode == ConcurrencyMode.Modal)
			{
				waitDialog.ShowDialog();
			}
			else
			{
				waitDialog.Show();
				waitDialog.WindowState = WindowState.Normal;
			}
		}

		public override void OnCancel(object sender, EventArgs e)
		{
			CancelRequested = true;	 

			
			// TODO: Ask the user before force abort
			MessageBox.Show("The process is being cancelled, it may take a while after you close this window");
			
			ForceAbort();

			//TODO: Show a dialog :" Please wait while cancelling" with
			//a button to force cancelation by aborting threads
		}

		private void TransferCompleted(object sender, ThreadEventArgs e)
		{
			ResponsiveEnum operationType = e.OperationType;
			WaitDialog wDialog = _dialogs.Dequeue();
			wDialog.Close();
			if (!e.Success)
			{
				string msg = "Operation Aborted \n";
				MessageBox.Show(msg + e.ExceptionMsg);
			}
			if (this.transferCompleteEventHandler != null)
			{
				transferCompleteEventHandler(sender, e);
			}
		}

		public override void OnTransferCompleted(object sender, ThreadEventArgs e)
		{
			waitDialog.Dispatcher.BeginInvoke(
				System.Windows.Threading.DispatcherPriority.Normal, 
				new ThreadEventHandler(TransferCompleted),
				sender,
				e
			);
		}
	}
}
