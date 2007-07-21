using System;
using System.Collections.Generic;
using System.Text;
using Boxerp.Client;

namespace ValidationTests
{
	public class OtherBusinessObject
	{
		private string _description = null;

		[Validate(ValidationConstraint.NotNull, ErrorMsg = "The Property cannot be null")]
		[Validate(ValidationConstraint.NotEmpty, ErrorMsg = "The Property cannot be empty")]
		public string Description
		{
			get
			{
				return _description;
			}

			set
			{
				_description = value;
			}
		}
	}
}
