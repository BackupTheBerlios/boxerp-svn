using System;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Client
{
	/// <summary>
	/// Defines the behaviour of a View that can be closed
	/// </summary>
	public interface ICloseView : IView
	{
		void Close();
	}
}
