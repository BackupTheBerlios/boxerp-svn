
using System;
using System.Collections;
using System.Threading;
using widgets;
using System.Reflection;
using Gtk;

namespace clientlib
{
	// TODO: llamar a upload/download pasandole el metodo que sea	
	public abstract class ResponsiveHelper : IResponsiveClient
	{
		widgets.WaitDialog waitDialog;
		widgets.WarningDialog warningDialog;
		protected ThreadStart threadStartUpload;
		protected ThreadStart threadStartDownload;		
		protected Thread threadUpload;
		protected Thread threadDownload;
		static ThreadNotify threadNotify;
		//protected System.Type instanceType;
		protected Gtk.Window parentWindow;
		protected bool uploadSuccess = false;
		protected bool downloadSuccess = true;
		
		protected void InitThreads()
		{
			threadStartUpload   = new ThreadStart(Upload);
			threadStartDownload = new ThreadStart(Download); 
			threadNotify = new ThreadNotify (new ReadyEvent (DownloadComplete));
		}
		
		public void Init(Gtk.Window win)
		{
			parentWindow = win;
			//instanceType = type;
			InitThreads();
		}
	
		public void StartUpload()
		{
			waitDialog = new WaitDialog(parentWindow); 
			threadUpload = new Thread(threadStartUpload);
			threadUpload.Start();
		}
	
		public void Upload()
		{
			try
			{
				ArrayList methods = this.GetResponsiveMethods(ResponsiveEnum.Upload);
				foreach (MethodInfo method in methods)
				{
					method.Invoke(this, null); // execute method	
				}
				uploadSuccess = true;
			}
			catch (Exception ex)
			{
            	Console.WriteLine("Exception: " + ex.Message);
            	uploadSuccess = false;
			}
			finally
			{
				threadNotify.WakeupMain();
			}			
		}
	
		public void StartDownload()
		{
			waitDialog = new WaitDialog(parentWindow); 
			threadDownload = new Thread(threadStartDownload);
			threadDownload.Start();		
		}
	
		public void Download()
		{
			try
			{
				ArrayList methods = this.GetResponsiveMethods(ResponsiveEnum.Download);
				foreach (MethodInfo method in methods)
				{
					method.Invoke(this, null); // execute method	
				}
				downloadSuccess = true;
			}
			catch (Exception ex)
			{
            	Console.WriteLine("Exception: " + ex.Message);
            	downloadSuccess = false;
			}
			finally
			{
				threadNotify.WakeupMain();
			}
		}
	
		public void UploadComplete()
		{
			if (uploadSuccess)
         	{
				waitDialog.Stop();
				waitDialog.Destroy();
				parentWindow.Present();
			}
			else
			{
				warningDialog = new WarningDialog();
         		warningDialog.Message = "Connection error";
         		warningDialog.QuitOnOk = false;
            	warningDialog.Present();
         	}
		}
	
		public void DownloadComplete()
		{
			if (downloadSuccess)
         	{
				waitDialog.Stop();
				waitDialog.Destroy();
				this.PopulateGUI(); // FIXME: this could freeze, it is not resonsible
				parentWindow.Present();
			}
			else
			{
				warningDialog = new WarningDialog();
         		warningDialog.Message = "Connection error";
         		warningDialog.QuitOnOk = false;
            	warningDialog.Present();
         	}		
		}
		
		public virtual void PopulateGUI()
		{
		
		}
		
		public ArrayList GetResponsiveMethods(ResponsiveEnum rType)
		{
			ArrayList responsiveMethods = new ArrayList();
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
