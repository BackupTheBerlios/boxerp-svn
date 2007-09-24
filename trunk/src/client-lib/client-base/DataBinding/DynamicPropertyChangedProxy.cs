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
	public static class DynamicPropertyChangedProxy
	{
		private static AssemblyBuilder _assemblyBuilder = null;
		private static ModuleBuilder _moduleBuilder = null;
		private static string ASSEMBLY_NAME = "BoxerpDynamicAssembly";
		private static string DYNAMIC_MOD_NAME = ASSEMBLY_NAME;
		private static string ASSEMBLY_DLL = DYNAMIC_MOD_NAME + ".dll";
		

		private static AssemblyBuilder MyAssemblyBuilder
		{
			get
			{
				if (_assemblyBuilder == null)
				{
					AssemblyName myAsmName = new AssemblyName();
					myAsmName.Name = ASSEMBLY_NAME;
					#if CREATE_DLL_FILE
						_assemblyBuilder = Thread.GetDomain().DefineDynamicAssembly(myAsmName, AssemblyBuilderAccess.RunAndSave);
					#else
						_assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(myAsmName, AssemblyBuilderAccess.Run);
					#endif

						AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
				}

				return _assemblyBuilder;
			}
		}

		private static ModuleBuilder MyModuleBuilder
		{
			get
			{
				if (_moduleBuilder == null)
				{
					try
					{
						#if CREATE_DLL_FILE
							_moduleBuilder = MyAssemblyBuilder.DefineDynamicModule(DYNAMIC_MOD_NAME, ASSEMBLY_DLL);
						#else
							_moduleBuilder = MyAssemblyBuilder.DefineDynamicModule(DYNAMIC_MOD_NAME);
						#endif
					}
					catch (Exception ex)
					{
						Console.Out.WriteLine("Module Builder creation exception: " + ex.Message);
					}
				}

				return _moduleBuilder;
			}
		}

		public static Assembly OnAssemblyResolve(Object sender, ResolveEventArgs args)
		{
			Console.Out.WriteLine("ON ASSEMBLY RESOLVE: (requesting)" + args.Name);
			if (args.Name.StartsWith(DYNAMIC_MOD_NAME))
			{
				Console.Out.WriteLine("ON ASSEMBLY RESOLVE: " + MyAssemblyBuilder.GetTypes()[0].ToString());
				Assembly tmpAssembly = Assembly.GetAssembly(MyAssemblyBuilder.GetTypes()[0]);
				Console.Out.WriteLine("ON ASSEMBLY RESOLVE (loaded assembly): " + tmpAssembly.FullName);
				return tmpAssembly;
				
			}
			Console.Out.WriteLine("ASSEMBLY NOT FOUND");
			return null;
		}

		public static string CleanGarbageSimbols(string sourceName)
		{
			char[] garbage = new char[] { '.', '[', ']', '+', '\'', '\\', '`', ',' };
			string cleaned = String.Empty;

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

		#region clean names functions
		public static string CleanBaseTypeName(string sourceName)
		{
			string[] namespaces = sourceName.Split(new char[] { '.' });
			sourceName = namespaces[namespaces.Length -1];
			sourceName += DateTime.Now.ToString("ddMMyyyyHH");// +Guid.NewGuid().ToString();
			/*Random random = new Random();
			for (int i = 0; i < 3; i++)
			{
				sourceName += random.Next(9999 * i).ToString();
			}*/

			return CleanGarbageSimbols(sourceName);
		}

		public static string GetBindableClassName(string sourceName)
		{
			sourceName = CleanGarbageSimbols(sourceName);
			int start = sourceName.IndexOf("Bindable");
			int end = sourceName.IndexOf("1", start);
			return sourceName.Substring(start);
		}
		#endregion

		public static Type CreateINotifyPropertyChangedBindableProxy(Type baseType, Type[] constructorParamsTypes)
		{
			string className = "PropChPrxy_" + GetBindableClassName(baseType.ToString());
			return CreateINotifyPropertyChangedTypeProxy(baseType, constructorParamsTypes, className);
		}

		public static Type CreateINotifyPropertyChangedTypeProxy(Type baseType, Type[] constructorParamsTypes)
		{
			string className = "PropChPrxy_" + CleanBaseTypeName(baseType.ToString());
			return CreateINotifyPropertyChangedTypeProxy(baseType, constructorParamsTypes, className);
		}

		public static Type CreateINotifyPropertyChangedTypeProxy(Type baseType, Type[] constructorParamsTypes, string className)
		{
			// If this proxy has been created already do not create it again
			foreach (Type t in MyAssemblyBuilder.GetTypes())
			{
				if (t.ToString() == className)
				{
					return t; 
				}
			}

			#if CREATE_DLL_FILE
				if (System.IO.File.Exists(ASSEMBLY_DLL))
				{
					System.IO.File.Delete(ASSEMBLY_DLL);
				}
			#endif

			Type targetType = null;
			Type[] interfaces = new Type[] { typeof(ICustomNotifyPropertyChanged) };

			TypeBuilder targetTypeBld = MyModuleBuilder.DefineType(className, TypeAttributes.Public, baseType, interfaces);

			// add the Serializable Attribute to the class:
			ConstructorInfo attributeCtorInfo = typeof(SerializableAttribute).GetConstructor(new Type[0]);
			CustomAttributeBuilder customAttBuilder = new CustomAttributeBuilder(attributeCtorInfo, new object[0]);
			targetTypeBld.SetCustomAttribute(customAttBuilder);

			EventBuilder eventField = targetTypeBld.DefineEvent("PropertyChanged", EventAttributes.None, typeof(PropertyChangedEventHandler));

			// the propertyChanged event shouldn't be serializable
			FieldBuilder eventHandler = targetTypeBld.DefineField("PropertyChanged", typeof(PropertyChangedEventHandler), FieldAttributes.Private | FieldAttributes.NotSerialized);

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
			GetSubscribersListMethod(targetTypeBld, eventHandler);

			targetType = targetTypeBld.CreateType();

			#if CREATE_DLL_FILE
				MyAssemblyBuilder.Save(ASSEMBLY_DLL);
			#endif

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
		private static void HasSubscribersMethod(TypeBuilder builder, FieldBuilder eventHandler)
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
		private static void ThrowPropertyChangedMethod(TypeBuilder builder, FieldBuilder eventHandler)
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
		private static void GetSubscribersListMethod(TypeBuilder builder, FieldBuilder eventHandler)
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
