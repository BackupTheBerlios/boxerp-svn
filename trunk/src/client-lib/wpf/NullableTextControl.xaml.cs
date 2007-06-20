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
	/// Interaction logic for NullableTextControl.xaml
	/// </summary>

	public partial class NullableTextControl : System.Windows.Controls.UserControl
	{
		public NullableTextControl()
		{
			InitializeComponent();
		}

		public static DependencyProperty TitleProperty = DependencyProperty.Register(
			"Title",
			typeof(string),
			typeof(NullableTextControl),
			new FrameworkPropertyMetadata("title", FrameworkPropertyMetadataOptions.AffectsRender,
				new PropertyChangedCallback(OnTitleChanged), null));

		private static void OnTitleChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			NullableTextControl control = (NullableTextControl)o;
			control._title.Content = (string)e.NewValue;
		}

		public static DependencyProperty TextProperty = DependencyProperty.Register(
			"Text",
			typeof(string),
			typeof(NullableTextControl),
			new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender,
				new PropertyChangedCallback(OnTextChanged), null));

		private static void OnTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			NullableTextControl control = (NullableTextControl)o;
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