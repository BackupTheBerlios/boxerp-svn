using System;
using NUnit.Framework;
using Boxerp.Client;
using bindableObjectsTests;
using System.ComponentModel;

namespace Boxerp.Client.Tests
{
	[TestFixture]
	public class BindableCollectionTests
	{
		[Test]
		public void IsSerializable()
		{
			BindableCollection<SimpleBusinessObject> lBindableCollection = new BindableCollection<SimpleBusinessObject>();
			lBindableCollection.Add(new SimpleBusinessObject());
			lBindableCollection[0].Name = "Alice";
			lBindableCollection[0].Age = 30;
			lBindableCollection[0].Description = "wants to get an encrypted message from bob :)";

			System.Runtime.Serialization.Formatters.Binary.BinaryFormatter lBinaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			System.IO.MemoryStream lMemoryStream = new System.IO.MemoryStream();
			lBinaryFormatter.Serialize(lMemoryStream, lBindableCollection);
		}

		[Test]
		public void ItemAddedEventRaised()
		{
			BindableCollection<SimpleBusinessObject> lBindableCollection = new BindableCollection<SimpleBusinessObject>();
			bool itemAddedRaised = false;
			
			lBindableCollection.ItemAddedEvent += delegate(object sender, AddingNewEventArgs ea)
			{
				itemAddedRaised = true;
			};
			Assert.IsFalse(itemAddedRaised);

			lBindableCollection.Add(new SimpleBusinessObject());

			Assert.IsTrue(itemAddedRaised);
			
		}

		[Test]
		public void RemoveEventRaised()
		{
			BindableCollection<SimpleBusinessObject> lBindableCollection = new BindableCollection<SimpleBusinessObject>();
			bool removingRaised = false;
			lBindableCollection.ItemRemovedEvent += delegate(object sender, ItemRemovedEventArgs ea)
				{
					removingRaised = true;
				};

			Assert.IsFalse(removingRaised);

			lBindableCollection.Add(new SimpleBusinessObject());

			Assert.IsFalse(removingRaised);

			lBindableCollection.RemoveAt(0);

			Assert.IsTrue(removingRaised);

			SimpleBusinessObject sbo = new SimpleBusinessObject();
			lBindableCollection.Add(sbo);
			removingRaised = false;

			lBindableCollection.Remove(sbo);

			Assert.IsTrue(removingRaised);
		}
	}
}
