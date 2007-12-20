//
// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
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
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Xml;

namespace Boxerp.Client.Configuration
{
	/// <summary>
	/// This class gives access to the Boxerp config section within the App.config
	/// </summary>
	public class ClientConfiguration
	{
		private static ClientConfiguration _instance = null;
		private string _bindableWidgetsPrefix;
		private string _guiToolkit;

		private LoggerConfiguration _loggerConfiguration;

		private ClientConfiguration(){ }

		#region Config Data
		public string BindableWidgetsPrefix
		{
			get { return _bindableWidgetsPrefix; }
			set { _bindableWidgetsPrefix = value; }
		}
		
		public LoggerConfiguration Logger
		{
			get { return _loggerConfiguration; }
			set { _loggerConfiguration = value; }
		}

		public string GuiToolkit
		{
			get { return _guiToolkit; }
			set { _guiToolkit = value; }
		}
		#endregion

		/// <summary>
		/// The class is a Singleton and this is the way to access it
		/// </summary>
		/// <returns></returns>
		public static ClientConfiguration GetInstance()
		{
			if (_instance == null)
			{
				_instance = ConfigurationManager.GetSection("boxerp.client") as ClientConfiguration;
			}

			return _instance;
		}
		
		/// <summary>
		/// This is a work around the bug that I've found in Mono regarding the App.config.
		/// If you can't build your app because the compiler complains about the app.config,
		/// you can exclude the file from the config and use this hack
		/// </summary>
		/// <returns></returns>
		[Obsolete]
		public static ClientConfiguration GetInstanceFromFile()
		{
			return GetInstanceFromFile(Path.Combine(
				Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName,
			    "app.config"));
		}

		/// <summary>
		/// This is a work around the bug that I've found in Mono regarding the App.config.
		/// If you can't build your app because the compiler complains about the app.config,
		/// you can exclude the file from the config and use this hack
		/// </summary>
		/// <returns></returns>
		[Obsolete]
		public static ClientConfiguration GetInstanceFromFile(string path)
		{
			Boxerp.Client.Logger.GetInstance().WriteLine("path:" + path);
			if (_instance == null)
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(path);
				
				foreach (XmlNode node in doc.GetElementsByTagName("boxerp.client"))
				{
					Boxerp.Client.Logger.GetInstance().WriteLine("found boxerp.client");
					_instance = BuildFromXmlNode(node);
					break;
				}
			}
			return _instance; 
		}

		internal static ClientConfiguration BuildFromXmlNode(XmlNode sectionNode)
		{
			ClientConfiguration config = new ClientConfiguration();

			foreach (XmlNode node in sectionNode.ChildNodes)
			{
				switch (node.Name)
				{
					case "bindableWidgetsPrefix":
						config.BindableWidgetsPrefix = node.Attributes["prefix"].Value;
						break;
					case "guiToolkit":
						config.GuiToolkit = node.Attributes["name"].Value;
						break;
					case "logger":
						config.Logger = LoggerConfiguration.BuildFromXmlNode(node);
						break;
				}	
			}

			return config;
		}
	}
}
