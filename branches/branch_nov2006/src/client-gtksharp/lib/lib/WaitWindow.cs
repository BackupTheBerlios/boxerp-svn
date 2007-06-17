
using System;

namespace Boxerp.Client.GtkSharp.Lib
{
	
	public class WaitWindow : Gtk.Window
	{
		protected Gtk.ProgressBar progressbar;
		protected bool nonstop = true;
		protected bool firstInstant = true;
		protected Gtk.Label labelMsg;
		protected Gtk.Button button;
		protected Gtk.HButtonBox actionArea;
		private EventHandler cancelEventHandler;
		private bool exitOnCancel = true;

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

		public WaitWindow() : 
				base("")
		{
			Stetic.Gui.Build(this, typeof(Boxerp.Client.GtkSharp.Lib.WaitWindow));
			this.Modal = true;
			this.Hide();
			progressbar.BarStyle = Gtk.ProgressBarStyle.Continuous;
			progressbar.PulseStep = 0.05;
			GLib.Timeout.Add (300, new GLib.TimeoutHandler (FirstInstant));
			GLib.Timeout.Add (100, new GLib.TimeoutHandler (DoPulse));
			exitOnCancel = true;
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
