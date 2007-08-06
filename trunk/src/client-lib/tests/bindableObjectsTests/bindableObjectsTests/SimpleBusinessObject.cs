// /home/carlos/boxerp_completo/trunk/src/client-lib/tests/bindableObjectsTests/bindableObjectsTests/SimpleBusinessObject.cs created with MonoDevelop
// User: carlos at 3:18 PM 7/7/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace bindableObjectsTests
{
	
	[Serializable]
	public class SimpleBusinessObject : ICloneable
	{
		private string _name;
		private string _description;
		private int _age;
		
		public virtual string Name {
			get 
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public virtual string Description {
			get 
			{
				return _description;
			}
			set
			{
				_description = value;
			}
		}

		public virtual int Age {
			get 
			{
				return _age;
			}
			set 
			{
				_age = value;
			}
		}
		
		public SimpleBusinessObject()
		{
		}
		
		private SimpleBusinessObject(string name, string desc, int age)
		{
			_name = name;
			_description = desc;
			_age = age;
		}
		
		public object Clone()
		{
			return new SimpleBusinessObject(_name, _description, _age);
		}
	}

	public class AnySubscriber
	{
		public void OnPropertyChanged(Object sender, EventArgs args)
		{
			throw new Exception("testing");
		}
	}

}
