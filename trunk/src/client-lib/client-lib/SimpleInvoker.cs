
/*
http://www.issociate.de/board/index.php?t=msg&goto=494323&rid=0
*/

using System;
using System.Reflection;

namespace Boxerp.Client
{
	
	
	public class SimpleInvoker
	{
		MethodInfo _method;
		Object _caller;
		
		private void CheckMethodSignature ()
		{
			if ((_method.GetParameters().Length != 0) || (_method.ReturnType != typeof(void)))
				throw new System.NotSupportedException("Invalid responsive method signature");
		}
		
		public SimpleInvoker(MethodInfo method, Object caller)
		{	
			_method = method;
			_caller = caller;
			CheckMethodSignature();
		}
		
		public void Invoke()
		{
			#if REMOTING
					Console.WriteLine("remotiniiiiiiiiiiii");
					UserInformation.SetSessionToken(SessionSingleton.GetInstance().GetSession());
			#endif
			_method.Invoke(_caller, null);
		}
	}
}
