// /home/carlos/boxerp_completo/trunk/src/client-lib/tests/bindableObjectsTests/bindableObjectsTests/SimpleBusinessObject.cs created with MonoDevelop
// User: carlos at 3:18 PM 7/7/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;

namespace bindableObjectsTests
{
	
	[Serializable]
	public class SimpleBusinessObject 
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
	}

	[Serializable]
	public class SimpleObjectComplexProperties
	{
		private string _code;
		private List<string> _names;
		private string _description;
		private int[] _ages = new int[] { 10, 20, 30, 40, 50, 60 };

		public virtual string Code
		{
			get { return _code; }
			set { _code = value; }
		}
		
		public virtual string Description
		{
			get { return _description; }
			set { _description = value; }
		}
		
		public virtual int[] Ages
		{
			get { return _ages; }
			set { _ages = value; }
		}
		
		public virtual List<string> Names
		{
			get { return _names; }
			set { _names = value; }
		}
	}

	[Serializable]
	public class SimpleObjectReadOnlyProperties
	{
		private string _code;
		
		public virtual string Code
		{
			get { return _code; }
		}

		public void SetCode(string code)
		{
			_code = code + DateTime.Now.ToShortDateString();
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
