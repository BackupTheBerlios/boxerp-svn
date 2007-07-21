// /home/carlos/boxerp_completo/trunk/src/client-lib/gtksharp/Controls/Controls/IntegerTextBoxControl.cs created with MonoDevelop
// User: carlos at 10:33 PM 7/21/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gdk;
using Boxerp.Client.GtkSharp;

namespace Boxerp.Client.GtkSharp.Controls
{
	
	
	public partial class IntegerTextBoxControl : Gtk.Bin
	{
		
		public IntegerTextBoxControl()
		{
			this.Build();
		}

		protected virtual void OnKeyReleased (object o, Gtk.KeyReleaseEventArgs args)
		{
			
			if ((args.Event.Key != Key.Tab) && (args.Event.Key != Key.Delete) &&
				(args.Event.Key != Key.Left) && (args.Event.Key != Key.Right) &&
				(args.Event.Key != Key.Return) && (args.Event.Key != Key.End) &&
				(args.Event.Key != Key.Home) && (args.Event.Key != Key.Clear) &&
				(args.Event.Key != Key.BackSpace))
			{
				string key = args.Event.Key.ToString();
			    char character = key[key.Length - 1];
				Console.WriteLine(character);
				if (!System.Char.IsNumber(character))
				{
					WarningDialog wdialog = new WarningDialog();
					wdialog.Message = "Error: Only numbers are allowed in this box";
					wdialog.Present();
					_textBox.Text = IntegerTextBoxHelper.CleanString(_textBox.Text);
				}
			}
		}
	}
}
