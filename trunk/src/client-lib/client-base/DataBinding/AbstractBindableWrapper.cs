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
	/*[Serializable]
	public abstract class AbstractBindableWrapper<T> : 
		AbstractBindableWrapper<T, AbstractBindableWrapper<T>.WrapObject<T>>
	{
		public AbstractBindableWrapper(T businessObj, bool disableWrapperInterception,
			bool disableBusinessObjectInterception, bool disableUndoRedo, object[] constructorParams)
			: base (businessObj, disableWrapperInterception, disableBusinessObjectInterception, disableUndoRedo, constructorParams)
		{}

		public AbstractBindableWrapper(T businessObj, object[] constructorParams)
			: this(businessObj, false, false, false, constructorParams)	{}

		public AbstractBindableWrapper(T businessObj, bool disableWrapperInterception, bool disableBusinessObjectInterception)
			: this(businessObj, disableWrapperInterception, disableBusinessObjectInterception, false, null)	{}

		public AbstractBindableWrapper(T businessObj, bool disableInterception, object[] constructorParams)
			: this(businessObj, disableInterception, disableInterception, false, constructorParams)	{}

		public AbstractBindableWrapper(T businessObj, bool disableWrapperInterception, bool disableBOInterception, bool disableUndoRedo)
			: this(businessObj, disableWrapperInterception, disableBOInterception, disableUndoRedo, null){}

		public AbstractBindableWrapper(T businessObj, bool disableInterception)
			: this(businessObj, disableInterception, disableInterception, true, null){}

		public AbstractBindableWrapper(T businessObj)
			: this(businessObj, false, false, false, null){	}


		[Serializable]
		public abstract class WrapObject<D> : 
			AbstractBindableWrapper<D, AbstractBindableWrapper<D>.WrapObject<D>>.BindableFields<D>
		{
			public WrapObject(IInterceptor interceptor)
				: base(interceptor)
			{

			}
		}
	}*/
		
	[Serializable]
	public abstract class AbstractBindableWrapper<T, Y> : IInterceptor, INotifyPropertyChanged,
		IBindableWrapper<T, Y> where Y : AbstractBindableWrapper<T, Y>.BindableFields<T>
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

		public Y Data
        {
            get
            {
                return _bindableFields;
            }
        }

        public bool HasSubscribers
        {
            get
            {
                return (PropertyChanged != null);
            }
        }

		/// <summary>
		/// When the wrapper class constructor requires parameters they must be passed in thru the constructorParams
		/// </summary>
		/// <param name="businessObj">The business object to wrap</param>
		/// <param name="wrapper">The wrapper class type</param>
		/// <param name="constructorParams">The parameters to the wrapper class constructor</param>
		/// <param name="disableBusinessObjectInterception">Whether enable or disable BO interception, it means Undo and Redo capability for the BO</param>
		/// <param name="disableUndoRedo">If disableWrapperInterception is false and disableBusinessObjectInterception is false the Undo-Redo feature is enable by default unless you set the disableUndoRedo to true</param>
		protected AbstractBindableWrapper(T businessObj, bool disableWrapperInterception, 
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
					Type notifiableType = DynamicPropertyChangedProxy.CreateBindableWrapperProxy(typeof(Y), argumentTypes, argumentsForConstructor);
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
		protected AbstractBindableWrapper(T businessObj, object[] constructorParams)
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
		protected AbstractBindableWrapper(T businessObj, bool disableWrapperInterception, bool disableBusinessObjectInterception)
			: this (businessObj, disableWrapperInterception, disableBusinessObjectInterception, false, null)
		{
			
		}

		protected AbstractBindableWrapper(T businessObj, bool disableInterception, object[] constructorParams)
			: this(businessObj, disableInterception, disableInterception, false, constructorParams)
		{

		}

		protected AbstractBindableWrapper(T businessObj, bool disableWrapperInterception, bool disableBOInterception, bool disableUndoRedo)
			: this(businessObj, disableWrapperInterception, disableBOInterception, disableUndoRedo, null)
		{

		}

		protected AbstractBindableWrapper(T businessObj, bool disableInterception)
			: this(businessObj, disableInterception, disableInterception, true, null)
		{

		}

		/// <summary>
		/// Business object with a default constructor and wrapper class without parameters, other than the interceptor
		/// </summary>
		/// <param name="businessObj">The business object to wrap</param>
		/// <param name="wrapper">The wrapper class type</param>
		protected AbstractBindableWrapper(T businessObj)
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
				T proxy = createDoubleProxy();
                Data.BusinessObj = proxy;
				copyBOtoProxy(businessObj);
			}
		}

		/// <summary>
		/// The first proxy is a class that extends T and implements the ICustomNotifyPropertyChanged
		/// The second proxy is the Interception one using the Castle.DynamicProxy2.
		/// In case the business object T already implements the INotifyPropertyChanged, then the first proxy is not needed
		/// </summary>
		/// <returns></returns>
		private T createDoubleProxy()
		{
			Type notifiableType;

			if (!(typeof(T) is INotifyPropertyChanged))
			{
				notifiableType = DynamicPropertyChangedProxy.CreateBusinessObjectProxy(typeof(T), new Type[0]);
			}
			else
			{
				notifiableType = typeof(T);
			}

			return (T)_generator.CreateClassProxy(notifiableType, new IInterceptor[] { this });
		}

		

		public Type GetWrappedObjectType()
		{
			return typeof(T);
		}

		public virtual void Undo()
		{
			if ((!_disableUndoRedo) && (_undoStack.Count > 0))
			{
				_redoStack.Push(cloneBindable());
				if (_undoStack.Count > 0)
				{
					Y tmpBindable = _undoStack.Pop();
					copyBindableProperties(tmpBindable);
                    throwPropertyChangedAllProperties();
			    }
			}
		}

		public virtual void Redo()
		{
			if ((!_disableUndoRedo) && (_redoStack.Count > 0))
			{
				_undoStack.Push(cloneBindable());
				if (_redoStack.Count > 0)
				{
					Y tmpBindable = _redoStack.Pop();
                    copyBindableProperties(tmpBindable);
                    throwPropertyChangedAllProperties();
				}
			}
		}

		public override string ToString()
		{
			return Data.BusinessObj.ToString();
		}

		protected virtual void copyBOtoProxy(T businessObj)
		{
            lock (this)
            {
                _dontIntercept = true;
                initializeProxyFromBusinessObject(businessObj);
                _dontIntercept = false;
            }
		}

		/// <summary>
        /// Copy the properties of the source into _bindableFields.
        /// FIXME: I think PropertyInfo.GetValue and SetValue passing in null as the last param throws 
        /// an exception on indexed properties like arrays. Test this.
		/// 
        /// </summary>
        /// <param name="source"></param>
        protected virtual void copyBindableProperties(Y source)
        {
            lock (this)
            {
                _dontIntercept = true;

                List<string> properties = new List<string>();

                foreach (PropertyInfo pInfo in typeof(Y).GetProperties())
                {
                    if (!properties.Contains(pInfo.Name))
                    {
                        if (pInfo.CanWrite)
                        {
                            object val = pInfo.GetValue(source, null);

							if ((!typeof(string).IsAssignableFrom(pInfo.PropertyType))
								&& (pInfo.PropertyType.GetProperties().Length > 0)
								&& (typeof(T) != pInfo.PropertyType))
							{
								/* If the property type is an object that in turn has properties, then if it is not a IBindableWrapper,
									do not copy the property. That is: if the property is not being intercepted, do not change it
									when another property is changed.
								 */
							}
							else
							{
								if (typeof(T) == pInfo.PropertyType)
								{
									copyBusinessObjectProperties((T)val);
								}
								else
								{
									typeof(Y).GetProperty(pInfo.Name).SetValue(_bindableFields, val, null);
								}
							}
                        }
                        properties.Add(pInfo.Name);
                    }
                }
                _dontIntercept = false;
            }
        }

        /// <summary>
        /// Copy the properties of the source into _bindableFields.BusinessObj.
        /// FIXME: I think PropertyInfo.GetValue and SetValue passing in null as the last param throws 
        /// an exception on indexed properties like arrays. Test this.
        /// </summary>
        /// <param name="source"></param>
        protected virtual void copyBusinessObjectProperties(T source)
        {
            List<string> properties = new List<string>();

            foreach (PropertyInfo pInfo in typeof(T).GetProperties())
            {
                if (!properties.Contains(pInfo.Name))
                {
                    if (pInfo.CanWrite)
                    {
						if ((!typeof(string).IsAssignableFrom(pInfo.PropertyType))
								&& (pInfo.PropertyType.GetProperties().Length > 0))
						{
							/* If the property type is an object that in turn has properties, then if it is not a IBindableWrapper,
									do not copy the property. That is: if the property is not being intercepted, do not change it
									when another property is changed.
								 */
						}
						else
						{
							if (pInfo.GetGetMethod().GetParameters().Length > 0)
							{
								// Do nothing on indexed properties
							}
							else
							{
								object val = pInfo.GetValue(source, null);
								typeof(T).GetProperty(pInfo.Name).SetValue(_bindableFields.BusinessObj, val, null);
							}
						}
						properties.Add(pInfo.Name);
                    }
                }
            }
        }

		/// <summary>
		/// FIXME: I think PropertyInfo.GetValue and SetValue passing in null as the last param throws 
		/// an exception on indexed properties like arrays. Test this.
		/// </summary>
		/// <param name="source"></param>
		protected virtual void initializeProxyFromBusinessObject(T source)
		{
			List<string> properties = new List<string>();

			foreach (PropertyInfo pInfo in typeof(T).GetProperties())
			{
				if (!properties.Contains(pInfo.Name))
				{
					if (pInfo.CanWrite)
					{
						if (pInfo.GetGetMethod().GetParameters().Length > 0)
						{
							// Do nothing on indexed properties
						}
						else
						{
							object val = pInfo.GetValue(source, null);
							typeof(T).GetProperty(pInfo.Name).SetValue(_bindableFields.BusinessObj, val, null);
						}
						properties.Add(pInfo.Name);
					}
				}
			}
		}

		protected virtual Y cloneBindable()
		{
			Y bindableClone = (Y)Cloner.GetSerializedClone(_bindableFields);
			return bindableClone;
		}

		#region Interceptor implementation
		public void Intercept(IInvocation invocation)
		{
			if (!_dontIntercept)
			{
				if (invocation.Method.Name.StartsWith("set_"))	// setting the property 
				{
					string propDirtyName = invocation.Method.Name;
					string propName = propDirtyName.Substring(propDirtyName.IndexOf('_') + 1);
					PropertyInfo propInfo;
					object oldValue = null;

					propInfo = typeof(Y).GetProperty(propName);
					if (propInfo == null)
					{
                        propInfo = typeof(T).GetProperty(propName);
                        oldValue = propInfo.GetValue(_bindableFields.BusinessObj, null);
                    }
					else
					{
						oldValue = propInfo.GetValue(_bindableFields, null);
					}

					if (!areEqual(oldValue, invocation.Arguments[0]))
					{
						if (!_disableUndoRedo)
						{
							_undoStack.Push(cloneBindable());
						}

						invocation.Proceed();
                        throwPropertyChangedIfSubscribers(propInfo.Name);
                        return;
					}			
				}
			}
			invocation.Proceed();
		}
		#endregion

        private bool areEqual(object obj1, object obj2)
        {
			if (obj1 == null || obj2 == null)
			{
				return obj1 == obj2;
			}
			
            if (obj1.GetType() == typeof(string))
            {
                if (obj1.ToString().CompareTo(obj2.ToString()) == 0)
                {
                    return true;
                }

                return false;
            }
            return (obj1.Equals(obj2));
        }

		/// <summary>
		/// There are 3 objects having a PropertyChanged event. This class, the _bindableFields and the Business Object.
		/// The event should be raised for all the subscribers in the 3 objects
		/// </summary>
		private void throwPropertyChangedIfSubscribers(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(_bindableFields, new PropertyChangedEventArgs(propertyName));
			}
			if (_bindableFields is ICustomNotifyPropertyChanged)
			{
				ICustomNotifyPropertyChanged notifiable = _bindableFields as ICustomNotifyPropertyChanged;
				if (notifiable.HasSubscribers())
				{
					notifiable.ThrowPropertyChangedEvent(propertyName);
				}
			}
			if (_bindableFields.BusinessObj is ICustomNotifyPropertyChanged)
			{
				ICustomNotifyPropertyChanged notifiable = _bindableFields.BusinessObj as ICustomNotifyPropertyChanged;
				if (notifiable.HasSubscribers())
				{
					notifiable.ThrowPropertyChangedEvent(propertyName);
				}
			}
		}

		/// <summary>
		/// this works for the properties of the business object but not for the properties of the _bindableFields. Fix this ASAP
		/// </summary>
		private void throwPropertyChangedAllProperties()
		{
            List<string> properties = new List<string>();

			foreach (PropertyInfo pInfo in typeof(T).GetProperties())
			{
                if (!properties.Contains(pInfo.Name))
                {
                    properties.Add(pInfo.Name);
				    throwPropertyChangedIfSubscribers(pInfo.Name);
                }
			}
		}

		[Serializable]
		public abstract class BindableFields<D> : ISimpleWrapper<D>
		{
			private D _businessObj;

			[field: NonSerialized]
			protected IInterceptor _interceptor;

			internal IInterceptor Interceptor
			{
				get
				{
					return _interceptor;
				}
				set
				{
					_interceptor = value;
				}
			}

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
		}

		
	}
}
