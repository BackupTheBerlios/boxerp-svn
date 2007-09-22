// /home/carlos/boxerp_completo/trunk/src/client/testApp2/testApp2/Main.cs created with MonoDevelop
// User: carlos at 2:40 PM 7/7/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
// project created on 7/7/2007 at 2:40 PM
using System;
using Gtk;

namespace testApp2
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