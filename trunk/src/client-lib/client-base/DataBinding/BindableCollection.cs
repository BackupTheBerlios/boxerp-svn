using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

using Boxerp.Collections;

// Documentation: 
// Michael Weinhardt - http://msdn2.microsoft.com/en-us/library/ms993236.aspx
//

namespace Boxerp.Client
{
	public class BindableCollection<T> : BindingList<T> 
	{
		private bool _isSorted = false;
		private PropertyComparer<T> _comparer = null;

		public BindableCollection(){}

		public BindableCollection(PropertyComparer<T> comparer)
		{
			_comparer = comparer;
		}

		/// <summary>
		///  If the object we are inserting implements the INotifyPropertyChanged interface, then a change in the object should reset the 
		///  item presentation in the UI.
		/// </summary>
		/// <param name="item"></param>
		public new void Add(T item)
		{
			if (item is INotifyPropertyChanged)
			{
				INotifyPropertyChanged inotifyable = (INotifyPropertyChanged)item;
				inotifyable.PropertyChanged += OnItemChanged;
			}

			base.Add(item);
		}

		private void OnItemChanged(Object sender, PropertyChangedEventArgs args)
		{
			if (sender is T)
			{
				ResetItem(Items.IndexOf((T)sender));
			}
		}

		protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
		{
			List<T> items = this.Items as List<T>;

			if (items != null)
			{
				if (_comparer != null)
				{
					_comparer.PropDescriptor = prop;
					_comparer.SortDirection = direction;
				}
				else
				{
					_comparer = new PropertyComparer<T>(prop, direction);
				}

				items.Sort(_comparer);
				_isSorted = true;
			}

			this.OnListChanged(
			  new ListChangedEventArgs(ListChangedType.Reset, -1));
		}

		public void Sort(string propertyName, ListSortDirection direction)
		{
			PropertyDescriptor descriptor = TypeDescriptor.GetProperties(typeof(T))[propertyName];
			if (descriptor != null)
			{
				ApplySortCore(descriptor, direction);
			}
		}

		protected override bool  SupportsSortingCore
		{
			get { return true; }
		}

		protected override bool IsSortedCore
		{
			get
			{
				return _isSorted;
			}
		}

		protected override ListSortDirection SortDirectionCore
		{
			get
			{
				return _comparer.SortDirection;
			}
		}

		protected override PropertyDescriptor SortPropertyCore
		{
			get
			{
				return _comparer.PropDescriptor;
			}
		}

		
	}
}
