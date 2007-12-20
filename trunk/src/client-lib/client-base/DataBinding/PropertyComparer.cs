using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Boxerp.Client
{
	/// <summary>
	/// It compares the ToString() object of the given object property.
	/// It is used in the BindableCollection.
	/// </summary>
	/// <param name="T"></param>
	/// 
	public class PropertyComparer<T> : IComparer<T>
	{
		protected PropertyDescriptor _propDescriptor;
		protected ListSortDirection _sortDirection;

		public PropertyDescriptor PropDescriptor
		{
			get { return _propDescriptor; }
			set { _propDescriptor = value; }
		}
		
		public ListSortDirection SortDirection
		{
			get { return _sortDirection; }
			set { _sortDirection = value; }
		}

		public PropertyComparer(PropertyDescriptor descriptor, ListSortDirection direction)
		{
			_propDescriptor = descriptor;
			_sortDirection = direction;
		}


		#region IComparer<T> Members

		public virtual int Compare(T x, T y)
		{
			object valueX = _propDescriptor.GetValue(x);
			object valueY = _propDescriptor.GetValue(y);
			if (_sortDirection == ListSortDirection.Ascending)
			{
				return (valueX.ToString().CompareTo(valueY.ToString()));
			}
			else
			{
				return  -(valueX.ToString().CompareTo(valueY.ToString()));
			}
		}
		#endregion
	}
}
