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
	/// Interaction logic for NullableTextBox.xaml
	/// </summary>

	public partial class NullableTextBox : System.Windows.Controls.UserControl
	{
		public NullableTextBox()
		{
			InitializeComponent();
		}

		public static DependencyProperty TitleProperty = DependencyProperty.Register(
			"Title",
			typeof(string),
			typeof(NullableTextBox),
			new FrameworkPropertyMetadata("title", FrameworkPropertyMetadataOptions.AffectsRender,
				new PropertyChangedCallback(OnTitleChanged), null));

		private static void OnTitleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			NullableTextBox control = (NullableTextBox)o;
			control._title.Content = (string)e.NewValue;
		}

		public static DependencyProperty TextProperty = DependencyProperty.Register(
			"Text",
			typeof(string),
			typeof(NullableTextBox),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender,
				new PropertyChangedCallback(OnTextChanged), null));

		private static void OnTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			NullableTextBox control = (NullableTextBox)o;
			control._text.Text = (string)e.NewValue;
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

		public string Text
		{
			get
			{
				return (string)GetValue(TextProperty);
			}
			set
			{
				SetValue(TextProperty, value);
			}
		}

		public void OnCheckBoxClicked(Object sender, RoutedEventArgs args)
		{
			if (_checkBox.IsChecked == true)
			{
				_checkBox.IsEnabled = true;
			}
			else
			{
				_checkBox.IsEnabled = false;
			}
		}

	}
}
