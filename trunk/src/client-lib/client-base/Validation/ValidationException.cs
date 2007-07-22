using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Boxerp.Client
{
	

	public class ValidationException : Exception
	{
		private ValidationConstraint _validationConstraint;
		private PropertyInfo _property;
		private bool _isBoxerpValidationAttribute = true;

		public bool IsBoxerpValidationAttribute
		{
			get { return _isBoxerpValidationAttribute; }
			set { _isBoxerpValidationAttribute = value; }
		}

		public PropertyInfo Property
		{
			get { return _property; }
			set { _property = value; }
		}

		public ValidationConstraint ValidationConstraint
		{
			get { return _validationConstraint; }
			set { _validationConstraint = value; }
		}

		public ValidationException() { }

		public ValidationException(ValidationConstraint constraint, string msg) : base(msg) 
		{
			ValidationConstraint = constraint;
		}

		public ValidationException(ValidationConstraint constraint, string msg, bool isBoxerpValidationAttribute)
			: base(msg)
		{
			ValidationConstraint = constraint;
			_isBoxerpValidationAttribute = IsBoxerpValidationAttribute;
		}

		public ValidationException(ValidationConstraint constraint, PropertyInfo prop)
			: base(prop.ToString())
		{
			ValidationConstraint = constraint;
			Property = prop;

		}

		public ValidationException(ValidationConstraint constraint, PropertyInfo prop, string msg)
			: base(msg)
		{
			ValidationConstraint = constraint;
			Property = prop;

		}
	}
}
