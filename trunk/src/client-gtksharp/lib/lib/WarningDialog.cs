
using System;

namespace lib
{
	
	public class WarningDialog : Gtk.Dialog
	{
		protected Gtk.Label label;
		protected bool quitOnOk = false;
		
		public WarningDialog()
		{
			Stetic.Gui.Build(this, typeof(lib.WarningDialog));
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
