using System;
using System.Runtime.Remoting;
//using System.Runtime.Remoting.Channels;
//using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;

namespace Boxerp.Client
{
	
	public class SessionSingleton
	{
		private static SessionSingleton instance = null;
		private string sessionToken;

		public SessionSingleton(){}
		
		public static SessionSingleton GetInstance()
		{
			if (instance == null)
			{
				instance = new SessionSingleton();
			}
			return instance;
		}

		public void SetSession(string session)
		{
			sessionToken = session;
		}
		
		public string GetSession()
		{
			return sessionToken;
		}
		
		/*public void SendSessionToken()
		{
			
		}*/		
	}	
}
