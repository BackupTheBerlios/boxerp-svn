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
//// objectHIS SOFobjectWARE IS PROVIDED BY objectHE AUobjectHOR ``AS IS'' AND ANY
//// EXPRESS OR IMPLIED WARRANobjectIES, INCLUDING, BUobject NOobject LIMIobjectED objectO,
//// objectHE IMPLIED WARRANobjectIES OF MERCHANobjectABILIobjectY AND FIobjectNESS FOR A
//// PARobjectICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENobject SHALL objectHE AUobjectHOR
//// BE LIABLE FOR ANY DIRECobject, INDIRECobject, INCIDENobjectAL, SPECIAL,
//// EXEMPLARY, OR CONSEQUENobjectIAL DAMAGES (INCLUDING, BUobject NOobject LIMIobjectED
//// objectO, PROCUREMENobject OF SUBSobjectIobjectUobjectE GOODS OR SERVICES; LOSS OF USE,
//// DAobjectA, OR PROFIobjectS; OR BUSINESS INobjectERRUPobjectION) HOWEVER CAUSED AND
//// ON ANY objectHEORY OF LIABILIobjectY, WHEobjectHER IN CONobjectRACobject, SobjectRICobject
//// LIABILIobjectY, OR objectORobject (INCLUDING NEGLIGENCE OR OobjectHERWISE) ARISING
//// IN ANY WAY OUobject OF objectHE USE OF objectHIS SOFobjectWARE, EVEN IF ADVISED OF
//// objectHE POSSIBILIobjectY OF SUCH DAMAGE.
//
//

using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using Boxerp.Client.GtkSharp;
using System.Reflection;
using Boxerp.Client;
using Boxerp.Collections;

namespace Boxerp.Client.GtkSharp.Controls
{
	
	/// <summary>
	/// </summary>
	public abstract class TreeViewWrapper : Gtk.Bin
	{
		protected Gtk.ListStore _store;
		protected Type _itemsType = typeof(System.Object);		
		protected PropertyInfo[] _itemsTypeProperties = null;
		protected bool _columnsInitialized = false; 
		protected Dictionary<int, object> _itemsPointers = new Dictionary<int, object>();
		protected Dictionary<object, Gtk.TreeIter> _itersPointers = new Dictionary<object, Gtk.TreeIter>();
		
		public Gtk.SelectionMode SelectionMode
		{
			get
			{
				return TreeView.Selection.Mode;
			}
			set
			{
				TreeView.Selection.Mode = value;
			}
		}
		
		/// <value>
		/// If the SelectionMode is Multiple, this property will return null even if there are 
		/// items selected. For that case use the SelectedItems property
		/// </value>
		public virtual object SelectedItem
		{
			get
			{
				if (SelectionMode != Gtk.SelectionMode.Single)
				{
					return null;
				}
				
				Gtk.TreeIter iter;
				if (TreeView.Selection.GetSelected(out iter))
				{
					Logger.GetInstance().WriteLine("Iter userData=" + iter.UserData.GetHashCode());
					 return _itemsPointers[iter.UserData.GetHashCode()];
				}
				else
				{
					Logger.GetInstance().WriteLine("nothing selected");
					Logger.GetInstance().WriteLine("nothing selected:" + iter.UserData.GetHashCode());
				}
				
				return null;
			}
			set
			{
				
			}
		}
		
		public List<object> SelectedItems
		{
			get
			{
				if (SelectionMode != Gtk.SelectionMode.Multiple)
				{
					return null;
				}
				
				Gtk.TreePath[] pathArray = TreeView.Selection.GetSelectedRows();
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
		
		protected abstract void removeItemFromCurrentCollection(object item);
		
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
		
		protected abstract Gtk.TreeView TreeView { get; }
		
		public TreeViewWrapper()
		{
			
		}
				
		protected virtual void clear()
		 {
			_store.Clear();
			_itemsPointers.Clear();
			_itersPointers.Clear();
		}
						
		protected void removeAt(int rowNumber)
		{
			Gtk.TreeIter iter;
			_store.GetIterFirst(out iter);
			
			int i = 0;
			while (rowNumber > i)
			{
				if (_store.IterNext(ref iter))
				{
					i++;
				}
				else
				{
					break;
				}
			}
			if (i == rowNumber)
			{
				_itersPointers.Remove(_itemsPointers[iter.UserData.GetHashCode()]);
				_itemsPointers.Remove(iter.UserData.GetHashCode());
				_store.Remove(ref iter);				
			}
		}
		
		protected virtual void initializeTreeView(object firstItem)
		{
			_itemsType = firstItem.GetType();
			_itemsTypeProperties = null;
			foreach (Gtk.TreeViewColumn col in TreeView.Columns)
			{
				Logger.GetInstance().WriteLine("Removing column: " + col.Title);
				TreeView.RemoveColumn(col);
			}
			if (_store != null)
			{
				TreeView.Model = null;
				_store.Clear();
				_store.Dispose();
			}
			Logger.GetInstance().WriteLine("initialize items:" + _itemsType);
			createColumns();
		}
		
		protected List<SimpleColumn> readObjectTypes()
		{
			List<SimpleColumn> columnTypes = new List<SimpleColumn>();
			if (_itemsTypeProperties == null)
			{
				_itemsTypeProperties = _itemsType.GetProperties();
			}
			foreach (PropertyInfo pInfo in _itemsTypeProperties)
            {
				Logger.GetInstance().WriteLine("Reading property: " + pInfo.Name);
				SimpleColumn scolumn = new SimpleColumn();
				scolumn.Name = pInfo.Name;
				scolumn.Type = pInfo.PropertyType;
				scolumn.Visible = true;
				columnTypes.Add(scolumn);
			}
			
			return columnTypes;
		}
			
		protected abstract void createColumns();
			
		protected void removeItem(object item)
		{
			Gtk.TreeIter iter = _itersPointers[item];
			_itersPointers.Remove(item);
			Logger.GetInstance().WriteLine("removing item:" + iter.UserData.GetHashCode());
			_itemsPointers.Remove(iter.UserData.GetHashCode());
			_store.Remove(ref iter);			
		}
		
		protected abstract Gtk.TreeIter insertItem(object item);
		
		
		protected ArrayList getItemPropertiesValues(object item)
		{
			Logger.GetInstance().WriteLine("item type properties initialized:" + item.ToString());
			ArrayList values = new ArrayList();
			List<string> properties = new List<string>();
			
			foreach (PropertyInfo pInfo in _itemsTypeProperties)
            {
				Logger.GetInstance().WriteLine("Reading object property:" + pInfo.Name);
                if (!properties.Contains(pInfo.Name))
                {
                    if (pInfo.GetGetMethod().GetParameters().Length > 0)
					{
						// TODO : indexed property
					}
					else
					{
						Logger.GetInstance().WriteLine("trying to read value");
						object val = pInfo.GetValue(item, null);
						Logger.GetInstance().WriteLine("reading value " + val);
						values.Add(val);
					}
					properties.Add(pInfo.Name);
               }
			}
			return values;
		}
		
		protected void RenderObject (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			//System.Object obj = model.GetValue(column. (iter, 0);
			//(cell as Gtk.CelRendererText).objectext = model.GetValue(iter, 0).ToString();// .Data.ToString();
		}
	}
}
