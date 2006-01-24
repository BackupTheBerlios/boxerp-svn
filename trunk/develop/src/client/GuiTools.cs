//
// GuiTools.cs
//
// Authors:
// 	Zebenzui Perez Ramos <zebenperez@shidix.com>
// 	Carlos Ble Jurado <carlosble@shidix.com>
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
using Boxerp.Debug;
using Boxerp.Errors;

namespace Boxerp.Tools
{
	static class GuiTools
	{
		///////////////////////////////////////////////////////////////
		
		public static void BuildComboBox(DataSet ds, ref ComboBox combob)
		{
			try
			{
				for (int i = 0; i <= combob.Model.IterNChildren(); i++)  // clear combo first
					combob.RemoveText(0);
				combob.RemoveText(0);
				DataRow[] rows = DataTools.GetRows(ds);
				for (int i = 0; i < rows.Length; i++)
				{
					combob.AppendText((string)rows[i][1]);
				}
			}
			catch (Exception ex)
			{
        throw ex;
			}
		}

		////////////////////////////////////////////////////////////////
		
		public static void BuildComboBoxEntry(DataSet ds, ref ComboBoxEntry comboent)
		{
			/*DataRow[] rows = DataTools.GetRows(ds);
			for (int i = 0; i < rows.Length; i++)
			{
				combob.PrependText((string)rows[i][0]);
			}*/
		}

		////////////////////////////////////////////////////////////////
	}
}

