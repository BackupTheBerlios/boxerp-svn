using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Reflection;
using Castle.DynamicProxy;

namespace Boxerp.Client
{
	public abstract class AbstractBindableWrapper<T, Y> : StandardInterceptor, 
		IBindableWrapper<T> where Y : AbstractBindableWrapper<T, Y>.BindableFields<T>
	{
		private Y _bindableFields;
		private Stack<Y> _undoStack = new Stack<Y>();
		private Stack<Y> _redoStack = new Stack<Y>();
		private ProxyGenerator _generator = new ProxyGenerator();
		private bool _dontIntercept = false;

		public AbstractBindableWrapper(T businessObj, Type wrapper)
		{
			lock (this)
			{
				_bindableFields = (Y)_generator.CreateClassProxy(wrapper, this, businessObj, this);
				copyBOtoProxy(businessObj);
			}
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

		public void UnDo()
		{
			if (_undoStack.Count > 0)
			{
				_redoStack.Push(_bindableFields);
				if (_undoStack.Count > 0)
				{
					_bindableFields = _undoStack.Pop();
				}
			}
		}

		public void ReDo()
		{
			if (_redoStack.Count > 0)
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
				object copyValue = originalProp.GetValue(original, null);

				if (copyValue is ICloneable)
				{
					if (copyValue is T)	// not only clone but create a proxy
					{
						T proxyCopy = (T)_generator.CreateClassProxy(typeof(T), this);
						copyBOtoProxy(proxyCopy, (T)((ICloneable)copyValue).Clone());
						copyProp.SetValue(copy, proxyCopy, null);
					}
					else
					{
						copyProp.SetValue(copy, ((ICloneable)copyValue).Clone(), null);
					}
				}
				else if (copyValue is IEnumerable)
				{
					IList enumerable = (IList)copyValue;

					int j = 0;
					foreach (object value in enumerable)
					{
						if (value is ICloneable)
						{
							enumerable[j] = ((ICloneable)value).Clone();
						}
						j++;
					}
				}
				i++;
			}
			_dontIntercept = false;
			return (Y)copy;
		}

		#region Interceptor implementation
		public override object Intercept(IInvocation invocation, params object[] args)
		{
			if (!_dontIntercept)
			{
				if (invocation.Method.Name.Contains("set_"))	// setting the property value
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
								break;
							}
						}
					}
					else
					{
						oldValue = propInfo.GetValue(_bindableFields, null);
					}

					if (oldValue != args[0])
					{
						_undoStack.Push(cloneBindable(_bindableFields, _bindableFields.SwallowCopy()));		// property is gonna change, put it in the undo stack
					}

					base.Intercept(invocation, args);	// set the property
				}
			}
			return base.Intercept(invocation, args);
		}
		#endregion



		public abstract class BindableFields<D>
		{
			private bool _isDirty = false;
			private D _businessObj;
			private ProxyGenerator _proxyGenerator = new ProxyGenerator();

			public BindableFields(D businessObj, IInterceptor interceptor)
			{
				_businessObj = (D)_proxyGenerator.CreateClassProxy(typeof(D), interceptor);
			}

			public bool IsDirty
			{
				get { return _isDirty; }
				set { _isDirty = value; }
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

			public BindableFields<D> SwallowCopy()
			{
				return (BindableFields<D>)MemberwiseClone();
			}
		}
	}
}
