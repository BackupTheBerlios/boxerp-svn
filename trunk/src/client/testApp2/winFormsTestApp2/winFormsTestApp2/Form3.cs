using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Boxerp.Client;

namespace winFormsTestApp2
{
	public partial class Form3 : Form
	{
		private CollectionBObj _bObj = new CollectionBObj();
		private BindableWrapper<CollectionBObj> _bindable;		
		public Form3()
		{
			InitializeComponent();

			_bObj.Title = "Test";
			
			SampleBObj child = new SampleBObj("test", "test", 5);
			_bObj.Collection.Add(child);
			child = new SampleBObj("test2", "test2", 15);
			_bObj.Collection.Add(child);
			child = new SampleBObj("test3", "test3", 46);
			_bObj.Collection.Add(child);

			_bindable = new BindableWrapper<CollectionBObj>(_bObj);

			_title.DataBindings.Add("Text", _bindable.Data.BusinessObjBinding, "Title", false, DataSourceUpdateMode.OnPropertyChanged);
			_collection.DataBindings.Add("Items", _bindable.Data.BusinessObjBinding, "Collection", false, DataSourceUpdateMode.OnPropertyChanged);
			
		}
	}
}