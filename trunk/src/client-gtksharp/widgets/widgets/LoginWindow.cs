
using System;
using Gtk;
using System.Threading;
using Boxerp.Models;
using Boxerp.Objects;

namespace widgets
{
	
	public class LoginWindow : Gtk.Window
	{
		protected Gtk.Button buttonConnect;
		protected Gtk.Entry entryServer;
		protected Gtk.Entry entryUser;
		protected Gtk.Entry entryPass;
		protected WaitWindow waitWindow;
		public  delegate void LoginEventHandler();
		public  event LoginEventHandler loginEvent;
		
		protected ThreadStart delegateLogin; 
		protected Thread threadLogin; 
		static ThreadNotify notify;
		protected bool logon = false;
		protected string session = null;
		public bool connectionFailure = false;
		protected ILogin loginObj;
		
		public LoginWindow() : 
				base("")
		{
			Stetic.Gui.Build(this, typeof(widgets.LoginWindow));
			delegateLogin = new ThreadStart (Login);
			threadLogin  = new Thread(delegateLogin);
			notify = new ThreadNotify (new ReadyEvent (Logged));
		}
	
	
		public string Session
		{
			get { return session;}
			set { session = value;}
		}
	
		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}

		protected virtual void OnConnect(object sender, System.EventArgs e)
		{
			if ((entryUser.Text.Length > 0) && (entryPass.Text.Length > 0))
			{
				waitWindow = new WaitWindow();
				waitWindow.SetMsg("Registering user and password: Please wait...");
				this.Hide();
				threadLogin.Start();
			}
		}
	
		protected void Login()
		{
			try
			{
				loginObj = (ILogin) RemotingHelper.GetObject(typeof(ILogin));
				session = loginObj.Login(entryUser.Text, entryPass.Text);
				if (session != null)
					logon = true;
				
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
					//wd. a.textviewDesc.Buffer.Text = "Nombre de Usuario o Contraseña erróneos";
					//a.labelMsg.Text = "Error: Acceso Denegado";
				}
				wd.Present();
			}
		}
 	}
}
