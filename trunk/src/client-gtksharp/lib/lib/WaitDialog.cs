
using System;
using Gtk;

namespace Boxerp.Client.GtkSharp.Lib
{
	
	public class WaitDialog : global::Gtk.Dialog
	{
		protected Gtk.ProgressBar progressbar;
		protected bool nonstop = true;
		protected bool firstInstant = true;
		protected Gtk.Label labelMsg;
		protected Gtk.Button button;
		protected Gtk.HButtonBox actionArea;
		private EventHandler cancelEventHandler;

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

		public WaitDialog(Gtk.Window parent)
		{
			Stetic.Gui.Build(this, typeof(Boxerp.Client.GtkSharp.Lib.WaitDialog));
			this.Modal = true;
			this.TransientFor = parent;
			this.Hide();
			//actionArea.Remove(button);
			progressbar.BarStyle = Gtk.ProgressBarStyle.Continuous;
			progressbar.PulseStep = 0.05;
			GLib.Timeout.Add (300, new GLib.TimeoutHandler (FirstInstant));
			GLib.Timeout.Add (100, new GLib.TimeoutHandler (DoPulse));
		}
		
		public WaitDialog()
		{
			Stetic.Gui.Build(this, typeof(Boxerp.Client.GtkSharp.Lib.WaitDialog));
			this.Modal = true;
			this.Hide();
			//actionArea.Remove(button);
			progressbar.BarStyle = Gtk.ProgressBarStyle.Continuous;
			progressbar.PulseStep = 0.05;
			GLib.Timeout.Add (300, new GLib.TimeoutHandler (FirstInstant));
			GLib.Timeout.Add (100, new GLib.TimeoutHandler (DoPulse));
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
                //cancelEventHandler.Invoke(this, null);
		}

		protected virtual void OnClose(object sender, System.EventArgs e)
		{
		    Console.WriteLine("ON CLOSE!!!!!!");
		    OnCancel(sender, e);
		}

		protected virtual void OnDeleteEvent(object o, Gtk.DeleteEventArgs args)
		{
		    /*Console.WriteLine("delte event:" + o.ToString()+","+ args.Event.Type.ToString());
		    if (args.Event.SendEvent)
		    {
		        Console.WriteLine("send event!!:" + args.Event.Type.ToString());
		    }*/
		    if (o == this)
		    {
	            OnCancel(o, null);
		    }
		}
		

	}
	
}
