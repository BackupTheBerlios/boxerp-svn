using System;
using System.Collections.Generic;
using System.Text;
using Boxerp.Client;
using Boxerp.Validation;

namespace ValidationTests
{
	public class SimpleBusinessObject
	{
		private string _name = null;

		[Validate(ValidationConstraint.NotNull | ValidationConstraint.NotEmpty, ErrorMsg="The Name cannot be empty")]
		public string Name
		{
			get
			{
				return _name;
			}

			set
			{
				_name = value;
			}
		}
	}
}
