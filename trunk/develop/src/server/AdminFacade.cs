//
// AdminFacade.cs
//
// Authors:
// 	Carlos Ble Jurado <carlosble@shidix.com>
//
// Copyright (C) 2005,2006 Shidix Technologies (www.shidix.com)
// 
// Redistribution and use in source and binary forms, with or
// without modification, are permitted provided that the following
// conditions are met:
// Redistributions of source code must retain the above
// copyright notice, this list of conditions and the following
// disclaimer.
// Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following
// disclaimer in the documentation and/or other materials
// provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
// PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR
// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.IO;
using System.Data;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Services;
using System.Xml;
using Boxerp.Database;
using Boxerp.Tools;
using Boxerp.Admin;
using Boxerp.Exceptions;
using Boxerp.Errors;
using Boxerp.Debug;
using Boxerp.Objects;

namespace Boxerp.Facade
{
			///<summary>
			///AdminFacade manage system users, groups and enterprises
			///This is a server interface to the client. See docs at boxerp.org
			///</summary>
	[WebService (Description="Admin Facade WebService")]
 	public class AdminFacade : System.Web.Services.WebService
	{
		private ConcurrencyControllerObject concurrencyObj;
		private HistoryObject historyObj;
		private SessionsObject sessionsObj;
		private FacadeCache cacheMem;
		private Users users;
		private Groups groups;
		private Enterprises enterprises;
		string SKELENTERPRISE = "Skel"; // FIXME: get skel name from config
		string SKELGROUP = "Skel";      // FIXME: get skel name from config
		GenericDatabase db;
		
