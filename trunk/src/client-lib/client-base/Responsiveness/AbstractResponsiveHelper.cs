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
		private  Queue<Dictionary<int, Thread>> _threadDictionariesQueue = new Queue<Dictionary<int, Thread>>();
		private  Queue<bool> _operationSuccessQueue = new Queue<bool>();
		private  Queue<string> _exceptionQueue = new Queue<string>();
		private  Queue<ResponsiveEnum> _operationTypeQueue = new Queue<ResponsiveEnum>();
		private  Queue<bool> _cancelRequestQueue = new Queue<bool>(); 
		        
		protected ConcurrencyMode _concurrencyMode;
				

		// Just to notify clients when the transfer is completed
		protected ThreadEventHandler transferCompleteEventHandler;
		public event ThreadEventHandler TransferCompleteEvent
      	{
        	add
         	{
            	transferCompleteEventHandler += value;
         	}
         	remove
        	{
            	transferCompleteEventHandler -= value;
         	}
      	}
		
		public AbstractResponsiveHelper(ConcurrencyMode mode)
		{
			_concurrencyMode = mode;
		}

		protected int RunningThreads
		{
			get
			{
				return _threadDictionariesQueue.Count;
			}
		}
		
		public bool CancelRequested
		{
			get
			{
				return _cancelRequestQueue.Peek();
			}
			protected set
			{
				lock (_cancelRequestQueue)
				{
					_cancelRequestQueue.Dequeue();
					bool isCancel = value;
					_cancelRequestQueue.Enqueue(isCancel);
				}
			}
		}
		
		private bool canGoAhead()
		{
			if (_threadDictionariesQueue.Count != 0)
			{
				if (_concurrencyMode != ConcurrencyMode.Parallel)
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Create a new thread and put it in the queue 
		/// (as long as there are no more running threads and the mode not be parallel)
		/// </summary>
		/// <param name="method"></param>
      	public virtual void StartAsyncCall(SimpleDelegate method)
		{
			try
			{
				if (canGoAhead())
				{
					Dictionary<int, Thread> threadsBlock = new Dictionary<int, Thread>();

					ThreadStart methodStart = new SimpleInvoker(method).Invoke;
					Thread methodThread = new Thread(methodStart);
					methodThread.Start();

					threadsBlock[methodThread.ManagedThreadId] = methodThread;

					lock (this)
					{
						_threadDictionariesQueue.Enqueue(threadsBlock);
						_operationSuccessQueue.Enqueue(true);
						_exceptionQueue.Enqueue(null);
						_operationTypeQueue.Enqueue(ResponsiveEnum.Other);
						_cancelRequestQueue.Enqueue(false);
					}
				}
			}
			catch (TargetInvocationException ex)
			{
				Console.WriteLine("responsive method raises exception:" + ex.Message + ex.StackTrace);
				throw ex;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Create a block of parallel threads and put it in a single slot in the queue
		/// </summary>
		/// <param name="trType"></param>
		/// <param name="controller"></param>
		public virtual void StartAsyncCallList(Boxerp.Client.ResponsiveEnum trType, IController controller)
		{
			try
			{
				if (canGoAhead())
				{
					List<MethodInfo> methods = this.GetResponsiveMethods(trType, controller);
					if (methods.Count == 0)
					{
						throw new NullReferenceException("No private/protected responsive methods found");
					}

					Dictionary<int, Thread> threadsBlock = new Dictionary<int, Thread>();
					lock (this)
					{
						foreach (MethodInfo method in methods)
						{
							ThreadStart methodStart = new SimpleInvoker(method, controller).Invoke;
							Thread methodThread = new Thread(methodStart);
							methodThread.Start();

							threadsBlock[methodThread.ManagedThreadId] = methodThread;
						}
						_threadDictionariesQueue.Enqueue(threadsBlock);
						_operationSuccessQueue.Enqueue(true);
						_exceptionQueue.Enqueue(null);
						_operationTypeQueue.Enqueue(trType);
						_cancelRequestQueue.Enqueue(false);
					}
				}
			}
			catch (TargetInvocationException ex)
			{
				Console.WriteLine("One responsive method raises exception:" + ex.Message + ex.StackTrace);
				throw ex;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// Stops the current thread passing in information about the thread itself, and any outcome information
		/// </summary>
		/// <param name="threadId">You can use the CurrentThread Abstract Controller's property or System.Threading.Thread.CurrentThread.ManagedThreadId</param>
		/// <param name="methodBase">The method who is running. You can call MethodBase(method), implemented in the Abstract Controller</param>
		/// <param name="output">Any info you want to send. It could be a message, an exception or whatever you need</param>
		public void StopAsyncMethod(int threadId, MethodBase methodBase, object output)
		{
			ThreadEventArgs tea = new ThreadEventArgs(threadId, methodBase, output);
			StopAsyncMethod(tea, output);
		}

		/// <summary>
		/// Stops the current thread passing in information about the thread itself, and any outcome information
		/// </summary>
		/// <param name="threadId">You can use the CurrentThread Abstract Controller's property or System.Threading.Thread.CurrentThread.ManagedThreadId</param>
		/// <param name="method">The method who is running. You can pass the method if its signature matches the SimpleDelegate</param>
		/// <param name="output">Any info you want to send. It could be a message, an exception or whatever you need</param>
		public void StopAsyncMethod(int threadId, SimpleDelegate method, object output)
		{
			ThreadEventArgs tea = new ThreadEventArgs(threadId, method, output);
			StopAsyncMethod(tea, output);
		}

		/// <summary>
		/// Take the first block of threads in the queue and remove it from the queue. 
		/// The contents of the block are the threads created by StartAsyncCall or StartAsyncCallList
		/// When all the threads from that block have finished invoke OnTransferCompleted to access the GUI
		/// </summary>
		/// <param name="args"></param>
		/// <param name="output"></param>
		private void StopAsyncMethod(ThreadEventArgs args, object output)
		{
			lock (this)
			{
				if (_threadDictionariesQueue.Count == 0)
				{
					throw new Exception("Error trying to stop asynchronous call. You are most likely calling StopAsyncMethod more than once");
				}
				else
				{
					Dictionary<int, Thread> firstThreadBlock = _threadDictionariesQueue.Peek();
					if (firstThreadBlock.Count > 0)
					{
						firstThreadBlock.Remove(args.ThreadId);
						if (firstThreadBlock.Count == 0)
						{
							_threadDictionariesQueue.Dequeue();
							_cancelRequestQueue.Dequeue();
							args.Success = _operationSuccessQueue.Dequeue();
							args.OperationType = _operationTypeQueue.Dequeue();
							args.ExceptionMsg = _exceptionQueue.Dequeue();
							OnTransferCompleted(args.OperationType, args);
						}
					}
				}
			}
		}
		
		/// <summary>
		///  Take the first block of threads in the queue and abort all of them
		/// </summary>
		protected void ForceAbort()
		{
			lock (this)
			{
				if (_threadDictionariesQueue.Count > 0)
				{
					Dictionary<int, Thread> threadsBlock = _threadDictionariesQueue.Peek();
					foreach (Thread thread in threadsBlock.Values)
					{
						Console.WriteLine("state: " + thread.IsAlive + "," + thread.ThreadState); 
						if ((thread.IsAlive) && (thread.ThreadState != ThreadState.Stopped))
						{
							thread.Abort();
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Pick up all methods marked with the ResponsiveAttribute
		/// </summary>
		/// <param name="trType"></param>
		/// <param name="controller"></param>
		/// <returns></returns>
		private List<System.Reflection.MethodInfo> GetResponsiveMethods(ResponsiveEnum trType, IController controller)
		{
			List<MethodInfo> responsiveMethods = new List<MethodInfo>();
			MethodInfo[] methods = controller.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);
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

		public void OnAsyncException(Exception ex)
		{
			lock (_exceptionQueue)
			{
				string exceptions = _exceptionQueue.Dequeue();
				exceptions += ex.Message + ", " + ex.StackTrace + "\n";	// FIXME, _exceptionQueue should be a Queue<Exception> to be able to separate the message from the stacktrace
				_exceptionQueue.Enqueue(exceptions);
			}

			lock (_operationSuccessQueue)
			{
				_operationSuccessQueue.Dequeue();
				_operationSuccessQueue.Enqueue(false);
			}
		}

		public void OnAbortAsyncCall(Exception ex)
		{
			string message = "Operation stopped.";
			if ((ex.StackTrace.IndexOf("WebAsyncResult.WaitUntilComplete") > 0) || (ex.StackTrace.IndexOf("WebConnection.EndWrite") > 0))
			{
				message += "Warning!, the operation seems to have been succeded at the server side";
				lock (_exceptionQueue)
				{
					string exceptions = _exceptionQueue.Dequeue();
					exceptions += message;
					_exceptionQueue.Enqueue(exceptions);
				}
			}
			lock (_operationSuccessQueue)
			{
				_operationSuccessQueue.Dequeue();
				_operationSuccessQueue.Enqueue(false);
			}
		}

		#region Abstract methods

		public abstract void OnCancel(object sender, EventArgs e);
		public abstract void OnTransferCompleted(object sender, ThreadEventArgs e);
		

		#endregion
	}
}
