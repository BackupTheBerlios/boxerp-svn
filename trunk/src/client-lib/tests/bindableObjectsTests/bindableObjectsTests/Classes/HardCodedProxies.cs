using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization;
using Boxerp.Client;
using System.ComponentModel;

namespace bindableObjectsTests
{
	[Serializable]
	public class SimpleSerializable : ISerializable
	{
		private string _name;

		public SimpleSerializable() {}

		protected SimpleSerializable(SerializationInfo info, StreamingContext c)
		{
			_name = (string)info.GetValue("_name", typeof(string));
		}

		public virtual string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		#region ISerializable Members

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("_name", _name);
		}

		#endregion
	}



	[Serializable]
	public class SimpleSerializableHardcodedProxy : SimpleSerializable, ISerializable 
	{
		public SimpleSerializableHardcodedProxy(){}

		protected SimpleSerializableHardcodedProxy(SerializationInfo info, StreamingContext c)	
			: base(info, c)	
		{
			
		}

		#region ISerializable Members

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
		#endregion
	}

// ---------------------------------------------------------------------

	[Serializable]
	public class SimpleObject
	{
		private string _name;

		public virtual string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}
	}

	/// <summary>
	/// THIS IMPLEMENTATION OF THE SERIALIZATION AND DESERIALIZATION HAS SOME PROBLEMS:
	/// The GetFields method does not go through the class hierarchy, so inherited fields will be lost. 
	/// It is working here because I am using the BaseType but it the class had in turn another base, 
	/// it wouldn't work. 
	/// </summary>
	[Serializable]
	public class SimpleHardcodedProxy : SimpleObject, ISerializable
	{
		public SimpleHardcodedProxy() { }

		protected SimpleHardcodedProxy(SerializationInfo info, StreamingContext c)
		{
			FieldInfo[] fields = this.GetType().BaseType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			foreach (FieldInfo field in fields)
			{
				object[] attributes = field.GetCustomAttributes(typeof(NonSerializedAttribute), true);
				if (attributes.Length == 0)
				{
					field.SetValue(this, info.GetValue(field.Name, field.FieldType));
				}
			}
		}

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			FieldInfo[] fields = this.GetType().BaseType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			foreach (FieldInfo field in fields)
			{
				object[] attributes = field.GetCustomAttributes(typeof(NonSerializedAttribute), true);
				if (attributes.Length == 0)
				{
					info.AddValue(field.Name, field.GetValue(this));
				}
			}
		}
	}

	/// <summary>
	/// THIS IMPLEMENTATION OF THE SERIALIZATION AND DESERIALIZATION HAS SOME PROBLEMS:
	/// The GetFields method does not go through the class hierarchy, so inherited fields will be lost. 
	/// It is working here because I am using the BaseType but it the class had in turn another base, 
	/// it wouldn't work. 
	/// </summary>
	[Serializable]
	public class SimpleRealHardcodedProxy : SimpleBusinessObject, ISerializable
	{
		public SimpleRealHardcodedProxy() { }

		protected SimpleRealHardcodedProxy(SerializationInfo info, StreamingContext c)
		{
			FieldInfo[] fields = this.GetType().BaseType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			foreach (FieldInfo field in fields)
			{
				object[] attributes = field.GetCustomAttributes(typeof(NonSerializedAttribute), true);
				if (attributes.Length == 0)
				{
					field.SetValue(this, info.GetValue(field.Name, field.FieldType));
				}
			}
		}

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			FieldInfo[] fields = this.GetType().BaseType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			foreach (FieldInfo field in fields)
			{
				object[] attributes = field.GetCustomAttributes(typeof(NonSerializedAttribute), true);
				if (attributes.Length == 0)
				{
					info.AddValue(field.Name, field.GetValue(this));
				}
			}
		}
	}

	/// <summary>
	/// THIS IMPLEMENTATION OF THE SERIALIZATION is the good one
	/// </summary>
	[Serializable]
	public class SimpleRealHardcoded_CompleteBoxerpProxy : SimpleBusinessObject, ICustomNotifyPropertyChanged, ISerializable
	{
		public SimpleRealHardcoded_CompleteBoxerpProxy() { }

		protected SimpleRealHardcoded_CompleteBoxerpProxy(SerializationInfo info, StreamingContext c)
		{
			object[] data = (object[])info.GetValue("_serializedData", typeof(object[]));
			MemberInfo[] members = FormatterServices.GetSerializableMembers(this.GetType());
			FormatterServices.PopulateObjectMembers(this, members, data);
		}

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			MemberInfo[] serializableMembers = FormatterServices.GetSerializableMembers(this.GetType());
			object [] data = FormatterServices.GetObjectData(this, serializableMembers);
			info.AddValue("_serializedData", data);
		}

		#region ICustomNotifyPropertyChanged Members

		public bool HasSubscribers()
		{
			bool retValue = false;
			if (PropertyChanged != null)
			{
				retValue = true;
			}

			return retValue;
		}

		public void ThrowPropertyChangedEvent(string propertyName)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public Delegate[] GetSubscribersList()
		{
			return PropertyChanged.GetInvocationList();
		}

		#endregion

		#region INotifyPropertyChanged Members

		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

		#endregion
	}

	[Serializable]
	public class WrapperHardcoded<T>
	{
		private BindableFields<T> _bindable = new BindableFields<T>();

		public WrapperHardcoded() { }

		public BindableFields<T> Data
		{
			get
			{
				return _bindable;
			}
			set
			{
				_bindable = value;
			}
		}

		[Serializable]
		public class BindableFields<D>
		{
			private D _innerObject;

			public BindableFields() { }

			public D InnerObject
			{
				get
				{
					return _innerObject;
				}
				set
				{
					_innerObject = value;
				}
			}
		}
	}


}
