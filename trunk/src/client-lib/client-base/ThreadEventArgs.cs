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

namespace Boxerp.Client
{
	
	
	public class ThreadEventArgs : EventArgs
	{
		MethodBase _methodBase;
		SimpleDelegate _method;
		object _returnValue;
		int _threadId;
		bool _success;
		ResponsiveEnum _operationType;
		string _exceptionMsg;

		public ThreadEventArgs(int t, MethodBase m, object o)
		{
		    _methodBase = m;
		    _returnValue = o;
		    _threadId = t;
		}

		public ThreadEventArgs(int t, SimpleDelegate  m, object o)
		{
			_method = m;
			_returnValue = o;
			_threadId = t;
		}
		
		public MethodBase MethodBase
		{
		    get { return _methodBase; }
		}
		
		public object ReturnValue
		{
		    get { return _returnValue; }
		}

		public string ExceptionMsg
		{
			get { return _exceptionMsg; }
			set { _exceptionMsg = value; }
		}

		public int ThreadId
		{
		    get { return _threadId; }
		}

		public SimpleDelegate Method
		{
			get
			{
				return _method;
			}
		}

		public bool Success
		{
			get
			{
				return _success;
			}
			set
			{
				_success = value;
			}
		}

		public ResponsiveEnum OperationType
		{
			get
			{
				return _operationType;
			}
			set
			{
				_operationType = value;
			}
		}

	}
}
