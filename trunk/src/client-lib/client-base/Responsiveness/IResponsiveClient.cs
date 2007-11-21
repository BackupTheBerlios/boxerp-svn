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
using System.Reflection;
using System.Collections.Generic;
using System.Threading;

namespace Boxerp.Client
{
	
	public interface IResponsiveClient
	{
		/* Implemented in the abstract class */
		List<Thread> StartAsyncCallList(ResponsiveEnum trType, IController controller);
		void StopAsyncMethod(int threadId, MethodBase MethodBase, object output);
		void StopAsyncMethod(int threadId, SimpleDelegate method, object output);
		Thread StartAsyncCall(SimpleDelegate method);
		void OnAsyncException(Exception ex);
		void OnAbortAsyncCall(Exception ex);
		bool CancelRequested { get; }

		/* Not implemented in the abstract class */
		void OnCancel(object sender, EventArgs e);
		void OnTransferCompleted(object sender, ThreadEventArgs e);
		event ThreadEventHandler TransferCompleteEvent;
	}
}
