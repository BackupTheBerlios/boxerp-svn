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

// created on 12/10/2006 at 13:57
using System;

namespace Boxerp.Client
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
	public class ValidateAttribute : Attribute
	{
		private ValidationConstraint _validationConstraint;
		private string _errorMsg;

		public string ErrorMsg
		{
			get { return _errorMsg; }
			set { _errorMsg = value; }
		}

		public ValidationConstraint ValidationConstraint
		{
			get { return _validationConstraint; }
			set { _validationConstraint = value; }
		}

		public ValidateAttribute(ValidationConstraint constraint)
		{
			_validationConstraint = constraint;
		}
		
		public ValidateAttribute(ValidationConstraint constraint, string errorMsg)
		{
			_validationConstraint = constraint;
			_errorMsg = errorMsg;
		}

	}
}
