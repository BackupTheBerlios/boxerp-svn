
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
	
	public partial class MainHelper : GtkResponsiveHelper
	{
		FilteredListView ftreeviewEnterprises, 
								ftreeviewUsers, 
								ftreeviewGroups;
		Enterprise[] _enterprises;
		User[] _users;
		Group[] _groups;
		IAdmin _adminObj;
		User _user;
		
		public Group[] Groups
		{
		    get { return _groups; }
		}
		
		public User[] Users
		{
		    get { return _users; }
		}
		
		public Enterprise[] Enterprises
		{
		    get { return _enterprises; }
		}
		
		public User User
		{
		    get { return _user; }
		    set { _user = value; }
		}
		
		public MainHelper(Gtk.Window win, ref FilteredListView e, 
					ref FilteredListView u, ref FilteredListView g)
		{
			ftreeviewEnterprises = e;
			ftreeviewUsers = u;
			ftreeviewGroups = g;
			_adminObj = (IAdmin) RemotingHelper.GetObject(typeof(IAdmin));
			base.Init(win);
		}
		
		[Responsive(ResponsiveEnum.Read)]
		protected void LoadModelsFromDb()
		{
			try
			{
				if (!CancelRequest)
					_enterprises = _adminObj.GetEnterprises();
				if (!CancelRequest)
					_users = _adminObj.GetUsers();
				if (!CancelRequest)
					_groups = _adminObj.GetGroups();
				
			}
			catch (ThreadAbortException ex)
			{
				_enterprises = null;
				_users = null;
				_groups = null;
				OnAbortRemoteCall(ex.StackTrace);
		    }
			catch (Exception ex)
			{
				Console.WriteLine("LoadTreeViewsFromDb:" + ex.Message +","+ ex.StackTrace);
				OnRemoteException(ex.Message);
			}
			finally
			{
				StopTransfer(Thread.CurrentThread.ManagedThreadId, MethodInfo.GetCurrentMethod(), null);
			}
		}
		
		public void DeleteUser()
		{
		    int userId = 0;
		    try
		    {
		        userId = _adminObj.DeleteUser(_user);
		    }
		    catch (ThreadAbortException ex)
			{
				userId = -1;
				OnAbortRemoteCall(ex.StackTrace);
			}
			catch (Exception ex)
			{
				Console.WriteLine("LoadTreeViewsFromDb:" + ex.Message +","+ ex.StackTrace);
				OnRemoteException(ex.Message);
			}
		    finally
		    {
		        base.StopTransfer(Thread.CurrentThread.ManagedThreadId, MethodInfo.GetCurrentMethod(), userId);
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
		

		
	}
}
