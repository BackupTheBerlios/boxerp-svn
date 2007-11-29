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
namespace mvcSample1
{
	public class TestUsersListView : AbstractTestView<UsersListController, TestUsersListData, IUsersListData>, IUsersListView
	{
		private Group _selectedGroup;
		private User _selectedUser;

		public TestUsersListView()
		{
			CreateData();
		}
		
		protected override void CreateData()
		{
			_data = new TestUsersListData(this);
		}
		
		public IUsersListData SharedData 
		{	
			get 
			{
				return Data;
			}
		}

		public Group SelectedGroup 
		{
			get 
			{
				return _selectedGroup;
			}
			set
			{
				_selectedGroup = value;
			}
		}

		public User SelectedUser 
		{
			get 
			{
				return _selectedUser;
			}
			set
			{
				_selectedUser = value;
			}
		}

		public void UpdateUsers()
		{
			foreach (User user in SharedData.Users)
			{
				Console.Out.WriteLine("Reading User:" + user.Username);
			}
		}

		public void UpdateGroups()
		{
			foreach (Group group in SharedData.Groups)
			{
				Console.Out.WriteLine("Reading Group:" + group.Name);
			}
		}
		
		public void OnSelectionChanged ()
		{
			Controller.RetrieveUsers(_selectedGroup);
		}

		public void OnDeleteUser ()
		{
			Controller.DeleteUser(_selectedUser);
		}

		public void OnEditUser ()
		{
			Controller.EditUser(_selectedUser);
		}

		public void OnAddUser ()
		{
			Controller.AddUser(new User(), _selectedGroup);
		}

		public IUserEditView GetUserEditView ()
		{
			TestUserEditView view = new TestUserEditView();
			return view;
		}
	}
}
