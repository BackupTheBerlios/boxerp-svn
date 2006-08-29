//
// ClientExceptions.cs
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
using System.Collections;
using Gtk;
using Glade;
using System.Xml;
using System.IO;

namespace Boxerp.Exceptions
{
				
			///<summary>
			///ClientCriticalMessages launchs a Dialogs showing exception messages and exit
			///</summary>
	public static class ClientCriticalMessages 
	{
		public static void OnDelete(object obj, EventArgs args)
		{
			Console.WriteLine("Aborted");
			Gtk.Application.Quit();
		}
		/////////////////////////////////////////////////////////////////////
		public static void OnContinue(object obj, EventArgs args)
		{
		}
		////////////////////////////////////////////////////////////////////
				///<summary>
				///ThrowMessage receives a msg and show a window. 
				///Is intended to show exception messages
				///</summary>
				// FIXME: Draw a better interface with l10n messages
		public static void ThrowMessage (string Msg, string short_msg) 
		{
				Gtk.Window msgwindow = new Gtk.Window("Exception");
				Gtk.VBox box = new Gtk.VBox(false, 0);
				Gtk.ScrolledWindow scrWin = new Gtk.ScrolledWindow();
				Gtk.TextView txtView = new Gtk.TextView();
				Gtk.TextBuffer buffer = txtView.Buffer;
				buffer.Text = "Critical exception: " + short_msg;
				buffer.Text += "\r\n \r\nReport for developers : " + Msg;
				Gtk.Button buttonAccept = new Button("Accept");
				Gtk.Button buttonExit = new Button("Exit");
				
				msgwindow.DeleteEvent += new DeleteEventHandler(OnDelete);
				buttonExit.Clicked += new EventHandler(OnDelete);
				msgwindow.DestroyEvent += new DestroyEventHandler(OnContinue);
				buttonAccept.Clicked += new EventHandler(OnContinue);
				msgwindow.Add(scrWin);
				scrWin.Add(txtView);
				box.Spacing = 20;
				box.PackStart(scrWin);
				box.PackStart(buttonExit);
				box.PackStart(buttonAccept);
				//txtView.Show();
				//buttonExit.Show();
				//box.Show();
				msgwindow.DefaultHeight = 300;
				msgwindow.DefaultWidth  = 600;
				msgwindow.SetPosition(WindowPosition.Center);
				msgwindow.ShowAll();
				msgwindow.Present();
		}
		//////////////////////////////////////////////////////////////////////
				///<summary>
				///ThrowMessage receives a msg and show a window. 
				///Is intended to show exception messages
				///</summary>
		public static void ThrowMessageDialog (string Msg, bool quit) 
		{
			MessageDialog md = new MessageDialog (
								 new Gtk.Window("Dialog"), Gtk.DialogFlags.DestroyWithParent,
						       Gtk.MessageType.Error, Gtk.ButtonsType.Close, Msg);
			md.Run ();
			md.Destroy();
		}
	}
	
	//********************************************************************************
			///<summary>
			///ClientErrorMessages launchs a Dialogs showing user errors messages 
			///</summary>
			// FIXME: Draw a better interface
	public class ClientErrorMessages 
	{
		Gtk.Window msgwindow; 
		Gtk.VBox vbox;
		Gtk.HBox hbox;
		Gtk.Label label; 
		Gtk.Button buttonAccept; 
	
		public ClientErrorMessages(string Msg)
		{
			try
			{
				msgwindow    = new Gtk.Window("Error");
				vbox         = new Gtk.VBox(false, 0);
				hbox         = new Gtk.HBox(true, 1); 
				label			   = new Gtk.Label(Msg);
				buttonAccept = new Button("Aceptar");
				msgwindow.DeleteEvent += new DeleteEventHandler(OnDelete);
				msgwindow.DestroyEvent += new DestroyEventHandler(OnContinue);
				buttonAccept.Clicked += new EventHandler(OnContinue);
				msgwindow.Modal = true;
				msgwindow.Add(vbox);
				vbox.Spacing = 20;
				vbox.PackStart(label, false, false, 10);
				vbox.PackEnd(hbox, false, false, 10);
				hbox.PackEnd(buttonAccept, false, false, 10);
				//msgwindow.DefaultHeight = 150;
				//msgwindow.DefaultWidth  = 400;
				msgwindow.SetPosition(WindowPosition.Center);
				msgwindow.ShowAll();
				msgwindow.Present();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Excepcion grave");
			}
		}
		//////////////////////////////////////////////////////////////////////
		
