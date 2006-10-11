
using System;

namespace widgets
{
	
	public class InfoExtendedDialog : Gtk.Dialog
	{
		
		public InfoExtendedDialog()
		{
			Stetic.Gui.Build(this, typeof(widgets.InfoExtendedDialog));
		}
	}
	
}
