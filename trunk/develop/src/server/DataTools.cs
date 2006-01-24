//
// DataTools.cs
//
// Authors:
// 	Carlos Ble Jurado <carlosble@shidix.com>
// 	Zebenzui Perez Ramos <zebenperez@shidix.com>
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

namespace Boxerp.Tools
{
	public struct	AdminFields
	{
		public static string USERNAME   = "user_name";
		public static string PASSWORD   = "password";
		public static string PUBLISHED  = "published"; 
		public static string ID         = "id";
		public static string GROUPNAME  = "group_name";
		public static string GROUPID    = "id_group";
		public static string EPRISEID   = "id_enterprise";
		public static string EPRISENAME = "name";
		public static string EPRISEDESC = "description";
		public static string ACTIONS    = "id_ctrld_actions";
		public static string SECTIONS   = "id_ctrld_sections";
	}

	public struct	GuiFields
	{
		public static string ICON       = "icon";
		public static string ICONPATH   = "icon_path";
		public static string PATH       = "path";
    }

	public struct DBTables
	{
		public static string GROUPS      = "groups";
		public static string ENTERPRISES = "enterprises";
		public static string USERS       = "users";
		public static string PERMS       = "permissions";
		public static string ICONS       = "icons";
	}
	/////////////////////////////////////////////////////////////////
	
				///<summary>
				///Tools for extract and validate data 
				///</summary>
	public static class DataTools
	{
	
		/////////////////////////////////////////////////////////////////////////////
		
		public static string GetTextField(DataSet ds, string field)
		{
			try
			{
				string data = (string)ds.Tables[0].Rows[0][field];
				// validate data
				return data;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		//////////////////////////////////////////////////////////////////////////////
        
		public static void SetTextField(ref DataSet ds, string field, string val)
		{
			try
			{
				ds.Tables[0].Rows[0][field] = val;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		//////////////////////////////////////////////////////////////////////////////
		
		public static bool GetBoolField(DataSet ds, string field)
		{
			try
			{
				bool data = (bool) ds.Tables[0].Rows[0][field];
				return data;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		//////////////////////////////////////////////////////////////////////////////
		
		public static int GetIntField(DataSet ds, string field)
		{
			try
			{
				int data = (int) ds.Tables[0].Rows[0][field];
				// validate data
				return data;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		////////////////////////////////////////////////////////////////////////////////

		public static DataRow[] GetRows(DataSet ds)
		{
			try
			{
				DataRow[] rows = ds.Tables[0].Select(null, null, DataViewRowState.CurrentRows);
				// validate data
				return rows;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		////////////////////////////////////////////////////////////////////////////////

		public static void AddColumn(ref DataSet ds, string name)
		{
			try
			{
				// validate data
				ds.Tables[0].Columns.Add(name);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		////////////////////////////////////////////////////////////////////////////////
	}
}
