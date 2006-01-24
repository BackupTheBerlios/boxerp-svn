
using System;
using Gtk;
using Glade;

class MainWindow
{
		//[Widget] Gtk.Label label1;
		//[Widget] Gtk.Button button1;
      [WidgetAttribute] Gtk.Window window1;
		
		/*void on_window_destroy(object o, EventArgs e)
		{
         Application.Quit();
		}
      void on_button1_clicked(object o, EventArgs e)
		{
         Console.WriteLine("clicked");
         //label1.Text = ((Gtk.Button)o).Label ;

		}*/
      public MainWindow()
		{
         Glade.XML gui = new Glade.XML ("proyecto7.glade", "window1", "");
         gui.Autoconnect(this);
			//window1 = (Gtk.Window)gui.GetWidget("window1");
			window1.Maximize();
			Console.WriteLine(window1.Name);
		}

}


class test 
{

	 public static void Main()
	 {
		try
		{
		  Gtk.Application.Init();
        MainWindow w = new MainWindow();
        Gtk.Application.Run();
		}
		catch (Exception ex)
		{
			Console.WriteLine("Excepcion producida: "+ex.Message + ex.StackTrace);// + ex.InnerException.Message);
		}
	 }

}

				
