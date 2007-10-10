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
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;

namespace Boxerp.Client.GtkSharp.Controls
{
	
	
	public partial class ComboBox : Gtk.Bin, IBindableWidget
	{
		private BindableWidgetCore _widgetCore;
		private object _selectedItem = null;
		private List<object> _items = new List<object>();
		
		public ComboBox()
		{
			this.Build();
			_widgetCore = new BindableWidgetCore(this);
		}

		public BindableWidgetCore WidgetCore
		{
			get
			{
				return _widgetCore;
			}
		}

		public object SelectedItem 
		{
			get 
			{
				return _selectedItem;
			}
		}

		public List<object> Items 
		{
			get 
			{
				return _items;
			}
		}
		
		public void UpdateValue(string property, object val)
		{
			_selectedItem = val;
			// update the combo without calling again the SetPropertyValue
		}

		protected virtual void OnComboChanged (object sender, System.EventArgs e)
		{
			// Get the selected item 
			//_selectedItem = _combo.Model.
			WidgetCore.SetPropertyValue(_selectedItem);
		}		
				
				
				
				
				
	}
}
