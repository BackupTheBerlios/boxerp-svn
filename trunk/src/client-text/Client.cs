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
   
	  	User[] users;
	
		if (args.Length != 2)
		{
			Console.WriteLine("Usage: ./cliente.exe username password");
			return;
		}
		Console.WriteLine(args[0]+","+args[1]);

		UserInformation.SetUserName(args[0]);
		if (args[1] == "login")
		{
			if (loginObj.Login(args[0], args[1]) == 0)
			{
				users = adminObj.GetUsers();
				foreach (User u in users)
				{
					Console.WriteLine("Ok!   user:" + u.UserName);
					u.RealName = "test";
					adminObj.SaveUser(u);
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
