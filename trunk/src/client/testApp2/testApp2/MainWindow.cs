// /home/carlos/boxerp_completo/trunk/src/client/testApp2/testApp2/MainWindow.cs created with MonoDevelop
// User: carlos at 2:40 PM 7/7/2007
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//
using System;
using Gtk;
using testApp2;
using Boxerp.Client;
using Boxerp.Client.GtkSharp;

public partial class MainWindow: Gtk.Window
{	
	private BindableWrapper<SampleBObj> _bindable; 
	private bool _update = true;
	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		_bindable = new BindableWrapper<SampleBObj>(new SampleBObj());
		
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected virtual void OnNameChanged (object sender, System.EventArgs e)
	{
		if (_update)
		{
			Console.WriteLine("name changed:" + _name.Text);
			_bindable.Data.BusinessObj.Name = _name.Text;
		}
	}

	protected virtual void OnDescriptionChanged (object sender, System.EventArgs e)
	{
		if (_update)
		{
			Console.WriteLine("description changed:" + _description.Text);
			_bindable.Data.BusinessObj.Description = _description.Text;
		}
	}

	protected virtual void OnAgeChanged (object sender, System.EventArgs e)
	{
		if ((_update) && (_age.Text.Length > 0))
		{
			Console.WriteLine("age changed:" + _age.Text);
			_bindable.Data.BusinessObj.Age = Convert.ToInt32(_age.Text);
		}
	}

	protected virtual void OnRedoClicked (object sender, System.EventArgs e)
	{
		_bindable.ReDo();
		refresh();
	}

	protected virtual void OnUndoClicked (object sender, System.EventArgs e)
	{
		_bindable.UnDo();
		refresh();
	}
	
	private void refresh()
	{
		_update = false;
		_name.Text = _bindable.Data.BusinessObj.Name;
		_description.Text = _bindable.Data.BusinessObj.Description;
		_age.Text = _bindable.Data.BusinessObj.Age.ToString();
		_update = true;
	}
}