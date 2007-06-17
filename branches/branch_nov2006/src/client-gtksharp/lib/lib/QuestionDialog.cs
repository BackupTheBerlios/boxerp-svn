
using System;

namespace Boxerp.Client.GtkSharp.Lib
{
	
	
	public class QuestionDialog : Gtk.Dialog
	{
		protected Gtk.Label label;
        private bool answer;
        
		public string Message 
		{
			get { return label.Text; }
			set { label.Text = value; } 
		}
		
		public bool Answer
		{
		    get { return answer; }
		}
		
		public QuestionDialog()
		{
			Stetic.Gui.Build(this, typeof(Boxerp.Client.GtkSharp.Lib.QuestionDialog));
		}

		protected virtual void OnNo(object sender, System.EventArgs e)
		{
		    answer = false;
		    this.Hide();
		}
		
		protected virtual void OnYes(object sender, System.EventArgs e)
		{
		    answer = true;
		    this.Hide();
		    
		}
	}
}
