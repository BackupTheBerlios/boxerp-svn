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
    /// Interaction logic for DateControl.xaml
    /// </summary>

    public partial class DateControl : System.Windows.Controls.UserControl
    {

        private const int YEARS_OFFSET_BIRTHDAY = 20;
		private const int YEARS_OFFSET_CURRENTLY = 10;
		private const int DAYS_MONTH = 31;
		private const int MONTHS_YEAR = 12;
		private bool _populatingCombo = false;
		private bool _innerSetDateProperty = false;

		public event SelectionChangedEventHandler DateChanged;

		public bool InnerSetDateProperty
		{
			get { return _innerSetDateProperty; }
			internal set { _innerSetDateProperty = value; }
		}

        public DateControl()
        {
			_populatingCombo = true;
            InitializeComponent();
            PopulateYears();
			_populatingCombo = false;
		}

		public ComboBox Days
		{
			get
			{
				return _days;
			}
		}

		public ComboBox Months
		{
			get
			{
				return _months;
			}
		}

		public ComboBox Years
		{
			get
			{
				return _years;
			}
		}

		#region XAML binding properties and methods

		public static DependencyProperty IsDateEnabledProperty = DependencyProperty.Register(
            "IsDateEnabled",
            typeof(bool),
            typeof(DateControl),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender,
                                        new PropertyChangedCallback(OnEnabledChanged), null));

        private static void OnEnabledChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DateControl control = (DateControl)o;
            if ((bool)e.NewValue == true)
            {
                control.enable();
            }
            else
            {
                control.disable();
            }
        }

		public bool IsDateEnabled
        {
            get 
            { 
                return (bool)GetValue(IsDateEnabledProperty); 
            }
            set
            {
                SetValue(IsDateEnabledProperty, value);
            }
		}

		public static DependencyProperty DateProperty = DependencyProperty.Register(
			"Date",
			typeof(DateTime?),
			typeof(DateControl),
			new FrameworkPropertyMetadata(DateTime.MinValue, FrameworkPropertyMetadataOptions.AffectsRender,
				new PropertyChangedCallback(OnDateChanged), null));

		private static void OnDateChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
		{
			DateControl control = (DateControl)o;
			if ((e.NewValue != null) && (!control.InnerSetDateProperty))
			{
				control.PopulateGui((DateTime)e.NewValue);
			}
		}

		public DateTime? Date
		{
			get
			{
				return (DateTime)GetValue(DateProperty);
			}
			set
			{
				SetValue(DateProperty, value);
			}
		}

		#endregion

		public void OnMonthDayChanged(Object sender, SelectionChangedEventArgs args)
		{
			try
			{
				if (!_populatingCombo)
				{
					lock (this)
					{
						_innerSetDateProperty = true;
						SetValue(DateProperty, GetDate());
						_innerSetDateProperty = false;
					}
				}
				if (DateChanged != null)
				{
					DateChanged.Invoke(this, null);
				}
			}
			catch (Exception)
			{
				MessageBox.Show("The selected date is invalid please select another date");
				_months.SelectedIndex = 0;
				_days.SelectedIndex = 0;
			}
		}

		

		public void PopulateGui(DateTime date)
        {
			if ((date != DateTime.MinValue) && (date != null))
			{
				_populatingCombo = true;
				// I wonder why ItemsSource and Items properties are null because I've defined the ItemsSource in XAML!!!
				// I can't loop through them programmatically !!!

				// This is better: As they are already populated from XAML, if you don't invoke Clear it blows up on runtime 
				// when you try to add the items in order to go through them and get one selected !!! 
				_days.ItemsSource = null;
				_days.Items.Clear();
				_months.ItemsSource = null;
				_months.Items.Clear();


				for (int i = 1; i <= DAYS_MONTH; i++)
				{
					_days.Items.Add(i);
					if (i == date.Day)
					{
						_days.SelectedItem = i;
					}
				}

				for (int i = 1; i <= MONTHS_YEAR; i++)
				{
					_months.Items.Add(i);
					if (i == date.Month)
					{
						_months.SelectedItem = i;
					}
				}

				_years.Items.Clear();
				for (int y = date.Year - YEARS_OFFSET_BIRTHDAY; y < date.Year + YEARS_OFFSET_BIRTHDAY; y++)
				{
					_years.Items.Add(y);
					if (y == date.Year)
					{
						_years.SelectedItem = y;
					}
				}
				if (_years.SelectedItem == null)
				{
					_years.SelectedIndex = 0;
				}

				_populatingCombo = false;
			}
			else
			{
				PopulateYears();
			}
        }

        public void PopulateYears()
        {
			_populatingCombo = true;

            for (int y = DateTime.Now.Year; y < DateTime.Now.Year + YEARS_OFFSET_CURRENTLY; y++)
            {
                _years.Items.Add(y);
            }
			_years.SelectedIndex = 0;

			_populatingCombo = false;
        }

		public void PopulateYearsBirthDate()
		{
			_populatingCombo = true;

			_years.Items.Clear();
			for (int y = 1900; y < DateTime.Now.Year - 16; y++)
			{
				_years.Items.Add(y);
			}
			_years.SelectedIndex = 0;

			_populatingCombo = false;
		}

		/// <summary>
		///  The controls could have been populated programmatically or by XAML (default) so a convertion may be needed.
		///  We might avoid the convertion by using Binding.ConvertParameter in XAML and writing a XMLConverter class
		/// </summary>
		/// <returns></returns>
        public DateTime GetDate()
        {
			if ((_days == null) || (_days.SelectedItem == null) || (_days.Items.Count == 0) || 
				(_months == null) || (_months.SelectedItem == null) || (_months.Items.Count == 0) ||
				(_years == null) || (_years.SelectedItem == null) || (_years.Items.Count == 0))	// fix wpf issue 
			{
				return DateTime.Now;
			}

			int day, month;
			if (_days.SelectedItem is System.Xml.XmlLinkedNode)
			{
				day = Convert.ToInt32(((System.Xml.XmlLinkedNode)_days.SelectedItem).InnerText);
			}
			else
			{
				day = (int)_days.SelectedItem;
			}
			if (_months.SelectedItem is System.Xml.XmlLinkedNode)
			{
				month = Convert.ToInt32(((System.Xml.XmlLinkedNode)_months.SelectedItem).InnerText);
			}
			else
			{
				month = (int)_months.SelectedItem;
			}
			
            DateTime date = new DateTime((int)_years.SelectedItem, month, day);
            return date;
        }

        private void enable()
        {
            _days.IsEnabled = true;
            _months.IsEnabled = true;
            _years.IsEnabled = true;
        }

        private void disable()
        {
            _days.IsEnabled = false;
            _months.IsEnabled = false;
            _years.IsEnabled = false;
        }
    }
}