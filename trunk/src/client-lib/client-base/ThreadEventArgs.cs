
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
