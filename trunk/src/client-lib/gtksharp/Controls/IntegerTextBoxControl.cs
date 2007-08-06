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
	
	
	public partial class IntegerTextBoxControl : Gtk.Bin, IUIWidget
	{
		private int _maxValue = Int32.MaxValue;
		private BindableWidgetCore WidgetCore;
		
		public IntegerTextBoxControl()
		{
			this.Build();
			WidgetCore = new BindableWidgetCore(this);
		}

		public int Integer
		{
			get
			{
				return Int32.Parse(_textBox.Text);
			}
			set
			{
				_textBox.Text = value.ToString();
			}
		}
		
		public int MaxValue
		{
			get
			{
				return _maxValue;
			}
			set
			{
				_maxValue = value;
			}
		}
		
		public void UpdateValue(object val)
		{
			_textBox.Text = val.ToString();
		}
		
		protected virtual void OnKeyReleased (object o, Gtk.KeyReleaseEventArgs args)
		{
			if (Helper.IsValidKey(args.Event.Key))
			{
				string key = args.Event.Key.ToString();
			    char character = key[key.Length - 1];
				
				if (!Helper.IsValidNumericCharacter(args.Event.Key, character))
				{
					WarningDialog wdialog = new WarningDialog();
					wdialog.Message = "Error: Only numbers are allowed in this box";
					wdialog.Present();
				}
				else
				{
					string text = _textBox.Text;

					string maxIntValue = Int32.MaxValue.ToString();
					if ((text.Length > maxIntValue.Length) || ((text.Length == maxIntValue.Length) && (text.CompareTo(maxIntValue) > 0)))
					{
						WarningDialog wdialog = new WarningDialog();
						wdialog.Message = "Error: Value is too big";
						wdialog.Present();
						Integer = Int32.MaxValue;
					}

					if ((MaxValue != Int32.MaxValue) && (Integer > MaxValue))
					{
						WarningDialog wdialog = new WarningDialog();
						wdialog.Message = "The maximun value allowed is: " + MaxValue;
						wdialog.Present();
						Integer = MaxValue;
					}
					
					if ((WidgetCore.BindingOptions == BindingOptions.TwoWay) && (WidgetCore.BindingProperty != null))
					{
						//Console.WriteLine("On key up:" +_bindingProperty.ToString() + ", " + _textBox.Text);
						
						WidgetCore.SetPropertyValue(Integer);
					}
					else
					{
						//Console.WriteLine(_bindingOptions);
					}
				}
				_textBox.Text = IntegerTextBoxHelper.CleanString(_textBox.Text);
			}
		}
	}
}
