
using System;

namespace Boxerp.Client
{
	
	
	public interface IResponsiveCommons
	{
		void PopulateGUI();
		void OnCancel(object sender, EventArgs e);
		void OnRemoteException(string msg);
		void OnTransferCompleted(object sender, ThreadEventArgs e);
		void OnAsyncCallStop(object sender, ThreadEventArgs teargs);
		event ThreadEventHandler TransferCompleteEvent;
		// TODO put the completed event here
	}
}
