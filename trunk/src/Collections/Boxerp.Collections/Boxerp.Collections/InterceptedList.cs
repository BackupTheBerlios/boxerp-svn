using System;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Collections
{
	public class InterceptedList<T> : IList<T>
	{
		public event EventHandler CollectionChangedEvent;
		public event EventHandler ItemAddedEvent;
		public event EventHandler ItemRemovedEvent;
		public event EventHandler ClearEvent;

        private List<T> innerList = new List<T>();

		public virtual void Add(T item)
		{
			innerList.Add(item);
			OnCollectionChanged(item);
			if (ItemAddedEvent != null)
			{
				ItemAddedEvent.Invoke(item, null);
			}
		}

		public virtual void Remove(T item)
		{
			innerList.Remove(item);
            OnCollectionChanged(item);
			if (ItemRemovedEvent != null)
			{
				ItemRemovedEvent.Invoke(item, null);
			}
		}

		public virtual void Clear()
		{
			innerList.Clear();
			//OnCollectionChanged();
			if (ClearEvent != null)
			{
				ClearEvent.Invoke(this, null);
			}
		}

        #region IList<T> Members

        public int IndexOf(T item)
        {
            return innerList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            innerList.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            innerList.RemoveAt(index);
        }

        public T this[int index]
        {
            get
            {
                return innerList[index];
            }
            set
            {
                innerList[index] = value;
                OnCollectionChanged(value);
            }
        }

        #endregion

        #region ICollection<T> Members


        public bool Contains(T item)
        {
            return innerList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            innerList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get
            {
                return innerList.Count; ;
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<T>.Remove(T item)
        {
            return innerList.Remove(item);
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return innerList.GetEnumerator();
        }
        #endregion

        protected void OnCollectionChanged(T item)
        {
            if (CollectionChangedEvent != null)
            {
                CollectionChangedEvent.Invoke(item, null);
            }
        }

    }
}
