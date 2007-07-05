using System;
using System.Collections.Generic;
using System.Text;
using Castle.DynamicProxy;

namespace Boxerp.Client
{
	public class BindableWithCollection<T, Y> : AbstractBindableWrapper<T, BindableWithCollection<T, Y>.WrapObject<T, Y>>
	{
		public BindableWithCollection(T businessObj)
			: base (businessObj, typeof(BindableWithCollection<T, Y>.WrapObject<T, Y>))
		{}

		public Type GetRelatedObjectType()
		{
			return typeof(Y);
		}

		public virtual Type GetCollectionType()
		{
			return typeof(List<Y>);
		}

		public class WrapObject<D, Z> : AbstractBindableWrapper<D, BindableWithCollection<D, Z>.WrapObject<D, Z>>.BindableFields<D>
		{
			private List<Z> _list = new List<Z>();

			public List<Z> Collection
			{
				get { return _list; }
				set { _list = value; }
			}
			
			public WrapObject(D businessObj, IInterceptor interceptor)
				: base(businessObj, interceptor)	{}
		}
	}
}
