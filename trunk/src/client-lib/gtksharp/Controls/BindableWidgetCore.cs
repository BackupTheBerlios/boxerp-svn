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


namespace Boxerp.Client.GtkSharp.Controls
{
	public class BindableWidgetCore : IBindableWidget
	{
		private IBindableWrapper _bindableObject = null;
		private object _propertyOwner;
		private PropertyInfo _bindingProperty = null;
		private string _propertyName = null;
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

		public virtual System.Reflection.PropertyInfo BindingProperty 
		{
			get 
			{
				return _bindingProperty;
			}
		}

		public virtual string PropertyName 
		{
			get 
			{
				return _propertyName;
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
		
		public void BindObject(IBindableWrapper bObject, object propertyOwner, string bindingProperty, BindingOptions options)
		{
			_bindableObject = bObject;
			_propertyOwner = propertyOwner;
			_bindingProperty = propertyOwner.GetType().GetProperty(bindingProperty);
			_propertyName = bindingProperty;
			if (_bindingProperty == null)
			{
				throw new NullReferenceException("Error binding object. Property " + bindingProperty + " doesn't exist");
			}
			Console.WriteLine("Binding Object property:" + _bindingProperty.Name);
			_bindableObject.PropertyChanged += OnPropertyChanged;
			_bindingOptions = options;
		}
		
		public void OnPropertyChanged(object o, PropertyChangedEventArgs args)
		{
			//Console.WriteLine("binding prop changed:" + args.PropertyName + "," + _propertyName);
			if (args.PropertyName.Equals(_propertyName))
			{
				object propValue = _bindingProperty.GetValue(_propertyOwner, null);
				Console.WriteLine("prop new value = " + propValue);
				if (propValue != null)
				{
					_uiWidget.UpdateValue(propValue);
				}
			}
		}
				
		public void SetPropertyValue(object val)
		{
			_bindingProperty.SetValue(_propertyOwner, val, null);	
		}
		
	}
}
