using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Reflection.Emit;
using System.ComponentModel;
using System.Runtime.Serialization;

// doc on Reflection.Emit:
// http://msdn2.microsoft.com/en-us/library/system.reflection.emit.constructorbuilder.aspx

namespace Boxerp.Client
{
	public static partial class DynamicPropertyChangedProxy
	{
		/// <summary>
		/// In case the base class implements ISerializable, the method should be like this:
		/// public override void GetObjectData(SerializationInfo info, StreamingContext context)
		/// {
		///	   base.GetObjectData(info, context);
		/// }
		/// Otherwise the signature should be this:
		/// public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		/// 
		/// Body: See the hardcoded proxy at the bindableObjectTests
		///   
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="baseType"></param>
		private static void getObjectDataMethod(TypeBuilder builder, Type baseType,
			FieldBuilder args4constructorField, FieldBuilder baseTypeField, FieldBuilder isBusinessObjField,
			FieldBuilder valuesForConstructorField)
		{
			MethodBuilder method;
			bool inheritsSerializable = baseType.IsAssignableFrom(typeof(ISerializable));

			// if the base type already implements the GetObjectData we should override it and it must be virtual
			if (inheritsSerializable)
			{
				MethodInfo baseGetObjectData = baseType.GetMethod("GetObjectData");
				if (!baseGetObjectData.IsVirtual || baseGetObjectData.IsFinal)
				{
					String message = String.Format("The type {0} implements ISerializable, but GetObjectData is not marked as virtual",
												   baseType.FullName);
					throw new ArgumentException(message);
				}

				method = builder.DefineMethod(
							   "GetObjectData",
							   MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Virtual,
							   typeof(void),
							   new Type[] { typeof(System.Runtime.Serialization.SerializationInfo), 
											typeof(System.Runtime.Serialization.StreamingContext)} );
			}
			else
			{
				method = builder.DefineMethod(
							   "GetObjectData",
							   MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual,
							   typeof(void),
							   new Type[] { typeof(System.Runtime.Serialization.SerializationInfo), 
											typeof(System.Runtime.Serialization.StreamingContext)});
			}

			ILGenerator mthdIL = method.GetILGenerator();

			LocalBuilder localDynamicProxyHelperType = mthdIL.DeclareLocal(typeof(Type));
			LocalBuilder localMembers = mthdIL.DeclareLocal(typeof(MemberInfo[]));
			LocalBuilder localData = mthdIL.DeclareLocal(typeof(object[]));

			// Tell the formatter that we want to Serialize the DynamicProxyHelper and not the proxy.
			// In the deserialization the DynamicProxyHelper will recreate the proxy and set its values because 
			// they are already contained in the SerializationInfo
			mthdIL.Emit(OpCodes.Nop);
			mthdIL.Emit(OpCodes.Ldarg_1);
			mthdIL.Emit(OpCodes.Ldtoken, typeof(DynamicProxyHelper));
			mthdIL.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) }));
			mthdIL.Emit(OpCodes.Callvirt, typeof(SerializationInfo).GetMethod("SetType", new Type[] { typeof(Type) }));
			mthdIL.Emit(OpCodes.Nop);

			// Now serialize the static fields needed for the DynamicProxyHelper
			mthdIL.Emit(OpCodes.Ldarg_1);
			mthdIL.Emit(OpCodes.Ldstr, ARGUMENTS_TYPES4CONSTRUCTOR);
			mthdIL.Emit(OpCodes.Ldsfld, args4constructorField);
			mthdIL.Emit(OpCodes.Callvirt,
				typeof(SerializationInfo).GetMethod("AddValue", new Type[] { typeof(string), typeof(Type[]) }));
			mthdIL.Emit(OpCodes.Nop);
			mthdIL.Emit(OpCodes.Ldarg_1);
			mthdIL.Emit(OpCodes.Ldstr, OBJECT_BASE_TYPE);
			mthdIL.Emit(OpCodes.Ldsfld, baseTypeField);
			mthdIL.Emit(OpCodes.Callvirt,
				typeof(SerializationInfo).GetMethod("AddValue", new Type[] { typeof(string), typeof(string) }));
			mthdIL.Emit(OpCodes.Nop);
			mthdIL.Emit(OpCodes.Ldarg_1);
			mthdIL.Emit(OpCodes.Ldstr, IS_BUSINESS_OBJECT);
			mthdIL.Emit(OpCodes.Ldsfld, isBusinessObjField);
			mthdIL.Emit(OpCodes.Callvirt,
				typeof(SerializationInfo).GetMethod("AddValue", new Type[] { typeof(string), typeof(bool) }));
			mthdIL.Emit(OpCodes.Nop);
			mthdIL.Emit(OpCodes.Ldarg_1);
			mthdIL.Emit(OpCodes.Ldstr, VALUES4CONSTRUCTOR);
			mthdIL.Emit(OpCodes.Ldsfld, valuesForConstructorField);
			mthdIL.Emit(OpCodes.Callvirt,
				typeof(SerializationInfo).GetMethod("AddValue", new Type[] { typeof(string), typeof(object[]) }));
			

			if (inheritsSerializable)
			{
				// C# : base.GetObjectData(info, context)
				mthdIL.Emit(OpCodes.Nop);	// IL_0000
				mthdIL.Emit(OpCodes.Ldarg_0);// IL_0001
				mthdIL.Emit(OpCodes.Ldarg_1);// IL_0002
				mthdIL.Emit(OpCodes.Ldarg_2);// IL_0003
				mthdIL.Emit(OpCodes.Call, baseType.GetMethod("GetObjectData", new Type[] { typeof(SerializationInfo), typeof(StreamingContext) })); // IL_0004
				mthdIL.Emit(OpCodes.Nop);
			}
			else
			{
				mthdIL.Emit(OpCodes.Nop); 

				mthdIL.Emit(OpCodes.Ldarg_0);
				mthdIL.Emit(OpCodes.Call, typeof(object).GetMethod("GetType", new Type[0]));
				mthdIL.Emit(OpCodes.Call, typeof(FormatterServices).GetMethod("GetSerializableMembers", new Type[] { typeof(Type) }));
				mthdIL.Emit(OpCodes.Stloc_1);
				mthdIL.Emit(OpCodes.Ldarg_0);
				mthdIL.Emit(OpCodes.Ldloc_1);
				mthdIL.Emit(OpCodes.Call, typeof(FormatterServices).GetMethod("GetObjectData", new Type[] { typeof(object), typeof(MemberInfo[]) }));
				mthdIL.Emit(OpCodes.Stloc_2);
				mthdIL.Emit(OpCodes.Ldarg_1);
				mthdIL.Emit(OpCodes.Ldstr, SERIALIZED_DATA);
				mthdIL.Emit(OpCodes.Ldloc_2);
				mthdIL.Emit(OpCodes.Callvirt, typeof(SerializationInfo).GetMethod("AddValue", new Type[] { typeof(string), typeof(object) })); 
				 
				mthdIL.Emit(OpCodes.Nop);
			}
			
			mthdIL.Emit(OpCodes.Ret); 
		}

		/// <summary>
		/// The code is different if the class already inherits from a class that implemets ISerializable:
		/// 
		/// protected SimplePropertyChangedImplementation(SerializationInfo info, StreamingContext c)	
		///	: base(info, c)	
		/// 
		/// Otherwise there is no need for calling the base. There is a sample of the C# generated code
		/// in the client-lib/tests/bindableObjectTests. The class is :
		///    SimpleRealHardcoded_CompleteBoxerpProxy
		/// 
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="baseType"></param>
		public static void createDeserializationConstructor(TypeBuilder builder, Type baseType, Type[] constructorParamsTypes)
		{
			bool inheritsSerializable = baseType.IsAssignableFrom(typeof(ISerializable));

			ConstructorBuilder targetCtor = builder.DefineConstructor(
				  MethodAttributes.Family | MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName,
				  CallingConventions.Standard, new Type[] { typeof(SerializationInfo), typeof(StreamingContext) });
			
			ILGenerator ctorIL = targetCtor.GetILGenerator();
			
			if (inheritsSerializable)
			{
				ConstructorInfo baseConstructor = baseType.GetConstructor(
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
					null,
					new Type[] { typeof(SerializationInfo), typeof(StreamingContext) },
					null);

				ctorIL.Emit(OpCodes.Ldarg_0);
				ctorIL.Emit(OpCodes.Ldarg_1);
				ctorIL.Emit(OpCodes.Ldarg_2);
				ctorIL.Emit(OpCodes.Call, baseConstructor);
				ctorIL.Emit(OpCodes.Nop);
				ctorIL.Emit(OpCodes.Nop);
			}
			else
			{
				/*LocalBuilder localData = ctorIL.DeclareLocal(typeof(object[]));
				LocalBuilder localMembers = ctorIL.DeclareLocal(typeof(MemberInfo[]));
				
				ctorIL.Emit(OpCodes.Ldarg_0);
				if (constructorParamsTypes == null)
				{
					ctorIL.Emit(OpCodes.Call, baseType.GetConstructor(new Type[0]));
				}
				else
				{
					ctorIL.Emit(OpCodes.Call, baseType.GetConstructor(constructorParamsTypes));
				}
				ctorIL.Emit(OpCodes.Nop);

				ctorIL.Emit(OpCodes.Nop);

				ctorIL.Emit(OpCodes.Ldarg_1);
				ctorIL.Emit(OpCodes.Ldstr, SERIALIZED_DATA);
				ctorIL.Emit(OpCodes.Ldtoken, typeof(object[]));
				ctorIL.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle", new Type[] { typeof(RuntimeTypeHandle) }));
				ctorIL.Emit(OpCodes.Callvirt, typeof(SerializationInfo).GetMethod("GetValue", new Type[] { typeof(string), typeof(Type) }));
				ctorIL.Emit(OpCodes.Castclass, typeof(object[]));
				ctorIL.Emit(OpCodes.Stloc_0);
				ctorIL.Emit(OpCodes.Ldarg_0);
				ctorIL.Emit(OpCodes.Call, typeof(object).GetMethod("GetType", new Type[0]));
				ctorIL.Emit(OpCodes.Call, typeof(FormatterServices).GetMethod("GetSerializableMembers", new Type[] { typeof(Type) }));
				ctorIL.Emit(OpCodes.Stloc_1);
				ctorIL.Emit(OpCodes.Ldarg_0);
				ctorIL.Emit(OpCodes.Ldloc_1);
				ctorIL.Emit(OpCodes.Ldloc_0);
				ctorIL.Emit(OpCodes.Call, typeof(FormatterServices).GetMethod("PopulateObjectMembers", new Type[] { typeof(object), typeof(MemberInfo[]), typeof(object[]) }));
				ctorIL.Emit(OpCodes.Pop);*/
				ctorIL.Emit(OpCodes.Nop);
			}

			ctorIL.Emit(OpCodes.Ret);
		}

		private static void createDefaultConstructor(TypeBuilder targetTypeBld, Type baseType, Type[] constructorParamsTypes)
		{
			// Create the constructor
			ConstructorInfo objCtor = baseType.GetConstructor(constructorParamsTypes);

			ConstructorBuilder targetCtor = targetTypeBld.DefineConstructor(
					  MethodAttributes.Public,
					  CallingConventions.Standard,
					  constructorParamsTypes);
			ILGenerator ctorIL = targetCtor.GetILGenerator();


			ctorIL.Emit(OpCodes.Ldarg_0); // this
			// pass all the parameters: 
			for (int i = 1; i <= constructorParamsTypes.Length; i++)
			{
				ctorIL.Emit(OpCodes.Ldarg_S, i);
			}

			ctorIL.Emit(OpCodes.Call, objCtor);

			ctorIL.Emit(OpCodes.Ret);
		}

		/// <summary>
		/// Implementation of the add_PropertyChanged method. The lines bellow but in IL Code
		///  MethodImplAttribute(MethodImplOptions.Synchronized)]
		///  public void add_PropertyChanged(PropertyChangedEventHandler handler) 
		///  {
		///     PropertyChanged = (PropertyChangedEventHandler) Delegate.Combine(PropertyChanged, handler);
		///  }
		///  Disassembled il code:
		///  IL_0000:  /* 02   |                  */ ldarg.0
		///  IL_0001:  /* 02   |                  */ ldarg.0
		///  IL_0002:  /* 7B   | (04)000001       */ ldfld      class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000004*/ ReflectionEmit.SimplePropertyChangedImplementation/*02000003*/::PropertyChanged /* 04000001 */
		///  IL_0007:  /* 03   |                  */ ldarg.1
		///  IL_0008:  /* 28   | (0A)00003B       */ call       class [mscorlib/*23000001*/]System.Delegate/*01000032*/ [mscorlib/*23000001*/]System.Delegate/*01000032*/::Combine(class [mscorlib/*23000001*/]System.Delegate/*01000032*/,
		///                                                    class [mscorlib/*23000001*/]System.Delegate/*01000032*/) /* 0A00003B */
		///  IL_000d:  /* 74   | (01)000004       */ castclass  [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000004*/
		///  IL_0012:  /* 7D   | (04)000001       */ stfld      class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000004*/ ReflectionEmit.SimplePropertyChangedImplementation/*02000003*/::PropertyChanged /* 04000001 */
		///  IL_0017:  /* 2A   |                  */ ret
		/// 
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="eventBuilder"></param>
		/// <param name="eventHandler"></param>
		/// <param name="operation"></param>
		private static void addOrRemoveMethod(TypeBuilder builder, EventBuilder eventBuilder, FieldBuilder eventHandler, bool operation)
		{
			string methodName;

			if (operation)
			{
				methodName = "add_PropertyChanged";
			}
			else
			{
				methodName = "remove_PropertyChanged";
			}

			// code  generation
			MethodBuilder method = builder.DefineMethod(
							 methodName,
							 MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.SpecialName |
							 MethodAttributes.HideBySig, typeof(void), new Type[] { typeof(PropertyChangedEventHandler) });
			
			ILGenerator mthdIL = method.GetILGenerator();

			mthdIL.Emit(OpCodes.Ldarg_0);
			mthdIL.Emit(OpCodes.Ldarg_0);
			mthdIL.Emit(OpCodes.Ldfld, eventHandler);
			mthdIL.Emit(OpCodes.Ldarg_1);
			if (operation)
			{
				mthdIL.Emit(OpCodes.Call, typeof(System.Delegate).GetMethod("Combine", new Type[] { typeof(Delegate), typeof(Delegate) }));
			}
			else
			{
				mthdIL.Emit(OpCodes.Call, typeof(System.Delegate).GetMethod("Remove", new Type[] { typeof(Delegate), typeof(Delegate) }));
			}

			mthdIL.Emit(OpCodes.Castclass, typeof(PropertyChangedEventHandler));
			mthdIL.Emit(OpCodes.Stfld, eventHandler);
			mthdIL.Emit(OpCodes.Ret);
			
			// attach the methods to the event
			if (operation)
			{
				eventBuilder.SetAddOnMethod(method);
			}
			else
			{
				eventBuilder.SetRemoveOnMethod(method);
			}
		}

		/// <summary>
		/// The generated code should be: return (PropertyChanged == null)
		/// CIL disassembled code:
		/// .maxstack  2
		/// .locals /*11000003*/ init ([0] bool CS$1$0000)
		/// IL_0000:  /* 00   |                  */ nop
		/// IL_0001:  /* 02   |                  */ ldarg.0
		/// IL_0002:  /* 7B   | (04)000002       */ ldfld      class [System/*23000003*/]System.ComponentModel.PropertyChangedEventHandler/*01000005*/ ConsoleApplication1.TestClass/*02000004*/::PropertyChanged /* 04000002 */
		/// IL_0007:  /* 14   |                  */ ldnull
		/// IL_0008:  /* FE01 |                  */ ceq
		/// IL_000a:  /* 16   |                  */ ldc.i4.0
		/// IL_000b:  /* FE01 |                  */ ceq
		/// IL_000d:  /* 0A   |                  */ stloc.0
		/// IL_000e:  /* 2B   | 00               */ br.s       IL_0010
		/// IL_0010:  /* 06   |                  */ ldloc.0
		/// IL_0011:  /* 2A   |                  */ ret
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="eventHandler"></param>
		private static void hasSubscribersMethod(TypeBuilder builder, FieldBuilder eventHandler)
		{
			MethodBuilder method = builder.DefineMethod(
							 "HasSubscribers",
							 MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final, 
							 typeof(bool), 
							 new Type[0]);

			ILGenerator mthdIL = method.GetILGenerator();
			
			LocalBuilder returnedValue = mthdIL.DeclareLocal(typeof(bool));
						
			mthdIL.Emit(OpCodes.Nop);
			mthdIL.Emit(OpCodes.Ldarg_0);
			mthdIL.Emit(OpCodes.Ldfld, eventHandler);
			mthdIL.Emit(OpCodes.Ldnull);
			mthdIL.Emit(OpCodes.Ceq);
			mthdIL.Emit(OpCodes.Ldc_I4_0);
			mthdIL.Emit(OpCodes.Ceq);
			mthdIL.Emit(OpCodes.Stloc_0);
			//mthdIL.Emit(OpCodes.Br_S);
			mthdIL.Emit(OpCodes.Ldloc_0);
			mthdIL.Emit(OpCodes.Ret);
		}

		/// <summary>
		/// The generated code should be: PropertyChanged.Invoke(this, new PropertyChangedEventArgs(value))
		/// CIL disassembled code:
		/// IL_0001:  /* 02   |                  */ ldarg.0
		/// IL_0002:  /* 7B   | (04)000002       */ ldfld      class [System/*23000003*/]System.ComponentModel.PropertyChangedEventHandler/*01000005*/ ConsoleApplication1.TestClass/*02000004*/::PropertyChanged /* 04000002 */
		/// IL_0007:  /* 02   |                  */ ldarg.0
		/// IL_0008:  /* 03   |                  */ ldarg.1
		/// IL_0009:  /* 73   | (0A)00001F       */ newobj     instance void [System/*23000003*/]System.ComponentModel.PropertyChangedEventArgs/*0100001D*/::.ctor(string) /* 0A00001F */
		/// IL_000e:  /* 6F   | (0A)000020       */ callvirt   instance void [System/*23000003*/]System.ComponentModel.PropertyChangedEventHandler/*01000005*/::Invoke(object, class [System/*23000003*/]System.ComponentModel.PropertyChangedEventArgs/*0100001D*/) /* 0A000020 */
		/// IL_0013:  /* 00   |                  */ nop
		/// .line 22,22 : 3,4 ''
		/// //000022: 		}
		/// IL_0014:  /* 2A   |                  */ ret
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="eventHandler"></param>
		private static void throwPropertyChangedMethod(TypeBuilder builder, FieldBuilder eventHandler)
		{
			MethodBuilder method = builder.DefineMethod(
							 "ThrowPropertyChangedEvent",
							 MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final, typeof(void), new Type[] { typeof(string) });

			ILGenerator mthdIL = method.GetILGenerator();

			ConstructorInfo propEventArgsCtor = typeof(PropertyChangedEventArgs).GetConstructor(new Type[] { typeof(string) });

			mthdIL.Emit(OpCodes.Nop);
			mthdIL.Emit(OpCodes.Ldarg_0);
			mthdIL.Emit(OpCodes.Ldfld, eventHandler);
			mthdIL.Emit(OpCodes.Ldarg_0);
			mthdIL.Emit(OpCodes.Ldarg_1);
			mthdIL.Emit(OpCodes.Newobj, propEventArgsCtor);
			mthdIL.Emit(OpCodes.Callvirt, typeof(PropertyChangedEventHandler).GetMethod("Invoke",  
				new Type[] { typeof(PropertyChangedEventHandler), typeof(PropertyChangedEventArgs)}));
			mthdIL.Emit(OpCodes.Nop);
			mthdIL.Emit(OpCodes.Ret);
		}

		/// <summary>
		/// The generated code should be: return PropertyChanged.GetInvocationList()
		/// CIL disassembled code:
		///   .locals /*11000002*/ init ([0] class [mscorlib/*23000001*/]System.Delegate/*01000004*/[] CS$1$0000)
		///    IL_0000:  /* 00   |                  */ nop
		///    IL_0001:  /* 02   |                  */ ldarg.0
		///	   IL_0002:  /* 7B   | (04)000001       */ ldfld      class [System/*23000003*/]System.ComponentModel.PropertyChangedEventHandler/*01000005*/ ConsoleApplication1.TestClass/*02000002*/::PropertyChanged /* 04000001 */
		///    IL_0007:  /* 6F   | (0A)000012       */ callvirt   instance class [mscorlib/*23000001*/]System.Delegate/*01000004*/[] [mscorlib/*23000001*/]System.Delegate/*01000004*/::GetInvocationList() /* 0A000012 */
		///    IL_000c:  /* 0A   |                  */ stloc.0
		///    IL_000d:  /* 2B   | 00               */ br.s       IL_000f
		///	   IL_000f:  /* 06   |                  */ ldloc.0
		///    IL_0010:  /* 2A   |                  */ ret
 		/// </summary>
		/// <param name="builder"></param>
		/// <param name="eventHandler"></param>
		private static void getSubscribersListMethod(TypeBuilder builder, FieldBuilder eventHandler)
		{
			MethodBuilder method = builder.DefineMethod(
							 "GetSubscribersList",
							 MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final, typeof(Delegate[]), new Type[0]);

			ILGenerator mthdIL = method.GetILGenerator();

			LocalBuilder returnedValue = mthdIL.DeclareLocal(typeof(Delegate[]));

			mthdIL.Emit(OpCodes.Nop);
			mthdIL.Emit(OpCodes.Ldarg_0);
			mthdIL.Emit(OpCodes.Ldfld, eventHandler);
			mthdIL.Emit(OpCodes.Callvirt, typeof(PropertyChangedEventHandler).GetMethod("GetInvocationList", new Type[0]));
			mthdIL.Emit(OpCodes.Stloc_0);
			//mthdIL.Emit(OpCodes.Br_S);
			mthdIL.Emit(OpCodes.Ldloc_0);
			mthdIL.Emit(OpCodes.Ret);
		}

		

	}
}
