#if WPF

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
	/// Interaction logic for Window1.xaml
	/// </summary>

	public partial class Window1 : System.Windows.Window, ITestWindow
	{
		private TestController _controller;

		public Window1()
		{
			InitializeComponent();
			_controller = new TestController(new WpfResponsiveHelper(ConcurrencyMode.Modal), this);
		}

		public void OnClicked(Object sender, RoutedEventArgs args)
		{
			_controller.RunMethod();
		}

		public void OnClickedWithCancellingLogic(Object sender, RoutedEventArgs args)
		{
			_controller.RunMethodWithCancellationLogic();
		}

		#region ITestWindow Members

		public void ShowSomething()
		{
			MessageBox.Show("Done!");
		}

		#endregion
	}
}

#endif