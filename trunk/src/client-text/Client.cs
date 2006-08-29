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
    User u = adminObj.GetUser("demo", "pass");	 
    Console.WriteLine(u.UserName);
	 User nu = new User();
	 nu.UserName = "prueba";
	 nu.Password = "pass2";
    adminObj.SaveUser(nu);
	 Console.WriteLine("Done");
  }	
}
