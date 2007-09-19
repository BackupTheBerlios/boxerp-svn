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

		public Window1()
		{
			InitializeComponent();

			SampleBObj businessObject = new SampleBObj();
			businessObject.Name = "test";
			businessObject.Description = "description";
			businessObject.Age = 10;

			_bindableSampleObj = new BindableWrapper<SampleBObj>(businessObject);
			//_bindableSampleObj.PropertyChanged += OnPropertyChanged;

			DataContext = _bindableSampleObj.Data.BusinessObj;
		}

		public void OnUndo(Object sender, RoutedEventArgs args)
		{
			_bindableSampleObj.Undo();
			//refreshDataContext();
		}

		//private void refreshDataContext()
		//{
			//DataContext = null;
			//DataContext = _bindableSampleObj.Data.BusinessObj;
			//this.UpdateLayout();
		//}/

		public void OnRedo(Object sender, RoutedEventArgs args)
		{
			_bindableSampleObj.Redo();
			//refreshDataContext();
		}

		private void OnPropertyChanged(Object sender, PropertyChangedEventArgs args)
		{
			Console.WriteLine("Property changed:" + args.PropertyName);
			//refreshDataContext();
		}

		public void OnChangeData(Object sender, RoutedEventArgs args)
		{
			_bindableSampleObj.Data.BusinessObj.Name = "whatever";
			_bindableSampleObj.Data.BusinessObj.Description = "whatever123";
			_bindableSampleObj.Data.BusinessObj.Age = 1000;
		}

		public void OnReadData(Object sender, RoutedEventArgs args)
		{
			MessageBox.Show(_bindableSampleObj.Data.BusinessObj.Name + ", " +
							_bindableSampleObj.Data.BusinessObj.Description + ", " +
							_bindableSampleObj.Data.BusinessObj.Age);
		}
	}
}