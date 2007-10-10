//
// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
//
// Redistribution and use in source and binary forms, with or
// without modification, are permitted provided that the following
// conditions are met:
// Redistributions of source code must retain the above
// copyright notice, this list of conditions and the following
// disclaimer.
// Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following
// disclaimer in the documentation and/or other materials
// provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
// PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR
// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Boxerp.Collections;

namespace Boxerp.Client.WPF.Controls
{
	/// <summary>
	/// Interaction logic for GenericComboBox.xaml
	/// </summary>

	public partial class GenericComboBox : System.Windows.Controls.UserControl
	{
		public GenericComboBox()
		{
			InitializeComponent();
		}

		public ComboBox Combo
		{
			get
			{
				return _combo;
			}
		}
	}

	public class GenericComboBox<T> : GenericComboBox
	{
		private bool _sortItems = false;
		private InterceptedList<T> _items = new InterceptedList<T>();

		public event SelectionChangedEventHandler SelectionChanged
		{
			add
			{
				Combo.SelectionChanged += value;
			}

			remove
			{
				Combo.SelectionChanged -= value;
			}
		}

		public GenericComboBox() 
		{
			_items.ItemAddedEvent += OnItemAdded;
			_items.ItemRemovedEvent += OnItemRemoved;
			_items.ClearEvent += OnClear;
		}

		public GenericComboBox(bool sortItems)
			: this ()
		{
			_sortItems = sortItems;
		}

		private void OnItemAdded(Object item, EventArgs args)
		{
			if ((_sortItems) && (Combo.Items.Count > 0))
			{
				bool inserted = false;
				for (int i = 0; i < Combo.Items.Count; i++)
				{
					if (Combo.Items[i].ToString().CompareTo(item.ToString()) > 0)
					{
						Combo.Items.Insert(i, item);
						inserted = true;
						break;
					}
				}
				if (!inserted)
				{
					Combo.Items.Insert(Combo.Items.Count, item);
				}
			}
			else
			{
				Combo.Items.Add(item);
			}
		}

		
		private void OnItemRemoved(Object item, EventArgs args)
		{
			Combo.Items.Remove(item);
		}

		private void OnClear(Object sender, EventArgs args)
		{
			Combo.Items.Clear();
		}

		public bool IsItemSelected
		{
			get
			{
				return Combo.SelectedItem != null;
			}
		}

		public T SelectedItem
		{
			get
			{
				return (T)Combo.SelectedItem;
			}
			set
			{
				Combo.SelectedItem = value;
			}
		}

		public int SelectedIndex
		{
			get
			{
				return Combo.SelectedIndex;
			}
			set
			{
				Combo.SelectedIndex = value;
			}
		}

		public InterceptedList<T> Items
		{
			get
			{
				return _items;
			}
			set
			{
				Combo.Items.Clear();
				foreach (T item in value)
				{
					Combo.Items.Add(item);
				}
			}
		}

		public void AdjustDimensions()
		{
			if ((Parent != null) && (Parent is Panel))
			{
				Width = ((Panel)Parent).Width;
				Height = ((Panel)Parent).Height;
			}
		}
	}
}