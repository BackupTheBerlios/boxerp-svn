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
using Boxerp.Client;

namespace Boxerp.Client.GtkSharp.Controls
{
	
	
	public partial class IntegerTextBox : Gtk.Bin, IBindableWidget
	{
		private int _maxValue = Int32.MaxValue;
		private BindableWidgetCore _widgetCore;
		
		public IntegerTextBox()
		{
			this.Build();
			_widgetCore = new BindableWidgetCore(this);
		}

#region IUIWidget implementation
		public BindableWidgetCore WidgetCore
		{
			get
			{
				return _widgetCore;
			}
		}

		public void BindObject(IBindableWrapper wrapper, string path, string widgetProperty, BindingOptions options)
		{
			_widgetCore.BindObject(wrapper, path, widgetProperty, options);
		}
		
		public void BindObject(IBindableWrapper wrapper, object owner, string path, string widgetProperty, BindingOptions options)
		{
			_widgetCore.BindObject(wrapper, owner, path, widgetProperty, options);
		}
		
		public void UpdateValue(string property, object val)
		{
			_textBox.Text = val.ToString();
		}
#endregion
		
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
					
					WidgetCore.SetPropertyValue(Integer);
				}
				_textBox.Text = IntegerTextBoxHelper.CleanString(_textBox.Text);
			}
		}
	}
}
