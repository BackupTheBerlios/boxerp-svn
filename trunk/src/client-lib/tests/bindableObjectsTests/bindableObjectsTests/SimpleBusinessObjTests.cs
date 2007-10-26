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
using System.Runtime.Serialization;
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
	public void CreateBindableSOCPWithData()
	{
		SimpleObjectComplexProperties bocp = new SimpleObjectComplexProperties();
		bocp.Names = new System.Collections.Generic.List<string>();
		bocp.Names.Add("name1");
		bocp.Names.Add("name2");

		BindableWrapper<SimpleObjectComplexProperties> bindableObj =
			new BindableWrapper<SimpleObjectComplexProperties>(bocp);

		Assert.IsNotNull(bindableObj);

		Assert.AreEqual(bindableObj.Data.BusinessObj.Ages.Length, 6);
		Assert.AreEqual(bindableObj.Data.BusinessObj.Ages[0], 10);
		Assert.AreEqual(bindableObj.Data.BusinessObj.Names.Count, 2);
		Assert.AreEqual(bindableObj.Data.BusinessObj.Names[0], "name1");
	}

	[Test]
	public void CreateBindableSOROP()
	{
		BindableWrapper<SimpleObjectReadOnlyProperties> bindableObj =
			new BindableWrapper<SimpleObjectReadOnlyProperties>(new SimpleObjectReadOnlyProperties());

		Assert.IsNotNull(bindableObj);
	}

	[Test]
	public void CreateBindableSIC()
	{
		BindableWrapper<SimpleIndexedClass> bindableObj =
			new BindableWrapper<SimpleIndexedClass>(new SimpleIndexedClass());

		Assert.IsNotNull(bindableObj);
	}

	[Test]
	public void CreateBindableSGO()
	{
		BindableWrapper<SimpleGenericObject<int>> bindableObj =
			new BindableWrapper<SimpleGenericObject<int>>(new SimpleGenericObject<int>());

		Assert.IsNotNull(bindableObj);
	}

	[Test]
	public void CreateBindableSAS()
	{
		SimpleAutoSerializable businessObj = new SimpleAutoSerializable();

		ConstructorInfo serializationConstructor = businessObj.GetType().GetConstructor(
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
					null,
					new Type[] { typeof(SerializationInfo), typeof(StreamingContext) },
					null);
		Assert.IsNotNull(serializationConstructor);

		BindableWrapper<SimpleAutoSerializable> bindableObj =
			new BindableWrapper<SimpleAutoSerializable>(businessObj);

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
		System.Diagnostics.Debug.WriteLine("asdfasdfsdf");
		BindableWrapper<SimpleBusinessObject> bindableObj =	
			new BindableWrapper<SimpleBusinessObject>(new SimpleBusinessObject());
		bindableObj.Data.BusinessObj.Name = "asdf";
		bindableObj.Data.BusinessObj.Description = "asdfsdf";
		bindableObj.Data.BusinessObj.Age = 25;
		
		Assert.AreEqual(bindableObj.Data.BusinessObj.Name, "asdf");
		Assert.AreEqual(bindableObj.Data.BusinessObj.Age, 25);
	}

	[Test]
	public void ChangeBindableSIC()
	{
		BindableWrapper<SimpleIndexedClass> bindableObj =
			new BindableWrapper<SimpleIndexedClass>(new SimpleIndexedClass());

		bindableObj.Data.BusinessObj.Code = "code";
		bindableObj.Data.BusinessObj["test"] = 1;
		Assert.AreEqual(bindableObj.Data.BusinessObj["test"], 1);
		bindableObj.Data.BusinessObj["test"] = 2;
		Assert.AreEqual(bindableObj.Data.BusinessObj["test"], 2);
	}

	[Test]
	public void ChangeBindableSGO()
	{
		BindableWrapper<SimpleGenericObject<string>> bindableObj =
			new BindableWrapper<SimpleGenericObject<string>>(new SimpleGenericObject<string>());

		bindableObj.Data.BusinessObj.Code = "code";
		bindableObj.Data.BusinessObj.GenericArray = new string[] { "test1", "test2"};
		Assert.AreEqual(bindableObj.Data.BusinessObj.GenericArray[0], "test1");
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
	public void UndoChangesSOVT()
	{
		BindableWrapper<SimpleObjectValueTypes> bindableObj =
			new BindableWrapper<SimpleObjectValueTypes>(new SimpleObjectValueTypes());
		
		bindableObj.Data.BusinessObj.Boolean = false;
		bindableObj.Data.BusinessObj.Char = 'a';
		bindableObj.Data.BusinessObj.Double = 100;
		bindableObj.Data.BusinessObj.Float = 100;
		bindableObj.Data.BusinessObj.Enum = SimpleEnum.line1;
		bindableObj.Data.BusinessObj.Integer = 5;
		bindableObj.Data.BusinessObj.Struct = new SimpleStruct();
		
		Assert.AreEqual(bindableObj.Data.BusinessObj.Boolean, false);
		Assert.AreEqual(bindableObj.Data.BusinessObj.Char, 'a');
		Assert.AreEqual(bindableObj.Data.BusinessObj.Double, 100);
		Assert.AreEqual(bindableObj.Data.BusinessObj.Float, 100);
		Assert.AreEqual(bindableObj.Data.BusinessObj.Enum, SimpleEnum.line1);
		Assert.AreEqual(bindableObj.Data.BusinessObj.Integer, 5);
		//Assert.AreEqual(bindableObj.Data.BusinessObj.Struct._int1, 1);

		bindableObj.Data.BusinessObj.Boolean = true;
		bindableObj.Undo();
		Assert.AreEqual(bindableObj.Data.BusinessObj.Boolean, false);

		bindableObj.Data.BusinessObj.Char = 'b';
		bindableObj.Undo();
		Assert.AreEqual(bindableObj.Data.BusinessObj.Char, 'a');

		bindableObj.Data.BusinessObj.Double = 200;
		bindableObj.Undo();
		Assert.AreEqual(bindableObj.Data.BusinessObj.Double, 100);

		bindableObj.Data.BusinessObj.Float = 200;
		bindableObj.Undo();
		Assert.AreEqual(bindableObj.Data.BusinessObj.Float, 100);

		bindableObj.Data.BusinessObj.Enum = SimpleEnum.line2;
		bindableObj.Undo();
		Assert.AreEqual(bindableObj.Data.BusinessObj.Enum, SimpleEnum.line1);

		bindableObj.Data.BusinessObj.Integer = 7;
		bindableObj.Undo();
		Assert.AreEqual(bindableObj.Data.BusinessObj.Integer, 5);
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
		// array is not intercepted so it does not change
		Assert.AreEqual(bindableObj.Data.BusinessObj.Ages[3], 22);
		Assert.IsTrue(bindableObj.Data.BusinessObj.Names.Contains("test")); // no undo for this as it is not intercepted

		bindableObj.Data.BusinessObj.Ages[3] = 22;
		bindableObj.Data.BusinessObj.Ages[3] = 25;
		bindableObj.Undo();
		Assert.AreEqual(bindableObj.Data.BusinessObj.Ages[3], 25); // not intercepted so nothing to undo
	}

	[Test]
	public void UndoChangesSIC()
	{
		BindableWrapper<SimpleIndexedClass> bindableObj =
			new BindableWrapper<SimpleIndexedClass>(new SimpleIndexedClass());
		
		bindableObj.Data.BusinessObj["test"] = 1;

		bindableObj.Data.BusinessObj["test"] = 2;

		Assert.AreEqual(bindableObj.Data.BusinessObj["test"], 2);

		bindableObj.Undo();

		// changes in the objects within the collection are not intercepted so no undo changes
		Assert.AreEqual(bindableObj.Data.BusinessObj["test"], 2);
	}

	[Test]
	public void UndoChangesSGO()
	{
		BindableWrapper<SimpleGenericObject<string>> bindableObj =
			new BindableWrapper<SimpleGenericObject<string>>(new SimpleGenericObject<string>());

		bindableObj.Data.BusinessObj.GenericArray = new string[] { "test1", "test2" };
		bindableObj.Data.BusinessObj.GenericArray[0] = "test3";
		bindableObj.Data.BusinessObj.Code = "code1";
		bindableObj.Data.BusinessObj.Code = "code2";
		Assert.AreEqual(bindableObj.Data.BusinessObj.Code, "code2");
		bindableObj.Undo();
		Assert.AreEqual(bindableObj.Data.BusinessObj.Code, "code1");
		// changes in array elements are not intercepted
		Assert.AreEqual(bindableObj.Data.BusinessObj.GenericArray[0], "test3");
	}

	[Test]
	public void UndoChangesSGO_CastleDynProxy()
	{
		Castle.DynamicProxy.ProxyGenerator generator = new Castle.DynamicProxy.ProxyGenerator();
		SimpleGenericObject<string> proxy = (SimpleGenericObject<string>)
			generator.CreateClassProxy(typeof(SimpleGenericObject<string>), new Castle.Core.Interceptor.IInterceptor[0]);
		proxy.GenericArray = new string[] { "test1", "test2" };
		proxy.GenericArray[0] = "test3";
		proxy.Code = "code1";
		Assert.AreEqual(proxy.Code, "code1");
		Assert.AreEqual(proxy.GenericArray[0], "test3");
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

		Assert.AreEqual(bindableObj.Data.BusinessObj.Ages[1], 80);

		bindableObj.Redo();

		Assert.AreEqual(bindableObj.Data.BusinessObj.Ages[1], 80);
	}

	[Test]
	public void UndoRedoChangesSIC()
	{
		BindableWrapper<SimpleIndexedClass> bindableObj =
			new BindableWrapper<SimpleIndexedClass>(new SimpleIndexedClass());

		bindableObj.Data.BusinessObj["test"] = 1;

		bindableObj.Data.BusinessObj["test"] = 2;

		bindableObj.Data.BusinessObj.Code = "asdf";
		bindableObj.Data.BusinessObj.Code = "12345";

		bindableObj.Undo();

		Assert.AreEqual(bindableObj.Data.BusinessObj["test"], 2);
		Assert.AreEqual(bindableObj.Data.BusinessObj.Code, "asdf");

		bindableObj.Redo();
		
		Assert.AreEqual(bindableObj.Data.BusinessObj["test"], 2);
		Assert.AreEqual(bindableObj.Data.BusinessObj.Code, "12345");
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
				DynamicPropertyChangedProxy.CreateBusinessObjectProxy(
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
				DynamicPropertyChangedProxy.CreateBusinessObjectProxy(
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