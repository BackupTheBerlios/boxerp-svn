// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace testApp3 {
    
    
    public partial class DataGridTests {
        
        private Gtk.VBox vbox1;
        
        private Gtk.Label _label;
        
        private Gtk.ScrolledWindow scrolledwindow1;
        
        private Gtk.HBox hbox1;
        
        private Gtk.Button button1;
        
        private Gtk.Button button2;
        
        private Gtk.Button button3;
        
        private Gtk.Button button7;
        
        private Gtk.Button button9;
        
        private Gtk.HBox hbox2;
        
        private Gtk.Button button6;
        
        private Gtk.Button button4;
        
        private Gtk.Button button5;
        
        private Gtk.Button button8;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget testApp3.DataGridTests
            this.Name = "testApp3.DataGridTests";
            this.Title = Mono.Unix.Catalog.GetString("DataGridTests");
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            // Container child testApp3.DataGridTests.Gtk.Container+ContainerChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            // Container child vbox1.Gtk.Box+BoxChild
            this._label = new Gtk.Label();
            this._label.Name = "_label";
            this._label.LabelProp = Mono.Unix.Catalog.GetString("Users");
            this.vbox1.Add(this._label);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.vbox1[this._label]));
            w1.Position = 0;
            w1.Expand = false;
            w1.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.scrolledwindow1 = new Gtk.ScrolledWindow();
            this.scrolledwindow1.CanFocus = true;
            this.scrolledwindow1.Name = "scrolledwindow1";
            this.scrolledwindow1.VscrollbarPolicy = ((Gtk.PolicyType)(1));
            this.scrolledwindow1.HscrollbarPolicy = ((Gtk.PolicyType)(1));
            this.scrolledwindow1.ShadowType = ((Gtk.ShadowType)(1));
            this.vbox1.Add(this.scrolledwindow1);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox1[this.scrolledwindow1]));
            w2.Position = 1;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.button1 = new Gtk.Button();
            this.button1.CanFocus = true;
            this.button1.Name = "button1";
            this.button1.UseUnderline = true;
            this.button1.Label = Mono.Unix.Catalog.GetString("Show Selected Item");
            this.hbox1.Add(this.button1);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox1[this.button1]));
            w3.Position = 0;
            w3.Expand = false;
            w3.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.button2 = new Gtk.Button();
            this.button2.CanFocus = true;
            this.button2.Name = "button2";
            this.button2.UseUnderline = true;
            this.button2.Label = Mono.Unix.Catalog.GetString("Delete Selected Item");
            this.hbox1.Add(this.button2);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.hbox1[this.button2]));
            w4.Position = 1;
            w4.Expand = false;
            w4.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.button3 = new Gtk.Button();
            this.button3.CanFocus = true;
            this.button3.Name = "button3";
            this.button3.UseUnderline = true;
            this.button3.Label = Mono.Unix.Catalog.GetString("Add Random Item");
            this.hbox1.Add(this.button3);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.hbox1[this.button3]));
            w5.Position = 2;
            w5.Expand = false;
            w5.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.button7 = new Gtk.Button();
            this.button7.CanFocus = true;
            this.button7.Name = "button7";
            this.button7.UseUnderline = true;
            this.button7.Label = Mono.Unix.Catalog.GetString("Memory test");
            this.hbox1.Add(this.button7);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.hbox1[this.button7]));
            w6.Position = 3;
            w6.Expand = false;
            w6.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.button9 = new Gtk.Button();
            this.button9.CanFocus = true;
            this.button9.Name = "button9";
            this.button9.UseUnderline = true;
            this.button9.Label = Mono.Unix.Catalog.GetString("Change item from code");
            this.hbox1.Add(this.button9);
            Gtk.Box.BoxChild w7 = ((Gtk.Box.BoxChild)(this.hbox1[this.button9]));
            w7.Position = 4;
            w7.Expand = false;
            w7.Fill = false;
            this.vbox1.Add(this.hbox1);
            Gtk.Box.BoxChild w8 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox1]));
            w8.Position = 2;
            w8.Expand = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox2 = new Gtk.HBox();
            this.hbox2.Name = "hbox2";
            this.hbox2.Spacing = 6;
            // Container child hbox2.Gtk.Box+BoxChild
            this.button6 = new Gtk.Button();
            this.button6.CanFocus = true;
            this.button6.Name = "button6";
            this.button6.UseUnderline = true;
            this.button6.Label = Mono.Unix.Catalog.GetString("Show Multiple Items");
            this.hbox2.Add(this.button6);
            Gtk.Box.BoxChild w9 = ((Gtk.Box.BoxChild)(this.hbox2[this.button6]));
            w9.Position = 0;
            w9.Expand = false;
            w9.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.button4 = new Gtk.Button();
            this.button4.CanFocus = true;
            this.button4.Name = "button4";
            this.button4.UseUnderline = true;
            this.button4.Label = Mono.Unix.Catalog.GetString("Change display mode");
            this.hbox2.Add(this.button4);
            Gtk.Box.BoxChild w10 = ((Gtk.Box.BoxChild)(this.hbox2[this.button4]));
            w10.Position = 1;
            w10.Expand = false;
            w10.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.button5 = new Gtk.Button();
            this.button5.CanFocus = true;
            this.button5.Name = "button5";
            this.button5.UseUnderline = true;
            this.button5.Label = Mono.Unix.Catalog.GetString("Change Selection Mode");
            this.hbox2.Add(this.button5);
            Gtk.Box.BoxChild w11 = ((Gtk.Box.BoxChild)(this.hbox2[this.button5]));
            w11.Position = 2;
            w11.Expand = false;
            w11.Fill = false;
            // Container child hbox2.Gtk.Box+BoxChild
            this.button8 = new Gtk.Button();
            this.button8.CanFocus = true;
            this.button8.Name = "button8";
            this.button8.UseUnderline = true;
            this.button8.Label = Mono.Unix.Catalog.GetString("Delete Multiple");
            this.hbox2.Add(this.button8);
            Gtk.Box.BoxChild w12 = ((Gtk.Box.BoxChild)(this.hbox2[this.button8]));
            w12.Position = 3;
            w12.Expand = false;
            w12.Fill = false;
            this.vbox1.Add(this.hbox2);
            Gtk.Box.BoxChild w13 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox2]));
            w13.Position = 3;
            w13.Expand = false;
            this.Add(this.vbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 703;
            this.DefaultHeight = 300;
            this.Show();
            this.button1.Clicked += new System.EventHandler(this.OnShowItem);
            this.button9.Clicked += new System.EventHandler(this.OnChangeUser);
        }
    }
}
