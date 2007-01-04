using System;
using System.Runtime.Remoting;
using System.Threading;
using Boxerp;
using Boxerp.Models;
using Boxerp.Objects;

using NUnit.Framework;

// Test1. Try to access server without login
[TestFixture]
public class Test3
{
    IAdmin adminObj = 
			(IAdmin) RemotingHelper.GetObject(typeof(IAdmin));
	Random random = new Random ();
 
    /*[Test]
	public void AbortWhileSavingUser()
	{
		
        try
        {   // what happen when the remote call is aborted while running?.
            // This code try to gather all possible failures.
            
		    for (int i = 0; i < 1000; i++)
		    {
		        ThreadStart ts = new ThreadStart(this.SaveUser);
		        Thread thread = new Thread(ts);
		        thread.Start();
		        int waitTime = (int)(random.NextDouble() * 100);
		        Console.WriteLine("< {0} <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<", i);
		        Console.WriteLine("WaitTime=" + waitTime.ToString());
		        Thread.Sleep(waitTime);
		        if (thread.IsAlive)
		        {
		            thread.Abort();
		        }
		        thread.Join();
		    }
		}
		catch (Exception ex)
		{
		    Console.WriteLine("EX: " + ex.Message);
		    Assert.IsNotNull(null);
		}	
	}*/		

    // try to save a random username	
	private void SaveUser()
	{
	    User user = null;
	    try
	    {
	        UserInformation.SetSessionToken(Boxerp.Client.SessionSingleton.GetInstance().GetSession());
	        user = new User();
	        //user.Groups = new Iesi.Collections.HashedSet();
	        user.UserName = DateTime.Now.ToLongTimeString() + DateTime.Now.Millisecond.ToString();
	        
	        adminObj.SaveUser(user);
	        Console.WriteLine("no exception");
	    }
	    catch (Exception ex)
	    {
	        Console.WriteLine("Exception ->:"+ ex.GetType().ToString() + ", " + ex.Message +","+ ex.StackTrace);
	        if ((ex.StackTrace.IndexOf("WebAsyncResult.WaitUntilComplete") > 0) || (ex.StackTrace.IndexOf("WebConnection.EndWrite") > 0))
	        {
	            Console.Write("Operation is likely commited at server side:");
	            
	        }
	        else
	        {
	            Console.Write("Operation is likely NOT commited at server side:");
	        }
	        if (user != null)
	        {
	            Console.WriteLine(user.UserName);
	        }
	        else
	        {
	            Console.WriteLine("");
	        }
	        while (ex.InnerException != null)
	        {
	            ex = ex.InnerException;
	            Console.WriteLine("Exception   :"+ ex.Message +","+ ex.StackTrace);
	        }
	        Console.WriteLine("");
	    }
	}
	
}

