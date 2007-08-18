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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Boxerp.Client.WPF.Controls
{
	/// <summary>
	/// Interaction logic for DateTimeControl.xaml
	/// </summary>

	public partial class DateTimeControl : System.Windows.Controls.UserControl
	{
		public event SelectionChangedEventHandler DateTimeChanged;

		public DateTimeControl()
		{
			InitializeComponent();
			_dateControl.DateChanged += OnDateChanged;
		}

		

		private void OnDateChanged(Object sender, SelectionChangedEventArgs args)
		{
			if (DateTimeChanged != null)
			{
				DateTimeChanged.Invoke(this, null);
			}
		}

		public DateTime DateTime
		{
			get
			{
				DateTime dt = _dateControl.GetDate();
				return new DateTime(dt.Year, dt.Month, dt.Day, _hours.Integer.Value, _minutes.Integer.Value, 0);
			}
			set
			{
				_dateControl.PopulateGui(value);
				_hours.Integer = value.Hour;
				_minutes.Integer = value.Minute;
			}
		}
	}
}