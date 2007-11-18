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
using System.Collections;
using System.Collections.Generic;

namespace Boxerp.Client.GtkSharp.Controls
{	
	public abstract class TreeViewWrapper<T> : TreeModelWrapper<T, CustomTreeView> 
		where T : SimpleColumn, new ()
	{
		public TreeViewWrapper()
			: base ()
		{
		}

		//protected abstract CustomTreeView TreeModelWidget { get; }
		
		public virtual Gtk.SelectionMode SelectionMode
		{
			get
			{
				return TreeModelWidget.Selection.Mode;
			}
			set
			{
				TreeModelWidget.Selection.Mode = value;
			}
		}
		
		protected override bool getSelectedIter(out Gtk.TreeIter iter)
		{
			bool isSelected = false;
			isSelected = TreeModelWidget.Selection.GetSelected(out iter);
			return isSelected;
		}
		
		protected override void setSelectedIter(Gtk.TreeIter iter)
		{
			TreeModelWidget.Selection.SelectIter(iter);
		}
		
		public virtual List<object> SelectedItems
		{
			get
			{
				if (SelectionMode != Gtk.SelectionMode.Multiple)
				{
					return null;
				}
				
				Gtk.TreePath[] pathArray = TreeModelWidget.Selection.GetSelectedRows();
				if (pathArray.Length > 0)
				{
					List<object> selectedItems = new List<object>();
					foreach (Gtk.TreePath path in pathArray)
					{
						Gtk.TreeIter iter;
						_store.GetIter(out iter, path);
						selectedItems.Add(_itemsPointers[iter.UserData.GetHashCode()]);
					}
					
					return selectedItems;
				}
				
				return null;
			}
			set
			{
				if (SelectionMode != Gtk.SelectionMode.Single)
				{
					throw new Exception("The selection mode is Multiple and have to be Single");
				}
				
				throw new NotImplementedException("This feature is not implemented yet");
			}
		}
		
		public void RemoveSelectedItems()
		{
			if (SelectedItems != null)
			{
				foreach (object item in SelectedItems)
				{
					removeItemFromCurrentCollection(item);
				}
			}
			else if (SelectedItem != null)
			{
				removeItemFromCurrentCollection(SelectedItem);
			}
		}
		
		protected override void removeTreeModelWidgetColumns()
		{
			foreach (Gtk.TreeViewColumn col in TreeModelWidget.Columns)
			{
				Logger.GetInstance().WriteLine("Removing column: " + col.Title);
				TreeModelWidget.RemoveColumn(col);
			}
		}

		protected override void setValueInStore(object item, ArrayList itemValues, Gtk.TreeIter iter)
		{
			int i = 0;
			foreach (object itm in itemValues)
			{
				if (itm == null)
				{
					_store.SetValue(iter, i, String.Empty);
				}
				else
				{
					Logger.GetInstance().WriteLine("inserting value:" + itm.GetType().ToString());
				
					switch (itm.GetType().ToString())
					{
					    case "System.Object":
						            _store.SetValue(iter, i, (string) itm.ToString());
						    break;
						case "System.String" :
							        _store.SetValue(iter, i, (string) itm);
							break;
						case "System.Int32" :
									_store.SetValue(iter, i, (int) itm);
							break;
					    case "System.Double":
						            _store.SetValue(iter, i, (double) itm);
						    break;
						case "System.Enum":
						            _store.SetValue(iter, i, (string) itm.ToString());
						    break;
					    case "null" :
						            Logger.GetInstance().WriteLine("Null column value");
						            _store.SetValue(iter, i, String.Empty);
						    break;
						// objectODO: add more cases with the remaining types
						default:
									_store.SetValue(iter, i, (string) itm.ToString());		
						    break;
					}
				}
				i++;
			}
		}
		
		protected override void refreshValueInStore(object item, ArrayList itemValues, Gtk.TreeIter iter)
		{
			setValueInStore(item, itemValues, iter);
		}
		
		protected override Gtk.TreeIter appendValueToStore(object item, ArrayList itemValues)
		{
			Gtk.TreeIter iter = _store.Append();
			setValueInStore(item, itemValues, iter);
			return iter;
		}
		
	}
}
