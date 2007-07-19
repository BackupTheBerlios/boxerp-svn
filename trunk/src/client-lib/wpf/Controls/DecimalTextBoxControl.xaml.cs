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

		public char DecimalSeparator
		{
			get { return _decimalSeparator; }
			set { _decimalSeparator = value; }
		}

		public int DecimalDigits
		{
			get { return _decimalDigits; }
			set { _decimalDigits = value; }
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
				bool readingDecimals = false;
				int decimals = 0;
				foreach (char c in currentStr)
				{
					if ((readingDecimals) && (decimals == DecimalDigits))
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
				if ((!Char.IsNumber(character)) && (character != DecimalSeparator))
				{
					MessageBox.Show("Error: Only numbers and decimal separator are allowed in this box");
					_textBox.Text = CleanString();
				}
			}
		}
	}
}