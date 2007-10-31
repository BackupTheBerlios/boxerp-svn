using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;
using Boxerp.Client;
using System.Reflection;

namespace bindableObjectsTests
{
	[Serializable]
	public class SimpleHardcodedPlusHelperProxy : SimpleBusinessObject, ICustomNotifyPropertyChanged, ISerializable
	{
		public SimpleHardcodedPlusHelperProxy() { }

		protected SimpleHardcodedPlusHelperProxy(SerializationInfo info, StreamingContext c)
		{
			// the proxy is never deserialized it is recreated and the helper copies the values
		}

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.SetType(typeof(SerializationHelperFirst)); // tell the formatter to serialize the helper, not this

			MemberInfo[] serializableMembers = FormatterServices.GetSerializableMembers(this.GetType());
			object[] data = FormatterServices.GetObjectData(this, serializableMembers);
			info.AddValue(DynamicPropertyChangedProxy.SERIALIZED_DATA, data);
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

	/// <summary>
	/// Helper. It is serialized and deserialized instead of serializing the proxy
	/// </summary>
	[Serializable]
	public class SerializationHelperFirst : IObjectReference, ISerializable, IDeserializationCallback
	{
		private object _proxy;
		private readonly SerializationInfo _info;
		private readonly StreamingContext _context;
		
		protected SerializationHelperFirst(SerializationInfo info, StreamingContext context)
		{
			_info = info;
			_context = context;

			Castle.DynamicProxy.ProxyGenerator generator = new Castle.DynamicProxy.ProxyGenerator();
			_proxy = (SimpleHardcodedPlusHelperProxy)
				generator.CreateClassProxy(typeof(SimpleHardcodedPlusHelperProxy), new Castle.Core.Interceptor.IInterceptor[0]);
		}

		#region IObjectReference Members

		public object GetRealObject(StreamingContext context)
		{
			return _proxy;
		}

		#endregion

		#region ISerializable Members

		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			// do nothing
		}

		#endregion

		#region IDeserializationCallback Members

		public void OnDeserialization(object sender)
		{
			object[] baseMemberData = (object[]) _info.GetValue (DynamicPropertyChangedProxy.SERIALIZED_DATA, typeof (object[]));
			MemberInfo[] members = FormatterServices.GetSerializableMembers (_proxy.GetType());
			FormatterServices.PopulateObjectMembers (_proxy, members, baseMemberData);
		}

		#endregion
	}

}
