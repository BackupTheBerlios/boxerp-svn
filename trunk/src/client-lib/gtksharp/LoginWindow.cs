//
// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
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
using Gtk;
using System.Threading;
using Boxerp.Models;
using Boxerp.Objects;

namespace Boxerp.Client.GtkSharp
{
	
	public class LoginWindow : Gtk.Window
	{
		protected Gtk.Button buttonConnect;
		protected Gtk.Entry entryServer;
		protected Gtk.Entry entryUser;
		protected Gtk.Entry entryPass;
		protected WaitWindow waitWindow;
		public  delegate void LoginEventHandler();
		public  event LoginEventHandler loginEvent;
		
		protected ThreadStart delegateLogin; 
		protected Thread threadLogin; 
		static ThreadNotify notify;
		protected bool logon = false;
		protected string session = null;
		public bool connectionFailure = false;
		protected ILogin loginObj;
		
		public LoginWindow() : 
				base("")
		{
			Stetic.Gui.Build(this, typeof(LoginWindow));
			delegateLogin = new ThreadStart (Login);
			threadLogin  = new Thread(delegateLogin);
			notify = new ThreadNotify (new ReadyEvent (Logged));
		}
	
	
		public string Session
		{
			get { return session;}
			set { session = value;}
		}
	
		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}

		protected virtual void OnConnect(object sender, System.EventArgs e)
		{
			if ((entryUser.Text.Length > 0) && (entryPass.Text.Length > 0))
			{
				waitWindow = new WaitWindow();
				waitWindow.SetMsg("Registering user and password: Please wait...");
				this.Hide();
				threadLogin.Start();
			}
		}
	
		protected void Login()
		{
			try
			{
				loginObj = (ILogin) RemotingHelper.GetObject(typeof(ILogin));
				session = loginObj.Login(entryUser.Text, entryPass.Text);
				if (session != null)
					logon = true;
				
			}
			catch (System.Net.WebException we)
			{
				connectionFailure = true;
				Console.WriteLine("Excepction: " + we.Message);
			}
			catch (Exception e)
			{
				logon = false;
				Console.WriteLine("Exception: " + e.Message);
			}
		
			notify.WakeupMain();
		}
	
		protected void Logged()
		{
			if (logon)
			{
				waitWindow.Hide();
				waitWindow.Stop();
				waitWindow.Destroy();
				loginEvent();
			}
			else
			{
				waitWindow.Hide();
				waitWindow.Stop();
				waitWindow.Destroy();
				WarningDialog wd = new WarningDialog();
				if (!connectionFailure)
				{
					//wd. a.textviewDesc.Buffer.Text = "Nombre de Usuario o Contraseña erróneos";
					//a.labelMsg.Text = "Error: Acceso Denegado";
				}
				wd.Present();
			}
		}
 	}
}
