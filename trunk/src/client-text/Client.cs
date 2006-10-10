using System;
using System.Runtime.Remoting;
using Boxerp;
using Boxerp.Models;
using Boxerp.Objects;
using System.Threading;

class Client
{
	static void Main(string[] args)
	{
		// USAGE: ./client.exe username login|nologin
		RemotingConfiguration.Configure("./clientRemoting.config");

		ILogin loginObj = 
			(ILogin) RemotingHelper.GetObject(typeof(ILogin));
		IAdmin adminObj = 
			(IAdmin) RemotingHelper.GetObject(typeof(IAdmin));
    
		UserInformation.SetUserName(args[0]);
		if (args[1] == "login")
		{
			if (loginObj.Login(args[0], "pass") == 0)
			{
				foreach (User u in adminObj.GetUsers())
				{
					Console.WriteLine("Ok!   user:" + u.UserName);
				}
			}
			else
				Console.WriteLine("Login incorrect");
		}
		else	// try use adminObj withou login
		{
			if (adminObj.GetUsers() == null)
			{
				Console.WriteLine("Permission denied");
			}
		}				  
	}	
}
