//
// ErrorManager.cs
//
// Authors:
// 	Zebenzui Perez Ramos <zebenperez@shidix.com>
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
using System.Data;
using System.Collections;
using System.Xml;
using Mono.Unix;

namespace Boxerp.Errors
{

	public struct ErrorsMsg
	{
		public static string LOGIN_INCORRECT  = 
				  Catalog.GetString("Login incorrect. Wrong username or password");
		public static string EMPTY_ENTERPRISE = Catalog.GetString("The enterprise has no groups"); 
		public static string NULL_ENTERPRISE  = Catalog.GetString("The enterprise doesnt exist");
		public static string ENTERPRISE_EXIST = Catalog.GetString("The enterprise already exist");
	}
	/////////////////////////////////////////////////////////////////
    
	public struct Layers
	{
		public static string ERROR   = "ERROR";
		public static string LAYER   = "LAYER";
	}
	/////////////////////////////////////////////////////////////////
    
	public struct PathNodeAttributes
	{
		public static string NAME   = "NAME";
		public static string METHOD   = "METHOD";
	}
	/////////////////////////////////////////////////////////////////

	public struct ErrorNodeAttributes
	{
		public static string ID        = "ID";
		public static string SHORT     = "SHORT_DESC";
		public static string LARGE     = "LARGE_DESC";
		public static string TYPE      = "TYPE";
		public static string EXCEPTION = "EXCEPTION_MSG";
	}
	/////////////////////////////////////////////////////////////////
	
