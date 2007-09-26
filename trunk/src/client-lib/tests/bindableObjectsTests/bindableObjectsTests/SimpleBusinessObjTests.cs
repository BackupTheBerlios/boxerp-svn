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
using System.ComponentModel;


namespace bindableObjectsTests
{

	
[TestFixture]
public class BindableObjectsMain
{
	
	[Test]
	public void CreateBindableSBO()
	{
			BindableWrapper<SimpleBusinessObject> bindableObj =
				new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject());
		
		Assert.IsNotNull(bindableObj);
	}

	[Test]
	public void CreateBindableSOCP()
	{
		BindableWrapper<SimpleObjectComplexProperties> bindableObj =
			new BindableWrapper<SimpleObjectComplexProperties>(new SimpleObjectComplexProperties());

		Assert.IsNotNull(bindableObj);
	}

	[Test]
	public void CreateBindableSOROP()
	{
		BindableWrapper<SimpleObjectReadOnlyProperties> bindableObj =
			new BindableWrapper<SimpleObjectReadOnlyProperties>(new SimpleObjectReadOnlyProperties());

		Assert.IsNotNull(bindableObj);
	}

	[Test]
	public void CreateBindableChangingFlags()
	{
		BindableWrapper<SimpleBusinessObject> bindableObj =
				new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject(), true);

