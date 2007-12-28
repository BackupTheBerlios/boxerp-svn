using System;
using Gtk;
using Boxerp.Client;
using Boxerp.Client.GtkSharp;

namespace  gtkResponsivenessTest
{
public partial class MainWindow: Gtk.Window, ISampleView
{	
	private Controller _modalController;
	private Controller _singletonController;
	private Controller _parallelController;

	public Controller Controller
	{
		get
		{
			return _modalController;
		}
		set
		{
		}
	}

	private void stressTheEngine()
	{
		Random random = new Random();
		for (int i = 0; i < 100; i++)
		{
			_modalController.DoAsyncOperation();
			_singletonController.DoAsyncOperation();
			_parallelController.DoAsyncOperation();
			System.Threading.Thread.Sleep(random.Next(3));
			Console.Out.WriteLine("Iteration:" + i);
		}
	}

	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		_modalController = new Controller(new GtkResponsiveHelper(ConcurrencyMode.Modal), this);
		_singletonController = new Controller(new GtkResponsiveHelper(ConcurrencyMode.SingletonThread), this);
		_parallelController = new Controller(new GtkResponsiveHelper(ConcurrencyMode.Parallel), this);		
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected virtual void OnClick (object sender, System.EventArgs e)
	{
		stressTheEngine();
	}
}
}