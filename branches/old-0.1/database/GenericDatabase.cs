//
// GenericDatabase.cs
//
// Authors:
// 	Zebenzui Perez Ramos <zebenperez@shidix.com>
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
using System.Xml;
using System.IO;
using Boxerp.Debug;
using Boxerp.Errors;
using Boxerp.Exceptions;

namespace Boxerp.Database
{
			///<summary>
			///Database abstraction layer. You can change db conector and this 
			///layer will doesnt change. 
			///Query methods with constraints, are called xxxWhereas 
			///and the first parameter is a IDictionary 
			///with contraint. Fields, constraints, and ordersby  
			///are always passed as IDictionary or IEnumerable, 
			///no as strings. 
			///The order of arguments is from first to last: 
			///contraints, orderby, fields, strings, int, others
			///</summary>
	public class GenericDatabase
	{
		Database db;
		string dbServer, dbName, dbPort, dbUser, dbPass;
		string selectedTable;

			/// <summary>
			/// Reads database connection configuration and create a new instance of 
			/// the selected database type.
			/// </summary>
		public GenericDatabase()
		{
			try
			{
				GetConnectionParameters();
				db = new Database(dbServer, dbName, dbPort, dbUser, dbPass);
			}
			catch (Exception e)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(e, RunningClass.GetName(this), RunningClass.GetMethod())); 
				throw wrappedEx;
			}

		}
		////////////////////////////////////////////////////////////////////////////////
				///<summary>
				///Class constructor. 
				///</summary>
				///<param name="dbServer">Database server name or ip address</param>
				///<param name="dbName">Database name</param>
				///<param name="dbPort">Database connection port</param>
				///<param name="dbUser">Database owner</param>
				///<param name="dbPass">Database owner password</param>
		public GenericDatabase (string dbServer, string dbName, string dbPort, 
							 			string dbUser, string dbPass)
		{
			try
			{
				db = new Database(dbServer, dbName, dbPort, dbUser, dbPass);
			}
			catch (Exception e)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(e, RunningClass.GetName(this), RunningClass.GetMethod())); 
				throw wrappedEx;
			}
		}
		///////////////////////////////////////////////////////////////////////////////////
				/// <summary>
				/// Read database connection parameters from database.config
				/// At the moment only the first configuration is readed and type is PostgreSQL.
				/// </summary>
		private void GetConnectionParameters()
		{
			try
			{
				XmlDocument doc = new XmlDocument();
				//doc.Load(Path.Combine(Boxerp.Defines.SERVER_DIR,  "database.config"));
				doc.Load(Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, 
																"server/database.config")); 
				//doc.Load("/home/carlosble/boxerp/svn/develop/src/server/database.config");
				XmlNodeList xmlNodeList = doc.GetElementsByTagName("database");
				XmlNode node = xmlNodeList[0]; // Get only the first database configuration
				XmlNodeList childNodeList = node.ChildNodes;
				foreach (XmlNode childNode in childNodeList) 
				{
					switch (childNode.Name)
					{
						case "dbServer":	this.dbServer = childNode.InnerText;
								  			break;
						case "dbPort":		this.dbPort = childNode.InnerText;
								  			break;
						case "dbName":		this.dbName = childNode.InnerText; 
								  			break;
						case "dbUser":		this.dbUser = childNode.InnerText;
								  			break;
						case "dbPass":		this.dbPass = childNode.InnerText;
								  			break;
					}
				}
			}
			catch (Exception e)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(e, RunningClass.GetName(this), RunningClass.GetMethod())); 
				throw wrappedEx;
			}
		}
		///////////////////////////////////////////////////////////////////////////////////
		
		public string Table
		{
			get {return selectedTable;}
			set {selectedTable = value;}
		}
		
		/*****************************************************************
	 	*									Base Database Functions				 *
	 	***************************************************************/

		public void Connect()
		{
			try 
			{
				db.Connect();
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		//////////////////////////////////////////////////////////////////////////

		public void Reconnect(string dbServer, string dbName, 
							 		string dbPort, string dbUser, string dbPass)
		{
			try
			{
				db.Reconnect(dbServer, dbName, dbPort, dbUser, dbPass);
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		////////////////////////////////////////////////////////////////////////////////
		
		public void Reconnect()
		{
			try
			{
				db.Reconnect();
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		/////////////////////////////////////////////////////////////////////////////////
		/*
		public void ExecuteQuery (string str_query)
		{
			try
			{
				db.ExecuteQuery(str_query);	
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}*/
		////////////////////////////////////////////////////////////////////////////

		public IDbTransaction BeginTransaction ()
		{
			try
			{
				return db.BeginTransaction();
			} 
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		/////////////////////////////////////////////////////////////////////////////////////

		public void Commit(IDbTransaction transaction)
		{
			try
			{
				db.Commit(transaction);
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		/////////////////////////////////////////////////////////////////////////////////////

		public void Rollback(IDbTransaction transaction)
		{
			try
			{
				db.Rollback(transaction);
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		/////////////////////////////////////////////////////////////////////////////////////
		/*	
		public void ExecuteTransaction ()
		{
			try
			{
				db.ExecuteTransaction();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}*/
		/////////////////////////////////////////////////////////////////////////////////////
	
		public void ExecuteQuery (string str_query)
		{
			try
			{
				db.ExecuteQuery(str_query);
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		///////////////////////////////////////////////////////////////////////////

		public DataSet ActiveDataSet ()
		{
			try 
			{
				return db.ActiveDataSet();
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
	
		/*******************************************************************************
	 	*							GenericDatabase Functions										  *
	 	********************************************************************/
		
		public int GetMaxId()
		{
			try 
			{
				db.ExecuteQuery("SELECT max(id) FROM " + selectedTable);
				DataSet ds = db.ActiveDataSet();
				return (int) ds.Tables[0].Rows[0][0];
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		////////////////////////////////////////////////////////////////////////////////
				///<summary>
				///Overloaded
				///<param name="table">Database table to access </param>
				///</summary>	
		public int GetMaxId(string table)
		{
			try 
			{
				db.ExecuteQuery("SELECT max(id) FROM " + table);
				DataSet ds = db.ActiveDataSet();
				return (int) ds.Tables[0].Rows[0][0];
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		/////////////////////////////////////////////////////////////////////

		public long RowCount()
		{
			try 
			{
				string sql = "SELECT count(*) FROM " + selectedTable;
				db.ExecuteQuery(sql);
				DataSet ds = db.ActiveDataSet();
				return ((long)ds.Tables[0].Rows[0][0]);
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
				///Overloaded
				///<param name="table">Database table to access </param>
				///</summary>	
		public long RowCount(string table)
		{
			try 
			{
				string sql = "SELECT count(*) FROM " + table;
				db.ExecuteQuery(sql);
				DataSet ds = db.ActiveDataSet();
				return ((long)ds.Tables[0].Rows[0][0]);
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		///////////////////////////////////////////////////////////////////////////////

		public long RowCountWhereas(IDictionary constraint)
		{	  
			ICollection constraint_keys;
			string sql;
			try
			{
				sql = "SELECT count(*) FROM " + selectedTable;
				constraint_keys = constraint.Keys;
				if (constraint_keys.Count != 0)
					sql += " WHERE ";
				foreach (string key in constraint_keys)
					sql += key + constraint[key] + " AND ";
				sql = sql.Substring(0,(sql.Length - 4)); // Remove invalid chars at the end of string
				db.ExecuteQuery(sql); 
				DataSet ds = db.ActiveDataSet();
				return ((long)ds.Tables[0].Rows[0][0]);
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
				///Overloaded
				///<param name="table">Database table to access </param>
				///</summary>	
		public long RowCountWhereas(IDictionary constraint, string table)
		{	  
			ICollection constraint_keys;
			string sql;
			try
			{
				sql = "SELECT count(*) FROM " + table;
				constraint_keys = constraint.Keys;
				if (constraint_keys.Count != 0)
					sql += " WHERE ";
				foreach (string key in constraint_keys)
					sql += key + constraint[key] + " AND ";
				sql = sql.Substring(0,(sql.Length - 4)); // Remove invalid chars at the end of string
				db.ExecuteQuery(sql);
				DataSet ds = db.ActiveDataSet();
				return ((long)ds.Tables[0].Rows[0][0]);
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
				///<param name="table">Database table to access </param>
				///</summary>	
		public bool Exist(IDictionary constraint, string table)
		{	  
			ICollection constraint_keys;
			string sql;
			try
			{
				sql = "SELECT count(*) FROM " + table;
				constraint_keys = constraint.Keys;
				if (constraint_keys.Count != 0)
					sql += " WHERE ";
				foreach (string key in constraint_keys)
					sql += key + constraint[key] + " AND ";
				sql = sql.Substring(0,(sql.Length - 4)); // Remove invalid chars at the end of string
				db.ExecuteQuery(sql);
				DataSet ds = db.ActiveDataSet();
				if ((long)ds.Tables[0].Rows[0][0] > 0)
					return true;
				else
					return false;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		///////////////////////////////////////////////////////////////////////////
	
		public int GetNewId()
		{	
			try
			{
				if (selectedTable != "")
				{
					int new_id = this.GetMaxId() + 1;
					if (new_id != -1)
						return (new_id);
					else
						return -1;
				}
				else
					return -1;
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
				///Overloaded
				///<param name="table">Database table to access </param>
				///</summary>	
		public int GetNewId(string table)
		{	
			try
			{
				if (table != "")
				{
					int new_id = this.GetMaxId(table) + 1;
					if (new_id != -1)
						return (new_id);
					else
						return -1;
				}
				else
					return -1;
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
				/// Insert columns on specific table
				///</summary>
				///<param name="fields">it is a dictionary with keys and values for the table </param>
				///<returns></returns>
		public void Insert(IDictionary fields, ref int id)
		{
			ICollection keys;
			string index_str = "", values_str = "", sql;

			try 
			{
				id = this.GetNewId();
				if (id != -1)
				{
					fields["id"] = id;
					keys = fields.Keys;
					foreach (string key in keys)
					{
						index_str += key + ",";
						values_str += "'" + fields[key] + "',";
					}
					index_str = index_str.Substring(0,(index_str.Length - 1));
					values_str = values_str.Substring(0,(values_str.Length - 1));
					sql = "INSERT INTO " + selectedTable + " (" + index_str + ") VALUES (" + 
							  values_str + ");"; 
					db.ExecuteQuery(sql);
				}
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		///////////////////////////////////////////////////////////////////////////////
		
		public void Insert(IDictionary fields)
		{
			ICollection keys;
			string index_str = "", values_str = "", sql;
			int id;

			try 
			{
				id = this.GetNewId();
				if (id != -1)
				{
					fields["id"] = id;
					keys = fields.Keys;
					foreach (string key in keys)
					{
						index_str += key + ",";
						values_str += "'" + fields[key] + "',";
					}
					index_str = index_str.Substring(0,(index_str.Length - 1));
					values_str = values_str.Substring(0,(values_str.Length - 1));
					sql = "INSERT INTO " + selectedTable + " (" + index_str + ") VALUES (" + 
							  values_str + ");"; 
					db.ExecuteQuery(sql);
				}
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		/////////////////////////////////////////////////////////////////////////////
		
		public void Insert(IDictionary fields, string table)
		{
			ICollection keys;
			string index_str = "", values_str = "", sql;
			int id;

			try 
			{
				id = this.GetNewId(table);
				if (id != -1)
				{
					fields["id"] = id;
					keys = fields.Keys;
					foreach (string key in keys)
					{
						index_str += key + ",";
						values_str += "'" + fields[key] + "',";
					}
					index_str = index_str.Substring(0,(index_str.Length - 1));
					values_str = values_str.Substring(0,(values_str.Length - 1));
					sql = "INSERT INTO " + table + " (" + index_str + ") VALUES (" + 
							  values_str + ");"; 
					db.ExecuteQuery(sql);
				}
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		//////////////////////////////////////////////////////////////////////////////
		
		public void Insert(IDictionary fields, string table, ref int id)
		{
			ICollection keys;
			string index_str = "", values_str = "", sql;

			try 
			{
				id = this.GetNewId(table);
				if (id != -1)
				{
					fields["id"] = id;
					keys = fields.Keys;
					foreach (string key in keys)
					{
						index_str += key + ",";
						values_str += "'" + fields[key] + "',";
					}
					index_str = index_str.Substring(0,(index_str.Length - 1));
					values_str = values_str.Substring(0,(values_str.Length - 1));
					sql = "INSERT INTO " + table + " (" + index_str + ") VALUES (" + values_str + ");"; 
					db.ExecuteQuery(sql);
				}
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		////////////////////////////////////////////////////////////////////////////////

		public void Delete()
		{
			try
			{
				string sql = "DELETE FROM " + selectedTable;
				db.ExecuteQuery(sql);
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		/////////////////////////////////////////////////////////////////////////////////
				///<summary>
				///Overloaded
				///<param name="table">Database table to access </param>
				///</summary>	
		public void Delete(string table)
		{
			try
			{
				string sql = "DELETE FROM " + table;
				db.ExecuteQuery(sql);
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
				/// Delete rows on specific table 
				///</summary>
				///<param name="constraint">it is a dictionary with the constraint to delete</param>
				///<returns>Una cadena vacia o un codigo de error si ha habido problemas</returns>
		public void DeleteWhereas(IDictionary constraint)
		{
			ICollection constraint_keys;
			string sql;

			try
			{
				sql = "DELETE FROM " + selectedTable;
				constraint_keys = constraint.Keys;
				if (constraint_keys.Count != 0)
					sql += " WHERE ";
				foreach (string key in constraint_keys)
					sql += key + constraint[key] + " AND ";
				sql = sql.Substring(0,(sql.Length - 4)); // Remove invalid chars at the end of string
				db.ExecuteQuery(sql);
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		//////////////////////////////////////////////////////////////////////////////////
				///<summary>
				/// Delete rows on specific table 
				///</summary>
				///<param name="constraint">it is a dictionary with the constraint to delete</param>
				///<returns>Una cadena vacia o un codigo de error si ha habido problemas</returns>
		public void DeleteWhereas(IDictionary constraint, string table)
		{
			ICollection constraint_keys;
			string sql;

			try
			{
				sql = "DELETE FROM " + table;
				constraint_keys = constraint.Keys;
				if (constraint_keys.Count != 0)
					sql += " WHERE ";
				foreach (string key in constraint_keys)
					sql += key + constraint[key] + " AND ";
				sql = sql.Substring(0,(sql.Length - 4)); // Remove invalid chars at the end of string
				db.ExecuteQuery(sql);
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		/////////////////////////////////////////////////////////////////////////////

		public DataSet GetFields ()
		{
			try
			{
				string sql = "SELECT * FROM " + selectedTable;
				db.ExecuteQuery(sql);
				DataSet tmpDs = db.ActiveDataSet();
				if (tmpDs.Tables[0].Rows.Count == 0)
				{
					NullDataSetException nre = new NullDataSetException();
					throw nre;
				}
				else
					return tmpDs;
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
				///Overloaded
				///<param name="table">Database table to access </param>
				///</summary>	
		public DataSet GetFields (string table)
		{
			try 
			{
				string sql = "SELECT * FROM " + table;
				db.ExecuteQuery(sql);
				DataSet tmpDs = db.ActiveDataSet();
				if (tmpDs.Tables[0].Rows.Count == 0)
				{
					NullDataSetException nds = new NullDataSetException();
					throw nds;
				}
				else
					return tmpDs;
			}
			catch (NullDataSetException nds)
			{
				NullDataSetException wrappedEx = new 
					NullDataSetException(
						ErrorManager.AddLayer(nds, RunningClass.GetName(this), 
								  					RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		/////////////////////////////////////////////////////////////////////////

		public DataSet GetFields (IEnumerable fields)
		{
			try
			{
				string sql = "SELECT " ; 
				IEnumerator fieldsIe = fields.GetEnumerator(); 
				while (fieldsIe.MoveNext()) 
					sql += fieldsIe.Current + ", ";
				sql.Substring(0, (sql.Length - 2));
				sql += " FROM " + selectedTable;
				db.ExecuteQuery(sql);
				DataSet tmpDs = db.ActiveDataSet();
				if (tmpDs.Tables[0].Rows.Count == 0)
				{
					NullDataSetException nds = new NullDataSetException();
					throw nds;
				}
				else
					return tmpDs;
			}
			catch (NullDataSetException nds)
			{
				NullDataSetException wrappedEx = new 
					NullDataSetException(
						ErrorManager.AddLayer(nds, RunningClass.GetName(this), 
								  					RunningClass.GetMethod()));
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
				///Overloaded
				///<param name="table">Database table to access </param>
				///</summary>	
		public DataSet GetFields (IEnumerable fields, string table)
		{
			try
			{
				string sql = "SELECT " ; 
				IEnumerator fieldsIe = fields.GetEnumerator(); 
				while (fieldsIe.MoveNext()) 
					sql += fieldsIe.Current + ", ";
				sql.Substring(0, (sql.Length - 2));
				sql += " FROM " + table;
				db.ExecuteQuery(sql);
				DataSet tmpDs = db.ActiveDataSet();
				if (tmpDs.Tables[0].Rows.Count == 0)
				{
					NullDataSetException nre = new NullDataSetException();
					throw nre;
				}
				else
					return tmpDs;
			}
			catch (NullDataSetException nds)
			{
				NullDataSetException wrappedEx = new 
					NullDataSetException(
						ErrorManager.AddLayer(nds, RunningClass.GetName(this), 
								  					RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		/////////////////////////////////////////////////////////////////////////////////
				///<summary>
				/// Get fields of a specific table 
				///</summary>
				///<param name="fields">it is a string with specific fields</param>
				///<param name="constraint">it is a dictionary with the constraint. 
        ///The operator has to be include in the several values 
        ///(p.e.: "=miname")
				///</param>
				///<returns>DataSet with requires values</returns>
		public DataSet GetFieldsWhereas (IDictionary constraint, IEnumerable fields)
		{
			ICollection constraint_keys;
			string sql;
			
			try
			{
				sql = "SELECT " ; 
				IEnumerator fieldsIe = fields.GetEnumerator(); 
				while (fieldsIe.MoveNext()) 
					sql += fieldsIe.Current + ", ";
				sql.Substring(0, (sql.Length - 2));
				sql += " FROM " + selectedTable;
				constraint_keys = constraint.Keys;
				if (constraint_keys.Count != 0)
					sql += " WHERE ";
				foreach (string key in constraint_keys)
					sql += key + constraint[key] + " AND ";
				sql = sql.Substring(0, (sql.Length - 5)); 	// Erase latest AND
				db.ExecuteQuery(sql);
				DataSet tmpDs = db.ActiveDataSet();
				if (tmpDs.Tables[0].Rows.Count == 0)
				{
					NullDataSetException nds = new NullDataSetException();
					throw nds;
				}
				else
					return tmpDs;
			}
			catch (NullDataSetException nds)
			{
				NullDataSetException wrappedEx = new 
					NullDataSetException(
						ErrorManager.AddLayer(nds, RunningClass.GetName(this), 
								  					RunningClass.GetMethod()));
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
				///Overloaded
				///<param name="table">Database table to access </param>
				///</summary>	
		public DataSet GetFieldsWhereas(IDictionary constraint, IEnumerable fields, string table)
		{
			ICollection constraint_keys;
			string sql;
			
			try
			{
				sql = "SELECT " ; 
				IEnumerator fieldsIe = fields.GetEnumerator(); 
				while (fieldsIe.MoveNext()) 
					sql += fieldsIe.Current + ", ";
				sql = sql.Substring(0, (sql.Length - 2));
				sql += " FROM " + table;
				constraint_keys = constraint.Keys;
				if (constraint_keys.Count != 0)
					sql += " WHERE ";
				foreach (string key in constraint_keys)
					sql += key + constraint[key] + " AND ";
				sql = sql.Substring(0, (sql.Length - 5)); 	// Erase latest AND
				db.ExecuteQuery(sql);
				DataSet tmpDs = db.ActiveDataSet();
				if (tmpDs.Tables[0].Rows.Count == 0) //.Rows.Count == 0)
				{
					NullDataSetException nre = new NullDataSetException();
					throw nre;
				}
				else
				{
					return tmpDs;
				}
			}
			catch (NullDataSetException nds)
			{
				NullDataSetException wrappedEx = new 
					NullDataSetException(
						ErrorManager.AddLayer(nds, RunningClass.GetName(this), 
								  					RunningClass.GetMethod()));
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
				/// Get fields of a specific table 
				///</summary>
				///<param name="fields">it is a string with specific fields</param>
				///<param name="constraint">it is a dictionary with the constraint
                ///The operator has to be include in the several values 
                ///(p.e.: "=miname" )</param>
				///<param name="orderby">it is a dictionary with order constraint</param>
				///<returns></returns>
		public DataSet GetFieldsWhereasAndOrder(IDictionary constraint, 
							 			IDictionary orderby, IEnumerable fields)
		{
			ICollection constraint_keys, orderby_keys;
			string sql;
			
			try
			{
				sql = "SELECT " ; 
				IEnumerator fieldsIe = fields.GetEnumerator(); 
				while (fieldsIe.MoveNext()) 
					sql += fieldsIe.Current + ", ";
				sql.Substring(0, (sql.Length - 2));
				sql += " FROM " + selectedTable;
				constraint_keys = constraint.Keys;
				if (constraint_keys.Count != 0)
					sql += " WHERE ";
				foreach (string key in constraint_keys)
					sql += key + constraint[key] + " AND ";
				sql = sql.Substring(0, (sql.Length - 5)); // Remove invalid chars at the end	
				orderby_keys = orderby.Keys;
				if (orderby_keys.Count != 0)
					sql += " ORDER BY ";
				foreach (string key in orderby_keys)
					sql += key + " " + orderby[key] + ", ";
				sql = sql.Substring(0, (sql.Length - 2)); 
				db.ExecuteQuery(sql);
				DataSet tmpDs = db.ActiveDataSet();
				if (tmpDs.Tables[0].Rows.Count == 0)
				{
					NullDataSetException nre = new NullDataSetException();
					throw nre;
				}
				else
					return tmpDs;
			}
			catch (NullDataSetException nds)
			{
				NullDataSetException wrappedEx = new 
					NullDataSetException(
						ErrorManager.AddLayer(nds, RunningClass.GetName(this), 
								  			RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}	
		}
		///////////////////////////////////////////////////////////////////////////
				///<summary>
				///Overloaded
				///<param name="table">Database table to access </param>
				///</summary>	
		public DataSet GetFieldsWhereasAndOrder(IDictionary constraint, IDictionary orderby, 
							 IEnumerable fields, string table)
		{
			ICollection constraint_keys, orderby_keys;
			string sql;
			
			try
			{
				sql = "SELECT " ; 
				IEnumerator fieldsIe = fields.GetEnumerator(); 
				while (fieldsIe.MoveNext()) 
					sql += fieldsIe.Current + ", ";
				sql.Substring(0, (sql.Length - 2));
				sql += " FROM " + table;
				constraint_keys = constraint.Keys;
				if (constraint_keys.Count != 0)
					sql += " WHERE ";
				foreach (string key in constraint_keys)
					sql += key + constraint[key] + " AND ";
				sql = sql.Substring(0, (sql.Length - 5)); // Remove invalid chars at the end	
				orderby_keys = orderby.Keys;
				if (orderby_keys.Count != 0)
					sql += " ORDER BY ";
				foreach (string key in orderby_keys)
					sql += key + " " + orderby[key] + ", ";
				sql = sql.Substring(0, (sql.Length - 2)); 
				db.ExecuteQuery(sql);
				DataSet tmpDs = db.ActiveDataSet();
				if (tmpDs.Tables[0].Rows.Count == 0)
				{
					NullDataSetException nre = new NullDataSetException();
					throw nre;
				}
				else
					return tmpDs;
			}
			catch (NullDataSetException nds)
			{
				NullDataSetException wrappedEx = new 
					NullDataSetException(
						ErrorManager.AddLayer(nds, RunningClass.GetName(this), 
											RunningClass.GetMethod()));
				throw wrappedEx;
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}	
		}
		//////////////////////////////////////////////////////////////////////
		
		public DataSet GetFieldsWhereas(IDictionary constraint)
		{
			ICollection constraint_keys;
			string sql;
			
			try
			{
				sql = "SELECT * FROM " + selectedTable;
				constraint_keys = constraint.Keys;
				if (constraint_keys.Count != 0)
					sql += " WHERE ";
				foreach (string key in constraint_keys)
					sql += key + constraint[key] + " AND ";
				sql = sql.Substring(0, (sql.Length - 5)); 	// Erase latest AND
				db.ExecuteQuery(sql);
				DataSet tmpDs = db.ActiveDataSet();
				if (tmpDs.Tables[0].Rows.Count == 0)
				{
					NullDataSetException nre = new NullDataSetException();
					throw nre;
				}
				else
					return tmpDs;
			}
			catch (NullDataSetException nds)
			{
				NullDataSetException wrappedEx = new 
					NullDataSetException(
						ErrorManager.AddLayer(nds, RunningClass.GetName(this), 
								  				RunningClass.GetMethod()));
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
				///Overloaded
				///<param name="table">Database table to access </param>
				///</summary>	
		public DataSet GetFieldsWhereas(IDictionary constraint, string table)
		{
			ICollection constraint_keys;
			string sql;
			
			try
			{
				sql = "SELECT * FROM " + table;
				constraint_keys = constraint.Keys;
				if (constraint_keys.Count != 0)
					sql += " WHERE ";
				foreach (string key in constraint_keys)
					sql += key + constraint[key] + " AND ";
				sql = sql.Substring(0, (sql.Length - 5)); 	// Erase latest AND
				db.ExecuteQuery(sql);
				DataSet tmpDs = db.ActiveDataSet();
				if (tmpDs.Tables[0].Rows.Count == 0)
				{
					NullDataSetException nre = new NullDataSetException();
					throw nre;
				}
				else
					return tmpDs;
			}
			catch (NullDataSetException nds)
			{
				NullDataSetException wrappedEx = new 
					NullDataSetException(
						ErrorManager.AddLayer(nds, RunningClass.GetName(this), 
								  				RunningClass.GetMethod()));
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
				///<summary>
				/// Update fields of a specific table 
				///</summary>
				///<param name="fields">it is a dictionary with keys and values for the table</param>
				///<returns></returns>
		public void SetFields (IDictionary fields)
		{
			ICollection fields_keys;
			string sql;
			
			try
			{
				sql = "UPDATE " + selectedTable + " SET ";
				fields_keys = fields.Keys;
				foreach (string key in fields_keys)
					sql += key + "=" + "'" + fields[key] + "', ";
				sql.Substring(0, (sql.Length - 2));
				db.ExecuteQuery(sql);
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		////////////////////////////////////////////////////////////////////
				///<summary>
				///Overloaded
				///<param name="table">Database table to access </param>
				///</summary>	
		public void SetFields (IDictionary fields, string table)
		{
			ICollection fields_keys;
			string sql;
			
			try
			{
				sql = "UPDATE " + table + " SET ";
				fields_keys = fields.Keys;
				foreach (string key in fields_keys)
					sql += key + "=" + "'" + fields[key] + "', ";
				sql = sql.Substring(0, (sql.Length - 2));
				db.ExecuteQuery(sql);
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		//////////////////////////////////////////////////////////////////
				///<summary>
				/// Set fields of a specific table 
				///</summary>
				///<param name="fields">it is a dictionary with keys and values for the table</param>
				///<param name="constraint">it is a dictionary with the constraint</param>
				///<returns>Una cadena vacia o un codigo de error si ha habido problemas</returns>
		public void SetFieldsWhereas(IDictionary constraint, IDictionary fields)
		{
			ICollection fields_keys, constraint_keys;
			string sql;

			try
			{
				sql = "UPDATE " + selectedTable + " SET ";
				fields_keys = fields.Keys;
				foreach (string key in fields_keys)
					sql += key + "=" + "'" + fields[key] + "', ";
				sql = sql.Substring(0, (sql.Length - 2));
				constraint_keys = constraint.Keys;
				if (constraint_keys.Count != 0)
					sql += " WHERE ";
				foreach (string key in constraint_keys)
					sql += key + constraint[key] + " AND ";
				sql = sql.Substring(0, (sql.Length - 5));
				db.ExecuteQuery(sql);
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
				///Overloaded
				///<param name="table">Database table to access </param>
				///</summary>	
		public void SetFieldsWhereas (IDictionary constraint, IDictionary fields, string table)
		{
			ICollection fields_keys, constraint_keys;
			string sql;

			try
			{
				sql = "UPDATE " + table + " SET ";
				fields_keys = fields.Keys;
				foreach (string key in fields_keys)
					sql += key + "=" + "'" + fields[key] + "', ";
				sql = sql.Substring(0, (sql.Length - 2));
				constraint_keys = constraint.Keys;
				if (constraint_keys.Count != 0)
					sql += " WHERE ";
				foreach (string key in constraint_keys)
					sql += key + constraint[key] + " AND ";
				sql = sql.Substring(0, (sql.Length - 5));
				db.ExecuteQuery(sql);
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		//////////////////////////////////////////////////////////////////////////

		public DataSet GetById (int id)
		{
			try
			{
				IDictionary constraint = new Hashtable();
				constraint["id"] = "=" + id;
				return this.GetFieldsWhereas(constraint);
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		/////////////////////////////////////////////////////////////////////////
				///<summary>
				///Overloaded
				///<param name="table">Database table to access </param>
				///</summary>	
		public DataSet GetById (int id, string table)
		{
			try
			{
				IDictionary constraint = new Hashtable();
				constraint["id"] = "=" + id;
				return this.GetFieldsWhereas(constraint, table);
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		///////////////////////////////////////////////////////////////////////////
				///<summary>
				/// Get row id from given field 
				///</summary>
				///<param name="constraint">it is a dictionary with the constraint </param>
				///<returns>id</returns>
		public int GetIdWhereas (IDictionary constraint)
		{
			ICollection constraint_keys;
			string sql;
			
			try
			{
				sql = "SELECT id FROM " + selectedTable;
				constraint_keys = constraint.Keys;
				if (constraint_keys.Count != 0)
					sql += " WHERE ";
				foreach (string key in constraint_keys)
					sql += key + constraint[key] + " AND ";
				sql = sql.Substring(0, (sql.Length - 5)); 	// Erase latest AND
				db.ExecuteQuery(sql);
				return ((int)(db.ActiveDataSet()).Tables[0].Rows[0][0]);
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		/////////////////////////////////////////////////////////////////////////////

		public int GetIdWhereas (IDictionary constraint, string table)
		{
			ICollection constraint_keys;
			string sql;
			
			try
			{
				sql = "SELECT id FROM " + table;
				constraint_keys = constraint.Keys;
				if (constraint_keys.Count != 0)
					sql += " WHERE ";
				foreach (string key in constraint_keys)
					sql += key + constraint[key] + " AND ";
				sql = sql.Substring(0, (sql.Length - 5)); 	// Erase latest AND
				db.ExecuteQuery(sql);
				return ((int)(db.ActiveDataSet()).Tables[0].Rows[0][0]);
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
				throw wrappedEx;
			}
		}
		///////////////////////////////////////////////////////////////////
	}

/*	public class Test
	{
		static void Main ()
		{
			GenericDatabase db = new GenericDatabase ("127.0.0.1", "mono", "5432", "conflux", "");
			//db.setQuery ("INSERT INTO users (id, user_name, password, published) VALUES (0, 'carlos', 'clave', true)");
			//db.executeTransaction();
			db.Table = "users";
			IDictionary tmp = new Hashtable();
			tmp["user_name"] = "zeben";
			tmp["password"] = "zeben";
			tmp["published"] = true;
			//db.insert(tmp);
			
			IDictionary tmp2 = new Hashtable();
			tmp2["id"] = "=1";
			IDictionary tmp3 = new Hashtable();
			tmp3["id"] = "asc";
			tmp2["user_name"] = "='zeben'";
			//db.drop(tmp2);
			DataSet a = db.getFields("*", tmp2, tmp3);
			Console.WriteLine (a.Tables[0].Rows[0][0]);
    	}

	}*/
}
