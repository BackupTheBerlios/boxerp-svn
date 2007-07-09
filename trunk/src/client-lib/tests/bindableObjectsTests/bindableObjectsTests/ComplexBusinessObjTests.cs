// /home/carlos/boxerp_completo/trunk/src/client-lib/tests/bindableObjectsTests/bindableObjectsTests/ComplexBusinessObjTests.cs created with MonoDevelop
// User: carlos at 7:19 PM 7/7/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
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
		SimpleBusinessObject sbo2 = new SimpleBusinessObject();	
		bindableObj.Data.BusinessObj.NestedObject = sbo1;
		
		Assert.AreEqual(bindableObj.Data.BusinessObj.NestedObject, sbo1);
		
		bindableObj.Data.BusinessObj.NestedObject = sbo2;
		Assert.AreEqual(bindableObj.Data.BusinessObj.NestedObject, sbo2);
			
		bindableObj.Undo();
		Assert.AreEqual(bindableObj.Data.BusinessObj.NestedObject, sbo1);
		
		// the nested object is not IBindableWrapper so changes on its internal properties are not intercepted
		bindableObj.Data.BusinessObj.NestedObject.Name = "whatever";
		bindableObj.Data.BusinessObj.NestedObject.Name = "somethingelse";
			
		bindableObj.Undo();
			
		Assert.IsNull(bindableObj.Data.BusinessObj.NestedObject);	
	}
	
	[Test]
	public void UndoRedoNestedChanges()
	{
		BindableWrapper<ComplexBusinessObject> bindableObj =	
			new BindableWrapper<ComplexBusinessObject>(new ComplexBusinessObject());
		
		SimpleBusinessObject sbo1 = new SimpleBusinessObject();
		SimpleBusinessObject sbo2 = new SimpleBusinessObject();	
		bindableObj.Data.BusinessObj.NestedObject = sbo1;
		
		Assert.AreEqual(bindableObj.Data.BusinessObj.NestedObject, sbo1);
		
		bindableObj.Data.BusinessObj.NestedObject = sbo2;
		Assert.AreEqual(bindableObj.Data.BusinessObj.NestedObject, sbo2);
			
		bindableObj.Undo();
		Assert.AreEqual(bindableObj.Data.BusinessObj.NestedObject, sbo1);
			
		bindableObj.Redo();
		Assert.AreEqual(bindableObj.Data.BusinessObj.NestedObject, sbo2);
	}

}
}
