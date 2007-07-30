//
// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
//
// Copyright (C) 2005,2006 Shidix Technologies (www.shidix.com)
//
// Redistribution and use in source and binary forms, with or
// without modification, are permitted provided that the following
// conditions are met:
// Redistributions of source code must retain the above
// copyright notice, this list of conditions and the following
// disclaimer.
// Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following
// disclaimer in the documentation and/or other materials
// provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
// PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR
// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Boxerp.Validation
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

		public ValidationException(ValidationConstraint constraint, string msg, bool isBoxerpValidation)
			: base(msg)
		{
			ValidationConstraint = constraint;
			_isBoxerpValidationAttribute = isBoxerpValidation;
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
