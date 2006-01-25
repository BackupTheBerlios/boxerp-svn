//
// Enterprises.cs
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
	public struct EnterpriseStruct
	{
		public string name;
		public int    id;
		public bool   published;
		public string desc;
	}
	
			///<summary>
			///
			///</summary>
 	public class Enterprises 
	{
		GenericDatabase db;

		public Enterprises(GenericDatabase db)
		{
			this.db = db; 
		}
		//////////////////////////////////////////////////////////////	
				///<summary>
				///CreateEnterprise: eprisename must'n exist.
				///<return>New enterprise id </return>
				///</summary>
		public int CreateEnterprise(EnterpriseStruct eprise)
		{
			IDictionary epriseRecord, epriseFields;
			
			try
			{
				epriseRecord = new Hashtable();
				epriseRecord[AdminFields.EPRISENAME] = "= '" +eprise.name +"'"; 
				if (!db.Exist(epriseRecord, DBTables.ENTERPRISES))
				{
					epriseFields = new Hashtable();
					epriseFields[AdminFields.EPRISENAME] = eprise.name;
					epriseFields[AdminFields.EPRISEDESC] = eprise.desc;
					epriseFields[AdminFields.PUBLISHED]  = eprise.published;
					db.Insert(epriseFields, DBTables.ENTERPRISES, ref eprise.id);
					return eprise.id;
				}
				else 
				{
					EnterpriseAlreadyExistException eae = new EnterpriseAlreadyExistException();
					throw eae;
				}
			}
			catch (EnterpriseAlreadyExistException eae)
			{
				EnterpriseAlreadyExistException wrappedEx = new 
					EnterpriseAlreadyExistException(
						ErrorManager.AddLayer(eae, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		////////////////////////////////////////////////////////////////////////////////////	
				///<summary>
				///DeleteEnterprise: Only deletes a row in enterprise table. Groups  
				///must be deleted with DeleteAllGroups or throught Facade layout, before this
				///</summary>
		public void DelEnterprise(int enterpriseid)
		{
			IDictionary epriseRecord;

			try
			{
				epriseRecord = new Hashtable();
				epriseRecord[AdminFields.ID] = "= " +enterpriseid;
				if (db.Exist(epriseRecord, DBTables.ENTERPRISES))
				{
					db.DeleteWhereas(epriseRecord, DBTables.ENTERPRISES);
				}
				else
				{
					NullEnterpriseException nee = new NullEnterpriseException();
					throw nee;
				}
			}
			catch (NullEnterpriseException nee)
			{
				NullEnterpriseException wrappedEx = new 
					NullEnterpriseException(
						ErrorManager.AddLayer(nee,	RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
						ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		////////////////////////////////////////////////////////////////////////////////////
				///<summary>
				///ModifyEnterprise: epriseid must be an existent enterprise. 
				///neweprise must'n exist
				///</summary>
		public void ModifyEnterprise(int enterpriseid, EnterpriseStruct neweprise)
		{
			IDictionary thisEpriseRecord, epriseFields;
			
			try
			{
				thisEpriseRecord = new Hashtable();
				thisEpriseRecord[AdminFields.ID] = "= " +enterpriseid;
				if (db.Exist(thisEpriseRecord, DBTables.ENTERPRISES))
				{
						epriseFields = new Hashtable();
						epriseFields[AdminFields.EPRISENAME] = neweprise.name;
						epriseFields[AdminFields.EPRISEDESC] = neweprise.desc;
						epriseFields[AdminFields.PUBLISHED]  = neweprise.published;
						db.SetFieldsWhereas(thisEpriseRecord, epriseFields, DBTables.ENTERPRISES);
				}
				else
				{
					NullEnterpriseException nee = new NullEnterpriseException();
					throw nee;
				}
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
		///////////////////////////////////////////////////////////////////////////////////	
				///<summary>
				///
				///</summary>
		public DataSet GetAllGroups(int enterpriseid)
		{
			IDictionary epriseRecord;
			
			try
			{
				epriseRecord = new Hashtable();
				epriseRecord[AdminFields.ID] = "= " +enterpriseid;
				if (db.Exist(epriseRecord, DBTables.ENTERPRISES))
				{
					epriseRecord.Clear();
					epriseRecord[AdminFields.EPRISEID] = "= " +enterpriseid;
					return db.GetFieldsWhereas(epriseRecord, DBTables.GROUPS);
				}
				else
				{
					NullEnterpriseException nee = new NullEnterpriseException();
					throw nee;
				}
			}
			catch (NullDataSetException nds)
			{
				NullDataSetException wrappedEx = new 
					NullDataSetException(
						ErrorManager.AddLayer(nds, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (NullEnterpriseException nee)
			{
				NullEnterpriseException wrappedEx = new 
					NullEnterpriseException(
						ErrorManager.AddLayer(nee,	RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
						ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		///////////////////////////////////////////////////////////////////////////////////////	
				///<summary>
				///Get all groups from a given enterprise. 
				///If a NullDataSet exception occurs, the given enterprise
				///doesnt exist.
				///</summary>
		public DataSet GetAllGroups(string epriseName)
		{
			IDictionary epriseRecord;
			
			try
			{
				EnterpriseStruct es = GetEnterpriseByName(epriseName);
				epriseRecord = new Hashtable();
				epriseRecord[AdminFields.EPRISEID] = "= " + es.id +"";
				return db.GetFieldsWhereas(epriseRecord, DBTables.GROUPS);
			}
			catch (NullDataSetException nds)
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
		//////////////////////////////////////////////////////////////////////////////////////
				///<summary>
				///DeleteAllGroups: delete all groups for a given enterprise, 
				///deleting groups permissions
				///and group users. 
				///If a NullDataSet exception occurs then, 
				///some of the groups or permissiosn doesnt exist.
				///</summary>
		public void DeleteAllGroups(int enterpriseid)
		{
			IDictionary groupRecord, epriseRecord;
			ArrayList fields;
			
			try
			{
				epriseRecord = new Hashtable();
				epriseRecord[AdminFields.ID] = "= " + enterpriseid;
				if (db.Exist(epriseRecord, DBTables.ENTERPRISES))
				{
					epriseRecord.Clear();
					epriseRecord[AdminFields.EPRISEID] = "= " +enterpriseid;
					DataSet ds;
					fields = new ArrayList();
					fields.Add(AdminFields.ID);
					ds = db.GetFieldsWhereas(epriseRecord, fields, DBTables.GROUPS);
					DataRow[] dsrows = ds.Tables[0].Select(null, null, DataViewRowState.CurrentRows);
					groupRecord = new Hashtable();
					for (int i = 0; i < dsrows.Length; i++)	
					{
						int id_group  = (int)dsrows[0][0]; 
						groupRecord[AdminFields.ID] = id_group;
						db.DeleteWhereas(groupRecord, DBTables.GROUPS);
						groupRecord.Clear();
						groupRecord[AdminFields.GROUPID] = id_group;
						db.DeleteWhereas(groupRecord, DBTables.PERMS);
						db.DeleteWhereas(groupRecord, DBTables.USERS);
					}
				}
				else 
				{
					NullEnterpriseException nee = new NullEnterpriseException();
					throw nee;
				}
			}
			catch (NullDataSetException nds)
			{
				NullDataSetException wrappedEx = new 
					NullDataSetException(
						ErrorManager.AddLayer(nds, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (NullEnterpriseException nee)
			{
				NullEnterpriseException wrappedEx = new 
					NullEnterpriseException(
						ErrorManager.AddLayer(nee,	RunningClass.GetName(this), RunningClass.GetMethod()));
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
		
		public EnterpriseStruct GetEnterpriseByName(string epriseName)
		{
			IDictionary epriseRecord;

			try
			{
				epriseRecord = new Hashtable();
				epriseRecord[AdminFields.EPRISENAME] = "= '" +epriseName +"'";
				if (db.Exist(epriseRecord, DBTables.ENTERPRISES))
				{
					DataSet ds = db.GetFieldsWhereas(epriseRecord, DBTables.ENTERPRISES);
					EnterpriseStruct es = new EnterpriseStruct();
					es.name       = epriseName;
					es.id         = DataTools.GetIntField (ds, AdminFields.ID);
					es.desc       = DataTools.GetTextField(ds, AdminFields.EPRISEDESC);
					es.published  = DataTools.GetBoolField(ds, AdminFields.PUBLISHED);
					return es;
				}
				else
				{
					NullEnterpriseException nee = new NullEnterpriseException();
					throw nee;
				}
			}
			catch (NullEnterpriseException nee)
			{
				NullEnterpriseException wrappedEx = new 
					NullEnterpriseException(
						ErrorManager.AddLayer(nee,	RunningClass.GetName(this), RunningClass.GetMethod()));
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

		public DataSet GetAllEnterprises()
		{
			try
			{
				return db.GetFields(DBTables.ENTERPRISES);
			}
			catch (NullDataSetException nds)
			{
				NullDataSetException wrappedEx = new 
					NullDataSetException(
						ErrorManager.AddLayer(nds, RunningClass.GetName(this), RunningClass.GetMethod()));
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
	}
}

