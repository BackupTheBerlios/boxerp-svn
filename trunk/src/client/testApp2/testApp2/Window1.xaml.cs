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

namespace testApp2
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>

	public partial class Window1 : System.Windows.Window
	{
		private BindableWrapper<SampleBObj> _bindableSampleObj = new BindableWrapper<SampleBObj>(new SampleBObj());

		public Window1()
		{
			InitializeComponent();
		}

		private void refresh()
		{
			_name.Text = _bindableSampleObj.Data.BusinessObj.Name;
			_description.Text = _bindableSampleObj.Data.BusinessObj.Description;
			_age.Text = _bindableSampleObj.Data.BusinessObj.Age.ToString();
		}

		public void OnUndo(Object sender, RoutedEventArgs args)
		{
			_bindableSampleObj.Undo();
			refresh();
		}

		public void OnRedo(Object sender, RoutedEventArgs args)
		{
			_bindableSampleObj.Redo();
			refresh();
		}

		public void OnNameLostFocus(Object sender, RoutedEventArgs args)
		{
			_bindableSampleObj.Data.BusinessObj.Name = _name.Text;
		}

		public void OnDescriptionLostFocus(Object sender, RoutedEventArgs args)
		{
			_bindableSampleObj.Data.BusinessObj.Description = _description.Text;
		}

		public void OnAgeLostFocus(Object sender, RoutedEventArgs args)
		{
			if (_age.Text.Length > 0)
			{
				_bindableSampleObj.Data.BusinessObj.Age = Convert.ToInt32(_age.Text);
			}
		}
	}
}