// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace Stetic {
    
    internal class Gui {
        
        public static void Build(object obj, System.Type type) {
            Stetic.Gui.Build(obj, type.FullName);
        }
        
        public static void Build(object obj, string id) {
            System.Collections.Hashtable bindings = new System.Collections.Hashtable();
            if ((id == "widgets.AdvancedTreeView")) {
                Gtk.Bin cobj = ((Gtk.Bin)(obj));
                // Widget widgets.AdvancedTreeView
                BinContainer.Attach(cobj);
                cobj.Events = ((Gdk.EventMask)(256));
                cobj.Name = "widgets.AdvancedTreeView";
                // Container child widgets.AdvancedTreeView.Gtk.Container+ContainerChild
                Gtk.TreeView w1 = new Gtk.TreeView();
                w1.CanFocus = true;
                w1.Events = ((Gdk.EventMask)(0));
                w1.Name = "treeview";
                bindings["treeview"] = w1;
                cobj.Add(w1);
                bindings["widgets.AdvancedTreeView"] = cobj;
                w1.Show();
                cobj.Show();
            }
            else {
                if ((id == "widgets.InfoExtendedDialog")) {
                    Gtk.Dialog cobj = ((Gtk.Dialog)(obj));
                    // Widget widgets.InfoExtendedDialog
                    cobj.Title = "InfoExtendedDialog";
                    cobj.WindowPosition = ((Gtk.WindowPosition)(4));
                    cobj.HasSeparator = false;
                    cobj.Events = ((Gdk.EventMask)(256));
                    cobj.Name = "widgets.InfoExtendedDialog";
                    // Internal child widgets.InfoExtendedDialog.VBox
                    Gtk.VBox w1 = cobj.VBox;
                    w1.BorderWidth = ((uint)(2));
                    w1.Events = ((Gdk.EventMask)(256));
                    w1.Name = "dialog_VBox";
                    bindings["dialog_VBox"] = w1;
                    // Internal child widgets.InfoExtendedDialog.ActionArea
                    Gtk.HButtonBox w2 = cobj.ActionArea;
                    w2.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
                    w2.Spacing = 10;
                    w2.BorderWidth = ((uint)(5));
                    w2.Events = ((Gdk.EventMask)(256));
                    w2.Name = "widgets.InfoExtendedDialog_ActionArea";
                    // Container child widgets.InfoExtendedDialog_ActionArea.Gtk.ButtonBox+ButtonBoxChild
                    Gtk.Button w3 = new Gtk.Button();
                    w3.CanFocus = true;
                    w3.Events = ((Gdk.EventMask)(0));
                    w3.Name = "button7";
                    w3.CanDefault = true;
                    w3.Label = "button7";
                    bindings["button7"] = w3;
                    cobj.AddActionWidget(w3, 0);
                    Gtk.ButtonBox.ButtonBoxChild w4 = ((Gtk.ButtonBox.ButtonBoxChild)(w2[w3]));
                    w4.Expand = false;
                    w4.Fill = false;
                    bindings["widgets.InfoExtendedDialog_ActionArea"] = w2;
                    cobj.DefaultWidth = 400;
                    cobj.DefaultHeight = 300;
                    bindings["widgets.InfoExtendedDialog"] = cobj;
                    w1.Show();
                    w3.Show();
                    w2.Show();
                    cobj.Show();
                }
                else {
                    if ((id == "widgets.WarningDialog")) {
                        Gtk.Dialog cobj = ((Gtk.Dialog)(obj));
                        // Widget widgets.WarningDialog
                        cobj.Title = "Warning";
                        cobj.Icon = Gtk.IconTheme.Default.LoadIcon("gtk-dialog-warning", 16, 0);
                        cobj.WindowPosition = ((Gtk.WindowPosition)(4));
                        cobj.HasSeparator = false;
                        cobj.Resizable = false;
                        cobj.AllowGrow = false;
                        cobj.DefaultWidth = 500;
                        cobj.Events = ((Gdk.EventMask)(256));
                        cobj.Name = "widgets.WarningDialog";
                        // Internal child widgets.WarningDialog.VBox
                        Gtk.VBox w1 = cobj.VBox;
                        w1.BorderWidth = ((uint)(2));
                        w1.Events = ((Gdk.EventMask)(256));
                        w1.Name = "dialog_VBox";
                        // Container child dialog_VBox.Gtk.Box+BoxChild
                        Gtk.HBox w2 = new Gtk.HBox();
                        w2.Spacing = 5;
                        w2.BorderWidth = ((uint)(5));
                        w2.Events = ((Gdk.EventMask)(0));
                        w2.Name = "hbox1";
                        // Container child hbox1.Gtk.Box+BoxChild
                        Gtk.Image w3 = new Gtk.Image();
                        w3.Pixbuf = Gtk.IconTheme.Default.LoadIcon("stock_dialog-warning", 16, 0);
                        w3.Events = ((Gdk.EventMask)(0));
                        w3.Name = "image";
                        bindings["image"] = w3;
                        w2.Add(w3);
                        Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(w2[w3]));
                        w4.Position = 0;
                        w4.Expand = false;
                        w4.Fill = false;
                        // Container child hbox1.Gtk.Box+BoxChild
                        Gtk.Label w5 = new Gtk.Label();
                        w5.LabelProp = "Warning";
                        w5.Events = ((Gdk.EventMask)(0));
                        w5.Name = "label";
                        bindings["label"] = w5;
                        w2.Add(w5);
                        Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(w2[w5]));
                        w6.Position = 1;
                        bindings["hbox1"] = w2;
                        w1.Add(w2);
                        Gtk.Box.BoxChild w7 = ((Gtk.Box.BoxChild)(w1[w2]));
                        w7.Position = 0;
                        w7.Expand = false;
                        w7.Fill = false;
                        bindings["dialog_VBox"] = w1;
                        // Internal child widgets.WarningDialog.ActionArea
                        Gtk.HButtonBox w8 = cobj.ActionArea;
                        w8.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
                        w8.Spacing = 10;
                        w8.BorderWidth = ((uint)(5));
                        w8.Events = ((Gdk.EventMask)(256));
                        w8.Name = "widgets.WarningDialog_ActionArea";
                        // Container child widgets.WarningDialog_ActionArea.Gtk.ButtonBox+ButtonBoxChild
                        Gtk.Button w9 = new Gtk.Button();
                        w9.CanFocus = true;
                        w9.Events = ((Gdk.EventMask)(0));
                        w9.Name = "buttonOk";
                        w9.CanDefault = true;
                        // Container child buttonOk.Gtk.Container+ContainerChild
                        Gtk.Alignment w10 = new Gtk.Alignment(0.5F, 0.5F, 0F, 0F);
                        w10.Events = ((Gdk.EventMask)(0));
                        w10.Name = "GtkAlignment";
                        // Container child GtkAlignment.Gtk.Container+ContainerChild
                        Gtk.HBox w11 = new Gtk.HBox();
                        w11.Spacing = 2;
                        w11.Events = ((Gdk.EventMask)(0));
                        w11.Name = "GtkHBox";
                        // Container child GtkHBox.Gtk.Container+ContainerChild
                        Gtk.Image w12 = new Gtk.Image();
                        w12.Pixbuf = Gtk.IconTheme.Default.LoadIcon("gtk-ok", 16, 0);
                        w12.Events = ((Gdk.EventMask)(0));
                        w12.Name = "image1";
                        bindings["image1"] = w12;
                        w11.Add(w12);
                        // Container child GtkHBox.Gtk.Container+ContainerChild
                        Gtk.Label w14 = new Gtk.Label();
                        w14.LabelProp = "Ok";
                        w14.Events = ((Gdk.EventMask)(0));
                        w14.Name = "GtkLabel";
                        bindings["GtkLabel"] = w14;
                        w11.Add(w14);
                        bindings["GtkHBox"] = w11;
                        w10.Add(w11);
                        bindings["GtkAlignment"] = w10;
                        w9.Add(w10);
                        bindings["buttonOk"] = w9;
                        cobj.AddActionWidget(w9, 0);
                        Gtk.ButtonBox.ButtonBoxChild w18 = ((Gtk.ButtonBox.ButtonBoxChild)(w8[w9]));
                        w18.Expand = false;
                        w18.Fill = false;
                        bindings["widgets.WarningDialog_ActionArea"] = w8;
                        cobj.DefaultHeight = 104;
                        bindings["widgets.WarningDialog"] = cobj;
                        w3.Show();
                        w5.Show();
                        w2.Show();
                        w1.Show();
                        w12.Show();
                        w14.Show();
                        w11.Show();
                        w10.Show();
                        w9.Show();
                        w8.Show();
                        cobj.Show();
                        w9.Clicked += ((System.EventHandler)(System.Delegate.CreateDelegate(typeof(System.EventHandler), cobj, "OnOk")));
                    }
                    else {
                        if ((id == "widgets.WaitWindow")) {
                            Gtk.Window cobj = ((Gtk.Window)(obj));
                            // Widget widgets.WaitWindow
                            cobj.Title = "Operation in progress...";
                            cobj.WindowPosition = ((Gtk.WindowPosition)(1));
                            cobj.Events = ((Gdk.EventMask)(0));
                            cobj.Name = "widgets.WaitWindow";
                            // Container child widgets.WaitWindow.Gtk.Container+ContainerChild
                            Gtk.VBox w1 = new Gtk.VBox();
                            w1.Spacing = 2;
                            w1.BorderWidth = ((uint)(5));
                            w1.Events = ((Gdk.EventMask)(256));
                            w1.Name = "vbox";
                            // Container child vbox.Gtk.Box+BoxChild
                            Gtk.Label w2 = new Gtk.Label();
                            w2.LabelProp = "label1";
                            w2.Events = ((Gdk.EventMask)(0));
                            w2.Name = "labelMsg";
                            bindings["labelMsg"] = w2;
                            w1.Add(w2);
                            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(w1[w2]));
                            w3.Position = 0;
                            w3.Expand = false;
                            w3.Fill = false;
                            // Container child vbox.Gtk.Box+BoxChild
                            Gtk.ProgressBar w4 = new Gtk.ProgressBar();
                            w4.Events = ((Gdk.EventMask)(0));
                            w4.Name = "progressbar";
                            bindings["progressbar"] = w4;
                            w1.Add(w4);
                            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(w1[w4]));
                            w5.Position = 1;
                            w5.Expand = false;
                            w5.Fill = false;
                            bindings["vbox"] = w1;
                            cobj.Add(w1);
                            cobj.DefaultWidth = 400;
                            cobj.DefaultHeight = 78;
                            bindings["widgets.WaitWindow"] = cobj;
                            w2.Show();
                            w4.Show();
                            w1.Show();
                            cobj.Show();
                        }
                        else {
                            if ((id == "widgets.InfoDialog")) {
                                Gtk.Dialog cobj = ((Gtk.Dialog)(obj));
                                // Widget widgets.InfoDialog
                                cobj.Title = "InfoDialog";
                                cobj.WindowPosition = ((Gtk.WindowPosition)(4));
                                cobj.HasSeparator = false;
                                cobj.Events = ((Gdk.EventMask)(256));
                                cobj.Name = "widgets.InfoDialog";
                                // Internal child widgets.InfoDialog.VBox
                                Gtk.VBox w1 = cobj.VBox;
                                w1.BorderWidth = ((uint)(2));
                                w1.Events = ((Gdk.EventMask)(256));
                                w1.Name = "dialog_VBox";
                                bindings["dialog_VBox"] = w1;
                                // Internal child widgets.InfoDialog.ActionArea
                                Gtk.HButtonBox w2 = cobj.ActionArea;
                                w2.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
                                w2.Events = ((Gdk.EventMask)(256));
                                w2.Name = "widgets.InfoDialog_ActionArea";
                                // Container child widgets.InfoDialog_ActionArea.Gtk.ButtonBox+ButtonBoxChild
                                Gtk.Button w3 = new Gtk.Button();
                                w3.CanFocus = true;
                                w3.Events = ((Gdk.EventMask)(0));
                                w3.Name = "button5";
                                w3.CanDefault = true;
                                w3.Label = "button5";
                                bindings["button5"] = w3;
                                cobj.AddActionWidget(w3, 0);
                                Gtk.ButtonBox.ButtonBoxChild w4 = ((Gtk.ButtonBox.ButtonBoxChild)(w2[w3]));
                                w4.Expand = false;
                                w4.Fill = false;
                                bindings["widgets.InfoDialog_ActionArea"] = w2;
                                cobj.DefaultWidth = 400;
                                cobj.DefaultHeight = 300;
                                bindings["widgets.InfoDialog"] = cobj;
                                w1.Show();
                                w3.Show();
                                w2.Show();
                                cobj.Show();
                            }
                            else {
                                if ((id == "widgets.WaitDialog")) {
                                    Gtk.Dialog cobj = ((Gtk.Dialog)(obj));
                                    // Widget widgets.WaitDialog
                                    cobj.Title = "Operation in progress...";
                                    cobj.WindowPosition = ((Gtk.WindowPosition)(1));
                                    cobj.HasSeparator = false;
                                    cobj.Resizable = false;
                                    cobj.AllowGrow = false;
                                    cobj.Events = ((Gdk.EventMask)(256));
                                    cobj.Name = "widgets.WaitDialog";
                                    // Internal child widgets.WaitDialog.VBox
                                    Gtk.VBox w1 = cobj.VBox;
                                    w1.Spacing = 2;
                                    w1.BorderWidth = ((uint)(2));
                                    w1.Events = ((Gdk.EventMask)(256));
                                    w1.Name = "dialog_VBox";
                                    // Container child dialog_VBox.Gtk.Box+BoxChild
                                    Gtk.Label w2 = new Gtk.Label();
                                    w2.LabelProp = "label1";
                                    w2.Events = ((Gdk.EventMask)(0));
                                    w2.Name = "labelMsg";
                                    bindings["labelMsg"] = w2;
                                    w1.Add(w2);
                                    Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(w1[w2]));
                                    w3.Position = 0;
                                    w3.Expand = false;
                                    w3.Fill = false;
                                    // Container child dialog_VBox.Gtk.Box+BoxChild
                                    Gtk.ProgressBar w4 = new Gtk.ProgressBar();
                                    w4.Events = ((Gdk.EventMask)(0));
                                    w4.Name = "progressbar";
                                    bindings["progressbar"] = w4;
                                    w1.Add(w4);
                                    Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(w1[w4]));
                                    w5.Position = 1;
                                    w5.Expand = false;
                                    w5.Fill = false;
                                    bindings["dialog_VBox"] = w1;
                                    // Internal child widgets.WaitDialog.ActionArea
                                    Gtk.HButtonBox w6 = cobj.ActionArea;
                                    w6.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
                                    w6.Spacing = 10;
                                    w6.BorderWidth = ((uint)(5));
                                    w6.Events = ((Gdk.EventMask)(256));
                                    w6.Name = "actionArea";
                                    // Container child actionArea.Gtk.ButtonBox+ButtonBoxChild
                                    Gtk.Button w7 = new Gtk.Button();
                                    w7.CanFocus = true;
                                    w7.Events = ((Gdk.EventMask)(0));
                                    w7.Name = "button";
                                    w7.CanDefault = true;
                                    w7.Label = "button1";
                                    bindings["button"] = w7;
                                    cobj.AddActionWidget(w7, 0);
                                    Gtk.ButtonBox.ButtonBoxChild w8 = ((Gtk.ButtonBox.ButtonBoxChild)(w6[w7]));
                                    w8.Expand = false;
                                    w8.Fill = false;
                                    bindings["actionArea"] = w6;
                                    cobj.DefaultWidth = 400;
                                    cobj.DefaultHeight = 119;
                                    bindings["widgets.WaitDialog"] = cobj;
                                    w2.Show();
                                    w4.Show();
                                    w1.Show();
                                    w6.Show();
                                    cobj.Show();
                                }
                                else {
                                    if ((id == "widgets.LoginWindow")) {
                                        Gtk.Window cobj = ((Gtk.Window)(obj));
                                        // Widget widgets.LoginWindow
                                        cobj.Title = "LoginWindow";
                                        cobj.WindowPosition = ((Gtk.WindowPosition)(4));
                                        cobj.Events = ((Gdk.EventMask)(0));
                                        cobj.Name = "widgets.LoginWindow";
                                        cobj.DefaultWidth = 400;
                                        cobj.DefaultHeight = 300;
                                        bindings["widgets.LoginWindow"] = cobj;
                                        cobj.Show();
                                    }
                                    else {
                                        if ((id == "MainWindow")) {
                                            Gtk.Window cobj = ((Gtk.Window)(obj));
                                            // Widget MainWindow
                                            cobj.Title = "MainWindow";
                                            cobj.WindowPosition = ((Gtk.WindowPosition)(4));
                                            cobj.Events = ((Gdk.EventMask)(0));
                                            cobj.Name = "MainWindow";
                                            cobj.DefaultWidth = 400;
                                            cobj.DefaultHeight = 300;
                                            bindings["MainWindow"] = cobj;
                                            cobj.Show();
                                            cobj.DeleteEvent += ((Gtk.DeleteEventHandler)(System.Delegate.CreateDelegate(typeof(Gtk.DeleteEventHandler), cobj, "OnDeleteEvent")));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            System.Reflection.FieldInfo[] fields = obj.GetType().GetFields(((System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic) | System.Reflection.BindingFlags.Instance));
            for (int n = 0; (n < fields.Length); n = (n + 1)) {
                System.Reflection.FieldInfo field = fields[n];
                object widget = bindings[field.Name];
                if (((widget != null) && field.FieldType.IsInstanceOfType(widget))) {
                    field.SetValue(obj, widget);
                }
            }
        }
    }
    
    internal class BinContainer {
        
        private Gtk.Widget child;
        
        private Gtk.UIManager uimanager;
        
        public static BinContainer Attach(Gtk.Bin bin) {
            BinContainer bc = new BinContainer();
            bin.SizeRequested += new Gtk.SizeRequestedHandler(bc.OnSizeRequested);
            bin.SizeAllocated += new Gtk.SizeAllocatedHandler(bc.OnSizeAllocated);
            bin.Added += new Gtk.AddedHandler(bc.OnAdded);
            return bc;
        }
        
        private void OnSizeRequested(object sender, Gtk.SizeRequestedArgs args) {
            if ((this.child != null)) {
                args.Requisition = this.child.SizeRequest();
            }
        }
        
        private void OnSizeAllocated(object sender, Gtk.SizeAllocatedArgs args) {
            if ((this.child != null)) {
                this.child.Allocation = args.Allocation;
            }
        }
        
        private void OnAdded(object sender, Gtk.AddedArgs args) {
            this.child = args.Widget;
        }
        
        public void SetUiManager(Gtk.UIManager uim) {
            this.uimanager = uim;
            this.child.Realized += new System.EventHandler(this.OnRealized);
        }
        
        private void OnRealized(object sender, System.EventArgs args) {
            if ((this.uimanager != null)) {
                Gtk.Widget w;
                w = this.child.Toplevel;
                if (((w != null) && typeof(Gtk.Window).IsInstanceOfType(w))) {
                    ((Gtk.Window)(w)).AddAccelGroup(this.uimanager.AccelGroup);
                    this.uimanager = null;
                }
            }
        }
    }
    
    internal class ActionGroups {
        
        public static Gtk.ActionGroup GetActionGroup(System.Type type) {
            return Stetic.ActionGroups.GetActionGroup(type.FullName);
        }
        
        public static Gtk.ActionGroup GetActionGroup(string name) {
            return null;
        }
    }
    
}
