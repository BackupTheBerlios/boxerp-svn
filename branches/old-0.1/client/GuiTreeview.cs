//
// GuiTreeview.cs
//
// Authors:
// 	Zebenzui Perez Ramos <zebenperez@shidix.com>
//
// Copyright (C) 2005,2006 Shidix Technologies (www.shidix.com)
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
using System.Data;
using System.Xml;
using System.Collections;
using Gtk;
using Glade;

namespace Boxerp.Client
{
    public struct Column
    {
        public string name;
        public string type;
        public bool   view;
    }
        
	class GuiTreeview
	{
		TreeStore store;
		TreeView treeview;

		public GuiTreeview ()
		{
		}

		public TreeStore Store
		{
			get {return store;}
			set {store = value;}
		}

		public TreeView Treeview
		{
			get {return treeview;}
			set {treeview = value;}
		}

		public void NewTreestore (ArrayList columns)
		{
			System.Type[] columnsTypes = new Type[columns.Count];
			int i = 0;

			foreach (Column col in columns)
            {
				switch(col.type)
		        {
			        case "string":
			            columnsTypes[i++] = typeof(string);
		                break;
		            case "int":
		                columnsTypes[i++] = typeof(int);
		                break;
		            case "float":
		                columnsTypes[i++] = typeof(float);
		                break;
                    case "icon":    
		                columnsTypes[i++] = typeof(Gdk.Pixbuf);
		                break;
		        }
            }    

			store = new TreeStore(columnsTypes);
		}

		///<summary>
		/// Create a new treeview. The main treeview is a referenced treeview
		///</summary>
		public void NewTreeView (ArrayList columns, ref TreeView tv)
		{
			NewTreestore(columns);
			tv.Model = store;

			int i = 0;
			foreach (Column col in columns)
			{
                if (col.type == "icon")
                {
			        TreeViewColumn column = tv.AppendColumn ("", new CellRendererPixbuf (), "pixbuf", i++);
				    column.Visible = col.view;
                }
                else
                {
				    TreeViewColumn column = tv.AppendColumn (col.name, new CellRendererText (), "text", i++);
				    column.Visible = col.view;
                }
			}
		}

		public TreeIter InsertRow (TreeIter parent, IDictionary row)
		{
			TreeIter myIter;
			// If parent is not the root of the tree 
			if (!parent.Equals(TreeIter.Zero))
				myIter = store.AppendNode(parent);
			else
				myIter = store.AppendNode();

			ICollection keys = row.Keys;
			foreach (int key in keys)
				store.SetValue (myIter, key, row[key]);

			return myIter;
		}

		public TreeIter InsertRow (TreeIter parent, string[] row)
		{
			TreeIter myIter;
			// If parent is not the root of the tree 
			if (!parent.Equals(TreeIter.Zero))
				myIter = store.AppendNode(parent);
			else
				myIter = store.AppendNode();

            int i = 0;
			foreach (string val in row)
				store.SetValue (myIter, i++, val);

			return myIter;
		}

		public TreeIter InsertRow (TreeIter parent, ArrayList row)
		{
			TreeIter myIter;
			// If parent is not the root of the tree 
			if (!parent.Equals(TreeIter.Zero))
				myIter = store.AppendNode(parent);
			else
				myIter = store.AppendNode();

            for (int i = 0; i < row.Count; i++)
				store.SetValue (myIter, i, row[i]);

			return myIter;
		}

		/////////////////////////////////////////////////////////////////////////////////
		///<summary>
		///Build a tree with xml nodes
		///</summary>
		///<param name="node">xml node</param>
		///<param name="parent">row parent</param>
		public void BuildTree (XmlNode node, TreeIter parent)
		{
			if (!node.HasChildNodes)
				return;
			else 
			{
				XmlNodeList childNodeList = node.ChildNodes;
				foreach (XmlNode childNode in childNodeList) 
				{
					if (childNode.Name == "node")
					{
						// Creamos el diccionario con los valores de las columnas. 
						IDictionary colValues = new Hashtable();

						XmlNodeList childs = childNode.ChildNodes;
						int i = 0;
					    foreach (XmlNode child in childs)
						    if (child.Name == "column")
								colValues[i++] = child.Attributes["value"].Value;


						TreeIter newParent = InsertRow (parent, colValues);
						BuildTree (childNode, newParent);
					}
				}
			}
		} // build_tree

		public static void RemoveColumns (TreeView tv)
		{
			TreeViewColumn[] columns = tv.Columns;
			foreach (TreeViewColumn column in columns)
				tv.RemoveColumn (column);
		} // RemoveColumns

        public void StoreDataSet (DataSet ds, ref TreeView tv)
        {
            DataTable table = ds.Tables[0];
            ArrayList dataRow = new ArrayList();
            ArrayList columns = new ArrayList();
            Column col; 
            int i;

            RemoveColumns(tv);
            
            for (i = 0; i < table.Columns.Count; i++)
            {
                col.name = table.Columns[i].ToString();
                col.type = "string";
                col.view = true;
                columns.Add(col);
            }
            NewTreeView(columns, ref tv);

            // Recorrer el dataset y rellenar el treestore
            for (i = 0; i < table.Rows.Count; i++)
            {
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    //Console.Write ("-{0}-", table.Rows[i][j].ToString());
                    dataRow.Add(table.Rows[i][j].ToString());
                }
                InsertRow (TreeIter.Zero, dataRow);
								dataRow.Clear();
            }
        }

        public void StoreDataSet (DataSet ds, XmlNode xmlNode, ref TreeView tv)
        {
            DataTable table = ds.Tables[0];
            ArrayList dataRow = new ArrayList();
            ArrayList columns = new ArrayList();
            Column col; 
            int i = 0;

            RemoveColumns(tv);

			XmlNodeList childs = xmlNode.ChildNodes;
			foreach (XmlNode node in childs)
			{
                col.name = node.Attributes["name"].Value;
                col.type = node.Attributes["type"].Value;
                col.view = bool.Parse(node.Attributes["view"].Value);
                columns.Add(col);
            }    
            NewTreeView(columns, ref tv);

            // Recorrer el dataset y rellenar el treestore
            for (i = 0; i < table.Rows.Count; i++)
            {
			    foreach (XmlNode node in childs)
                {
                    if (table.Columns[node.Attributes["field"].Value].ToString() == "icon_path")
                        dataRow.Add(new Gdk.Pixbuf(Boxerp.Defines.GUI_DIR + 
										table.Rows[i][node.Attributes["field"].Value].ToString()));
                    else    
                        dataRow.Add(table.Rows[i][node.Attributes["field"].Value].ToString());
                }
                InsertRow (TreeIter.Zero, dataRow);
            }
        }

	} //class GuiTreeview
} // namespace
