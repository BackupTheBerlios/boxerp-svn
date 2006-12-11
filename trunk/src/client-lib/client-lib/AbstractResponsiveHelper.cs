using System;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System.Reflection;

namespace Boxerp.Client
{
	/// <summary>
	/// Description of AbstractResponsiveHelper.
	/// </summary>
	public abstract class AbstractResponsiveHelper: IResponsiveClient
	{
		private static Hashtable threadsPoolHash = Hashtable.Synchronized(new Hashtable());
        
        private int _asyncCallsCount = 0; 
        private ResponsiveEnum _transferType;
        private bool _cancelRequest = false;
        
		private EventHandler _baseTransferCompleteHandler;
				
		public event EventHandler BaseTransferCompleteEvent
      	{
        	add
         	{
            	_baseTransferCompleteHandler += value;
         	}
         	remove
        	{
        		_baseTransferCompleteHandler += value;
            }
      	}
      	
      	public bool CancelRequest
      	{
      		get { return _cancelRequest; }
      		set { _cancelRequest = value; }
      	}
      	
      	public void StartAsyncCall(SimpleDelegate method)
		{
			ProcessAsyncCall(method);
		}
		
		public void StopTransfer()
		{
			lock(this)
			{
				if (_asyncCallsCount > 0)
				{
					_asyncCallsCount --;
					if (_asyncCallsCount == 0)
					{
						threadsPoolHash.Clear();
						if (_baseTransferCompleteHandler != null)
            				_baseTransferCompleteHandler(_transferType, null);
					}
				}
			}
		}
		
		
		private void ProcessAsyncCall(SimpleDelegate method)
		{
			try
			{
				_asyncCallsCount = 1;
					//FIXME: invoke asyncrhonously
				method(); 
				//.Invoke(this, null); // execute method
			}
			catch (TargetInvocationException ex)
            {
                Console.WriteLine("responsive method raises exception:" + ex.Message+ ex.StackTrace);
                //throw ex; // FIXME: How to catch an exception inside a thread?
            }
            finally
            {
            }
		}
		
		public virtual void StartTransfer(Boxerp.Client.ResponsiveEnum trType)
		{
			_transferType = trType;
			try
			{
				if (threadsPoolHash.Count != 0)
				{
					// busy, show an error message
				}
				
				List<MethodInfo> methods = this.GetResponsiveMethods(_transferType);
				_asyncCallsCount = methods.Count;
				foreach (MethodInfo method in methods)
				{
					
					ThreadStart methodStart = new SimpleInvoker(method, this).Invoke;
					Thread methodThread = new Thread(methodStart);
					methodThread.Start();
					threadsPoolHash[methodThread.ManagedThreadId] = methodThread;
					//method.Invoke(this, null); // execute method
				}
			}
			catch (TargetInvocationException ex)
            {
                Console.WriteLine("One responsive method raises exception:" + ex.Message+ ex.StackTrace);
                throw ex;
            }
            catch(Exception ex)
            {
            	throw ex;
            }
        	// TODO : Write the code to stop threads
		}
		
		
		private List<System.Reflection.MethodInfo> GetResponsiveMethods(ResponsiveEnum trType)
		{
			List<MethodInfo> responsiveMethods = new List<MethodInfo>();
			MethodInfo[] methods = this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
			foreach(MethodInfo method in methods)
			{
				object[] attributes = method.GetCustomAttributes(typeof(ResponsiveAttribute), true);
				if (attributes.Length != 0)
				{
					ResponsiveAttribute att = (ResponsiveAttribute)attributes[0];
					if (att.RespType == trType)
					{
						responsiveMethods.Add(method);						
					}
				}
			}
			return responsiveMethods;
		}
	}
}
