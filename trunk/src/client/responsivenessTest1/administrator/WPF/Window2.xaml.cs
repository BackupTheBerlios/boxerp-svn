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

using Admin.Interfaces;
using Admin.Controllers;
using Boxerp.Client;
using Boxerp.Client.WPF;

namespace Admin
{
	/// <summary>
	/// Interaction logic for Window2.xaml
	/// </summary>

	public partial class Window2 : System.Windows.Window, ITestWindow
	{
		private TestController _controller;

		public Window2()
		{
			InitializeComponent();
			_controller = new TestController(new WpfResponsiveHelper(ConcurrencyMode.SingletonThread), this);
		}

		public void OnClicked(Object sender, RoutedEventArgs args)
		{
			_controller.RunMethod();
		}

		#region ITestWindow Members

		public void ShowSomething()
		{
			MessageBox.Show("Done!");
		}

		#endregion

	}
}