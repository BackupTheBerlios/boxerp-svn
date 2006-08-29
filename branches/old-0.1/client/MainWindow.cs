//
// MainWindow.cs
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
using System.Collections;
using System.Reflection;
using Gtk;
using Glade;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Boxerp.Exceptions;
//using Boxerp.Objects;

namespace Boxerp.Client
{
	class MainWindow : BoxerpWindow
	{
		[Widget] Gtk.Label labelModName;      
		[Widget] Gtk.Button buttonViewLeft;      	
		[Widget] Gtk.Button buttonPurchases;
		[Widget] Gtk.TreeView treevwModule;      	
		[Widget] Gtk.Window mainWin;          		
		[Widget] Gtk.HPaned mainHpn;  		  		
		[Widget] Gtk.HBox modContainerHbox;   		
		
		XmlNode nodeTreeview;                 		
		XmlNode nodeSelectedTreevwModule = null;	

		///////////////////////////////////////////////////////////
		///<summary>
		/// Constructor de la clase. Extrae de un fichero xml los datos 
		/// de la ventana a cargar, en este caso Main
		///</summary>
		///<param name="sid">Identificador de usuario</param>
		///<param name="gid">Identificador del grupo del usuario</param>
		///<param name="permissions">Permisos para ese usuario</param>
		public MainWindow(string sid, int gid, Hashtable permissions, Hashtable asiface, 
							 Glade.XML gladegui)
						:base (sid, gid, permissions, asiface, gladegui)
		{
			try
			{
				this.CheckGladeWidgets(labelModName, buttonViewLeft, buttonPurchases, 
												treevwModule, mainWin, mainHpn, modContainerHbox);
				this.SetWidgetsData();	
				this.window = mainWin;
				this.window.Maximize();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Excepcion en Main: {0}", ex.Message);
			}
		}
		//////////////////////////////////////////////////////////////////////////////

		public override void SetWidgetsData()
		{
			nodeTreeview = GuiExtractor.GetTreevw("Main", treevwModule.Name);
		}

		/////////////////////////////////////////////////////////////////////////////
		//                      	EVENTS HANDLERS                               
		//
		/////////////////////////////////////////////////////////////////////////////
		///<summary>
		/// Este evento se dispara cuando se pincha con el ratón en algún elemento del 
		/// treeview principal 
		///</summary>
		void OnModuleTvwCursorChanged (object o, EventArgs e)
		{
			string moduleName; 			            //Nombre del modulo a cargar en el lado derecho
			TreeModel model;                    //Datos del treeview 
			TreeIter iter;                      //Fila seleccionada del treeview

			try
			{
				if (modContainerHbox.Children.Length > 0)
					modContainerHbox.Remove(modContainerHbox.Children[0]);

				if (treevwModule.Selection.GetSelected (out model, out iter))
				{
					moduleName = (string) treevwModule.Model.GetValue(iter, 0);
					BoxerpWindow rightBox = this.GetChildWindowClass(moduleName);//get widget and launch its manager gui class
					modContainerHbox.Child = rightBox.mainContainer; 
					rightBox.SetWidgetsData();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Excepcion: {0}", ex.Message);
			}
		} 
		/////////////////////////////////////////////////////////////////////////////

		void OnModuleTvwRowActivated (object o, EventArgs e)
		/* On enter key over a treeview row */
		{
		} 
		/////////////////////////////////////////////////////////////////////////////

		void OnBoxerpDestroy(object o, EventArgs e)
		/* Quit application when close then main window */
		{
			Application.Quit();
		}
		/////////////////////////////////////////////////////////////////////////////

		void OnBtnViewLeftClicked(object o, EventArgs e)
		{
			try
			{
				mainHpn.Position = 195;
				buttonViewLeft.Hide();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		/////////////////////////////////////////////////////////////////////////////

		void OnBtnHideLeftClicked(object o, EventArgs e)
		{
			try
			{
				mainHpn.Position = 30;
				buttonViewLeft.Show();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		/////////////////////////////////////////////////////////////////////////////
		
		///<summary>
		/// On button clicked from the main left-down panel
		///</summary>
		void OnBtnClicked(object o, EventArgs e)
		{
			string element = ((Gtk.Button)o).Name;

			labelModName.Text = ((Gtk.Button)o).Label ;

			// Empty module container (right)
			if (modContainerHbox.Children.Length > 0)
				modContainerHbox.Remove(modContainerHbox.Children[0]);
			try
			{
			   	// Get treeview columns from xml
                ArrayList columns = new ArrayList();
                Column col;

				nodeSelectedTreevwModule = null;
				XmlNodeList childs = nodeTreeview.ChildNodes;
				foreach (XmlNode node in childs)
				{
					if (node.Name == "column")
					{
                        col.type = node.Attributes["value"].Value;
                        col.view = bool.Parse(node.Attributes["visible"].Value);
                        col.name = node.Attributes["name"].Value;
                        columns.Add(col);
					}
					else
						if (node.Name == "module")
							if (node.Attributes["name"].Value.ToUpper() == element.ToUpper())
								nodeSelectedTreevwModule = node;
				}
			
				// Clean treeview before load new data
				GuiTreeview.RemoveColumns(treevwModule);
				// Create a new treeview 
				GuiTreeview gui_tvw = new GuiTreeview();
				gui_tvw.NewTreeView (columns, ref treevwModule);
				// Load data
				if (nodeSelectedTreevwModule != null)
					gui_tvw.BuildTree(nodeSelectedTreevwModule, TreeIter.Zero);
			}
			catch (ArgumentNullException ex)
			{
				new PermissionException();
			//	throw ex;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error:{0}", ex);
				//throw ex;
			}
		}
		/////////////////////////////////////////////////////////////////////////////

	} // MainWindow
}
