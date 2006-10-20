using System;
using System.Collections;
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
		
		public EditUserHelper(ref DoubleListView dlv)
		{
			this.doubleListView = dlv;
			adminObj = (IAdmin) RemotingHelper.GetObject(typeof(IAdmin));
		}
		
		public EditUserHelper(ref DoubleListView dlv, User u)
		{
			this.doubleListView = dlv;
			adminObj = (IAdmin) RemotingHelper.GetObject(typeof(IAdmin));
			user = u;
			isNewUser = false;
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
			ArrayList columns = new ArrayList();
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
						
			if (groups != null)
			foreach (Group i in groups)
			{
				ArrayList columns = new ArrayList();
				columns.Add(i.Id.ToString());
				columns.Add(i);
				this.doubleListView.InsertRowRight(columns);
			}	
		}
		
		public void PopulateUserTreeView(User u)
		{
			user = u;
			if (user != null)
			Console.WriteLine("total user groups:" + user.Groups.Count);
			foreach (Group i in user.Groups)
			{	
				Console.WriteLine("group name=" + i.GroupName);
				ArrayList columns = new ArrayList();
				columns.Add(i.Id.ToString());
				columns.Add(i);
				this.doubleListView.InsertRowLeft(columns);
			}
		}
		
		public void ClearUser()
		{
			user = null;
		}
		
		public void PopulateUserWithGroups(ref User u)
		{
			//u.Groups = null;
			u.Groups.Clear();
			ArrayList groups = doubleListView.GetLeftObjectList();
			if (groups != null)
			foreach(Group g in groups)
			{
				u.Groups.Add(g);
			}
		}
	}
}