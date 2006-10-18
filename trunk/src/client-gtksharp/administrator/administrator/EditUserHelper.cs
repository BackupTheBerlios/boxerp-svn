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
		User[] users;
		Group[] groups;
		IAdmin adminObj;
		
		public EditUserHelper(ref DoubleListView dlv)
		{
			this.doubleListView = dlv;
			adminObj = (IAdmin) RemotingHelper.GetObject(typeof(IAdmin));
		}
		
		/*[Responsive(ResponsiveEnum.Download)]
		public void LoadUsers()
		{
			try
			{
				users = adminObj.GetUsers();
			}
			catch (Exception ex)
			{
				users = null;
			}		
		}*/
		
		[Responsive(ResponsiveEnum.Download)]
		public void LoadGroups()
		{
			try
			{
				groups = adminObj.GetGroups();
			}
			catch (Exception ex)
			{
				Console.WriteLine("LoadGroups:" + ex.Message);
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
			
			/*if (users != null)
			foreach (User i in users)
			{
				ArrayList columns = new ArrayList();
				columns.Add(i.Id.ToString());
				columns.Add(i);
				this.streeviewUsers.InsertRow(TreeIter.Zero, columns);
			}*/
			
			if (groups != null)
			foreach (Group i in groups)
			{
				ArrayList columns = new ArrayList();
				columns.Add(i.Id.ToString());
				columns.Add(i);
				this.doubleListView.InsertRowRight(columns);
			}	
		}	
	}
}