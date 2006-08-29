
using System;
using Gtk;
using Glade;

class MainWindow
{
		[Widget] Gtk.Label label1;
		[Widget] Gtk.Button button1;
    [Widget] Gtk.Window window1;
		[Widget] Gtk.VBox vbox1;
		[Widget] Gtk.HBox hbox1;
		[Widget] Gtk.HBox hbox4;
		[Widget] Gtk.Notebook notebook2;
		[Widget] Gtk.Notebook notebook3;
		
		void on_Boxerp_destroy(object o, EventArgs e)
		{
				Application.Quit();
		}
		void on_button1_clicked(object o, EventArgs e)
		{
				if (hbox4.Children.Length > 0) 
					hbox4.Remove(hbox4.Children[0]);
				hbox4.Child = notebook2;
		}
		void on_button2_clicked(object o, EventArgs e)
		{
			if (hbox4.Children.Length > 0) 
				hbox4.Remove(hbox4.Children[0]);
			hbox4.Child = notebook3;
		}
		
		public MainWindow()
		{
			Glade.XML maingui = new Glade.XML ("./ventana/ventana.glade", "window1", "");
			maingui.Autoconnect(this);
			Glade.XML panel1 = new Glade.XML ("./ventana/ventana.glade", "notebook2", "");
			panel1.Autoconnect(this);
			Glade.XML panel2 = new Glade.XML ("./ventana/ventana.glade", "notebook3", "");
			panel2.Autoconnect(this);
			window1.Maximize();
		}

}


class test 
{
		public static void Main()
		{
			Gtk.Application.Init();
			MainWindow w = new MainWindow();
			Gtk.Application.Run();
		}
}

				
