// /home/carlos/boxerp_completo/trunk/src/client-lib/gtksharp/QuestionDialo.cs created with MonoDevelop
// User: carlos at 1:12 PM 6/23/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Boxerp.Client.GtkSharp
{
	
	
	public partial class QuestionDialog : Gtk.Dialog
	{
		private bool answer;
		
		public QuestionDialog()
		{
			this.Build();
		}
		
		public string Message 
		{
			get { return label.Text; }
			set { label.Text = value; } 
		}
		
		public bool Answer
		{
		    get { return answer; }
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
