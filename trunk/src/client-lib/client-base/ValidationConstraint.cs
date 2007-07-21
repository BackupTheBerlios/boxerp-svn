using System;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Client
{
	[Flags]
	public enum ValidationConstraint
	{
		NoConstraints,
		NotNull,
		NotEmpty
	};
}