		Assert.IsNotNull(bindableObj);
	}

	[Test]
	public void ChangeBindableSBO()
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
	public void ChangeBindableSOCP()
	{
		BindableWrapper<SimpleObjectComplexProperties> bindableObj =
			new BindableWrapper<SimpleObjectComplexProperties>(new SimpleObjectComplexProperties());
		bindableObj.Data.BusinessObj.Code = "asdf";
		bindableObj.Data.BusinessObj.Ages[2] = 70;
		bindableObj.Data.BusinessObj.Names = new System.Collections.Generic.List<string>();
		bindableObj.Data.BusinessObj.Names.Add("test");

		Assert.AreEqual(bindableObj.Data.BusinessObj.Code, "asdf");
		Assert.AreEqual(bindableObj.Data.BusinessObj.Ages[2], 70);
		Assert.IsTrue(bindableObj.Data.BusinessObj.Names.Contains("test"));
	}
	
	[Test]
	public void UndoChangesSBO()
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
	public void UndoChangesSBONoInterception()
	{
		BindableWrapper<SimpleBusinessObject> bindableObj =
			new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject(), true);
		bindableObj.Data.BusinessObj.Name = "asdf";
		bindableObj.Data.BusinessObj.Description = "asdfsdf";
		bindableObj.Data.BusinessObj.Age = 25;

		bindableObj.Data.BusinessObj.Name = "qwerty";
		Assert.AreEqual(bindableObj.Data.BusinessObj.Name, "qwerty");

		bindableObj.Undo();
		Assert.AreEqual(bindableObj.Data.BusinessObj.Name, "qwerty"); // no changes, no interception
	}

	[Test]
	public void UndoChangesSOCP()
	{
		BindableWrapper<SimpleObjectComplexProperties> bindableObj =
			new BindableWrapper<SimpleObjectComplexProperties>(new SimpleObjectComplexProperties());
		bindableObj.Data.BusinessObj.Code = "asdf";
		bindableObj.Data.BusinessObj.Ages[3] = 777;
		bindableObj.Data.BusinessObj.Names = new System.Collections.Generic.List<string>();
		bindableObj.Data.BusinessObj.Names.Add("test");

		bindableObj.Data.BusinessObj.Code = "change";
		bindableObj.Data.BusinessObj.Ages[3] = 22;
		Assert.AreEqual(bindableObj.Data.BusinessObj.Code, "change");
		Assert.AreEqual(bindableObj.Data.BusinessObj.Ages[3], 22);
		bindableObj.Undo();
		Assert.AreEqual(bindableObj.Data.BusinessObj.Code, "asdf");
		Assert.AreEqual(bindableObj.Data.BusinessObj.Ages[3], 777); // array change is intercepted
		Assert.IsTrue(bindableObj.Data.BusinessObj.Names.Contains("test")); // no undo for this as it is not intercepted
	}
	
	[Test]
	public void UndoRedoChangesSBO()
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
	public void UndoRedoChangesSOBCP()
	{
		BindableWrapper<SimpleObjectComplexProperties> bindableObj =
			new BindableWrapper<SimpleObjectComplexProperties>(new SimpleObjectComplexProperties());
		bindableObj.Data.BusinessObj.Code = "asdf";
		bindableObj.Data.BusinessObj.Ages[1]  = 0;
		bindableObj.Data.BusinessObj.Names = new System.Collections.Generic.List<string>();
		bindableObj.Data.BusinessObj.Names.Add("test");

		bindableObj.Data.BusinessObj.Ages[1] = 80;
		Assert.AreEqual(bindableObj.Data.BusinessObj.Ages[1], 80);

		bindableObj.Undo();

		Assert.AreEqual(bindableObj.Data.BusinessObj.Ages[1], 0);

		bindableObj.Redo();

		Assert.AreEqual(bindableObj.Data.BusinessObj.Ages[1], 80);
	}

	[Test]
	public void SerializationSBOTest()
	{
		BindableWrapper<SimpleBusinessObject> bindableObj =
			new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject());

		MemoryStream stream = new MemoryStream();
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, bindableObj.Data.BusinessObj);
		stream.Position = 0;

		object deserializedObject = formatter.Deserialize(stream);
		Assert.IsNotNull(deserializedObject);
		SimpleBusinessObject deserializedBindable = (SimpleBusinessObject)deserializedObject;

		Assert.IsNotNull(deserializedBindable);
	}

	[Test]
	public void SerializationSOCPTest()
	{
		BindableWrapper<SimpleObjectComplexProperties> bindableObj =
			new BindableWrapper<SimpleObjectComplexProperties>(new SimpleObjectComplexProperties());
		
		MemoryStream stream = new MemoryStream();
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, bindableObj.Data.BusinessObj);
		stream.Position = 0;

		object deserializedObject = formatter.Deserialize(stream);
		Assert.IsNotNull(deserializedObject);
		SimpleObjectComplexProperties deserializedBindable = (SimpleObjectComplexProperties)deserializedObject;

		Assert.IsNotNull(deserializedBindable);
	}

	[Test]
	public void SerializationCastleDynamicProxy2()
	{
		Castle.DynamicProxy.ProxyGenerator generator = new Castle.DynamicProxy.ProxyGenerator();
		SimpleBusinessObject proxy = (SimpleBusinessObject)
			generator.CreateClassProxy(typeof(SimpleBusinessObject), new Castle.Core.Interceptor.IInterceptor[0]);
		MemoryStream stream = new MemoryStream();
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, proxy);
		stream.Position = 0;
		object deserializedObject = formatter.Deserialize(stream);
		Assert.IsNotNull(deserializedObject);
	}

	[Test]
	public void SerializationBWSBOTest()
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

	[Test]
	public void SerializationBWSOCPTest()
	{
		BindableWrapper<SimpleObjectComplexProperties> bindableObj =
			new BindableWrapper<SimpleObjectComplexProperties>(new SimpleObjectComplexProperties());
		
		MemoryStream stream = new MemoryStream();
		BinaryFormatter formatter = new BinaryFormatter();
		formatter.Serialize(stream, bindableObj);
		stream.Position = 0;

		object deserializedObject = formatter.Deserialize(stream);
		Assert.IsNotNull(deserializedObject);
		BindableWrapper<SimpleObjectComplexProperties> deserializedBindable = (BindableWrapper<SimpleObjectComplexProperties>)deserializedObject;

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

	[Test]
	public void DoubleProxyTest()
	{
		Type generatedType =
				DynamicPropertyChangedProxy.CreateINotifyPropertyChangedTypeProxy(
					typeof(SimpleBusinessObject), new Type[0]);
		Castle.DynamicProxy.ProxyGenerator generator = new Castle.DynamicProxy.ProxyGenerator();
		SimpleBusinessObject proxy = (SimpleBusinessObject)
			generator.CreateClassProxy(generatedType, new Castle.Core.Interceptor.IInterceptor[0]);
	}

	[Test]
	public void PropertyChangedSubscriberTest()
	{
		BindableWrapper<SimpleBusinessObject> bindableObj =
		new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject());

		bindableObj.PropertyChanged += onPropertyChanged;

		bindableObj.Data.BusinessObj.Name = "change1";
		bindableObj.Data.BusinessObj.Name = "change2";
	}

	[Test]
	public void InternalPropertyChangedSubscriberTest()
	{
		BindableWrapper<SimpleBusinessObject> bindableObj =
		new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject());

		bindableObj.Data.BusinessObjBinding.PropertyChanged += onPropertyChanged;

		bindableObj.Data.BusinessObj.Name = "change1";
		bindableObj.Data.BusinessObj.Name = "change2";
		Assert.IsTrue(((ICustomNotifyPropertyChanged)bindableObj.Data.BusinessObj).HasSubscribers());
	}

	private void onPropertyChanged(Object sender, PropertyChangedEventArgs args)
	{
		Console.Out.WriteLine("Changed: " + args.PropertyName);
	}
}

}