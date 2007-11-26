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
using System.Collections;
using System.Collections.Generic;

namespace mvcSample1
{
	public abstract class AbstractUsersListData<T> : AbstractData<IUsersListView, UsersListController, T>, IUsersListData
		where T : IUsersListView
	{
		private List<Group> _groups = new List<Group>();
		private List<User> _users = new List<User>();
		
		public System.Collections.Generic.List<Group> Groups 
		{
			get { return _groups; }
			set { _groups = value; }
		}

		public System.Collections.Generic.List<User> Users 
		{
			get { return _users;}
			set { _users = value;}
		}
		
		public AbstractUsersListData(T view)
			: base(view)
		{
			
		}
	}
}
