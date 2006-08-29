//
// UsersWindow.cs
//
// Authors:
// 	Carlos Ble Jurado <carlosble@shidix.com>
// 	Zebenzui Perez Ramos <carlosble@shidix.com>
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
	class UsersWindow : BoxerpWindow
	{
		[Widget] Gtk.ComboBox combobEprises;
		[Widget] Gtk.TreeView treevwGroups;
		[Widget] Gtk.TreeView treevwUsers;
		[Widget] Gtk.HBox modUsersHbox;   		// Contenedor principal
		
		AdminFacade adminFacade;

		///////////////////////////////////////////////////////////
		
		public UsersWindow(string sid, int gid, Hashtable permissions, Hashtable asiface, Glade.XML gladegui)
						:base (sid, gid, permissions, asiface, gladegui)
		{
			try
			{
				adminFacade = new AdminFacade();
				this.CheckGladeWidgets(combobEprises, treevwGroups, treevwUsers, modUsersHbox);
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
				//combobEprises.Clear();
				DataSet ds = adminFacade.GetEnterprisesList(sid, 0,0);
				GuiTools.BuildComboBox(ds, ref combobEprises); 
			}
			catch (Exception ex)
			{
				ClientMessages.ShowException(ex);
			}
		}
		/////////////////////////////////////////////////////////////////////////////
		//                      	EVENTS HANDLERS                               
		/////////////////////////////////////////////////////////////////////////////
		
		void OnCombobEprisesButtonPress (object o, EventArgs e)
		{
		} 
		/////////////////////////////////////////////////////////////////////////////

		void OnCombobEprisesChanged (object o, EventArgs e)
		{
			try
			{
				GuiTreeview.RemoveColumns(treevwUsers);
				GuiTreeview.RemoveColumns(treevwGroups);
				ComboBox combob = (ComboBox) o;
				TreeIter iter;
				string epriseName = "";
				if (combob.GetActiveIter (out iter))
					epriseName = (string) combob.Model.GetValue (iter, 0);
				DataSet ds = adminFacade.GetIconGroupsList(sid, epriseName, 0, 0); 

				XmlDocument doc = new XmlDocument();
				doc.Load(Boxerp.Defines.GUI_DIR + "treeviews.xml");
				XmlNodeList xmlNode = doc.GetElementsByTagName("groups");

				GuiTreeview guiTvw = new GuiTreeview();
				guiTvw.StoreDataSet(ds, xmlNode.Item(0), ref treevwGroups);
				GuiTreeview.RemoveColumns(treevwUsers);
			}
			catch (Exception ex)
			{
				ClientMessages.ShowException(ex);
			}
		}
		/////////////////////////////////////////////////////////////////////////////

		void OnTreevwGroupsCursorChanged (object o, EventArgs e)
		{
			TreeModel model;                    //Datos del treeview 
			TreeIter iter;                      //Fila seleccionada del treeview
			int groupId = 0;
			try
			{
				if (treevwGroups.Selection.GetSelected (out model, out iter))
				{
					groupId = int.Parse((string) treevwGroups.Model.GetValue (iter, 0));
				}

				DataSet ds = adminFacade.GetUsersList(sid, groupId, 0, 0); 
				GuiTreeview guiTvw = new GuiTreeview();
				guiTvw.StoreDataSet(ds, ref treevwUsers);
			}
			catch (Exception ex)
			{
				ClientMessages.ShowException(ex);
			}
		}
		////////////////////////////////////////////////////////////////////////////////

		void OnTreevwUsersRowActivated (object o, EventArgs e)
		{
			TreeModel model;                    //Datos del treeview 
			TreeIter iter;                      //Fila seleccionada del treeview
			int userId;
			string userName, userPassword;
			bool published;
			try
			{
				if (treevwUsers.Selection.GetSelected (out model, out iter))
				{
					userId       = int.Parse((string) treevwUsers.Model.GetValue (iter, 0));
					//groupId      = int.Parse((string) treevwUsers.Model.GetValue (iter, 1));
					userName     = (string) treevwUsers.Model.GetValue (iter, 2);
					userPassword = (string) treevwUsers.Model.GetValue (iter, 3); 
					published    = bool.Parse((string) treevwUsers.Model.GetValue (iter, 4));	
					EditUserWindow editUserWindow = (EditUserWindow) this.GetChildWindowClass("EditUser");
					editUserWindow.FillData(userId, userName, userPassword, published);
				}
			}
			catch (Exception ex)
			{
				ClientMessages.ShowException(ex);
			}
		}
		////////////////////////////////////////////////////////////////////////////////
		
		void OnButtonAddUserClicked (object o, EventArgs e)
		{
			try
			{
				TreeModel model;                   
				TreeIter iter;                      
				int groupId;
				if (treevwGroups.Selection.GetSelected (out model, out iter))	   // A group must be selected
				{
					groupId = int.Parse((string) treevwGroups.Model.GetValue (iter, 0));
					EditUserWindow editUserWindow = (EditUserWindow) this.GetChildWindowClass("EditUser");
					editUserWindow.ClearData();
					editUserWindow.GroupId = groupId;
				}
				else
				{
					new ClientErrorMessages("Por favor, seleccione un grupo primero");
				}
			}
			catch (Exception ex)
			{
				ClientMessages.ShowException(ex);
			}
		}
		/////////////////////////////////////////////////////////////////////////////////

		void OnButtonEditUserClicked (object o, EventArgs e)
		{
			try
			{
				this.OnTreevwUsersRowActivated(o, e);	
			}
			catch (Exception ex)
			{
				ClientMessages.ShowException(ex);
			}
		}
		/////////////////////////////////////////////////////////////////////////////////

		void OnButtonDelUserClicked (object o, EventArgs e)
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

	} 
}
