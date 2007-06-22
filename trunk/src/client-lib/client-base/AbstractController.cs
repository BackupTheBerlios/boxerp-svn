using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Boxerp.Client
{
	/// <summary>
	/// All the controllers should extend this class, which enforce the responsive helper to be protected
	/// and binds the complete event to the operation finish method
	/// </summary>
	public abstract class AbstractController : IController
	{
		protected IResponsiveClient _responsiveHelper;

		protected IResponsiveClient ResponsiveHelper
		{
			get 
			{ 
				return _responsiveHelper; 
			}
			set 
			{ 
				_responsiveHelper = value; 
			}
		}

		public AbstractController(IResponsiveClient helper)
		{
			_responsiveHelper = helper;
			_responsiveHelper.TransferCompleteEvent += OnAsyncOperationFinish;
		}

		protected abstract void OnAsyncOperationFinish(Object sender, ThreadEventArgs args);

		protected void StopAsyncCall()
		{
			System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(1);
			System.Reflection.MethodBase mb = sf.GetMethod();
			_responsiveHelper.StopAsyncMethod(System.Threading.Thread.CurrentThread.ManagedThreadId, mb, null);
		}

		protected MethodBase Method(SimpleDelegate del)
		{
			return (MethodBase)del.Method;
		}
	}
}
