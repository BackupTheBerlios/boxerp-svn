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
using System.Windows.Shapes;

namespace Boxerp.Client.WPF
{
	/// <summary>
	/// Interaction logic for ErrorWindow.xaml
	/// </summary>

	public partial class ErrorWindow : System.Windows.Window
	{

		public ErrorWindow()
		{
			InitializeComponent();
		}

		public string Msg
		{
			get
			{
				return _msg.Text;
			}
			set
			{
				_msg.Text = value;
			}
		}

		public void OnClose(Object sender, RoutedEventArgs args)
		{
			Close();
		}
	}
}