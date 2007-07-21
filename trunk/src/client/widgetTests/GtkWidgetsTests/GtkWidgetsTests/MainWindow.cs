// /home/carlos/boxerp_completo/trunk/src/client/widgetTests/GtkWidgetsTests/GtkWidgetsTests/MainWindow.cs created with MonoDevelop
// User: carlos at 11:01 PM 7/21/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using Gtk;
using Boxerp.Client.GtkSharp.Controls;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		vbox1.PackEnd(new IntegerTextBoxControl(), true, true, 0);
		vbox1.PackEnd(new IntegerTextBoxControl(), true, true, 0);
		vbox1.PackEnd(new IntegerTextBoxControl(), true, true, 0);
		vbox1.PackEnd(new IntegerTextBoxControl(), true, true, 0);
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}