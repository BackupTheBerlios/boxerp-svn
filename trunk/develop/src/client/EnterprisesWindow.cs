//
// EnterprisesWindow.cs
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
	class EnterprisesWindow : BoxerpWindow
	{
		[Widget] Gtk.TreeView treevwEnterprises;
		[Widget] Gtk.HBox modEnterprisesHbox;   		// Contenedor principal
		
		AdminFacade adminFacade;

		///////////////////////////////////////////////////////////
		
		public EnterprisesWindow(string sid, int gid, Hashtable permissions, Hashtable asiface, Glade.XML gladegui)
						:base (sid, gid, permissions, asiface, gladegui)
		{
			try
			{
				adminFacade = new AdminFacade();
				this.CheckGladeWidgets(treevwEnterprises, modEnterprisesHbox);
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
	
		void OnSaveSuccess()
		{
			SetWidgetsData();
		}
		/////////////////////////////////////////////////////////////////////////////
		
		void OnButtonAddEnterpriseClicked (object o, EventArgs e)
		{
			try
			{
					EditEnterpriseWindow editEnterpriseWindow = (EditEnterpriseWindow) this.GetChildWindowClass("EditEnterprise");
					editEnterpriseWindow.ClearData();
					if (!editEnterpriseWindow.memLoaded)	// if is loaded from glade, add SaveSuccessEventHandler
					{
						editEnterpriseWindow.SaveSuccessEvent += new EditEnterpriseWindow.SaveSuccessEventHandler (this.OnSaveSuccess);
					}
					editEnterpriseWindow.window.Present();
			}
			catch (Exception ex)
			{
				ClientMessages.ShowException(ex);
			}
		}
		//////////////////////////////////////////////////////////////////////////////
		
		void OnButtonEditEnterpriseClicked (object o, EventArgs e)
		{
			try
			{
				this.OnTreevwEnterprisesRowActivated(o, e);
			}
			catch (Exception ex)
			{
				ClientMessages.ShowException(ex);
			}
		}
		//////////////////////////////////////////////////////////////////////////////

		void OnTreevwEnterprisesRowActivated (object o, EventArgs e)
		{
			TreeModel model;                    //Datos del treeview 
			TreeIter iter;                      //Fila seleccionada del treeview
			int epriseId;
			string epriseName, epriseDesc;
			bool published;
			try
			{
				if (treevwEnterprises.Selection.GetSelected (out model, out iter))
				{
					epriseId   = int.Parse((string) treevwEnterprises.Model.GetValue (iter, 0));
					epriseName = (string) treevwEnterprises.Model.GetValue (iter, 1);
					epriseDesc = (string) treevwEnterprises.Model.GetValue (iter, 3);
					published  = bool.Parse((string) treevwEnterprises.Model.GetValue (iter, 2));	
					EditEnterpriseWindow editEnterpriseWindow = (EditEnterpriseWindow) this.GetChildWindowClass("EditEnterprise");
					editEnterpriseWindow.FillData(epriseId, epriseName, epriseDesc, published, false);
					if (!editEnterpriseWindow.memLoaded)	// if is loaded from glade, add SaveSuccessEventHandler
						editEnterpriseWindow.SaveSuccessEvent += new EditEnterpriseWindow.SaveSuccessEventHandler (this.OnSaveSuccess);
					editEnterpriseWindow.window.Present();
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
		//////////////////////////////////////////////////////////////////////////////

		void OnButtonDelEnterpriseClicked (object o, EventArgs e)
		{
			TreeModel model;                    //Datos del treeview 
			TreeIter iter;                      //Fila seleccionada del treeview
			int epriseId;
			try
			{
				if (treevwEnterprises.Selection.GetSelected (out model, out iter))
				{
					epriseId   = int.Parse((string) treevwEnterprises.Model.GetValue (iter,0));
//																	GuiTreeview.GetColumn(AdminFields.EPRISEID, treevwEnterprises)));
					adminFacade.DelEnterprise(sid, epriseId);
					SetWidgetsData();
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
		//////////////////////////////////////////////////////////////////////////////

	} 
}
