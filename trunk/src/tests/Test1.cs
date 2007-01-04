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
    [TestFixtureSetUp()]
    public void Initialize()
    {
        RemotingConfiguration.Configure("./clientRemoting.config");
    }
    
	[Test]
	public void GetUsersWithoutLogin()
	{
		IAdmin adminObj = 
			(IAdmin) RemotingHelper.GetObject(typeof(IAdmin));
 
        try
        {
		    adminObj.GetUsers();
		}
		catch (Boxerp.Exceptions.UnauthorizedException)
		{
		    Assert.IsNull(null);
		}	
	}				  
}
