using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using EnvDTE;
using EnvDTE80;

namespace Migrator
{
	public partial class Main : Form, IMain
	{
		MainController _controller;
		Dictionary<string, ProjectItem> _filesDict;
		Dictionary<string, ProjectItem> _foldersDict;
		Dictionary<string, Project> _projectsDict;

		public MainController Controller
		{
			get { return _controller; }
			set { _controller = value; }
		}

		public Main()
		{
			InitializeComponent();
		}

		private void OnClose(object sender, EventArgs e)
		{
			Close();
		}

		private bool validate()
		{
			if ((_controllers.SelectedItem == null) ||
				(_testViews.SelectedItem == null) ||
				(_interfaces.SelectedItem == null) ||
				(_sharedData.SelectedItem == null))
			{
				return false;
			}

			return true;
		}

		private void OnMigrate(object sender, EventArgs e)
		{
			List<SelectedFile> files = new List<SelectedFile>();
			foreach (ListViewItem item in _files.SelectedItems)
			{
				SelectedFile sfile = new SelectedFile();
				sfile.File = _filesDict[item.Text];
				sfile.NeedsSharedData = item.Checked;
				files.Add(sfile);
			}
			if (validate())
			{
				_statusLabel.Text = "Generating code...";
				this.UseWaitCursor = true;
				this.Cursor = Cursors.WaitCursor;
				this.Refresh();
				Singleton.Instance.SourcesHeader = _header.Text;
				Controller.IsWinForms = _winForms.Checked;
				Controller.IsWPF = _wpf.Checked;
				Controller.Migrate(files,
					_foldersDict[_controllers.SelectedItem.ToString()],
					_foldersDict[_testViews.SelectedItem.ToString()],
					_foldersDict[_interfaces.SelectedItem.ToString()],
					_foldersDict[_sharedData.SelectedItem.ToString()]);
				_statusLabel.Text = "Done";
				this.UseWaitCursor = false;
				this.Cursor = Cursors.Default;
			}
			else
			{
				MessageBox.Show("Please make a choise in all the dropdowns");
			}
		}

		#region IMain Members

		public void PopulateFolders(Dictionary<string, ProjectItem> items)
		{
			_foldersDict = items;
			_controllers.Items.Clear();
			_sharedData.Items.Clear();
			_interfaces.Items.Clear();
			_testViews.Items.Clear();
			foreach (ProjectItem item in items.Values)
			{
				_controllers.Items.Add(item.Name);
				_sharedData.Items.Add(item.Name);
				_interfaces.Items.Add(item.Name);
				_testViews.Items.Add(item.Name);
			}
		}

		public void PopulateFiles(Dictionary<string, ProjectItem> items)
		{
			_filesDict = items;
			_files.Items.Clear();
			foreach (ProjectItem item in items.Values)
			{
				_files.Items.Add(item.Name);
			}
		}

		public void PopulateProjects(Dictionary<string, Project> items)
		{
			_projectsDict = items;
			_projects.Items.Clear();
			foreach (Project item in items.Values)
			{
				_projects.Items.Add(item.Name);
			}
		}

		public bool AskUser(string msg)
		{
			return (MessageBox.Show(msg, "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes);
		}

		public void SetStatus(string msg)
		{
			_statusLabel.Text = msg;
			this.Refresh();
		}

		#endregion

		private void OnProjectChanged(object sender, EventArgs e)
		{
			Controller.RetrieveFiles(_projectsDict[_projects.SelectedItem as string]);
		}
	}
}