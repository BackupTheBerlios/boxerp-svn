using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Windows.Forms;
using Boxerp.Client.WindowsForms;

namespace Boxerp.Client.WindowsForms.Tests
{
	[TestFixture]
	public class WinFormsBindableWrapperTests
	{
		[Test]
		public void BindsTextBox()
		{
			Person bob = new Person("Bob");
			TestForm form = new TestForm(bob);

			Assert.AreEqual("Bob", form.NameTxt.Text);

			bob.Name = "joe";

			Assert.AreEqual("joe", form.NameTxt.Text);
		}


		public class TestForm : Form
		{
			TextBox name = new TextBox();
			public TestForm(Person person)
			{
				name.Name = "name";
				WinFormsBindableWrapper<Person> _bindable = new WinFormsBindableWrapper<Person>(person,this);
				
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
