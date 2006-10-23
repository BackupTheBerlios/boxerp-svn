using System;
using System.Collections;
using System.Collections.Generic;
using clientlib;
using widgets;
using Boxerp.Models;
using Boxerp.Objects;
using Gtk;

namespace administrator
{
	
	public class EditUserHelper : ResponsiveHelper
	{
		widgets.DoubleListView doubleListView;
		User user;
		Group[] groups;
		IAdmin adminObj;
		bool isNewUser = true;
		
		public EditUserHelper(Gtk.Window win, ref DoubleListView dlv)
		{
			this.doubleListView = dlv;
			adminObj = (IAdmin) RemotingHelper.GetObject(typeof(IAdmin));
			base.Init(win);
		}
		
		public EditUserHelper(Gtk.Window win, ref DoubleListView dlv, User u)
		{
			this.doubleListView = dlv;
			adminObj = (IAdmin) RemotingHelper.GetObject(typeof(IAdmin));
			user = u;
			isNewUser = false;
			base.Init(win);
		}
		
		[Responsive(ResponsiveEnum.Download)]
		public void LoadGroups()
		{
			try
			{
				if (isNewUser)
					groups = adminObj.GetGroups();
				else
					groups = adminObj.GetDistinctGroups(user);
			}
			catch (Exception ex)
			{
				Console.WriteLine("LoadGroups:" + ex.Message);
				groups = null;
			}		
		}

		[Responsive(ResponsiveEnum.Upload)]
		public void SaveUser()
		{
			try
			{
				if (user != null)
					adminObj.SaveUser(user);
			}
			catch (Exception ex)
			{
				Console.WriteLine("SaveUser:" + ex.Message);
				groups = null;
			}		
		}
		
		// TODO: load the trees from xml, not hardcoded
		private void InitTreeViews()
		{
			List<SimpleColumn> columns = new List<SimpleColumn>();
			
			// Users treeview:
			SimpleColumn column = new SimpleColumn();
			column.Name = "Code";
			column.Type = typeof(string);
			column.Visible = true;
			columns.Add(column);
			
			column.Name = "GroupName";
			column.Type = typeof(object);
			column.Visible = true;
			columns.Add(column);
			this.doubleListView.CreateLeftList(columns);
			
			// Groups treeview:
			columns.Clear();
			column.Name = "Code";
			column.Type = typeof(string);
			column.Visible = true;
			columns.Add(column);
			
			column.Name = "Groupname";
			column.Type = typeof(object);
			column.Visible = true;
			columns.Add(column);
			this.doubleListView.CreateRightList(columns);							
		}
		
		public override void PopulateGUI()
		{
			InitTreeViews();
			PopulateUserTreeView();			
			if (groups != null)
			foreach (Group i in groups)
			{
				ArrayList columns = new ArrayList();
				columns.Add(i.Id.ToString());
				columns.Add(i);
				this.doubleListView.InsertRowRight(columns);
			}	
		}
		
		private void PopulateUserTreeView()
		{
			//Console.WriteLine("total user groups:" + user.Groups.Count);
			if (user != null)
			foreach (Group i in user.Groups)
			{	
				Console.WriteLine("group name=" + i.GroupName);
				ArrayList columns = new ArrayList();
				columns.Add(i.Id.ToString());
				columns.Add(i);
				if (columns == null)
					Console.WriteLine("nullll!!!!");
				this.doubleListView.InsertRowLeft(columns);
			}
		}
		
		public void ClearUser()
		{
			user = null;
		}
		
		public void PopulateUser(string userName, string realName, string password, 
						string email, bool active)
		{
			user.UserName = userName;
			user.RealName = realName;
			user.Password = password;
			user.Email = email;
			user.Active = active;
			user.Groups.Clear();
			List<IBoxerpModel> groups = doubleListView.GetLeftObjectList();
			if (groups != null)
			foreach(Group g in groups)
			{
				user.Groups.Add(g);
			}
		}
	}
}