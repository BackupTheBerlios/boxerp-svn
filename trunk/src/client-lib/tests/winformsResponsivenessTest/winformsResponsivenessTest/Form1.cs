using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Boxerp.Client;
using Boxerp.Client.WindowsForms;

namespace winformsResponsivenessTest
{
	public partial class Form1 : Form, ISampleView
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

		public Form1()
		{
			InitializeComponent();
			_modalController = new Controller(new WinFormsResponsiveHelper(ConcurrencyMode.Modal), this);
			_singletonController = new Controller(new WinFormsResponsiveHelper(ConcurrencyMode.SingletonThread), this);
			_parallelController = new Controller(new WinFormsResponsiveHelper(ConcurrencyMode.Parallel), this);
		}

		private void stressTheEngine()
		{
			Random random = new Random();
			for (int i = 0; i < 100; i++)
			{
				_modalController.DoAsyncOperation();
				_singletonController.DoAsyncOperation();
				_parallelController.DoAsyncOperation();
				System.Threading.Thread.Sleep(random.Next(3));
				Console.Out.WriteLine("Iteration:" + i);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			stressTheEngine();
		}
	}
}