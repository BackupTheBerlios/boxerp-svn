// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace Boxerp.Client.GtkSharp.Controls {
    
    
    public partial class ListView {
        
        private Gtk.TreeView _treeview;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget Boxerp.Client.GtkSharp.Controls.ListView
            Stetic.BinContainer.Attach(this);
            this.Name = "Boxerp.Client.GtkSharp.Controls.ListView";
            // Container child Boxerp.Client.GtkSharp.Controls.ListView.Gtk.Container+ContainerChild
            this._treeview = new Gtk.TreeView();
            this._treeview.CanFocus = true;
            this._treeview.Name = "_treeview";
            this._treeview.HeadersClickable = true;
            this.Add(this._treeview);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
        }
    }
}
