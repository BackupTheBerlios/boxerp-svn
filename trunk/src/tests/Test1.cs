using System;
using System.Runtime.Remoting;
using Boxerp;
using Boxerp.Models;
using Boxerp.Objects;
using System.Threading;

using NUnit.Framework;

// Test1. Try to access server without login
[TestFixture]
public class Test1
{
	[Test]
	public void GetUsersWithoutLogin()
	{
		// USAGE: ./client.exe username login|nologin
		RemotingConfiguration.Configure("./clientRemoting.config");

		/*ILogin loginObj = 
			(ILogin) RemotingHelper.GetObject(typeof(ILogin));*/
		IAdmin adminObj = 
			(IAdmin) RemotingHelper.GetObject(typeof(IAdmin));
 
		Assert.IsNull(adminObj.GetUsers()); 	
		Console.WriteLine("Permission denied test passed. OK");
	}				  
}
