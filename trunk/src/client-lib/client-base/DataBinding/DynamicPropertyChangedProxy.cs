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


			//Type objType = Type.GetType("System.Object");
			ConstructorInfo objCtor = baseType.GetConstructor(constructorParamsTypes); 

			ConstructorBuilder targetCtor = targetTypeBld.DefineConstructor(
                      MethodAttributes.Public,
                      CallingConventions.Standard,
                      constructorParamsTypes);
			ILGenerator ctorIL = targetCtor.GetILGenerator();

			// pass all the parameters: 
			for (int i = 0; i < constructorParamsTypes.Length; i++)
			{
				ctorIL.Emit(OpCodes.Ldarg_S, i);
			}

			ctorIL.Emit(OpCodes.Call, objCtor);

			ctorIL.Emit(OpCodes.Ret); 


		   	// Implementation of the add_PropertyChanged method. The lines bellow but in IL Code
				//MethodImplAttribute(MethodImplOptions.Synchronized)]
				// public void add_PropertyChanged(PropertyChangedEventHandler handler) 
				// {
				//     PropertyChanged = (PropertyChangedEventHandler) Delegate.Combine(PropertyChanged, handler);
				// }

			string[] mthdNames = new string[] { "add_PropertyChanged", "remove_PropertyChanged" };

			foreach (string mthdName in mthdNames)
			{
				MethodBuilder getFieldMthd = targetTypeBld.DefineMethod(
							 mthdName,
							 MethodAttributes.Public, typeof(void), 
							 new Type[] { typeof(PropertyChangedEventHandler) } );

				ILGenerator mthdIL = getFieldMthd.GetILGenerator();

				//mthdIL.Emit(OpCodes.Mkrefany, eventHandler); this line?
				mthdIL.Emit(OpCodes.Stfld, eventHandler); // or maybe this? // I want to push the eventHanderl onto the stack
				mthdIL.Emit(OpCodes.Ldarg_0);	// this pushes the argument onto the stack
				mthdIL.Emit(OpCodes.Call, typeof(Delegate).GetMethod("Combine", new Type[] { typeof(Delegate), typeof(Delegate) }));

				mthdIL.Emit(OpCodes.Ret);
			}
			
			targetType = targetTypeBld.CreateType();

		   // Let's save it, just for posterity.
	       
		   myAsmBuilder.Save("Dynamic.dll");
	    
		   return targetType;
		}
	}
}
