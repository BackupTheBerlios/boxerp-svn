
using System;

namespace Boxerp.Client
{
	
	public interface IResponsiveClient
	{
		bool CancelRequest { get; set;}
		void StartTransfer(ResponsiveEnum trType);
		void StopTransfer();
		void StartAsyncCall(SimpleDelegate method);
	}
}
