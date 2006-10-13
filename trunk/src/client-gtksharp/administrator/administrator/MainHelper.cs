
using System;
using System.Collections;
using clientlib;
using widgets;
using Boxerp.Models;
using Boxerp.Objects;
using Gtk;

namespace administrator
{
	
	public class MainHelper : ResponsiveHelper
	{
		widgets.AdvancedTreeView atreeviewEnterprises, 
								atreeviewUsers, 
								atreeviewGroups;
		Enterprise[] enterprises;
		User[] users;
		Group[] groups;
		IAdmin adminObj;
		
		public MainHelper(ref AdvancedTreeView e, 
					ref AdvancedTreeView u, ref AdvancedTreeView g)
		{
			this.atreeviewEnterprises = e;
			this.atreeviewUsers = u;
			this.atreeviewGroups = g;
			adminObj = (IAdmin) RemotingHelper.GetObject(typeof(IAdmin));
			UserInformation.SetSessionToken(SessionSingleton.GetInstance().GetSession());
		}
		
		[Responsive(ResponsiveEnum.Download)]
		public void LoadEnterprises()
		{
			try
			{
				UserInformation.SetSessionToken(SessionSingleton.GetInstance().GetSession());
				enterprises = adminObj.GetEnterprises();
			}
			catch (Exception ex)
			{
				Console.WriteLine("LoadEnterprise:" + ex.Message);
				enterprises = null;
			}
		}
		
		[Responsive(ResponsiveEnum.Download)]
		public void LoadUsers()
		{
			try
			{
				UserInformation.SetSessionToken(SessionSingleton.GetInstance().GetSession());
				users = adminObj.GetUsers();
			}
			catch (Exception ex)
			{
				users = null;
			}		
		}
		
		[Responsive(ResponsiveEnum.Download)]
		public void LoadGroups()
		{
			try
			{
				UserInformation.SetSessionToken(SessionSingleton.GetInstance().GetSession());
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
			// Enterprises treeview:
			ArrayList columns = new ArrayList();
			SimpleColumn column = new SimpleColumn();
			column.Name = "Code";
			column.Type = typeof(int);
			column.Visible = true;
			columns.Add(column);
			
			column.Name = "Name";
			column.Type = typeof(object);
			column.Visible = true;
			columns.Add(column);
			
			column.Name = "Description";
			column.Type = typeof(string);
			column.Visible = true;
			columns.Add(column);
			this.atreeviewEnterprises.Create(columns);

			// Users treeview:
			columns.Clear();
			column.Name = "Code";
			column.Type = typeof(int);
			column.Visible = true;
			columns.Add(column);
			
			column.Name = "Username";
			column.Type = typeof(object);
			column.Visible = true;
			columns.Add(column);			
			this.atreeviewUsers.Create(columns);			

			// Groups treeview:
			columns.Clear();
			column.Name = "Code";
			column.Type = typeof(int);
			column.Visible = true;
			columns.Add(column);
			
			column.Name = "Groupname";
			column.Type = typeof(object);
			column.Visible = true;
			columns.Add(column);			
			this.atreeviewGroups.Create(columns);									
		}
		
		public override void PopulateGUI()
		{
			InitTreeViews();
			if (enterprises != null)
			foreach (Enterprise i in enterprises)
			{
				ArrayList columns = new ArrayList();
				columns.Add(i.Id);
				columns.Add(i);
				columns.Add(i.Description);
				this.atreeviewEnterprises.InsertRow(TreeIter.Zero, columns);
			}
			
			if (users != null)
			foreach (User i in users)
			{
				ArrayList columns = new ArrayList();
				columns.Add(i.Id);
				columns.Add(i);
				this.atreeviewUsers.InsertRow(TreeIter.Zero, columns);
			}
			
			if (groups != null)
			foreach (Group i in groups)
			{
				ArrayList columns = new ArrayList();
				columns.Add(i.Id);
				columns.Add(i);
				this.atreeviewGroups.InsertRow(TreeIter.Zero, columns);
			}	
		}	
	}
}
