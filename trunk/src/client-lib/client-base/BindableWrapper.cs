using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;

namespace Boxerp.Client
{
	/// <summary>
	/// This structure forces the devoloper who extends BindableWrapper to write a private nested class 
	/// which provide for the bindable fields
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class BindableWrapper<T> : AbstractBindableWrapper<T, BindableWrapper<T>.WrapObject<T>>
	{
		public BindableWrapper(T businessObj)
			: base(businessObj, typeof(BindableWrapper<T>.WrapObject<T>))
		{}

		public class WrapObject<D> : AbstractBindableWrapper<D, BindableWrapper<D>.WrapObject<D>>.BindableFields<D>
		{
			public WrapObject(D businessObj, IInterceptor interceptor) : 
				base (businessObj, interceptor) {}
		}
	}
}
