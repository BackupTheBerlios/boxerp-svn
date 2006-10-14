
using System;
using Gtk;
using Boxerp.Models;

namespace widgets
{
	
	// TODO: Implement drag&drop between treeviews
	public class DoubleTreeView : Gtk.Bin
	{
		public widgets.SimpleTreeView streeviewRight;
		public widgets.SimpleTreeView streeviewLeft;
		protected Gtk.Label labelLeft;
		protected Gtk.Label labelRight;

		/*public SimpleTreeView LeftTreeView
		{
			get { return streeviewLeft; }
		}
		
		public SimpleTreeView RightTreeView
		{
			get { return streeviewLeft; }
		}*/
		
		public string LeftLabel 
		{
			set { labelLeft.Text = value; }
		}
		
		public string RightLabel
		{
			set { labelRight.Text = value; }
		}
		
		public DoubleTreeView()
		{
			Stetic.Gui.Build(this, typeof(widgets.DoubleTreeView));
		}
		
		/*public SimpleTreeView GetLeftTreeview()
		{
			return this.streeviewLeft;
		}
		
		public SimpleTreeView GetRightTreeview()
		{
			return this.streeviewRight;
		}*/
		
		protected virtual void OnLeftClicked(object sender, System.EventArgs e)
		{
			TreeModel model;
			TreeIter iter;
			if (streeviewRight.TreeView.Selection.GetSelected(out model, out iter))
			{
				int id = (int)model.GetValue(iter, 0);
				IBoxerpModel obj = (IBoxerpModel)model.GetValue(iter, 1);
				((TreeStore)model).Remove(ref iter);
				TreeStore leftModel = (TreeStore)streeviewLeft.TreeView.Model;
				iter = leftModel.AppendNode();
				leftModel.SetValue(iter, 0, id);
				leftModel.SetValue(iter, 0, obj);
           	}
        }

		protected virtual void OnRightClicked(object sender, System.EventArgs e)
		{
			TreeModel model;
			TreeIter iter;
			if (streeviewLeft.TreeView.Selection.GetSelected(out model, out iter))
			{
				int id = (int)model.GetValue(iter, 0);
				IBoxerpModel obj = (IBoxerpModel)model.GetValue(iter, 1);
				((TreeStore)model).Remove(ref iter);
				TreeStore rightModel = (TreeStore)streeviewRight.TreeView.Model;
				iter = rightModel.AppendNode();
				rightModel.SetValue(iter, 0, id);
				rightModel.SetValue(iter, 0, obj);
           	}
		}
	}
	
}
