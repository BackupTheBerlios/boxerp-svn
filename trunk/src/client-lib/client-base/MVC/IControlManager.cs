using System;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Client
{
	public interface IControlManager<T> 
		where T : IControl
	{
		void ShowControl(T control);
		void ShowControl(T control, string title);
	}

	public interface IControlManager
	{
		void ShowControl(IControl control);
		void ShowControl(IControl control, string title);
	}
}
