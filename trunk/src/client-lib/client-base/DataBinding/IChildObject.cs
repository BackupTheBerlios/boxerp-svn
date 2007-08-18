using System;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Client
{
	public interface IChildObject 
	{
		IChildObject Parent { get; set; }
	}
}
