// /home/carlos/boxerp_completo/trunk/src/client-lib/tests/bindableObjectsTests/bindableObjectsTests/NestedCollectionBusinessObjTests.cs created with MonoDevelop
// User: carlos at 7:33 PM 7/7/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Boxerp.Client;
using NUnit.Framework;

namespace bindableObjectsTests
{
	
[TestFixture]
public class NestedCollectionBusinessObjTests
{
		
	[Test]
	public void CreateBindable()
	{
			BindableWithCollection<SimpleBusinessObject, SimpleBusinessObject> 
				bindableWithCollection = new BindableWithCollection<SimpleBusinessObject,SimpleBusinessObject>
					(new SimpleBusinessObject());
			Assert.IsNotNull(bindableWithCollection);
	}
		
	[Test]
	public void ChangeProperties()
	{
			Boxerp.Client.BindableWithCollection<SimpleBusinessObject, SimpleBusinessObject> 
				bindableWithCollection = new BindableWithCollection<SimpleBusinessObject,SimpleBusinessObject>
					(new SimpleBusinessObject());
			
			bindableWithCollection.Data.Collection.Add(
				new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject()));
			bindableWithCollection.Data.Collection[0].Data.BusinessObj.Name = "whatever";
			bindableWithCollection.Data.BusinessObj.Name = "asdf";
			Assert.AreEqual(bindableWithCollection.Data.BusinessObj.Name, "asdf");
	}
	
	[Test]
	public void UndoRedoProperties()
	{
			Boxerp.Client.BindableWithCollection<SimpleBusinessObject, SimpleBusinessObject> 
				bindableWithCollection = new BindableWithCollection<SimpleBusinessObject,SimpleBusinessObject>
					(new SimpleBusinessObject());
			
			bindableWithCollection.Data.Collection.Add(
				new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject()));
			
			bindableWithCollection.Data.Collection[0].Data.BusinessObj.Name = "whatever";
			
			bindableWithCollection.Data.Collection[0].Data.BusinessObj.Name = "somethingelse";
			
			bindableWithCollection.Undo();
			
			Assert.AreEqual(bindableWithCollection.Data.Collection[0].Data.BusinessObj.Name, "somethingelse");
			
			bindableWithCollection.Redo();
			
			Assert.AreEqual(bindableWithCollection.Data.Collection[0].Data.BusinessObj.Name, "somethingelse");
			
			bindableWithCollection.Data.BusinessObj.Name = "asdf";
			
			bindableWithCollection.Data.BusinessObj.Name = "qwerty";
			
			bindableWithCollection.Undo();
			
			Assert.AreEqual(bindableWithCollection.Data.BusinessObj.Name, "asdf");
			
			bindableWithCollection.Redo();
			
			Assert.AreEqual(bindableWithCollection.Data.BusinessObj.Name, "qwerty");
	}
		
	
}
	
}
