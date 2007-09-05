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
			// I've noticed that there is a bug in the Undo feature, well, not in that but in the procedure
			// that keeps track of the changes, because the same change is saved several times in the 
			// changesStack. So please do some changes in the textboxes and click Undo several times to see any effects.
			_bindable.Undo();
			MessageBox.Show("The name is: " + _bindable.Data.BusinessObj.Name);

			// why is the textbox not updated?. The object is suppously bound! . I think it could be a bug in the bindableWrapper architecture.
			// leave this to me.

		}

		public void OnPropertyChanged(Object sender, PropertyChangedEventArgs args)
		{
			Console.WriteLine("This is just to check that the bindableWrapper works");
		}

		private void OnRedoClicked(object sender, EventArgs e)
		{
			_bindable.Redo();
			MessageBox.Show("The name is: " + _bindable.Data.BusinessObj.Name);
		}

		private void OnChangeData(object sender, EventArgs e)
		{
			// the UI is bound so it should change automaticly
			_bindable.Data.BusinessObj.Name = "Paco";
			_bindable.Data.BusinessObj.Description = "Es un tio de puta madre";
			_bindable.Data.BusinessObj.Age = 50;

			// This is a single change, not an undo operation. This is for you
			// Question for you: Why the UI doesn't refresh untill we explictly change the Text property?
			// if you remove this line it doesn't refresh unless you write something in one of the textboxes
			_name.Text = "Paco";
			
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