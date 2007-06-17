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
	
	public partial class EditUserController 
	{
		DoubleListView doubleListView;
		User _user;
		//Group[] _groups;
		IAdmin _adminObj;
		bool _isNewUser = true;
		
		public User User 
		{
		    get { return _user; }
		    set { _user = value; }
		}
		
		public bool IsNewUser
		{
		    get { return _isNewUser; }
		    set { _isNewUser = value; }
		}
		
		public EditUserHelper(Gtk.Window win, ref DoubleListView dlv)
		{
			this.doubleListView = dlv;
			_adminObj = (IAdmin) RemotingHelper.GetObject(typeof(IAdmin));
			base.Init(win);
		}
		
		public EditUserHelper(Gtk.Window win, ref DoubleListView dlv, User u)
		{
			this.doubleListView = dlv;
			_adminObj = (IAdmin) RemotingHelper.GetObject(typeof(IAdmin));
			_user = u;
			base.Init(win);
		}
		
		/*[Responsive(ResponsiveEnum.Read)]
		public void LoadGroups()
		{
			try
			{
			    if (_isNewUser)
				{
					_groups = _adminObj.GetGroups();
				}
				else
				{
					_groups = _adminObj.GetDistinctGroups(_user);
				}
			}
			catch (ThreadAbortException)
			{
				_groups = null;
			}
			catch (Exception ex)
			{
				_groups = null;
				OnRemoteException(ex.Message);
			}
			finally
			{
				Console.WriteLine("ok finally load gropus:" + transferSuccess);
				StopTransfer();
			}
		}*/

		[Responsive(ResponsiveEnum.Write)]
		protected void SaveUser()
		{
			try
			{
				if (_user != null)
				{
					_user.Id = _adminObj.SaveUser(_user);
				}
			}
			catch(ThreadAbortException ex)
			{
			    OnAbortRemoteCall(ex.StackTrace);
			}
			catch (Exception ex)
			{
				Console.WriteLine("SaveUser:" + ex.Message);
				OnRemoteException(ex.Message);
			}		
			finally
			{
				StopTransfer(Thread.CurrentThread.ManagedThreadId, MethodInfo.GetCurrentMethod(), _user.Id);
			}
		}
		
		/// <summary>
		/// Called to populate User from form, before saving such an user at the server side
		/// </summary>
		public void PopulateUser(string userName, string realName, string password, 
						string email, bool active)
		{
		    if (_user == null)
		    {
		        _user = new User();
		    }
			_user.UserName = userName;
			_user.RealName = realName;
			_user.Password = password;
			_user.Email = email;
			_user.Active = active;
			if (_user.Groups != null)
			{
			    _user.Groups.Clear();
			}
			else
			{
			    _user.Groups = new HashedSet();
			}
			List<IBoxerpModel> userGroups = doubleListView.GetLeftObjectList();
			if (userGroups != null)
			{
			    foreach(Group g in userGroups)
			    {
				    _user.Groups.Add(g);
			    }
			}
		}
		
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