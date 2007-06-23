// /home/carlos/boxerp_completo/trunk/src/client-lib/gtksharp/WaitDialo.cs created with MonoDevelop
// User: carlos at 1:22 PM 6/23/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Gtk;

namespace Boxerp.Client.GtkSharp
{
	
	
	public partial class WaitDialog : Gtk.Dialog
	{
		protected bool nonstop = true;
		protected bool firstInstant = true;
		private EventHandler cancelEventHandler;

		public WaitDialog()
		{
			this.Build();
			this.Modal = true;
			//this.TransientFor = parent;
			this.Hide();
			progressbar.BarStyle = Gtk.ProgressBarStyle.Continuous;
			progressbar.PulseStep = 0.05;
			GLib.Timeout.Add (300, new GLib.TimeoutHandler (FirstInstant));
			GLib.Timeout.Add (100, new GLib.TimeoutHandler (DoPulse));
		}
		
		
        public event EventHandler CancelEvent
        {
            add
            {
                cancelEventHandler += value;
            }
            remove
            {
                cancelEventHandler -= value;
            }
        }

		public string Message
		{
			get { return labelMsg.Text;}
			set { labelMsg.Text = value;}
		}

		public void Stop()
		{
			nonstop = false;
		}
		
		public void Start()
		{
			nonstop = true;
		}
		
		public bool FirstInstant()
		{
			if (nonstop) this.Present();
			firstInstant = false;
			return false;
		}
		
		public bool DoPulse()
		{
			if (!firstInstant)
				progressbar.Pulse();
			return nonstop;
		}

		protected virtual void OnCancel(object sender, System.EventArgs e)
		{
			if (cancelEventHandler != null)
				cancelEventHandler(this, null);
        }

		protected virtual void OnClose(object sender, System.EventArgs e)
		{
		    Console.WriteLine("ON CLOSE!!!!!!");
		    OnCancel(sender, e);
		}

		protected virtual void OnDeleteEvent(object o, Gtk.DeleteEventArgs args)
		{
		    if (o == this)
		    {
	            OnCancel(o, null);
		    }
		}
	}
}
