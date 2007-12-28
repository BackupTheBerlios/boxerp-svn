// ////
////// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
//////
////// Redistribution and use in source and binary forms, with or
////// without modification, are permitted provided that the following
////// conditions are met:
////// Redistributions of source code must retain the above
////// copyright notice, this list of conditions and the following
////// disclaimer.
////// Redistributions in binary form must reproduce the above
////// copyright notice, this list of conditions and the following
////// disclaimer in the documentation and/or other materials
////// provided with the distribution.
//////
////// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY
////// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
////// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
////// PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR
////// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
////// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
////// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
////// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
////// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
////// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
////// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
////// THE POSSIBILITY OF SUCH DAMAGE.
////
////

using System;
using Boxerp.Client;

namespace Boxerp.Client.GtkSharp
{
	public class WaitControl : IWaitControl
	{
		private IWaitControl _innerWait;
		private EventHandler cancelEventHandler;
		private bool _isModal;
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
				return _isModal;
			}
			set 
			{
				_isModal = value;
			}
		}

		public bool IsProgressDiscrete 
		{
			get 
			{
				return false;
			}
			set 
			{
				throw new NotImplementedException();
			}
		}

		public bool IsBeingDisplayed {
			get {
				throw new NotImplementedException();
			}
		}

		public WaitControl()
		{
		}

		public void ShowControl ()
		{
			if (_isModal)
			{
				_innerWait = new WaitDialog();
			}
			else
			{
				_innerWait = new WaitWindow();
			}
			_innerWait.CancelEvent += OnCancel;
			_innerWait.ShowControl();
		}
		
		public void UpdateProgress (int amount, int total)
		{
		}

		public void CloseControl ()
		{
			_innerWait.CloseControl();
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
		
		protected virtual void OnCancel(object sender, System.EventArgs e)
		{
			Logger.GetInstance().WriteLine("user press cancel");
			if (cancelEventHandler != null)
				cancelEventHandler(this, null);
        }

		public virtual void UpdateStatus (string msg)
		{}
	}
}
