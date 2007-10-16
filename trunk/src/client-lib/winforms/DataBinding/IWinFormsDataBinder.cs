using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Boxerp.Client.WindowsForms
{
	public interface IWinFormsDataBinder<T, Y> : IDataBinder<T>
		where Y : IContainerControl
		where T : IBindableWrapper<T>
	{
		Y Control
		{
			get;
			set;
		}
	}

	
}
