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
		private static Hashtable _threadsPoolHash = Hashtable.Synchronized(new Hashtable());
        
        private int _asyncCallsCount = 0; 
        private ResponsiveEnum _transferType;
        private bool _cancelRequest = false;
        
		private ThreadEventHandler _baseTransferCompleteHandler;
				
		public event ThreadEventHandler BaseTransferCompleteEvent
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
      	
      	public virtual void StartAsyncCall(SimpleDelegate method)
		{
			ProcessAsyncCall(method);
		}
		
		public void StopTransfer(int threadId, MethodBase methodBase, object output)
		{
			lock(this)
			{
				if (_asyncCallsCount > 0)
				{
					_asyncCallsCount --;
					if (_asyncCallsCount == 0)
					{
						_threadsPoolHash.Clear();
						if (_baseTransferCompleteHandler != null)
						{
            				ThreadEventArgs tea = new ThreadEventArgs(threadId, methodBase, output);
            				Delegate[] invList = _baseTransferCompleteHandler.GetInvocationList();
            				if (invList.Length > 0)
            				{
            				    object[] parameters = { _transferType, tea };
            				    invList[0].DynamicInvoke(parameters);
            				        
            			        //_baseTransferCompleteHandler(_transferType, tea);
            			    }
            			    else
            			    {
            			        throw new NullReferenceException("BaseTransferCompleteEvent has no handler");
            			    }
            			}
					}
				}
			}
		}
		
		
		private void ProcessAsyncCall(SimpleDelegate method)
		{
			try
			{
			    _transferType = ResponsiveEnum.Other;
				_asyncCallsCount++; // = 1;
				ThreadStart methodStart = new SimpleInvoker(method).Invoke;
				Thread methodThread = new Thread(methodStart);
				methodThread.Start();
				_threadsPoolHash[methodThread.ManagedThreadId] = methodThread;
			}
			catch (TargetInvocationException ex)
            {
                Console.WriteLine("responsive method raises exception:" + ex.Message+ ex.StackTrace);
                throw ex; 
            }
            catch(Exception ex)
            {
            	throw ex;
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
				if (_threadsPoolHash.Count != 0)
				{
					// busy, show an error message
				}
				
				List<MethodInfo> methods = this.GetResponsiveMethods(_transferType);
				if (methods.Count == 0)
				{
				    throw new NullReferenceException("No private/protected responsive methods found");
				}
				
				_asyncCallsCount = methods.Count;
				
				foreach (MethodInfo method in methods)
				{
					
					ThreadStart methodStart = new SimpleInvoker(method, this).Invoke;
					Thread methodThread = new Thread(methodStart);
					methodThread.Start();
					_threadsPoolHash[methodThread.ManagedThreadId] = methodThread;
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
		
		protected void ForceAbort()
		{
		    foreach(Thread thread in _threadsPoolHash.Values)
		    {
		        thread.Abort();
		    }
		}
		
		private List<System.Reflection.MethodInfo> GetResponsiveMethods(ResponsiveEnum trType)
		{
			List<MethodInfo> responsiveMethods = new List<MethodInfo>();
			MethodInfo[] methods = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
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
