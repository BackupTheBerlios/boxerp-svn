// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.42
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace gtkResponsivenessTest {
    
    
    public partial class UpdateWaitMsgTest {
        
        private Gtk.Button button1;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget gtkResponsivenessTest.UpdateWaitMsgTest
            this.Name = "gtkResponsivenessTest.UpdateWaitMsgTest";
            this.Title = Mono.Unix.Catalog.GetString("UpdateWaitMsgTest");
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            // Container child gtkResponsivenessTest.UpdateWaitMsgTest.Gtk.Container+ContainerChild
            this.button1 = new Gtk.Button();
            this.button1.CanFocus = true;
            this.button1.Name = "button1";
            this.button1.UseUnderline = true;
            this.button1.Label = Mono.Unix.Catalog.GetString("Start Test");
            this.Add(this.button1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 400;
            this.DefaultHeight = 85;
            this.Show();
            this.button1.Clicked += new System.EventHandler(this.OnClick);
        }
    }
}