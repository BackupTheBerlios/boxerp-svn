// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace mvcSample1 {
    
    
    public partial class UsersListView {
        
        private Gtk.VBox vbox1;
        
        private Gtk.HBox hbox1;
        
        private Gtk.HBox hbox2;
        
        private Gtk.Label label1;
        
        private Gtk.Label _activeUsers;
        
        private Gtk.ScrolledWindow _usersScrollWin;
        
        private Gtk.HButtonBox hbuttonbox1;
        
        private Gtk.Button button2;
        
        private Gtk.Button button3;
        
        private Gtk.Button button4;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget mvcSample1.UsersListView
            Stetic.BinContainer.Attach(this);
            this.Name = "mvcSample1.UsersListView";
            // Container child mvcSample1.UsersListView.Gtk.Container+ContainerChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.hbox2 = new Gtk.HBox();
            this.hbox2.HeightRequest = 30;
            this.hbox2.Name = "hbox2";
            this.hbox2.Spacing = 6;
            this.hbox1.Add(this.hbox2);
            Gtk.Box.BoxChild w1 = ((Gtk.Box.BoxChild)(this.hbox1[this.hbox2]));
            w1.Position = 0;
            // Container child hbox1.Gtk.Box+BoxChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("Active Users:");
            this.hbox1.Add(this.label1);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.hbox1[this.label1]));
            w2.Position = 1;
            w2.Expand = false;
            w2.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this._activeUsers = new Gtk.Label();
            this._activeUsers.Name = "_activeUsers";
            this._activeUsers.LabelProp = Mono.Unix.Catalog.GetString("label2");
            this.hbox1.Add(this._activeUsers);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.hbox1[this._activeUsers]));
            w3.Position = 2;
            w3.Expand = false;
            w3.Fill = false;
            this.vbox1.Add(this.hbox1);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbox1]));
            w4.Position = 0;
            w4.Expand = false;
            w4.Fill = false;
            // Container child vbox1.Gtk.Box+BoxChild
            this._usersScrollWin = new Gtk.ScrolledWindow();
            this._usersScrollWin.CanFocus = true;
            this._usersScrollWin.Name = "_usersScrollWin";
            this._usersScrollWin.VscrollbarPolicy = ((Gtk.PolicyType)(1));
            this._usersScrollWin.HscrollbarPolicy = ((Gtk.PolicyType)(1));
            this._usersScrollWin.ShadowType = ((Gtk.ShadowType)(1));
            this.vbox1.Add(this._usersScrollWin);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.vbox1[this._usersScrollWin]));
            w5.Position = 1;
            // Container child vbox1.Gtk.Box+BoxChild
            this.hbuttonbox1 = new Gtk.HButtonBox();
            this.hbuttonbox1.Name = "hbuttonbox1";
            this.hbuttonbox1.Spacing = 2;
            this.hbuttonbox1.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child hbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
            this.button2 = new Gtk.Button();
            this.button2.CanFocus = true;
            this.button2.Name = "button2";
            this.button2.UseUnderline = true;
            this.button2.Label = Mono.Unix.Catalog.GetString("Add");
            this.hbuttonbox1.Add(this.button2);
            Gtk.ButtonBox.ButtonBoxChild w6 = ((Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox1[this.button2]));
            w6.Expand = false;
            w6.Fill = false;
            // Container child hbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
            this.button3 = new Gtk.Button();
            this.button3.CanFocus = true;
            this.button3.Name = "button3";
            this.button3.UseUnderline = true;
            this.button3.Label = Mono.Unix.Catalog.GetString("Edit");
            this.hbuttonbox1.Add(this.button3);
            Gtk.ButtonBox.ButtonBoxChild w7 = ((Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox1[this.button3]));
            w7.Position = 1;
            w7.Expand = false;
            w7.Fill = false;
            // Container child hbuttonbox1.Gtk.ButtonBox+ButtonBoxChild
            this.button4 = new Gtk.Button();
            this.button4.CanFocus = true;
            this.button4.Name = "button4";
            this.button4.UseUnderline = true;
            this.button4.Label = Mono.Unix.Catalog.GetString("Delete");
            this.hbuttonbox1.Add(this.button4);
            Gtk.ButtonBox.ButtonBoxChild w8 = ((Gtk.ButtonBox.ButtonBoxChild)(this.hbuttonbox1[this.button4]));
            w8.Position = 2;
            w8.Expand = false;
            w8.Fill = false;
            this.vbox1.Add(this.hbuttonbox1);
            Gtk.Box.BoxChild w9 = ((Gtk.Box.BoxChild)(this.vbox1[this.hbuttonbox1]));
            w9.PackType = ((Gtk.PackType)(1));
            w9.Position = 2;
            w9.Expand = false;
            w9.Fill = false;
            w9.Padding = ((uint)(5));
            this.Add(this.vbox1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
            this.button2.Clicked += new System.EventHandler(this.OnAddUser);
            this.button3.Clicked += new System.EventHandler(this.OnEditUser);
            this.button4.Clicked += new System.EventHandler(this.OnDeleteUser);
        }
    }
}
