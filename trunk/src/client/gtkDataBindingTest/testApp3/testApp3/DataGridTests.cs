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
using Boxerp.Client;
using Boxerp.Client.GtkSharp;
using Boxerp.Client.GtkSharp.Controls;

namespace testApp3
{
	
	
	public partial class DataGridTests : Gtk.Window
	{
		private DataGrid _dataGrid;
		private BindableWrapper<User> bindableUser2;
		
		public DataGridTests() : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			_dataGrid = new DataGrid();
			_dataGrid.ItemsDisplayMode = ItemsDisplayMode.BindingDescriptor;
			_dataGrid.BindingDescriptor = createUserBindingDescriptor();
			this.scrolledwindow1.AddWithViewport(_dataGrid);
			this.ReshowWithInitialSize();
			this.Child.ShowAll();
			addUsers();
		}
		
		private BindingDescriptor<EditableColumn> createUserBindingDescriptor()
		{
			BindingDescriptor<EditableColumn> bd = new BindingDescriptor<EditableColumn>();

			EditableColumn column = new EditableColumn();
			column.DataType = typeof(string);
			column.Name = "Username";
			column.ObjectPropertyName = column.Name;
			column.Editable = true;
			column.Widget = typeof(TextBox);
			column.Visible = true;
			
			EditableColumn column2 = new EditableColumn();
			column2.DataType = typeof(string);
			column2.Name = "Email";
			column2.ObjectPropertyName = column2.Name;
			column2.Editable = true;
			column2.Widget = typeof(TextBox);
			column2.Visible = true;
			
			EditableColumn column3 = new EditableColumn();
			column3.DataType = typeof(int);
			column3.Name = "Desk";
			column3.ObjectPropertyName = column3.Name;
			column3.Editable = true;
			column3.Widget = typeof(IntegerTextBox);
			column3.Visible = true;
				
			EditableColumn column4 = new EditableColumn();
			column4.DataType = typeof(bool);
			column4.Name = "Is Active";
			column4.ObjectPropertyName = "IsActive";
			column4.Editable = true;
			column4.Widget = typeof(CheckBox);
			column4.Visible = true;		
			
			bd.BindingColumns.Add(column);
			bd.BindingColumns.Add(column2);
			bd.BindingColumns.Add(column3);
			bd.BindingColumns.Add(column4);
			
			return bd;
		}
		
		private void addUsers()
		{
			User user1 = new User();
			user1.Username = "test3";
			user1.Desk = 540;
			user1.Email = "user1@user3.com";
			user1.Password = "unsafe3";
			
			User user2 = new User();
			user2.Username = "test4";
			user2.Desk = 143;
			user2.Email = "user2@user4.com";
			user2.Password = "unsafe4";
			bindableUser2 = new BindableWrapper<User>(user2);
			Logger.GetInstance().WriteLine("gonna insert:" + _dataGrid.Items);
			_dataGrid.Items.Add(user1);
			_dataGrid.Items.Add(bindableUser2.Data.BusinessObj);

		}

		protected virtual void OnShowItem (object sender, System.EventArgs e)
		{
			if (_dataGrid.SelectedItem != null)
			{
				object item = _dataGrid.SelectedItem;
				displayItem(item);
			}
		}
		
		private void displayItem(object item)
		{
			InfoDialog id = new InfoDialog();
			id.Message = item.ToString();
			User selectedUser = (User)item;
			string userProperties = ", " + selectedUser.Desk + ", " + selectedUser.Email  + ", "+ selectedUser.Password  + ", "+ selectedUser.Username;
			id.Message += userProperties;
		}
		
		protected virtual void OnDeleteItem (object sender, System.EventArgs e)
		{
			if (_dataGrid.SelectedItem != null)
			{
				_dataGrid.Items.Remove(_dataGrid.SelectedItem);
			}
		}

		protected virtual void OnAddItem (object sender, System.EventArgs e)
		{
			int i = _dataGrid.Items.Count;
			User user = new User();
			user.Username = "random" + DateTime.Now;
			user.Password = "asdfsdf";
			user.Desk = i;
			_dataGrid.Items.Add(user);
		}
		
		protected virtual void OnToggleSelectionMode (object sender, System.EventArgs e)
		{
			if (_dataGrid.SelectionMode == Gtk.SelectionMode.Single)
			{
				_dataGrid.SelectionMode = Gtk.SelectionMode.Multiple;
			}
			else
			{
				_dataGrid.SelectionMode = Gtk.SelectionMode.Single;
			}
		}
		
		protected virtual void OnToggleDisplayMode (object sender, System.EventArgs e)
		{
			if (_dataGrid.ItemsDisplayMode == ItemsDisplayMode.ObjectToString)
			{
				_dataGrid.ItemsDisplayMode = ItemsDisplayMode.AutoCreateColumns;
			}
			else
			{
				_dataGrid.ItemsDisplayMode = ItemsDisplayMode.ObjectToString;
			}
		}

		protected virtual void OnShowMultiple (object sender, System.EventArgs e)
		{
			if (_dataGrid.SelectedItems != null)
			{
				foreach (object item in _dataGrid.SelectedItems)
				{
					displayItem(item);
				}
			}
		}

		protected void OnDeleteEvent (object sender, Gtk.DeleteEventArgs a)
		{
			
		}

		protected virtual void OnMemoryTest (object sender, System.EventArgs e)
		{
			Logger.GetInstance().ShowDebugInfo = false;
			
			for (int i = 0; i < 10000; i++)
			{
				this.OnAddItem(sender, e);
			}
		}

		protected virtual void OnChangeUser (object sender, System.EventArgs e)
		{
			bindableUser2.Data.BusinessObj.Username = "changed from code";
		}

	}
}
