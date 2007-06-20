
using System;
using System.Collections.Generic;
using Gtk;
using System.Collections;


namespace Boxerp.Client.GtkSharp

{
	// TODO: Thinking about write a complete treeview wrapper instead
	// of access the TreeView property.
	
	public class SimpleListView : global::Gtk.Bin
	{
		protected ListStore store;
		public Gtk.TreeView treeview;
		public event Gtk.RowActivatedHandler RowActivatedEvent;
		public event System.EventHandler ColumnsChangedEvent;
		
		public TreeView TreeView 
		{
			get { return treeview;}
		}
		
		public SimpleListView()
		{
			Stetic.Gui.Build(this, typeof(Boxerp.Client.GtkSharp.SimpleListView));
			treeview.RowActivated += new Gtk.RowActivatedHandler(OnRowActivated);
			treeview.ColumnsChanged += new System.EventHandler(OnColumnsChanged);
		}
		
		public void Create(List<SimpleColumn> columns)
		{
			try
			{
				
				System.Type[] columnsTypes = new Type[columns.Count];
				int i = 0;

				foreach (SimpleColumn column in columns)
        	    {
        	    	columnsTypes[i++] = column.Type;
        	    }
        	    store = new ListStore(columnsTypes);
        	    treeview.Model = store;
        	    //Console.WriteLine("create3");
        	    i = 0;
        	    //Console.WriteLine("create4");
				foreach (SimpleColumn column in columns)
				{
					//Console.WriteLine("COLUMN TYPE=" + column.Type.ToString());
                	if (column.Type == typeof(Gdk.Pixbuf))
                	{
                		//Console.WriteLine("create5 loop");
			        	TreeViewColumn tc = treeview.AppendColumn ("", new CellRendererPixbuf (), "pixbuf", i++);
			        	//Console.WriteLine("create6 loop");
				    	if (tc != null)
					    	tc.Visible = column.Visible;
					}
					else if (column.Type == typeof(System.Object)) 
					{
						Gtk.TreeViewColumn objColumn = new Gtk.TreeViewColumn ();
						objColumn.Title = column.Name;
						Gtk.CellRendererText objCell = new Gtk.CellRendererText ();
						objColumn.PackStart (objCell, true);
						objColumn.SetCellDataFunc (objCell, new Gtk.TreeCellDataFunc (RenderObject));
				
						treeview.AppendColumn(objColumn);
			        	//TreeViewColumn tc = treeview.AppendColumn ("", new CellRendererPixbuf (), "object", i++);
				    	//tc.Visible = column.Visible;
					}
                	else
                	{
                		
                		//Console.WriteLine("create 7 loop:" + column.Name);
                		if (treeview == null)
                			treeview = new Gtk.TreeView();
				    	TreeViewColumn tc = treeview.AppendColumn (column.Name, new CellRendererText (), "text", i++);
				    	//Console.WriteLine("create8 loop");
				    	if (tc != null)
				    		tc.Visible = column.Visible;
                	}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Create exception:" + ex.Message + ex.StackTrace);
			}
		}
		
		public TreeIter InsertRow (ArrayList row)
		{
			TreeIter iter = store.Append();
			if (row == null)
			{
				Console.WriteLine("simple list row null");
				return Gtk.TreeIter.Zero;
			}
            for (int i = 0; i < row.Count; i++)
            {
            	Console.WriteLine("insertRow:" + i);
				store.SetValue (iter, i, row[i]);
				Console.WriteLine("Inserting oclumn=" + row[i].GetType().ToString());	
			}
			return iter;
		}
		
		public TreeIter InsertModel (System.Object model)
		{
		    TreeIter iter = store.Append();
		    if (model != null)
		    {
		        //store.SetValue(iter, 0, model.Id.ToString());
		        store.SetValue(iter, 0, model);
		        return iter;
		    }
		    else
		    {
		        return Gtk.TreeIter.Zero;
		    }
		}
		
		public void DeleteModelById(int id)
		{
		    TreeIter iter = TreeIter.Zero;
		    store.GetIterFirst(out iter);
		    do
		    {
		        string tmpId = (string)store.GetValue(iter, 0);
		        if (tmpId == id.ToString())
		        {
		            store.Remove(ref iter);
		            break;
		        }
		    } while (store.IterNext(ref iter));
		}
		
		public void DeleteByColumnId(int column, int id)
		{
		    TreeIter iter = TreeIter.Zero;
		    store.GetIterFirst(out iter);
		    do
		    {
		        string tmpId = (string)store.GetValue(iter, column);
		        if (tmpId == id.ToString())
		        {
		            store.Remove(ref iter);
		            break;
		        }
		    } while (store.IterNext(ref iter));
		}

		private void RenderObject (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			object item = model.GetValue (iter, 1);
			(cell as Gtk.CellRendererText).Text = item.ToString();
		}

		public void OnRowActivated (object o, Gtk.RowActivatedArgs args)
		{
			if (RowActivatedEvent != null)
				RowActivatedEvent(o, args);
		}
		
		public void OnColumnsChanged(object o, System.EventArgs args)
		{
			if (ColumnsChangedEvent != null)
				ColumnsChangedEvent(o, args);
		}
		
		public List<System.Object> GetObjectsList()
		{
			TreeIter iter;
			List<System.Object> objects = new List<System.Object>();
			try 
			{
				store.GetIterFirst(out iter);
				do 
				{
					objects.Add((object)store.GetValue(iter, 1));		
				} while (store.IterNext(ref iter));
				if (objects.Count == 0)
					return null;
				return objects;
			}
			catch (Exception)
			{
				return null;
			}
		}
		
		public void Clear()
		{
		    Array.Clear(treeview.Columns, 0, treeview.Columns.Length);
		    while (treeview.Columns.Length > 0)
		    {
		        //for (int i = 0; i < treeview.Columns.Length; i++)
		        //{
		        treeview.RemoveColumn(treeview.Columns[0]);
		        //}
		    }
		    treeview.Model = null;
		    store = null;
		}
	}
}