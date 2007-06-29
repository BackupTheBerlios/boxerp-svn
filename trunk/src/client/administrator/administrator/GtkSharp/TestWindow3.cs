// /home/carlos/boxerp_completo/trunk/src/client/administrator/administrator/GtkSharp/TestWindow3.cs created with MonoDevelop
// User: carlos at 7:12 AM 6/29/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

#if GTK

using System;
using Admin.Controllers;
using Admin.Interfaces;
using Boxerp.Client.GtkSharp;
using Boxerp.Client;
namespace Admin
{
	
	
	public partial class TestWindow3 : Gtk.Window, ITestWindow
	{
		private TestController _controller;
		
		public TestWindow3() : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			_controller = new TestController(new GtkResponsiveHelper(ConcurrencyMode.Parallel), this);
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