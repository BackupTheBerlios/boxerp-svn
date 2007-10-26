using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using Boxerp.Client;

namespace bindableObjectsTests
{
	[TestFixture]
	public class HardCodedProxiesTests
	{
		[Test]
		public void SerializeSimpleSerializable()
		{
			SimpleSerializable serializableBusinessObject = new SimpleSerializable();
			serializableBusinessObject.Name = "test";

			SimpleSerializable deserialized = (SimpleSerializable) Cloner.GetSerializedClone(serializableBusinessObject);

			Assert.AreEqual(deserialized.Name, "test");
		}

		[Test]
		public void SerializeSimpleSerializableHardcodedProxy()
		{
			SimpleSerializableHardcodedProxy serializableBusinessObject = new SimpleSerializableHardcodedProxy();
			serializableBusinessObject.Name = "test";

			SimpleSerializableHardcodedProxy deserialized = (SimpleSerializableHardcodedProxy)Cloner.GetSerializedClone(serializableBusinessObject);

			Assert.AreEqual(deserialized.Name, "test");
		}

		[Test]
		public void SimpleHardcodedProxy()
		{
			SimpleHardcodedProxy serializableBusinessObject = new SimpleHardcodedProxy();
			serializableBusinessObject.Name = "test";

			SimpleHardcodedProxy deserialized = (SimpleHardcodedProxy)Cloner.GetSerializedClone(serializableBusinessObject);

			Assert.AreEqual(deserialized.Name, "test");
		}

		[Test]
		public void SimpleRealHardcodedProxy()
		{
			SimpleRealHardcodedProxy serializableBusinessObject = new SimpleRealHardcodedProxy();
			serializableBusinessObject.Name = "test";

			SimpleRealHardcodedProxy deserialized = (SimpleRealHardcodedProxy)Cloner.GetSerializedClone(serializableBusinessObject);

			Assert.AreEqual(deserialized.Name, "test");
		}

		[Test]
		public void SimpleRealHardcoded_CompleteBoxerpProxy()
		{
			SimpleRealHardcoded_CompleteBoxerpProxy serializableBusinessObject = new SimpleRealHardcoded_CompleteBoxerpProxy();
			serializableBusinessObject.Name = "test";
			
			SimpleRealHardcoded_CompleteBoxerpProxy deserialized = (SimpleRealHardcoded_CompleteBoxerpProxy)Cloner.GetSerializedClone(serializableBusinessObject);

			Assert.AreEqual(deserialized.Name, "test");
		}

		[Test]
		public void SimpleRealHardcoded_Complete_BoxerpPlusCastle_Proxy()
		{
			SimpleRealHardcoded_CompleteBoxerpProxy serializableBusinessObject = new SimpleRealHardcoded_CompleteBoxerpProxy();
			
			Castle.DynamicProxy.ProxyGenerator generator = new Castle.DynamicProxy.ProxyGenerator();
			SimpleRealHardcoded_CompleteBoxerpProxy proxy = (SimpleRealHardcoded_CompleteBoxerpProxy)
				generator.CreateClassProxy(typeof(SimpleRealHardcoded_CompleteBoxerpProxy), 
				new Castle.Core.Interceptor.IInterceptor[0]);

			proxy.Name = "test";

			SimpleRealHardcoded_CompleteBoxerpProxy deserialized = 
				(SimpleRealHardcoded_CompleteBoxerpProxy)Cloner.GetSerializedClone(proxy);

			Assert.AreEqual(deserialized.Name, "test");
		}

		[Test]
		public void WrapperHardcoded()
		{
			SimpleRealHardcoded_CompleteBoxerpProxy serializableBusinessObject = new SimpleRealHardcoded_CompleteBoxerpProxy();
			serializableBusinessObject.Name = "test";

			WrapperHardcoded<SimpleRealHardcoded_CompleteBoxerpProxy> wrapper = new WrapperHardcoded<SimpleRealHardcoded_CompleteBoxerpProxy>();
			wrapper.Data.InnerObject = serializableBusinessObject;

			WrapperHardcoded<SimpleRealHardcoded_CompleteBoxerpProxy> deserialized =
				(WrapperHardcoded<SimpleRealHardcoded_CompleteBoxerpProxy>)Cloner.GetSerializedClone(wrapper);

			Assert.AreEqual(deserialized.Data.InnerObject.Name, "test");
		}
	}
}
