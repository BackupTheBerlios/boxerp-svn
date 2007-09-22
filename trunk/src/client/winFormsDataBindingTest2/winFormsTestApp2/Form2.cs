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

	public partial class Form2 : Form
	{
		private BindableWrapper<SampleBObj> _bindable;

		public Form2()
		{
			InitializeComponent();
			SampleBObj businessObject = new SampleBObj();
			businessObject.Name = "test2";
			businessObject.Description = "test2";
			businessObject.Age = 44;


			_bindable = new BindableWrapper<SampleBObj>(businessObject);

			// bind the object

			_name.DataBindings.Add("Text", _bindable.Data.BusinessObjBinding, "Name", false, DataSourceUpdateMode.OnPropertyChanged);
			_description.DataBindings.Add("Text", _bindable.Data.BusinessObjBinding, "Description", false, DataSourceUpdateMode.OnPropertyChanged);
			_age.DataBindings.Add("Text", _bindable.Data.BusinessObjBinding, "Age", false, DataSourceUpdateMode.OnPropertyChanged);
		}

		private void OnUndo(object sender, EventArgs e)
		{
			_bindable.Undo();
		}

		public void OnPropertyChanged(Object sender, PropertyChangedEventArgs args)
		{
			Console.WriteLine("Property changed: " + args.PropertyName);
		}

		private void OnRedo(object sender, EventArgs e)
		{
			_bindable.Redo();
		}

		private void OnChangeData(object sender, EventArgs e)
		{
			// the UI is bound so it should change automaticly
			_bindable.Data.BusinessObj.Name = "name changed";
			_bindable.Data.BusinessObj.Description = "description changeg";
			_bindable.Data.BusinessObj.Age = 505;
		}

		private void OnReadData(object sender, EventArgs e)
		{
			//the bindable is bound so it should contain the same thing than the UI
			MessageBox.Show(_bindable.Data.BusinessObj.Name + ", " +
							_bindable.Data.BusinessObj.Description + ", " +
							_bindable.Data.BusinessObj.Age);
		}




	}
}