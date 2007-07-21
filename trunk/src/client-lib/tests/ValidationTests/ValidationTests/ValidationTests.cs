using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Boxerp.Client;

namespace ValidationTests
{
	[TestFixture]
	public class ValidationTests
	{

		[Test]
		public void SimpleValidationFail()
		{
			SimpleBusinessObject simpleBo = new SimpleBusinessObject();
			try
			{
				Validator.Validate(simpleBo);
			}
			catch (ValidationException ex)
			{
				Assert.AreEqual(ex.ValidationConstraint, ValidationConstraint.NotNull);
				Assert.AreEqual(ex.Property, simpleBo.GetType().GetProperties()[0]);
			}
		}

		[Test]
		public void SimpleValidationSuccess()
		{
			SimpleBusinessObject simpleBo = new SimpleBusinessObject();
			simpleBo.Name = "notnullsomething";
			Validator.Validate(simpleBo);
			Assert.IsTrue(true);
		}

		[Test]
		public void SimpleValidationFailEmpty()
		{
			SimpleBusinessObject simpleBo = new SimpleBusinessObject();
			try
			{
				simpleBo.Name = String.Empty;
				Validator.Validate(simpleBo);
			}
			catch (ValidationException ex)
			{
				Assert.AreEqual(ex.ValidationConstraint, ValidationConstraint.NotEmpty);
				Assert.AreEqual(ex.Message, "The Name cannot be empty");
			}
		}

		[Test]
		public void MultipleValidationSuccess()
		{
			SimpleBusinessObject simpleBo = new SimpleBusinessObject();
			simpleBo.Name = "notnullsomething";
			Validator.Validate(simpleBo);
			Assert.IsTrue(true);
		}

		[Test]
		public void MultipleValidationExceptions()
		{
			SimpleBusinessObject simpleBo = new SimpleBusinessObject();
			try
			{
				Validator.Validate(simpleBo);
			}
			catch (ValidationException ex)
			{
				Assert.AreEqual(ex.ValidationConstraint, ValidationConstraint.NotNull);
				Assert.AreEqual(ex.Message, "The Property cannot be null");
			}

			simpleBo.Name = String.Empty;

			try
			{
				Validator.Validate(simpleBo);
			}
			catch (ValidationException ex)
			{
				Assert.AreEqual(ex.ValidationConstraint, ValidationConstraint.NotEmpty);
				Assert.AreEqual(ex.Message, "The Property cannot be null");
			}
		}
	}
}
