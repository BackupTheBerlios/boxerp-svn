// project created on 24/11/2007 at 10:01
using System;
using Gtk;
using Boxerp.Client;
using Boxerp.Client.GtkSharp;

namespace mvcSample1
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			startFirstController();
			Application.Run ();
		}
		
		public static void startFirstController()
		{
			// Gtk version:
			UsersListView view = new UsersListView();
			UsersListController controller = 
				new UsersListController(new GtkResponsiveHelper(ConcurrencyMode.Modal), view);
			
			MainWindow win = new MainWindow ();
			win.Add(view);
			win.Show ();
			win.ReshowWithInitialSize();
			win.Child.ShowAll();
		}
	}
}