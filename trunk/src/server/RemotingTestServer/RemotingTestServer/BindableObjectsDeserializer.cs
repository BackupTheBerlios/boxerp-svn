using System;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Configuration;
using Boxerp.Client;
using bindableObjectsTests;
using System.Collections.Generic;
using System.Text;

namespace RemotingTestServer
{
	public class BindableObjectsDeserializer : MarshalByRefObject
	{
		public void ReadBindableObject(BindableWrapper<SimpleBusinessObject> bindable)
		{
			if (!bindable.Data.BusinessObj.Name.Equals("asdf"))
			{
				throw new Exception("The name does not match: " + bindable.Data.BusinessObj.Name);
			}

			Console.Out.WriteLine("Reading bindable name:" + bindable.Data.BusinessObj.Name);
		}

		public void ReadBindableComplexObject(BindableWrapper<ComplexBusinessObject> bindable)
		{
			if (!bindable.Data.BusinessObj.NestedObject.Name.Equals("asdf"))
			{
				throw new Exception("The name does not match: " + bindable.Data.BusinessObj.NestedObject.Name);
			}

			Console.Out.WriteLine("Reading bindable name:" + bindable.Data.BusinessObj.NestedObject.Name);
		}

		public void ReadBindableNestedWrapper(BdWithNestedWrapper<SimpleBusinessObject, BindableWrapper<SimpleBusinessObject>> bindable)
		{
			if (!bindable.Data.NestedWrapper.Data.BusinessObj.Name.Equals("asdf"))
			{
				throw new Exception("The name does not match: " + bindable.Data.NestedWrapper.Data.BusinessObj.Name);
			}

			Console.Out.WriteLine("Reading bindable name:" + bindable.Data.NestedWrapper.Data.BusinessObj.Name);
		}

	}
}
