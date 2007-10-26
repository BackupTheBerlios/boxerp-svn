// /home/carlos/boxerp_completo/trunk/src/client-lib/tests/bindableObjectsTests/bindableObjectsTests/SimpleBusinessObject.cs created with MonoDevelop
// User: carlos at 3:18 PM 7/7/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

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

	public enum SimpleEnum
	{
		line1,
		line2
	}

	[Serializable]
	public struct SimpleStruct
	{
		public int _int1;
		public bool _bool1;
	}

	[Serializable]
	public class SimpleObjectValueTypes
	{
		// Value types
		private int _number;
		public virtual int Integer
		{
			get { return _number; }
			set { _number = value; }
		}

		private bool _boolean;
		public virtual bool Boolean
		{
			get { return _boolean; }
			set { _boolean = value; }
		}

		private float _float;
		public virtual float Float
		{
			get { return _float; }
			set { _float = value; }
		}

		private double _double;
		public virtual double Double
		{
			get { return _double; }
			set { _double = value; }
		}

		private char _char;
		public virtual char Char
		{
			get { return _char; }
			set { _char = value; }
		}

		private SimpleEnum _enum;
		public virtual SimpleEnum Enum
		{
			get { return _enum; }
			set { _enum = value; }
		}

		private SimpleStruct _struct;
		public virtual SimpleStruct Struct
		{
			get { return _struct; }
			set { _struct = value; }
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

	[Serializable]
	public class SimpleIndexedClass : Dictionary<string, int>
	{
		private string _code;
		
		public virtual string Code
		{
			get { return _code; }
			set { _code = value; }
		}

		public SimpleIndexedClass() { }

		/*public SimpleIndexedClass(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
			_code = info.GetString("_code");
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("_code", _code);
		}*/
	}

	//[Serializable]
	public class SimpleAutoSerializable : ISerializable
	{
		public SimpleAutoSerializable() { }

		public SimpleAutoSerializable(SerializationInfo info, StreamingContext context)
		{
			// implement this
		}

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			// implement this
		}
	}

	[Serializable]
	public class SimpleGenericObject<T>
	{
		private string _code;
		private T[] _genericArray;

		public virtual string Code
		{
			get { return _code; }
			set { _code = value; }
		}
		
		public virtual T[] GenericArray
		{
			get { return _genericArray; }
			set { _genericArray = value; }
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
