// /home/carlos/boxerp_completo/trunk/src/client-lib/tests/bindableObjectsTests/bindableObjectsTests/ComplexBusinessObjTests.cs created with MonoDevelop
// User: carlos at 7:19 PM 7/7/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Boxerp.Client;
using NUnit.Framework;

namespace bindableObjectsTests
{
	
[TestFixture]
public class ComplexBusinessObjTests
{
		
	[Test]
	public void CreateNestedBindable()
	{
		BindableWrapper<ComplexBusinessObject> bindableObj = 
			new BindableWrapper<ComplexBusinessObject>(new ComplexBusinessObject());
		Assert.IsNotNull(bindableObj);
	}

	[Test]
	public void ChangeNestedBindable()
	{
		BindableWrapper<ComplexBusinessObject> bindableObj =	
			new BindableWrapper<ComplexBusinessObject>(new ComplexBusinessObject());
		bindableObj.Data.BusinessObj.SomeFlag = true;
		bindableObj.Data.BusinessObj.NestedObject = new SimpleBusinessObject();
		
		Assert.IsNotNull(bindableObj.Data.BusinessObj.NestedObject);
	}
	
	[Test]
	public void UndoNestedChanges()
	{
		BindableWrapper<ComplexBusinessObject> bindableObj =	
			new BindableWrapper<ComplexBusinessObject>(new ComplexBusinessObject());
		
		SimpleBusinessObject sbo1 = new SimpleBusinessObject();
		sbo1.Name = "sbo1";
		SimpleBusinessObject sbo2 = new SimpleBusinessObject();
		sbo2.Name = "sbo2";
		
		bindableObj.Data.BusinessObj.NestedObject = sbo1;
		Assert.AreEqual(bindableObj.Data.BusinessObj.NestedObject.Name, sbo1.Name);
		
		bindableObj.Data.BusinessObj.NestedObject = sbo2;
		Assert.AreEqual(bindableObj.Data.BusinessObj.NestedObject.Name, sbo2.Name);
			
		bindableObj.Undo();
		
		// As the nested object is not intercepted undo does not have any effect
		Assert.AreEqual(bindableObj.Data.BusinessObj.NestedObject.Name, sbo2.Name);
		
		// the nested object is not IBindableWrapper so changes on its internal properties are not intercepted
		bindableObj.Data.BusinessObj.NestedObject.Name = "whatever";
		bindableObj.Data.BusinessObj.NestedObject.Name = "somethingelse";
	}
	
	[Test]
	public void UndoRedoNestedChanges()
	{
		BindableWrapper<ComplexBusinessObject> bindableObj =	
			new BindableWrapper<ComplexBusinessObject>(new ComplexBusinessObject());

		SimpleBusinessObject sbo1 = new SimpleBusinessObject();
		sbo1.Name = "sbo1";
		SimpleBusinessObject sbo2 = new SimpleBusinessObject();
		sbo2.Name = "sbo2";
		
		bindableObj.Data.BusinessObj.NestedObject = sbo1;
		Assert.AreEqual(bindableObj.Data.BusinessObj.NestedObject.Name, sbo1.Name);
		
		bindableObj.Data.BusinessObj.NestedObject = sbo2;
		Assert.AreEqual(bindableObj.Data.BusinessObj.NestedObject.Name, sbo2.Name);
			
		bindableObj.Undo();
		
		Assert.AreEqual(bindableObj.Data.BusinessObj.NestedObject.Name, sbo2.Name);
			
		bindableObj.Redo();
		Assert.AreEqual(bindableObj.Data.BusinessObj.NestedObject.Name, sbo2.Name);
	}

	[Test]
	public void SerializationTest()
	{
		BindableWrapper<ComplexBusinessObject> bindableObj =
			new BindableWrapper<ComplexBusinessObject>(new ComplexBusinessObject());
		bindableObj.Data.BusinessObj.SomeFlag = true;
		bindableObj.Data.BusinessObj.NestedObject = new SimpleBusinessObject();

		MemoryStream stream = new MemoryStream();
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, bindableObj);
		stream.Position = 0;

		object deserializedObject = formatter.Deserialize(stream);
		Assert.IsNotNull(deserializedObject);
		BindableWrapper<ComplexBusinessObject> deserializedBindable = (BindableWrapper<ComplexBusinessObject>)deserializedObject;

		Assert.IsNotNull(deserializedBindable);
		Assert.IsTrue(deserializedBindable.Data.BusinessObj.SomeFlag);
		Assert.IsNotNull(deserializedBindable.Data.BusinessObj.NestedObject);
	}
}
}
