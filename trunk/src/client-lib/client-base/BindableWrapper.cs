using System;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Client
{
	public class BindableWrapper<T> : AbstractBindableWrapper<T, BindableWrapper<T>.WrapObject<T>>
	{
		public BindableWrapper(T businessObj)
		{
			Data = new BindableWrapper<T>.WrapObject<T>(businessObj);
		}

		public class WrapObject<D> : AbstractBindableWrapper<D, BindableWrapper<D>.WrapObject<D>>.BindableFields<D>
		{
			public WrapObject(D businessObj) : base (businessObj) {}
		}
	}
}
