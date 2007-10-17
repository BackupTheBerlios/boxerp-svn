using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Boxerp.Client;
using Boxerp.Client.WindowsForms;
using System.Windows.Forms;

namespace dataBinderTests
{

	[TestFixture]
	public class DataBinderTests
	{
		[Test]
		public void DataBinderCreation()
		{
			Boxerp.Client.WindowsForms.DataBinder<SampleObj, Form> dataBinder =
				new Boxerp.Client.WindowsForms.DataBinder<SampleObj, Form>(new SampleObj(), new Form());
		}
	}
}
