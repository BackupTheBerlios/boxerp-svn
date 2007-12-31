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

namespace Boxerp.Client.GtkSharp
{
	/// <summary>
	/// Contains the parameters required for a column in a ListView or any other TreeView based widget 
	/// </summary>
	public class SimpleColumn
	{
		private string _name;
		private string _objectPropertyName;
		private bool _visible;
		private Type _dataType;
		private int _order;
		
		public string Name 
		{
			get 
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public string ObjectPropertyName 
		{
			get 
			{
				return _objectPropertyName;
			}
			set
			{
				_objectPropertyName = value;
			}
		}
		
		public bool Visible 
		{
			get 
			{
				return _visible;
			}
			set
			{
				_visible = value;
			}
		}

		public System.Type DataType 
		{
			get 
			{
				return _dataType;
			}
			set
			{
				_dataType = value;
			}
		}
	}
	
	/// <summary>
	/// Contains the parameters required for an editable column in a ListView or any other TreeView based widget.
	/// It allows the user specify what widget should be used in the cell. So you may 
	/// place a CheckBox, a ComboBox, or any other widget apart from the default Gtk.Entry 
	/// </summary>
	public class EditableColumn : SimpleColumn
	{
		private bool _editable;
		private Type _widget = typeof(Gtk.Entry);
		
		
		public bool Editable 
		{
			get 
			{
				return _editable;
			}
			set
			{
				_editable = value;
			}
		}

		public System.Type Widget 
		{
			get 
			{
				return _widget;
			}
			set
			{
				_widget = value;
			}
		}
		
		public EditableColumn()
		{
		}
	}
}
