//
// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
//
// Redistribution and use in source and binary forms, with or
// without modification, are permitted provided that the following
// conditions are met:
// Redistributions of source code must retain the above
// copyright notice, this list of conditions and the following
// disclaimer.
// Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following
// disclaimer in the documentation and/or other materials
// provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
// PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR
// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;
// Documentation: 
// Michael Weinhardt - http://msdn2.microsoft.com/en-us/library/ms993236.aspx
//

namespace Boxerp.Client
{
	/// <summary>
	/// Provides a generic collection that supports databinding. This class implements the IBindingList.
	/// If you bind a BindableCollection to a list control and then add, remove or change any of its 
	/// elements from code, the UI will get refreshed.
	/// </summary>
	/// <param name="T"></param>
	[Serializable]
	public class BindableCollection<T> : BindingList<T>
	{
		private bool _isSorted = false;
		[NonSerialized]
		private PropertyComparer<T> _comparer = null;

		#region events...
        public event EventHandler<ListChangedEventArgs> CollectionChangedEvent;
		public event EventHandler<AddingNewEventArgs> ItemAddedEvent;
		public event EventHandler<ItemRemovedEventArgs> ItemRemovedEvent;
		public event EventHandler ClearEvent;
		#region OnCollectionChangedEvent
		/// <summary>
		/// Triggers the CollectionChangedEvent event.
		/// </summary>
		protected virtual void OnCollectionChangedEvent(ListChangedEventArgs ea)
		{
			if (CollectionChangedEvent != null)
				CollectionChangedEvent(null/*this*/, ea);
		}
		#endregion
		#region OnItemAddedEvent
		/// <summary>
		/// Triggers the ItemAddedEvent event.
		/// </summary>
		private /*protected virtual*/ void OnItemAddedEvent(AddingNewEventArgs ea)
		{
			if (ItemAddedEvent != null)
				ItemAddedEvent(null/*this*/, ea);
		}
		#endregion
		#region OnItemRemovedEvent
		/// <summary>
		/// Triggers the ItemRemovedEvent event.
		/// </summary>
		protected virtual void OnItemRemovedEvent(ItemRemovedEventArgs ea)
		{
			if (ItemRemovedEvent != null)
				ItemRemovedEvent(null/*this*/, ea);
		}
		#endregion
		#region OnClearEvent
		/// <summary>
		/// Triggers the ClearEvent event.
		/// </summary>
		protected virtual void OnClearEvent(EventArgs ea)
		{
			if (ClearEvent != null)
				ClearEvent(null/*this*/, ea);
		}
		#endregion
		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="BindableCollection&lt;T&gt;"/> class.
		/// </summary>
		public BindableCollection(){}

		/// <summary>
		/// Initializes a new instance of the <see cref="BindableCollection&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="comparer">The comparer.</param>
		public BindableCollection(PropertyComparer<T> comparer)
		{
			_comparer = comparer;
		}

		/// <summary>
		/// Raises the System.ComponentModel.BindingList<T>.ListChanged event.
		/// </summary>
		/// <param name="e">A System.ComponentModel.ListChangedEventArgs that contains the event data.</param>
		protected override void OnListChanged(ListChangedEventArgs e)
		{
			base.OnListChanged(e);
			if (e.ListChangedType == ListChangedType.ItemAdded)
			{
				INotifyPropertyChanged inotifyable = this[e.NewIndex] as INotifyPropertyChanged;
				if (null != inotifyable)
				{
					inotifyable.PropertyChanged += OnItemChanged;
				}
				OnItemAddedEvent(new AddingNewEventArgs(this[e.NewIndex]));
			}
			else if (e.ListChangedType== ListChangedType.Reset && this.Count==0)
			{
				OnClearEvent(e);
			}
			else if (e.ListChangedType== ListChangedType.ItemDeleted)
			{
				OnItemRemovedEvent(new ItemRemovedEventArgs(e.NewIndex));
			}
		}

		private void OnItemChanged(Object sender, PropertyChangedEventArgs args)
		{
			if (sender is T)
			{
				ResetItem(Items.IndexOf((T)sender));
			}
		}

		/// <summary>
		/// Sorts the items if overridden in a derived class; otherwise, throws a <see cref="T:System.NotSupportedException"/>.
		/// </summary>
		/// <param name="prop">A <see cref="T:System.ComponentModel.PropertyDescriptor"/> that specifies the property to sort on.</param>
		/// <param name="direction">One of the <see cref="T:System.ComponentModel.ListSortDirection"/>  values.</param>
		/// <exception cref="T:System.NotSupportedException">Method is not overridden in a derived class. </exception>
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

		/// <summary>
		/// Sorts the specified property name.
		/// </summary>
		/// <param name="propertyName">Name of the property.</param>
		/// <param name="direction">The direction.</param>
		public virtual void Sort(string propertyName, ListSortDirection direction)
		{
			PropertyDescriptor descriptor = TypeDescriptor.GetProperties(typeof(T))[propertyName];
			if (descriptor != null)
			{
				ApplySortCore(descriptor, direction);
			}
		}

		/// <summary>
		/// Gets a value indicating whether the list supports sorting.
		/// </summary>
		/// <value></value>
		/// <returns>true if the list supports sorting; otherwise, false. The default is false.</returns>
		protected override bool  SupportsSortingCore
		{
			get { return true; }
		}

		/// <summary>
		/// Gets a value indicating whether the list is sorted.
		/// </summary>
		/// <value></value>
		/// <returns>true if the list is sorted; otherwise, false. The default is false.</returns>
		protected override bool IsSortedCore
		{
			get
			{
				return _isSorted;
			}
		}

		/// <summary>
		/// Gets the direction the list is sorted.
		/// </summary>
		/// <value></value>
		/// <returns>One of the <see cref="T:System.ComponentModel.ListSortDirection"/> values. The default is <see cref="F:System.ComponentModel.ListSortDirection.Ascending"/>. </returns>
		protected override ListSortDirection SortDirectionCore
		{
			get
			{
				return _comparer.SortDirection;
			}
		}

		/// <summary>
		/// Gets the property descriptor that is used for sorting the list if sorting is implemented in a derived class; otherwise, returns null.
		/// </summary>
		/// <value></value>
		/// <returns>The <see cref="T:System.ComponentModel.PropertyDescriptor"/> used for sorting the list.</returns>
		protected override PropertyDescriptor SortPropertyCore
		{
			get
			{
				return _comparer.PropDescriptor;
			}
		}

		
	}
	/// <summary>
	/// Provides data for the ItemRemovedEvent event. 
	/// </summary>
	public class ItemRemovedEventArgs : EventArgs
	{
		private int index;
		public int Index
		{
			get { return index; }
			set
			{
				index = value;
			}
		}
		/// <summary>
		/// Initializes a new instance of the ItemRemovedEventArgs class.
		/// </summary>
		/// <param name="index"></param>
		public ItemRemovedEventArgs(int index)
		{
			this.index = index;
		}
	}
}
