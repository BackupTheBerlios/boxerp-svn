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

	// TODO: strongly typed listview:
	/*public class ListView<T> : ListView
	{
		private InterceptedList<T> _items = new InterceptedList<T>();
		
		public InterceptedList<T> Items
		{
			get
			{
				return _items;
			}
		}
	}*/
	
	/// <summary>
	/// Use cases: 
	///  1 - The user binds a collection: clean everything and initialize. If Items and BoundItems are used both, throw exception
	///  2 - The user is using the Items properties. The items on it implement the INotifyPropertyChanged. What happen when an item changes? 
	/// 
	/// </summary>
	public partial class ListView : TreeViewWrapper, IBindableWidget
	{
		private BindableWidgetCore _widgetCore;
		protected InterceptedList<object> _items = new InterceptedList<object>();
		protected IBindingList _boundItems;
		private ItemsDisplayMode _itemsDisplayMode = ItemsDisplayMode.Reflection;
		private bool _itemsDisplayModeChanged = false;
		
		public ItemsDisplayMode ItemsDisplayMode
		{
			get
			{
				return _itemsDisplayMode;
			}
			set
			{
				if (value != _itemsDisplayMode)
				{
					_itemsDisplayModeChanged = true;
					
					if (BoundItems != null)
					{
						throw new NotSupportedException("Changing the items display mode when there are items bound to the list is not allow. Please call Unbind first");
					}
					_items.Clear();
					_itemsDisplayMode = value;
				}
			}
		}
		
		protected override Gtk.TreeView TreeView 
		{ 
			get
			{
				return _treeview;
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
		
		public override object SelectedItem
		{
			get
			{
				return base.SelectedItem;
			}
			set
			{
				if (SelectionMode != Gtk.SelectionMode.Single)
				{
					throw new Exception("The selection mode is Multiple and have to be Single");
				}
				if (_items.Contains(value))
				{
					_treeview.Selection.SelectIter(_itersPointers[value]);
				}
			}
		}
		
		protected override void removeItemFromCurrentCollection(object item)
		{
			if (BoundItems != null)
			{
				BoundItems.Remove(item);	
			}
			else
			{
				Items.Remove(item);
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
		
		private void checkDataBindingIsNull()
		{
			if (BoundItems != null)
			{
				throw new NotSupportedException("The ListView has items bound to it. Please call Unbind before using the Items property");
			}
		}
		
		/// <summary>
		/// Invoke when an item is added to the Items property
		/// </summary>
		/// <param name="sender">
		/// A <see cref="System.Object"/>
		/// </param>
		/// <param name="args">
		/// A <see cref="EventArgs"/>
		/// </param>
		private void OnItemAdded(System.Object sender, EventArgs args)
		{
			checkDataBindingIsNull();
			insertItem((object)sender); 
		}
		
		private void OnItemsClear(System.Object sender, EventArgs args)
		{
			checkDataBindingIsNull();
			clear();
		}
		
		protected override void clear()
		 {
			checkDataBindingIsNull();
			base.clear();
		}
		
		public void Unbind()
		{
			BoundItems = null;
		}
		
		private void OnItemRemoved(System.Object sender, EventArgs args)
		{
			removeItem((object)sender);
		}
						
		private void OnBoundItemsListChanged(System.Object sender, ListChangedEventArgs args)
		{
			Logger.GetInstance().WriteLine("OnBoundItemsListChanged:" + args.ListChangedType);
			if (args.ListChangedType == ListChangedType.ItemAdded)
			{
				insertItem(this.BoundItems[args.NewIndex]);
			}
			else if (args.ListChangedType == ListChangedType.ItemDeleted)
			{
				removeAt(args.NewIndex);// removeItem(sender);
			}
			else if (args.ListChangedType == ListChangedType.ItemChanged)
			{
				Logger.GetInstance().WriteLine("Bound item has changed");
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
		void IBindableWidget.OnBoundDataChanged(string property, object val)
		{
			Logger.GetInstance().WriteLine("updateValue:" + property);
			if (property.Equals("BoundItems"))
			{
				_boundItems = (IBindingList)val;
				_boundItems.ListChanged += OnBoundItemsListChanged;
				updateCollection(_boundItems);
			}
			if (property.Equals("Items"))
			{
				if (WidgetCore.BindingOptions == BindingOptions.TwoWay)
				{
					throw new NotSupportedException("The Items property does not support two-way databinding. Please use the BoundItems property");
				}
				else
				{
					updateCollection((IList)val);
				}
			}
			else
			{
				updateItem(val);
			}
		}
        #endregion
		
		private void updateCollection(IList sourceItems)
		{
			Logger.GetInstance().WriteLine("updateCollection:" + sourceItems.Count);
			if ((sourceItems != null) && (sourceItems.Count > 0))
			{
				initializeTreeView(sourceItems[0]);
				foreach (object item in sourceItems)
				{
					insertItem((object)item);
				}
				Logger.GetInstance().WriteLine("items added");
			}
		}
				
		private void updateItem(object item)
		{
			
		}
		
		protected override void createColumns()
		{
			List<SimpleColumn> columns;
			
			if (_itemsDisplayMode != ItemsDisplayMode.Reflection)
			{
				columns = new List<SimpleColumn>();
				SimpleColumn column = new SimpleColumn();
				column.Type = typeof(string);
				column.Name = this._itemsType.ToString();
				column.Visible = true;
				columns.Add(column);				
			}
			else
			{
				columns = readObjectTypes();
			}
			
			try
			{
				System.Type[] columnsTypes = new Type[columns.Count];
				int i = 0;
				foreach (SimpleColumn column in columns)
        	    {
        	    	columnsTypes[i] = column.Type;
					Logger.GetInstance().WriteLine("COLUMN objectYPE =" + column.Type);
					i++;
        	    }
        	    _store = new ListStore(columnsTypes);
        	    _treeview.Model = _store;
				Logger.GetInstance().WriteLine("create columns 3" );
								
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
						Logger.GetInstance().WriteLine("appending column:" + column.Name + i);
						
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
				Logger.GetInstance().WriteLine("Create exception:" + ex.Message + ex.StackTrace);
				throw ex;
			}
		}
	
		protected override TreeIter insertItem(object item)
		{
			if (_itemsDisplayModeChanged || 
			    ((_itemsTypeProperties == null) && (ItemsDisplayMode == ItemsDisplayMode.Reflection)))
			{
				_itemsDisplayModeChanged = false;
				initializeTreeView(item);				
			}
			
			Logger.GetInstance().WriteLine("insert item:" + item);
			ArrayList itemValues;
			    
			if (_itemsDisplayMode != ItemsDisplayMode.Reflection)
			{
				itemValues = new ArrayList();
				itemValues.Add(item.ToString());
			}
			else
			{
				itemValues = getItemPropertiesValues(item);
			}
			
			TreeIter iter = _store.Append();
		    if (_treeview.Model != null)
		    {
				_itersPointers[item] = iter;
				
		        //_store.SetValue(iter, 0, item.ToString()); // the first item is the object itself
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
				_itemsPointers[iter.UserData.GetHashCode()] = item;
				Logger.GetInstance().WriteLine("Iter userData=" + iter.UserData.GetHashCode());
				
		        return iter;
		    }
		    else
		    {
		        return Gtk.TreeIter.Zero;
		    }
		}
	}
}
