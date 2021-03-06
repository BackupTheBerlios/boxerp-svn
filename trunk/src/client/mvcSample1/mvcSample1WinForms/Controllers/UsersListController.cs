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

namespace mvcSample1WinForms
{
		
	public class UsersListController : AbstractController<IUsersListView, IUsersListData, UsersListController>
	{
		public UsersListController(IResponsiveClient helper, IUsersListView view)
			: base (helper, view)
		{}
		
		public void RetrieveGroups()
		{
			if (SharedData == null)
			{
				Console.Out.WriteLine("wrong!");
			}
			SharedData.Groups = Database.GetAllGroups();
			View.UpdateGroups();
		}
		
		public void RetrieveUsers(Group group)
		{
			SharedData.Users = Database.GetUsers(group);
			int count = 0;
			foreach (User user in SharedData.Users)
			{
				count = user.IsActive ? count +1 : count;
			}
            View.ActiveUsers = count;
			
			View.UpdateUsers();
		}
		
		public void DeleteUser(User user)
		{
			SharedData.Users.Remove(user);
		}

		public void EditUser(User user)
		{
			UserEditController controller = getUserEditController();
			controller.PopulateGui(user);
			ViewsManager.DisplayView(controller.View);
		}

		public void AddUser(User user, Group group)
		{
			UserEditController controller = getUserEditController();
			User newUser = new User();
			newUser.Group = group;
			controller.PopulateGui(newUser);
			ViewsManager.DisplayView(controller.View);
		}

		private UserEditController getUserEditController()
		{
			IUserEditView view = View.GetUserEditView();
			UserEditController controller = 
				new UserEditController(
				    ResponsiveHelperFactory.GetRunningToolkitTypeHelper(
				                            ConcurrencyMode.Modal), 
				    view);
			return controller;
		}

        public void OnGroupSelectionChanged(Group group)
        {
            RetrieveUsers(group);
        }

		protected override  void OnAsyncOperationFinish(Object sender, ThreadEventArgs args)
		{
			
		}
	}
}
