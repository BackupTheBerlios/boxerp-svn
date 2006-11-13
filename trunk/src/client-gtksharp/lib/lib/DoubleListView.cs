
using System;
using System.Collections.Generic;
using Gtk;
using Boxerp.Models;
using System.Collections;

namespace Boxerp.Client.GtkSharp.Lib
{
	
	// TODO: Implement drag&drop between treeviews
	public class DoubleListView : global::Gtk.Bin
	{
		protected Gtk.Label labelLeft;
		protected Gtk.Label labelRight;
		protected Boxerp.Client.GtkSharp.Lib.SimpleListView slistviewRight;
		protected Boxerp.Client.GtkSharp.Lib.SimpleListView slistviewLeft;
		

		public string LeftLabel 
		{
			set { labelLeft.Text = value; }
		}
		
		public string RightLabel
		{
			set { labelRight.Text = value; }
		}
		
		public DoubleListView()
		{
			Stetic.Gui.Build(this, typeof(Boxerp.Client.GtkSharp.Lib.DoubleListView));
		}
		
		public void CreateLeftList(List<SimpleColumn> columns)
		{
			this.slistviewLeft.Create(columns);
		}
		
		public void CreateRightList(List<SimpleColumn> columns)
		{
			this.slistviewRight.Create(columns);
		}
		
		public TreeIter InsertRowLeft(ArrayList row)
		{
			if (row == null)
				Console.WriteLine("double list null");
			return (this.slistviewLeft.InsertRow(row));
		}
		
		public TreeIter InsertRowRight(ArrayList row)
		{
			return (this.slistviewRight.InsertRow(row));
		}
		
		public List<IBoxerpModel> GetLeftObjectList()
		{
			return slistviewLeft.GetObjectsList();
		}
		
		public List<IBoxerpModel> GetRightObjectList()
		{
			return slistviewRight.GetObjectsList();
		}
		protected virtual void OnLeftClicked(object sender, System.EventArgs e)
		{
			TreeModel leftModel = slistviewLeft.TreeView.Model;
			TreeModel model;
			TreeIter iter;
			Console.WriteLine("get selected:");
			TreeViewColumn[] cols = slistviewRight.treeview.Columns;
			foreach(TreeViewColumn c in cols)
			{
				Console.WriteLine("title:  " + c.Title);
			}
			Console.WriteLine(slistviewRight.treeview.Model.GetColumnType(0).ToString());
			Console.WriteLine(slistviewRight.treeview.Model.GetColumnType(1).ToString());
			if (slistviewRight.treeview.Selection.GetSelected(out model, out iter))
			{
				string code = (string) model.GetValue(iter, 0);
				IBoxerpModel obj = (IBoxerpModel) model.GetValue(iter, 1);
				((ListStore)model).Remove(ref iter);
				iter = ((ListStore)leftModel).Append();
				leftModel.SetValue(iter, 0, code);
				leftModel.SetValue(iter, 1, obj);
				
           	}
        }

		protected virtual void OnRightClicked(object sender, System.EventArgs e)
		{
			TreeModel rightModel = slistviewRight.TreeView.Model;
			TreeModel model;
			TreeIter iter;
			if (slistviewLeft.TreeView.Selection.GetSelected(out model, out iter))
			{
				string code = (string) model.GetValue(iter, 0);
				IBoxerpModel obj = (IBoxerpModel) model.GetValue(iter, 1);
				((ListStore)model).Remove(ref iter);
				iter = ((ListStore)rightModel).Append();
				rightModel.SetValue(iter, 0, code);
				rightModel.SetValue(iter, 1, obj);
           	}
		}
	}
	
}