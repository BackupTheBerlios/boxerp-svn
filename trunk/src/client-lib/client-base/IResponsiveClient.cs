
using System;
using System.Reflection;

namespace Boxerp.Client
{
	
	public interface IResponsiveClient
	{
		/* Implemented in the abstract class */
		bool CancelRequest { get; set;}
		void StartAsyncCallList(ResponsiveEnum trType);
		void StopAsyncMethod(int threadId, MethodBase MethodBase, object output);
		void StopAsyncMethod(int threadId, SimpleDelegate method, object output);
		void StartAsyncCall(SimpleDelegate method);

		/* Not implemented in the abstract class */
		void PopulateGUI();
		void OnCancel(object sender, EventArgs e);
		void OnRemoteException(string msg);
		void OnAbortRemoteCall(string msg);
		void OnTransferCompleted(object sender, ThreadEventArgs e);
		void OnAsyncCallStop(object sender, ThreadEventArgs teargs);
		event ThreadEventHandler TransferCompleteEvent;
	}
}
