using System;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Collections
{
	public class InterceptedList<T> : List<T>
	{
		public event EventHandler CollectionChangedEvent;
		public event EventHandler ItemAddedEvent;
		public event EventHandler ItemRemovedEvent;
		public event EventHandler ClearEvent;

		public new void Add(T item)
		{
			base.Add(item);
			if (CollectionChangedEvent != null)
			{
				CollectionChangedEvent.Invoke(item, null);
			}
			if (ItemAddedEvent != null)
			{
				ItemAddedEvent.Invoke(item, null);
			}
		}

		public new void Remove(T item)
		{
			base.Remove(item);
			if (CollectionChangedEvent != null)
			{
				CollectionChangedEvent.Invoke(item, null);
			}
			if (ItemRemovedEvent != null)
			{
				ItemRemovedEvent.Invoke(item, null);
			}
		}

		public new void Clear()
		{
			base.Clear();
			if (CollectionChangedEvent != null)
			{
				CollectionChangedEvent.Invoke(this, null);
			}
			if (ClearEvent != null)
			{
				ClearEvent.Invoke(this, null);
			}
		}
	}
}
