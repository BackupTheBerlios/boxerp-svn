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
using Gdk;
using Boxerp.Client.GtkSharp;
using System.Reflection;
using System.ComponentModel;


namespace Boxerp.Client.GtkSharp
{
	public class BindableWidgetCore : IBindableWidget
	{
		private IBindableWrapper _bindableObject = null;
		private object _propertyOwner;
		private PropertyInfo _boBindingProperty = null;
		private string _boPropertyName = null;
		private string _widgetBindingProperty = null;
		private BindingOptions _bindingOptions = BindingOptions.OneWay;
		private IUIWidget _uiWidget;
		
		public virtual Boxerp.Client.IBindableWrapper BindableObject 
		{
			get 
			{
				return _bindableObject;
			}
		}

		public virtual object PropertyOwner 
		{
			get 
			{
				return _propertyOwner;
			}
		}

		public virtual System.Reflection.PropertyInfo BOBindingProperty 
		{
			get 
			{
				return _boBindingProperty;
			}
		}

		public virtual string BOPropertyName 
		{
			get 
			{
				return _boPropertyName;
			}
		}

		public virtual Boxerp.Client.BindingOptions BindingOptions 
		{
			get 
			{
				return _bindingOptions;
			}
		}

		public BindableWidgetCore(IUIWidget uiWidget)
		{
			_uiWidget = uiWidget;
		}
		
		public void BindObject(IBindableWrapper bObject, object propertyOwner, 
		                       string bindingProperty, string widgetProperty, BindingOptions options)
		{
			_widgetBindingProperty = widgetProperty; 
			_bindableObject = bObject;
			_propertyOwner = propertyOwner;
			_boBindingProperty = propertyOwner.GetType().GetProperty(bindingProperty);
			_boPropertyName = bindingProperty;
			if (_boBindingProperty == null)
			{
				throw new NullReferenceException("Error binding object. Property " + bindingProperty + " doesn't exist");
			}
			Console.WriteLine("Binding Object property:" + _boBindingProperty.Name);
			_bindableObject.PropertyChanged += OnPropertyChanged;
			_bindingOptions = options;
		}
		
		public void BindObject(IBindableWrapper bObject, string path, string widgetProperty, BindingOptions options)
		{
			_widgetBindingProperty = widgetProperty;
			_bindableObject = bObject;
			_bindingOptions = options;
			_bindableObject.PropertyChanged += OnPropertyChanged;
			
			string[] pathArray = path.Split('.');
			_boPropertyName = pathArray[pathArray.Length -1]; // the last element of the path is the property
			// now get the object whose property we've got
			if (pathArray.Length == 1)      
			{
				_propertyOwner = bObject;
			}
			else
			{
				// the first element of the path have to be a property of the bObject
				int i = 0;
				PropertyInfo prop = bObject.GetType().GetProperty(pathArray[0]);
				_propertyOwner = prop.GetValue(bObject, null);
				while (pathArray.Length -2 > i)
				{
					i++;
					prop = _propertyOwner.GetType().GetProperty(pathArray[i]);
					_propertyOwner = prop.GetValue(_propertyOwner, null); // try this with indexed properties!
					_boBindingProperty = _propertyOwner.GetType().GetProperty(_boPropertyName);
				}
			}
			
			Console.WriteLine("}}}" + _propertyOwner);
			Console.WriteLine("}}}" + _boBindingProperty.Name);
		}
		
		public void OnPropertyChanged(object o, PropertyChangedEventArgs args)
		{
			//Console.WriteLine("binding prop changed:" + args.PropertyName + "," + _propertyName);
			if (args.PropertyName.Equals(_boPropertyName))
			{
				object propValue = _boBindingProperty.GetValue(_propertyOwner, null);
				Console.WriteLine("prop new value = " + propValue);
				if (propValue != null)
				{
					_uiWidget.UpdateValue(_widgetBindingProperty, propValue);
				}
			}
		}
				
		public void SetPropertyValue(object val)
		{
			if ((BindingOptions == BindingOptions.TwoWay) && (BOBindingProperty != null))
			{
				Console.WriteLine("setting value: " + val);
				_boBindingProperty.SetValue(_propertyOwner, val, null);
			}
		}
		
	}
}