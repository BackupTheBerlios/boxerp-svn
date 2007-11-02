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

			form.Show();//TODO: need the winforms application event loop to run ot update the binding - NUnitForms?

			Assert.AreEqual("Bob", form.NameTxt.Text);


			form.NameTxt.Text = "Alice";
			//TODO: need the winforms application event loop to run ot update the binding - NUnitForms?
			form.Hide();
			form.Show();
			Assert.AreEqual("Alice", bob.Name);

			bob.Name = "joe";
			//TODO: need the winforms application event loop to run ot update the binding - NUnitForms?
			Assert.AreEqual("joe", form.NameTxt.Text);


			form.Close();
		}

		[Test, Explicit]
		public void InteractiveWinFormsTest()
		{
			Person bob = new Person("Bob");
			TestForm form = new TestForm(bob);

			Application.Run(form);
			
		}

		public class TestForm : Form
		{
			TextBox name = new TextBox();
			TextBox _name = new TextBox();
			TextBox inputbox = new TextBox();
			Person person;
			FlowLayoutPanel flowPanel = new FlowLayoutPanel();
			public TestForm(Person person)
			{
				this.person = person;
				name.Name = "name";
				_name.Name = "_name";
				inputbox.Name = "inputbox";
				Control holder = flowPanel;
				Label lNewLabel = new Label() { Text = "name" };
                holder.Controls.Add(lNewLabel);
				holder.Controls.Add(name);
				holder.Controls.Add(new Label(){Text = "_name"});
				holder.Controls.Add(_name);
				holder.Controls.Add(new Label() { Text = "input"});
				holder.Controls.Add(inputbox);
				flowPanel.Dock = DockStyle.Fill;
				Controls.Add(flowPanel);
				BindableWrapper<Person> _bindable = new BindableWrapper<Person>(person);
				DataBinder<Person, Form> dataBinder  = new DataBinder<Person, Form>( _bindable, this);
				dataBinder.BindWithReflection();

				inputbox.TextChanged += (sender, ea) => person.Name = inputbox.Text;

				//inputbox.TextChanged += (sender, ea) => name.Text = inputbox.Text;

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


//		[Serializable]
//		public class Person : System.ComponentModel.INotifyPropertyChanged
//		{
//			private string name;
//			public virtual string Name
//			{
//				get { return name; }
//				set
//				{
//					if (name == value)
//						return;
//					name = value;
//					OnPropertyChanged("Name");
//				}
//			}
//
//			protected void OnPropertyChanged(string p)
//			{
//				if (null != PropertyChanged)
//					PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(p));
//			}
//			public Person()
//			{
//
//			}
//			/// <summary>
//			/// Initializes a new instance of the Person class.
//			/// </summary>
//			/// <param name="name"></param>
//			public Person(string name)
//			{
//				this.name = name;
//			}
//
//			#region INotifyPropertyChanged Members
//
//			public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
//
//			#endregion
//		}
	}
}
