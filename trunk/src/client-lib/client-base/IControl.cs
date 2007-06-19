using System;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Client
{
	/// <summary>
	/// All GUI controls should implement this interface (windows or custom controls)
	/// 
	/// </summary>
 
	public interface IControl
	{
		void Close();
	}
}
