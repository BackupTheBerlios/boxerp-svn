
/*
http://www.issociate.de/board/index.php?t=msg&goto=494323&rid=0
*/

using System;
using System.Reflection;

namespace Boxerp.Client
{
	
	
	public class SimpleInvoker
	{
		MethodInfo _method = null;
		Object[] _params = null;
		SimpleDelegate _sdelegate = null;
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
		
		public SimpleInvoker(SimpleDelegate method)
		{
		    _sdelegate = method;
		}
		
		public void Invoke()
		{
			#if REMOTING
					UserInformation.SetSessionToken(SessionSingleton.GetInstance().GetSession());
			#endif
			if (_method != null)
			{
			    _method.Invoke(_caller, _params);	// _params is always null, so far
			}
			else if (_sdelegate != null)
			{
			    _sdelegate();
			}
			   
		}
	}
}
