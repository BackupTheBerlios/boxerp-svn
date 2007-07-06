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
