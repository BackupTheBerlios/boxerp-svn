// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace Boxerp.Client.GtkSharp {
    
    
    public partial class FilteredListView {
        
        private Gtk.TreeView treeview;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize();
            // Widget Boxerp.Client.GtkSharp.FilteredListView
            Stetic.BinContainer.Attach(this);
            this.Events = ((Gdk.EventMask)(256));
            this.Name = "Boxerp.Client.GtkSharp.FilteredListView";
            // Container child Boxerp.Client.GtkSharp.FilteredListView.Gtk.Container+ContainerChild
            this.treeview = new Gtk.TreeView();
            this.treeview.CanFocus = true;
            this.treeview.Name = "treeview";
            this.Add(this.treeview);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
        }
    }
}
