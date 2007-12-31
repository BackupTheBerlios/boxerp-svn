//
// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
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
//

using System;
using Boxerp.Client;

namespace Boxerp.Client.GtkSharp
{
	/// <summary>
	/// Alternative to Windows.Forms MessageBox with two buttons.
	/// 
	/// Usage: Create an instance, populate the Msg property and call Show().
	/// </summary>
	public partial class QuestionDialog : Gtk.Dialog, IQuestionWindow
	{
		private bool answer;
		
		public QuestionDialog()
		{
			this.Build();
		}
		
		public string Msg 
		{
			get { return label.Text; }
			set { label.Text = value; } 
		}
		
		public bool IsAfirmative
		{
		    get { return answer; }
			set { answer = value; }
		}

		public string AfirmativeOption 
		{
			get 
			{
				return String.Empty;
			}
			set 
			{
			}
		}

		public string NegativeOption 
		{
			get 
			{
				return String.Empty;
			}
			set 
			{
			}
		}
		
		protected virtual void OnCancel(object sender, System.EventArgs e)
		{
		    answer = false;
		    this.Hide();
		}
		
		protected virtual void OnOk(object sender, System.EventArgs e)
		{
		    answer = true;
		    this.Hide();		    
		}

		public void ShowDialog ()
		{
			this.Run();
		}

		public new void Close ()
		{
			this.Destroy();
		}
	}
}
