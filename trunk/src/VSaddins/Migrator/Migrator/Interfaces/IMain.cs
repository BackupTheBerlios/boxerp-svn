using System;
using System.Collections.Generic;
using System.Text;
using Boxerp.Client;
using EnvDTE;
using EnvDTE80;

namespace Migrator
{
	public interface IMain : IView<MainController>
	{
		void PopulateFolders(Dictionary<string, ProjectItem> items);
		void PopulateProjects(Dictionary<string, Project> items);
		void PopulateFiles(Dictionary<string, ProjectItem> items);
		bool AskUser(string msg);
	}
}
