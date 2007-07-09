// /home/carlos/boxerp_completo/trunk/src/client-lib/tests/bindableObjectsTests/bindableObjectsTests/NestedBindableWrapperTests.cs created with MonoDevelop
// User: carlos at 10:52 PM 7/7/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Boxerp.Client;
using NUnit.Framework;


namespace bindableObjectsTests
{
	
[TestFixture]
public class NestedBindableWrapperTests
{
		
	[Test]
	public void CreateBindables()
	{
		BdWithNestedWrapper<SimpleBusinessObject, BindableWrapper<SimpleBusinessObject>> bindable =
				new BdWithNestedWrapper<SimpleBusinessObject,BindableWrapper<SimpleBusinessObject>>
					(new SimpleBusinessObject(), new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject()));
		Assert.IsNotNull(bindable);
	}
		
	[Test]
	public void ChangeProperties()
	{
		BdWithNestedWrapper<SimpleBusinessObject, BindableWrapper<SimpleBusinessObject>> bindable =
				new BdWithNestedWrapper<SimpleBusinessObject,BindableWrapper<SimpleBusinessObject>>
					(new SimpleBusinessObject(), new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject()));
		
			
		bindable.Data.NestedWrapper.Data.BusinessObj.Name = "asdf";
		Assert.AreEqual(bindable.Data.NestedWrapper.Data.BusinessObj.Name, "asdf");
			
		
	}
		
	[Test]
	public void UndoRedoProperties()
	{
		BdWithNestedWrapper<SimpleBusinessObject, BindableWrapper<SimpleBusinessObject>> bindable =
				new BdWithNestedWrapper<SimpleBusinessObject,BindableWrapper<SimpleBusinessObject>>
					(new SimpleBusinessObject(), new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject()));
			
		
		bindable.Data.NestedWrapper.Data.BusinessObj.Name = "whatever";
		bindable.Data.NestedWrapper.Data.BusinessObj.Name = "somethingelse";	
		
		bindable.Undo();
			
		Assert.AreEqual(bindable.Data.NestedWrapper.Data.BusinessObj.Name, "whatever");
			
		bindable.Redo();
			
		Assert.AreEqual(bindable.Data.NestedWrapper.Data.BusinessObj.Name, "somethingelse");
			
		bindable.Data.BusinessObj.Name = "asdf";
			
		bindable.Data.BusinessObj.Name = "qwerty";
			
		bindable.Undo();
			
		Assert.AreEqual(bindable.Data.BusinessObj.Name, "asdf");
			
		bindable.Redo();
			
		Assert.AreEqual(bindable.Data.BusinessObj.Name, "qwerty");
	}
}
	
}
