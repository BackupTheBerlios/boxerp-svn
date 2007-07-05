using System;
using System.Collections.Generic;
using System.Text;

namespace testApp2
{
	public class SampleBObj : ICloneable
	{
		private string _name, _description;
		private int _age;

		public virtual string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		public virtual string Name
		{
			get { return _name; }
			set { _name = value; }
		}
		
		public virtual int Age
		{
			get { return _age; }
			set { _age = value; }
		}

		public SampleBObj() { }

		public SampleBObj(string name, string description, int age) 
		{
			_name = name;
			_description = description;
			_age = age;
		}
		

		#region ICloneable Members

		public object Clone()
		{
			SampleBObj clone = new SampleBObj(Name, Description, Age);
			return clone;
		}

		#endregion

		
	}
}
