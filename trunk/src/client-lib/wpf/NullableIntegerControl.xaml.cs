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

namespace Boxerp.Client.WPF
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