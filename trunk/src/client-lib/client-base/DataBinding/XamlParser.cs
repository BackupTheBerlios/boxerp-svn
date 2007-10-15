////
//// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
////
//// Redistribution and use in source and binary forms, with or
//// without modification, are permitted provided that the following
//// conditions are met:
//// Redistributions of source code must retain the above
//// copyright notice, this list of conditions and the following
//// disclaimer.
//// Redistributions in binary form must reproduce the above
//// copyright notice, this list of conditions and the following
//// disclaimer in the documentation and/or other materials
//// provided with the distribution.
////
//// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY
//// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
//// PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR
//// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
//// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
//// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
//// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
//// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
//// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
//// THE POSSIBILITY OF SUCH DAMAGE.
//
//

using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;

namespace Boxerp.Client
{
	
	
	public class XamlParser
	{
		private const string BOXERP_ASSEMBLY = "assembly=Boxerp.Client.";
		private Dictionary<string, BindingMetadata> _metadataIndexer = new Dictionary<string, BindingMetadata>();
		private string _boxerpXmlns = null;
		private string _xaml = null;
		private XmlDocument _doc;
		
		public virtual System.Collections.Generic.Dictionary<string, BindingMetadata> MetadataIndexer 
		{
			get 
			{
				return _metadataIndexer;
			}
		}

		public XamlParser(string xaml)
		{
			_xaml = xaml;
			_doc = new XmlDocument();
			try
			{
				_doc.Load(_xaml);
			}
			catch (System.IO.FileNotFoundException)
			{
				string projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.ToString();
				string uri = projectFolder + Path.DirectorySeparatorChar.ToString() + _xaml;
				Console.WriteLine(uri);
				_doc.Load(uri);
			}
		}
		
		private void parseBoxerpXmlns()
		{
			foreach (XmlNode node in _doc.ChildNodes)
			{
				foreach (XmlAttribute xmlns in node.Attributes)
				{
					if (xmlns.Value.Contains(BOXERP_ASSEMBLY))
					{
						_boxerpXmlns = xmlns.Name.Split(':')[1];
						return;
					}
				}
			}
			
			throw new NullReferenceException("Boxerp xmlns not found in xaml");
		}
		
		public void ParseXaml()
		{
			parseBoxerpXmlns();
			foreach (XmlNode node in _doc.ChildNodes)
			{
				exploreNode(node);
			}
		}
		
		private void exploreNode(XmlNode node)
		{
			if (node.Name.Contains(_boxerpXmlns + ":"))
			{
				BindingMetadata metadata = new BindingMetadata();
				foreach (XmlAttribute attribute in node.Attributes)
				{
					if (attribute.Name.Contains("x:Name"))
					{
						metadata.WidgetName = attribute.Value;
					}
					else if (attribute.Value.Contains("Binding"))
					{
						metadata.WidgetBindingProperty = attribute.Name;
						metadata.BindingPath = getPath(attribute.Value);
						metadata.BindingOptions = getBindingMode(attribute.Value);
						_metadataIndexer[metadata.WidgetName] = metadata;
					}
				}
			}
			
			foreach (XmlNode child in node.ChildNodes)
			{
				exploreNode(child);
			}
		}
					
		private string getPath(string bindingDetails)
		{
			Console.WriteLine("details =" + bindingDetails);
			int startPath = bindingDetails.IndexOf("Path");
			Console.WriteLine("path =" + startPath);
			int startPathValue = bindingDetails.IndexOf("=", startPath) + 1;
			Console.WriteLine("start path val=" + startPathValue);
			int endPathValue = bindingDetails.IndexOf(" ", startPathValue);
			if (endPathValue == -1)
			{
				endPathValue = bindingDetails.IndexOf("}", startPathValue);
			}
			Console.WriteLine("end path =" + endPathValue);
			int length = endPathValue - startPathValue;
			Console.WriteLine("length =" + length);
			return bindingDetails.Substring(startPathValue, length);
		}
					
		private BindingOptions getBindingMode(string bindingDetails)
		{
			int startMode = bindingDetails.IndexOf("Mode=");
			if (startMode == -1)
			{
				return BindingOptions.TwoWay;
			}
			else
			{
				// TODO : implement this
				return BindingOptions.TwoWay;
			}
		}
	}
}
