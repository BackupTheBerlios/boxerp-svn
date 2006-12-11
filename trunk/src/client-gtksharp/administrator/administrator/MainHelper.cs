
using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using Boxerp.Models;
using Boxerp.Objects;
using Gtk;
using Boxerp.Client.GtkSharp.Lib;
using Boxerp.Client;

namespace administrator
{
	
	public class MainHelper : GtkResponsiveHelper
	{
		FilteredListView streeviewEnterprises, 
								streeviewUsers, 
								streeviewGroups;
		Enterprise[] enterprises;
		User[] users;
		Group[] groups;
		IAdmin adminObj;
		
		public MainHelper(Gtk.Window win, ref FilteredListView e, 
					ref FilteredListView u, ref FilteredListView g)
		{
			this.streeviewEnterprises = e;
			this.streeviewUsers = u;
			this.streeviewGroups = g;
			adminObj = (IAdmin) RemotingHelper.GetObject(typeof(IAdmin));
			base.Init(win);
		}
		
		[Responsive(ResponsiveEnum.Read)]
		public void LoadTreeViewsFromDb()
		{
			try
			{
				if (!CancelRequest)
					enterprises = adminObj.GetEnterprises();
				if (!CancelRequest)
					users = adminObj.GetUsers();
				if (!CancelRequest)
					groups = adminObj.GetGroups();
				
			}
			catch (ThreadAbortException)
			{
				enterprises = null;
				users = null;
				groups = null;
			}
			catch (Exception ex)
			{
				Console.WriteLine("LoadTreeViewsFromDb:" + ex.Message +","+ ex.StackTrace);
				OnRemoteException(ex.Message);
			}
			finally
			{
				Console.WriteLine("ok finally");
				StopTransfer();
			}
		}
		
		/*[Responsive(ResponsiveEnum.Read)]
		public void LoadUsers()
		{
			try
			{
				// do async
				users = adminObj.GetUsers();
			}
			catch (Exception)
			{
				users = null;
			}		
		}
		
		public delegate Group[] GroupsDelegate();
		
		[Responsive(ResponsiveEnum.Read)]
		public void LoadGroups()
		{
			try
			{
				GroupsDelegate remoteCall = adminObj.GetGroups;
				IAsyncResult asyncResult = remoteCall.BeginInvoke(null,null);
				bool success = true;
				while (asyncResult.IsCompleted == false)
				{
					System.Threading.Thread.Sleep(100);
					if (CancelRequest == true)
					{
						// abort the remote call
						success = false;
					}
				}
				if (success)
				{
					groups = remoteCall.EndInvoke(asyncResult);
				}
				groups = adminObj.GetGroups();
			}
			catch (Exception ex)
			{
				Console.WriteLine("LoadGroups:" + ex.Message);
				groups = null;
			}		
			finally 
			{
				groups = null;
			}
		}*/
		
		// TODO: load the trees from xml, not hardcoded
		private void InitTreeViews()
		{
			// Enterprises treeview:
			List<SimpleColumn> columns = new List<SimpleColumn>();
			SimpleColumn column = new SimpleColumn();
			column.Name = "Code";
			column.Type = typeof(string);
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
			this.streeviewEnterprises.Create(columns);

			// Users treeview:
			columns.Clear();
			column.Name = "Code";
			column.Type = typeof(string);
			column.Visible = true;
			columns.Add(column);
			
			column.Name = "Username";
			column.Type = typeof(object);
			column.Visible = true;
			columns.Add(column);			
			this.streeviewUsers.Create(columns);			

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
			this.streeviewGroups.Create(columns);									
		}
		
		public override void PopulateGUI()
		{
			InitTreeViews();
			if (enterprises != null)
			foreach (Enterprise i in enterprises)
			{
				ArrayList columns = new ArrayList();
				columns.Add(i.Id.ToString());
				columns.Add(i);
				columns.Add(i.Description);
				this.streeviewEnterprises.InsertRow(columns);
			}
			
			if (users != null)
			foreach (User i in users)
			{
				ArrayList columns = new ArrayList();
				columns.Add(i.Id.ToString());
				columns.Add(i);
				this.streeviewUsers.InsertRow(columns);
			}
			
			if (groups != null)
			foreach (Group i in groups)
			{
				ArrayList columns = new ArrayList();
				columns.Add(i.Id.ToString());
				columns.Add(i);
				this.streeviewGroups.InsertRow(columns);
			}	
		}	
	}
}
