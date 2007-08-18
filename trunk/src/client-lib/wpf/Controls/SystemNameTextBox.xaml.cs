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
	/// Interaction logic for SystemNameTextBox.xaml
	/// </summary>

	public partial class SystemNameTextBox : System.Windows.Controls.UserControl
	{
		private bool _cleaned = false;

		public bool Cleaned
		{
			get { return _cleaned; }
			set { _cleaned = value; }
		}
		
		public SystemNameTextBox()
		{
			InitializeComponent();
		}

		public static DependencyProperty MaxLengthProperty = DependencyProperty.Register(
			"MaxLength",
			typeof(int),
			typeof(SystemNameTextBox),
			new FrameworkPropertyMetadata(Int32.MaxValue, FrameworkPropertyMetadataOptions.AffectsRender,
				new PropertyChangedCallback(OnMaxLengthChanged), null));

		private static void OnMaxLengthChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) { }

		public int MaxLength
		{
			get
			{
				return (int)GetValue(MaxLengthProperty);
			}
			set
			{
				SetValue(MaxLengthProperty, value);
			}
		}

		public static DependencyProperty TextProperty = DependencyProperty.Register(
			"Text",
			typeof(string),
			typeof(SystemNameTextBox),
			new FrameworkPropertyMetadata(String.Empty, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OnTextChanged), null));

		private static void OnTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			SystemNameTextBox control = (SystemNameTextBox)o;
			if (e.NewValue != null)
			{
				if (control.Cleaned)
				{
					control._textBox.Text = (string)e.NewValue;
				}
				else
				{
					control._textBox.Text = SystemNameTextBoxHelper.CleanString((string)e.NewValue);
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
				return (string)GetValue(TextProperty);
			}
			set
			{
				lock (this)
				{
					Cleaned = true;
					if (value != null)
					{
						Cleaned = true;
						string cleaned = SystemNameTextBoxHelper.CleanString(value);
						SetValue(TextProperty, cleaned);
						Cleaned = false;
					}
				}
			}
		}

		private string CleanString()
		{
			return SystemNameTextBoxHelper.CleanString(_textBox.Text);
		}

		private void OnKeyUp(Object sender, KeyEventArgs args)
		{
			if (Helper.IsValidKey(args.Key))
			{
				string key = args.Key.ToString();
				char character = key[key.Length - 1];

				if ((!Helper.IsDeleteOrBack(args.Key) && (key.Length > 2)) || (!Helper.IsLetterOrDigit(args.Key, character)))
				{
					MessageBox.Show("Error: Only letters from 'a' to 'z' and numbers are allowed in this box");
				}

				_textBox.Text = CleanString();
				Text = _textBox.Text;
				
				if (_textBox.Text.Length > MaxLength)
				{
					MessageBox.Show("The maximun length allowed is: " + MaxLength);
					_textBox.Text = _textBox.Text.Substring(0, MaxLength);
					Text = _textBox.Text;
				}
			}
		}
	}
}