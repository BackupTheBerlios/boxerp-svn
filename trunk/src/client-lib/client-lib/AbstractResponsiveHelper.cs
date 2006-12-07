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
		/*protected abstract void Init();*/
		//private ThreadStart threadStartDownload;
        //private ThreadStart threadStartUpload; 
        //public Thread uploadThread; 
        //public Thread downloadThread;
        public delegate void SimpleDelegate();
        
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
      	
      	public virtual void StartTransfer()
		{}
		
		public void StartAsyncCall(SimpleDelegate method)
		{
			ProcessAsyncCall(method);
		}
		
		
		public virtual void PopulateGUI()
		{
		
		}
		
        public void Init()
        {
            /*threadStartDownload = new ThreadStart(Download);
            threadStartUpload = new ThreadStart(Upload);
            uploadThread = new Thread(threadStartUpload);
            downloadThread = new Thread(threadStartDownload);*/
        }

		public void StopCallHandler()
		{
			lock(this)
			{
				if (_asyncCallsCount > 0)
				{
					_asyncCallsCount --;
					if (_asyncCallsCount == 0)
					{
						if (_baseTransferCompleteHandler != null)
            				_baseTransferCompleteHandler(_transferType, null);
					}
				}
			}
		}
		
		
		protected void ProcessAsyncCall(SimpleDelegate method)
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
		
		protected void Transfer(ResponsiveEnum trType)
		{
			_transferType = trType;
			try
			{
				#if REMOTING
					UserInformation.SetSessionToken(SessionSingleton.GetInstance().GetSession());
				#endif
				List<MethodInfo> methods = this.GetResponsiveMethods(_transferType);
				_asyncCallsCount = methods.Count;
				foreach (MethodInfo method in methods)
				{
					//FIXME: invoke asyncrhonously
					method.Invoke(this, null); // execute method
				}
			}
			catch (TargetInvocationException ex)
            {
                Console.WriteLine("One responsive method raises exception:" + ex.Message+ ex.StackTrace);
                //throw ex; // FIXME: How to catch an exception inside a thread?
            }
            finally
            {
            	//if (threadUploadStopHandler != null)
            	//	threadUploadStopHandler(this, null);
            }
		}
		
		/*protected void Download()
		{
            try
            {
				#if REMOTING
	   	 			UserInformation.SetSessionToken(SessionSingleton.GetInstance().GetSession());
				#endif
                List<MethodInfo> methods = this.GetResponsiveMethods(ResponsiveEnum.Download);
                foreach (MethodInfo method in methods)
                    method.Invoke(this, null); // execute method	
            }
            catch (TargetInvocationException ex)
            {
                Console.WriteLine("On responsive method raises exception:" + ex.Message+ ex.StackTrace);
                //throw ex; // FIXME: How to catch an exception inside a thread?
            }
            finally
            {
            	if (threadDownloadStopHandler != null)
            		threadDownloadStopHandler(this , null);
            }

		}*/
		
		public List<System.Reflection.MethodInfo> GetResponsiveMethods(ResponsiveEnum rType)
		{
			List<MethodInfo> responsiveMethods = new List<MethodInfo>();
			//ArrayList responsiveMethods = new ArrayList();
			MethodInfo[] methods = this.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);
			foreach(MethodInfo method in methods)
			{
				object[] attributes = method.GetCustomAttributes(typeof(ResponsiveAttribute), true);
				if (attributes.Length != 0)
				{
					ResponsiveAttribute att = (ResponsiveAttribute)attributes[0];
					if (att.RespType == rType)
						responsiveMethods.Add(method);
				}
			}
			return responsiveMethods;
		}
	}
}
