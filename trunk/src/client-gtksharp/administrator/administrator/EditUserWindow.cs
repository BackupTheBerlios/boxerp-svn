
using System;

namespace administrator
{
	
	public class EditUserWindow : Gtk.Window
	{
		protected widgets.DoubleListView dtreeview;
		protected Gtk.Entry entryPassword;
		protected Gtk.Entry entryEmail;
		protected Gtk.Entry entryRealName;
		protected Gtk.Entry entryUserName;
		protected EditUserHelper helper;

		
		public EditUserWindow() : 
				base("")
		{
			Stetic.Gui.Build(this, typeof(administrator.EditUserWindow));
			helper = new EditUserHelper(ref dtreeview);
			dtreeview.LeftLabel = "User groups:";
			dtreeview.RightLabel = "Rest of groups:";
			helper.Init(this);
			helper.StartDownload();
		}
	}
	
}
