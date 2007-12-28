// project created on 28/12/2007 at 14:41
using System;
using Gtk;

namespace gtkResponsivenessTest
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			UpdateWaitMsgTest win2 = new UpdateWaitMsgTest();
			win2.Show();
			Application.Run ();
		}
	}
}