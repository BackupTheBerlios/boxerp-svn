using System;
using System.Collections.Generic;
using System.Text;

namespace dataBinderTests
{
	[Serializable]
	public class SampleObj
	{
		private string _name;

		public virtual string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		private string _description;

		public virtual string Description
		{
			get { return _description; }
			set { _description = value; }
		}

	}
}
