using System;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Client
{
	/// <summary>
	/// To build tree structures
	/// </summary>
	public interface IChildObject 
	{
		IChildObject Parent { get; set; }
	}
}
