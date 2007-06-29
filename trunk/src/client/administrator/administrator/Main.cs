// project created on 11/10/2006 at 16:50
// project created on 11/10/2006 at 15:35
using System;
using Gtk;
using System.Runtime.Remoting;
using Boxerp;
using Boxerp.Models;
using Boxerp.Objects;
using System.Threading;
using Boxerp.Client;

namespace Admin
{
	class MainClass
	{		
		public static void Main (string[] args)
		{
			
#if GTK
			Application.Init ();
			TestWindow test = new TestWindow();
			test.Show();
			TestWindow2 test2 = new TestWindow2();
			test2.Show();
			TestWindow3 test3 = new TestWindow3();
			test3.Show();
			Application.Run();
#elif WPF

			
#endif
			

		}
	}
}