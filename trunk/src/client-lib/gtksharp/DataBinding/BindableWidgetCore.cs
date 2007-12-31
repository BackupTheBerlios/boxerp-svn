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
	/// <summary>
	/// This class is only used by the framework and it could be internal in 
	/// future releases.
	/// </summary>
	public class BindableWidgetCore : IBindableCore
	{
		private IBindableWrapper _bindableObject = null;
		private object _propertyOwner;
		private PropertyInfo _boBindingProperty = null;
		private string _boPropertyName = null;
		private string _widgetBindingProperty = null;
		private BindingOptions _bindingOptions = BindingOptions.OneWay;
		private IBindableWidget _uiWidget;
		
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

		public BindableWidgetCore(IBindableWidget uiWidget)
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
			Logger.GetInstance().WriteLine("Binding Object property:" + _boBindingProperty.Name);
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
			Logger.GetInstance().WriteLine("Property owner= " + _propertyOwner);
			Logger.GetInstance().WriteLine("bo binding property = " + _boBindingProperty.Name);
			// read the value of the property for the first time
			OnPropertyChanged(this, new PropertyChangedEventArgs(_boPropertyName));
			
		}
		
		public void OnPropertyChanged(object o, PropertyChangedEventArgs args)
		{
			try
			{
				Logger.GetInstance().WriteLine("binding prop changed:" + args.PropertyName + "," + _boPropertyName + "," + _propertyOwner
				                  + ", " + _boBindingProperty);
				Logger.GetInstance().WriteLine("binding prop " +_widgetBindingProperty + "," + _uiWidget);
				if (args.PropertyName.Equals(_boPropertyName))
				{
					object propValue = _boBindingProperty.GetValue(_propertyOwner, null);
					Logger.GetInstance().WriteLine("prop new value = " + propValue);
					if (propValue != null)
					{
						_uiWidget.OnBoundDataChanged(_widgetBindingProperty, propValue);
					}
				}
			}
			catch (NullReferenceException ex)
			{
				Logger.GetInstance().WriteLine("The binding path is not correct:" + args.PropertyName);
				throw ex;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
				
		public void SetPropertyValue(object val)
		{
			if ((BindingOptions == BindingOptions.TwoWay) && (BOBindingProperty != null))
			{
				Logger.GetInstance().WriteLine("setting value: " + val);
				_boBindingProperty.SetValue(_propertyOwner, val, null);
			}
		}
		
		public void SetPropertyValue(string propertyName, object val)
		{
			if ((BindingOptions == BindingOptions.TwoWay) && (BOBindingProperty != null))
			{
				Logger.GetInstance().WriteLine("setting value: " + val);
				_boBindingProperty.SetValue(_propertyOwner, val, null);
			}
		}
		
	}
}
