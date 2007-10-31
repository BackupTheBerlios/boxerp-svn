using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Boxerp.Client;
using NUnit.Framework;
using System.Reflection;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace bindableObjectsTests
{
	[TestFixture]
	public class HardcodedPlusIObjectRefTests
	{
		[Test]
		public void SerializeDoubleProxy()
		{
			Castle.DynamicProxy.ProxyGenerator generator = new Castle.DynamicProxy.ProxyGenerator();
			SimpleHardcodedPlusHelperProxy proxy = (SimpleHardcodedPlusHelperProxy)
				generator.CreateClassProxy(typeof(SimpleHardcodedPlusHelperProxy), new Castle.Core.Interceptor.IInterceptor[0]);

			proxy.Name = "test";
			SimpleHardcodedPlusHelperProxy deserialized = (SimpleHardcodedPlusHelperProxy)Cloner.GetSerializedClone(proxy);
			
			Assert.AreEqual(deserialized.Name, "test");
		}
	}
}
