using System;
using Gtk;
using Glade;

public class Tree {
                
	[Widget] Gtk.Window window;
	[Widget] ScrolledWindow scrolledwindow1; //para colocar dentro el texto
	[Widget] Button b_agregar;
	[Widget] Button b_eliminar;
	[Widget] Button b_limpiar;
	[Widget] Button b_salir;
	TreeView tv;
	TreeStore store;
	Entry entryNombre;
	Entry entryApe;
                
	public static void Main (string [] args)
	{              
		new Tree (args);
	}
	
	
	public Tree (string[] args)
	{
		Application.Init ();
		
		store = new TreeStore (typeof (string), typeof (string));
		
		Glade.XML gxml = new Glade.XML(null, "tree.glade", "window", null );
		gxml.Autoconnect( this );
		
		window.Resize(500, 400 );
		// eventos
		window.DeleteEvent += new DeleteEventHandler( Salir );
		b_agregar.Clicked += new EventHandler( Agregar );
		b_eliminar.Clicked += new EventHandler( Borrar );
		b_limpiar.Clicked += new EventHandler( Limpiar );
		b_salir.Clicked += new EventHandler( Cerrar );
		
		// crear arbol
		
		tv = new TreeView ();
		tv.Model = store;
		tv.HeadersVisible = true;
		
		tv.AppendColumn ("Nombre", new CellRendererText (), "text", 0);
		tv.AppendColumn ("Apellidos", new CellRendererText (), "text", 1);
		              
		scrolledwindow1.Add (tv);
		window.ShowAll ();
		Application.Run ();	
	}
	
	void Agregar( object o, EventArgs args )
	{
		// crear dialogo
		Dialog dialog = new Dialog ("Agregar", window, Gtk.DialogFlags.DestroyWithParent);
		dialog.Modal = true;
		dialog.AddButton ("Aceptar", ResponseType.Ok);
		dialog.AddButton ("Cerrar", ResponseType.Close);
		Label lab = new Label("Información que desea agregar:");
		lab.Show();
		dialog.VBox.PackStart (lab , true, true, 5 );
		Table table = new Table (2, 2, false);
		Label labNombre = new Label("Nombre:");
		labNombre.Show();
		table.Attach(labNombre , 0, 1, 0, 1);
		entryNombre = new Entry();
		entryNombre.Show();
		table.Attach(entryNombre, 1, 2, 0, 1);
		Label labApe = new Label("Apellidos:");
		labApe.Show();
		table.Attach(labApe , 0, 1, 1, 2);
		entryApe = new Entry();
		entryApe.Show();
		table.Attach(entryApe , 1, 2, 1, 2);
		table.Show();
		dialog.VBox.PackStart (table, true, true, 5 );
		dialog.Response += new ResponseHandler (on_dialog_agregar);
		dialog.Run ();
		dialog.Destroy ();
		
	}
	
	void on_dialog_agregar (object obj, ResponseArgs args)
	{
		switch ((ResponseType)args.ResponseId) {
			case ResponseType.Ok: 
				TreeIter iter = store.AppendValues (entryNombre.Text, entryApe.Text);
			break;
		}
	}
	
	void Borrar( object o, EventArgs args )
	{
                TreeIter iter;
                TreeModel model;
 
                if (tv.Selection.GetSelected (out model, out iter))
                {
                	store.Remove(ref iter);
                }
	}
	
	void Limpiar( object o, EventArgs args )
	{
		store.Clear();
	}
	
	void Cerrar( object o, EventArgs args )
	{
		Application.Quit();
	}
	
	void Salir(object o, DeleteEventArgs args)
	{
		Application.Quit();
	}
	
}