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
	/// Interaction logic for QuestionWindow.xaml
	/// </summary>

	public partial class QuestionWindow : System.Windows.Window
	{
		private bool _yes = true;

		public bool Yes
		{
			get { return _yes; }
			set { _yes = value; }
		}

		public QuestionWindow()
		{
			InitializeComponent();
		}

		public string Msg
		{
			get
			{
				return _msg.Text as string;
			}
			set
			{
				_msg.Text = value;
			}
		}

		public string YesButtonLabel
		{
			get
			{
				return _yesButton.Content as string;
			}
			set
			{
				_yesButton.Content = value;
			}
		}

		public string NoButtonLabel
		{
			get
			{
				return _noButton.Content as string;
			}
			set
			{
				_noButton.Content = value;
			}
		}

		public void OnYes(Object sender, RoutedEventArgs args)
		{
			_yes = true;
			Close();
		}

		public void OnNo(Object sender, RoutedEventArgs args)
		{
			_yes = false;
			Close();
		}
	}
}