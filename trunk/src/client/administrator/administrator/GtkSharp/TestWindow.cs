// /home/carlos/boxerp_completo/trunk/src/client/administrator/administrator/TestWindow.cs created with MonoDevelop
// User: carlos at 10:01 PM 6/23/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Admin.Interfaces;
using Admin.Controllers;
using Boxerp.Client;
using Boxerp.Client.GtkSharp;

namespace Admin
{
	
	
	public partial class TestWindow : Gtk.Window, ITestWindow
	{
		TestController _controller;
		
		public TestWindow() : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			_controller = new TestController(new GtkResponsiveHelper(ConcurrencyMode.Modal), this);
		}

		protected virtual void OnClicked (object sender, System.EventArgs e)
		{
			_controller.RunMethod();
		}
		
		public void ShowSomething()
		{
			InfoDialog iDialog = new InfoDialog();
			iDialog.Message = "Operation success";
			iDialog.Present();
		}

		protected virtual void OnDeleteEvent (object o, Gtk.DeleteEventArgs args)
		{
			Gtk.Application.Quit();
		}
	}
}
