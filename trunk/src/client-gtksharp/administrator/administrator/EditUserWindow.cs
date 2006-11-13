
using System;
using Boxerp.Models;

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
		protected User user;
		protected Gtk.CheckButton checkActive;
		
		// Constructor: To create a new user
		public EditUserWindow() : 
				base("")
		{
			Stetic.Gui.Build(this, typeof(administrator.EditUserWindow));
			helper = new EditUserHelper(this, ref dtreeview);
			dtreeview.LeftLabel = "User groups:";
			dtreeview.RightLabel = "Rest of groups:";
			helper.StartDownload();
		}
		
		// Constructor: To edit an existing user
		public EditUserWindow(User u) :
				base("")
		{
			Stetic.Gui.Build(this, typeof(administrator.EditUserWindow));
			user = u;
			this.PopulateUserFields();
			helper = new EditUserHelper(this, ref dtreeview, user);
			dtreeview.LeftLabel = "User groups:";
			dtreeview.RightLabel = "Rest of groups:";
			helper.StartDownload();
		}
		
		private void PopulateUserFields()
		{
			entryUserName.Text = user.UserName;
			entryRealName.Text = user.RealName;
			entryEmail.Text = user.Email;
			checkActive.Active = user.Active;
		}

		protected virtual void OnOkClicked(object sender, System.EventArgs e)
		{
			helper.PopulateUser(entryUserName.Text, entryRealName.Text,
							entryPassword.Text, entryEmail.Text, checkActive.Active);
			helper.StartUpload();
		}

		protected virtual void OnCancelClicked(object sender, System.EventArgs e)
		{
			helper.ClearUser();
			user = null;
			this.Destroy();
		}
	}
	
}
