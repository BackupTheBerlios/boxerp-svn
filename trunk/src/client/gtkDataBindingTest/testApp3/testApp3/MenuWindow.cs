// ////
////// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
//////
////// Redistribution and use in source and binary forms, with or
////// without modification, are permitted provided that the following
////// conditions are met:
////// Redistributions of source code must retain the above
////// copyright notice, this list of conditions and the following
////// disclaimer.
////// Redistributions in binary form must reproduce the above
////// copyright notice, this list of conditions and the following
////// disclaimer in the documentation and/or other materials
////// provided with the distribution.
//////
////// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY
////// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
////// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
////// PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR
////// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
////// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
////// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
////// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
////// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
////// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
////// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
////// THE POSSIBILITY OF SUCH DAMAGE.
////
////

using System;
using System.Data;

namespace testApp3
{
	
	
	public partial class MenuWindow : Gtk.Window
	{
		
		public MenuWindow() : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
		}

		protected virtual void OnBasicDataBinding (object sender, System.EventArgs e)
		{
			MainWindow win = new MainWindow ();
			win.Show ();
		}

		protected virtual void OnBasicListView (object sender, System.EventArgs e)
		{
			GroupsWindow gwin = new GroupsWindow();
			gwin.ShowAll();
			gwin.Child.ShowAll();
		}

		protected virtual void OnListViewDataBinding (object sender, System.EventArgs e)
		{
			ListViewTest2 lvt2 = new ListViewTest2();
			lvt2.Show();
		}

		protected virtual void OnExit (object o, Gtk.DeleteEventArgs args)
		{
			Gtk.Application.Quit ();
			args.RetVal = true;
		}

		protected virtual void OnComboBoxTests (object sender, System.EventArgs e)
		{
		}

		protected virtual void OnDataGridTests (object sender, System.EventArgs e)
		{
			DataGridTests win = new DataGridTests();
			win.Show();
			win.Child.ShowAll();
		}

		protected virtual void OnDataGridDataTable (object sender, System.EventArgs e)
		{
			DataGridDataTable win = new DataGridDataTable();
			win.Show();
			win.Child.ShowAll();
		}
	}
}
