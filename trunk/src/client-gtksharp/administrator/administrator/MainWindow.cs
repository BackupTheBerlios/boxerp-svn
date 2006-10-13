using System;
using Gtk;

namespace administrator 
{
public class MainWindow: Gtk.Window
{
	protected MainHelper helper;
	protected widgets.AdvancedTreeView atreeviewEnterprises;
	protected widgets.AdvancedTreeView atreeviewUsers;
	protected widgets.AdvancedTreeView atreeviewGroups;
	
	public MainWindow (): base ("")
	{
		Stetic.Gui.Build (this, typeof(MainWindow));
		helper = new MainHelper(ref this.atreeviewEnterprises,
								ref this.atreeviewUsers, ref this.atreeviewGroups);
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
}
}
