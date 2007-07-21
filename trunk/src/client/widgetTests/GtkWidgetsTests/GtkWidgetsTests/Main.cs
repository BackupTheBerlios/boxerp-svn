// /home/carlos/boxerp_completo/trunk/src/client/widgetTests/GtkWidgetsTests/GtkWidgetsTests/Main.cs created with MonoDevelop
// User: carlos at 11:01 PM 7/21/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
// project created on 7/21/2007 at 11:01 PM
using System;
using Gtk;

namespace GtkWidgetsTests
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}
	}
}