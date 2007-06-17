
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
	
	
	public partial class LoginHelper
	{
		
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
