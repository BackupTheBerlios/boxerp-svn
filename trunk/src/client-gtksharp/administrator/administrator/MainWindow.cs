using System;
using Gtk;

namespace administrator 
{
public class MainWindow: Gtk.Window
{
	protected MainHelper helper;
	protected widgets.FilteredListView streeviewEnterprises;
	protected widgets.FilteredListView streeviewUsers;
	protected widgets.FilteredListView streeviewGroups;
	protected Gtk.Entry entryUser;
	protected Gtk.Entry entryGroup;
	protected Gtk.Entry entryEnterprise;
	protected EditUserWindow editUserWindow;
	
	public MainWindow (): base ("")
	{
		Stetic.Gui.Build (this, typeof(MainWindow));
		helper = new MainHelper(ref this.streeviewEnterprises,
								ref this.streeviewUsers, ref this.streeviewGroups);
		helper.Init(this);
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
		streeviewUsers.FilterRegex = entryUser.Text;
		streeviewUsers.Refilter();
	}

	protected virtual void OnFindGroup(object sender, System.EventArgs e)
	{
		streeviewGroups.FilterRegex = entryUser.Text;
		streeviewGroups.Refilter();
	}

	protected virtual void OnFindEnterprise(object sender, System.EventArgs e)
	{
		streeviewEnterprises.FilterRegex = entryGroup.Text;
		streeviewEnterprises.Refilter();
	}

	protected virtual void OnNewUserClicked(object sender, System.EventArgs e)
	{
		if (editUserWindow == null)
			editUserWindow = new EditUserWindow();
	}

	protected virtual void OnEditUserClicked(object sender, System.EventArgs e)
	{
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
