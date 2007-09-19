using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Boxerp.Client;

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

	public class Proxy1 : SampleBObj, ICustomNotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		public bool HasSubscribers()
		{
			return PropertyChanged != null;
		}

		public void ThrowPropertyChangedEvent(string name)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(name));
		}
	}

	public class Proxy2 : Proxy1
	{
		public override string Name
		{
			get
			{
				return base.Name;
			}
			set
			{
				base.Name = value;
				if (HasSubscribers())
				{
					ThrowPropertyChangedEvent("Name");
				}
			}
		}

		public override string Description
		{
			get
			{
				return base.Description;
			}
			set
			{
				base.Description = value;
				if (HasSubscribers())
				{
					ThrowPropertyChangedEvent("Description");
				}
			}
		}

		public override int Age
		{
			get
			{
				return base.Age;
			}
			set
			{
				base.Age = value;
				if (HasSubscribers())
				{
					ThrowPropertyChangedEvent("Age");
				}
			}
		}
	}		
}
