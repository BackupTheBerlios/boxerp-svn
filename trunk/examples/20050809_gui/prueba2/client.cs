using Gtk;
using Glade;
using System;

class Ventana
{
/*	[Widget] Gtk.Entry entry1;
	[Widget] Gtk.Label label1;*/
	[Widget] Gtk.Button b_user;
	[Widget] Gtk.Button b_group;
	[Widget] Gtk.Button b_enterprise;
	[Widget] Gtk.ToggleButton tb_admin;
	[Widget] Gtk.Window w_admin;

	public Ventana()
	{
		Glade.XML gui = new Glade.XML ("administration/administration.glade", "w_admin", "");
		gui.Autoconnect(this);
		w_admin.Maximize();
	}

	void on_tb_admin_clicked (object o, EventArgs e)
	{
//		if (gtk_toggle_button_get_activate (GTK_TOGGLE_BUTTON (tb_admin)))
		if (tb_admin.Active)
		{
			b_user.Visible = true;
			b_group.Visible = true;
			b_enterprise.Visible = true;
		} 
		else
		{
			b_user.Visible = false;
			b_group.Visible = false;
			b_enterprise.Visible = false;
		}
	}
	
	void on_copiar1_activate (object o, EventArgs e)
	{
	}
	void on_guardar_como1_activate (object o, EventArgs e)
	{
	}
	void on_guardar1_activate (object o, EventArgs e)
	{
	}
	void on_pegar1_activate (object o, EventArgs e)
	{
	}
	void on_nuevo1_activate (object o, EventArgs e)
	{
	}
	void on_abrir1_activate (object o, EventArgs e)
	{
	}
	void on_cortar1_activate (object o, EventArgs e)
	{
	}
	void on_borrar1_activate (object o, EventArgs e)
	{
	}
	void on_acerca_de1_activate (object o, EventArgs e)
	{
	}
	void on_salir1_activate (object o, EventArgs e)
	{
	}
/*	void on_button1_clicked (object o, EventArgs e)
	{
		Console.WriteLine ("Ha pinchao joer!");
		Console.WriteLine ("El texo escrito es: " + entry1.Text);

		label1.Text = "Weiren";
		
	}*/
}

class Demo
{

	static void Main()
	{
		Gtk.Application.Init();
		Ventana ven = new Ventana ();
		Gtk.Application.Run();
	}
}
