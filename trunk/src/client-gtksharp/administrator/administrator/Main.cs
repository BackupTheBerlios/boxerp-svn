// project created on 11/10/2006 at 16:50
// project created on 11/10/2006 at 15:35
using System;
using Gtk;
using System.Runtime.Remoting;
using Boxerp;
using Boxerp.Models;
using Boxerp.Objects;
using System.Threading;
using clientlib;

namespace administrator
{
	class MainClass
	{
		LoginWindow loginWindow;
		MainWindow mainWindow;
		public void LoginSuccess()
		{
			//string session = loginWindow.Session;
			loginWindow.Destroy();
			Console.WriteLine("Login success");
			string session = SessionSingleton.GetInstance().GetSession();
			Console.WriteLine(session);
			mainWindow = new MainWindow();
			mainWindow.Present();
		}

		
		public static void Main (string[] args)
		{
			Application.Init ();
			RemotingConfiguration.Configure("./clientRemoting.config");
			MainClass main = new MainClass();
			main.loginWindow = new LoginWindow ();
         	main.loginWindow.loginEvent += new LoginWindow.LoginEventHandler(main.LoginSuccess);
         	main.loginWindow.Show ();
			Application.Run ();
		}
	}
}