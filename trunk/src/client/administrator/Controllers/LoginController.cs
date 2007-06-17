
using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System.Reflection;
using Boxerp.Models;
using Boxerp.Objects;
using Gtk;
using Boxerp.Client.GtkSharp.Lib;
using Boxerp.Client;

namespace administrator
{
	public partial class LoginController
	{
		
	    protected ILogin loginObj;
		private bool logon;
		private EventHandler loginEvent;
		private string username, password;
		
		public event EventHandler LoginEvent
		{
		    add 
		    {
		        loginEvent += value;
		    }
		    remove
		    {
		        loginEvent -= value;
		    }
		}
		
		public string UserName
		{
		    set { username = value; }
		}
		
		public string Password
		{
		    set { password = value; }
		}
		
		public LoginHelper()
		{
		    loginObj = (ILogin) RemotingHelper.GetObject(typeof(ILogin));
		    base.Init();
		}
		
		[Responsive(ResponsiveEnum.Read)]
		protected void Login()
		{
			try
			{
			    
			    Console.WriteLine("login!");
				UserInformation.SetUserName(username);
				int code = loginObj.Login(username, password);
				if (code == 0)
				{
					logon = true;
					SessionSingleton.GetInstance().SetSession(UserInformation.GetSessionToken());
				}
			}
			catch (ThreadAbortException ex)
			{
			    logon = false;
			    OnAbortRemoteCall(ex.StackTrace);
			}
			catch (System.Net.WebException we)
			{
			    logon = false;
				Console.WriteLine("Excepction: " + we.Message);
				OnRemoteException("Network failure:" + we.Message);
			}
			catch (Exception e)
			{
				logon = false;
				Console.WriteLine("Exception: " + e.Message);
				OnRemoteException(e.Message);
			}
			finally
			{
			    StopTransfer(Thread.CurrentThread.ManagedThreadId, MethodInfo.GetCurrentMethod(), logon);
			}
		}
		
				public override void PopulateGUI()
		{
		    if (logon)
			{
			    if (loginEvent != null)
			    {
			        loginEvent(this, null);
				}
			}
			else
			{
			    WarningDialog wd = new WarningDialog();
			    wd.Message = "Login incorrect";
			    wd.Run();
				Gtk.Application.Quit();
			}
		}

	}
}
