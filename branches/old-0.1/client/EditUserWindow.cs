//
// EditUserWindow.cs
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
	class EditUserWindow : BoxerpWindow
	{
		[Widget] Gtk.Window editUserWin;            	// Ventana principal
		[Widget] Gtk.Entry entryName;
		[Widget] Gtk.Entry entryPassword;
		[Widget] Gtk.CheckButton checkbPublished;
		
		AdminFacade adminFacade;
		int userId;
		int groupId;

		public int GroupId
		{
			set { groupId = value; }
			get { return groupId; }
		}

		///////////////////////////////////////////////////////////
		
		public EditUserWindow(string sid, int gid, Hashtable permissions, Hashtable asiface, Glade.XML gladegui)
						:base (sid, gid, permissions, asiface, gladegui)
		{
			try
			{
				adminFacade = new AdminFacade();
				this.CheckGladeWidgets(editUserWin, entryName, entryPassword, checkbPublished);
				this.window = editUserWin;
				this.window.Modal = true;
				this.SetWidgetsData();	
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

		public void FillData(int userId, string userName, string userPassword, bool published)
		{
			try
			{
				this.userId = userId;
				entryName.Text         = userName;
				entryPassword.Text     = userPassword;
				checkbPublished.Active = published;
				editUserWin.ShowAll();
			}
			catch (Exception ex)
			{
				ClientMessages.ShowException(ex);
			}
		}
		/////////////////////////////////////////////////////////////////////////////

		public void ClearData()
		{
			try
			{
				this.userId = -1;
				entryName.Text         = "";
				entryPassword.Text     = "";
				checkbPublished.Active = false;
				editUserWin.ShowAll();
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

		void OnButtonSaveClicked (object o, EventArgs e)
		{

		}
		//////////////////////////////////////////////////////////////////////////////
		
	} 
}
