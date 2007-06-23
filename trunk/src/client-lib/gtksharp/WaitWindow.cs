// /home/carlos/boxerp_completo/trunk/src/client-lib/gtksharp/WaitDialo.cs created with MonoDevelop
// User: carlos at 6:18 PM 6/23/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace Boxerp.Client.GtkSharp
{
	public partial class WaitWindow : Gtk.Window
	{
		protected bool nonstop = true;
		protected bool firstInstant = true;
		private EventHandler cancelEventHandler;
		private bool exitOnCancel = true;
		
		public WaitWindow() : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			
			this.Modal = true;
			this.Hide();
			progressbar.BarStyle = Gtk.ProgressBarStyle.Continuous;
			progressbar.PulseStep = 0.05;
			GLib.Timeout.Add (300, new GLib.TimeoutHandler (FirstInstant));
			GLib.Timeout.Add (100, new GLib.TimeoutHandler (DoPulse));
			exitOnCancel = true;

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
		
		public bool ExitOnCancel
		{
		    get { return exitOnCancel; }
		    set { exitOnCancel = value; }
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

		protected virtual void OnDeleteEvent(object o, Gtk.DeleteEventArgs args)
		{
		    if (o == this)
		    {   
		        Console.WriteLine("On delete event");
		        Stop();
		        if (exitOnCancel)
		        {
		            Console.WriteLine("exit on cancel");
		            args.RetVal = true;
		            Gtk.Application.Quit();
		        }
		        else
		        {
		            OnCancel(o, null);
		        }
		    }
		}

	}
}
