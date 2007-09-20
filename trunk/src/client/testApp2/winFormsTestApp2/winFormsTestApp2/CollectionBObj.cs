using System;
using System.Collections.Generic;
using System.Text;

namespace winFormsTestApp2
{
	public class CollectionBObj : ICloneable
	{
		private string _title;
		private List<SampleBObj> _collection = new List<SampleBObj>();

		public virtual string Title
		{
			get { return _title; }
			set { _title = value; }
		}
		
		public virtual List<SampleBObj> Collection
		{
			get { return _collection; }
			set { _collection = value; }
		}

		#region ICloneable Members

		public object Clone()
		{
			CollectionBObj copy = new CollectionBObj();
			copy.Title = Title;
			foreach (SampleBObj child in Collection)
			{
				copy.Collection.Add(child);
			}

			return copy;
		}

		#endregion
	}
}
