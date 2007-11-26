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
namespace mvcSample1
{
	public class TestUsersListView : AbstractTestView<UsersListController, TestUsersListData, IUsersListData>, IUsersListView
	{
		
		protected override void CreateData()
		{
			_data = new TestUsersListData(this);
		}
		
		public IUsersListData SharedData 
		{	
			get 
			{
				return Data;
			}
		}

		public void UpdateUsers()
		{}

		public void UpdateGroups()
		{}
		
		public void OnSelectionChanged ()
		{
			throw new NotImplementedException();
		}

		public void OnDeleteUser ()
		{
			throw new NotImplementedException();
		}

		public void OnEditUser ()
		{
			throw new NotImplementedException();
		}

		public void OnAddUser ()
		{
			throw new NotImplementedException();
		}

		public IUserEditView GetUserEditView ()
		{
			throw new NotImplementedException();
		}

		public void DisplayView (IUserEditView view)
		{
			throw new NotImplementedException();
		}
	}
}