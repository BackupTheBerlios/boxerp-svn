//
// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Boxerp.Client.WPF
{
	/// <summary>
	/// Interaction logic for WaitDialog.xaml
	/// </summary>

	public partial class WaitDialog : System.Windows.Window, IWpfWaitControl
	{
		private EventHandler _cancelEventHandler;
		private bool _isProgressDiscrete;
		private bool _isModal;
		private bool _isBeingDisplayed = false;
		private int _associatedThreadId = -1;

		public int AssociatedThreadId
		{
			get { return _associatedThreadId; }
			set { _associatedThreadId = value; }
		}

		public event EventHandler CancelEvent
		{
			add
			{
				_cancelEventHandler += value;
			}
			remove
			{
				_cancelEventHandler -= value;
			}
		}

		public WaitDialog()
		{
			InitializeComponent();
		}

		public void Stop()
		{
					
		}

		public void Destroy()
		{
		
		}

		public void OnCancel(Object sender, RoutedEventArgs args)
		{
			_cancelEventHandler(this, null);
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
			_infoLabel.Content = msg;
		}

		public void ShowControl()
		{
			if (_isModal)
			{
				ShowDialog();
			}
			else
			{
				Show(); 
				WindowState = WindowState.Normal;
			}
			_isBeingDisplayed = true;
		}

		public void CloseControl()
		{
			Console.Out.WriteLine("window is closing:" + GetHashCode());
			Close();
			_isBeingDisplayed = false;
		}

		public bool IsBeingDisplayed
		{
			get
			{
				return _isBeingDisplayed;
			}
		}

		private void OnActivated(Object sender, EventArgs args)
		{
			_isBeingDisplayed = true;
		}
	}
}
