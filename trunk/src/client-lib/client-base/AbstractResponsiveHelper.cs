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
        
		public bool CancelRequest
      	{
      		get { return _cancelRequest; }
      		set { _cancelRequest = value; }
      	}
      	
      	public virtual void StartAsyncCall(SimpleDelegate method)
		{
			ProcessAsyncCall(method);
		}
		
		public void StopAsyncMethod(int threadId, MethodBase methodBase, object output)
		{
			ThreadEventArgs tea = new ThreadEventArgs(threadId, methodBase, output);
			StopAsyncMethod(threadId, tea, output);
		}

		public void StopAsyncMethod(int threadId, SimpleDelegate method, object output)
		{
			ThreadEventArgs tea = new ThreadEventArgs(threadId, method, output);
			StopAsyncMethod(threadId, tea, output);
		}

		private void StopAsyncMethod(int threadId, ThreadEventArgs args, object output)
		{
			lock (this)
			{
				if (_asyncCallsCount > 0)
				{
					_asyncCallsCount--;
					if (_asyncCallsCount == 0)
					{
						_threadsPoolHash.Clear();
						OnTransferCompleted(_transferType, args);
						/*if (_baseTransferCompleteHandler != null)
						{
							Delegate[] invList = _baseTransferCompleteHandler.GetInvocationList();
							if (invList.Length > 0)
							{
								object[] parameters = { _transferType, args };
								invList[0].DynamicInvoke(parameters);
							}
							else
							{
								throw new NullReferenceException("BaseTransferCompleteEvent has no handler");
							}
						}*/
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
		
		public virtual void StartAsyncCallList(Boxerp.Client.ResponsiveEnum trType)
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
					    // TODO: Search within method code calls to OnAbortRemoteCall, OnRemoteException, StopTransfer: Cecil?
						responsiveMethods.Add(method);						
					}
				}
			}
			return responsiveMethods;
		}

		#region Abstract methods

		public abstract void PopulateGUI();
		public abstract void OnCancel(object sender, EventArgs e);
		public abstract void OnRemoteException(string msg);
		public abstract void OnAbortRemoteCall(string msg);
		public abstract void OnTransferCompleted(object sender, ThreadEventArgs e);
		public abstract void OnAsyncCallStop(object sender, ThreadEventArgs teargs);
		public abstract event ThreadEventHandler TransferCompleteEvent;

		#endregion
	}
}
