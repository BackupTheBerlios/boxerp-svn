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

	public class WpfResponsiveHelper<T> : GenericResponsiveHelper<T, QuestionWindow>
		where T : class, IWpfWaitControl, new()
	{
		public WpfResponsiveHelper(ConcurrencyMode mode) : this (mode, true, null){ }

		public WpfResponsiveHelper(ConcurrencyMode mode, bool displayExceptions)
			: this(mode, true, null)
		{}

		public WpfResponsiveHelper(ConcurrencyMode mode, bool displayExceptions, T waitDialogInstance)
			: base(mode)
		{}

		public override void OnTransferCompleted(object sender, ThreadEventArgs e)
		{
			Console.Out.WriteLine("thread completed:" + Thread.CurrentThread.ManagedThreadId);
			
			_waitDialog.Dispatcher.BeginInvoke(
				System.Windows.Threading.DispatcherPriority.Normal, 
				new ThreadEventHandler(TransferCompleted),
				sender,
				e
			);
		}

		protected override void showException(string msg)
		{
			ErrorWindow win = new ErrorWindow();
			win.Msg = msg;
			win.ShowDialog();
		}

		protected override void showMessage(string msg)
		{
			MessageBox.Show(msg);
		}

		public override void CallUIfromAsyncThread(SimpleDelegate anonymousMethod)
		{
			_waitDialog.Dispatcher.Invoke(
				System.Windows.Threading.DispatcherPriority.Normal,
				anonymousMethod);
		}
	}
}
