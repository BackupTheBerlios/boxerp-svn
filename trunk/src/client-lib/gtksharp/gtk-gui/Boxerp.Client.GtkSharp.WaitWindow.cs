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
    
    
    public partial class WaitWindow {
        
        private Gtk.VBox vbox;
        
        private Gtk.Label labelMsg;
        
        private Gtk.ProgressBar progressbar;
        
        private Gtk.HButtonBox actionArea;
        
        private Gtk.Button button;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize();
            // Widget Boxerp.Client.GtkSharp.WaitWindow
            this.Name = "Boxerp.Client.GtkSharp.WaitWindow";
            this.Title = Mono.Unix.Catalog.GetString("Operation in progress...");
            this.WindowPosition = ((Gtk.WindowPosition)(1));
            // Container child Boxerp.Client.GtkSharp.WaitWindow.Gtk.Container+ContainerChild
            this.vbox = new Gtk.VBox();
            this.vbox.Events = ((Gdk.EventMask)(256));
            this.vbox.Name = "vbox";
            this.vbox.Spacing = 2;
            this.vbox.BorderWidth = ((uint)(5));
            // Container child vbox.Gtk.Box+BoxChild
            this.labelMsg = new Gtk.Label();
            this.labelMsg.Name = "labelMsg";
            this.labelMsg.LabelProp = "Please wait";
            this.vbox.Add(this.labelMsg);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.vbox[this.labelMsg]));
            w1.Position = 0;
            // Container child vbox.Gtk.Box+BoxChild
            this.progressbar = new Gtk.ProgressBar();
            this.progressbar.Name = "progressbar";
            this.vbox.Add(this.progressbar);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox[this.progressbar]));
            w2.Position = 1;
            w2.Expand = false;
            w2.Fill = false;
            // Container child vbox.Gtk.Box+BoxChild
            this.actionArea = new Gtk.HButtonBox();
            this.actionArea.Events = ((Gdk.EventMask)(256));
            this.actionArea.Name = "actionArea";
            this.actionArea.Spacing = 10;
            this.actionArea.BorderWidth = ((uint)(5));
            this.actionArea.LayoutStyle = ((Gtk.ButtonBoxStyle)(2));
            // Container child actionArea.Gtk.ButtonBox+ButtonBoxChild
            this.button = new Gtk.Button();
            this.button.CanDefault = true;
            this.button.CanFocus = true;
            this.button.Name = "button";
            // Container child button.Gtk.Container+ContainerChild
            Gtk.Alignment w3 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            w3.Name = "GtkAlignment";
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w4 = new Gtk.HBox();
            w4.Name = "GtkHBox";
            w4.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w5 = new Gtk.Image();
            w5.Name = "image33";
            w5.Pixbuf = Stetic.IconLoader.LoadIcon("gtk-cancel", 16);
            w4.Add(w5);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w7 = new Gtk.Label();
            w7.Name = "GtkLabel";
            w7.LabelProp = Mono.Unix.Catalog.GetString("Cancel");
            w4.Add(w7);
            w3.Add(w4);
            this.button.Add(w3);
            this.actionArea.Add(this.button);
            Gtk.ButtonBox.ButtonBoxChild w11 = ((Gtk.ButtonBox.ButtonBoxChild)(this.actionArea[this.button]));
            w11.Expand = false;
            w11.Fill = false;
            this.vbox.Add(this.actionArea);
            Gtk.Box.BoxChild w12 = ((Gtk.Box.BoxChild)(this.vbox[this.actionArea]));
            w12.Position = 2;
            w12.Expand = false;
            w12.Fill = false;
            this.Add(this.vbox);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 400;
            this.DefaultHeight = 119;
            this.Show();
            this.DeleteEvent += new Gtk.DeleteEventHandler(this.OnDeleteEvent);
            this.button.Clicked += new System.EventHandler(this.OnCancel);
        }
    }
}