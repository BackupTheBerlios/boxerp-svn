// /home/carlos/boxerp_completo/trunk/src/client-lib/tests/bindableObjectsTests/bindableObjectsTests/MyClass.cs created with MonoDevelop
// User: carlos at 3:15 PM 7/7/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
// project created on 7/7/2007 at 3:15 PM
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Boxerp.Client;
using NUnit.Framework;
using System.Reflection;

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

	[Test]
	public void SerializationTest()
	{
		BindableWrapper<SimpleBusinessObject> bindableObj =
			new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject());
		
		MemoryStream stream = new MemoryStream();
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, bindableObj);
		stream.Position = 0;

		object deserializedObject = formatter.Deserialize(stream);
		Assert.IsNotNull(deserializedObject);
		BindableWrapper<SimpleBusinessObject> deserializedBindable = (BindableWrapper<SimpleBusinessObject>)deserializedObject;

		Assert.IsNotNull(deserializedBindable);
	}

	/// <summary>
	/// Show how the references to the business object are affected when it is wrapped into a bindable and also how the references to the bindable
	/// work. As you may see the business object is not updated but the bindable object that wraps it. 
	/// </summary>
	[Test]
	public void ObjectReferencesOnUndo()
	{
		BindableWrapper<SimpleBusinessObject> bindableReference;
		SimpleBusinessObject businessObjReference;

		SimpleBusinessObject businessObj = new SimpleBusinessObject();
		businessObjReference = businessObj;

		BindableWrapper<SimpleBusinessObject> bindableObj =
			new BindableWrapper<SimpleBusinessObject>(businessObj);
		bindableReference = bindableObj;


		bindableObj.Data.BusinessObj.Name = "asdf";
		bindableObj.Data.BusinessObj.Description = "asdfsdf";
		bindableObj.Data.BusinessObj.Age = 25;

		bindableObj.Data.BusinessObj.Name = "qwerty";
		
		Assert.AreEqual(bindableObj.Data.BusinessObj.Name, "qwerty");
		Assert.AreEqual(bindableReference.Data.BusinessObj.Name, "qwerty");
		Assert.IsNull(businessObjReference.Name);

		bindableObj.Undo();

		Assert.AreEqual(bindableObj.Data.BusinessObj.Name, "asdf");
		Assert.AreEqual(bindableReference.Data.BusinessObj.Name, "asdf");
		Assert.IsNull(businessObjReference.Name);

	}

	[Test]
	public void SerializationWithEventSubscriberTest()
	{
		BindableWrapper<SimpleBusinessObject> bindableObj =
			new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject());
		
		AnySubscriber subscriber = new AnySubscriber();
		bindableObj.PropertyChanged += subscriber.OnPropertyChanged;

		Assert.IsTrue(bindableObj.HasSubscribers);

		MemoryStream stream = new MemoryStream();
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, bindableObj);
		stream.Position = 0;

		object deserializedObject = formatter.Deserialize(stream);
		Assert.IsNotNull(deserializedObject);
		BindableWrapper<SimpleBusinessObject> deserializedBindable = (BindableWrapper<SimpleBusinessObject>)deserializedObject;

		Assert.IsNotNull(deserializedBindable);

		Assert.IsFalse(deserializedBindable.HasSubscribers);
	}

	[Test]
	public void DynamicINotifyPropertyChangedProxy_TEST1()
	{
		Type generatedType =
				DynamicPropertyChangedProxy.CreateINotifyPropertyChangedTypeProxy(
					typeof(SimpleBusinessObject), new Type[0]);
		foreach (MemberInfo member in generatedType.GetMembers())
		{
			Console.WriteLine("-- {0} {1};", member.MemberType, member.Name);
		}

		ICustomNotifyPropertyChanged myObject = (ICustomNotifyPropertyChanged) Activator.CreateInstance(generatedType);
		
		Assert.IsTrue(true);
	}

	[Test]
	public void CastleDynamicProxy2Test()
	{
		Castle.DynamicProxy.ProxyGenerator generator = new Castle.DynamicProxy.ProxyGenerator();
		SimpleExplicitPropChanged proxy = (SimpleExplicitPropChanged)
			generator.CreateClassProxy(typeof(SimpleExplicitPropChanged), new Castle.Core.Interceptor.IInterceptor[0]);
	}

}

}