
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

		public void Init(Gtk.Window win)
		{
			parentWindow = win;
			base.BaseTransferCompleteEvent += this.OnTransferCompleted;
		}
	
		public override void StartTransfer(ResponsiveEnum transferType)
		{
			waitDialog = new WaitDialog(parentWindow); 
			waitDialog.CancelEvent += OnCancel;
			transferSuccess = true;
			base.StartTransfer(transferType);
		}
        
        public void OnRemoteException(string msg)
        {
        	exceptionsMsgPool[Thread.CurrentThread.ManagedThreadId] = msg;
			transferSuccess = false;
        }
        
		public void OnCancel(object sender, EventArgs e)
		{
        	CancelRequest = true;	
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
				parentWindow.Present();
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
         		transferCompleteEventHandler(this, null);
         	}
		}
		
		public void OnTransferCompleted(object sender, EventArgs e)
		{
			Application.Invoke(sender, e, TransferCompleted);
		}
		
		public virtual void PopulateGUI()
		{
		
		}		
	}
}