	public struct ErrorNodeValues
	{
		public static string CRITICAL = "CriticalError";
		public static string USER     = "UserError";
		public static string UNKNOWN  = "UnknownError";
	}
	/////////////////////////////////////////////////////////////////
				///<summary>
				///Tools for extract and validate data 
				///</summary>
	public static class ErrorManager
	{
		/*	  ///<summary>
			  ///Create a XML exception description
			  ///</summary>
		public static XmlDocument GenerateError(IDictionary pathValues, IDictionary errorValues)
		{
			try
			{
				XmlDocument doc = new XmlDocument();
				XmlElement pathNode = doc.CreateElement((string)pathValues[PathNodeAttributes.NAME]);
				pathNode.SetAttribute (PathNodeAttributes.METHOD, 
								(string)pathValues[PathNodeAttributes.METHOD]);
				XmlElement errorNode = doc.CreateElement(Layers.ERROR);
				errorNode.SetAttribute (ErrorNodeAttributes.ID, 
								(string)errorValues[ErrorNodeAttributes.ID]);
				errorNode.SetAttribute (ErrorNodeAttributes.SHORT, 
								(string)errorValues[ErrorNodeAttributes.SHORT]);
				errorNode.SetAttribute (ErrorNodeAttributes.LARGE, 
								(string)errorValues[ErrorNodeAttributes.LARGE]);
				errorNode.SetAttribute (ErrorNodeAttributes.EXCEPTION, 
								(string)errorValues[ErrorNodeAttributes.EXCEPTION]);
				pathNode.AppendChild(errorNode);
				doc.AppendChild(pathNode);
				// validate data
				return doc;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}*/
		//////////////////////////////////////////////////////////////////////////////
				///<summary>
				///GenerateError: This method is invoked by AddLayer 
				///when a system exception is catched.
				///So could be a critical excepcion. 
				///<param name="class_name"> The name of the class wich 
				///                          method raises exception </param>
				///<param name="class_method"> The name of the method wich raises exception</param>
				///<param name="error_id"> Error identifier</param>
				///<param name="error_short"> Short message to show to user</param>
				///<param name="error_large"> Full XML information</param>
				///<param name="error_exception">The exception name and .Message </param>
				///</summary>
		public static XmlDocument GenerateError(string class_name, string class_method, 
							 								string error_id, string error_short, 
															string error_large, string error_exception)
		{
			try
			{
				XmlDocument doc = new XmlDocument();
				XmlElement pathNode = doc.CreateElement(class_name);
				pathNode.SetAttribute (PathNodeAttributes.METHOD, class_method);
				XmlElement errorNode = doc.CreateElement(Layers.ERROR);
				errorNode.SetAttribute (ErrorNodeAttributes.TYPE, ErrorNodeValues.CRITICAL);
				errorNode.SetAttribute (ErrorNodeAttributes.ID, error_id);
				errorNode.SetAttribute (ErrorNodeAttributes.SHORT, error_short);
				errorNode.SetAttribute (ErrorNodeAttributes.LARGE, error_large);
				errorNode.SetAttribute (ErrorNodeAttributes.EXCEPTION, error_exception);
				pathNode.AppendChild(errorNode);
				doc.AppendChild(pathNode);
				return doc;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		//////////////////////////////////////////////////////////////////////////////
				///<summary>
				///GenerateError: This method is invoked in other clases to generate 
				///a new exception. This 
				///is usually for notify user errror, such as login incorrect or 
				///something like that.
				///<param name="class_name"> The name of the class wich 
				///                          method raises exception </param>
				///<param name="class_method"> The name of the method wich 
				///                            raises exception</param>
				///<param name="error_exception">The exception .Message </param>
				///</summary>
		public static string GenerateError(string class_name, string class_method, 
							 						string error_exception)
		{
			try
			{
				XmlDocument doc = new XmlDocument();
				XmlElement pathNode = doc.CreateElement(class_name);
				pathNode.SetAttribute (PathNodeAttributes.METHOD, class_method);
				XmlElement errorNode = doc.CreateElement(Layers.ERROR);
				errorNode.SetAttribute (ErrorNodeAttributes.TYPE, ErrorNodeValues.USER);
				//errorNode.SetAttribute (ErrorNodeAttributes.ID, "");
				//errorNode.SetAttribute (ErrorNodeAttributes.SHORT, "");
				//errorNode.SetAttribute (ErrorNodeAttributes.LARGE, "");
				errorNode.SetAttribute (ErrorNodeAttributes.EXCEPTION, error_exception);
				pathNode.AppendChild(errorNode);
				doc.AppendChild(pathNode);
				return doc.OuterXml;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		//////////////////////////////////////////////////////////////////////////////
				///<summary>
				///When an exception is catched must be wrapped to reach the client
				///<param name="ex">The catched exceptcion </param>
				///<param name="class_name">The name of the class wich method is 
				///                         catching the exception</param>
				///<param name="class_method">The name of the method wicth 
				///                           is catching the method</param>
				///</summary>
		public static string AddLayer(Exception ex, string class_name, string class_method)
		{
			XmlDocument doc;
			try
			{
				doc = new XmlDocument();
				// If ex is a wrapped exception (xml):
				doc.LoadXml(ex.Message);
				XmlElement firstPathNode = doc.DocumentElement;
				XmlElement pathNode = doc.CreateElement(class_name);
				pathNode.SetAttribute (PathNodeAttributes.METHOD, class_method);
				pathNode.AppendChild(firstPathNode);
				doc.LoadXml(pathNode.OuterXml);
				return doc.OuterXml;
				// Else:
			}
			catch (XmlException xmlEx)
			{
				// FIXME: Get all stackTrace: now it builds an string but must build an xml
				Exception e = ex;
				string exceptionTrace = "";
				while (true)
				{
					if (e == null)
					 	break;
					exceptionTrace += e.ToString() + ":" + e.Message; // + " Traza: " + e.StackTrace;
					e = e.InnerException;
				}
				doc = GenerateError(class_name, class_method, "", ex.StackTrace, 
									 		exceptionTrace, ex.ToString() + ":" + ex.Message);
				XmlElement firstPathNode = doc.DocumentElement;
				XmlElement pathNode = doc.CreateElement(class_name);
				pathNode.SetAttribute (PathNodeAttributes.METHOD, class_method);
				pathNode.AppendChild(firstPathNode);
				doc.LoadXml(pathNode.OuterXml);
				return pathNode.OuterXml;
			}
		}
		//////////////////////////////////////////////////////////////////////////////
	}
}		//////////////////////////////////////////////////////////////////////////////
		/*public static XmlDocument AddLayer(XmlDocument doc, IDictionary pathValues)
		{
			try
			{
                XmlElement firstPathNode = doc.DocumentElement;

                XmlElement pathNode = doc.CreateElement((string)pathValues[PathNodeAttributes.NAME]);
                pathNode.SetAttribute (PathNodeAttributes.METHOD, (string)pathValues[PathNodeAttributes.METHOD]);

                pathNode.AppendChild(firstPathNode);
                return doc;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}*/

