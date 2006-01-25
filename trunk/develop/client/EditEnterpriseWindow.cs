//
// EditEnterpriseWindow.cs
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
using System.Xml;
using System.Data;
using System.Collections;
using Gtk;
using Glade;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Boxerp.Exceptions;
using Boxerp.Facade;
using Boxerp.Tools;

namespace Boxerp.Client
{
	class EditEnterpriseWindow : BoxerpWindow
	{
		[Widget] Gtk.Window editEnterpriseWin;            	// Ventana principal
		[Widget] Gtk.Entry entryName;
		[Widget] Gtk.Entry entryDescription;
		[Widget] Gtk.CheckButton checkbPublished;
		
		AdminFacade adminFacade;
		int epriseId;
		bool newEnterprise;
						
		public delegate void SaveSuccessEventHandler(); 
		public event SaveSuccessEventHandler SaveSuccessEvent;
		
		///////////////////////////////////////////////////////////
		
		public EditEnterpriseWindow(string sid, int gid, Hashtable permissions, Hashtable asiface, Glade.XML gladegui)
						:base (sid, gid, permissions, asiface, gladegui)
		{
			try
			{
				adminFacade = new AdminFacade();
				this.CheckGladeWidgets(editEnterpriseWin, entryName, entryDescription, checkbPublished);
				this.window = editEnterpriseWin;
				this.window.Modal = true;
				this.SetWidgetsData();
				newEnterprise = true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		/////////////////////////////////////////////////////////////////////////////
		
		public override void SetWidgetsData()
		{
		}
		/////////////////////////////////////////////////////////////////////////////

		public void FillData(int epriseId, string epriseName, string epriseDesc, bool published, bool isNew)
		{
			try
			{
				this.epriseId = epriseId;
				entryName.Text         = epriseName;
				entryDescription.Text  = epriseDesc;
				checkbPublished.Active = published;
				editEnterpriseWin.ShowAll();
				newEnterprise = isNew;
			}
			catch (Exception ex)
			{
				ClientMessages.ShowException(ex);
			}

		}
		//////////////////////////////////////////////////////////////////////////////

		public void ClearData()
		{
			try
			{
				entryName.Text = "";
				entryDescription.Text = "";
				checkbPublished.Active = false;
				newEnterprise = true;
			}
			catch (Exception ex)
			{
				ClientMessages.ShowException(ex);
			}
		}
		//////////////////////////////////////////////////////////////////////////////

		public DataTable BuildTableEnterprises()
		{
			try
			{
				DataTable dTable = new DataTable(DBTables.ENTERPRISES); 
				DataColumn nameCol = new  DataColumn();
				nameCol.DataType = System.Type.GetType("System.String");
				nameCol.ColumnName = AdminFields.EPRISENAME;
				dTable.Columns.Add(nameCol);
				DataColumn idCol = new  DataColumn();
				idCol.DataType = System.Type.GetType("System.Int32");
				idCol.ColumnName = AdminFields.EPRISEID;
				dTable.Columns.Add(idCol);
				DataColumn publishedCol = new  DataColumn();
				publishedCol.DataType = System.Type.GetType("System.Boolean");
				publishedCol.ColumnName = AdminFields.PUBLISHED;
				dTable.Columns.Add(publishedCol);
				DataColumn descCol = new  DataColumn();
				descCol.DataType = System.Type.GetType("System.String");
				descCol.ColumnName = AdminFields.EPRISEDESC;
				dTable.Columns.Add(descCol);
				return dTable;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		/////////////////////////////////////////////////////////////////////////////
		//                      	EVENTS HANDLERS                               
		/////////////////////////////////////////////////////////////////////////////
		
		/////////////////////////////////////////////////////////////////////////////

		void OnButtonSaveClicked (object o, EventArgs e)
		{
			try
			{
				string epriseName = entryName.Text;
				string epriseDesc = entryDescription.Text;
				DataSet ds   = new DataSet();
				DataTable dTable = this.BuildTableEnterprises();
				DataRow dRow = dTable.NewRow();
				dRow[AdminFields.EPRISENAME] = epriseName;
				dRow[AdminFields.PUBLISHED]  = true ;//checkbPublished.Active;
				dRow[AdminFields.EPRISEDESC] = epriseDesc;
				dRow[AdminFields.EPRISEID] = this.epriseId;
				dTable.Rows.Add(dRow);
				ds.Tables.Add(dTable);
				if (newEnterprise)
					adminFacade.AddEnterprise(sid, ds);
				else
					adminFacade.ModifyEnterprise(sid, ds);
				this.window.Hide();
				SaveSuccessEvent();
			}
			catch (Exception ex)
			{
				this.window.Hide();
				ClientMessages.ShowException(ex);
			}
		}
		//////////////////////////////////////////////////////////////////////////////
		
	} 
}
