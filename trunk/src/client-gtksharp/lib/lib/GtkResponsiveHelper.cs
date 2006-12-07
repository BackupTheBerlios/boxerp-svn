
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
		//static ThreadNotify threadNotify; // static ?, error prune code
		protected bool transferSuccess;
		protected Gtk.Window parentWindow;
		
		// Just to notify clients when the transfer is completed
		private EventHandler transferCompleteEventHandler;
		public event EventHandler TransferCompleteEvent
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

		
		/*protected void InitThreads()
		{
			//threadStartUpload   = new ThreadStart(Upload);
			//threadStartDownload = new ThreadStart(Download); 
			
		}*/
		
		public void Init(Gtk.Window win)
		{
			parentWindow = win;
			//InitThreads();
			base.BaseTransferCompleteEvent += this.OnTransferCompleted;
			//ThreadDownloadStopEvent += OnDownloadStop;
			//ThreadUploadStopEvent += OnUploadStop;
		}
	
		public void StartTransfer(ResponsiveEnum transferType)
		{
			waitDialog = new WaitDialog(parentWindow); 
			waitDialog.CancelEvent += OnCancel;
			transferSuccess = true;
			base.Transfer(transferType);
			/*if (uploadThread.ThreadState != ThreadState.Unstarted)
            {
                //Console.WriteLine("thread is aborted:" + downloadThread.ThreadState.ToString());
                uploadThread = new Thread(base.Upload);
            }
            uploadThread.Start();*/
        }
		
		public void OnCancel(object sender, EventArgs e)
		{
        	CancelRequest = true;	
        	//TODO: Show a dialog :" Please wait while cancelling" with
        	//a button to force cancelation by aborting threads
		}
		
		public void OnTransferCompleted(object sender, EventArgs e)
		{
			ResponsiveEnum transferType = (ResponsiveEnum)sender;
			
			if (transferSuccess) // FIXME: transferSuccess must be syncrhonized
         	{
				waitDialog.Stop();
				waitDialog.Destroy();
				if (transferType == ResponsiveEnum.Read)
				{
					this.PopulateGUI(); // FIXME: this could freeze, it is not resonsible
				}
				parentWindow.Present();
			}
			else
			{
				warningDialog = new WarningDialog();
         		warningDialog.Message = "Connection error";
         		warningDialog.QuitOnOk = false;
            	warningDialog.Present();
         	}		
         	if (this.transferCompleteEventHandler != null)
         	{
         		transferCompleteEventHandler(this, null);
         	}
		}
		
		/*public void OnTransferStop(object sender, EventArgs e)
		{
			threadNotify = new ThreadNotify (new ReadyEvent (TransferCompleted));
			threadNotify.WakeupMain();
		}*/
		
		/*public void OnDownloadStop(object sender, EventArgs e)
		{
			threadNotify = new ThreadNotify (new ReadyEvent (DownloadComplete));
			threadNotify.WakeupMain();
		}*/
		
	
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
		/*public override void StartDownload()
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
		}*/
	
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
		/*public void UploadComplete()
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
		}*/
	
		/*public void OnTransferCompleted(object sender, EventArgs e)
		{
			ResponsiveEnum transferType = sender as ResponsiveEnum;
			// fixme: how to notify without jump to another method
			threadNotify = new ThreadNotify (new ReadyEvent (TransferCompleted));
			threadNotify.WakeupMain();
			
		}*/
		

		
		public override void PopulateGUI()
		{
		
		}		
	}
	
}
