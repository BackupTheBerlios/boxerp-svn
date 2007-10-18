using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Windows.Forms;
using Boxerp.Client.WindowsForms;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Boxerp.Client.WindowsForms.Tests
{
	[TestFixture]
	public class WinFormsDataBinderTests
	{
		[Test]
		public void BindsTextBox()
		{
			Person bob = new Person("Bob");
			TestForm form = new TestForm(bob);

			form.Show();

			Assert.AreEqual("Bob", form.NameTxt.Text);

			bob.Name = "joe";

			Assert.AreEqual("joe", form.NameTxt.Text);

			form.Close();
		}

		public class TestForm : Form
		{
			TextBox name = new TextBox();
			public TestForm(Person person)
			{
				name.Name = "name";
				BindableWrapper<Person> _bindable = new BindableWrapper<Person>(person);
				DataBinder<Person, Form> dataBinder  = new DataBinder<Person, Form>( _bindable, this);
				dataBinder.BindWithReflection();
				
			}
			public TextBox NameTxt
			{
				get { return name; }
			}
		}
		[Serializable]
		public class Person
		{
			private string name;
			public virtual string Name
			{
				get { return name; }
				set
				{
					name = value;
				}
			}
			public Person()
			{

			}
			/// <summary>
			/// Initializes a new instance of the Person class.
			/// </summary>
			/// <param name="name"></param>
			public Person(string name)
			{
				this.name = name;
			}
		}
	}
}
