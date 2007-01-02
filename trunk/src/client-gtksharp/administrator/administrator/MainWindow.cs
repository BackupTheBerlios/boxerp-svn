using System;
using Boxerp.Models;
using Gtk;
using Boxerp.Client.GtkSharp.Lib;
using Boxerp.Client;

namespace administrator 
{
public class MainWindow: Gtk.Window
{
	protected MainHelper _helper;
	protected FilteredListView ftreeviewEnterprises;
	protected FilteredListView ftreeviewUsers;
	protected FilteredListView ftreeviewGroups;
	protected Gtk.Entry entryUser;
	protected Gtk.Entry entryGroup;
	protected Gtk.Entry entryEnterprise;
	protected EditUserWindow editUserWindow;
	
	public MainWindow (): base ("")
	{
		Stetic.Gui.Build (this, typeof(MainWindow));
		_helper = new MainHelper(this, ref ftreeviewEnterprises,
								ref ftreeviewUsers, ref ftreeviewGroups);
		_helper.StartTransfer(ResponsiveEnum.Read);
		
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
		//if (editUserWindow == null)
		//{
			editUserWindow = new EditUserWindow(_helper.Groups);
			editUserWindow.SaveSucessEvent += OnEditUserWindowSave;
		//}
		/*else
		{
		    editUserWindow.Clear();
		    editUserWindow.Reload();
		    editUserWindow.Present();
		}*/
	}

	protected virtual void OnEditUserClicked(object sender, System.EventArgs e)
	{
	    //Console.WriteLine("ON EDIT USER CLICKED");
		if (ftreeviewUsers.IsSelected())
		{
		    //Console.WriteLine("ok, is selected: " + ftreeviewUsers.SelectedObject);
			editUserWindow = new EditUserWindow(_helper.Groups, (User)ftreeviewUsers.SelectedObject);
	    }
		else
		{
		    editUserWindow = new EditUserWindow(_helper.Groups); // FIXME: get groups from client cache instead of passing in to this method
		}
		editUserWindow.SaveSucessEvent += OnEditUserWindowSave;
	}

    protected virtual void OnEditUserWindowSave(object sender, System.EventArgs e)
    {
        Console.WriteLine("save user ok");
        if ((editUserWindow.User != null) && (editUserWindow.IsNewUser))
        {
            User user = editUserWindow.User;
            ftreeviewUsers.InsertModel(user);
            editUserWindow.IsNewUser = false;
            /*ftreeviewUsers.TreeView.Selection.SelectAll();
            if (ftreeviewUsers.IsSelected())
            {
                Console.WriteLine("ok, is selected: " + ftreeviewUsers.SelectedObject);
            }*/
        }
    }
    
	protected virtual void OnDelUserClicked(object sender, System.EventArgs e)
	{
	    if (ftreeviewUsers.IsSelected())
		{
		    lock(_helper)
		    {
		        _helper.User = (User)ftreeviewUsers.SelectedObject;
		        _helper.StartAsyncCall(_helper.DeleteUser);
		    }
		}
		else
		{
		    WarningDialog wdialog = new WarningDialog();
		    wdialog.Message = "Please select an user first";
            wdialog.Present();		    
		}
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
