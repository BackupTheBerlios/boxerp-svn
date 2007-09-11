////
//// Copyright (c) 2007, Boxerp Project (www.boxerp.org)
////
//// Redistribution and use in source and binary forms, with or
//// without modification, are permitted provided that the following
//// conditions are met:
//// Redistributions of source code must retain the above
//// copyright notice, this list of conditions and the following
//// disclaimer.
//// Redistributions in binary form must reproduce the above
//// copyright notice, this list of conditions and the following
//// disclaimer in the documentation and/or other materials
//// provided with the distribution.
////
//// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY
//// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
//// PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR
//// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
//// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
//// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
//// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
//// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
//// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
//// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
//// THE POSSIBILITY OF SUCH DAMAGE.
//
//
using System;
using Gtk;
using testApp3;
using Boxerp.Client.GtkSharp.Controls;
using Boxerp.Client.GtkSharp;
using Boxerp.Client;

public partial class MainWindow: Gtk.Window
{	
	private User _user;
	private BindableWrapper<User> _bindable;
	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		_user = new User();
		
		_bindable = new BindableWrapper<User>(_user, false);
		_bindable.Data.BusinessObj.Username = "Paco";
		
		
		//XamlParser parser = new XamlParser("MainWindow.xaml");
		
		DataBinder binder = new DataBinder(this, _bindable);
		binder.Bind();
		
	}
	
	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected virtual void OnChangeClicked (object sender, System.EventArgs e)
	{
		_bindable.Data.BusinessObj.Username = "changing data";
		_bindable.Data.BusinessObj.Password = "from source code";
		_bindable.Data.BusinessObj.Email = "into the business object wrapper";
		_bindable.Data.BusinessObj.Desk = 7000;
	}

	protected virtual void OnShowClicked (object sender, System.EventArgs e)
	{
		Console.WriteLine(_bindable.Data.BusinessObj.Username);
		Console.WriteLine(_bindable.Data.BusinessObj.Password);
		Console.WriteLine(_bindable.Data.BusinessObj.Email);
		Console.WriteLine(_bindable.Data.BusinessObj.Desk);
	}

	protected virtual void OnRedoClicked (object sender, System.EventArgs e)
	{
		_bindable.Redo();
	}

	protected virtual void OnUndoClicked (object sender, System.EventArgs e)
	{
		_bindable.Undo();
	}
}