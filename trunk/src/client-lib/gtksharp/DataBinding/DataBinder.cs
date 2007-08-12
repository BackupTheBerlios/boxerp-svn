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
using System.Collections.Generic;
using Boxerp.Client.GtkSharp;
using Boxerp.Client;
using System.Reflection;

namespace Boxerp.Client.GtkSharp
{
	
	
	public class DataBinder
	{
		private Gtk.Bin _container;
		private IBindableWrapper _bindable;
		
			
		public DataBinder(Gtk.Bin container, IBindableWrapper bindable)
		{
			_container = container;
			_bindable = bindable;
		}
		
		public void Bind()
		{
			Bind(_container.Name + ".xaml");
		}
		
		public void Bind(string xaml)
		{
			FieldInfo[] fields = _container.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
			foreach (FieldInfo field in fields)
			{
				object fieldValue = field.GetValue(_container);
				Console.WriteLine("------- " + field.Name + field.FieldType);
				//Console.WriteLine(" INTER  " + field.FieldType.GetInterfaces()[0].ToString());
				if (fieldValue is IUIWidget)
				{
					IUIWidget widget = (IUIWidget) fieldValue;
					//Console.WriteLine(" ** widget ** " + widget);
					//widget.WidgetCore.BindObject(_bindable, xxx, yyy, BindingOptions.TwoWay);
					
					Console.WriteLine(_container.Name);
					XamlParser parser = new XamlParser(xaml);
					parser.ParseXaml();
					try
					{
						BindingMetadata metadata = parser.MetadataIndexer[field.Name];
						Console.WriteLine(metadata.WidgetName + "," + metadata.BindingPath);
						widget.WidgetCore.BindObject(_bindable, metadata.BindingPath,  
						                             metadata.WidgetBindingProperty, metadata.BindingOptions);
					}
					catch (KeyNotFoundException)
					{
						Console.Out.WriteLine("The field " + field.Name + "does not have any binding metadata");
					}
				}
			}	                              
		}
	}
}

					
					
					
					
					
					