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

namespace Boxerp.Client.WPF.Controls
{
	/// <summary>
	/// Interaction logic for DecimalTextBoxControl.xaml
	/// </summary>

	public partial class DecimalTextBoxControl : System.Windows.Controls.UserControl
	{

		private char _decimalSeparator = '.';
		private int _decimalDigits = 2;

		public DecimalTextBoxControl()
		{
			InitializeComponent();
		}

		public static DependencyProperty MaxValueProperty = DependencyProperty.Register(
			"MaxValue",
			typeof(int?),
			typeof(DecimalTextBoxControl),
			new FrameworkPropertyMetadata(Int32.MaxValue, FrameworkPropertyMetadataOptions.AffectsRender,
				new PropertyChangedCallback(OnMaxValueChanged), null));

		private static void OnMaxValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e){}

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

		public static DependencyProperty TextProperty = DependencyProperty.Register(
			"Text",
			typeof(string),
			typeof(DecimalTextBoxControl),
			new FrameworkPropertyMetadata("0.0", FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnTextChanged), null));

		private static void OnTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			DecimalTextBoxControl control = (DecimalTextBoxControl)o;
			if (e.NewValue != null)
			{
				control._textBox.Text = control.CleanString((string)e.NewValue);
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
				return (string)GetValue(TextProperty);
			}
			set
			{
				SetValue(TextProperty, _textBox.Text);
			}
		}

		public static DependencyProperty DecimalSeparatorProperty = DependencyProperty.Register(
			"DecimalSeparator",
			typeof(char),
			typeof(DecimalTextBoxControl),
			new FrameworkPropertyMetadata('.', FrameworkPropertyMetadataOptions.None, null, null));

		public char DecimalSeparator
		{
			get { return _decimalSeparator; }
			set { _decimalSeparator = value; }
		}

		public static DependencyProperty DecimalDigitsProperty = DependencyProperty.Register(
			"DecimalDigits",
			typeof(int),
			typeof(DecimalTextBoxControl),
			new FrameworkPropertyMetadata(2, FrameworkPropertyMetadataOptions.None, null, null));

		public int DecimalDigits
		{
			get { return _decimalDigits; }
			set { _decimalDigits = value; }
		}

		
		private string CleanString()
		{
			return CleanString(_textBox.Text);
		}

		private string CleanString(string val)
		{
			string currentStr = val;
			string cleaned = String.Empty;
			if (currentStr.Length > 0)
			{
				bool readingDecimals = false;
				int decimals = 0;
				foreach (char c in currentStr)
				{
					if ((readingDecimals) && (decimals == DecimalDigits))
					{
						break;
					}
					else if ((readingDecimals) && (c == DecimalSeparator))
					{
						break;
					}
					else if (readingDecimals)
					{
						decimals++;
					}

					if (Char.IsNumber(c))
					{
						cleaned += c.ToString();
					}
					else if (c == DecimalSeparator)
					{
						readingDecimals = true;
						cleaned += c.ToString();
					}
				}
			}
			return cleaned;
		}

		private bool ContainsDecimalSymbol()
		{
			if (_textBox.Text.Contains(DecimalSeparator.ToString()))
			{
				if (_textBox.Text.IndexOf(DecimalSeparator.ToString()) == _textBox.Text.LastIndexOf(DecimalSeparator.ToString()))
				{
					return false;
				}
			}
			else
			{
				return false;
			}

			return true;
		}

		private void OnKeyUp(Object sender, KeyEventArgs args)
		{
			if (Helper.IsValidKey(args.Key))
			{
				string key = args.Key.ToString();
				char character = key[key.Length - 1];

				if (((args.Key == Key.OemPeriod) && (DecimalSeparator == '.')) || 
					((args.Key == Key.OemComma) && (DecimalSeparator == ',')))
				{
					character = DecimalSeparator;
				}

				if ((!Helper.IsValidCharacter(args.Key, character)) && (character != DecimalSeparator))
				{
					MessageBox.Show("Error: Only numbers and decimal separator are allowed in this box");
					_textBox.Text = CleanString();
				}
				else
				{
					if (ContainsDecimalSymbol())
					{
						MessageBox.Show("The decimal separator has been already set");
						_textBox.Text = CleanString();
					}
				}

				int indexOfDecimal = _textBox.Text.IndexOf(DecimalSeparator.ToString());
				int lengthOfDecimals = _textBox.Text.Length - indexOfDecimal;

				if ((_textBox.Text.Contains(DecimalSeparator.ToString()) && 
					(_textBox.Text.Substring(indexOfDecimal, lengthOfDecimals -1).Length > DecimalDigits)))
				{
					_textBox.Text = CleanString();
				}

				Text = _textBox.Text;
				decimal val = Convert.ToDecimal(Text);
				if ((MaxValue != null) && (val > MaxValue))
				{
					MessageBox.Show("The maximun value allowed is: " + MaxValue);
					_textBox.Text = MaxValue.ToString();
					Text = _textBox.Text;
				}
			}
		}
	}
}