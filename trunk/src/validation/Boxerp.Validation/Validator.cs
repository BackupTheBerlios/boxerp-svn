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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Boxerp.Validation
{
	public class Validator 
	{

		public static void Validate(Object businessObject)
		{
			Validate(businessObject, true);
		}

		public static void Validate(Object businessObject, bool recursive)
		{
			PropertyInfo[] properties = businessObject.GetType().GetProperties();
			foreach (PropertyInfo property in properties)
			{
				object[] attributes = property.GetCustomAttributes(true);
				object propertyValue = null;

				try
				{
					propertyValue = property.GetValue(businessObject, null);
				}
				catch (System.Reflection.TargetParameterCountException)
				{
					// 	Validation of collections not supported yet
				}
				catch (Exception ex)
				{
					throw ex;
				}

				if (attributes.Length != 0)
				{
					foreach (Attribute att in attributes)
					{
						ValidationConstraint constraints = ValidationConstraint.NoConstraints;
						string error = null;
						bool isBoxerpValidation = false;
						if (att is ValidateAttribute)	// if the validation attribute comes from Boxerp.Client
						{
							ValidateAttribute vAtt = (ValidateAttribute)att;
							constraints = vAtt.ValidationConstraint;
							error = vAtt.ErrorMsg;
							isBoxerpValidation = true;
						}
						else                            // if the validation attribute comes from MS Enterprise Library or any other 
						{
							error = property.Name;
							if (att.ToString().Contains("NotNull"))	// FIXME : change the string for a value from the app.config or web.config
							{
								constraints = constraints | ValidationConstraint.NotNull;
							}
							if (att.ToString().Contains("NotEmpty"))	// FIXME : change the string for a value from the app.config or web.config
							{
								constraints = constraints | ValidationConstraint.NotEmpty;
							}
						}

						if ((constraints == ValidationConstraint.NotNull) && (propertyValue == null))
						{
							throw new ValidationException(ValidationConstraint.NotNull, error, isBoxerpValidation);
						}
						if ((constraints == ValidationConstraint.NotEmpty) && (propertyValue == null))
						{
							throw new ValidationException(ValidationConstraint.NotEmpty, error, isBoxerpValidation);
						}
					}
				}
				if ((recursive) && (propertyValue != null))
				{
					Type valueType = propertyValue.GetType();
					if (valueType.IsClass && !valueType.IsPrimitive)		// validate recursively
					{
						Validate(propertyValue, recursive);
					}
				}
			}
		}

	}
}
