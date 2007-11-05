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
	public partial class DataGrid : TreeViewWrapper, IBindableWidget
	{
		private BindableWidgetCore _widgetCore;
		
		public DataGrid()
		{
			this.Build();
			_widgetCore = new BindableWidgetCore(this);
		}
		
		protected override Gtk.TreeView TreeView 
		{ 
			get
			{
				return _treeview;
			}
		}
		
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
			/*Logger.GetInstance().WriteLine("updateValue:" + property);
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
			 */
		}
			 
		protected override void createColumns ()
		{
			
		}
		
		protected override TreeIter insertItem (object item)
		{
			throw new NotImplementedException();
		}
		
		protected override void removeItemFromCurrentCollection (object item)
		{
			
		}



	}
}
