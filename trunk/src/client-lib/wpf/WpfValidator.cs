using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;

namespace Boxerp.Client.WPF
{
	public class WpfValidator : IValidator
	{

		public bool Validate(object businessObject)
		{
			try
			{
				Validator.Validate(businessObject);
				return true;
			}
			catch (ValidationException ex)
			{
				if (ex.IsBoxerpValidationAttribute)
				{
					MessageBox.Show(ex.Message);
				}
				else
				{
					if (ex.ValidationConstraint == ValidationConstraint.NotNull)
					{
						MessageBox.Show("The " + ex.Message + " field cannot be null");
					}
					if (ex.ValidationConstraint == ValidationConstraint.NotEmpty)
					{
						MessageBox.Show("The " + ex.Message + " field cannot be empty");
					}
				}

				return false;
			}
		}
	}
}
