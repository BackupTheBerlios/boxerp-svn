using System;
using Boxerp.Models;
using Gtk;

namespace administrator 
{
public class MainWindow: Gtk.Window
{
	protected MainHelper helper;
	protected widgets.FilteredListView ftreeviewEnterprises;
	protected widgets.FilteredListView ftreeviewUsers;
	protected widgets.FilteredListView ftreeviewGroups;
	protected Gtk.Entry entryUser;
	protected Gtk.Entry entryGroup;
	protected Gtk.Entry entryEnterprise;
	protected EditUserWindow editUserWindow;
	
	public MainWindow (): base ("")
	{
		Stetic.Gui.Build (this, typeof(MainWindow));
		helper = new MainHelper(this, ref ftreeviewEnterprises,
								ref ftreeviewUsers, ref ftreeviewGroups);
		helper.StartDownload();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected virtual void OnTreeviewUsersActivated(object o, Gtk.RowActivatedArgs args)
	{
		Console.WriteLine("ACTIVATED");
		Console.WriteLine("Users activated:" + args.RetVal);
	}

	protected virtual void OnFindUser(object sender, System.EventArgs e)
	{
		ftreeviewUsers.FilterRegex = entryUser.Text;
		ftreeviewUsers.Refilter();
	}

	protected virtual void OnFindGroup(object sender, System.EventArgs e)
	{
		ftreeviewGroups.FilterRegex = entryUser.Text;
		ftreeviewGroups.Refilter();
	}

	protected virtual void OnFindEnterprise(object sender, System.EventArgs e)
	{
		ftreeviewEnterprises.FilterRegex = entryGroup.Text;
		ftreeviewEnterprises.Refilter();
	}

	protected virtual void OnNewUserClicked(object sender, System.EventArgs e)
	{
		if (editUserWindow == null)
			editUserWindow = new EditUserWindow();
	}

	protected virtual void OnEditUserClicked(object sender, System.EventArgs e)
	{
		if (ftreeviewUsers.IsSelected())
			editUserWindow = new EditUserWindow((User)ftreeviewUsers.SelectedObject);
		else
			editUserWindow = new EditUserWindow();
	}

	protected virtual void OnDelUserClicked(object sender, System.EventArgs e)
	{
	}

	protected virtual void OnDelGroupClicked(object sender, System.EventArgs e)
	{
	}

	protected virtual void OnEditGroupClicked(object sender, System.EventArgs e)
	{
	}

	protected virtual void OnNewGroupClicked(object sender, System.EventArgs e)
	{
	}

	protected virtual void OnNewEnterpriseClicked(object sender, System.EventArgs e)
	{
	}

	protected virtual void OnEditEnterpriseClicked(object sender, System.EventArgs e)
	{
	}

	protected virtual void OnDelEnterpriseClicked(object sender, System.EventArgs e)
	{
	}
}
}
