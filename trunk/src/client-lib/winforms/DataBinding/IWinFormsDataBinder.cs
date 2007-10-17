using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Boxerp.Client.WindowsForms
{
	public interface IWinFormsDataBinder<T, X, Y, Z> : IDataBinder<T, X, Y>
		where Z : IContainerControl
		where T : IBindableWrapper<X, Y>
		where Y : ISimpleWrapper<X>
	{
		Y Control
		{
			get;
			set;
		}
	}

	public interface IWinFormsDataBinder<X, Z> : IDataBinder<X>
		where Z : IContainerControl
	{
		Z Control
		{
			get;
			set;
		}
	}
}
