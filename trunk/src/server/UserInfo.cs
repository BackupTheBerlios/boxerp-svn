// http://dotnetjunkies.com/WebLog/johnwood/archive/2005/01/05/41688.aspx

using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
using System;

[Serializable]
public class UserInformation : ILogicalThreadAffinative
{
	const string CallContextKey = "UserInfo";
	private string userName;
	private string sessionToken;

	public UserInformation(string userName)
	{
		this.userName = userName;
	}

	public UserInformation(string userName, string session)
	{
		this.userName = userName;
		this.sessionToken = session;
	}

	public string UserName
	{
		get { return userName; }
	}

	public string SessionToken
	{
		get { return sessionToken; }
		set { sessionToken = value; }
	}
	
	// SetUserName is called by the client
	public static void SetUserName(string userName)
	{
		CallContext.SetData(CallContextKey, new UserInformation(userName));
	}
	
	// SetSessionToken is called by the server
	public static void SetSessionToken(string session)
	{

		UserInformation ui = (UserInformation)CallContext.GetData(CallContextKey);
		ui.SessionToken = session;	
		Console.WriteLine("ui=" + ui.UserName + ":" + ui.SessionToken);
		CallContext.SetData(CallContextKey, ui);
	}
	
	public static string GetUser()
	{
		return ((UserInformation) CallContext.GetData(CallContextKey)).UserName;
	}

	public static string GetSessionToken()
	{
		return ((UserInformation) CallContext.GetData(CallContextKey)).SessionToken;
	}
}
