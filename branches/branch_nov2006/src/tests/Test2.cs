using System;
using System.Runtime.Remoting;
using Boxerp;
using Boxerp.Models;
using Boxerp.Objects;
using System.Threading;
using System.Security.Principal;

using NUnit.Framework;

[TestFixture]
public class Test2
{
	User[] users;
	Group[] groups;
	ILogin loginObj = 
			(ILogin) RemotingHelper.GetObject(typeof(ILogin));
	IAdmin adminObj = 
			(IAdmin) RemotingHelper.GetObject(typeof(IAdmin));
	
	[Test]
	public void GetUsers()
	{
		UserInformation.SetUserName("demo");
		if (loginObj.Login("demo", "pass") == 0)
		{
		    Boxerp.Client.SessionSingleton.GetInstance().SetSession(UserInformation.GetSessionToken());
			users = adminObj.GetUsers();
			Assert.IsNotNull(users);	
		}
		else
			Assert.Fail("Login failed");
	}

    [Test]
    public void TrySendIdentity()
    {
        
            GenericIdentity gident = new GenericIdentity("Testing");
            GenericPrincipal gprincipal = new GenericPrincipal(gident, null);
            Thread.CurrentPrincipal = gprincipal;
            Console.WriteLine("IDentity="+ Thread.CurrentPrincipal.Identity.Name);
            loginObj.ReadIdentity();
        
    }
    
	[Test]
	public void GetGroups()
	{
		groups = adminObj.GetGroups();
		Assert.IsNotNull(groups);	
	}

	
	/*[Test]
	public void SaveUsers()
	{
		Assert.IsNotNull(users);
		foreach (User u in users)
		{
			Console.WriteLine("Ok!   user:" + u.UserName);
			u.RealName = "test";
			u.Email = "test@testing.com";
			u.Groups.Add(groups[0]);
			Assert.AreEqual(adminObj.SaveUser(u), 0);
		}
		
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message + " : " + ex.StackTrace);
			if (ex.InnerException != null)
				Console.WriteLine(ex.InnerException.Message + " : " + ex.InnerException.StackTrace);
		}
	}*/	
}
