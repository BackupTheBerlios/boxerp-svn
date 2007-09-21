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
using System.Collections;
using System.ComponentModel;
using System.Text;
using System.Reflection;
using Castle.DynamicProxy;
using Castle.Core.Interceptor;

namespace Boxerp.Client
{
	[Serializable]
	public abstract class AbstractBindableWrapper<T, Y> : IInterceptor, INotifyPropertyChanged,
		IBindableWrapper<T> where Y : AbstractBindableWrapper<T, Y>.BindableFields<T>
	{
		#region INotifyPropertyChanged Members

		[field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
			
		private Y _bindableFields;

		[field: NonSerialized]
		private Stack<Y> _undoStack = new Stack<Y>();

		[field: NonSerialized]
		private Stack<Y> _redoStack = new Stack<Y>();
		
		[field: NonSerialized]
		private ProxyGenerator _generator = new ProxyGenerator();

		private bool _dontIntercept = false;
		private bool _disableBusinessObjInterception = false;
		private bool _disableWrapperInterception = false;
		private bool _disableUndoRedo = false;

		/// <summary>
		/// When the wrapper class constructor requires parameters they must be passed in thru the constructorParams
		/// </summary>
		/// <param name="businessObj">The business object to wrap</param>
		/// <param name="wrapper">The wrapper class type</param>
		/// <param name="constructorParams">The parameters to the wrapper class constructor</param>
		/// <param name="disableBusinessObjectInterception">Whether enable or disable BO interception, it means Undo and Redo capability for the BO</param>
		/// <param name="disableUndoRedo">If disableWrapperInterception is false and disableBusinessObjectInterception is false the Undo-Redo feature is enable by default unless you set the disableUndoRedo to true</param>
		public AbstractBindableWrapper(T businessObj, bool disableWrapperInterception, 
			bool disableBusinessObjectInterception, bool disableUndoRedo, object[] constructorParams)
		{
			lock (this)
			{
				_disableUndoRedo = disableUndoRedo;
				_disableBusinessObjInterception = disableBusinessObjectInterception;
				_disableWrapperInterception = disableWrapperInterception;
				if (disableBusinessObjectInterception && disableWrapperInterception)
				{
					_disableUndoRedo = true;
				}

				object[] argumentsForConstructor;
				Type[] argumentTypes;

				if ((constructorParams != null) && (constructorParams.Length > 0))
				{
					argumentsForConstructor = new object[constructorParams.Length + 1];
					argumentTypes = new Type[constructorParams.Length + 1];
					for (int i = 1; i < argumentsForConstructor.Length; i++)
					{
						object item = constructorParams[i - 1];
						argumentsForConstructor[i] = item;
						argumentTypes[i] = item.GetType();
					}
				}
				else
				{
					argumentsForConstructor = new object[1];
					argumentTypes = new Type[1];
				}
				
				argumentsForConstructor[0] = this;
				argumentTypes[0] = typeof(IInterceptor);

				if (disableWrapperInterception)
				{
					ConstructorInfo constructor = typeof(Y).GetConstructor(argumentTypes);
					_bindableFields = (Y)constructor.Invoke(argumentsForConstructor);
				}
				else
				{
					// double proxy. The first one implents the INotifyPropertyChanged interface
					Type notifiableType = DynamicPropertyChangedProxy.CreateINotifyPropertyChangedBindableProxy(typeof(Y), argumentTypes);
					_bindableFields = (Y)_generator.CreateClassProxy(notifiableType, new IInterceptor[] { this }, argumentsForConstructor);

				}

				RefreshBusinessObj(businessObj);
			}
		}

		/// <summary>
		/// When the wrapper class constructor requires parameters they must be passed in thru the constructorParams
		/// </summary>
		/// <param name="businessObj">The business object to wrap</param>
		/// <param name="wrapper">The wrapper class type</param>
		/// <param name="constructorParams">The parameters to the wrapper class constructor</param>
		public AbstractBindableWrapper(T businessObj, object[] constructorParams)
			: this(businessObj, false, false, false, constructorParams)
		{
			
		}


		/// <summary>
		/// Business object with a default constructor and wrapper class without parameters, other than the interceptor
		/// </summary>
		/// <param name="businessObj">The business object to wrap</param>
		/// <param name="wrapper">The wrapper class type</param>
		/// <param name="disableBusinessObjectInterception">Whether enable or disable BO interception, it means Undo and Redo capability for the BO</param>
		/// <param name="disableWrapperInterception">Whether disable Undo and Redo capability for the wrapper</param>
		public AbstractBindableWrapper(T businessObj, bool disableWrapperInterception, bool disableBusinessObjectInterception)
			: this (businessObj, disableWrapperInterception, disableBusinessObjectInterception, false, null)
		{
			
		}

		public AbstractBindableWrapper(T businessObj, bool disableInterception, object[] constructorParams)
			: this(businessObj, disableInterception, disableInterception, false, constructorParams)
		{

		}

		public AbstractBindableWrapper(T businessObj, bool disableWrapperInterception, bool disableBOInterception, bool disableUndoRedo)
			: this(businessObj, disableWrapperInterception, disableBOInterception, disableUndoRedo, null)
		{

		}

		public AbstractBindableWrapper(T businessObj, bool disableInterception)
			: this(businessObj, disableInterception, disableInterception, true, null)
		{

		}

		/// <summary>
		/// Business object with a default constructor and wrapper class without parameters, other than the interceptor
		/// </summary>
		/// <param name="businessObj">The business object to wrap</param>
		/// <param name="wrapper">The wrapper class type</param>
		public AbstractBindableWrapper(T businessObj)
			: this(businessObj, false, false, false, null)
		{
	
		}

		/// <summary>
		/// Intended to be used when the business object doesn't have a default constructor with no parameters:
		/// public BOName() {}
		/// </summary>
		/// <param name="businessObj">The business object to wrap</param>
		/// <param name="wrapper">The wrapper class type</param>
		/// <param name="businessObjInterface">The business object interface</param>
		/*public AbstractBindableWrapper(T businessObj, Type businessObjInterface)
		{
			lock (this)
			{
				IInterceptor[] interceptors = new AbstractBindableWrapper<T, Y>[1];
				interceptors[0] = this;
				object[] arguments = new object[1];
				arguments[0] = this;
				_bindableFields = (Y)_generator.CreateClassProxy(typeof(Y), interceptors, arguments);
				// TODO: Make this work, because it is failing
				T proxy = (T)_generator.CreateInterfaceProxyWithTarget(businessObjInterface, businessObj, interceptors);
				copyBOtoProxy(proxy, businessObj);
				Data.BusinessObj = proxy;
			}
		}*/

		public void RefreshBusinessObj(T businessObj)
		{
			if (_disableBusinessObjInterception)
			{
				Data.BusinessObj = businessObj;
			}
			else
			{
				T proxy = CreateDoubleProxy();
				copyBOtoProxy(proxy, businessObj);
				Data.BusinessObj = proxy;
			}
		}

		/// <summary>
		/// The first proxy is a class that extends T and implements the ICustomNotifyPropertyChanged
		/// The second proxy is the Interception one using the Castle.DynamicProxy2.
		/// In case the business object T already implements the INotifyPropertyChanged, then the first proxy is not needed
		/// </summary>
		/// <returns></returns>
		private T CreateDoubleProxy()
		{
			Type notifiableType;

			if (!(typeof(T) is INotifyPropertyChanged))
			{
				notifiableType = DynamicPropertyChangedProxy.CreateINotifyPropertyChangedTypeProxy(typeof(T), new Type[0]);
			}
			else
			{
				notifiableType = typeof(T);
			}

			return (T)_generator.CreateClassProxy(notifiableType, new IInterceptor[] { this });
		}

		public Y Data
		{
			get
			{
				return _bindableFields;
			}
		}

		public Type GetWrappedObjectType()
		{
			return typeof(T);
		}

		public bool HasSubscribers
		{
			get
			{
				return (PropertyChanged != null);
			}
		}

		public virtual void Undo()
		{
			if ((!_disableUndoRedo) && (_undoStack.Count > 0))
			{
				_redoStack.Push(_bindableFields);
				if (_undoStack.Count > 0)
				{
					_bindableFields = _undoStack.Pop();
				}
			}
		}

		public virtual void Redo()
		{
			if ((!_disableUndoRedo) && (_redoStack.Count > 0))
			{
				_undoStack.Push(_bindableFields);
				if (_redoStack.Count > 0)
				{
					_bindableFields = _redoStack.Pop();
				}
			}
		}

		public override string ToString()
		{
			return Data.BusinessObj.ToString();
		}

		protected virtual void copyBOtoProxy(T businessObj)
		{
			_dontIntercept = true;
			foreach (PropertyInfo boProperty in businessObj.GetType().GetProperties())
			{
				object propValue = boProperty.GetValue(businessObj, null);

				foreach (PropertyInfo proxyProperty in Data.BusinessObj.GetType().GetProperties())
				{
					if (boProperty.Name == proxyProperty.Name)
					{
						proxyProperty.SetValue(Data.BusinessObj, propValue, null);
						break;
					}
				}
			}
			_dontIntercept = false;
		}

		protected virtual void copyBOtoProxy(T proxy, T businessObj)
		{
			lock (this)
			{
				_dontIntercept = true;
				foreach (PropertyInfo boProperty in businessObj.GetType().GetProperties())
				{
					object propValue = boProperty.GetValue(businessObj, null);

					foreach (PropertyInfo proxyProperty in proxy.GetType().GetProperties())
					{
						if (boProperty.Name == proxyProperty.Name)
						{
							proxyProperty.SetValue(proxy, propValue, null);
							break;
						}
					}
				}
				_dontIntercept = false;
			}
		}

		protected virtual Y cloneBindable(Y original, AbstractBindableWrapper<T, Y>.BindableFields<T> copy)
		{
			PropertyInfo[] properties = copy.GetType().GetProperties();

			_dontIntercept = true;
			int i = 0;
			foreach (PropertyInfo originalProp in original.GetType().GetProperties())
			{
				PropertyInfo copyProp = properties[i];
				if (originalProp.CanRead)
				{
					object copyValue = originalProp.GetValue(original, null);

					if (! (copyValue is IEnumerable)) // fix this. The question should be. if copyValue is Serializable
					{
						if (copyValue is T)	// if it is the business object
						{
							// at this line the business object should be a proxy already so clone the whole thing
							//T proxy = CreateDoubleProxy();
							//copyBOtoProxy(proxy, (T)((ICloneable)copyValue).Clone());
							if (copyProp.CanWrite)
							{
								T proxyClone = (T) Cloner.Clone(copyValue);
								//proxyClone = copyPropertyChangedSubscribers(copyValue, proxyClone)
								copyProp.SetValue(copy, proxyClone, null);
								// the propertyChanged event is not cloned as it shouldn't be serializable to avoid serializing subscribers
								// so after cloning it is necessary to copy the subscribers
							}
						}
						else
						{
							if (copyValue is ICloneable)
							{
								copyProp.SetValue(copy, ((ICloneable)copyValue).Clone(), null);
							}
							/*
							 * else if is Serializable make the copy serializing 
							 */
						}
					}
					else if (copyValue is IEnumerable)
					{
						IList enumerable = (IList)copyValue;

						object[] enumerableCopy = new object[enumerable.Count];
						for (int k = 0; k < enumerable.Count; k++)
						{
							object value = enumerable[k];
							if (value is ICloneable)
							{
								enumerableCopy[k] = ((ICloneable)value).Clone();
							}
						}
						copyValue = enumerableCopy;	// TODO: review this making sure the garbage collection works 

					}
				}
				i++;
			}
			_dontIntercept = false;
			return (Y)copy;
		}

		#region Interceptor implementation
		public void Intercept(IInvocation invocation)
		{
			if (!_dontIntercept)
			{
				if (invocation.Method.Name.StartsWith("set_"))	// setting the property value
				{
					string propDirtyName = invocation.Method.Name;
					string propName = propDirtyName.Substring(propDirtyName.IndexOf('_') + 1);
					PropertyInfo propInfo;
					object oldValue = null;

					// Search in the bindableFiels properties:
					propInfo = _bindableFields.GetType().GetProperty(propName);
					if (propInfo == null)
					{
						// Search in the business object
						// I wanted to do this: 
						// pInfo = _bindableFields.BusinessObj.GetType().GetProperty(propName)
						// But the Proxy generator creates a class which extends from the Business Object, 
						// so that it is ambiguous.
						// This loop avoids the Ambiguos Match exception that happen because of the dinamic proxy
						foreach (PropertyInfo pInfo in _bindableFields.BusinessObj.GetType().GetProperties())
						{
							if (pInfo.Name == propName)
							{
								oldValue = pInfo.GetValue(_bindableFields.BusinessObj, null);
								propInfo = pInfo;
								break;
							}
						}
					}
					else
					{
						oldValue = propInfo.GetValue(_bindableFields, null);
					}

					if (oldValue != invocation.Arguments[0])
					{
						if (!_disableUndoRedo)
						{
							_undoStack.Push(cloneBindable(_bindableFields, _bindableFields.SwallowCopy()));		// property is gonna change, put it in the undo stack
						}

						invocation.Proceed();

						if (PropertyChanged != null)
						{
							PropertyChanged(_bindableFields, new PropertyChangedEventArgs(propInfo.Name));		
						}
						if (_bindableFields is ICustomNotifyPropertyChanged)
						{
							ICustomNotifyPropertyChanged notifiable = _bindableFields as ICustomNotifyPropertyChanged;
							if (notifiable.HasSubscribers())
							{
								notifiable.ThrowPropertyChangedEvent(propInfo.Name);
							}
						}
						if (_bindableFields.BusinessObj is ICustomNotifyPropertyChanged)
						{
							ICustomNotifyPropertyChanged notifiable = _bindableFields.BusinessObj as ICustomNotifyPropertyChanged;
							if (notifiable.HasSubscribers())
							{
								notifiable.ThrowPropertyChangedEvent(propInfo.Name);
							}
						}
						return;
					}			
				}
			}
			invocation.Proceed();
		}
		#endregion

		[Serializable]
		public abstract class BindableFields<D>
		{
			private D _businessObj;
			protected IInterceptor _interceptor;
			
			public BindableFields(IInterceptor interceptor)
			{
				_interceptor = interceptor;
			}

			public D BusinessObj
			{
				get
				{
					return _businessObj;
				}
				internal set
				{
					_businessObj = value;
				}
			}

			public INotifyPropertyChanged BusinessObjBinding
			{
				get
				{
					return (INotifyPropertyChanged)_businessObj;
				}
			}

			public BindableFields<D> SwallowCopy()
			{
				return (BindableFields<D>)MemberwiseClone();
			}
		}
	}
}
