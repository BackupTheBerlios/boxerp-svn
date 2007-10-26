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
	/// <summary>
	/// TODO: Keep a reference to every item in the treeview somehow. To get the selected objects
	/// 
	/// Use cases: 
	///  1 - The user binds a collection: clean everything and initialize. If Items and BoundItems are used both, throw exception
	///  2 - The user is using the Items properties. The items on it implement the INotifyPropertyChanged. What happen when an item changes? 
	/// 
	/// </summary>
	public partial class ListView : Gtk.Bin, IBindableWidget
	{
		private BindableWidgetCore _widgetCore;
		protected InterceptedList<object> _items = new InterceptedList<object>();
		protected IBindingList _boundItems;
		protected ListStore _store;
		private Type _itemsType = typeof(System.Object);
		// TODO: Widgets shouldn't support multithreading, throw an exception if that happens
		private PropertyInfo[] _itemsTypeProperties = null;
		protected bool _columnsInitialized = false; 
		private ItemsDisplayMode _itemsDisplayMode = ItemsDisplayMode.Reflection;

		private Dictionary<TreeIter, object> _itemsPointers = new Dictionary<TreeIter,object>();
		private Dictionary<object, TreeIter> _itersPointers = new Dictionary<object,TreeIter>();
		
		public ItemsDisplayMode ItemsDisplayMode
		{
			get
			{
				return _itemsDisplayMode;
			}
			set
			{
				_itemsDisplayMode = value;
			}
		}
		
		public InterceptedList<object> Items 
		{
			get 
			{
				return _items;
			}
		}
		
		public IBindingList BoundItems
		{
			get
			{	
				return _boundItems;
			}
			internal set
			{
				_boundItems = value;
			}
		}
		
		public Gtk.SelectionMode SelectionMode
		{
			get
			{
				return _treeview.Selection.Mode;
			}
			set
			{
				_treeview.Selection.Mode = value;
			}
		}
		
		public object SelectedItem
		{
			get
			{
				TreeIter iter;
				if (_treeview.Selection.GetSelected(out iter))
				{
					 return _itemsPointers[iter];
				}
				
				return null;
			}
			set
			{
				if (_items.Contains(value))
				{
					_treeview.Selection.SelectIter(_itersPointers[value]);
				}
			}
		}
		
		public List<object> SelectedItems
		{
			get
			{
				if (_treeview.Selection.Mode != Gtk.SelectionMode.Multiple)
				{
					return null;
				}
				
				TreePath[] pathArray = _treeview.Selection.GetSelectedRows();
				if (pathArray.Length > 0)
				{
					List<object> selectedItems = new List<object>();
					foreach (TreePath path in pathArray)
					{
						TreeIter iter;
						_store.GetIter(out iter, path);
						selectedItems.Add(_itemsPointers[iter]);
					}
					
					return selectedItems;
				}
				
				return null;
			}
		}
		
		internal TreeView TreeView
		{
			get
			{
				return _treeview;
			}
		}
		
		public ListView()
		{
			this.Build();
			_widgetCore = new BindableWidgetCore(this);
			_items.ItemAddedEvent += OnItemAdded;
			_items.ClearEvent += OnItemsClear;
			_items.ItemRemovedEvent += OnItemRemoved;
		}
		
		private void OnItemAdded(System.Object sender, EventArgs args)
		{
			insertItem(sender); 
		}
		
		private void OnItemsClear(System.Object sender, EventArgs args)
		{
			_store.Clear();
			_itemsPointers.Clear();
			_itersPointers.Clear();
		}
		
		private void OnItemRemoved(System.Object sender, EventArgs args)
		{
			removeItem(sender);
		}
		
		private void OnBoundItemsListChanged(System.Object sender, ListChangedEventArgs args)
		{
			if (args.ListChangedType == ListChangedType.ItemAdded)
			{
				insertItem(sender);
			}
			else if (args.ListChangedType == ListChangedType.ItemDeleted)
			{
				removeItem(sender);
			}
			else if (args.ListChangedType == ListChangedType.ItemChanged)
			{
				
			}
		}
		
        #region IUIWidget implementation
		public BindableWidgetCore WidgetCore
		{
			get
			{
				return _widgetCore;
			}
		}

		public void BindObject(IBindableWrapper wrapper, string path, string widgetProperty, BindingOptions options)
		{
			_widgetCore.BindObject(wrapper, path, widgetProperty, options);
		}
		
		public void BindObject(IBindableWrapper wrapper, object owner, string path, string widgetProperty, BindingOptions options)
		{
			_widgetCore.BindObject(wrapper, owner, path, widgetProperty, options);
		}
		
		// IBindableWidget
		public void UpdateValue(string property, object val)
		{
			Console.WriteLine("updateValue:" + property);
			if (property.Equals("BoundItems"))
			{
				_boundItems = (IBindingList)val;
				_boundItems.ListChanged += OnBoundItemsListChanged;
				updateCollection(_boundItems);
			}
			if (property.Equals("Items"))
			{
				updateCollection((IList)val);
			}
			else
			{
				updateItem(val);
			}
		}
        #endregion
		
		private void updateCollection(IList sourceItems)
		{
			Console.WriteLine("updateCollection:" + sourceItems.Count);
			if ((sourceItems != null) && (sourceItems.Count > 0))
			{
				initializeItems(sourceItems[0]);
				foreach (object item in sourceItems)
				{
					insertItem(item);
				}
			}
		}
		
		private void initializeItems(object firstItem)
		{
			_itemsType = firstItem.GetType();
			_itemsTypeProperties = null;
				
			Console.WriteLine("updateCollection:" + _itemsType);
			createColumns();
		}
		
		private void updateItem(object item)
		{
			
		}
		
		private List<SimpleColumn> readObjectTypes()
		{
			List<SimpleColumn> columnTypes = new List<SimpleColumn>();
			if (_itemsTypeProperties == null)
			{
				_itemsTypeProperties = _itemsType.GetProperties();
			}
			foreach (PropertyInfo pInfo in _itemsTypeProperties)
            {
				Console.WriteLine("Reading property: " + pInfo.Name);
				SimpleColumn scolumn = new SimpleColumn();
				scolumn.Name = pInfo.Name;
				scolumn.Type = pInfo.PropertyType;
				scolumn.Visible = true;
				columnTypes.Add(scolumn);
			}
			
			return columnTypes;
		}
			
		private void createColumns()
		{
			List<SimpleColumn> columns = readObjectTypes();
			try
			{
				System.Type[] columnsTypes = new Type[columns.Count];
				int i = 0;
				foreach (SimpleColumn column in columns)
        	    {
        	    	columnsTypes[i] = column.Type;
					Console.WriteLine("COLUMN TYPE =" + column.Type);
					i++;
        	    }
        	    _store = new ListStore(columnsTypes);
        	    _treeview.Model = _store;
				Console.WriteLine("create columns 3" );
								
				i = 0;
				foreach (SimpleColumn column in columns)
				{
                	if (column.Type == typeof(Gdk.Pixbuf))
                	{
			        	TreeViewColumn tc = _treeview.AppendColumn ("", new CellRendererPixbuf (), "pixbuf", i);
				    	if (tc != null)
						{
					    	tc.Visible = column.Visible;
						}
					}
					else if (column.Type == typeof(System.Object)) 
					{
						Gtk.TreeViewColumn objColumn = new Gtk.TreeViewColumn ();
						objColumn.Title = column.Name;
						Gtk.CellRendererText objCell = new Gtk.CellRendererText ();
						objColumn.PackStart (objCell, true);
						objColumn.SetCellDataFunc (objCell, new Gtk.TreeCellDataFunc (RenderObject));		
						_treeview.AppendColumn(objColumn);
					}
                	else
                	{
                		if (_treeview == null)
						{
                			_treeview = new Gtk.TreeView();
						}
						
				    	TreeViewColumn tc = 
							_treeview.AppendColumn (column.Name, new CellRendererText (), "text", i);
						Console.WriteLine("appending column:" + column.Name + i);
						
						if (tc != null)
						{
				    		tc.Visible = column.Visible;
						}
                	}
					i++;
				}
				
				_columnsInitialized = true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Create exception:" + ex.Message + ex.StackTrace);
				throw ex;
			}
		}
	
		private void removeItem(object item)
		{
			TreeIter iter = _itersPointers[item];
			_store.Remove(ref iter);
		}
		
		private TreeIter insertItem(object item)
		{
			ArrayList itemValues = getItemPropertiesValues(item);
			
			TreeIter iter = _store.Append();
		    if (_treeview.Model != null)
		    {
				_itersPointers[item] = iter;
				_itemsPointers[iter] = item;
		        //_store.SetValue(iter, 0, item.ToString()); // the first item is the object itself
				int i = 0;
				foreach (object itm in itemValues)
				{
					Console.WriteLine("inserting value:" + itm);
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
						// TODO: add more cases with the remaining types
						default:
									_store.SetValue(iter, i, (string) itm.ToString());		
						    break;
					}
					
					i++;
				}
				
		        return iter;
		    }
		    else
		    {
		        return Gtk.TreeIter.Zero;
		    }
		}
		
		private ArrayList getItemPropertiesValues(object item)
		{
			if (_itemsTypeProperties == null)
			{
				initializeItems(item);
			}
			
			ArrayList values = new ArrayList();
			List<string> properties = new List<string>();
			
			foreach (PropertyInfo pInfo in _itemsTypeProperties)
            {
                if (!properties.Contains(pInfo.Name))
                {
                    if (pInfo.GetGetMethod().GetParameters().Length > 0)
					{
						// TODO : indexed property
					}
					else
					{
						object val = pInfo.GetValue(item, null);
						Console.WriteLine("reading value " + val);
						values.Add(val);
					}
					properties.Add(pInfo.Name);
               }
			}
			return values;
		}
		
		private void RenderObject (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			//System.Object obj = model.GetValue(column. (iter, 0);
			//(cell as Gtk.CellRendererText).Text = model.GetValue(iter, 0).ToString();// .Data.ToString();
		}
	}
}
