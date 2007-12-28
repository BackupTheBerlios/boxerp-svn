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
	public class GtkResponsiveHelper : GtkResponsiveHelper<WaitControl>
	{
		public GtkResponsiveHelper(ConcurrencyMode mode) : this(mode, true) { }

		public GtkResponsiveHelper(ConcurrencyMode mode, bool displayExceptions)
			: base(mode, displayExceptions)	{}
	}

	public class GtkResponsiveHelper<T> : GenericResponsiveHelper<T, QuestionDialog>
		where T : class, IWaitControl, new()
	{
		public GtkResponsiveHelper(ConcurrencyMode mode) : this (mode, true, null){ }

		public GtkResponsiveHelper(ConcurrencyMode mode, bool displayExceptions)
			: this(mode, true, null)
		{}

		public GtkResponsiveHelper(ConcurrencyMode mode, bool displayExceptions, T waitDialogInstance)
			: base(mode)
		{}

		public override List<Thread> StartAsyncCallList(ResponsiveEnum transferType, IController controller)
		{
			return StartAsyncCallList(transferType, controller, true);
		}
		
		public override void OnTransferCompleted(object sender, ThreadEventArgs e)
		{
			Application.Invoke(sender, (EventArgs)e, gtkTransferCompleted);
		}
		
		protected void gtkTransferCompleted(object sender, EventArgs e)
		{
			base.TransferCompleted(sender, (ThreadEventArgs) e);
		}
		
		protected override void showException(string msg)
		{
			WarningDialog win = new WarningDialog();
			win.Message = msg;
			win.Run();
		}

		protected override void showMessage(string msg)
		{
			InfoDialog win = new InfoDialog();
			win.Message = msg;
			win.Run();
		}

		public override void CallUIfromAsyncThread(SimpleDelegate anonymousMethod)
		{
			Application.Invoke(
			           (
			                 delegate(object sender, EventArgs args)
			                 {
				                    anonymousMethod.Invoke();
			                 }
			           ));
		}
	}
}
