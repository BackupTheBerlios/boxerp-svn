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
    
    
    public partial class WaitDialog {
        
        private Gtk.Label labelMsg;
        
        private Gtk.ProgressBar progressbar;
        
        private Gtk.Button button;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize();
            // Widget Boxerp.Client.GtkSharp.WaitDialog
            this.Events = ((Gdk.EventMask)(256));
            this.Name = "Boxerp.Client.GtkSharp.WaitDialog";
            this.Title = Mono.Unix.Catalog.GetString("Operation in progress...");
            this.WindowPosition = ((Gtk.WindowPosition)(1));
            this.HasSeparator = false;
            // Internal child Boxerp.Client.GtkSharp.WaitDialog.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Events = ((Gdk.EventMask)(256));
            w1.Name = "dialog_VBox";
            w1.Spacing = 2;
            w1.BorderWidth = ((uint)(2));
            // Container child dialog_VBox.Gtk.Box+BoxChild
            this.labelMsg = new Gtk.Label();
            this.labelMsg.Name = "labelMsg";
            this.labelMsg.LabelProp = "Please wait";
            w1.Add(this.labelMsg);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(w1[this.labelMsg]));
            w2.Position = 0;
            // Container child dialog_VBox.Gtk.Box+BoxChild
            this.progressbar = new Gtk.ProgressBar();
            this.progressbar.Name = "progressbar";
            w1.Add(this.progressbar);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(w1[this.progressbar]));
            w3.Position = 1;
            w3.Expand = false;
            w3.Fill = false;
            // Internal child Boxerp.Client.GtkSharp.WaitDialog.ActionArea
            Gtk.HButtonBox w4 = this.ActionArea;
            w4.Events = ((Gdk.EventMask)(256));
            w4.Name = "actionArea";
            w4.Spacing = 6;
            w4.BorderWidth = ((uint)(5));
            w4.LayoutStyle = ((Gtk.ButtonBoxStyle)(2));
            // Container child actionArea.Gtk.ButtonBox+ButtonBoxChild
            this.button = new Gtk.Button();
            this.button.CanDefault = true;
            this.button.CanFocus = true;
            this.button.Name = "button";
            // Container child button.Gtk.Container+ContainerChild
            Gtk.Alignment w5 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
            w5.Name = "GtkAlignment";
            // Container child GtkAlignment.Gtk.Container+ContainerChild
            Gtk.HBox w6 = new Gtk.HBox();
            w6.Name = "GtkHBox";
            w6.Spacing = 2;
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Image w7 = new Gtk.Image();
            w7.Name = "image38";
            w7.Pixbuf = Stetic.IconLoader.LoadIcon("gtk-cancel", 16);
            w6.Add(w7);
            // Container child GtkHBox.Gtk.Container+ContainerChild
            Gtk.Label w9 = new Gtk.Label();
            w9.Name = "GtkLabel";
            w9.LabelProp = Mono.Unix.Catalog.GetString("Cancel");
            w6.Add(w9);
            w5.Add(w6);
            this.button.Add(w5);
            this.AddActionWidget(this.button, -7);
            Gtk.ButtonBox.ButtonBoxChild w13 = ((Gtk.ButtonBox.ButtonBoxChild)(w4[this.button]));
            w13.Expand = false;
            w13.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 400;
            this.DefaultHeight = 113;
            this.Show();
            this.Close += new System.EventHandler(this.OnClose);
            this.DeleteEvent += new Gtk.DeleteEventHandler(this.OnDeleteEvent);
            this.button.Clicked += new System.EventHandler(this.OnCancel);
        }
    }
}