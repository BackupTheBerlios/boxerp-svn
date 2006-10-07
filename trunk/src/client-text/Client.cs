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
    RemotingConfiguration.Configure("./clientRemoting.config");

    ILogin loginObj = 
      (ILogin) RemotingHelper.GetObject(typeof(ILogin));
    IAdmin adminObj = 
      (IAdmin) RemotingHelper.GetObject(typeof(IAdmin));
    
	 loginObj.Login("demo", "pass");	 
	 UserInformation.SetUser(args[0]);
	 Console.WriteLine("get user");
	 while (true)
    {
		 adminObj.GetUser("asdf", "asdfff");
		 Console.WriteLine("admin obj");
		 Thread.Sleep(3000);
    }
	 /*User nu = new User();
	 nu.UserName = "pruebaasdfsdf";
	 nu.Password = "pass2sdfsdfsf";
    adminObj.SaveUser(nu);*/
	 Console.WriteLine("Done");
  }	
}
