using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using EnvDTE80;

namespace Migrator
{
	public struct SelectedFile
	{
		public ProjectItem File;
		public bool NeedsSharedData;
	}
}
