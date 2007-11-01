using System;
using System.Runtime.Remoting;
using Boxerp.Client;
using System.Threading;
using RemotingTestServer;
using bindableObjectsTests;

namespace RemotingClientTest
{

	public class Client
	{
		static void Main(string[] args)
		{
			RemotingConfiguration.Configure("..\\..\\clientRemoting.config", false);

			BindableObjectsDeserializer deserializer = 
					(BindableObjectsDeserializer) RemotingHelper.GetObject(typeof(BindableObjectsDeserializer));

			SendBindableWrapperSBO(deserializer);
			SendBindableWrapperComplexObject(deserializer);
			SendBindableWrapperNestedWrapper(deserializer);

			Console.WriteLine("All calls succeded");
			Console.ReadLine();
		}

		private static void SendBindableWrapperSBO(BindableObjectsDeserializer bod)
		{
			BindableWrapper<SimpleBusinessObject> bindableObj =
			new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject());
			bindableObj.Data.BusinessObj.Name = "asdf";
			bindableObj.Data.BusinessObj.Description = "asdfsdf";
			bindableObj.Data.BusinessObj.Age = 25;

			bod.ReadBindableObject(bindableObj);

			Console.WriteLine("Server called");
		}

		private static void SendBindableWrapperComplexObject(BindableObjectsDeserializer bod)
		{
			BindableWrapper<ComplexBusinessObject> bindableObj =
			new BindableWrapper<ComplexBusinessObject>(new ComplexBusinessObject());
			bindableObj.Data.BusinessObj.SomeFlag = true;
			bindableObj.Data.BusinessObj.NestedObject = new SimpleBusinessObject();
			bindableObj.Data.BusinessObj.NestedObject.Name = "asdf";
			bod.ReadBindableComplexObject(bindableObj);

			Console.WriteLine("Server called");
		}

		private static void SendBindableWrapperNestedWrapper(BindableObjectsDeserializer bod)
		{
			BdWithNestedWrapper<SimpleBusinessObject, BindableWrapper<SimpleBusinessObject>> bindable =
				new BdWithNestedWrapper<SimpleBusinessObject, BindableWrapper<SimpleBusinessObject>>
					(new SimpleBusinessObject(), new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject()));


			bindable.Data.NestedWrapper.Data.BusinessObj.Name = "asdf";

			bod.ReadBindableNestedWrapper(bindable);

			Console.WriteLine("Server called");
		}
	}
}