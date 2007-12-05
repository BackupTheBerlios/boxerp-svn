using System;
using System.Collections.Generic;
using System.Text;
using Boxerp.Client;
using Extensibility;
using System.Threading;
using EnvDTE;
using EnvDTE80;
using System.IO;

namespace Migrator
{
	public class MainController : AbstractController<IMain, MainController>
	{
		DTE _appObj;
		Dictionary<string, ProjectItem> _files = new Dictionary<string, ProjectItem>();
		Dictionary<string, ProjectItem> _folders = new Dictionary<string, ProjectItem>();
		Dictionary<string, Project> _projects = new Dictionary<string, Project>();
		List<SelectedFile> _selectedFiles;
		List<string> _viewMethods = new List<string>();
		ProjectItem _controllersFolder, _testViewsFolder, _interfacesFolder, _sharedDataFolder;
		string[] _fileExtensions = new string[] { "cs" };
		string _className;
		bool _createSharedData;
		bool _isWinForms;
		bool _isWPF;

		public bool IsWinForms
		{
			get { return _isWinForms; }
			set { _isWinForms = value; }
		}
		

		public bool IsWPF
		{
			get { return _isWPF; }
			set { _isWPF = value; }
		}

		public DTE AppObj
		{
			get { return _appObj; }
			set { _appObj = value; }
		}

		public MainController(IResponsiveClient helper, IMain view)
			: base(helper, view)
		{
		}

		private bool fileMatchExtension(string extension)
		{
			foreach (string ext in _fileExtensions)
			{
				if (ext == extension)
				{
					return true;
				}
			}

			return false;
		}

		public void RetrieveProjects()
		{
			foreach (Project prj in AppObj.Solution.Projects)
			{
				if ((prj.ProjectItems != null) && (prj.ProjectItems.Count > 0))
				{
					_projects[prj.Name] = prj;
				}
			}

			View.PopulateProjects(_projects);
		}

		public void RetrieveFiles(Project prj)
		{
			try
			{
				_files.Clear();
				_folders.Clear();
				retrieveFilesRecursively(prj.ProjectItems);
				
				View.PopulateFiles(_files);
				View.PopulateFolders(_folders);
			}
			catch (Exception ex)
			{

			}
		}

		private void retrieveFilesRecursively(ProjectItems items)
		{
			foreach (ProjectItem item in items)
			{
				string name = item.Name;
				string extension = String.Empty;
				System.Diagnostics.Debug.WriteLine("file:" + name);
				if (name.Contains("."))
				{
					extension = name.Substring(name.LastIndexOf('.') + 1);
				}
				if ((item.ProjectItems != null) && (item.ProjectItems.Count == 0))
				{
					if (fileMatchExtension(extension))
					{
						_files[item.Name] = item;
					}
				}
				else
				{
					// fixme: the folder might content dots and we would be missing them
					if (!item.Name.Contains("."))
					{
						_folders[item.Name] = item;
					}
					if (item.ProjectItems != null)
					{
						retrieveFilesRecursively(item.ProjectItems);
					}
				}
			}
		}

		public void Migrate(List<SelectedFile> files,
			ProjectItem controllersFolder, ProjectItem testViewsFolder, ProjectItem interfacesFolder,
			ProjectItem sharedDataFolder)
		{
			_controllersFolder = controllersFolder;
			_testViewsFolder = testViewsFolder;
			_interfacesFolder = interfacesFolder;
			_sharedDataFolder = sharedDataFolder;
			_selectedFiles = files;
			//ResponsiveHelper.StartAsyncCall(migrate);
			_viewMethods.Clear();
			migrate();
		}

		private void migrate()
		{
			foreach (SelectedFile sfile in _selectedFiles)
			{
				System.Diagnostics.Debug.WriteLine(sfile.File.Name);
				_className = sfile.File.Name.Split('.')[0];
				_createSharedData = sfile.NeedsSharedData;
				bool alreadyMigrated = false;
				foreach (ProjectItem item in _interfacesFolder.ProjectItems)
				{
					if (item.Name.ToString().Equals(CodeGenerator.CreateViewIfaceName(_className) + ".cs"))
					{
						alreadyMigrated = true;
						if (View.AskUser("This file seems to be migrated already. Are you sure you want to overwrite the files?"))
						{
							alreadyMigrated = false;
						}
					}
				}
				if (!alreadyMigrated)
				{
					processView(getFilePath(sfile.File));
				}
			}
		}

		private string getFilePath(ProjectItem item)
		{
			string[] path = item.ContainingProject.FullName.Split(Path.DirectorySeparatorChar);
			string finalPath = String.Empty;
			for (int i = 0; i < path.Length -1; i++)
			{
				finalPath += path[i] + Path.DirectorySeparatorChar; 
			}
			finalPath += item.Name;
			return finalPath;
		}

		private string getFilePath(ProjectItem folderItem, string filename)
		{
			return Path.Combine(getFilePath(folderItem), filename);
		}

