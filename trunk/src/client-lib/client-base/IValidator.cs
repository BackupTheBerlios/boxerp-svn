using System;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Client
{
	public interface IValidator
	{
		bool Validate(Object businessObject);
	}
}
