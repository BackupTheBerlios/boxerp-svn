// /home/carlos/boxerp_completo/trunk/src/client-lib/gtksharp/InfoDia.cs created with MonoDevelop
// User: carlos at 6:30 PM 6/23/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Boxerp.Client.GtkSharp
{
	
	
	public partial class InfoDialog : Gtk.Dialog
	{
		
		public InfoDialog()
		{
			this.Build();
		}
		
		public string Message 
		{
			get { return label.Text; }
			set { label.Text = value; } 
		}
	}
}
