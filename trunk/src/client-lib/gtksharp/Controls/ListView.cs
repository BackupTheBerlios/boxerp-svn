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

	// TODO: strongly typed listview
	/// <summary>
	/// Use cases: 
	///  1 - The user binds a collection: clean everything and initialize. If Items and BoundItems are used both, throw exception
	///  2 - The user is using the Items properties. The items on it implement the INotifyPropertyChanged. What happen when an item changes? 
	/// 
	/// </summary>
	public class ListView : TreeViewWrapper<SimpleColumn>, IBindableWidget
	{
		public ListView()
			: base()
		{
		}
		
		protected override void addTreeViewColumn(SimpleColumn column, int colNumber)
		{
	        if (column.DataType == typeof(Gdk.Pixbuf))
        	{
	        	TreeViewColumn tc = TreeView.AppendColumn ("", new CellRendererPixbuf (), "pixbuf", colNumber);
		    	if (tc != null)
				{
			    	tc.Visible = column.Visible;
				}
			}
			else if (column.DataType == typeof(System.Object)) 
			{
				Gtk.TreeViewColumn objColumn = new Gtk.TreeViewColumn ();
				objColumn.Title = column.Name;
				Gtk.CellRendererText objCell = new Gtk.CellRendererText ();
				objColumn.PackStart (objCell, true);
				objColumn.SetCellDataFunc (objCell, new Gtk.TreeCellDataFunc (RenderObject));		
				TreeView.AppendColumn(objColumn);
			}
        	else
        	{
        		TreeViewColumn tc = 
					TreeView.AppendColumn (column.Name, new CellRendererText (), "text", colNumber);
				Logger.GetInstance().WriteLine("appending column:" + column.Name + colNumber);
				
				if (tc != null)
				{
		    		tc.Visible = column.Visible;
				}
        	}
		}
	}
}
