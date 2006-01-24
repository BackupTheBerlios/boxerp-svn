//
// Users.cs
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
	public struct UserStruct
	{
		public string name;
		public int    id;
		public string password;
		public int    groupid;
		public bool   published;
	}

			///<summary>
			///User manage system users; create, modify, delete, change group, change permissions...
			///</summary>
 	public class Users 
	{
		GenericDatabase db;
		
		public Users (GenericDatabase db)
		{
			this.db = db;
		}
		///////////////////////////////////////////////////////////////////////////////////	
				///<summary>
				///	CreateUsers: insert a new user into table users. username must'n exist. 
				///	id_group must be an existent group
				///	<param name="group"> id_group, must exists in groups table</param>
				///	<return>New user id </return>
				///</summary>	
		public int CreateUser(UserStruct user)
		{
			IDictionary groupRecord, userRecord, userFields;

			try
			{
				groupRecord = new Hashtable();
				groupRecord[AdminFields.ID] = "= " +user.groupid; 
				userRecord = new Hashtable();
				userRecord[AdminFields.USERNAME] = "= '" + user.name + "'";
				if (db.Exist(groupRecord, DBTables.GROUPS)) 
				{
					if (!db.Exist(userRecord, DBTables.USERS))
					{
						userFields = new Hashtable();
						userFields[AdminFields.USERNAME]  = user.name;
						userFields[AdminFields.PASSWORD]  = user.password;
						userFields[AdminFields.GROUPID]   = user.groupid;
						userFields[AdminFields.PUBLISHED] = user.published;
						db.Insert(userFields, DBTables.USERS, ref user.id);
						return user.id;
					}
					else
					{
						UserAlreadyExistException uae = new UserAlreadyExistException();
						throw uae;
					}
				}
				else 
				{
					NullGroupException nge = new NullGroupException();
					throw nge;
				}
			}
			catch (UserAlreadyExistException uae)
			{
				UserAlreadyExistException wrappedEx = new 
					UserAlreadyExistException(
						ErrorManager.AddLayer(uae, RunningClass.GetName(this), RunningClass.GetMethod()));
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
		///////////////////////////////////////////////////////////////////////////////////
			///<summary>
			///Delete user from users table
			///</summary>
		public void DeleteUser(int userid)
		{
			try
			{
				IDictionary userRecord = new Hashtable();
				userRecord[AdminFields.ID] = "= " +userid;
				db.DeleteWhereas(userRecord, DBTables.USERS);
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}

		///////////////////////////////////////////////////////////////////////////////////	
				///<summary>
				///ModifyUser: username must be an existent user. newuser must'n exist. 
				///groupid must be an existent group
				///</summary>
		public void ModifyUser(int userid, UserStruct newuser)
		{
			IDictionary userRecord, groupRecord, thisUserRecord, userFields;
			
			try
			{
				groupRecord = new Hashtable();
				groupRecord[AdminFields.GROUPID] = "= " +newuser.groupid;
				userRecord = new Hashtable();
				userRecord[AdminFields.USERNAME] = "= '" + newuser.name + "'";
				if (db.Exist(groupRecord, DBTables.GROUPS)) 
				{
					if (!db.Exist(userRecord, DBTables.USERS))
					{
						thisUserRecord = new Hashtable();
						thisUserRecord[AdminFields.ID] = "= " +userid; 
						userFields = new Hashtable();
						userFields[AdminFields.USERNAME]  = newuser.name;
						userFields[AdminFields.PASSWORD]  = newuser.password;
						userFields[AdminFields.GROUPID]   = newuser.groupid;
						userFields[AdminFields.PUBLISHED] = newuser.published;
						db.SetFieldsWhereas(thisUserRecord, userFields, DBTables.USERS);
					}
					else
					{
						UserAlreadyExistException uae = new UserAlreadyExistException();
						throw uae;
					}
				}
				else
				{
					NullGroupException nge = new NullGroupException();
					throw nge;
				}
			}
			catch (UserAlreadyExistException uae)
			{
				UserAlreadyExistException wrappedEx = new 
					UserAlreadyExistException(
						ErrorManager.AddLayer(uae,RunningClass.GetName(this), RunningClass.GetMethod()));
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
 		/////////////////////////////////////////////////////////////////////////////////////		
				///<summary>
				///
				///</summary>
		public UserStruct GetUserByName(string userName)
		{
			IDictionary userRecord;  
			
			try
			{
				userRecord = new Hashtable();
				userRecord[AdminFields.USERNAME] = "= '" +userName +"'";
				DataSet ds = db.GetFieldsWhereas(userRecord, DBTables.USERS);
				UserStruct us = new UserStruct();
				us.name       = userName;
				us.password   = (string) DataTools.GetTextField(ds, AdminFields.PASSWORD);
				us.groupid    = (int)    DataTools.GetIntField (ds, AdminFields.GROUPID);
				us.id         = (int)    DataTools.GetIntField (ds, AdminFields.ID);
				us.published  = (bool)   DataTools.GetBoolField(ds, AdminFields.PUBLISHED);
				return us;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new 
					Exception(
						ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}			  
		}
 		/////////////////////////////////////////////////////////////////////////////////////		
		/*
		public bool AskForNewUser(string userName, ref int userid, ref bool isNewUser, ref string error_msg)
		{
			IDictionary userRecord; 
			DataSet ds;
			
			try
			{
				userRecord = new Hashtable();
				userRecord[AdminFields.USERNAME] = userName;
				ds = db.GetFieldsWhereas(userRecord, DBTables.USERS);
				if (ds != null)
				{
					userid = (int) DataTools.GetIntField(ds, AdminFields.ID);
					isNewUser = false;
				}
				else
					isNewUser = true;
				return true;
			}
			catch (Exception ex)
			{
				error_msg = "SE HA PRODUCIDO UNA EXCEPCION: " + ex.Message;
				return false;
			}			  
		}
		*/
		/////////////////////////////////////////////////////////////////////////////////////
	}

}
