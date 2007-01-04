
using System;
using System.Collections;
using System.Threading;
using System.Reflection;
using Gtk;

namespace Boxerp.Client.GtkSharp.Lib
{
	public abstract class GtkResponsiveHelper : AbstractResponsiveHelper, IResponsiveCommons
	{
		WaitDialog waitDialog;
		WarningDialog warningDialog;
		private static Hashtable exceptionsMsgPool = Hashtable.Synchronized(new Hashtable());
		protected bool transferSuccess;
		protected Gtk.Window parentWindow = null;
		
		// Just to notify clients when the transfer is completed
		private ThreadEventHandler transferCompleteEventHandler;
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

        /// <summary>
        /// Pass in the parent window if you want it to present after the 
        // asyncrhonous operation
        /// </summary>
		public void Init(Gtk.Window win)
		{
			parentWindow = win;
            Init();
		}
		
		/// <summary>
		/// Use this method when the parent window and this helper gonna be 
		/// destroyed at the end of the async operation
		/// <summary>
		public void Init()
		{
			base.BaseTransferCompleteEvent += this.OnTransferCompleted;		    
		}
	
		public override void StartTransfer(ResponsiveEnum transferType)
		{
		    if (parentWindow != null)
		    {
			    waitDialog = new WaitDialog(parentWindow);
			}
			else
			{
			    waitDialog = new WaitDialog();
			}
			waitDialog.CancelEvent += OnCancel;
			transferSuccess = true;
			base.StartTransfer(transferType);
		}
		
		public override void StartAsyncCall(SimpleDelegate method)
		{
		    if (parentWindow != null)
		    {
			    waitDialog = new WaitDialog(parentWindow);
			}
			else
			{
			    waitDialog = new WaitDialog();
			}
			waitDialog.CancelEvent += OnCancel;
			transferSuccess = true;
			base.StartAsyncCall(method);
		}
		
        
        public void OnRemoteException(string msg)
        {
        	exceptionsMsgPool[Thread.CurrentThread.ManagedThreadId] = msg;
			transferSuccess = false;
        }
        
        public void OnAbortRemoteCall(string stacktrace)
        {
            string message = "Operation stopped.";
            if ((stacktrace.IndexOf("WebAsyncResult.WaitUntilComplete") > 0) || (stacktrace.IndexOf("WebConnection.EndWrite") > 0))
	        {
	            message += "Warning!, the operation seems to have been succeded at the server side";
                transferSuccess = true;
                exceptionsMsgPool[Thread.CurrentThread.ManagedThreadId] = message;    
	        }
	        else
	        {
	            transferSuccess = false;           
            }
        }
        
		public void OnCancel(object sender, EventArgs e)
		{
        	CancelRequest = true;
        	QuestionDialog qdialog = new QuestionDialog();
        	qdialog.Message = "The process is being cancelled, please wait. Do you want to force abort right now?";
            int rtype = qdialog.Run();
            if (rtype == (int)ResponseType.Yes)
            {
                ForceAbort();      
            }
            
        	//TODO: Show a dialog :" Please wait while cancelling" with
        	//a button to force cancelation by aborting threads
		}
		
		private void TransferCompleted(object sender, EventArgs e)
		{
			ResponsiveEnum transferType = (ResponsiveEnum)sender;
			waitDialog.Stop();
			waitDialog.Destroy();
			if (transferSuccess) // FIXME: transferSuccess must be syncrhonized
         	{
				if (transferType == ResponsiveEnum.Read)
				{
					this.PopulateGUI(); // FIXME: this could freeze, it is not resonsible
				}
				else
				{
				    InfoDialog idialog = new InfoDialog();
				    idialog.Message = "Operation Sucess";
				    idialog.Present();
				}
				if (parentWindow != null)
				{
				    parentWindow.Present();
				}
			}
			else
			{
				warningDialog = new WarningDialog();
				string msg = "";
				foreach (string i in exceptionsMsgPool.Values)
					msg += i + "\n";
         		warningDialog.Message = msg;
         		warningDialog.QuitOnOk = false;
            	warningDialog.Present();
         	}		
         	if (this.transferCompleteEventHandler != null)
         	{
         	    transferCompleteEventHandler(sender, (ThreadEventArgs)e);
         	}
            OnAsyncCallStop(sender, (ThreadEventArgs)e);
		}
		
		public void OnTransferCompleted(object sender, ThreadEventArgs e)
		{
			Application.Invoke(sender, e, TransferCompleted);
		}
		
		public virtual void PopulateGUI(){}
		
		public virtual void OnAsyncCallStop(object sender, ThreadEventArgs teargs){}
	}
}
