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
using Boxerp.Client.GtkSharp;
using Boxerp.Client.GtkSharp.Controls;

namespace testApp3
{
	
	
	public partial class GroupsWindow : Gtk.Window
	{
		private Group _group;
		private BindableWrapper<Group> _bindableGroup;
		
		public GroupsWindow() : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			
			addUsers();
		}
		
		private void tryDataBinding()
		{
			createGroup();
			_bindableGroup = new BindableWrapper<Group>(_group);			
			Console.WriteLine("users:" + _bindableGroup.Data.BusinessObj.Users.Count);
			_listView.BindObject(_bindableGroup, "Data.BusinessObj.Users", "Items", BindingOptions.TwoWay);
		}
		
		
		private void createGroup()
		{
			User user1 = new User();
			user1.Username = "test1";
			user1.Desk = 50;
			user1.Email = "user1@user1.com";
			user1.Password = "unsafe1";
			
			User user2 = new User();
			user2.Username = "test2";
			user2.Desk = 43;
			user2.Email = "user2@user2.com";
			user2.Password = "unsafe2";
			
			_group = new Group();
			_group.Name = "test";
			_group.Users.Add(user1);
			_group.Users.Add(user2);
		}
		
		private void addUsers()
		{
			User user1 = new User();
			user1.Username = "test3";
			user1.Desk = 540;
			user1.Email = "user1@user3.com";
			user1.Password = "unsafe3";
			
			User user2 = new User();
			user2.Username = "test4";
			user2.Desk = 143;
			user2.Email = "user2@user4.com";
			user2.Password = "unsafe4";
		
			_listView.Items.Add(user1);
			_listView.Items.Add(user2);

		}

		protected virtual void OnShowItem (object sender, System.EventArgs e)
		{
			if (_listView.SelectedItem != null)
			{
				object item = _listView.SelectedItem;
				InfoDialog id = new InfoDialog();
				id.Message = item.ToString();
			}
		}
		
		protected virtual void OnDeleteItem (object sender, System.EventArgs e)
		{
			if (_listView.SelectedItem != null)
			{
				_listView.Items.Remove(_listView.SelectedItem);
			}
		}

		protected virtual void OnAddItem (object sender, System.EventArgs e)
		{
			User user = new User();
			user.Username = "random" + DateTime.Now;
			user.Password = "asdfsdf";
			_listView.Items.Add(user);
		}
	}
}
