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
	/// Interaction logic for IntegerTextBoxControl.xaml
	/// </summary>

	public partial class IntegerTextBoxControl : System.Windows.Controls.UserControl
	{
		public IntegerTextBoxControl()
		{
			InitializeComponent();
		}

		public string Text
		{
			get
			{
				return _textBox.Text;
			}
			set
			{
				_textBox.Text = CleanString(value);
			}
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
				foreach (char c in currentStr)
				{
					if (Char.IsNumber(c))
					{
						cleaned += c.ToString();
					}
				}
			}
			return cleaned;
		}

		private void OnKeyUp(Object sender, KeyEventArgs args)
		{
			if ((args.Key != Key.Tab) && (args.Key != Key.Delete) &&
				(args.Key != Key.Left) && (args.Key != Key.Right) &&
				(args.Key != Key.Enter) && (args.Key != Key.End) &&
				(args.Key != Key.Home) && (args.Key != Key.Clear) &&
				(args.Key != Key.Back))
			{
				string key = args.Key.ToString();
				char character = key[key.Length - 1];
				if (!Char.IsNumber(character))
				{
					MessageBox.Show("Error: Only numbers are allowed in this box");
					_textBox.Text = CleanString();
				}
			}
		}


	}
}