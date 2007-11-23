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
		// All threads launched at a time are kept in a queue, trying to have a single unit for every client async call.
		// Along with the threads there should be queues for success messages, exceptions, cancelling requests and type of async operations:
		object _innerLock;
		private List<Dictionary<int, Thread>> _threadDictionariesList = new List<Dictionary<int, Thread>>();
		private Dictionary<int, bool> _operationSucess = new Dictionary<int, bool>();
		private Dictionary<int, string> _exceptions = new Dictionary<int, string>();
		private Dictionary<int, ResponsiveEnum> _operationTypes = new Dictionary<int, ResponsiveEnum>();
		private Dictionary<int, bool> _cancelRequests = new Dictionary<int, bool>();
		        
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
		
		protected AbstractResponsiveHelper(ConcurrencyMode mode)
		{
			_concurrencyMode = mode;
			_innerLock = _threadDictionariesList;
		}

		protected int RunningThreads
		{
			get
			{
				lock(_innerLock)
				{
					return _threadDictionariesList.Count;
				}
			}
		}
		
		public bool CancelRequested
		{
			get
			{
				if (_cancelRequests.Count > 0)
				{
					return _cancelRequests[Thread.CurrentThread.ManagedThreadId];
				}
				else
				{
					return false;
				}
			}
			protected set
			{
				lock(_innerLock)
				{
					_cancelRequests[Thread.CurrentThread.ManagedThreadId] = value;
				}
			}
		}
		
		private bool canGoAhead()
		{
			lock(_innerLock)
			{
				if (_threadDictionariesList.Count != 0)
				{
					if (_concurrencyMode != ConcurrencyMode.Parallel)
					{
						return false;
					}
				}

				return true;
			}
		}

		/// <summary>
		/// Create a new thread and put it in the queue 
		/// (as long as there are no more running threads and the mode is not parallel)
		/// </summary>
		/// <param name="method"></param>
      	public virtual Thread StartAsyncCall(SimpleDelegate method)
		{
			try
			{
				Thread methodThread = null;
				if (canGoAhead())
				{
					Dictionary<int, Thread> threadsBlock = new Dictionary<int, Thread>();

					ThreadStart methodStart = new SimpleInvoker(method).Invoke;
					methodThread = new Thread(methodStart);
					lock(_threadDictionariesList)
					{
						methodThread.Start();
						int id = methodThread.ManagedThreadId;
						threadsBlock[id] = methodThread;
						_operationSucess[id] = true;
						_exceptions[id] = null;
						_operationTypes[id] = ResponsiveEnum.Other;
						_cancelRequests[id] = false;

						_threadDictionariesList.Add(threadsBlock);
						
						Console.Out.WriteLine("*** *** *** thread is in queue now:" + id + "," + _threadDictionariesList.Count);
					}
				}
				return methodThread;
			}
			catch (TargetInvocationException ex)
			{
				Console.WriteLine("User responsive method raises exception:" + ex.Message + ex.StackTrace);
				throw ex;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public abstract Thread StartAsyncCall(SimpleDelegate method, bool showWaitControl);
		


		/// <summary>
		/// Create a block of parallel threads and put it in a single slot in the queue
		/// </summary>
		/// <param name="trType"></param>
		/// <param name="controller"></param>
		public virtual List<Thread> StartAsyncCallList(Boxerp.Client.ResponsiveEnum trType, IController controller)
		{
			try
			{
				List<Thread> threads = new List<Thread>();
				if (canGoAhead())
				{
					List<MethodInfo> methods = this.GetResponsiveMethods(trType, controller);
					if (methods.Count == 0)
					{
						throw new NullReferenceException("No private/protected responsive methods found");
					}

					Dictionary<int, Thread> threadsBlock = new Dictionary<int, Thread>();
					lock(_innerLock)
					{
						foreach (MethodInfo method in methods)
						{
							ThreadStart methodStart = new SimpleInvoker(method, controller).Invoke;
							Thread methodThread = new Thread(methodStart);
							methodThread.Start();
							int id = methodThread.ManagedThreadId;
							threads.Add(methodThread);
							threadsBlock[id] = methodThread;
							_operationSucess[id] = true;
							_exceptions[id] = null;
							_operationTypes[id] = trType;
							_cancelRequests[id] = false;
						}
						_threadDictionariesList.Add(threadsBlock);
					}
				}
				return threads;
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

		public abstract List<Thread> StartAsyncCallList(Boxerp.Client.ResponsiveEnum trType, IController controller, bool showWaitControl);

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
			lock(_innerLock)
			{
				if (_threadDictionariesList.Count == 0)
				{
					throw new Exception("Error trying to stop asynchronous call. You are most likely calling StopAsyncMethod more than once");
				}
				else
				{
					Console.Out.WriteLine("*** stopping: " + _threadDictionariesList.Count);
					for (int i = 0; i < _threadDictionariesList.Count; i++)
					{
						Dictionary<int, Thread> threadBlock = _threadDictionariesList[i];
						if (threadBlock.ContainsKey(args.ThreadId))
						{
							Console.Out.WriteLine("threadBlog count:" + threadBlock.Count + "," + Thread.CurrentThread.ManagedThreadId);
							if (threadBlock.Count > 0)
							{
								threadBlock.Remove(args.ThreadId);
								_cancelRequests.Remove(args.ThreadId);
								// FIXME: doing this, the args is populated with the last thread results, 
								// loosing the previous ones if the operation launched a group of threads 
								// in parallell
								args.Success = args.Success && _operationSucess[args.ThreadId];
								_operationSucess.Remove(args.ThreadId); ;
								args.OperationType = _operationTypes[args.ThreadId];
								_operationTypes.Remove(args.ThreadId); ;
								args.ExceptionMsg += _exceptions[args.ThreadId];
								_exceptions.Remove(args.ThreadId);

								Console.Out.WriteLine("  now after removing:" + threadBlock.Count);
								if (threadBlock.Count == 0)
								{
									Console.Out.WriteLine("DEQUEUING DATA FROM STORAGE:" + Thread.CurrentThread.ManagedThreadId);
									_threadDictionariesList.RemoveAt(i);
									OnTransferCompleted(args.OperationType, args);
									break;
								}
							}
						}
					}
					
				}
			}
		}
		
		/// <summary>
		///  
		/// </summary>
		protected void ForceAbort(int threadId)
		{
			lock(_innerLock)
			{
				if (_threadDictionariesList.Count > 0)
				{
					for (int i = 0; i < _threadDictionariesList.Count; i++)
					{
						Dictionary<int, Thread> threadsBlock = _threadDictionariesList[i];
						if (threadsBlock.ContainsKey(threadId))
						{
							foreach (Thread thread in threadsBlock.Values)
							{

								Console.WriteLine("state: " + thread.IsAlive + "," + thread.ThreadState);
								if ((thread.IsAlive) && (thread.ThreadState != ThreadState.Stopped))
								{
									thread.Abort();
								}
							}
							break;
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
			lock(_innerLock)
			{
					// FIXME, _exceptions should be a Queue<Exception> to be able to separate the message from the stacktrace
				_exceptions[Thread.CurrentThread.ManagedThreadId] += ex.Message + ", " + ex.StackTrace + "\n";
			}

			lock(_innerLock)
			{
				_operationSucess[Thread.CurrentThread.ManagedThreadId] = false;
			}
		}

		public void OnAbortAsyncCall(Exception ex)
		{
			string message = "Operation stopped.";
			if ((ex.StackTrace.IndexOf("WebAsyncResult.WaitUntilComplete") > 0) || (ex.StackTrace.IndexOf("WebConnection.EndWrite") > 0))
			{
				message += "Warning!, the operation seems to have been succeded at the server side";
				lock(_innerLock)
				{
					_exceptions[Thread.CurrentThread.ManagedThreadId] += message;
				}
			}
			lock(_innerLock)
			{
				_operationSucess[Thread.CurrentThread.ManagedThreadId] = false;
			}
		}

		#region Abstract methods

		public abstract void OnCancel(object sender, EventArgs e);
		public abstract void OnTransferCompleted(object sender, ThreadEventArgs e);
		

		#endregion
	}
}
