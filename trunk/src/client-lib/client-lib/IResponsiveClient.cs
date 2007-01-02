
using System;
using System.Reflection;

namespace Boxerp.Client
{
	
	public interface IResponsiveClient
	{
		bool CancelRequest { get; set;}
		void StartTransfer(ResponsiveEnum trType);
		void StopTransfer(int threadId, MethodBase MethodBase, object output);
		void StartAsyncCall(SimpleDelegate method);
		
	}
}
