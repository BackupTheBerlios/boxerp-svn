
using System;

namespace Boxerp.Client.GtkSharp
{
	
	public class InfoExtendedDialog : Gtk.Dialog
	{
		
		public InfoExtendedDialog()
		{
			Stetic.Gui.Build(this, typeof(Boxerp.Client.GtkSharp.InfoExtendedDialog));
		}
	}
	
}
