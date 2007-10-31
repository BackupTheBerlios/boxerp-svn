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
using System.Runtime.Serialization;

// doc on Reflection.Emit:
// http://msdn2.microsoft.com/en-us/library/system.reflection.emit.constructorbuilder.aspx

namespace Boxerp.Client
{
	public static partial class DynamicPropertyChangedProxy
	{
		private static AssemblyBuilder _assemblyBuilder = null;
		private static ModuleBuilder _moduleBuilder = null;
		private static string ASSEMBLY_NAME = "BoxerpDynamicAssembly";
		private static string DYNAMIC_MOD_NAME = ASSEMBLY_NAME;
		private static string ASSEMBLY_DLL = DYNAMIC_MOD_NAME + ".dll";
		public static string SERIALIZED_DATA = "__serializedData";
		public static string OBJECT_BASE_TYPE = "__businessObjBaseType";
		public static string ARGUMENTS4CONSTRUCTOR = "__arguments4constructor";
		public static string IS_BUSINESS_OBJECT = "__isBusinessObject";
		private static bool _isBusinessObj = false;
		

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
						//AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
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

		/*public static Assembly OnAssemblyResolve(Object sender, ResolveEventArgs args)
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
		}*/

		#region clean names functions

		private static string cleanGarbageSimbols(string sourceName)
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

		/// <summary>
		/// TODO: When the class is generic it contains the "<" and ">". Change to code to take this into account
		/// </summary>
		/// <param name="sourceName"></param>
		/// <returns></returns>
		private static string cleanBaseTypeName(string sourceName)
		{
			string[] qualifiedNameParts = sourceName.Split(',');
			string nspace = qualifiedNameParts[0].Trim();
			string assembly = qualifiedNameParts[1].Trim();
			string firstPart;
			if (assembly.Contains(nspace))
			{
				firstPart = nspace + assembly;
			}
			else
			{
				firstPart = nspace;
			}
			string version = qualifiedNameParts[2].Split('=')[1].Trim();
			string culture = qualifiedNameParts[3].Split('=')[1].Trim();
			string pubToken = qualifiedNameParts[4].Split('=')[1].Trim();

			sourceName = firstPart + version + culture + pubToken;
			
			return cleanGarbageSimbols(sourceName);
		}

		private static string getBindableClassName(string sourceName)
		{
			sourceName = cleanGarbageSimbols(sourceName);
			int start = sourceName.IndexOf("Bindable");
			int end = sourceName.IndexOf("1", start);
			return sourceName.Substring(start);
		}
		#endregion

		public static Type CreateBindableWrapperProxy(Type baseType, Type[] constructorParamsTypes)
		{
			_isBusinessObj = false;
			string className = "PropChPrxy_" + getBindableClassName(baseType.ToString());
			return CreateBusinessObjectProxy(baseType, constructorParamsTypes, className);
		}

		public static Type CreateBusinessObjectProxy(Type baseType, Type[] constructorParamsTypes)
		{
			_isBusinessObj = true;
			string className = "PropChPrxy_" + cleanBaseTypeName(baseType.AssemblyQualifiedName);
			return CreateBusinessObjectProxy(baseType, constructorParamsTypes, className);
		}

		/// <summary>
		/// This proxy is quite similar to Castle.DynamicProxy2. It inherits the base type and make it 
		/// implement 2 interfaces: ICustomNotifyPropertyChanged and ISerializable. 
		/// You can see an example a hardcoded proxy here: 
		///      client-lib/tests/bindableObjectTests.SimpleRealHardcoded_CompleteBoxerpProxy
		/// The code that this proxy generates is something like that class above but it is generic,
		/// it extends from any kind of base class. 
		/// </summary>
		public static Type CreateBusinessObjectProxy(Type baseType, Type[] constructorParamsTypes, string className)
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
			Type[] interfaces = new Type[] { typeof(ICustomNotifyPropertyChanged), typeof(ISerializable) };

			TypeBuilder targetTypeBld = MyModuleBuilder.DefineType(className, TypeAttributes.Public, baseType, interfaces);

			// add the Serializable Attribute to the class:
			ConstructorInfo attributeCtorInfo = typeof(SerializableAttribute).GetConstructor(new Type[0]);
			CustomAttributeBuilder customAttBuilder = new CustomAttributeBuilder(attributeCtorInfo, new object[0]);
			targetTypeBld.SetCustomAttribute(customAttBuilder);
			// add event 
			EventBuilder eventField = targetTypeBld.DefineEvent("PropertyChanged", EventAttributes.None, typeof(PropertyChangedEventHandler));
			// the propertyChanged event shouldn't be serializable
			FieldBuilder eventHandler = targetTypeBld.DefineField("PropertyChanged", typeof(PropertyChangedEventHandler), FieldAttributes.Private | FieldAttributes.NotSerialized);
			
			// the properties for the DynamicProxyHelper
			FieldBuilder argsForConstructorField = targetTypeBld.DefineField(ARGUMENTS4CONSTRUCTOR, typeof(Type[]), FieldAttributes.Private | FieldAttributes.Static);
			FieldBuilder baseTypeField = targetTypeBld.DefineField(OBJECT_BASE_TYPE, typeof(string), FieldAttributes.Private | FieldAttributes.Static);
			FieldBuilder isBusinesSObjField = targetTypeBld.DefineField(IS_BUSINESS_OBJECT, typeof(bool), FieldAttributes.Private | FieldAttributes.Static);
			
			createDefaultConstructor(targetTypeBld, baseType, constructorParamsTypes);
			createDeserializationConstructor(targetTypeBld, baseType, constructorParamsTypes);
			getObjectDataMethod(targetTypeBld, baseType, argsForConstructorField, baseTypeField, isBusinesSObjField);
			addOrRemoveMethod(targetTypeBld, eventField, eventHandler, true);
			addOrRemoveMethod(targetTypeBld, eventField, eventHandler, false);
			hasSubscribersMethod(targetTypeBld, eventHandler);
			throwPropertyChangedMethod(targetTypeBld, eventHandler);
			getSubscribersListMethod(targetTypeBld, eventHandler);
			
			targetType = targetTypeBld.CreateType();

			// set the private fields that are needed for the DynamicProxyHelper
			targetType.GetField(argsForConstructorField.Name, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, constructorParamsTypes);
			targetType.GetField(baseTypeField.Name, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, baseType.AssemblyQualifiedName);
			targetType.GetField(isBusinesSObjField.Name, BindingFlags.NonPublic | BindingFlags.Static).SetValue(null, _isBusinessObj);

			#if CREATE_DLL_FILE
				MyAssemblyBuilder.Save(ASSEMBLY_DLL);
			#endif

			return targetType;
		}

		public static void ConsolePrint(ILGenerator generator, string msg)
		{
			generator.Emit(OpCodes.Call, typeof(Console).GetMethod("get_Out", new Type[0]));
			generator.Emit(OpCodes.Ldstr, msg);
			generator.Emit(OpCodes.Callvirt, typeof(System.IO.TextWriter).GetMethod("WriteLine", new Type[] { typeof(string) }));
			generator.Emit(OpCodes.Nop);
		}
	}
}
