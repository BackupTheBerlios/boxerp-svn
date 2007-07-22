// /home/carlos/boxerp_completo/trunk/src/client-lib/gtksharp/Controls/Controls/IntegerTextBoxControl.cs created with MonoDevelop
// User: carlos at 10:33 PM 7/21/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gdk;
using Boxerp.Client.GtkSharp;
using System.Reflection;
using System.ComponentModel;

namespace Boxerp.Client.GtkSharp.Controls
{
	
	
	public partial class IntegerTextBoxControl : Gtk.Bin, IBindableWidget
	{
		private IBindableWrapper _bindableObject = null;
		private object _owner;
		private PropertyInfo _bindingProperty = null;
		private string _propertyName = null;
		private BindingOptions _bindingOptions = BindingOptions.OneWay;
		
		public IntegerTextBoxControl()
		{
			this.Build();
		}

		public void BindObject(IBindableWrapper bObject, object owner, string bindingProperty, BindingOptions options)
		{
			_bindableObject = bObject;
			_owner = owner;
			_bindingProperty = owner.GetType().GetProperty(bindingProperty);
			_propertyName = bindingProperty;
			if (_bindingProperty == null)
			{
				throw new NullReferenceException("Error binding object. Property " + bindingProperty + " doesn't exist");
			}
			Console.WriteLine("Binding Object property:" + _bindingProperty.Name);
			_bindableObject.PropertyChanged += OnBindingPropertyChanged;
			_bindingOptions = options;
		}
		
		private void OnBindingPropertyChanged(object o, PropertyChangedEventArgs args)
		{
			Console.WriteLine("binding prop changed:" + args.PropertyName + "," + _propertyName);
			if (args.PropertyName.Equals(_propertyName))
			{
				object propValue = _bindingProperty.GetValue(_owner, null);
				Console.WriteLine("prop new value = " + propValue);
				if (propValue != null)
				{
					_textBox.Text = propValue.ToString();
				}
			}
		}
		
		protected virtual void OnKeyReleased (object o, Gtk.KeyReleaseEventArgs args)
		{
			
			if ((args.Event.Key != Key.Tab) && (args.Event.Key != Key.Delete) &&
				(args.Event.Key != Key.Left) && (args.Event.Key != Key.Right) &&
				(args.Event.Key != Key.Return) && (args.Event.Key != Key.End) &&
				(args.Event.Key != Key.Home) && (args.Event.Key != Key.Clear) &&
				(args.Event.Key != Key.BackSpace))
			{
				string key = args.Event.Key.ToString();
			    char character = key[key.Length - 1];
				Console.WriteLine(character);
				if (!System.Char.IsNumber(character))
				{
					WarningDialog wdialog = new WarningDialog();
					wdialog.Message = "Error: Only numbers are allowed in this box";
					wdialog.Present();
					_textBox.Text = IntegerTextBoxHelper.CleanString(_textBox.Text);
				}
				else
				{
					if ((_bindingOptions == BindingOptions.TwoWay) && (_bindingProperty != null))
					{
						Console.WriteLine("On key up:" +_bindingProperty.ToString() + ", " + _textBox.Text);
						_bindingProperty.SetValue(_owner, Int32.Parse(_textBox.Text), null);
					}
					else
					{
						Console.WriteLine(_bindingOptions);
					}
				}
			}
		}
	}
}
