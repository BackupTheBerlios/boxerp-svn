
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
		
		public EditUserWindow(User u) :
				base("")
		{
			Stetic.Gui.Build(this, typeof(administrator.EditUserWindow));
			user = u;
			this.PopulateUserFields();
			helper = new EditUserHelper(ref dtreeview);
			dtreeview.LeftLabel = "User groups:";
			dtreeview.RightLabel = "Rest of groups:";
			helper.Init(this);
			helper.StartDownload();
		}
		
		public User User
		{
			get { return user;}
			set 
			{ 
				user = value;
				this.PopulateUserFields();
			}
		}
		
		private void PopulateUserFields()
		{
			entryUserName.Text = user.UserName;
			entryRealName.Text = user.RealName;
			entryEmail.Text = user.Email;
			checkActive.Active = user.Published;
			helper.PopulateUserTreeView(user);
		}

		protected virtual void OnOkClicked(object sender, System.EventArgs e)
		{
			user.UserName = entryUserName.Text;
			user.RealName = entryRealName.Text;
			user.Password = entryPassword.Text;
			user.Email = entryEmail.Text;
			user.Published = checkActive.Active;
			helper.PopulateUserWithGroups(ref user);
			helper.StartUpload();
			
		}

		protected virtual void OnCancelClicked(object sender, System.EventArgs e)
		{
			helper.ClearUser();
			user = null;
			this.Hide();
		}
	}
	
}
