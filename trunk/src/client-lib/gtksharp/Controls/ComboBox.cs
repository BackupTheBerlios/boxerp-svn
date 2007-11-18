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

namespace Boxerp.Client.GtkSharp.Controls
{
	
	
	public class ComboBox : TreeModelWrapper<SimpleColumn, Boxerp.Client.GtkSharp.Controls.ComboBox>, 
	      IBindableWidget, ITreeModel
	{
		private Gtk.ComboBox _combo;
			
		public ComboBox()
			: base()
		{
			_combo = new Gtk.ComboBox();
			this.Add(_combo);
		}

		protected override Boxerp.Client.GtkSharp.Controls.ComboBox TreeModelWidget 
		{ 
			get
			{
				return this;
			}
		}
		
		public Gtk.TreeModel Model 
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
		
		protected override void refreshValueInStore(object item, ArrayList itemValues, Gtk.TreeIter iter)
		{
			setValueInStore(item, itemValues, iter);
		}
		
		protected override void setValueInStore(object item, ArrayList itemValues, Gtk.TreeIter iter)
		{
			if (item == null)
			{
				_store.SetValue(iter, 0, String.Empty);
			}
			else
			{
				_store.SetValue(iter, 0 , (string) item.ToString());
			}
		}
		
		protected override Gtk.TreeIter appendValueToStore(object item, ArrayList itemValues)
		{
			Gtk.TreeIter iter = _store.Append();
			setValueInStore(item, itemValues, iter);
			return iter;
		}
		
	}
}
