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

namespace Boxerp.Client
{
	public class DynamicPropertyChangedProxy
	{
		// http://msdn2.microsoft.com/en-us/library/system.reflection.emit.constructorbuilder.aspx

		public static Type CreateINotifyPropertyChangedTypeProxy(Type baseType, Type[] constructorParamsTypes)
		{
			Type targetType = null;
			
			AppDomain myDomain = Thread.GetDomain();
			AssemblyName myAsmName = new AssemblyName();
			myAsmName.Name = "Boxerp.DynamicAssembly";
    
			AssemblyBuilder myAsmBuilder = myDomain.DefineDynamicAssembly(myAsmName, AssemblyBuilderAccess.RunAndSave);

			ModuleBuilder targetModule = myAsmBuilder.DefineDynamicModule("DynamicModule", "Dynamic.dll");

			Type[] interfaces = new Type[] { typeof(INotifyPropertyChanged) };

			TypeBuilder targetTypeBld = targetModule.DefineType("PropertyChangedProxy", TypeAttributes.Public, baseType, interfaces);

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
			
			targetType = targetTypeBld.CreateType();

		    myAsmBuilder.Save("Dynamic.dll");
	    
			return targetType;
		}

		private static void AddOrRemoveMethod(TypeBuilder builder, EventBuilder eventBuilder, FieldBuilder eventHandler, bool operation)
		{
			// Implementation of the add_PropertyChanged method. The lines bellow but in IL Code
			//MethodImplAttribute(MethodImplOptions.Synchronized)]
			// public void add_PropertyChanged(PropertyChangedEventHandler handler) 
			// {
			//     PropertyChanged = (PropertyChangedEventHandler) Delegate.Combine(PropertyChanged, handler);
			// }
			// Disassembled il code:
			// IL_0000:  /* 02   |                  */ ldarg.0
			// IL_0001:  /* 02   |                  */ ldarg.0
			// IL_0002:  /* 7B   | (04)000001       */ ldfld      class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000004*/ ReflectionEmit.SimplePropertyChangedImplementation/*02000003*/::PropertyChanged /* 04000001 */
			// IL_0007:  /* 03   |                  */ ldarg.1
			// IL_0008:  /* 28   | (0A)00003B       */ call       class [mscorlib/*23000001*/]System.Delegate/*01000032*/ [mscorlib/*23000001*/]System.Delegate/*01000032*/::Combine(class [mscorlib/*23000001*/]System.Delegate/*01000032*/,
			//                                                    class [mscorlib/*23000001*/]System.Delegate/*01000032*/) /* 0A00003B */
			// IL_000d:  /* 74   | (01)000004       */ castclass  [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000004*/
			// IL_0012:  /* 7D   | (04)000001       */ stfld      class [System/*23000002*/]System.ComponentModel.PropertyChangedEventHandler/*01000004*/ ReflectionEmit.SimplePropertyChangedImplementation/*02000003*/::PropertyChanged /* 04000001 */
			// IL_0017:  /* 2A   |                  */ ret

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
							 MethodAttributes.Public, typeof(void),
							 new Type[] { typeof(PropertyChangedEventHandler) });

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

	}
}
