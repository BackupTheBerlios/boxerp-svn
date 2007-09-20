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
using Boxerp.Client;
using Boxerp.Client.WPF;
using Boxerp.Client.WPF.Controls;

using System.ComponentModel;

namespace testApp2
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>

	public partial class Window1 : System.Windows.Window
	{
		private BindableWrapper<SampleBObj> _bindableSampleObj;
		private Proxy2 _proxy2;
		bool _useRealProxy = false;

		public Window1()
		{
			InitializeComponent();

			try
			{
				SampleBObj businessObject = new SampleBObj();
				businessObject.Name = "test";
				businessObject.Description = "description";
				businessObject.Age = 10;

				_bindableSampleObj = new BindableWrapper<SampleBObj>(businessObject);

				_proxy2 = new Proxy2();
				_proxy2.Name = "test";

				if (_useRealProxy)
				{
					DataContext = _proxy2;
				}
				else
				{
					DataContext = _bindableSampleObj.Data.BusinessObj;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + ex.StackTrace);
			}
		}

		public void OnUndo(Object sender, RoutedEventArgs args)
		{
			_bindableSampleObj.Undo();
		}

		public void OnRedo(Object sender, RoutedEventArgs args)
		{
			_bindableSampleObj.Redo();
		}

		private void OnPropertyChanged(Object sender, PropertyChangedEventArgs args)
		{
			Console.WriteLine("Property changed:" + args.PropertyName);
		}

		public void OnChangeData(Object sender, RoutedEventArgs args)
		{
			if (_useRealProxy)
			{
				_proxy2.Name = "change";
				_proxy2.Description = "asdf";
				_proxy2.Age = 111;
			}
			else
			{
				_bindableSampleObj.Data.BusinessObj.Name = "whatever";
				_bindableSampleObj.Data.BusinessObj.Description = "whatever123";
				_bindableSampleObj.Data.BusinessObj.Age = 1000;
			}
		}

		public void OnReadData(Object sender, RoutedEventArgs args)
		{
			if (_useRealProxy)
			{
				MessageBox.Show(_proxy2.Name + _proxy2.Description + _proxy2.Age);
			}
			else
			{
				MessageBox.Show(_bindableSampleObj.Data.BusinessObj.Name + ", " +
							_bindableSampleObj.Data.BusinessObj.Description + ", " +
							_bindableSampleObj.Data.BusinessObj.Age);
			}
		}
	}
}