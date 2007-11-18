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
using Gtk;
using System.Collections.Generic;
using Boxerp.Client.GtkSharp;
using System.Reflection;
using Boxerp.Client;
using Boxerp.Collections;

namespace Boxerp.Client.GtkSharp.Controls
{
	
	/// <summary>
	/// </summary>
	public abstract class TreeModelWrapper<T, C> : Gtk.Bin, IBindableWidget
		where C : ITreeModel
		where T : SimpleColumn, new()		
	{
		protected BindableWidgetCore _widgetCore;
		protected BindingDescriptor<T> _descriptor;
		protected Gtk.ListStore _store;
		protected InterceptedList<object> _items = new InterceptedList<object>();
		protected IBindingList _boundItems;
		protected ItemsDisplayMode _itemsDisplayMode = ItemsDisplayMode.AutoCreateColumns;
		protected bool _itemsDisplayModeChanged = false;
		protected Type _itemsType = typeof(System.Object);		
		protected PropertyInfo[] _itemsTypeProperties = null;
		protected bool _columnsInitialized = false; 
		protected Dictionary<int, object> _itemsPointers = new Dictionary<int, object>();
		protected Dictionary<object, Gtk.TreeIter> _itersPointers = new Dictionary<object, Gtk.TreeIter>();
		
		public TreeModelWrapper()
		{
			//Stetic.Gui.Initialize(this);
            //Stetic.BinContainer.Attach(this);
            //this.Name = this.GetType().ToString();
			_items.ItemAddedEvent += OnItemAdded;
			_items.ClearEvent += OnItemsClear;
			_items.ItemRemovedEvent += OnItemRemoved;
			_widgetCore = new BindableWidgetCore(this);
		}

		protected abstract C TreeModelWidget { get; }
		
		public BindableWidgetCore WidgetCore
		{
			get
			{
				return _widgetCore;
			}
		}
		
		public virtual ItemsDisplayMode ItemsDisplayMode
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
		
		
		
		public BindingDescriptor<T> BindingDescriptor
		{
			get
			{
				return _descriptor;
			}
			set
			{
				_descriptor = value;
			}
		}
		
		public InterceptedList<object> Items 
		{
			get 
			{
				Logger.GetInstance().WriteLine("getting items");
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
		
		protected abstract bool getSelectedIter(out Gtk.TreeIter iter);
		protected abstract void setSelectedIter(Gtk.TreeIter iter);
			
		/// <value>
		/// If the SelectionMode is Multiple, this property will return null even if there are 
		/// items selected. For that case use the SelectedItems property
		/// </value>
		public object SelectedItem
		{
			get
			{
				Gtk.TreeIter iter;
				if (getSelectedIter(out iter))
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
				if (_items.Contains(value))
				{
					setSelectedIter(_itersPointers[value]);
				}
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
			Logger.GetInstance().WriteLine("on item added");
			checkDataBindingIsNull();
			insertItem((object)sender); 
		}
		
		private void OnItemChanged(System.Object sender, PropertyChangedEventArgs args)
		{
			if (this._itersPointers.ContainsKey(sender))
			{
				Gtk.TreeIter iter = _itersPointers[sender];
				refreshValueInStore(sender, getItemValues(sender), iter);
			}
			else
			{
				throw new NotSupportedException("wrong!");
			}
		}
		
		private void OnItemsClear(System.Object sender, EventArgs args)
		{
			checkDataBindingIsNull();
			clear();
		}
		
		private void OnItemRemoved(System.Object sender, EventArgs args)
		{
			removeItem((object)sender);
		}
		
		#region IUIWidget implementation
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
				initializeTreeModelWidget(sourceItems[0]);
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
		
		protected void removeItemFromCurrentCollection(object item)
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
		
		private void checkDataBindingIsNull()
		{
			if (BoundItems != null)
			{
				throw new NotSupportedException("The ListView has items bound to it. Please call Unbind before using the Items property");
			}
		}
		
		public void Unbind()
		{
			BoundItems = null;
		}
		
		protected virtual void clear()
		 {
			checkDataBindingIsNull();
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

		protected virtual void removeTreeModelWidgetColumns(){}
				
		protected virtual void initializeTreeModelWidget(object firstItem)
		{
			Logger.GetInstance().WriteLine("initializing treeview: " + firstItem);
			_itemsType = firstItem.GetType();
			_itemsTypeProperties = null;
			
			removeTreeModelWidgetColumns();
			
			if (_store != null)
			{
				TreeModelWidget.Model = null;
				_store.Clear();
				_store.Dispose();
			}
			Logger.GetInstance().WriteLine("initialize items:" + _itemsType);
			createColumns();
		}
		
		protected List<T> readObjectTypes()
		{
			List<T> columnTypes = new List<T>();
			if (_itemsTypeProperties == null)
			{
				_itemsTypeProperties = _itemsType.GetProperties();
			}
			foreach (PropertyInfo pInfo in _itemsTypeProperties)
            {
				Logger.GetInstance().WriteLine("Reading property: " + pInfo.Name);
				T scolumn = new T();
				scolumn.Name = pInfo.Name;
				scolumn.DataType = pInfo.PropertyType;
				scolumn.Visible = true;
				columnTypes.Add(scolumn);
			}
			
			return columnTypes;
		}
			
		protected void createColumns()
		{
			List<T> columns = null;
			
			if (_itemsDisplayMode == ItemsDisplayMode.ObjectToString)
			{
				columns = new List<T>();
				T column = new T();
				column.DataType = typeof(string);
				column.Name = this._itemsType.ToString();
				column.Visible = true;
				columns.Add(column);				
			}
			else if (_itemsDisplayMode == ItemsDisplayMode.AutoCreateColumns)
			{
				columns = readObjectTypes();
			}
			else if (_itemsDisplayMode == ItemsDisplayMode.BindingDescriptor)
			{
				if (BindingDescriptor != null)
				{
					columns = BindingDescriptor.BindingColumns;
				}
				else
				{
					throw new NullReferenceException("The ItemsDisplayMode is BindingDescriptor but it is null");
				}
			}
			
			try
			{
				System.Type[] columnsTypes = new Type[columns.Count];
				int i = 0;
				foreach (T column in columns)
        	    {
        	    	columnsTypes[i] = column.DataType;
					Logger.GetInstance().WriteLine("COLUMN objectYPE =" + column.DataType);
					i++;
        	    }
        	    _store = new ListStore(columnsTypes);
        	    TreeModelWidget.Model = _store;
				Logger.GetInstance().WriteLine("create columns 3" );
								
				i = 0;
				foreach (T column in columns)
				{
					addTreeViewColumn(column, i);
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
			
		protected virtual void addTreeViewColumn(T column, int colNumber){}
		
		protected abstract void setValueInStore(object item, ArrayList itemValues, Gtk.TreeIter iter);
			
		protected abstract Gtk.TreeIter appendValueToStore(object item, ArrayList itemValues);
		
		protected abstract void refreshValueInStore(object item, ArrayList itemValues, Gtk.TreeIter iter);
		
		protected ArrayList getItemValues(object item)
		{
			ArrayList itemValues;
			    
			if (_itemsDisplayMode == ItemsDisplayMode.ObjectToString)
			{
				itemValues = new ArrayList();
				itemValues.Add(item.ToString());
			}
			else 
			{
				itemValues = getItemPropertiesValues(item);
			}
			
			return itemValues;
		}
		
		protected virtual TreeIter insertItem(object item)
		{
			if (typeof(INotifyPropertyChanged).IsAssignableFrom(item.GetType()))
			{
				INotifyPropertyChanged notifiable = (INotifyPropertyChanged) item;
				notifiable.PropertyChanged += OnItemChanged;
			}
			
			Logger.GetInstance().WriteLine("insert item: " + item);
			if (_itemsDisplayModeChanged || 
			    ((_itemsTypeProperties == null) && 
			     (ItemsDisplayMode != ItemsDisplayMode.ObjectToString)))
			{
				_itemsDisplayModeChanged = false;
				initializeTreeModelWidget(item);				
			}
			
			Logger.GetInstance().WriteLine("insert item:" + item);
			ArrayList itemValues = getItemValues(item);
			    
			if (TreeModelWidget.Model != null)
			{
				TreeIter iter = appendValueToStore(item, itemValues);
				_itersPointers[item] = iter;
				_itemsPointers[iter.UserData.GetHashCode()] = item;
				Logger.GetInstance().WriteLine("Iter userData=" + iter.UserData.GetHashCode());
				return iter;
		    }
		    else
		    {
		        return Gtk.TreeIter.Zero;
		    }
		}
		
		protected void removeItem(object item)
		{
			Gtk.TreeIter iter = _itersPointers[item];
			_itersPointers.Remove(item);
			Logger.GetInstance().WriteLine("removing item:" + iter.UserData.GetHashCode());
			_itemsPointers.Remove(iter.UserData.GetHashCode());
			_store.Remove(ref iter);			
		}
		
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
