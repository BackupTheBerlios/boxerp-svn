
using System;
using System.Collections.Generic;
using Gtk;
using System.Collections;
using Boxerp.Models;

namespace Boxerp.Client.GtkSharp

{
	// TODO: Thinking about write a complete treeview wrapper instead
	// of access the TreeView property.
	
	public class FilteredListView : Gtk.Bin
	{
		protected ListStore store;
		protected TreeModelFilter filter;
		protected Gtk.TreeView treeview;
		public event Gtk.RowActivatedHandler RowActivatedEvent;
		public event System.EventHandler ColumnsChangedEvent;
		public string filterRegex;
		private IBoxerpModel selectedObject;
		
		public string FilterRegex
		{
			set { filterRegex = value; }
		}
		
		public TreeView TreeView 
		{
			get { return treeview;}
		}
		
		public FilteredListView()
		{
			Stetic.Gui.Build(this, typeof(Boxerp.Client.GtkSharp.FilteredListView));
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
        	    filter = new Gtk.TreeModelFilter (store, null);
        	    filter.VisibleFunc = new Gtk.TreeModelFilterVisibleFunc (FilterTree);
        	    treeview.Model = filter;
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
		
		public TreeIter InsertRow (/*TreeIter parent,*/ ArrayList row)
		{
			TreeIter iter = store.Append();
			// if store is a TreeStore:
			/*TreeIter iter;
			if (!parent.Equals(TreeIter.Zero))
				iter = ((TreeStore)filter.Model).AppendNode(parent);
			else
				iter = ((TreeStore)filter.Model).AppendNode();
			*/
            for (int i = 0; i < row.Count; i++)
            {
            	store.SetValue (iter, i, row[i]);
				//Console.WriteLine("INserting oclumn=" + row[i].GetType().ToString());	
			}
			return iter;
		}
		
		public TreeIter InsertModel (IBoxerpModel model)
		{
		    TreeIter iter = store.Append();
		    if (model != null)
		    {
		        store.SetValue(iter, 0, model.Id.ToString());
		        store.SetValue(iter, 1, model);
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
			IBoxerpModel obj = (IBoxerpModel) model.GetValue (iter, 1);
			(cell as Gtk.CellRendererText).Text = obj.ToString();
		}

		private bool FilterTree (Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			IBoxerpModel obj = (IBoxerpModel)model.GetValue (iter, 1);
 
 			if (filterRegex == null)
 				return true;
			if (filterRegex == "")
				return true;
 
			if (obj.ToString().IndexOf(filterRegex) > -1)
				return true;
			else
				return false;
		}
		
		public void Refilter()
		{
			filter.Refilter();
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
		
		public bool IsSelected()
		{
			TreeIter iter;
			TreeModel model;
			
			try
			{
				if (treeview.Selection.GetSelected(out model, out iter))
				{
					selectedObject = (IBoxerpModel) model.GetValue(iter, 1);
					
					return true;
				}
				else
					return false;
			}
			catch (Exception)
			{
				//Console.WriteLine("IsSelected:" + ex.Message + ","+ex.StackTrace);
				return false;
			}
		}
		
		public IBoxerpModel SelectedObject
		{
			get {return selectedObject; }
		}
		
		
	}
}
