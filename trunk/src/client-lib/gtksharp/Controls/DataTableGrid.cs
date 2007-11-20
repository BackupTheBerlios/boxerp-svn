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
using System.Data;

namespace Boxerp.Client.GtkSharp.Controls
{	
	public class DataTableGrid : DataGrid
	{
		private DataTable _dataTable = null;
		
		public override ItemsDisplayMode ItemsDisplayMode 
		{
			get 
			{   
				return ItemsDisplayMode.BindingDescriptor;
			}
			set 
			{  
				throw new NotSupportedException("It is not possible to change the display mode of this widget");
			}
		}

		public DataTableGrid()
		{
		}
		
		public void BindDataTable(DataTable table)
		{
			_dataTable = table;
			createBindingDescriptorFromTable();
			ModelIsInitialized = false;
			foreach (DataRow row in _dataTable.Rows)
			{
				this.insertItem(row);
			}
		}
		
		public void UnbindDataTable()
		{
			_dataTable = null;
			BindingDescriptor = null;
		}
		
		private void createBindingDescriptorFromTable()
		{
			BindingDescriptor = new BindingDescriptor<EditableColumn>();
			
			foreach (DataColumn dColumn in _dataTable.Columns)
			{
				Logger.GetInstance().WriteLine("dataTableGrid reading column:" + dColumn.ColumnName);
				EditableColumn editColumn = new EditableColumn();
				editColumn.Editable = true;
				editColumn.DataType = dColumn.DataType;
				editColumn.Name = dColumn.ColumnName;
				editColumn.ObjectPropertyName = dColumn.ColumnName;
				editColumn.Visible = true;
				if (typeof(int).IsAssignableFrom(dColumn.DataType))
				{
					editColumn.Widget = typeof(IntegerTextBox);
				}
				else if (typeof(bool).IsAssignableFrom(dColumn.DataType))
				{
					editColumn.Widget = typeof(CheckBox);
				}
				else
				{
					editColumn.Widget = typeof(TextBox);
				}
				BindingDescriptor.BindingColumns.Add(editColumn);
			}
		}

		protected override void getItemPropertiesValues(object item)
		{
			DataRow row = item as DataRow;
			foreach (DataColumn column in _dataTable.Columns)
			{
				_itemValues.Add(column.ColumnName, row[column]);
			}
		}
		
		protected override void updateItem(object item, object val, int colNumber, string propertyName)
		{
			DataRow row = item as DataRow;
			row[colNumber] = val;
		}
	}
}
