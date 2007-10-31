using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Reflection;
using Castle.DynamicProxy;
using Castle.Core.Interceptor;

namespace Boxerp.Client
{
	/// <summary>
	/// See this post to understand the need for this helper. This is the Boxerp version of the 
	/// Castle.DynamicProxy2\Castle.DynamicProxy\Serialization\ProxyObjectReference class
	/// </summary>
	[Serializable]
	public class DynamicProxyHelper : IObjectReference, ISerializable, IDeserializationCallback
	{
		private object _proxy;
		private Type[] _argumentsForConstructor;
		private readonly SerializationInfo _info;
		private readonly StreamingContext _context;
		private string _baseTypeName;
		private Type _baseT;

		public DynamicProxyHelper(SerializationInfo info, StreamingContext context)
		{
			_info = info;
			_context = context;

			_argumentsForConstructor = (Type[])info.GetValue(DynamicPropertyChangedProxy.ARGUMENTS4CONSTRUCTOR, typeof(Type[]));
			_baseTypeName = (string)info.GetValue(DynamicPropertyChangedProxy.OBJECT_BASE_TYPE, typeof(string));
			bool isBusinessObj = (bool)info.GetValue(DynamicPropertyChangedProxy.IS_BUSINESS_OBJECT, typeof(bool));
			_baseT = Type.GetType(_baseTypeName, true, false);
			Type firstProxyType;
			if (isBusinessObj)
			{
				firstProxyType = DynamicPropertyChangedProxy.CreateBusinessObjectProxy(_baseT, _argumentsForConstructor);
			}
			else
			{
				firstProxyType = DynamicPropertyChangedProxy.CreateBindableWrapperProxy(_baseT, _argumentsForConstructor);
			}

			object[] baseMemberData = (object[])_info.GetValue(DynamicPropertyChangedProxy.SERIALIZED_DATA, typeof(object[]));
			
			ProxyGenerator generator = new ProxyGenerator();
			_proxy = generator.CreateClassProxy(firstProxyType, (IInterceptor[])baseMemberData[0]);
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
