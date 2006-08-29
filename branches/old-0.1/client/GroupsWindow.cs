//
// GroupsWindow.cs
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
	class GroupsWindow : BoxerpWindow
	{
		[Widget] Gtk.TreeView treevwEnterprises;
		[Widget] Gtk.TreeView treevwGroups;
		[Widget] Gtk.HBox modGroupsHbox;   		// Contenedor principal
		
		AdminFacade adminFacade;

		///////////////////////////////////////////////////////////
		
		public GroupsWindow(string sid, int gid, Hashtable permissions, Hashtable asiface, Glade.XML gladegui)
						:base (sid, gid, permissions, asiface, gladegui)
		{
			try
			{
				adminFacade = new AdminFacade();
				this.CheckGladeWidgets(treevwEnterprises, treevwGroups, modGroupsHbox);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		/////////////////////////////////////////////////////////////////////////////
				///<summary>
				///Load data widgets from server; Fill combos and so on.
				///</summary>
		public override void SetWidgetsData()
		{
			try
			{
				DataSet ds = adminFacade.GetEnterprisesList(sid, 0,0);
				GuiTreeview guiTvw = new GuiTreeview();
				guiTvw.StoreDataSet(ds, ref treevwEnterprises);
			}
			catch (Exception ex)
			{
				ClientMessages.ShowException(ex);
			}
		}
		/////////////////////////////////////////////////////////////////////////////
		//                      	EVENTS HANDLERS                               
		/////////////////////////////////////////////////////////////////////////////
		
		/////////////////////////////////////////////////////////////////////////////

		void OnButtonAddGroupClicked (object o, EventArgs e)
		{
			try
			{
				TreeModel model;                   
				TreeIter iter;                      
				int epriseId;
				if (treevwEnterprises.Selection.GetSelected (out model, out iter))	   // A group must be selected
				{
					epriseId = int.Parse((string) treevwEnterprises.Model.GetValue (iter, 0));
					EditGroupWindow editGroupWindow = (EditGroupWindow) this.GetChildWindowClass("EditGroup");
					editGroupWindow.ClearData();
					editGroupWindow.EnterpriseId = epriseId;
				}
				else
				{
					new ClientErrorMessages("Por favor, seleccione una empresa primero");
				}
			}
			catch (Exception ex)
			{
				ClientMessages.ShowException(ex);
			}
		}
		/////////////////////////////////////////////////////////////////////////////////

		void OnButtonEditGroupClicked (object o, EventArgs e)
		{
			try
			{
				this.OnTreevwGroupsRowActivated(o, e);
			}
			catch (Exception ex)
			{
				ClientMessages.ShowException(ex);
			}
		}
		/////////////////////////////////////////////////////////////////////////////////

		void OnButtonDelGroupClicked (object o, EventArgs e)
		{
			
		}
		/////////////////////////////////////////////////////////////////////////////////

		void OnButtonSearchClicked (object o, EventArgs e)
		{
			
		}
		/////////////////////////////////////////////////////////////////////////////////

		void OnButtonCleanClicked (object o, EventArgs e)
		{
			
		}
		/////////////////////////////////////////////////////////////////////////////////

		void OnButtonEditFoundClicked (object o, EventArgs e)
		{
			
		}
		/////////////////////////////////////////////////////////////////////////////////

		void OnButtonDelFoundClicked (object o, EventArgs e)
		{
			
		}
		/////////////////////////////////////////////////////////////////////////////////

		void OnTreevwGroupsRowActivated (object o, EventArgs e)
		{
			TreeModel model;                    //Datos del treeview 
			TreeIter iter;                      //Fila seleccionada del treeview
			int groupId;
			string groupName;
			bool published;
			try
			{
				if (treevwGroups.Selection.GetSelected (out model, out iter))
				{
					groupId   = int.Parse((string) treevwGroups.Model.GetValue (iter, 0));
					groupName = (string) treevwGroups.Model.GetValue (iter, 2);
					published  = bool.Parse((string) treevwGroups.Model.GetValue (iter, 3));	
					EditGroupWindow editGroupWindow = (EditGroupWindow) this.GetChildWindowClass("EditGroup");
					editGroupWindow.FillData(groupId, groupName, published);
				}
			}
			catch (Exception ex)
			{
				ClientMessages.ShowException(ex);
			}
		}
		/////////////////////////////////////////////////////////////////////////////////

		void OnTreevwEnterprisesCursorChanged (object o, EventArgs e)
		{
			try
			{
				TreeModel model;                    //Datos del treeview 
				TreeIter iter;                      //Fila seleccionada del treeview
				int epriseId = 0;
	
				GuiTreeview.RemoveColumns(treevwGroups);	
 				if (treevwEnterprises.Selection.GetSelected (out model, out iter))
				{
					epriseId = int.Parse((string) treevwEnterprises.Model.GetValue (iter, 0));
				}
				DataSet ds = adminFacade.GetGroupsListById(sid, epriseId, 0, 0); 
				GuiTreeview guiTvw = new GuiTreeview();
				guiTvw.StoreDataSet(ds, ref treevwGroups);
			}
			catch (Exception ex)
			{
				ClientMessages.ShowException(ex);
			}
		}
		/////////////////////////////////////////////////////////////////////////////////
			
	} 
}
