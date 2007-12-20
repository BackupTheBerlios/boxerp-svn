using System;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Client
{
	/// <summary>
	/// Strongly type version of IControlManager
	/// </summary>
	/// <param name="T">The View Interface</param>
	public interface IControlManager<T> 
		where T : IView
	{
		void ShowControl(T control);
		void ShowControl(T control, string title);
	}

	/// <summary>
	/// If a class implements IControlManager, it means that is is able to display Views.
	/// Sample: A View is able to pop up other views.
	/// </summary>
	public interface IControlManager
	{
		void ShowControl(IView control);
		void ShowControl(IView control, string title);
	}
}
