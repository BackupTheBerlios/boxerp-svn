// /home/carlos/boxerp_completo/trunk/src/client-lib/tests/bindableObjectsTests/bindableObjectsTests/MyClass.cs created with MonoDevelop
// User: carlos at 3:15 PM 7/7/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
// project created on 7/7/2007 at 3:15 PM
using System;
using Boxerp.Client;
using NUnit.Framework;

namespace bindableObjectsTests
{

	
[TestFixture]
public class BindableObjectsMain
{
	
	[Test]
	public void CreateBindable()
	{
		BindableWrapper<SimpleBusinessObject> bindableObj = 
			new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject());
		Assert.IsNotNull(bindableObj);
	}

	[Test]
	public void ChangeBindable()
	{
		BindableWrapper<SimpleBusinessObject> bindableObj =	
			new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject());
		bindableObj.Data.BusinessObj.Name = "asdf";
		bindableObj.Data.BusinessObj.Description = "asdfsdf";
		bindableObj.Data.BusinessObj.Age = 25;
		
		Assert.AreEqual(bindableObj.Data.BusinessObj.Name, "asdf");
		Assert.AreEqual(bindableObj.Data.BusinessObj.Age, 25);
	}
	
	[Test]
	public void UndoChanges()
	{
		BindableWrapper<SimpleBusinessObject> bindableObj =	
			new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject());
		bindableObj.Data.BusinessObj.Name = "asdf";
		bindableObj.Data.BusinessObj.Description = "asdfsdf";
		bindableObj.Data.BusinessObj.Age = 25;
		
		bindableObj.Data.BusinessObj.Name = "qwerty";
		Assert.AreEqual(bindableObj.Data.BusinessObj.Name, "qwerty");
		
		bindableObj.Undo();
		Assert.AreEqual(bindableObj.Data.BusinessObj.Name, "asdf");
	}
	
	[Test]
	public void UndoRedoChanges()
	{
		BindableWrapper<SimpleBusinessObject> bindableObj =	
			new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject());
		bindableObj.Data.BusinessObj.Name = "asdf";
		bindableObj.Data.BusinessObj.Description = "asdfsdf";
		bindableObj.Data.BusinessObj.Age = 25;
		
		bindableObj.Data.BusinessObj.Name = "qwerty";
		Assert.AreEqual(bindableObj.Data.BusinessObj.Name, "qwerty");
		
		bindableObj.Undo();
		Assert.AreEqual(bindableObj.Data.BusinessObj.Name, "asdf");
		
		bindableObj.Redo();
		Assert.AreEqual(bindableObj.Data.BusinessObj.Name, "qwerty");
	}	
	

}

}