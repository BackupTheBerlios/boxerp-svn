
using System;
using Gtk;

namespace lib
{
	
	public class WaitDialog : Gtk.Dialog
	{
		protected Gtk.ProgressBar progressbar;
		protected bool nonstop = true;
		protected bool firstInstant = true;
		protected Gtk.Label labelMsg;
		protected Gtk.Button button;
		protected Gtk.HButtonBox actionArea;
		
		public string Message
		{
			get { return labelMsg.Text;}
			set { labelMsg.Text = value;}
		}

		public WaitDialog(Gtk.Window parent)
		{
			Stetic.Gui.Build(this, typeof(lib.WaitDialog));
			this.Modal = true;
			this.TransientFor = parent;
			this.Hide();
			actionArea.Remove(button);
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
		

	}
	
}
