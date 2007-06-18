using System;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Client
{
	public interface IMultipleObjectsReaderControl
	{
		void PopulateGui<T>(List<BindableWrapper<T>> list);
	}

	public interface ISingleObjectReaderControl
	{
		void PopulateGui<T>(BindableWrapper<T> item);
	}
}
