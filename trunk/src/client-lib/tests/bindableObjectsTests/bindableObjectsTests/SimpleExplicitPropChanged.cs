using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace bindableObjectsTests
{
	public class SimpleExplicitPropChanged : INotifyPropertyChanged
	{
		private string _name;

		public string Name
		{
			get { return _name; }
			set 
			{ 
				_name = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("Name"));
				}
			}
		}
		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}
