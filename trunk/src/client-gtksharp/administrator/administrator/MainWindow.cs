using System;
using Gtk;

namespace administrator 
{
public class MainWindow: Gtk.Window
{
	protected MainHelper helper;
	protected widgets.SimpleTreeView streeviewEnterprises;
	protected widgets.SimpleTreeView streeviewUsers;
	protected widgets.SimpleTreeView streeviewGroups;
	protected Gtk.Entry entryUser;
	protected Gtk.Entry entryGroup;
	protected Gtk.Entry entryEnterprise;
	
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
}
}
