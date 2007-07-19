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
	/// Interaction logic for NullableIntegerControl.xaml
	/// </summary>

	public partial class NullableIntegerControl : System.Windows.Controls.UserControl
	{
		public NullableIntegerControl()
		{
			InitializeComponent();
		}

		public static DependencyProperty TitleProperty = DependencyProperty.Register(
			"Title",
			typeof(string),
			typeof(NullableIntegerControl),
			new FrameworkPropertyMetadata("title", FrameworkPropertyMetadataOptions.AffectsRender,
			new PropertyChangedCallback(OnTitleChanged), null));

		private static void OnTitleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			NullableIntegerControl control = (NullableIntegerControl)o;
			control._title.Content = (string)e.NewValue;
		}

		public static DependencyProperty TextProperty = DependencyProperty.Register(
			"Integer",
			typeof(int?),
			typeof(NullableIntegerControl),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender,
				new PropertyChangedCallback(OnIntegerChanged), null));

		private static void OnIntegerChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue != null)
			{
				NullableIntegerControl control = (NullableIntegerControl)o;
				control._text._textBox.Text = e.NewValue.ToString();
			}
		}

		public string Title
		{
			get
			{
				return (string)GetValue(TitleProperty);
			}
			set
			{
				SetValue(TitleProperty, value);
			}
		}

		public int? Integer
		{
			get
			{
				if ((_text._textBox.Text != String.Empty) && (_checkBox.IsChecked == true))
				{
					return Int32.Parse(_text._textBox.Text);
				}
				else
				{
					return null;
				}
			}
			set
			{
				SetValue(TextProperty, value);
				if (value != null)
				{
					_checkBox.IsChecked = true;
					_text._textBox.IsEnabled = true;
					_text.IsEnabled = true;
				}
			}
		}

		public void OnCheckBoxClicked(Object sender, RoutedEventArgs args)
		{
			if (_checkBox.IsChecked == true)
			{
				_text.IsEnabled = true;
			}
			else
			{
				_text.IsEnabled = false;
			}
		}

	}
}
