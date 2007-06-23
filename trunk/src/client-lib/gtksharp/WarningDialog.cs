// /home/carlos/boxerp_completo/trunk/src/client-lib/gtksharp/WarningDia.cs created with MonoDevelop
// User: carlos at 6:36 PM 6/23/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Boxerp.Client.GtkSharp
{
	
	
	public partial class WarningDialog : Gtk.Dialog
	{
		protected bool quitOnOk = false;
		
		public WarningDialog()
		{
			this.Build();
		}
		
		public string Message 
		{
			get { return label.Text; }
			set { label.Text = value; } 
		}
		
		public bool QuitOnOk
		{
			get { return quitOnOk; }
			set { quitOnOk = value; }
		}
		
		protected virtual void OnOk(object sender, System.EventArgs e)
		{
			this.Destroy();
			if (quitOnOk)
				Gtk.Application.Quit();
		}

	}
}
