
using System;

namespace lib
{
	
	public class InfoExtendedDialog : Gtk.Dialog
	{
		
		public InfoExtendedDialog()
		{
			Stetic.Gui.Build(this, typeof(lib.InfoExtendedDialog));
		}
	}
	
}
