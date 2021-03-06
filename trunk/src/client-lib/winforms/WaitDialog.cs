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
using System.Windows.Forms;

namespace Boxerp.Client.WindowsForms
{
    /// <summary>
    /// This is the winforms implementation of the waiting dialog while work is being done.
    /// </summary>
	public partial class WaitDialog : Form, IWinFormsWaitControl
	{
		private EventHandler cancelEventHandler;
        protected bool _isModal;
		private bool _isBeingDisplayed = false;
		private bool _isProgressDiscrete;
        private int _associatedThreadId = -1;

		public WaitDialog()
		{
			InitializeComponent();
		}


        public int AssociatedThreadId
        {
            get { return _associatedThreadId; }
            set { _associatedThreadId = value; }
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

        public void Run()
        {
			_isBeingDisplayed = true;
            if (_isModal)
            {
                this.ShowDialog();
            }
            else
            {
                this.Show();
            }
        }

		/// <summary>
        /// Fired when the form is closing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
	    private void WaitDialogForm_FormClosing(object sender, FormClosingEventArgs e)
	    {
            Console.WriteLine("Closing!!");
	    }

        protected virtual void OnCancel(object sender, System.EventArgs e)
        {
            if (cancelEventHandler != null)
            {
                cancelEventHandler(this, null);
            }
        }

        /// <summary>
        /// Fired when the form is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
	    private void WaitDialogForm_FormClosed(object sender, FormClosedEventArgs e)
	    {
	        Console.WriteLine("Closed!");
	    }

        private void WaitDialog_Load(object sender, EventArgs e)
        {

        }

		#region IWaitControl Members


		public void ShowControl()
		{
			Run();
		}

		public bool IsProgressDiscrete
		{
			get
			{
				return _isProgressDiscrete;
			}
			set
			{
				_isProgressDiscrete = value;
			}
		}

		public void UpdateProgress(int amount, int total)
		{
			if (!_isProgressDiscrete)
			{
				throw new Exception("Can not update the progress on a continuos status bar");
			}
		}

		public void UpdateStatus(string msg)
		{
			_infoLabel.Text = msg;
		}

		public void CloseControl()
		{
			Close();
			_isBeingDisplayed = false;
		}
		#endregion

		#region IWaitControl Members


		public bool IsBeingDisplayed
		{
			get
			{
				return _isBeingDisplayed;
			}
		}

		#endregion

		private void OnShown(object sender, EventArgs e)
		{
			_isBeingDisplayed = true;
		}
	}
}