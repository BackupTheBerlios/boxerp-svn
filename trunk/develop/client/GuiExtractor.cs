//
// GuiExtractor.cs
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
using System.Collections;
using Gtk;
using Glade;
using System.IO;
using System.Xml;
using Boxerp.Exceptions;

namespace Boxerp.Client
{
	///<summary>
	///	GuiExtractor class contains methods to extract or get glade files and other gui metadata	
	///	like widget actions on sections 
	///</summary>
	public static class GuiExtractor
	{
		
		///<summary>
		///	build_asiface reads gui metadata file containing widget actions on sections
		///	and fill the hashtable 
		///	<param name="asiface">The Hashtable to build</param>
		///	<param name="filepath">Path to gui metadata file </param>
		///	<returns> Status code will be 0 for success </returns>
		///</summary>
		public static void BuildAsiface(out Hashtable asiface, string filepath)
		{
			string hashkey;
			asiface = null;

			try 
			{
				asiface = new Hashtable();
				XmlDocument doc = new XmlDocument();
				doc.Load(filepath);
				XmlNodeList xmlNodeList = doc.GetElementsByTagName("widget");
				foreach (XmlNode node in xmlNodeList) 
				{
					hashkey = node.Attributes["id"].Value;
					XmlNodeList childNodeList = node.ChildNodes;
					foreach (XmlNode child_node in childNodeList) 
					{
						XmlNode action_node  = child_node.ChildNodes[0];
						XmlNode section_node = child_node.ChildNodes[1];
						string newhashkey = "";
						if (child_node.Name == "show")
						{
							newhashkey = hashkey + "|" + "show";
						}
						else if (child_node.Name == "signal")
						{
							newhashkey = hashkey + "|" + child_node.Attributes["handler"].Value;
						}
						asiface[newhashkey] = action_node.Attributes["name"].Value + "|" + 
																	section_node.Attributes["name"].Value;		
					}
				}
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw ex;
			}
		}
		/////////////////////////////////////////////////////////////////////////////////	
		///<summary>
		///	get_iface provides parameters to build a Glade.XML object and a path to 
		///	gui metadata asiface
		///	<param name="ifaceName">Short name of the screen requested</param>
		///	<param name="guiPath">Path to glade file</param>
		///	<param name="guiContainer">Main container at glade file</param>
		///	<param name="asiface">Hashtable with gui metadata</param>
		///	<returns> Status code will be 0 on sucess</returns>
		///</summary>
		public static void GetIface(string ifaceName, out string guiPath, 
												out string guiContainer, out string asifacePath)
		{
			guiPath = guiContainer = asifacePath = "";
			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(Path.Combine(Boxerp.Defines.GUI_DIR,  "gui_index.xml"));
				XmlNodeList xmlNodeList = doc.GetElementsByTagName("window");
				foreach (XmlNode node in xmlNodeList) 
				{
						if (ifaceName == node.Attributes["name"].Value)
						{
							XmlNode gladeNode = node.ChildNodes[0];
							guiPath = Path.Combine(Boxerp.Defines.GUI_DIR, gladeNode.InnerText);
							guiContainer = gladeNode.Attributes["container"].Value;
							if (node.ChildNodes.Count > 1)
							{
								XmlNode asnode = node.ChildNodes[1];
								//asifacePath = System.AppDomain.CurrentDomain.BaseDirectory + asnode.InnerText;
								asifacePath = Path.Combine(Boxerp.Defines.GUI_DIR, asnode.InnerText);
							}
							else
								asifacePath = null;
						}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		/////////////////////////////////////////////////////////////////////////////
		
		public static void GetIface(string ifaceName, out string guiPath, out string guiContainer)
		{
			guiPath = "";
			guiContainer = "";

			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(Path.Combine(Boxerp.Defines.GUI_DIR, "gui_index.xml"));
				XmlNodeList xmlNodeList = doc.GetElementsByTagName("window");
				foreach (XmlNode node in xmlNodeList) 
				{
						if (ifaceName == node.Attributes["name"].Value)
						{
							XmlNode gladeNode = node.ChildNodes[0];
							guiPath = Path.Combine(Boxerp.Defines.GUI_DIR, gladeNode.InnerText);
							guiContainer = gladeNode.Attributes["container"].Value;
						}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw ex;
			}
		}
		/////////////////////////////////////////////////////////////////////////////
		
		public static void GetIface(string ifaceName, out string guiPath, out string guiContainer,
															out string asifacePath, out string managerClass)
		{
			guiPath = guiContainer = asifacePath = managerClass = "";
			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(Path.Combine(Boxerp.Defines.GUI_DIR, "gui_index.xml"));
				XmlNodeList xmlNodeList = doc.GetElementsByTagName("window");
				foreach (XmlNode node in xmlNodeList) 
				{
						if (ifaceName == node.Attributes["name"].Value)
						{
							XmlNode gladeNode = node.ChildNodes[0];
							guiPath = Path.Combine(Boxerp.Defines.GUI_DIR, gladeNode.InnerText);
							guiContainer = gladeNode.Attributes["container"].Value;
							if (node.ChildNodes.Count > 1)
							{
								XmlNode asnode = node.ChildNodes[1];
								asifacePath = Path.Combine(Boxerp.Defines.GUI_DIR, asnode.InnerText);
								if (node.ChildNodes.Count > 2)
								{
									XmlNode managerNode = node.ChildNodes[2];
									managerClass = managerNode.Attributes["class"].Value;
								}
								else
								{
									// lanzar excepcion
								}
							}
							else
								asifacePath = null;
						}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw ex;
			}
		}
		/////////////////////////////////////////////////////////////////////////////
		
		public static XmlNode GetTreevw(string ifaceName, string treevwName)
		{
			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(Boxerp.Defines.GUI_DIR + "gui_index.xml");
				XmlNodeList xmlNodeList = doc.GetElementsByTagName("window");
				foreach (XmlNode node in xmlNodeList) 
				{
						if (ifaceName == node.Attributes["name"].Value)
						{
							foreach (XmlNode childNode in node.ChildNodes)
							{
								if ((childNode.LocalName == "treeview") 
									&& (treevwName == childNode.Attributes["name"].Value))
									return childNode;
							}
						}
				}
				throw new NullInterfaceException();
			}
			catch (Exception ex)
			{
				ClientCriticalMessages.ThrowMessage(ex.StackTrace, ex.Message);
				throw ex;
			}
		}
		/////////////////////////////////////////////////////////////////////////////
/*		///<summary>
		///	get_iface provides parameters to build a Glade.XML object and a path to 
		///	gui metadata asiface
		///	<param name="ifaceName">Short name of the screen requested</param>
		///	<param name="guiPath">Path to glade file</param>
		///	<param name="guiContainer">Main container at glade file</param>
		///	<param name="asiface">Hashtable with gui metadata</param>
		///	<returns> Status code will be 0 on sucess</returns>
		///</summary>
		public static void GetIface(string ifaceName, out string guiPath, 
									out string guiContainer, out string asifacePath, out XmlNode treeviewnode)
		{
			treeviewnode = null;
			guiContainer = guiPath = asifacePath = "";
			try
			{
				XmlDocument doc = new XmlDocument();
				doc.Load("gui/gui_index.xml");
				XmlNodeList xmlNodeList = doc.GetElementsByTagName("window");
				foreach (XmlNode node in xmlNodeList) 
				{
						if (ifaceName == node.Attributes["name"].Value)
						{
							XmlNode gladeNode = node.ChildNodes[0];
							guiPath = gladeNode.InnerText;
							guiContainer = gladeNode.Attributes["container"].Value;
							Console.WriteLine("cogiendo xml: {0},{1}", guiPath, guiContainer);
							if (node.ChildNodes.Count > 1)
							{
								XmlNode asnode = node.ChildNodes[1];
								asifacePath = asnode.InnerText;
								// Extract the treeview node from xml file
								if (node.ChildNodes.Count > 2)
									treeviewnode = node.ChildNodes[2];
								else
									treeviewnode = null;
							}
							else
								asifacePath = null;
						}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				throw ex;
			}
		}
		*/
	}
}
			







