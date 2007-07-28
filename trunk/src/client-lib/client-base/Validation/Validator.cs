using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Boxerp.Client
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
						if (att is Boxerp.Client.ValidateAttribute)	// if the validation attribute comes from Boxerp.Client
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
