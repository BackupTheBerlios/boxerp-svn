
using System;

namespace widgets
{
	
	public class InfoDialog : Gtk.Dialog
	{
		
		public InfoDialog()
		{
			Stetic.Gui.Build(this, typeof(widgets.InfoDialog));
		}
	}
	
}
