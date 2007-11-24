//
// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
//
// Copyright (C) 2005,2006 Shidix Technologies (www.shidix.com)
//
// Redistribution and use in source and binary forms, with or
// without modification, are permitted provided that the following
// conditions are met:
// Redistributions of source code must retain the above
// copyright notice, this list of conditions and the following
// disclaimer.
// Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following
// disclaimer in the documentation and/or other materials
// provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
// PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR
// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.
//
//

using System;
using Gtk;

namespace Boxerp.Client.GtkSharp
{
	
	
	public partial class WaitDialog : Gtk.Dialog, IGtkWaitControl
	{
		protected bool nonstop = true;
		protected bool firstInstant = true;
		private EventHandler cancelEventHandler;
		private int _assocThread;
		
		public int AssociatedThreadId
		{
			get
			{
				return _assocThread;
			}
			set
			{
				_assocThread = value;
			}
		}
		
		public bool IsModal 
		{
			get 
			{
				return true;
			}
			set 
			{
				
			}
		}

		public bool IsProgressDiscrete 
		{
			get 
			{
				return progressbar.BarStyle == Gtk.ProgressBarStyle.Continuous;
			}
			set 
			{
				
			}
		}

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

		public void ShowControl ()
		{
			Run();
		}

		public void CloseControl ()
		{
			//OnCancel(this, new EventArgs());
			Stop();
			Hide();
		}
		
		public void DestroyWidget()
		{
			Destroy();
		}

		public void UpdateProgress (int amount, int total)
		{
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
