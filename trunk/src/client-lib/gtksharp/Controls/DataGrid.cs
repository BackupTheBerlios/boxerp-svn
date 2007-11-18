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
using System.ComponentModel;
using Gtk;
using GLib;
using System.Collections;
using System.Collections.Generic;
using Boxerp.Client.GtkSharp;
using System.Reflection;
using Boxerp.Client;
using Boxerp.Collections;

namespace Boxerp.Client.GtkSharp.Controls
{
	public class DataGrid : TreeViewWrapper<EditableColumn>, IBindableWidget
	{
		private CustomTreeView _treeview;
		public DataGrid()
		{
			_treeview = new CustomTreeView();
			this.Add(_treeview);
		}
		
		protected override CustomTreeView TreeModelWidget
		{ 
			get
			{
				return _treeview;
			}
		}
		
		protected override void addTreeViewColumn(EditableColumn column, int colNumber)
		{
			Gtk.TreeViewColumn tvColumn = new Gtk.TreeViewColumn ();
			tvColumn.Title = column.Name;
			tvColumn.Visible = column.Visible;
			
			if (typeof(CheckBox).IsAssignableFrom(column.Widget))
			{
				Gtk.CellRendererToggle renderer = new CellRendererToggle();
				renderer.Activatable = column.Editable;
				renderer.Toggled += OnCheckBoxChanged;
				tvColumn.PackStart(renderer, true);
			}
			else if (typeof(ComboBox).IsAssignableFrom(column.Widget))
			{
				Gtk.CellRendererCombo renderer = new CellRendererCombo();
				renderer.Editable = column.Editable;
				renderer.Edited += OnComboBoxEdited;
				tvColumn.PackStart(renderer, true);
			}
			else  // if (typeof(TextBox).IsAssignableFrom(column.Widget))
			{
				Gtk.CellRendererText renderer = new CellRendererText();
				renderer.Editable = column.Editable;
				renderer.Edited += OnTextCellEdited;
				tvColumn.PackStart(renderer, true);
			}
			
			TreeModelWidget.AppendColumn(tvColumn);
		}

		private void OnTextCellEdited(System.Object sender, Gtk.EditedArgs args)
		{
			
		}
		
		private void OnCheckBoxChanged(System.Object sender, Gtk.ToggledArgs args)
		{
			
		}
		
		private void OnComboBoxEdited(System.Object sender, Gtk.EditedArgs arg)
		{
			
		}
	}
}
