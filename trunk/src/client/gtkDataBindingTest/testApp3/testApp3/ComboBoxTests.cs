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
using Boxerp.Client.GtkSharp;
using Boxerp.Client;

namespace testApp3
{	
	public partial class ComboBoxTests : Gtk.Window
	{
		private Boxerp.Client.GtkSharp.Controls.ComboBox _combo = new Boxerp.Client.GtkSharp.Controls.ComboBox();
		private BindableWrapper<User> bindableUser2;
		
		
		public ComboBoxTests() : 
				base(Gtk.WindowType.Toplevel)
		{
			this.Build();
			
			this.vbox1.PackStart(_combo);
			addUsers();
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
			_combo.Items.Add(user1);
			_combo.Items.Add(bindableUser2.Data.BusinessObj);
		}

		protected virtual void OnDelete (object sender, System.EventArgs e)
		{
		}

		protected virtual void OnAdd (object sender, System.EventArgs e)
		{
		}

		protected virtual void OnEdit (object sender, System.EventArgs e)
		{
		}

		protected virtual void OnShow (object sender, System.EventArgs e)
		{
		}
	}
}
