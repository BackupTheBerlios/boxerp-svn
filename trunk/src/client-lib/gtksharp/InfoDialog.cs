
using System;

namespace Boxerp.Client.GtkSharp.Lib
{
	
	public class InfoDialog : Gtk.Dialog
	{
		protected Gtk.Label label;

		public string Message 
		{
			get { return label.Text; }
			set { label.Text = value; } 
		}
		
		public InfoDialog()
		{
			Stetic.Gui.Build(this, typeof(Boxerp.Client.GtkSharp.Lib.InfoDialog));
		}

		protected virtual void OnOkClicked(object sender, System.EventArgs e)
		{
		    this.Destroy();
		}
	}
	
}