		public AdminFacade ()
		{
			try 
			{
				cacheMem = new FacadeCache(1000);
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		///////////////////////////////////////////////////////////////////////////////////	

		private void Initialize(string sid)
		{
			try
			{
				db = new GenericDatabase();
				db.Connect();
				users       = new Users(db);
				groups      = new Groups(db);
				enterprises = new Enterprises(db);
				ServicesTools.Connect2ObjectsServer(ref concurrencyObj, 
									 							ref historyObj, ref sessionsObj);
			}
			catch (Exception e)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(e, RunningClass.GetName(this), RunningClass.GetMethod())); 
				throw wrappedEx;
			}
		}
		///////////////////////////////////////////////////////////////////////////7	
				///<summary>
				///Checks for new user in cache. If isnt in cache search in database. 
				///If is in database, then copy to cache and return false
				///</summary>
		[WebMethod (Description="ExistUser")]
		private bool ExistUser(string sid, string userName)
		{
			try
			{
				Initialize(sid);
				if (cacheMem.IsInCache(DBTables.USERS + userName))									
					return true;
				else
				{
					UserStruct us = users.GetUserByName(userName);
					cacheMem.SetCacheData(us, DBTables.USERS + us.name);
					return true;
				}
			}
			catch (NullUserException ex)
			{
				return false;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		///////////////////////////////////////////////////////////////////////////////
				///<summary>
				///Checks for new group in cache. If isnt in cache search in database. 
				///If is in database, then copy to cache and return false. 
				///Group must be in given enterprise, otherwise a InvalidGroupException will
				///be thrown.
				///</summary>
		[WebMethod (Description="ExistGroup")]
		private bool ExistGroup(string sid, string groupName, ref int groupid, 
							 			string epriseName, ref int enterpriseid)
		{
			try
			{
				Initialize(sid);
				if (cacheMem.IsInCache(DBTables.GROUPS + groupName))									
				{
					if (cacheMem.IsInCache(DBTables.ENTERPRISES + epriseName))
					{
						EnterpriseStruct es = (EnterpriseStruct) cacheMem.GetCacheData(
								  					DBTables.ENTERPRISES + epriseName);
						GroupStruct gs = (GroupStruct) cacheMem.GetCacheData(
											 		DBTables.GROUPS + groupName);
						if (gs.enterpriseid != es.id)
						{
							InvalidGroupException ige = new InvalidGroupException();
							throw ige;
						}
						else
							return true;
					}
					else
					{
						InvalidGroupException ige = new InvalidGroupException();
						throw ige;
					}
				}
				else
				{
					GroupStruct g = groups.GetGroupByName(groupName);
					EnterpriseStruct e = enterprises.GetEnterpriseByName(epriseName);
					if (g.enterpriseid != e.id)
					{
						InvalidGroupException ige = new InvalidGroupException();
						throw ige;
					}
					cacheMem.SetCacheData(g, DBTables.GROUPS + g.name);
					groupid = g.id;
					enterpriseid = e.id;
					return true;
				}
			}
			catch (NullGroupException ex)
			{
				return false;
			}
			catch (InvalidGroupException ige)
			{
				InvalidGroupException wrappedEx = new 
					InvalidGroupException(
						ErrorManager.AddLayer(ige,RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		///////////////////////////////////////////////////////////////////////////////
		
		[WebMethod (Description="ExistEnterprise")]
		private bool ExistEnterprise(string sid, string epriseName)
		{
			try
			{
				Initialize(sid);
				if (cacheMem.IsInCache(DBTables.ENTERPRISES + epriseName))									
					return true;
				else
				{
					EnterpriseStruct e = enterprises.GetEnterpriseByName(epriseName);
					cacheMem.SetCacheData(e, DBTables.ENTERPRISES + e.name);
					return true;
				}
			}
			catch (NullEnterpriseException ex)
			{
				return false;
			}
			catch (InvalidGroupException ige)
			{
				InvalidGroupException wrappedEx = new 
					InvalidGroupException(
						ErrorManager.AddLayer(ige,	RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		////////////////////////////////////////////////////////////////////////////	
				///<summary>
				///Look for a group with given name and create and 
				///add user to group. Enterprise must exist.
				///If group doesnt exist, will be created.
				///</summary>
		[WebMethod (Description="AddUserToGroup")]
		public void AddUserToGroup(string sid, DataSet user, string groupName, string epriseName)
		{
			IDbTransaction t = null;
			try
			{
				Initialize(sid);
				int groupid = -1, enterpriseid = -1; 
				string userName = DataTools.GetTextField(user, AdminFields.USERNAME);
				
				UserStruct us = new UserStruct();
				us.name = userName;
				if (ExistGroup(sid, groupName, ref groupid, epriseName, ref enterpriseid))
				{
					us.groupid = groupid;
					t = db.BeginTransaction();
					us.published = DataTools.GetBoolField(user, AdminFields.PUBLISHED);
					us.id = users.CreateUser(us);
					cacheMem.SetCacheData(us, DBTables.USERS + us.name);
					if (t != null) db.Commit(t);
				}
				else
				{
					GroupStruct gs  = new GroupStruct();
					gs.name         = groupName;
					gs.published    = true;
					gs.enterpriseid = enterpriseid;
					t = db.BeginTransaction();
					gs.id = groups.CreateGroup(gs);
					cacheMem.SetCacheData(gs, DBTables.GROUPS + gs.name);
					us.groupid = groupid;
					us.id = users.CreateUser(us);
					cacheMem.SetCacheData(us, DBTables.USERS + us.name);
					if (t != null) db.Commit(t);
				}
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			finally
			{
				if (t != null) db.Commit(t);
			}
		}
		///////////////////////////////////////////////////////////////////////////////	
				///<summary>
				///Look for an enterprise with given name and create 
				///and add group to it. Enterprise must exist.
				///</summary>
		[WebMethod (Description="AddGroupToEnterprise")]
		public void AddGroupToEnterprise(string sid, DataSet group, string epriseName)
		{
			IDbTransaction t = null;
			try
			{
				Initialize(sid);
				string groupName = DataTools.GetTextField(group, AdminFields.GROUPNAME);
				
				GroupStruct gs  = new GroupStruct();
				gs.name         = groupName;
				gs.published    = DataTools.GetBoolField(group, AdminFields.PUBLISHED);
				gs.enterpriseid = DataTools.GetIntField(group, AdminFields.EPRISEID);
				t = db.BeginTransaction();
				gs.id = groups.CreateGroup(gs);
				cacheMem.SetCacheData(gs, DBTables.GROUPS + gs.name);
				if (t != null) db.Commit(t);
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			finally
			{
				if (t != null) db.Commit(t);
			}
		}
		////////////////////////////////////////////////////////////////////////////////	

		[WebMethod (Description="AddEnterprise")]
		public void AddEnterprise(string sid, DataSet enterprise)
		{
			IDbTransaction t = null;
			try
			{
				Initialize(sid);
				string epriseName = DataTools.GetTextField(enterprise, AdminFields.EPRISENAME);
				EnterpriseStruct es  = new EnterpriseStruct();
				es.name       = epriseName;
				es.published  = DataTools.GetBoolField(enterprise, AdminFields.PUBLISHED);
				es.desc       = DataTools.GetTextField(enterprise, AdminFields.EPRISEDESC);
				t = db.BeginTransaction();
				es.id = enterprises.CreateEnterprise(es);
				cacheMem.SetCacheData(es, DBTables.ENTERPRISES + es.name);
			}
			catch (EnterpriseAlreadyExistException eae)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.GenerateError(RunningClass.GetName(this), 
							  							RunningClass.GetMethod(), 
														ErrorsMsg.ENTERPRISE_EXIST));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			finally
			{
				if (t != null) db.Commit(t);
			}
		}
		/////////////////////////////////////////////////////////////////////////

		[WebMethod (Description="ModifyEnterprise")]
		public void ModifyEnterprise(string sid, DataSet enterprise)
		{
			IDbTransaction t = null;
			try
			{
				Initialize(sid);
				string epriseName = DataTools.GetTextField(enterprise, AdminFields.EPRISENAME);
				EnterpriseStruct es  = new EnterpriseStruct();
				es.name       = epriseName;
				es.published  = DataTools.GetBoolField(enterprise, AdminFields.PUBLISHED);
				es.id         = DataTools.GetIntField(enterprise, AdminFields.EPRISEID);
				es.desc       = DataTools.GetTextField(enterprise, AdminFields.EPRISEDESC);
				t = db.BeginTransaction();
				enterprises.ModifyEnterprise(es.id, es);
				cacheMem.SetCacheData(es, DBTables.ENTERPRISES + es.name);
			}
			catch (NullEnterpriseException nee)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.GenerateError(RunningClass.GetName(this), 
							  							RunningClass.GetMethod(), 
														ErrorsMsg.EMPTY_ENTERPRISE));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			finally
			{
				if (t != null) db.Commit(t);
			}
		}
		////////////////////////////////////////////////////////////////////////////////////
				///<summary>
				///Change user from its actual group to a destination group
				///</summary>
		[WebMethod (Description="ChangeUserToGroup")]
		public void ChangeUserToGroup(string sid, string userName, string dstGroup, string epriseName)
		{
			IDbTransaction t = null;
			try
			{
				int groupid = -1, enterpriseid = -1; 
							
				if (ExistUser(sid, userName))
				{
					UserStruct us = (UserStruct) cacheMem.GetCacheData(DBTables.USERS + userName);
					if (ExistGroup(sid, dstGroup, ref groupid, epriseName, ref enterpriseid))
					{
						us.groupid = groupid;
						us.name    = userName;
						t = db.BeginTransaction();
						users.DeleteUser(us.id);
						users.CreateUser(us);
						cacheMem.SetCacheData(us, DBTables.USERS + us.name);
						if (t != null) db.Commit(t);
					}
					else
					{
						NullGroupException nge = new NullGroupException();
						throw nge;
					}
				}
				else
				{
					NullUserException nue = new NullUserException();
					throw nue;
				}
			}
			catch (NullGroupException nge)
			{
				NullGroupException wrappedEx = new 
					NullGroupException(
						ErrorManager.AddLayer(nge,	RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (NullUserException nue)
			{
				NullUserException wrappedEx = new 
					NullUserException(
						ErrorManager.AddLayer(nue,RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			finally
			{
				if (t != null) db.Commit(t);
			}
		}
		////////////////////////////////////////////////////////////////////////////////
				/// FIXME: Query the cache and extract only a page.	
		[WebMethod (Description="GetUsersList")]
		public DataSet GetUsersList(string sid, int groupId, int startUser, int offsetUsers)
		{
			try
			{
				Initialize(sid);
				DataSet ds = groups.GetAllUsers(groupId);
				return ds;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		///////////////////////////////////////////////////////////////////////////////////
				/// FIXME: Query the cache and extract only a page.	
		[WebMethod (Description="GetGroupsList by Name")]
		public DataSet GetGroupsListByName(string sid, string epriseName, 
							 							int startUser, int offsetUsers)
		{
			try
			{
				Initialize(sid);
				DataSet ds = enterprises.GetAllGroups(epriseName);
				return ds;
			}
			catch (NullDataSetException e)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.GenerateError(RunningClass.GetName(this), 
							  							RunningClass.GetMethod(), 
														ErrorsMsg.EMPTY_ENTERPRISE));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		///////////////////////////////////////////////////////////////////////////////////
				/// FIXME: Query the cache and extract only a page.	
		[WebMethod (Description="GetGroupsList by ID")]
		public DataSet GetGroupsListById(string sid, int epriseId, int startUser, int offsetUsers)
		{
			try
			{
				Initialize(sid);
				DataSet ds = enterprises.GetAllGroups(epriseId);
				return ds;
			}
			catch (NullDataSetException e)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.GenerateError(RunningClass.GetName(this), 
														RunningClass.GetMethod(), 
														ErrorsMsg.EMPTY_ENTERPRISE));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
						ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		////////////////////////////////////////////////////////////////////////////////
				/// FIXME: Query the cache and extract only a page.	
		[WebMethod (Description="GetIconGroupsList")]
		public DataSet GetIconGroupsList(string sid, string epriseName, 
							 						int startUser, int offsetUsers)
		{
			try
			{
				Initialize(sid);
				DataSet ds = enterprises.GetAllGroups(epriseName);
				DataTools.AddColumn(ref ds, GuiFields.ICONPATH);
				DataRow[] rows = DataTools.GetRows(ds);
				DataSet iconDs;
				foreach (DataRow row in rows)
				{
					iconDs = db.GetById((int)row[GuiFields.ICON], DBTables.ICONS); 
					DataTools.SetTextField(ref ds, GuiFields.ICONPATH, 
													DataTools.GetTextField(iconDs, GuiFields.PATH));
				}
				return ds;
			}
			catch (NullDataSetException e)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.GenerateError(RunningClass.GetName(this), 
														RunningClass.GetMethod(), 
														ErrorsMsg.EMPTY_ENTERPRISE));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		/////////////////////////////////////////////////////////////////////////////////////
				/// FIXME: Query the cache and extract only a page.	
		[WebMethod (Description="GetEnterprisesList")]
		public DataSet GetEnterprisesList(string sid, int startUser, int offsetUsers)
		{
			try
			{
				Initialize(sid);
				return enterprises.GetAllEnterprises();
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		///////////////////////////////////////////////////////////////////////////////
				///<summary>
				///Delete enterprise information but no groups 
				///</summary>
		[WebMethod (Description="DelEnterprise")]	
		public void DelEnterprise(string sid, int epriseId)	
		{
			try
			{
				Initialize(sid);
				enterprises.DelEnterprise(epriseId);	
			}
			catch (NullEnterpriseException nee)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.GenerateError(RunningClass.GetName(this), 
														RunningClass.GetMethod(), 
														ErrorsMsg.NULL_ENTERPRISE));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
	
/*				///<summary>
				///Look for a group with same name; if doesnt exist creates a new group, else 
				///asign existent group if group belong to given enterprise, otherwise create a new
				///group with same user name plus a number. An existent enterprise must be given
				///</summary>
		public bool AddUserToEnterprise(DataSet user, string epriseName, ref string error_msg)
		{
			try
			{
				string userName = DataTools.GetTextField(user, AdminFields.USERNAME); 
				int groupid, userid, enterpriseid;
				bool isNewUser, isNewGroup, querySuccess;
				
				querySuccess =  usersObj.AskForNewUser(userName, ref userid, ref isNewUser, ref error_msg) && 
												groupsObj.AskForNewGroup(userName, ref groupid, ref isNewGroup, ref error_msg);
				if (querySucess)
				{
					if (isNewUser)
					{
						string passwd    = DataTools.GetTextField(user, DataTools.AdminFields.PASSWORD); 
						bool published   = DataTools.GetBoolField(user, DataTools.AdminFields.PUBLISHED); 
						if (!isNewGroup)
						{
							if (IsGroupInEnterprise(groupName, epriseName))
							{
								return (usersObj.CreateUser(userName, ref userid, passwd, groupid, ref error_msg)); 
							}
							else 
							{
								AddGroup que no exista , meterlo en la empresa y meterle el usuario
							}
						}
						else
						{
							string desc = "";
							return (eprisesObj.GetEnterpriseByName(epriseName, ref enterpriseid, ref desc, ref error_msg) &&
											groupsObj.CreateGroup(groupName, ref groupid, enterpriseid, published, ref error_msg) &&
											usersObj.CreateUser(userName, ref userid, passwd, groupid, ref error_msg));
						}
					}
					else
					{
						error_msg = "EL USUARIO YA EXISTE";
						return false;
					}
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;
		}
	*/	
		//////////////////////////////////////////////////////////////////////////////////////////////////
/*				///<summary>
				///AddUser: Create a new user with same name group and asign Skel Enterprise.
				///If groupname exist, deletes group and creates a new one.
				///</summary>
		public bool AddUserToSkel(DataSet user, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;

		}
		//////////////////////////////////////////////////////////////////////////////////////////////////
				///<summary>
				///Delete User but doesnt delete group
				///</summary>
		public bool DelUser(string userName, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;

		}
		//////////////////////////////////////////////////////////////////////////////////////////////////
				///<summary>
				///Delete User and Delete its group, only if there are no more users in same group
				///</summary>
		public bool DelPurgeUser(string userName, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;

		}
		//////////////////////////////////////////////////////////////////////////////////////////////////
				///<summary>
				///Edit user fields but not user permissions
				///</summary>
		public bool EditUser(string userName, DataSet user, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;

		}
		//////////////////////////////////////////////////////////////////////////////////////////////////
				///<summary>
				///A user only has a group at a time. If there are other users in same group, then copy the 
				///group to a new one, asign new group to user and change new group permissions
				///</summary>
		public bool ChangeUserPermissions(string userName, DataSet permissions, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;

		}
		//////////////////////////////////////////////////////////////////////////////////////////////////

		public bool GetUser(string userName, ref DataSet user, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;

		}
		//////////////////////////////////////////////////////////////////////////////////////////////////

		public bool SearchUser(string searchStr, ref DataSet result, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;

		}
		//////////////////////////////////////////////////////////////////////////////////////////////////
				///<summary>
				///Delete group information but no users, neither permissions
				///</summary>
		public bool DelGroup(string groupName, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;

		}
		//////////////////////////////////////////////////////////////////////////////////////////////////
				///<summary>
				///Delete group information and all users from group but no permissions
				///</summary>
		public bool DelCascadeGroup(string groupName, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;

		}
		//////////////////////////////////////////////////////////////////////////////////////////////////
				///<summary>
				///Delete group, users, and permissions. 
				///</summary>
		public bool DelPurgeCascadeGroup(string groupName, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;

		}
		//////////////////////////////////////////////////////////////////////////////////////////////////

		public bool EditGroup(string groupName, DataSet group, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;

		}
		//////////////////////////////////////////////////////////////////////////////////////////////////

		public bool ChangeGroupToEnterprise(string groupName, string dstEprise, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;

		}
		//////////////////////////////////////////////////////////////////////////////////////////////////
					///<summary>
					///Returns true if there is only one user in group. This method may be invoked before 
					///ChangeUserPermissions. 
					///</summary>
		public bool IsSingleUserGroup(string groupName, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;

		}
		//////////////////////////////////////////////////////////////////////////////////////////////////
				
		public bool ChangeGroupPermissions(string groupName, DataSet permissions, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;

		}
		//////////////////////////////////////////////////////////////////////////////////////////////////

		public bool GetGroupsList(string epriseName, ref DataSet groups, int startGroup, 
															int offsetGroups, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;
		}
		//////////////////////////////////////////////////////////////////////////////////////////////////

		public bool GetGroup(string groupName, ref DataSet group, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;
		}
		//////////////////////////////////////////////////////////////////////////////////////////////////

		public bool SearchGroup(string groupName, ref DataSet result, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;
		}
		*/
		//////////////////////////////////////////////////////////////////////////////////////////////////
		/*
		//////////////////////////////////////////////////////////////////////////////////////////////////
				///<summary>
				///Delete enterprise and groups and users, but no permissions
				///</summary>
		public bool DelCascadeEnterprise(string epriseName, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;
		}
		//////////////////////////////////////////////////////////////////////////////////////////////////
				///<summary>
				///Delete enterprise, groups, users and permissions
				///</summary>
		public bool DelPurgeCascadeEnterprise(string epriseName, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;
		}
		//////////////////////////////////////////////////////////////////////////////////////////////////

		public bool EditEnterprise(string epriseName, DataSet eprise, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;
		}
		//////////////////////////////////////////////////////////////////////////////////////////////////

		public bool GetEnterprisesList(string epriseName, ref DataSet eprise, int startEprise, 
																	int offsetEprises, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;
		}
		//////////////////////////////////////////////////////////////////////////////////////////////////

		public bool GetEnterprise(string epriseName, ref DataSet eprise, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;
		}
		//////////////////////////////////////////////////////////////////////////////////////////////////

		public bool SearchEnterprise(string searchStr, ref DataSet result, ref string error_msg)
		{
			try
			{
				
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}
			return true;
		}
*/		//////////////////////////////////////////////////////////////////////////////////////////////////
	}

}
