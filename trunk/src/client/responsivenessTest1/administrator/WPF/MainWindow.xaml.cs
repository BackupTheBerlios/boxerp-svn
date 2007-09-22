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

namespace Admin
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>

	public partial class MainWindow : System.Windows.Window
	{

		public MainWindow()
		{
			InitializeComponent();
		}

		public void OnClicked(Object sender, RoutedEventArgs args)
		{
			if (sender == _button1)
			{
				Window1 win = new Window1();
				win.Show();
			}
			else if (sender == _button2)
			{
				Window2 win = new Window2();
				win.Show();
			}
			else
			{
				Window3 win = new Window3();
				win.Show();
			}
		}
	}
}