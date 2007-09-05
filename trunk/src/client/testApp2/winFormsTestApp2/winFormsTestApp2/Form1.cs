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
		private BindableWrapper<SampleBObj> _bindable;

		public Form1()
		{
			InitializeComponent();
			SampleBObj businessObject = new SampleBObj();
			businessObject.Name = "jwormy";
			businessObject.Description = "is a cool developer";
			businessObject.Age = 25;

			_bindable = new BindableWrapper<SampleBObj>(businessObject, false);
			_bindable.PropertyChanged += OnPropertyChanged;

			// bind the object
			
			_name.DataBindings.Add("Text", _bindable.Data.BusinessObj, "Name", false, DataSourceUpdateMode.OnPropertyChanged);
			_description.DataBindings.Add("Text", _bindable.Data.BusinessObj, "Description", false, DataSourceUpdateMode.OnPropertyChanged);
			_age.DataBindings.Add("Text", _bindable.Data.BusinessObj, "Age", false, DataSourceUpdateMode.OnPropertyChanged);

			// In the Gtk# I've implented a class so that you can call dataBinder.Bind(this, _bindable)
			// and it does the same than the 3 lines above. It needs an xml with binding information.
			// It could be nice to have this also for windows forms, don't you think mate?
		}

		private void OnUndoClicked(object sender, EventArgs e)
		{
			_bindable.Undo();
			
			// I'd like not to need to call refresh explicitly. My understanding of the 2way binding is that
			// a change in the data source should update the UI.
			refreshBindings();

			// I think I know why it happens: the _bindable is the one that has a PropertyChanged event, 
			// but the bound object is not the _bindable but the _bindable.Data.BusinesObj.
		}

		/// <summary>
		/// I have to work to avoid writing methods like this
		/// </summary>
		public void refreshBindings()
		{
			_name.DataBindings.Clear();
			_name.DataBindings.Add("Text", _bindable.Data.BusinessObj, "Name", false, DataSourceUpdateMode.OnPropertyChanged);

			_description.DataBindings.Clear();
			_description.DataBindings.Add("Text", _bindable.Data.BusinessObj, "Description", false, DataSourceUpdateMode.OnPropertyChanged);

			_age.DataBindings.Clear();
			_age.DataBindings.Add("Text", _bindable.Data.BusinessObj, "Age", false, DataSourceUpdateMode.OnPropertyChanged);

		}

		public void OnPropertyChanged(Object sender, PropertyChangedEventArgs args)
		{
			Console.WriteLine("Property changed: " + args.PropertyName);
			refreshBindings();
		}

		private void OnRedoClicked(object sender, EventArgs e)
		{
			_bindable.Redo();
			refreshBindings();
		}

		private void OnChangeData(object sender, EventArgs e)
		{
			// the UI is bound so it should change automaticly
			_bindable.Data.BusinessObj.Name = "Paco";
			_bindable.Data.BusinessObj.Description = "Es un tio de puta madre";
			_bindable.Data.BusinessObj.Age = 50;

		}

		private void OnReadData(object sender, EventArgs e)
		{
			// the bindable is bound so it should contain the same thing than the UI
			MessageBox.Show(_bindable.Data.BusinessObj.Name + ", " +
							_bindable.Data.BusinessObj.Description + ", " +
							_bindable.Data.BusinessObj.Age);
		}


	}
}