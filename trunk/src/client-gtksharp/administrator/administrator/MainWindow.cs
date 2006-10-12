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
	protected string session;
	
	public MainWindow (string session): base ("")
	{
		Stetic.Gui.Build (this, typeof(MainWindow));
		this.session = session;
		helper = new MainHelper(ref this.atreeviewEnterprises,
								ref this.atreeviewUsers, ref this.atreeviewGroups,
								session);
		helper.Init(this);
		Console.WriteLine("sesssss=" + session);
		helper.StartDownload();
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
}