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
using winformsResponsivenessTest;

namespace wpfResponsinessTest
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>

	public partial class Window1 : System.Windows.Window, ISampleView
	{
		private Controller _modalController;
		private Controller _singletonController;
		private Controller _parallelController;

		public Controller Controller
		{
			get
			{
				return _modalController;
			}
			set
			{
			}
		}

		public Window1()
		{
			InitializeComponent();
			_modalController = new Controller(new WpfResponsiveHelper(ConcurrencyMode.Modal), this);
			_singletonController = new Controller(new WpfResponsiveHelper(ConcurrencyMode.SingletonThread), this);
			_parallelController = new Controller(new WpfResponsiveHelper(ConcurrencyMode.Parallel), this);
		}

		private void stressTheEngine()
		{
			Random random = new Random();
			for (int i = 0; i < 100; i++)
			{
				_modalController.DoAsyncOperation(i * 100);
				_singletonController.DoAsyncOperation(i * 100);
				_parallelController.DoAsyncOperation(i * 100);
				System.Threading.Thread.Sleep(random.Next(3));
				Console.Out.WriteLine("Iteration:" + i);
			}
		}

		private void OnClick(object sender, RoutedEventArgs args)
		{
			stressTheEngine();
		}

		

	}
}