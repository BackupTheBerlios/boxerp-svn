using System;
#if GTK
using Gtk;
#endif
using Boxerp;
using Boxerp.Client;

namespace Admin
{
	class MainClass
	{		
#if GTK
		public static void Main (string[] args)
		{
			
			Logger.GetInstance().ShowDebugInfo = true;
			Application.Init ();
			TestWindow test = new TestWindow();
			test.Show();
			TestWindow2 test2 = new TestWindow2();
			test2.Show();
			TestWindow3 test3 = new TestWindow3();
			test3.Show();
			Application.Run();		
		}
#elif WPF


#endif

	}
}