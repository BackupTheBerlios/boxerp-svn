//
// LoginWindow.cs
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
using Boxerp.Objects;
using Boxerp.Exceptions;
using Boxerp.Tools;
using Boxerp.Errors;
using Boxerp.Debug;

namespace Boxerp.Client
{
	class LoginWindow 
	{
		[Widget] Gtk.Window loginWin;
		[Widget] Gtk.Entry entUsername;
		[Widget] Gtk.Entry entPassword;

		Glade.XML gui;
		Hashtable permissions;
		string sid;
		int gid;
		LoginService loginService;

		/////////////////////////////////////////////////////

		public delegate void LoginEventHandler(); 
		
		public event LoginEventHandler login_event;
		//////////////////////////////////////////////////////////

		public LoginWindow(ref LoginService login_o)
		{
			try 
			{
				string guipath, guicontainer;
				this.loginService = login_o;
				GuiExtractor.GetIface("Login", out guipath, out guicontainer);
				this.gui = new Glade.XML (guipath, guicontainer, "");
				
				if (gui.GetWidget(guicontainer) == null)
				{
					throw new NullGladeException();  // break this try block and go to catch
				}
				else 
					this.gui.Autoconnect(this);
			}
			catch (NullGladeException ex)
			{
				throw ex;   // This windows cant be loaded, so throw the exception to the caller
			}
			catch (NullReferenceException ex)
			{
				throw new NullGladeException(ex.Message, ex);
			}
			catch (System.Exception e)
			{
				ClientMessages.ShowException(e);
			}
			permissions = new Hashtable();
		}
		/////////////////////////////////////////////////////////  
		
		public string Sid
		{
			get {return sid;}
			set {sid = value;}
		}
		////////////////////////////////////////////////////////
		
		public int Gid
		{
			get {return gid;}
			set {gid = value;}
		}
		///////////////////////////////////////////////////////
		
		public Hashtable Permissions
		{
			get {return permissions;}
			set {permissions = value;}
		}
		//////////////////////////////////////////////////////
	
		void OnButtonConnectClicked(object o, EventArgs e)
		{
			DataSet permset = new DataSet();
			string hashkey;

			try
			{
				loginService.login(entUsername.Text, entPassword.Text, ref sid, ref gid, ref permset);
				loginWin.Hide();

				DataRow[] permrows = DataTools.GetRows(permset);
				for (int i = 0; i < permrows.Length; i++)
				{
					hashkey = (string)permrows[i][0] + "|" + (string)permrows[i][1];
					permissions[hashkey] = 1;
				}
				if (login_event != null)
					login_event();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Capturando excepcion");
				if (ex.InnerException != null)
					Console.WriteLine("Inner:" + ex.InnerException.Message);
				ClientMessages.ShowException(ex);
			}
		}
		////////////////////////////////////////////////////////////////////////////////
		
		void OnLoginWinDeleteEvent (object o, EventArgs e)
		{
			Gtk.Application.Quit();
		}
	} 
}

		/*public string BuildExceptionTrace(Exception ex)
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
		}*/
		////////////////////////////////////////////////////////7
	
