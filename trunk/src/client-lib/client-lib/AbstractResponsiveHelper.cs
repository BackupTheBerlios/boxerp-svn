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
		public abstract void StartUpload();
		public abstract void StartDownload();
		protected abstract void PopulateGUI();
        private ThreadStart threadStartDownload;
        private ThreadStart threadStartUpload; 
        public Thread uploadThread; 
        public Thread downloadThread; 

		/*protected abstract void UploadComplete();
		protected abstract void DownloadComplete();*/

        public void Init()
        {
            threadStartDownload = new ThreadStart(Download);
            threadStartUpload = new ThreadStart(Upload);
            uploadThread = new Thread(threadStartUpload);
            downloadThread = new Thread(threadStartDownload);
        }

		protected void Upload()
		{
			try
			{
				#if REMOTING
					UserInformation.SetSessionToken(SessionSingleton.GetInstance().GetSession());
				#endif
				List<MethodInfo> methods = this.GetResponsiveMethods(ResponsiveEnum.Upload);
				foreach (MethodInfo method in methods)
				{
					method.Invoke(this, null); // execute method
				}
			}
			catch (TargetInvocationException ex)
            {
                Console.WriteLine("On responsive method raises exception:" + ex.Message+ ex.StackTrace);
                //throw ex; // FIXME: How to catch an exception inside a thread?
            }
		}
		
		protected void Download()
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

		}
		
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
