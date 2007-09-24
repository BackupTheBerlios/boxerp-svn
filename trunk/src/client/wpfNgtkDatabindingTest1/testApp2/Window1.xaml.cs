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
		private BindableWrapper<SampleBObj> _bindable;
		private Proxy2 _proxy2;
		bool _useRealProxy = false;

		public Window1()
		{
			InitializeComponent();

			try
			{
				_proxy2 = new Proxy2();
				_proxy2.Name = "test";


				SampleBObj businessObject = new SampleBObj();
				businessObject.Name = "test";
				businessObject.Description = "description";
				businessObject.Age = 10;

				_bindable = new BindableWrapper<SampleBObj>(businessObject);
							
				if (_useRealProxy)
				{
					DataContext = _proxy2;
				}
				else // using dynamic proxy
				{
					DataContext = _bindable.Data.BusinessObj;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + ex.StackTrace);
			}
		}

		public void OnUndo(Object sender, RoutedEventArgs args)
		{
			_bindable.Undo();
		}

		public void OnRedo(Object sender, RoutedEventArgs args)
		{
			_bindable.Redo();
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
				_bindable.Data.BusinessObj.Name = "whatever";
				_bindable.Data.BusinessObj.Description = "whatever123";
				_bindable.Data.BusinessObj.Age = 1000;
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
				MessageBox.Show(_bindable.Data.BusinessObj.Name + ", " +
							_bindable.Data.BusinessObj.Description + ", " +
							_bindable.Data.BusinessObj.Age);
			}
		}
	}
}