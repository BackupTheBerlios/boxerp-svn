//
// LoginService.cs
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
using System.IO;
using System.Xml;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Services;
using Boxerp.Database;
using Boxerp.Tools;
using Boxerp.Errors;
using Boxerp.Debug;
using Boxerp.Exceptions;
using Boxerp.Objects;

namespace Boxerp.Objects
{

	[WebService (Description="Boxerp Login Service")]
 	public class LoginService : System.Web.Services.WebService
	{
		private ConcurrencyControllerObject concurrencyObj;
		private HistoryObject historyObj;
		private SessionsObject sessionsObj;
		private GenericDatabase db;
		
		// Constructor
		public LoginService(){}

		private void Initialize()
		{
			try
			{
				db = new GenericDatabase();
				db.Connect();
				ServicesTools.Connect2ObjectsServer(ref concurrencyObj, ref historyObj, 
									 							ref sessionsObj);
			}
			catch (Exception e)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(e, RunningClass.GetName(this), RunningClass.GetMethod())); 
				throw wrappedEx;
			}
		}
		///////////////////////////////////////////////////////////////////////////////////	

		[WebMethod (Description="Boxerp Login")]
		public bool login(string user, string passwd, 
							 	ref string sid, ref int gid, ref DataSet permissions)
		{
			// Query username and password and return permissions if login success.
			Initialize();
			DataSet ds;
			ArrayList fields = new ArrayList();
			fields.Add(AdminFields.ID);
			fields.Add(AdminFields.GROUPID);
			IDictionary constraints = new Hashtable();
			constraints[AdminFields.USERNAME] = "='" + user + "'";
			constraints[AdminFields.PASSWORD] = "='" + passwd + "'";
			try
			{
				ds = db.GetFieldsWhereas(constraints, fields, DBTables.USERS);
				//sid = sessionObj.GetSid(user);
				sid = "0"; 
				gid = (int) DataTools.GetIntField(ds, AdminFields.GROUPID);
				string gettingperms;
				gettingperms  = "SELECT ctrld_actions.name, ctrld_sections.name FROM ctrld_actions, ";
				gettingperms += "ctrld_sections, permissions WHERE ";
				gettingperms += "ctrld_actions.id = permissions.id_ctrld_actions AND ";
				gettingperms += "ctrld_sections.id = permissions.id_ctrld_sections ";
				gettingperms += "AND permissions.id_group = ";
				gettingperms += "(SELECT id_group FROM users WHERE id = " + gid + ")";
				db.ExecuteQuery(gettingperms);
				permissions = db.ActiveDataSet();
				historyObj.Register("User " + user + " has logged in");
				return(true);
			}
			catch (NullDataSetException e)
			{
				Exception wrappedEx = new Exception(
						ErrorManager.GenerateError(RunningClass.GetName(this), 
															RunningClass.GetMethod(), 
															ErrorsMsg.LOGIN_INCORRECT));
				throw wrappedEx;
			}
			catch (Exception e)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(e, RunningClass.GetName(this), RunningClass.GetMethod())); 
				throw wrappedEx;
			}
		}
	}
}

