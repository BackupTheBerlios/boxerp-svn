

using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using System.Reflection;
using Boxerp.Models;
using Boxerp.Objects;
using Gtk;
using Boxerp.Client.GtkSharp.Lib;
using Boxerp.Client;

namespace administrator
{
	
	
	public partial class MainHelper
	{
		public override void OnAsyncCallStop(object sender, ThreadEventArgs teargs)
		{
		    Console.WriteLine("ASYNC CALL STOP");
		    Console.WriteLine("{0},{1},{2}", teargs.ThreadId, teargs.MethodBase.ToString(), teargs.ReturnValue);
		    SimpleDelegate deleteUser = this.DeleteUser;
		    
		    if (deleteUser.Method == teargs.MethodBase)
		    {
		        int userId = (int)teargs.ReturnValue;
		        if (userId > 0)
		        {
		            Console.WriteLine("userid = " + userId);
                    ftreeviewUsers.DeleteModelById(userId);
		        }
		    }
		}
		
		public override void PopulateGUI()
		{
			InitTreeViews();
			if (_enterprises != null)
			{
			    foreach (Enterprise i in _enterprises)
			    {
				    ArrayList columns = new ArrayList();
			    	columns.Add(i.Id.ToString());
			    	columns.Add(i);
			    	columns.Add(i.Description);
			    	ftreeviewEnterprises.InsertRow(columns);
			    }
			}
			if (_users != null)
			{
			    foreach (User i in _users)
			    {
				    ftreeviewUsers.InsertModel(i);
				}
			}
			if (_groups != null)
			{
			    foreach (Group i in _groups)
			    {
			        ftreeviewGroups.InsertModel(i);
				}
			}
		}
		
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
			ftreeviewEnterprises.Create(columns);

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
			ftreeviewUsers.Create(columns);			

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
			ftreeviewGroups.Create(columns);									
		}
	}
}
