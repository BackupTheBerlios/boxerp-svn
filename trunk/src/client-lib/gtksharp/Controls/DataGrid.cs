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
using System.Data;

namespace Boxerp.Client.GtkSharp.Controls
{
	/// <summary>
	/// Same as ListView but allowing cells edition. As it uses EditableColumn,
	/// you can specify the kind of widget you want in every column. So you can 
	/// have a CheckBox, an IntegerTextBox, a ComboBox...
	/// 
	/// Extends TreeViewWrapper<T>
	/// </summary>
	public class DataGrid : TreeViewWrapper<EditableColumn>, IBindableWidget
	{
		// FIXME: It should be possible to get the column number when a renderer is edited without this dict
		private Dictionary<object, int> _colNumbers = new Dictionary<object,int>();
		
		public DataGrid(){}
			
		protected override void addTreeViewColumnOrRenderer(EditableColumn column, int colNumber)
		{
			Gtk.TreeViewColumn tvColumn = new Gtk.TreeViewColumn ();
			tvColumn.Title = column.Name;
			tvColumn.Visible = column.Visible;
			
			if (typeof(CheckBox).IsAssignableFrom(column.Widget))
			{
				Gtk.CellRendererToggle renderer = new CellRendererToggle();
				renderer.Activatable = column.Editable;
				renderer.Toggled += OnCheckBoxChanged;
				_colNumbers.Add(renderer, colNumber);
				tvColumn.PackStart(renderer, true);
				tvColumn.AddAttribute(renderer, "active", colNumber);
			}
			else if (typeof(ComboBox).IsAssignableFrom(column.Widget))
			{
				Gtk.CellRendererCombo renderer = new CellRendererCombo();
				renderer.Editable = column.Editable;
				renderer.Edited += OnComboBoxEdited;
				_colNumbers.Add(renderer, colNumber);
				tvColumn.PackStart(renderer, true);
				tvColumn.AddAttribute(renderer, "text", colNumber);
			}
			else if (typeof(IntegerTextBox).IsAssignableFrom(column.Widget))
			{
				CellRendererInteger renderer = new CellRendererInteger();
				renderer.Editable = column.Editable;
				renderer.Edited += OnIntegerCellEdited;
				_colNumbers.Add(renderer, colNumber);
				tvColumn.PackStart(renderer, true);
				tvColumn.AddAttribute(renderer, "text", colNumber);
			}
			else  
			{
				Gtk.CellRendererText renderer = new CellRendererText();
				renderer.Editable = column.Editable;
				renderer.Edited += OnTextCellEdited;
				_colNumbers.Add(renderer, colNumber);
				tvColumn.PackStart(renderer, true);
				tvColumn.AddAttribute(renderer, "text", colNumber);
			}
			
			TreeView.AppendColumn(tvColumn);
		}

		/// <summary>
		/// Refreshes the treeviewmodel (UI) and also the implicit item
		/// </summary>
		/// <param name="renderer">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <param name="path">
		/// A <see cref="System.String"/>
		/// </param>
		/// <param name="newValue">
		/// A <see cref="System.Object"/>
		/// </param>
		protected void refreshModelAndItem(object renderer, string path, object newValue)
		{
			Gtk.TreeIter iter;
			Model.GetIterFromString(out iter, path);
			Model.SetValue(iter, _colNumbers[renderer], newValue);
			
			object item = _itemsPointers[iter.UserData.GetHashCode()];
			// fixme: refactor this to avoid the loop
			string propertyName = String.Empty;
			foreach (KeyValuePair<string, int> pair in _columnsOrder)
			{
				if (pair.Value == _colNumbers[renderer])
				{
					propertyName = pair.Key;
					break;
				}
			}
			updateItem(item, newValue, _colNumbers[renderer], propertyName);
		}
		
		protected virtual void updateItem(object item, object val, int colNumber, string propertyName)
		{
			_itemsType.GetProperty(propertyName).SetValue(item, val, null);
		}
		
		private void OnTextCellEdited(System.Object sender, Gtk.EditedArgs args)
		{
			CellRendererText renderer = (CellRendererText)sender;
			Logger.GetInstance().WriteLine("OnTextCellEdited:" + 
			                               sender.ToString() + "," + args.NewText + "," + 
			                               args.Path + "," + 
			                               args.RetVal);
			refreshModelAndItem(renderer, args.Path, args.NewText);
		}
		
		private void OnIntegerCellEdited(System.Object sender, Gtk.EditedArgs args)
		{
			CellRendererInteger renderer = (CellRendererInteger)sender;
			Logger.GetInstance().WriteLine("OnIntegerCellEdited:" + 
			                               sender.ToString() + "," + args.NewText + "," + 
			                               args.Path + "," + 
			                               args.RetVal);
			try
			{
				int newValue = Convert.ToInt32(args.NewText);
				refreshModelAndItem(renderer, args.Path, newValue);
			}
			catch (System.FormatException)
			{
				Logger.GetInstance().WriteLine("User is entering letter on an integer text box:" + args.NewText);
			}
			catch (System.OverflowException)
			{
				Logger.GetInstance().WriteLine("User has entered a number bigger than an integer:" + args.NewText);
				refreshModelAndItem(renderer, args.Path, Int32.MaxValue);
			}
		}
		
		private void OnCheckBoxChanged(System.Object sender, Gtk.ToggledArgs args)
		{
			Gtk.CellRendererToggle renderer = (CellRendererToggle)sender;
			Logger.GetInstance().WriteLine("OnCheckBoxChanged:" + 
			                               sender.ToString() + "," +  
			                               args.Path + "," + 
			                               args.RetVal);
			TreeIter iter;
			Model.GetIterFromString(out iter, args.Path);
			bool currentValue = (bool)Model.GetValue(iter, _colNumbers[renderer]);
			refreshModelAndItem(renderer, args.Path, !currentValue);
		}
		
		private void OnComboBoxEdited(System.Object sender, Gtk.EditedArgs args)
		{
			Logger.GetInstance().WriteLine("OnComboBoxEdited:" + 
			                               sender.ToString() + "," + args.NewText + "," + 
			                               args.Path + "," + 
			                               args.RetVal);
			
		}
	}
}
