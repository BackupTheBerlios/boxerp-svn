using System;
using System.Collections.Generic;
using System.Text;

namespace Migrator
{
	public class Singleton
	{
		private static Singleton _instance = new Singleton();
		private string _sourcesHeader;
		private string _namespace;

		public string Namespc
		{
			get { return _namespace; }
			set { _namespace = value; }
		}

		public string SourcesHeader
		{
			get { return _sourcesHeader; }
			set { _sourcesHeader = value; }
		}

		public static Singleton Instance
		{
			get { return _instance; }
		}

		private Singleton() { }


		

	}
}
