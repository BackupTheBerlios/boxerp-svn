
using System;
using System.Reflection;

namespace Boxerp.Client
{
	
	
	public class ThreadEventArgs : EventArgs
	{
		MethodBase methodBase;
		object returnValue;
		int threadId;
		
		public ThreadEventArgs(int t, MethodBase m, object o)
		{
		    methodBase = m;
		    returnValue = o;
		    threadId = t;
		}
		
		public MethodBase MethodBase
		{
		    get { return methodBase; }
		}
		
		public object ReturnValue
		{
		    get { return returnValue; }
		}
		
		public int ThreadId
		{
		    get { return threadId; }
		}
	}
}