		private void processView(string fullPath)
		{
			StringBuilder viewIfaceWriter = new StringBuilder();
			StringBuilder modifiedView = new StringBuilder();
			StreamReader reader = File.OpenText(fullPath);
			ParsingStatus status = new ParsingStatus();
			status = ParsingStatus.BeforeFindingClassDeclaration;
			do
			{
				parseLine(reader.ReadLine(), viewIfaceWriter, modifiedView, ref status);
			} while (!reader.EndOfStream);

			CodeGenerator.CloseViewInterface(viewIfaceWriter);
			reader.Close();
			File.WriteAllText(fullPath, modifiedView.ToString());
			string fileName = CodeGenerator.CreateViewIfaceName(_className) + ".cs";
			File.WriteAllText(getFilePath(_interfacesFolder, fileName), viewIfaceWriter.ToString());
			_interfacesFolder.ProjectItems.AddFromFile(getFilePath(_interfacesFolder, fileName));

			generateController();
			generateTestView();
			if (_createSharedData)
			{
				generateSharedData();
			}
		}

		private void parseLine(string line, StringBuilder viewIfaceWriter, StringBuilder modifiedView, ref ParsingStatus status)
		{
			if (Parser.IsClassDefinition(line))
			{
				status = ParsingStatus.AfterClassDeclarationLine;
				CodeGenerator.GenerateViewInterfaceDeclaration(_className, viewIfaceWriter, _createSharedData);
				modifiedView.AppendLine(line + ", " + CodeGenerator.CreateViewIfaceName(_className));
			}
			else
			{
				modifiedView.AppendLine(line);
			}
			if (Parser.IsPublicMethodDefinition(line))
			{
				_viewMethods.Add(line);
				CodeGenerator.AddMethodToInterface(line, viewIfaceWriter);
			}
			if (Parser.IsNamesSpaceDefinition(line))
			{
				Singleton.Instance.Namespc = Parser.GetNamesSpace(line);
			}
			
			if (Parser.IsOpenBracket(line))
			{
				if (status == ParsingStatus.AfterClassDeclarationLine)
				{
					status = ParsingStatus.ClassBody;
					CodeGenerator.AddControllerFieldToView(_className, modifiedView);
					if (_createSharedData)
					{
						CodeGenerator.AddDataFieldToView(_className, modifiedView, getGuiToolkit());
					}
				}
			}
		}

		private void generateController()
		{
			StringBuilder controllerBuilder = new StringBuilder();
			CodeGenerator.GenerateController(_className, controllerBuilder, _createSharedData);
			string controllerName = CodeGenerator.CreateControllerName(_className) + ".cs";
			File.WriteAllText(getFilePath(_controllersFolder, controllerName), controllerBuilder.ToString());
			_controllersFolder.ProjectItems.AddFromFile(getFilePath(_controllersFolder, controllerName));
		}

		private void generateTestView()
		{
			StringBuilder testViewBuilder = new StringBuilder();
			CodeGenerator.GenerateTestView(_className, testViewBuilder, _createSharedData, _viewMethods);
			string testViewName = CodeGenerator.CreateTestViewName(_className) + ".cs";
			File.WriteAllText(getFilePath(_testViewsFolder, testViewName), testViewBuilder.ToString());
			_testViewsFolder.ProjectItems.AddFromFile(getFilePath(_testViewsFolder, testViewName));
		}

		private void generateSharedData()
		{
			StringBuilder sharedDataIfaceBuilder = new StringBuilder();
			CodeGenerator.GenerateSharedDataIface(_className, sharedDataIfaceBuilder);
			string sharedDataIfaceName = CodeGenerator.CreateSharedDataIfaceName(_className) + ".cs";
			File.WriteAllText(getFilePath(_interfacesFolder, sharedDataIfaceName), sharedDataIfaceBuilder.ToString());
			_interfacesFolder.ProjectItems.AddFromFile(getFilePath(_interfacesFolder, sharedDataIfaceName));

			StringBuilder sharedDataAbstractBuilder = new StringBuilder();
			CodeGenerator.GenerateSharedDataAbstract(_className, sharedDataAbstractBuilder);
			string sharedDataAbstractName = CodeGenerator.CreateSharedDataAbstractName(_className) + ".cs";
			File.WriteAllText(getFilePath(_sharedDataFolder, sharedDataAbstractName), sharedDataAbstractBuilder.ToString());
			_sharedDataFolder.ProjectItems.AddFromFile(getFilePath(_sharedDataFolder, sharedDataAbstractName));

			StringBuilder sharedDataConcreteBuilder = new StringBuilder();
			CodeGenerator.GenerateSharedDataConcrete(_className, sharedDataConcreteBuilder, getGuiToolkit());
			string sharedDataConcreteName = CodeGenerator.CreateSharedDataConcreteName(_className, getGuiToolkit()) + ".cs";
			File.WriteAllText(getFilePath(_sharedDataFolder, sharedDataConcreteName), sharedDataConcreteBuilder.ToString());
			_sharedDataFolder.ProjectItems.AddFromFile(getFilePath(_sharedDataFolder, sharedDataConcreteName));

			StringBuilder sharedDataTestBuilder = new StringBuilder();
			CodeGenerator.GenerateSharedDataTest(_className, sharedDataTestBuilder);
			string sharedDataTestName = CodeGenerator.CreateSharedDataTestName(_className) + ".cs";
			File.WriteAllText(getFilePath(_sharedDataFolder, sharedDataTestName), sharedDataTestBuilder.ToString());
			_sharedDataFolder.ProjectItems.AddFromFile(getFilePath(_sharedDataFolder, sharedDataTestName));
		}

		private string getGuiToolkit()
		{
			return (_isWinForms ? "WinForms" : "WPF");	
		}

		protected override void OnAsyncOperationFinish(object sender, ThreadEventArgs args)
		{
			
		}
	}
}
