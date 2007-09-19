using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Boxerp.Client;
using Boxerp.Client.WindowsForms;


namespace winFormsTestApp2
{

	public partial class Form1 : Form
	{
		Proxy2 _businessObjProxy2;

		public Form1()
		{
			InitializeComponent();

			_businessObjProxy2 = new Proxy2();
			_businessObjProxy2.Name = "test";

	
			_name.DataBindings.Add("Text", _businessObjProxy2, "Name", false, DataSourceUpdateMode.OnPropertyChanged);
			_description.DataBindings.Add("Text", _businessObjProxy2, "Description", false, DataSourceUpdateMode.OnPropertyChanged);
			_age.DataBindings.Add("Text", _businessObjProxy2, "Age", false, DataSourceUpdateMode.OnPropertyChanged);
			
		}

		public void OnPropertyChanged(Object sender, PropertyChangedEventArgs args)
		{
			Console.WriteLine("Property changed: " + args.PropertyName);
		}

		private void OnChangeData(object sender, EventArgs e)
		{
			_businessObjProxy2.Name = "name changed";
			_businessObjProxy2.Description = "desc changed";
			_businessObjProxy2.Age = 100;

		}

		private void OnReadData(object sender, EventArgs e)
		{
			MessageBox.Show(_businessObjProxy2.Name + ", " +
							_businessObjProxy2.Description + ", " +
							_businessObjProxy2.Age + ", " +
							"Has Subscribers: " + _businessObjProxy2.HasSubscribers().ToString());

		}

		private void OnShowForm2(object sender, EventArgs e)
		{
			Form2 f2 = new Form2();
			f2.Show();
		}


	}
}