		public void OnDelete(object obj, EventArgs args){}
		/////////////////////////////////////////////////////////////////////

		public void OnContinue(object obj, EventArgs args)
		{
			msgwindow.Destroy();
		}
	}
	
	// *********************************************************************
	
	public static class ClientMessages
	{
		public static void ProcessInfo(XmlDocument doc)
		{
				XmlNodeList xmlNodeList = doc.GetElementsByTagName("ERROR");
				string exceptionType = xmlNodeList[0].Attributes["TYPE"].Value;
				string msg = xmlNodeList[0].Attributes["EXCEPTION_MSG"].Value;
				if (exceptionType == "UserError")
				{
					new ClientErrorMessages(msg);
				}
				else if (exceptionType == "CriticalError")
					ClientCriticalMessages.ThrowMessage(doc.OuterXml, msg);
		}
		/////////////////////////////////////////////////////////////
				///<summary>
				///ShowException gets an exception, try to understand it and 
				///launch apropiate dialog window. Expected exception.Message is an xml string, 
				///but if the exception happens in a remote class constructor, the system will produce 
				///other exception with this message; 
				/// Exception has been thrown by the target of an invocation, 
				///and we have to get the xml message from InnerException.
				///</summary>
		public static void ShowException(Exception ex)
		{
			try
			{
				XmlDocument doc = new XmlDocument();
				doc.LoadXml(ex.Message);	
				ProcessInfo(doc);
			}
			catch (Exception e)
			{
				if (ex.InnerException != null)	
				{
					XmlDocument doc = new XmlDocument();
					doc.LoadXml(ex.InnerException.Message);
					ProcessInfo(doc);
				}
				else
				{
					ClientCriticalMessages.ThrowMessage(ex.StackTrace, ex.ToString()+ ":" + ex.Message);
				}
			}
		}
	}

	// FIXME: use l10n for exceptions messages
	
	///////////////////////////////////////////////////////////////////////	
	///<summary>
	///NullGladeException is a renamed NullReferenceException
	///</summary>
	public class NullGladeException : ApplicationException
	{
		public NullGladeException()
			: base ("Error intentando cargar glade"){	}

		public NullGladeException(string Msg)
			: base (Msg)	{	}

		public NullGladeException(string Msg, System.Exception e)
			: base (Msg, e)	{	}
	}
	/////////////////////////////////////////////////////////////////
	///<summary>
	///NullGladeWidgetException is a renamed NullReferenceException
	///</summary>
	public class NullGladeWidgetException : ApplicationException
	{
		public NullGladeWidgetException()
			: base ("Excepcion cargando widgets. EL widget escrito en código no coincide con el " +
								"del fichero glade o no existe en el fichero glade o en el Glade.XML")	{	}

		public NullGladeWidgetException(string Msg)
			: base (Msg){	}

		public NullGladeWidgetException(string Msg, System.Exception e)
			: base (Msg, e)	{	}
	}
	/////////////////////////////////////////////////////////////////////
	///<summary>
	///NullInterfaceException is a renamed NullReferenceException
	///</summary>
	public class NullInterfaceException : ApplicationException
	{
		public NullInterfaceException()
			: base ("Excepcion cargando widgets. EL widget que intenta cargar desde xml no existe")	{	}

		public NullInterfaceException(string Msg)
			: base (Msg){	}

		public NullInterfaceException(string Msg, System.Exception e)
			: base (Msg, e)	{	}
	}
	///////////////////////////////////////////////////////////////////////////////
	///<summary>
	///PermissionException is a renamed ArgumentNullException
	///</summary>
	public class PermissionException : ApplicationException 
	{
		public PermissionException()
			: base ("No dispone de los permisos suficientes para acceder a esta zona")	{	}

		public PermissionException(string Msg)
			: base (Msg)	{	}
	}





}
