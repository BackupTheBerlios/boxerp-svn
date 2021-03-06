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
	/// Gtk.TreeView and Gtk.ComboBox use a Model-View-Controller approach to separate 
	/// concerns but that makes the API complex and it is not straightforward to 
	/// deal with those widgets. This class is a wrapper around a Gtk.TreeModel that 
	/// makes the API more simple. You can use the Boxerp ComboBox and the Boxerp 
	/// ListView or DataGrid now in the same manner because of this hierarchy.
	/// </summary>
	public abstract class TreeModelWrapper<T> : Gtk.Bin, IBindableWidget
		where T : SimpleColumn, new()		
	{
		protected BindableWidgetCore _widgetCore;
		protected BindingDescriptor<T> _descriptor;
		protected Gtk.ListStore _store;
		protected InterceptedList<object> _items = new InterceptedList<object>();
		protected IBindingList _boundItems;
		protected ItemsDisplayMode _itemsDisplayMode = ItemsDisplayMode.AutoCreateColumns;
		protected bool _itemsDisplayModeChanged = true;
		protected Type _itemsType = typeof(System.Object);		
		protected PropertyInfo[] _itemsTypeProperties = null;
		protected bool _columnsInitialized = false; 
		protected Dictionary<int, object> _itemsPointers = new Dictionary<int, object>();
		protected Dictionary<object, Gtk.TreeIter> _itersPointers = new Dictionary<object, Gtk.TreeIter>();
		protected Hashtable _itemValues = new Hashtable();
		protected Dictionary<string, int> _columnsOrder = new Dictionary<string, int>();
		protected System.Type[] _supportedDataTypes = 
		          new System.Type[] 
		          {
			            typeof(string),
			            typeof(int),
			            typeof(bool),
			            typeof(double)
		          };
		protected List<System.Type> _supportedTypes = new List<Type>();
		
		public TreeModelWrapper()
		{
			foreach (Type type in _supportedDataTypes)
			{
				_supportedTypes.Add(type);
			}
			_items.ItemAddedEvent += OnItemAdded;
			_items.ClearEvent += OnItemsClear;
			_items.ItemRemovedEvent += OnItemRemoved;
			_widgetCore = new BindableWidgetCore(this);
			
			this.SizeAllocated += OnSizeAllocated;
			this.SizeRequested += OnSizeRequested;
		}

		private void OnSizeAllocated(object sender, Gtk.SizeAllocatedArgs args)
		{
			Gdk.Rectangle rect = args.Allocation;
			InnerWidget.SizeAllocate(rect);
		}
		
		private void OnSizeRequested(object sender, Gtk.SizeRequestedArgs args)
		{
			SetSizeRequest(args.Requisition.Width, args.Requisition.Height);
		}
		
		/// <value>
		/// To access the Gtk widget that is being wrapped, get this property 
		/// and cast. It will allow you to change properties of the widget that 
		/// are not wrapped in this class hierarchy.
		/// </value>
		public abstract Gtk.Widget InnerWidget { get; }
		
		protected abstract Gtk.TreeModel Model { get; set; }

		/// <value>
		/// To support the Boxerp data binding architecture
		/// </value>
		public BindableWidgetCore WidgetCore
		{
			get
			{
				return _widgetCore;
			}
		}

		public bool ModelIsInitialized 
		{
			get
			{
				return !_itemsDisplayModeChanged;
			}
			set
			{
				_itemsDisplayModeChanged = !value;
			}
		}
		
		/// <value>
		/// See ItemsDisplayMode enumeration. If you change the display mode once 
		/// the widget is populated, you'll lose its content as the columns 
		/// have to be dropped and created from scratch. Change this property 
		/// just before adding items.
		/// </value>
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
					if (_items.Count > 0)
					{
					   _items.Clear();
					}
					
					_itemsDisplayMode = value;
				}
			}
		}
		
		/// <value>
		/// To descrive the way columns are created.
		/// </value>		
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
		
		/// <value>
		/// Access the Items in the widget when they are not a bound collection
		/// </value>
		public InterceptedList<object> Items 
		{
			get 
			{
				Logger.GetInstance().WriteLine("getting items");
				return _items;
			}
		}
		
		/// <value>
		/// Access the items of a Bound collection.
		/// </value>
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
					Logger.GetInstance().WriteLine("value:" + _itemsPointers[iter.UserData.GetHashCode()]);
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
				getItemValues(sender);
				refreshValueInStore(sender, _itemValues, iter);
			}
			else
			{
				throw new NotSupportedException("Trying to refresh an item that seems to be not in the list of items");
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
		
		/// <summary>
		/// Anytime your a going to bind a collection, you should unbind the one 
		/// that was bound before if any.
		/// </summary>
		public void Unbind()
		{
			BoundItems = null;
		}
		
		protected virtual void clear()
		 {
			checkDataBindingIsNull();
			if (_store != null)
			{
				_store.Clear();
				_itemsPointers.Clear();
				_itersPointers.Clear();
			}
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
				Model = null;
				_store.Clear();
				_store.Dispose();
			}
			createColumns();
			this.ModelIsInitialized = true;
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
				scolumn.ObjectPropertyName = pInfo.Name;
				if (_supportedTypes.Contains(pInfo.PropertyType))
				{
					scolumn.DataType = pInfo.PropertyType;
				}
				else
				{
					scolumn.DataType = typeof(string);
				}
				scolumn.Visible = true;
				columnTypes.Add(scolumn);
			}
			
			return columnTypes;
		}
			
		protected void createColumns()
		{
			List<T> columns = null;
			
			if (ItemsDisplayMode == ItemsDisplayMode.ObjectToString)
			{
				columns = new List<T>();
				T column = new T();
				column.DataType = typeof(string);
				column.Name = this._itemsType.ToString();
				column.ObjectPropertyName = column.Name;
				column.Visible = true;
				columns.Add(column);				
			}
			else if (ItemsDisplayMode == ItemsDisplayMode.AutoCreateColumns)
			{
				columns = readObjectTypes();
			}
			else if (ItemsDisplayMode == ItemsDisplayMode.BindingDescriptor)
			{
				if (BindingDescriptor != null)
				{
					if (_itemsTypeProperties == null)
					{
						_itemsTypeProperties = _itemsType.GetProperties();
					}
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
				_columnsOrder.Clear();
				int i = 0;
				foreach (T column in columns)
        	    {
        	    	columnsTypes[i] = column.DataType;
					_columnsOrder.Add(column.ObjectPropertyName, i);
					Logger.GetInstance().WriteLine("COLUMN objectYPE =" + column.DataType);
					Logger.GetInstance().WriteLine("Column name:" + column.ObjectPropertyName);
					i++;
        	    }
        	    _store = new ListStore(columnsTypes);
				Model = _store;
												
				foreach (T column in columns)
				{
					addTreeViewColumnOrRenderer(column, _columnsOrder[column.ObjectPropertyName]);
				}
				
				_columnsInitialized = true;
			}
			catch (Exception ex)
			{
				Logger.GetInstance().WriteLine("Create exception:" + ex.Message + ex.StackTrace);
				throw ex;
			}
		}
			
		protected virtual void addTreeViewColumnOrRenderer(T column, int colNumber){}
		
		protected abstract void setValueInStore(object item, Hashtable itemValues, Gtk.TreeIter iter);
			
		protected abstract Gtk.TreeIter appendValueToStore(object item, Hashtable itemValues);
		
		protected abstract void refreshValueInStore(object item, Hashtable itemValues, Gtk.TreeIter iter);
		
		protected void getItemValues(object item)
		{
			_itemValues.Clear();
			    
			if (ItemsDisplayMode == ItemsDisplayMode.ObjectToString)
			{
				Logger.GetInstance().WriteLine("getItemValues - _itemsType:" + _itemsType.ToString());
				_itemValues.Add(_itemsType.ToString(), item.ToString());
			}
			else 
			{
				getItemPropertiesValues(item);
			}
		}
		
		protected virtual TreeIter insertItem(object item)
		{
			if (typeof(INotifyPropertyChanged).IsAssignableFrom(item.GetType()))
			{
				INotifyPropertyChanged notifiable = (INotifyPropertyChanged) item;
				notifiable.PropertyChanged += OnItemChanged;
			}
			
			Logger.GetInstance().WriteLine("insert item: " + item);
			if (!ModelIsInitialized)
			{
				Logger.GetInstance().WriteLine("insert item, calling inializeTreeModelWidget");
				initializeTreeModelWidget(item);				
			}
			
			Logger.GetInstance().WriteLine("insert item:" + item);
			
			_itemValues.Clear();
			getItemValues(item);			
			    
			if (Model != null)
			{
				TreeIter iter = appendValueToStore(item, _itemValues);
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
		
		protected virtual void getItemPropertiesValues(object item)
		{
			Logger.GetInstance().WriteLine("item type properties initialized:" + item.ToString());
			List<string> properties = new List<string>();
			
			Logger.GetInstance().WriteLine("itemsTypeProperties:" + _itemsTypeProperties);
			foreach (PropertyInfo pInfo in _itemsTypeProperties)
            {
				Logger.GetInstance().WriteLine("Reading object property:" + pInfo.Name);
                if (!properties.Contains(pInfo.Name))
                {
					// if there is a binding descriptor, add only the properties described by it
					if (ItemsDisplayMode == ItemsDisplayMode.BindingDescriptor)
					{
						bool descriptorContainsProperty = false;
						foreach (T column in BindingDescriptor.BindingColumns)
						{
							Logger.GetInstance().WriteLine("binding column=" + column.Name + column.ObjectPropertyName);
							if ((column.ObjectPropertyName != null) && 
							    column.ObjectPropertyName.Equals(pInfo.Name))
							{
								Logger.GetInstance().WriteLine("descriptor contains property:" + pInfo.Name);
								descriptorContainsProperty = true;
								break;
							}
							else if (column.ObjectPropertyName == null) 
							{
								throw new NullReferenceException("The ObjectPropertyName of the column can not be null");
							}
						}
						if (!descriptorContainsProperty)
						{
							Logger.GetInstance().WriteLine("descriptor NOT:" + pInfo.Name);
							continue;
						}
					}
                    if (pInfo.GetGetMethod().GetParameters().Length > 0)
					{
						// TODO : indexed property
					}
					else
					{
						Logger.GetInstance().WriteLine("trying to read value");
						object val = pInfo.GetValue(item, null);
						Logger.GetInstance().WriteLine("reading value " + val);
						_itemValues.Add(pInfo.Name, val);
					}
					properties.Add(pInfo.Name);
               }
			}
		}
		
		protected void RenderObject (Gtk.TreeViewColumn column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			//System.Object obj = model.GetValue(column. (iter, 0);
			//(cell as Gtk.CelRendererText).objectext = model.GetValue(iter, 0).ToString();// .Data.ToString();
		}
	}
}
