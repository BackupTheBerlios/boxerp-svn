//
// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
//
// Redistribution and use in source and binary forms, with or
// without modification, are permitted provided that the following
// conditions are met:
// Redistributions of source code must retain the above
// copyright notice, this list of conditions and the following
// disclaimer.
// Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following
// disclaimer in the documentation and/or other materials
// provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
// PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR
// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Reflection.Emit;
using System.ComponentModel;

// doc on Reflection.Emit:
// http://msdn2.microsoft.com/en-us/library/system.reflection.emit.constructorbuilder.aspx

namespace Boxerp.Client
{
	public class DynamicPropertyChangedProxy
	{
		public static string CleanBaseTypeName(string sourceName)
		{
			char[] garbage = new char[] { '.', '[', ']', '+', '\'', '\\', '`', ','};
			string cleaned = String.Empty;
			//string[] namespaces = sourceName.Split(new char[] { '.' });
			//sourceName = namespaces[namespaces.Length - 1];
			foreach (char sourceChar in sourceName)
			{
				bool clean = true;
				foreach (char garbageChar in garbage)
				{
					if (sourceChar == garbageChar)
					{
						clean = false;
						break;
					}
				}
				if (clean)
				{
					cleaned += sourceChar;
				}
			}
			return cleaned;
		}

		public static Type CreateINotifyPropertyChangedTypeProxy(Type baseType, Type[] constructorParamsTypes)
		{
			Type targetType = null;
			
			AppDomain myDomain = Thread.GetDomain();
			AssemblyName myAsmName = new AssemblyName();
			myAsmName.Name = "Boxerp.DynamicAssembly";
    
			AssemblyBuilder myAsmBuilder = myDomain.DefineDynamicAssembly(myAsmName, AssemblyBuilderAccess.RunAndSave);

			ModuleBuilder targetModule = myAsmBuilder.DefineDynamicModule("DynamicModule", "Dynamic.dll");

			Type[] interfaces = new Type[] { typeof(ICustomNotifyPropertyChanged) };

			TypeBuilder targetTypeBld = targetModule.DefineType("PropertyChangedProxy_" + CleanBaseTypeName(baseType.ToString()), TypeAttributes.Public, baseType, interfaces);

			// add the Serializable Attribute:
			ConstructorInfo attributeCtorInfo = typeof(SerializableAttribute).GetConstructor(new Type[0]);
			CustomAttributeBuilder customAttBuilder = new CustomAttributeBuilder(attributeCtorInfo, new object[0]);
			targetTypeBld.SetCustomAttribute(customAttBuilder);

			EventBuilder eventField = targetTypeBld.DefineEvent("PropertyChanged", EventAttributes.None, typeof(PropertyChangedEventHandler));

			FieldBuilder eventHandler = targetTypeBld.DefineField("PropertyChanged", typeof(PropertyChangedEventHandler), FieldAttributes.Private);

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
			


			AddOrRemoveMethod(targetTypeBld, eventField, eventHandler, true);
			AddOrRemoveMethod(targetTypeBld, eventField, eventHandler, false);
			HasSubscribersMethod(targetTypeBld, eventHandler);
			ThrowPropertyChangedMethod(targetTypeBld, eventHandler);

			targetType = targetTypeBld.CreateType();

		    myAsmBuilder.Save("Dynamic.dll");
	    
			return targetType;
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
		private static void AddOrRemoveMethod(TypeBuilder builder, EventBuilder eventBuilder, FieldBuilder eventHandler, bool operation)
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
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="eventHandler"></param>
		private static void HasSubscribersMethod(TypeBuilder builder, FieldBuilder eventHandler)
		{
			MethodBuilder method = builder.DefineMethod(
							 "HasSubscribers",
							 MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final, typeof(bool), new Type[0]);

			ILGenerator mthdIL = method.GetILGenerator();

			LocalBuilder returnedValue = mthdIL.DeclareLocal(typeof(bool));

			// this code is not valid. I want to do:  return PropertyChanged != null
			//mthdIL.Emit(OpCodes.Ldarg_0);
			mthdIL.Emit(OpCodes.Ldarg_0);
			mthdIL.Emit(OpCodes.Ldfld, eventHandler);
			mthdIL.Emit(OpCodes.Brfalse, (byte)0);
			mthdIL.Emit(OpCodes.Ldind_I1, Convert.ToInt32(true));
			// but no fucking idea how to write it in IL. The lines above are just a first attemp

			mthdIL.Emit(OpCodes.Ret);
		}

		/// <summary>
		/// The generated code should be: PropertyChanged.Invoke(this, something)
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="eventHandler"></param>
		private static void ThrowPropertyChangedMethod(TypeBuilder builder, FieldBuilder eventHandler)
		{
			MethodBuilder method = builder.DefineMethod(
							 "ThrowPropertyChangedEvent",
							 MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final, typeof(void), new Type[] { typeof(string) });

			ILGenerator mthdIL = method.GetILGenerator();

			mthdIL.Emit(OpCodes.Ldarg_0);
			mthdIL.Emit(OpCodes.Ldarg_0);
			mthdIL.Emit(OpCodes.Ldfld, eventHandler);
			mthdIL.Emit(OpCodes.Ldarg_1);
			
			mthdIL.Emit(OpCodes.Call, typeof(PropertyChangedEventHandler).GetMethod("Invoke",  
				new Type[] { typeof(PropertyChangedEventHandler), typeof(PropertyChangedEventArgs)}));


			mthdIL.Emit(OpCodes.Ret);
		}
	}
}