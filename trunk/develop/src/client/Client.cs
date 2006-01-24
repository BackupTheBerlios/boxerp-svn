//
// Client.cs
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
using System.Collections;
using Gtk;
using Glade;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Mono.Unix;
using Boxerp.Objects;
using Boxerp.Exceptions;

namespace Boxerp.Client
{
	class Client 
	{
		LoginWindow logWin;
		MainWindow mainWin;
		ConcurrencyControllerObject access_obj;  
		LoginService loginObj;
		///////////////////////////////////////////////////////
		
		public Client()
		{
			try 
			{
				loginObj = new LoginService();
				if (loginObj == null)
				{
					Exception ex = new Exception();
					throw ex;
				}
				LoginWindow login = new LoginWindow(ref loginObj);
				Suscribe (login);
			}
			catch (NullGladeException ex)
			{
				ClientCriticalMessages.ThrowMessage(ex.Message, ex.ToString() + ":" + ex.Message);
			}
			catch (System.Exception ex)
			{
				ClientCriticalMessages.ThrowMessage(ex.StackTrace, ex.ToString() + ":" + ex.Message);
				Console.WriteLine("Imposible Conectar");
			}
		}
		//////////////////////////////////////////////////////
		
		public void Suscribe (LoginWindow logWin) 
		{
			this.logWin = logWin;
			logWin.login_event += new LoginWindow.LoginEventHandler (this.LoginSuccess);
		}
		/////////////////////////////////////////////////////
		
		public void Unsuscribe (LoginWindow logWin) 
		{
			logWin.login_event -= new LoginWindow.LoginEventHandler (this.LoginSuccess);
		}
		////////////////////////////////////////////////////		
		
		private void LoginSuccess () 
		{
			try
			{
				string guiPath, guiContainer, asifacePath;
				GuiExtractor.GetIface("Main", out guiPath, out guiContainer, out asifacePath);
				Glade.XML tmpGui = new Glade.XML (guiPath, guiContainer, "boxerp");
				Hashtable asiface;
				GuiExtractor.BuildAsiface(out asiface, asifacePath);
				mainWin = new MainWindow(logWin.Sid, logWin.Gid, logWin.Permissions, asiface, tmpGui);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Excepcion:{0}", ex.Message);
			}
		}
		/////////////////////////////////////////////////////
		
		public static void Main()
		{
			Gtk.Application.Init();
			Catalog.Init("boxerp", Boxerp.Defines.LOCALE_DIR);
			new Client();
			Gtk.Application.Run();
		}
	} // Client
} // namespace

				
