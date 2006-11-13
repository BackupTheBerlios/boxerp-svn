
using System;

namespace Boxerp.Client.GtkSharp.Lib
{
	
	public class InfoDialog : Gtk.Dialog
	{
		
		public InfoDialog()
		{
			Stetic.Gui.Build(this, typeof(Boxerp.Client.GtkSharp.Lib.InfoDialog));
		}
	}
	
}
