// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace Admin {
    
    
    public partial class TestWindow2 {
        
        private Gtk.HBox hbox1;
        
        private Gtk.VBox vbox1;
        
        private Gtk.Label label2;
        
        private Gtk.Button button4;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget Admin.TestWindow2
            this.Name = "Admin.TestWindow2";
            this.Title = Mono.Unix.Catalog.GetString("TestWindow2");
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            // Container child Admin.TestWindow2.Gtk.Container+ContainerChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            // Container child vbox1.Gtk.Box+BoxChild
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.LabelProp = Mono.Unix.Catalog.GetString("Click to test the GtkResponsiveHelper");
            this.vbox1.Add(this.label2);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.vbox1[this.label2]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            w1.Padding = ((uint)(5));
            // Container child vbox1.Gtk.Box+BoxChild
            this.button4 = new Gtk.Button();
            this.button4.CanFocus = true;
            this.button4.Name = "button4";
            this.button4.UseUnderline = true;
            this.button4.Label = Mono.Unix.Catalog.GetString("Start in ConcurrencyMode.SingletonThread");
            this.vbox1.Add(this.button4);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox1[this.button4]));
            w2.Position = 1;
            w2.Expand = false;
            w2.Fill = false;
            this.hbox1.Add(this.vbox1);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox1[this.vbox1]));
            w3.Position = 0;
            w3.Fill = false;
            this.Add(this.hbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 400;
            this.DefaultHeight = 114;
            this.Show();
            this.button4.Clicked += new System.EventHandler(this.OnClicked);
        }
    }
}
