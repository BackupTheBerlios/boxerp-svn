//
// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
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
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Boxerp.Client;

namespace Boxerp.Client.WPF.Controls
{
	/// <summary>
	/// Interaction logic for IntegerTextBox.xaml
	/// </summary>

	public partial class IntegerTextBox : System.Windows.Controls.UserControl
	{
		private bool _cleaned = false;

		public bool Cleaned
		{
			get { return _cleaned; }
			set { _cleaned = value; }
		}

		public IntegerTextBox()
		{
			InitializeComponent();
		}

		public event EventHandler IntegerChanged;

		public static DependencyProperty MaxValueProperty = DependencyProperty.Register(
			"MaxValue",
			typeof(int?),
			typeof(IntegerTextBox),
			new FrameworkPropertyMetadata(Int32.MaxValue, FrameworkPropertyMetadataOptions.AffectsRender,
				new PropertyChangedCallback(OnMaxValueChanged), null));

		private static void OnMaxValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			
		}

		public int? MaxValue
		{
			get
			{
				return (int?)GetValue(MaxValueProperty);
			}
			set
			{
				SetValue(MaxValueProperty, value);
			}
		}


		public static DependencyProperty IntegerProperty = DependencyProperty.Register(
			"Integer",
			typeof(int?),
			typeof(IntegerTextBox),
			new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.AffectsRender,
				new PropertyChangedCallback(OnIntegerChanged), null));

		private static void OnIntegerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			IntegerTextBox control = (IntegerTextBox)o;
			if (e.NewValue != null)
			{
				if (control.Cleaned)
				{
					control._textBox.Text = e.NewValue.ToString();
				}
				else
				{
					control._textBox.Text = IntegerTextBoxHelper.CleanString(e.NewValue.ToString());
				}
			}
		}

		public int? Integer
		{
			get
			{
				if (_textBox.Text.Length == 0)
				{
					return null;
				}
				return (int?)GetValue(IntegerProperty);
			}
			set
			{
				lock (this)
				{
					Cleaned = true;
					SetValue(IntegerProperty, value);
					Cleaned = false;
					if (IntegerChanged != null)
					{
						IntegerChanged.Invoke(this, null);
					}
				}
			}
		}

		public string Text
		{
			get
			{
				if (_textBox.Text.Length == 0)
				{
					return "0";
				}
				return _textBox.Text;
			}
			set
			{
				string cleaned = IntegerTextBoxHelper.CleanString(value);
				if (cleaned.Length == 0)
				{
					Integer = 0;
				}
				else
				{
					Integer = Int32.Parse(cleaned);
				}
			}
		}

		private string CleanString()
		{
			return  IntegerTextBoxHelper.CleanString(_textBox.Text);
		}

		private void OnKeyUp(Object sender, KeyEventArgs args)
		{
			if (Helper.IsValidKey(args.Key))
			{
				string key = args.Key.ToString();
				char character = key[key.Length - 1];
				
				if (!Helper.IsNumber(args.Key, character))
				{
					MessageBox.Show("Error: Only numbers are allowed in this box");
				}
				
				string text = _textBox.Text;

				string maxIntValue = Int32.MaxValue.ToString();
				if ((text.Length > maxIntValue.Length) || ((text.Length == maxIntValue.Length) && (text.CompareTo(maxIntValue) > 0)))
				{
					MessageBox.Show("Error: The value is too big");
					_textBox.Text = maxIntValue;
				}

				string cleaned = CleanString();
				_textBox.Text = cleaned;
				if (cleaned.Length == 0)
				{
					Integer = 0;
				}
				else
				{
					Integer = Int32.Parse(cleaned);
				}

				if ((MaxValue != null) && (Integer > MaxValue))
				{
					MessageBox.Show("The maximun value allowed is: " + MaxValue);
					Integer = MaxValue;
				}
			}
		}
	}
}
