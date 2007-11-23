using System;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Client
{
	public class ConsoleWaitDialog : IWaitControl
	{
		#region IWaitControl Members
		private EventHandler _cancelEventHandler;
		private bool _isModal;
		private int _assocThreadId;

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

		public void ShowControl()
		{
			Console.Out.WriteLine("Showing wait control");
		}

		public void CloseControl()
		{
			Console.Out.WriteLine("Closing wait control");
		}

		public void OnCancel(Object sender)
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

		public int AssociatedThreadId
		{
			get
			{
				return _assocThreadId;
			}
			set
			{
				_assocThreadId = value;
			}
		}

		public bool IsProgressDiscrete
		{
			get
			{
				return true;
			}
			set
			{
				
			}
		}

		public void UpdateProgress(int amount, int total)
		{
			
		}

		#endregion
	}
}
