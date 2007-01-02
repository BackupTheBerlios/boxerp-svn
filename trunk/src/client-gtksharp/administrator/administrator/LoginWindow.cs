
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
		protected Gtk.Entry entryLogin;
		protected Gtk.Entry entryPassword;
		private LoginHelper _helper;
		
		public event EventHandler LoginEvent
		{
		    add 
		    {
		        _helper.LoginEvent += value;
		    }
		    remove
		    {
		        _helper.LoginEvent -= value;
		    }
		}
		
		public LoginWindow() : 
				base("")
		{
			Stetic.Gui.Build(this, typeof(administrator.LoginWindow));
			_helper = new LoginHelper();
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
			    this.Hide();
			    _helper.UserName = entryLogin.Text;
			    _helper.Password = entryPassword.Text;
			    _helper.StartTransfer(ResponsiveEnum.Read);	
			}
		}
 	}
}
