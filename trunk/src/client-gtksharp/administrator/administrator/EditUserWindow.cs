
using System;

namespace administrator
{
	
	public class EditUserWindow : Gtk.Window
	{
		protected widgets.DoubleTreeView dtreeview;
		protected Gtk.Entry entryPassword;
		protected Gtk.Entry entryEmail;
		protected Gtk.Entry entryRealName;
		protected Gtk.Entry entryUserName;
		protected EditUserHelper helper;

		
		public EditUserWindow() : 
				base("")
		{
			Stetic.Gui.Build(this, typeof(administrator.EditUserWindow));
			helper = new EditUserHelper(ref dtreeview.streeviewLeft,
									ref dtreeview.streeviewRight);
			helper.Init(this);
			helper.StartDownload();
		}
	}
	
}
