// ////
////// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
//////
////// Redistribution and use in source and binary forms, with or
////// without modification, are permitted provided that the following
////// conditions are met:
////// Redistributions of source code must retain the above
////// copyright notice, this list of conditions and the following
////// disclaimer.
////// Redistributions in binary form must reproduce the above
////// copyright notice, this list of conditions and the following
////// disclaimer in the documentation and/or other materials
////// provided with the distribution.
//////
////// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY
////// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
////// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
////// PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR
////// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
////// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
////// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
////// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
////// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
////// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
////// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
////// THE POSSIBILITY OF SUCH DAMAGE.
////
////

using System;
using Boxerp.Client;
using Boxerp.Client.GtkSharp.Controls;

namespace mvcSample1
{
	public partial class UsersListView : Gtk.Bin, IUsersListView
	{
		private UsersListController _controller;
		private GtkUsersListData _data;
		private Boxerp.Client.GtkSharp.Controls.ComboBox _groupsCombo;
		private Boxerp.Client.GtkSharp.Controls.ListView _usersList;
		private bool _isPopulatingCombo = false;
		
		public UsersListController Controller 
		{
			get 
			{
				return _controller;
			}
			set
			{
				_controller = value;
			}
		}

		public IUsersListData SharedData 
		{
			get 
			{
				return _data;
			}
		}

		public ComboBox GroupsCombo {
			get {
				return _groupsCombo;
			}
		}

		public ListView UsersList {
			get {
				return _usersList;
			}
		}
		
		public UsersListView()
		{
			this.Build();
			_data = new GtkUsersListData(this);
			addBoxerpWidgets();
			if (Controller == null)
			{
				Console.Out.WriteLine("facu");
			}
		}

		private void addBoxerpWidgets()
		{
			_groupsCombo = new Boxerp.Client.GtkSharp.Controls.ComboBox();
			_usersList = new ListView();
			
			hbox2.PackStart(_groupsCombo);
			_usersScrollWin.AddWithViewport(_usersList);
			_usersList.ItemsDisplayMode = ItemsDisplayMode.AutoCreateColumns;
			_groupsCombo.SelectionChanged += OnSelectionChanged;
		}
		
		private void OnSelectionChanged(object sender, EventArgs args)
		{
			OnSelectionChanged();
		}
		
		public void OnSelectionChanged()
		{
			if ((!_isPopulatingCombo) && (_groupsCombo.SelectedItem != null))
			{
				Logger.GetInstance().WriteLine("Selection changed");
				Group group = _groupsCombo.SelectedItem as Group;
				Controller.RetrieveUsers(group);
			}
		}
		
		public void OnDeleteUser()
		{
			if (_usersList.SelectedItem != null)
			{
				Controller.DeleteUser(_usersList.SelectedItem as User);
			}			
		}
		
		protected virtual void OnDeleteUser (object sender, System.EventArgs e)
		{
			OnDeleteUser();
		}

		protected virtual void OnEditUser (object sender, System.EventArgs e)
		{
			OnEditUser();
		}

		public void OnEditUser()
		{
			if (_usersList.SelectedItem != null)
			{
				Controller.EditUser(_usersList.SelectedItem as User);
			}			
		}
		
		protected virtual void OnAddUser (object sender, System.EventArgs e)
		{
			OnAddUser();
		}
		
		public void OnAddUser()
		{
			if (_usersList.SelectedItem != null)
			{
				Controller.AddUser(new User(), _groupsCombo.SelectedItem as Group);
			}
		}
		
		public void UpdateGroups()
		{
			_isPopulatingCombo = true;
			_groupsCombo.Items.Clear();
			foreach (Group group in SharedData.Groups)
			{
				_groupsCombo.Items.Add(group);
			}
			_isPopulatingCombo = false;
		}
		
		public void UpdateUsers()
		{
			_activeUsers.Text = SharedData.PropertyBag["ActiveUsers"].ToString();
			_usersList.Items.Clear();
			foreach (User user in SharedData.Users)
			{
				_usersList.Items.Add(user);
			}
		}
		
		public IUserEditView GetUserEditView()
		{
			UserEditView ueView = new UserEditView();
			return ueView;
		}
		
		
	}
}
