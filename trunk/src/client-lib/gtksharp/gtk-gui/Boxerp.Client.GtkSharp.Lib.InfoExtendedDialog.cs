// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace Stetic.SteticGenerated {
    
    
    internal class BoxerpClientGtkSharpLibInfoExtendedDialog {
        
        public static void Build(Gtk.Dialog cobj) {
            System.Collections.Hashtable bindings = new System.Collections.Hashtable();
            // Widget Boxerp.Client.GtkSharp.Lib.InfoExtendedDialog
            cobj.Events = ((Gdk.EventMask)(256));
            cobj.Name = "Boxerp.Client.GtkSharp.Lib.InfoExtendedDialog";
            cobj.Title = Mono.Unix.Catalog.GetString("InfoExtendedDialog");
            cobj.WindowPosition = ((Gtk.WindowPosition)(4));
            cobj.HasSeparator = false;
            // Internal child Boxerp.Client.GtkSharp.Lib.InfoExtendedDialog.VBox
            Gtk.VBox w1 = cobj.VBox;
            w1.Events = ((Gdk.EventMask)(256));
            w1.Name = "dialog_VBox";
            w1.BorderWidth = ((uint)(2));
            // Internal child Boxerp.Client.GtkSharp.Lib.InfoExtendedDialog.ActionArea
            Gtk.HButtonBox w2 = cobj.ActionArea;
            w2.Events = ((Gdk.EventMask)(256));
            w2.Name = "Boxerp.Client.GtkSharp.Lib.InfoExtendedDialog_ActionArea";
            w2.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child Boxerp.Client.GtkSharp.Lib.InfoExtendedDialog_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            Gtk.Button w3 = new Gtk.Button();
            bindings["button6"] = w3;
            w3.CanDefault = true;
            w3.CanFocus = true;
            w3.Name = "button6";
            w3.Label = "button6";
            cobj.AddActionWidget(w3, 0);
            Gtk.ButtonBox.ButtonBoxChild w4 = ((Gtk.ButtonBox.ButtonBoxChild)(w2[w3]));
            w4.Expand = false;
            w4.Fill = false;
            if ((cobj.Child != null)) {
                cobj.Child.ShowAll();
            }
            cobj.DefaultWidth = 400;
            cobj.DefaultHeight = 300;
            cobj.Show();
            System.Reflection.FieldInfo[] fields = cobj.GetType().GetFields(((System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic) | System.Reflection.BindingFlags.Instance));
            for (int n = 0; (n < fields.Length); n = (n + 1)) {
                System.Reflection.FieldInfo field = fields[n];
                object widget = bindings[field.Name];
                if (((widget != null) && field.FieldType.IsInstanceOfType(widget))) {
                    field.SetValue(cobj, widget);
                }
            }
        }
    }
}
