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

		private void OnItemAdded(Object item, EventArgs args)
		{
			Combo.Items.Add(item);
		}

		
		private void OnItemRemoved(Object item, EventArgs args)
		{
			Combo.Items.Remove(item);
		}

		private void OnClear(Object sender, EventArgs args)
		{
			Combo.Items.Clear();
		}

		public T SelectedItem
		{
			get
			{
				return (T)Combo.SelectedItem;
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