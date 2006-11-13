
using System;

namespace lib
{
	
	public class InfoDialog : Gtk.Dialog
	{
		
		public InfoDialog()
		{
			Stetic.Gui.Build(this, typeof(lib.InfoDialog));
		}
	}
	
}
