// project created on 24/11/2007 at 10:01
using System;
using Gtk;

namespace mvcSample1
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