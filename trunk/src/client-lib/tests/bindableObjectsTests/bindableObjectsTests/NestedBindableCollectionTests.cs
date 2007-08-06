// /home/carlos/boxerp_completo/trunk/src/client-lib/tests/bindableObjectsTests/bindableObjectsTests/NestedBindableCollectionTests.cs created with MonoDevelop
// User: carlos at 10:52 PM 7/7/2007
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
public class NestedBindableCollectionTests
{
		
	[Test]
	public void CreateBindables()
	{
		BdWithBindableCollection<SimpleBusinessObject, BindableWrapper<SimpleBusinessObject>> bindable =
				new BdWithBindableCollection<SimpleBusinessObject,BindableWrapper<SimpleBusinessObject>>
					(new SimpleBusinessObject());
		Assert.IsNotNull(bindable);
	}
		
	[Test]
	public void ChangeProperties()
	{
		BdWithBindableCollection<SimpleBusinessObject, BindableWrapper<SimpleBusinessObject>> bindable =
				new BdWithBindableCollection<SimpleBusinessObject,BindableWrapper<SimpleBusinessObject>>
					(new SimpleBusinessObject());

		Assert.AreEqual(bindable.Data.Collection.Count, 0);
		bindable.Data.Collection.Add(new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject()));
		bindable.Data.Collection[0].Data.BusinessObj.Name = "whatever";
			
		bindable.Data.BusinessObj.Name = "asdf";
		Assert.AreEqual(bindable.Data.BusinessObj.Name, "asdf");
	}
		
	[Test]
	public void UndoRedoProperties()
	{
		BdWithBindableCollection<SimpleBusinessObject, BindableWrapper<SimpleBusinessObject>> bindable =
				new BdWithBindableCollection<SimpleBusinessObject,BindableWrapper<SimpleBusinessObject>>
					(new SimpleBusinessObject());	
		bindable.Data.Collection.Add(new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject()));
		bindable.Data.Collection[0].Data.BusinessObj.Name = "whatever";
		bindable.Data.Collection[0].Data.BusinessObj.Name = "somethingelse";	
		
		bindable.Data.Collection[0].Undo();
			
		Assert.AreEqual(bindable.Data.Collection[0].Data.BusinessObj.Name, "whatever");
			
		bindable.Data.Collection[0].Redo();
			
		Assert.AreEqual(bindable.Data.Collection[0].Data.BusinessObj.Name, "somethingelse");
			
		bindable.Data.BusinessObj.Name = "asdf";
			
		bindable.Data.BusinessObj.Name = "qwerty";
			
		bindable.Undo();
			
		Assert.AreEqual(bindable.Data.BusinessObj.Name, "asdf");
			
		bindable.Redo();
			
		Assert.AreEqual(bindable.Data.BusinessObj.Name, "qwerty");
	}

	[Test]
	public void SerializationTest()
	{
		BdWithBindableCollection<SimpleBusinessObject, BindableWrapper<SimpleBusinessObject>> bindable =
				new BdWithBindableCollection<SimpleBusinessObject, BindableWrapper<SimpleBusinessObject>>
					(new SimpleBusinessObject());

		bindable.Data.BusinessObj.Name = "asdf";
		
		MemoryStream stream = new MemoryStream();
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, bindable);
		stream.Position = 0;

		object deserializedObject = formatter.Deserialize(stream);
		Assert.IsNotNull(deserializedObject);
		BdWithBindableCollection<SimpleBusinessObject, BindableWrapper<SimpleBusinessObject>> deserializedBindable =
			(BdWithBindableCollection<SimpleBusinessObject, BindableWrapper<SimpleBusinessObject>>)deserializedObject;

		Assert.IsNotNull(deserializedBindable);
		Assert.AreEqual(deserializedBindable.Data.BusinessObj.Name, "asdf");
	}
}
	
}
