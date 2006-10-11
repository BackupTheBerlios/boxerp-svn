
using System;

namespace widgets
{
	
	public class WarningDialog : Gtk.Dialog
	{
		
		public WarningDialog()
		{
			Stetic.Gui.Build(this, typeof(widgets.WarningDialog));
		}
	}
	
}
