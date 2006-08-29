// project created on 28/08/2006 at 0:04
using System;
using Gtk;

namespace clientgtksharp
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