
using System;
using Gtk;
using System.Collections;
namespace widgets
{
	// TODO: Thinking about write a complete treeview wrapper instead
	// of access the TreeView property.
	
	public class AdvancedTreeView : Gtk.Bin
	{
		protected TreeStore store;
		protected Gtk.TreeView treeview;
		
		public TreeView TreeView 
		{
			get { return treeview;}
		}
		
		public AdvancedTreeView()
		{
			Stetic.Gui.Build(this, typeof(widgets.AdvancedTreeView));
		}
		
		public void Create(ArrayList columns)
		{
			try
			{
				System.Type[] columnsTypes = new Type[columns.Count];
				int i = 0;

				foreach (SimpleColumn column in columns)
        	    {
        	    	columnsTypes[i++] = column.Type;
        	    }
        	    store = new TreeStore(columnsTypes);
        	    treeview.Model = store;
        	    i = 0;
				foreach (SimpleColumn column in columns)
				{
                	if (column.Type == typeof(Gdk.Pixbuf))
                	{
			        	TreeViewColumn tc = treeview.AppendColumn ("", new CellRendererPixbuf (), "pixbuf", i++);
				    	tc.Visible = column.Visible;
					}
					/*else if (column.Type == typeof(System.Object)) // NOT VALID
					{
			        	TreeViewColumn tc = treeview.AppendColumn ("", new CellRendererPixbuf (), "object", i++);
				    	tc.Visible = column.Visible;
					}*/
                	else
                	{
				    	TreeViewColumn tc = treeview.AppendColumn (column.Name, new CellRendererText (), "text", i++);
				    	tc.Visible = column.Visible;
                	}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Create exception:" + ex.Message);
			}
		}
		
		public TreeIter InsertRow (TreeIter parent, ArrayList row)
		{
			TreeIter iter;
			if (!parent.Equals(TreeIter.Zero))
				iter = store.AppendNode(parent);
			else
				iter = store.AppendNode();

            for (int i = 0; i < row.Count; i++)
				store.SetValue (iter, i, row[i]);
			return iter;
		}
		
	}
}
