using System;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Client
{
	public interface IControlManager<T> 
		where T : IView
	{
		void ShowControl(T control);
		void ShowControl(T control, string title);
	}

	public interface IControlManager
	{
		void ShowControl(IView control);
		void ShowControl(IView control, string title);
	}
}
