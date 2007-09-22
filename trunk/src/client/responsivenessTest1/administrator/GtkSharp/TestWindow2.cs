// /home/carlos/boxerp_completo/trunk/src/client/administrator/administrator/GtkSharp/TestWindow2.cs created with MonoDevelop
// User: carlos at 6:54 AM 6/29/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

#if GTK

using System;
using Admin.Interfaces;
using Admin.Controllers;
using Boxerp.Client.GtkSharp;
using Boxerp.Client;

namespace Admin
{
	
	
	public partial class TestWindow2 : Gtk.Window, ITestWindow
	{
		private TestController _controller;
		
		public TestWindow2() : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			_controller = new TestController(new GtkResponsiveHelper(ConcurrencyMode.SingletonThread), this);
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

#endif