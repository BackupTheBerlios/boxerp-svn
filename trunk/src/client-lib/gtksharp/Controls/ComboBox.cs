////
//// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
////
//// Redistribution and use in source and binary forms, with or
//// without modification, are permitted provided that the following
//// conditions are met:
//// Redistributions of source code must retain the above
//// copyright notice, this list of conditions and the following
//// disclaimer.
//// Redistributions in binary form must reproduce the above
//// copyright notice, this list of conditions and the following
//// disclaimer in the documentation and/or other materials
//// provided with the distribution.
////
//// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY
//// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
//// PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR
//// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
//// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
//// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
//// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
//// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
//// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
//// THE POSSIBILITY OF SUCH DAMAGE.
//
//

using System;
using Gdk;
using Boxerp.Client.GtkSharp;
using Boxerp.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using Boxerp.Client;

namespace Boxerp.Client.GtkSharp.Controls
{
	/// <summary>
	/// Is a ComboBox that supports data binding and makes the API easier to understand.
	/// 
	/// Extends TreeModelWrapper<T>
	/// </summary>
	public class ComboBox : TreeModelWrapper<SimpleColumn>, 
	      IBindableWidget, ITreeModel
	{
		private Gtk.ComboBox _combo;
		public event EventHandler SelectionChanged;
		
		public ComboBox()
			: base()
		{
			_combo = new Gtk.ComboBox();
			this.Add(_combo);
			_combo.Changed += OnSelectionChanged;
		}

		protected void OnSelectionChanged(Object sender, EventArgs args)
		{
			if (SelectionChanged != null)
			{
				SelectionChanged(this, args);
			}
		}
			
		public override Gtk.Widget InnerWidget
		{
			get
			{
				return _combo;
			}
		}
		
		protected override Gtk.TreeModel Model 
		{ 
			get
			{
				return _combo.Model;
			}
			set
			{
				_combo.Model = value;
			}
		}
		
		void IBindableWidget.OnBoundDataChanged(string property, object val)
		{
			Logger.GetInstance().WriteLine("updateValue:" + property);
			if (property.Equals("SelectedItem"))
			{
				SelectedItem = val;
			}
			else if (property.Equals("Items"))
			{
				updateCollection((IList)val);
			}
		}

		protected override bool getSelectedIter(out Gtk.TreeIter iter)
		{
			bool isSelected = false;
			isSelected = _combo.GetActiveIter(out iter);
			return isSelected;
		}
		
		protected override void setSelectedIter(Gtk.TreeIter iter)
		{
			_combo.SetActiveIter(iter);
		}
		
		private void updateCollection(IList sourceItems)
		{
			Logger.GetInstance().WriteLine("updateCollection:" + sourceItems.Count);
			if ((sourceItems != null) && (sourceItems.Count > 0))
			{
				foreach (object item in sourceItems)
				{
					insertItem((object)item);
				}
				Logger.GetInstance().WriteLine("items added");
			}
		}
		
		public override ItemsDisplayMode ItemsDisplayMode
		{
			get
			{
				return ItemsDisplayMode.ObjectToString;
			}
		}
		
		protected virtual void OnComboChanged (object sender, System.EventArgs e)
		{
			WidgetCore.SetPropertyValue("SelectedItem", SelectedItem);
		}		
		
		protected override void refreshValueInStore(object item, Hashtable itemValues, Gtk.TreeIter iter)
		{
			setValueInStore(item, itemValues, iter);
		}
		
		protected override void setValueInStore(object item, Hashtable itemValues, Gtk.TreeIter iter)
		{
			Logger.GetInstance().WriteLine("Setting value of item in combo:" + item.ToString());
			if (item == null)
			{
				_store.SetValue(iter, 0, String.Empty);
			}
			else
			{
				_store.SetValue(iter, 0 , (string) item.ToString());
			}
		}
		
		protected override void addTreeViewColumnOrRenderer(SimpleColumn column, int colNumber)
		{
			if (colNumber == 0)
			{
				_combo.Clear();
				Gtk.CellRendererText cell = new Gtk.CellRendererText();
				_combo.PackStart(cell, false);
				_combo.AddAttribute(cell, "text", 0);
			}
			else
			{
				throw new NotSupportedException("ComboBox can only have one column");
			}
		}
			
		protected override Gtk.TreeIter appendValueToStore(object item, Hashtable itemValues)
		{
			Gtk.TreeIter iter = _store.Append();
			setValueInStore(item, itemValues, iter);
			return iter;
		}
		
	}
}
