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

	public interface IDataBinder<T>
		where T : IBindableWrapper<T>
	{
		void BindWithXml();
		void BindWithReflection();
		void BindWithXml(string xml);

		T BindableWrapper
		{
			get;
			set;
		}
	}
}
