//
// Groups.cs
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
using System.Data;
using System.Collections;
using Boxerp.Database;
using Boxerp.Tools;
using Boxerp.Exceptions;
using Boxerp.Errors;
using Boxerp.Debug;

namespace Boxerp.Admin
{
	public struct GroupStruct
	{
		public string name;
		public int    id;
		public int    enterpriseid;
		public bool   published;
	}
	
		///<summary>
		///Groups manage system users; create, modify, delete, change group, change permissions...
		///
		///</summary>
 	public class Groups 
	{
		GenericDatabase db;
		string SKELGROUP = "Skel";

		public Groups (GenericDatabase db)
		{
			this.db = db ;
		}
		/////////////////////////////////////////////////////////////////////////	
				///<summary>
				///CreateGroup: enterpriseid must be an existent enterprise. groupName must'n exist.
				///Permissions are cloned from skel group
				///<return>New group id</return>
				///</summary>
		public int CreateGroup(GroupStruct group)
		{
			IDictionary groupRecord, epriseRecord, groupFields;
			
			try
			{
				groupRecord = new Hashtable();
				groupRecord[AdminFields.GROUPNAME] = "= '" + group.name + "'"; 
				epriseRecord = new Hashtable();
				epriseRecord[AdminFields.ID] = "= " +group.enterpriseid;
				if (!db.Exist(groupRecord, DBTables.GROUPS)) 
				{
					if (db.Exist(epriseRecord, DBTables.ENTERPRISES))
					{
						groupFields = new Hashtable();
						groupFields[AdminFields.GROUPNAME] = group.name;
						groupFields[AdminFields.EPRISEID]  = group.enterpriseid;
						groupFields[AdminFields.PUBLISHED] = group.published;
						db.Insert(groupFields, DBTables.GROUPS, ref group.id);
						this.ClonePermissions(SKELGROUP, group.name);// Set skel permissions 4new group
						return group.id;
					}
					else
					{
						GroupAlreadyExistException gae = new GroupAlreadyExistException();
						throw gae;
					}
				}
				else 
				{
					NullEnterpriseException nee = new NullEnterpriseException();
					throw nee;
				}
			}
			catch (GroupAlreadyExistException gae)
			{
				GroupAlreadyExistException wrappedEx = new 
					GroupAlreadyExistException(
						ErrorManager.AddLayer(gae, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (NullEnterpriseException nee)
			{
				NullEnterpriseException wrappedEx = new 
					NullEnterpriseException(
						ErrorManager.AddLayer(nee, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
						ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		/////////////////////////////////////////////////////////////////////////////	
				///<summary>
				///CloneGroup: Same as CreateGroup, bud permissions are clone from selected group. 
				///<param name="srcgroup">Group_id to clone</param>
				///</summary>
		public void CloneGroup(int srcGroup, string dstGroupName)
		{
			IDictionary srcRecord, dstRecord, groupFields;
			
			try
			{
				srcRecord = new Hashtable();
				srcRecord[AdminFields.ID] = "= " +srcGroup; 
				dstRecord = new Hashtable();
				dstRecord[AdminFields.GROUPNAME] = "= '" + dstGroupName + "'";
				if (db.Exist(srcRecord, DBTables.GROUPS))
				{
					if (!db.Exist(dstRecord, DBTables.GROUPS))
					{
						DataSet ds = db.GetFieldsWhereas(srcRecord, DBTables.GROUPS);		
						groupFields = new Hashtable();
						groupFields[AdminFields.GROUPNAME] = dstGroupName;
						groupFields[AdminFields.EPRISEID]  = 
								  DataTools.GetIntField (ds, AdminFields.EPRISEID);
						groupFields[AdminFields.PUBLISHED] = 
								  DataTools.GetBoolField(ds, AdminFields.PUBLISHED);
						int dstGid = 0;
						db.Insert(groupFields, DBTables.GROUPS, ref dstGid);
						this.ClonePermissions(srcGroup, dstGid);
					}
					else 
					{
						GroupAlreadyExistException gae = new GroupAlreadyExistException();
						throw gae;
					}
				}
				else
				{
					NullGroupException nge = new NullGroupException();
					throw nge;
				}
			}
			catch (GroupAlreadyExistException gae)
			{
				GroupAlreadyExistException wrappedEx = new 
					GroupAlreadyExistException(
						ErrorManager.AddLayer(gae,	RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (NullGroupException nge)
			{
				NullGroupException wrappedEx = new 
					NullGroupException(
						ErrorManager.AddLayer(nge,	RunningClass.GetName(this), RunningClass.GetMethod()));
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
				///<summary>
				///DeleteGroup: Only deletes a row in groups table, and group permissions. Users 
				///must be deleted with DeleteAllUsers or throught Facade layout, before this
				///</summary>
		public void DeleteGroup(int groupid)
		{
			IDictionary groupRecord;

			try
			{
				groupRecord = new Hashtable();
				groupRecord[AdminFields.ID] = "= " +groupid;
				if (db.Exist(groupRecord, DBTables.GROUPS))
				{
					db.DeleteWhereas(groupRecord, DBTables.GROUPS);
					groupRecord.Clear();
					groupRecord[AdminFields.GROUPID] = "= " +groupid;
					db.DeleteWhereas(groupRecord, DBTables.PERMS);
				}
				else
				{
					NullGroupException nge = new NullGroupException();
					throw nge;
				}
			}
			catch (NullGroupException nge)
			{
				NullGroupException wrappedEx = new 
					NullGroupException(
						ErrorManager.AddLayer(nge,	RunningClass.GetName(this), RunningClass.GetMethod()));
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
				///<summary>
				///ModifyGroup: groupName must be an existent group. 
				///enterpriseid must be an existent enterprise.
				///newgroup must'n exist
				///</summary>
		public void ModifyGroup(int groupid, GroupStruct newgroup)
		{
			IDictionary epriseRecord, newGroupRecord, thisGroupRecord, groupFields;
			
			try
			{
				epriseRecord = new Hashtable();
				epriseRecord[AdminFields.EPRISEID] = "= " +newgroup.enterpriseid;
				newGroupRecord = new Hashtable();
				newGroupRecord[AdminFields.GROUPNAME] = "= '" + newgroup.name + "'";
				if (db.Exist(epriseRecord, DBTables.ENTERPRISES))
				{
					if (!db.Exist(newGroupRecord, DBTables.GROUPS))
					{
						thisGroupRecord = new Hashtable();
						thisGroupRecord[AdminFields.ID] = "= "+ groupid;
						if (db.Exist(thisGroupRecord, DBTables.GROUPS))
						{
							groupFields = new Hashtable();
							groupFields[AdminFields.GROUPNAME] = newgroup.name;
							groupFields[AdminFields.EPRISEID]  = newgroup.enterpriseid;
							groupFields[AdminFields.PUBLISHED] = newgroup.published;
							db.SetFieldsWhereas(thisGroupRecord, groupFields, DBTables.GROUPS);
						}
						else
						{
							NullGroupException nge = new NullGroupException();
							throw nge;
						}
					}
					else
					{
						GroupAlreadyExistException gae = new GroupAlreadyExistException();
						throw gae;
					}
				}
				else
				{
					NullEnterpriseException nee = new NullEnterpriseException();
					throw nee;
				}
			}
			catch (GroupAlreadyExistException gae)
			{
				GroupAlreadyExistException wrappedEx = new 
					GroupAlreadyExistException(
						ErrorManager.AddLayer(gae,	RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (NullEnterpriseException nee)
			{
				NullEnterpriseException wrappedEx = new 
					NullEnterpriseException(
						ErrorManager.AddLayer(nee,	RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (NullGroupException nge)
			{
				NullGroupException wrappedEx = new 
					NullGroupException(
						ErrorManager.AddLayer(nge,	RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		//////////////////////////////////////////////////////////////////////////////	
				///<summary>
				///ClonePermissions: get all rows from permissions where id_group = source group and 
				///insert permissions with id_group = destination group. 
				///If destination group permissions
				///exists, will be erased when cloning.
				///</summary>
		public void ClonePermissions(int srcgroup, int dstgroup)
		{
			IDictionary srcRecord, dstRecord, tmpClone;
			
			try
			{
				srcRecord = new Hashtable();
				srcRecord[AdminFields.GROUPID] = srcgroup;
				if (db.Exist(srcRecord, DBTables.PERMS))
				{
					dstRecord = new Hashtable();
					dstRecord[AdminFields.GROUPID] = "= "+dstgroup;
					db.DeleteWhereas(dstRecord, DBTables.PERMS);
					DataSet ds = db.GetFieldsWhereas(srcRecord, DBTables.PERMS);
					tmpClone = new Hashtable();
					tmpClone[AdminFields.GROUPID] = "= " +dstgroup;
					DataRow[] dsrows = ds.Tables[0].Select(null, null, DataViewRowState.CurrentRows);
					for (int i = 0; i < dsrows.Length; i++)	// copy from source to destination
					{
						int idaction  = (int)dsrows[i][AdminFields.ACTIONS]; 
						int idsection = (int)dsrows[i][AdminFields.SECTIONS]; 
						tmpClone[AdminFields.ACTIONS]  = idaction;
						tmpClone[AdminFields.SECTIONS] = idsection;
						db.Insert(tmpClone, DBTables.PERMS);
					}
				}
				else
				{
					NullGroupException nge = new NullGroupException();
					throw nge;
				}
			}
			catch (NullGroupException nge)
			{
				NullGroupException wrappedEx = new 
					NullGroupException(
						ErrorManager.AddLayer(nge,	RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		//////////////////////////////////////////////////////////////////////////////////////	
				///<summary>
				///ClonePermissions: get all rows from permissions where id_group = source group and 
				///insert permissions with id_group = destination group. 
				///If destination group permissions
				///exists, will be erased when cloning.
				///</summary>
		private void ClonePermissions(string srcgroup, string dstgroup)
		{
			IDictionary srcRecord, dstRecord;

			try
			{
				srcRecord = new Hashtable();
				srcRecord[AdminFields.GROUPNAME] = "= '" + srcgroup + "'";
				dstRecord = new Hashtable();
				dstRecord[AdminFields.GROUPNAME] = "= '" + dstgroup + "'";
				int srcid = db.GetIdWhereas(srcRecord, DBTables.GROUPS);
				int dstid = db.GetIdWhereas(dstRecord, DBTables.GROUPS);
				this.ClonePermissions(srcid, dstid);
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		/////////////////////////////////////////////////////////////////////////////
				///<summary>
				///
				///</summary>
		public DataSet GetAllUsers(int groupid)
		{
			IDictionary groupRecord;
			
			try
			{
				groupRecord = new Hashtable();
				groupRecord[AdminFields.ID] = "= " + groupid;
				if (db.Exist(groupRecord, DBTables.GROUPS))
				{
					groupRecord.Clear();
					groupRecord[AdminFields.GROUPID] = "= " + groupid;
					return db.GetFieldsWhereas(groupRecord, DBTables.USERS);
				}
				else
				{
					NullGroupException nge = new NullGroupException();
					throw nge;
				}
			}
			catch (NullGroupException nge)
			{
				NullGroupException wrappedEx = new 
					NullGroupException(
						ErrorManager.AddLayer(nge,	RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		////////////////////////////////////////////////////////////////////////////////////////
				///<summary>
				///
				///</summary>
		public DataSet GetAllUsers(string groupName)
		{
			IDictionary groupRecord;
			
			try
			{
				groupRecord = new Hashtable();
				groupRecord[AdminFields.GROUPNAME] = "= '" + groupName + "'";
				if (db.Exist(groupRecord, DBTables.GROUPS))
				{
					groupRecord.Clear();
					groupRecord[AdminFields.GROUPID] = "= '" +groupName + "'";
					return db.GetFieldsWhereas(groupRecord, DBTables.USERS);
				}
				else
				{
					NullGroupException nge = new NullGroupException();
					throw nge;
				}
			}
			catch (NullGroupException nge)
			{
				NullGroupException wrappedEx = new 
					NullGroupException(
						ErrorManager.AddLayer(nge,	RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		//////////////////////////////////////////////////////////////////////////////////////
				///<summary>
				///
				///</summary>
		public void DeleteAllUsers(int groupid)
		{
			IDictionary userRecord, groupRecord;
			
			try
			{
				groupRecord = new Hashtable();
				groupRecord[AdminFields.ID] = "= " + groupid;
				if (db.Exist(groupRecord, DBTables.GROUPS))
				{
					userRecord = new Hashtable();
					userRecord[AdminFields.GROUPID] = "= " +groupid;
					db.DeleteWhereas(userRecord, DBTables.USERS);
				}
				else 
				{
					NullGroupException nge = new NullGroupException();
					throw nge;
				}
			}
			catch (NullGroupException nge)
			{
				NullGroupException wrappedEx = new 
					NullGroupException(
						ErrorManager.AddLayer(nge, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
						ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		//////////////////////////////////////////////////////////////////////////////////////
				///<summary>
				///
				///</summary>
		public GroupStruct GetGroupByName(string groupName)
		{
			IDictionary groupRecord;

			try
			{
				groupRecord = new Hashtable();
				groupRecord[AdminFields.GROUPNAME] = "= '" + groupName + "'";
				if (db.Exist(groupRecord, DBTables.GROUPS))
				{
					DataSet ds = db.GetFieldsWhereas(groupRecord, DBTables.GROUPS);
					GroupStruct gs  = new GroupStruct();
					gs.name         = groupName;
					gs.enterpriseid = DataTools.GetIntField (ds, AdminFields.EPRISEID);
					gs.id           = DataTools.GetIntField (ds, AdminFields.GROUPID);
					gs.published    = DataTools.GetBoolField(ds, AdminFields.PUBLISHED);
					return gs;
				}
				else
				{
					NullGroupException nge = new NullGroupException();
					throw nge;
				}
			}
			catch (NullGroupException nge)
			{
				NullGroupException wrappedEx = new 
					NullGroupException(
						ErrorManager.AddLayer(nge,	RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		//////////////////////////////////////////////////////////////////////////////////////
		/*
		public bool AskForNewGroup(string groupName, ref int groupid, ref bool isNewGroup, ref string error_msg)
		{
			IDictionary groupRecord; 
			DataSet ds;
			
			try
			{
				groupRecord = new Hashtable();
				groupRecord[AdminFields.GROUPNAME] = groupName;
				ds = db.GetFieldsWhereas(groupRecord, DBTables.GROUPS);
				if (ds != null)
				{
					groupid = (int) DataTools.GetIntField(ds, AdminFields.ID);
					isNewGroup = false;
				}
				else
					isNewGroup = true;
				return true;
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}			  
		}*/
		/////////////////////////////////////////////////////////////////////////////////////
	}
}

