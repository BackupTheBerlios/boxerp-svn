using System;
using System.Reflection;
using System.Collections;
using Iesi.Collections;
using System.Collections.Generic;
using System.Threading;
using Boxerp.Models;
using Boxerp.Objects;
using Gtk;
using Boxerp.Client.GtkSharp.Lib;
using Boxerp.Client;

namespace administrator
{
	
	
	public partial class EditUserHelper
	{
		
		/// <summary>
		/// As the async op is completed, the editUserWindow must be hidden.
		/// </summary>
		public override void OnAsyncCallStop(object sender, ThreadEventArgs teargs)
		{
		    SimpleDelegate saveUser = this.SaveUser;
		    
		    if (saveUser.Method == teargs.MethodBase)
		    {
		        int userId = (int)teargs.ReturnValue;
		        if (userId > 0)
		        {
		            base.parentWindow.Destroy();
		        }
		    }
		}
		
		public void PopulateGUI(Group[] groups)
		{
			InitTreeViews();
			if (_user != null)
			{
			    foreach (Group i in _user.Groups)
			    {	
				    doubleListView.InsertModelToTheLeft(i);
				}
			}
			if (groups != null)
			{
			    foreach (Group i in groups)
			    {
				    if (_user != null)
				    {
				        if (!_user.Groups.Contains(i))
				        {
				            doubleListView.InsertModelToTheRight(i);
				        }
				    }
				    else
				    {
				        doubleListView.InsertModelToTheRight(i);
				    }
			    }
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
		
	}
}
