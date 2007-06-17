
using System;
using Boxerp.Models;
using Boxerp.Client.GtkSharp.Lib;
using Boxerp.Client;

namespace administrator
{
	
	public class EditUserWindow : Gtk.Window
	{
		protected DoubleListView dtreeview;
		protected Gtk.Entry entryPassword;
		protected Gtk.Entry entryEmail;
		protected Gtk.Entry entryRealName;
		protected Gtk.Entry entryUserName;
		protected EditUserHelper _helper;
		protected Gtk.CheckButton checkActive;
		protected Gtk.Notebook notebook1;
		
		public event ThreadEventHandler SaveSucessEvent
		{
		    add 
		    {
		        _helper.TransferCompleteEvent += value;
		    }
		    remove
		    {
		        _helper.TransferCompleteEvent -= value;
		    }
		}
		
		public User User 
		{
		    get { return _helper.User; }
		    set { _helper.User = value; }
		}
		
		public bool IsNewUser
		{
		    get { return _helper.IsNewUser; }
		    set { _helper.IsNewUser = value; }
		}
		
		// Constructor: To create a new user
		// FIXME: get groups from client cache instead of passing in to this method
		public EditUserWindow(Group[] groups) : 
				base("")
		{
			Stetic.Gui.Build(this, typeof(administrator.EditUserWindow));
			_helper = new EditUserHelper(this, ref dtreeview);
			IsNewUser = true;
			dtreeview.LeftLabel = "User groups:";
			dtreeview.RightLabel = "Rest of groups:";
			_helper.PopulateGUI(groups);
			//_helper.StartTransfer(ResponsiveEnum.Read);
		}
		
		// Constructor: To edit an existing user
		// FIXME: get groups from client cache instead of passing in to this method
		public EditUserWindow(Group[] groups, User u) :
				base("")
		{
			Stetic.Gui.Build(this, typeof(administrator.EditUserWindow));
			_helper = new EditUserHelper(this, ref dtreeview, u);
			IsNewUser = false;
			this.PopulateUserFields();
			dtreeview.LeftLabel = "User groups:";
			dtreeview.RightLabel = "Rest of groups:";
			_helper.PopulateGUI(groups);
			//_helper.StartTransfer(ResponsiveEnum.Read); 
		}
		
		private void PopulateUserFields()
		{
		    entryUserName.Text = User.UserName;
			entryRealName.Text = User.RealName;
			entryEmail.Text = User.Email;
			checkActive.Active = User.Active;
		}

		protected virtual void OnOkClicked(object sender, System.EventArgs e)
		{
			_helper.PopulateUser(entryUserName.Text, entryRealName.Text,
							entryPassword.Text, entryEmail.Text, checkActive.Active);
			_helper.StartTransfer(ResponsiveEnum.Write); 
		}

		protected virtual void OnCancelClicked(object sender, System.EventArgs e)
		{
			User = null;
			IsNewUser = false;
			this.Destroy();
		}

		protected virtual void OnClose(object o, Gtk.DeleteEventArgs args)
		{
		    OnCancelClicked(o, args);
		    this.Hide();
		    args.RetVal = true;
		    
		}
		
		/*public void Clear()
		{
		    entryUserName.Text = String.Empty;
			entryRealName.Text = String.Empty;
			entryEmail.Text = String.Empty;
			entryPassword.Text = String.Empty;
			checkActive.Active = false;
			dtreeview.Clear();
			User = null;
			IsNewUser = true;
		}
		
		public void Reload()
		{
		    _helper.StartTransfer(ResponsiveEnum.Read);
		}*/
	}
	
}
