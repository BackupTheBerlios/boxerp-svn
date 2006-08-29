
using System;

namespace clientgtksharp
{
	
	public class LoginWindow : Gtk.Window
	{
		
		public LoginWindow() : 
				base("")
		{
			Stetic.Gui.Build(this, typeof(clientgtksharp.LoginWindow));
		}
	}
	
}
