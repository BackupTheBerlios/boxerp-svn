using System;
using System.Runtime.Remoting;
using Boxerp;
using Boxerp.Models;
using Boxerp.Objects;

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
	 /*User nu = new User();
	 nu.UserName = "pruebaasdfsdf";
	 nu.Password = "pass2sdfsdfsf";
    adminObj.SaveUser(nu);*/
	 Console.WriteLine("Done");
  }	
}
