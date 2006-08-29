//
// BoxerpWindow.cs
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
using System.Reflection;
using Gtk;
using Glade;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Boxerp.Exceptions;

namespace Boxerp.Client
{
	public class BoxerpWindow
	{
		public Glade.XML gui;
		public Gtk.Window window;
		public Gtk.Container mainContainer;
		public string sid;
		public int gid;

		public Hashtable permissions;								// user permissions
		public Hashtable asiface;                   // actions/sections permissions
		public Hashtable screens;										// To store loaded screens
		public bool memLoaded;											// True; window has been loaded into memory. False: is loaded from glade

		//////////////////////////////////////////////////////////////////

		/*public BoxerpWindow(string moduleName, string sid, int gid, Hashtable permissions)
		{
			string guiPath, guiContainer, asifacePath;
			try
			{
				GuiExtractor.GetIface(moduleName, out guiPath, out guiContainer, 
															out asifacePath);
				this.screens     = new Hashtable();
				this.gui         = new Glade.XML (guiPath, guiContainer, "");
				this.permissions = permissions;
				this.uid         = uid;
				this.gid         = gid;
				this.gui.Autoconnect(this);
				GuiExtractor.BuildAsiface(out asiface, asifacePath);
				this.window = gui.GetWidget(guiContainer);
				this.PaintWithConstraints();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}*/
		//////////////////////////////////////////////////////////////////
		
		public BoxerpWindow(string sid, int gid, Hashtable permissions, Hashtable asiface, Glade.XML gladegui)
		{
				this.gui = gladegui;
				this.gui.Autoconnect(this);
				this.sid = sid;
				this.gid = gid;
				this.permissions = permissions;
				this.asiface = asiface;
				this.screens = new Hashtable();
				this.PaintWithConstraints();
		}
		/////////////////////////////////////////////////////////////////////

		public string BuildExceptionTrace(Exception ex)
		{
			Exception e = ex;
			string exceptionTrace = "";
			while (true)
			{
				if (e == null)
					 break;
				exceptionTrace += e.Message + " \r\n Traza: " + e.StackTrace;
				e = e.InnerException;
				exceptionTrace += "\r\n \r\n";
			}
			return exceptionTrace;
		}
		///////////////////////////////////////////////////////////////////
	
		public Gtk.Widget GetChildWidget(string moduleName)
		{
			string childGuiPath, childGuiContainer, childAsifacePath, managerClass;
		
			try
			{
				GuiExtractor.GetIface(moduleName, out childGuiPath, out childGuiContainer, 
															out childAsifacePath, out managerClass);
				if (screens.ContainsKey(managerClass))
				{
						BoxerpWindow bwindow = (BoxerpWindow) screens[managerClass]; // get existent window
						bwindow.mainContainer = (Gtk.Container) bwindow.gui.GetWidget(childGuiContainer); 
						bwindow.memLoaded = true;
						return bwindow.mainContainer;
				}
				else
				{
						Hashtable childAsiface = new Hashtable();
						Glade.XML tmpGui = new Glade.XML (childGuiPath, childGuiContainer, "boxerp");
						GuiExtractor.BuildAsiface(out childAsiface, childAsifacePath);
						System.Object[] paramsObj = new System.Object[] {this.sid, this.gid, 
																														this.permissions, childAsiface, tmpGui};
						Assembly assembly = Assembly.GetExecutingAssembly();
						System.Object childWin = 
								AppDomain.CurrentDomain.CreateInstanceAndUnwrap(assembly.FullName, managerClass,
																																true, BindingFlags.Instance|BindingFlags.Public,
																																null, paramsObj, null, null, null);
						((BoxerpWindow)childWin).mainContainer = (Gtk.Container) tmpGui.GetWidget(childGuiContainer);
						((BoxerpWindow)childWin).memLoaded = false;
						screens[managerClass] = childWin;
						//tmpGui.GetWidget(childGuiContainer);
						return ((BoxerpWindow)childWin).mainContainer;
				}
			}
			catch (Exception ex)
			{
				//string exceptionTrace = this.BuildExceptionTrace(ex);
				//ClientCriticalMessages.ThrowMessage(exceptionTrace, ex.Message);
				ClientMessages.ShowException(ex);
				throw ex;
			}
		}
		///////////////////////////////////////////////////////////////////
		
		public BoxerpWindow GetChildWindowClass(string moduleName)
		{
			string childGuiPath, childGuiContainer, childAsifacePath, managerClass;
		
			try
			{
				GuiExtractor.GetIface(moduleName, out childGuiPath, out childGuiContainer, 
															out childAsifacePath, out managerClass);
				if (screens.ContainsKey(managerClass))
				{
						BoxerpWindow bwindow = (BoxerpWindow) screens[managerClass]; // get existent window
						bwindow.mainContainer = (Gtk.Container) bwindow.gui.GetWidget(childGuiContainer); 
						bwindow.memLoaded = true;
						return bwindow; 
				}
				else
				{
						Hashtable childAsiface = new Hashtable();
						Glade.XML tmpGui = new Glade.XML (childGuiPath, childGuiContainer, "boxerp");
						GuiExtractor.BuildAsiface(out childAsiface, childAsifacePath);
						System.Object[] paramsObj = new System.Object[] {this.sid, this.gid, 
																														this.permissions, childAsiface, tmpGui};
						Assembly assembly = Assembly.GetExecutingAssembly();
						System.Object childWin = 
								AppDomain.CurrentDomain.CreateInstanceAndUnwrap(assembly.FullName, managerClass,
																																true, BindingFlags.Instance|BindingFlags.Public,
																																null, paramsObj, null, null, null);
						((BoxerpWindow)childWin).mainContainer = (Gtk.Container) tmpGui.GetWidget(childGuiContainer);
						((BoxerpWindow)childWin).memLoaded = false;
						screens[managerClass] = childWin;
						return (BoxerpWindow) childWin; 
				}
			}
			catch (Exception ex)
			{
				//string exceptionTrace = this.BuildExceptionTrace(ex);
				ClientMessages.ShowException(ex); 
				throw ex;
			}
		}
		///////////////////////////////////////////////////////////////////////////////////

		public void PaintWithConstraints()
		{
			ICollection asiface_keys;
			
			try
			{
				asiface_keys = asiface.Keys;
				if (asiface_keys.Count != 0)
				{
					string widgetName; 
					string signalName;
					Gtk.Widget widget;
					foreach (string key in asiface_keys)
					{
						if (!permissions.ContainsKey(asiface[key]))
						{
							widgetName = key.Split('|')[0];
							signalName = key.Split('|')[1];
							if (signalName == "show")
							{
								widget = gui.GetWidget(widgetName);
								if (widget != null)
									widget.State = StateType.Insensitive;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}			
		}
		///////////////////////////////////////////////////////////////////////////////////

		public void CheckGladeWidgets(params Gtk.Widget[] widgets)
		{
			try
			{
				foreach (Gtk.Widget w in widgets)
				{
					if (w == null)
					{
						NullGladeWidgetException nge = new NullGladeWidgetException();
						throw nge;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		////////////////////////////////////////////////////////////////////////////////////

		public virtual void SetWidgetsData(){}

		////////////////////////////////////////////////////////////////////////////////////
		
		public virtual void OnButtonCancelClicked (object o, EventArgs e)
		{
			this.window.Hide();	
		}
		////////////////////////////////////////////////////////////////////////////////////
		
		void OnWindowDeleteEvent (object o, DeleteEventArgs e)
		{
			e.RetVal = true;	
			this.window.Hide();
		}
		///////////////////////////////////////////////////////////////////////////////
	}
}
