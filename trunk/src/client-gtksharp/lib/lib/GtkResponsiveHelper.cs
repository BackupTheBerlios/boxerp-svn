
using System;
using System.Collections;
using System.Threading;
using System.Reflection;
using Gtk;

namespace Boxerp.Client.GtkSharp.Lib
{
	// TODO: llamar a upload/download pasandole el metodo que sea	
	public abstract class GtkResponsiveHelper : AbstractResponsiveHelper
	{
		WaitDialog waitDialog;
		WarningDialog warningDialog;
		//protected ThreadStart threadStartUpload;
		//protected ThreadStart threadStartDownload;		
		//protected Thread threadUpload;
		//protected Thread threadDownload;
		static ThreadNotify threadNotify;
		protected Gtk.Window parentWindow;
		protected bool uploadSuccess = false;
		protected bool downloadSuccess = true;
		private EventHandler downloadCompleteEventHandler;
		
		public event EventHandler DownloadCompleteEvent
      	{
        	add
         	{
            	downloadCompleteEventHandler += value;
         	}
         	remove
        	{
            	downloadCompleteEventHandler -= value;
         	}
      	}

		
		protected void InitThreads()
		{
			//threadStartUpload   = new ThreadStart(Upload);
			//threadStartDownload = new ThreadStart(Download); 
			
		}
		
		public void Init(Gtk.Window win)
		{
			parentWindow = win;
			InitThreads();
			ThreadDownloadStopEvent += OnDownloadStop;
			ThreadUploadStopEvent += OnUploadStop;
		}
	
		public override void StartUpload()
		{
			waitDialog = new WaitDialog(parentWindow); 
			waitDialog.CancelEvent += OnCancel;
			if (uploadThread.ThreadState != ThreadState.Unstarted)
            {
                //Console.WriteLine("thread is aborted:" + downloadThread.ThreadState.ToString());
                uploadThread = new Thread(base.Upload);
            }
            uploadThread.Start();
        }
		
		public void OnCancel(object sender, EventArgs e)
		{
		
		}
		
		public void OnUploadStop(object sender, EventArgs e)
		{
			threadNotify = new ThreadNotify (new ReadyEvent (UploadComplete));
			threadNotify.WakeupMain();
		}
		
		public void OnDownloadStop(object sender, EventArgs e)
		{
			threadNotify = new ThreadNotify (new ReadyEvent (DownloadComplete));
			threadNotify.WakeupMain();
		}
		
	
/*		protected override void Upload()
		{
			try
			{
				UserInformation.SetSessionToken(SessionSingleton.GetInstance().GetSession());
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
	*/
		public override void StartDownload()
		{
			waitDialog = new WaitDialog(parentWindow); 
			waitDialog.CancelEvent += OnCancel;
			if (downloadThread.ThreadState != ThreadState.Unstarted)
            {
                //Console.WriteLine("thread is aborted:" + downloadThread.ThreadState.ToString());
                downloadThread = new Thread(base.Download);
            }
            downloadThread.Start();
			//threadDownload = new Thread(threadStartDownload);
			//threadNotify = new ThreadNotify (new ReadyEvent (DownloadComplete));
			//threadDownload.Start();		
		}
	
/*		public override void Download()
		{
			try
			{
				UserInformation.SetSessionToken(SessionSingleton.GetInstance().GetSession());
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
	*/
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
		
		public override void PopulateGUI()
		{
		
		}		
	}
	
}
