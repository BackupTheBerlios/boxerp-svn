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

	public partial class WaitDialog : System.Windows.Window
	{
		private EventHandler _cancelEventHandler;

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
	}
}