
using System;
using Gtk;
using System.Threading;
using Boxerp.Models;
using Boxerp.Objects;
using System.Runtime.Remoting;
using Boxerp.Client.GtkSharp.Lib;
using Boxerp.Client;

namespace administrator
{
	
	public class LoginWindow : Gtk.Window
	{
		protected Gtk.Button buttonConnect;
		protected Gtk.Entry entryLogin;
		protected Gtk.Entry entryPassword;
		protected WaitWindow waitWindow;
		public  delegate void LoginEventHandler();
		public  event LoginEventHandler loginEvent;
		
		protected ThreadStart delegateLogin; 
		protected Thread threadLogin; 
		static ThreadNotify notify;
		protected bool logon = false;
		public bool connectionFailure = false;
		protected ILogin loginObj;
		protected string session;
		
		
		public LoginWindow() : 
				base("")
		{
			Stetic.Gui.Build(this, typeof(administrator.LoginWindow));
			delegateLogin = new ThreadStart (Login);
			threadLogin  = new Thread(delegateLogin);
			notify = new ThreadNotify (new ReadyEvent (Logged));
		}
	
		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			a.RetVal = true;
			Application.Quit ();
		}

		protected virtual void OnConnect(object sender, System.EventArgs e)
		{
			
			if ((entryLogin.Text.Length > 0) && (entryPassword.Text.Length > 0))
			{
				waitWindow = new WaitWindow();
				waitWindow.Message = "Registering user and password: Please wait...";
				this.Hide();
				threadLogin.Start();
			}
		}
	
		protected void Login()
		{
			try
			{
				UserInformation.SetUserName(entryLogin.Text);
				loginObj = (ILogin) RemotingHelper.GetObject(typeof(ILogin));
				int code = loginObj.Login(entryLogin.Text, entryPassword.Text);
				if (code == 0)
				{
					logon = true;
					SessionSingleton.GetInstance().SetSession(UserInformation.GetSessionToken());
				}
			}
			catch (System.Net.WebException we)
			{
				connectionFailure = true;
				Console.WriteLine("Excepction: " + we.Message);
			}
			catch (Exception e)
			{
				logon = false;
				Console.WriteLine("Exception: " + e.Message);
			}
			notify.WakeupMain();
		}
	
		protected void Logged()
		{
			if (logon)
			{
				waitWindow.Hide();
				waitWindow.Stop();
				waitWindow.Destroy();
				loginEvent();
			}
			else
			{
				waitWindow.Hide();
				waitWindow.Stop();
				waitWindow.Destroy();
				WarningDialog wd = new WarningDialog();
				if (!connectionFailure)
				{
					Console.WriteLine("Login incorrect");
					wd.Message = "Login incorrect";
					wd.QuitOnOk = true;
				}
				else
				{
					Console.WriteLine("Connection failure");
					wd.Message = "Server connection failure";
					wd.QuitOnOk = true;
				}
				wd.Present();
			}
		}
 	}
}
