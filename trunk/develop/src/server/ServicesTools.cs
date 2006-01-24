//
// ServicesTools.cs
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
using System.IO;
using System.Data;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Services;
using System.Xml;
using Boxerp.Database;
using Boxerp.Exceptions;
using Boxerp.Errors;
using Boxerp.Debug;
using Boxerp.Objects;

namespace Boxerp.Tools
{
			///<summary>
			/// Usefull and needed method for all Web Services
			///</summary>
	public static class ServicesTools
	{
		static HttpChannel chan;
		static IDictionary channelProperties;
			  /// <summary>
			  ///	This method connects the Web Service to the Objects Server getting
			  ///	proxy objects.
			  /// </summary>
		public static void Connect2ObjectsServer(ref ConcurrencyControllerObject concurrencyObj, 
							 						ref HistoryObject historyObj, 
													ref SessionsObject sessionsObj)
		{
			const string channelName = "BoxerpChannel";
			string configFile;
			try
			{
				// Initialize channel:
				if (channelProperties == null)
				{
					channelProperties = new Hashtable();
					channelProperties["name"] = channelName;
					channelProperties["port"] = 0;
					channelProperties["ref"] = "http";
					channelProperties["timeout"] = 2000;
				}
				if (chan == null && ChannelServices.GetChannel(channelName) == null)
				{
					chan = new HttpChannel(channelProperties, 
													new SoapClientFormatterSinkProvider(),
													new SoapServerFormatterSinkProvider());
					ChannelServices.RegisterChannel(chan);
				}
				configFile = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, 
											 		"server/clientRemoting.config");
				XmlDocument doc = new XmlDocument();
				doc.Load(configFile);
				XmlNodeList xmlNodeList = doc.GetElementsByTagName("wellknown");
				string objFullName, objUrl;
				foreach (XmlNode node in xmlNodeList) 
				{
					objUrl      = node.Attributes["url"].Value;   
					objFullName = node.Attributes["type"].Value.Split(',')[0];
					switch (objFullName)
					{
						case "Boxerp.Objects.ConcurrencyControllerObject":
							concurrencyObj = (ConcurrencyControllerObject) Activator.GetObject(
										typeof(Boxerp.Objects.ConcurrencyControllerObject), objUrl);
							break;
						case "Boxerp.Objects.HistoryObject":
							historyObj = (HistoryObject) Activator.GetObject(
										typeof(Boxerp.Objects.HistoryObject), objUrl);
							break;
						case "Boxerp.Objects.SessionsObject":
							sessionsObj = (SessionsObject) Activator.GetObject(
										typeof(Boxerp.Objects.SessionsObject), objUrl);
							break;
					}
				}
				/*if (historyObj == null || concurrencyObj == null)
				{
					concurrencyObj = new ConcurrencyControllerObject();
					historyObj     = new HistoryObject();
				}*/
			}
			catch (Exception e)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(e, "ServiceTools", "Connect2ObjectsServer")); 
				throw wrappedEx;
			}
		}
		
		///////////////////////////////////////////////////////////////////////////////
	}
}

