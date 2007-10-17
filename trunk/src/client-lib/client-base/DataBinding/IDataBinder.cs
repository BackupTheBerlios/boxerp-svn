using System;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Client
{
	public interface IDataBinder
	{
		void BindWithXml();
		void BindWithReflection();
		void BindWithXml(string xml);
	}

	public interface IDataBinder<A, T, Y>
		where A : IBindableWrapper<T, Y>
		where Y : ISimpleWrapper<T>
	{
		void BindWithXml();
		void BindWithReflection();
		void BindWithXml(string xml);

		A BindableWrapper
		{
			get;
			set;
		}
	}

	public interface IDataBinder<X> : IDataBinder<BindableWrapper<X>, X, BindableWrapper<X>.WrapObject<X>>
	{

	}

	/* if would be fantastic if we could declare the interface above like this:
	 * 
	 * public interface IDataBinder<T<X>>
	 *    where T : IBindableWrapper<X>
	 * 
	 * But as far as I know it is not possible
	 */
}
