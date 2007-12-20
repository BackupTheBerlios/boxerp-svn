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
	/// Dialog with two buttons (Accept, Cancel). Although it would be possible to use a MessageBox,
	/// the ResponsiveHelper hierarchy need an instance of an IQuestionWindow for a generic implementation
	/// </summary>
	public partial class QuestionWindow : System.Windows.Window, IQuestionWindow
	{
		private bool _yes = true;

		public bool IsAfirmative
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

		public string AfirmativeOption
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

		public string NegativeOption
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

		

		void IQuestionWindow.ShowDialog()
		{
			base.ShowDialog();
		}

		
	}
}