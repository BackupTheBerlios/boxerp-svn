//
//
// Authors:
// 	Carlos Ble Jurado <carlosble@shidix.com>
//
// Copyright (C) 2005,2006 Carlos Ble 
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
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
using Boxerp.Models;
using Castle.ActiveRecord;
using NHibernate;

namespace Boxerp.Objects
{

 	public interface IAdmin
	{
		User GetUser(string user, string password);
		User[] GetUsers();
		Enterprise[] GetEnterprises();
		Group[] GetGroups();
		Group[] GetDistinctGroups(User user);
		int SaveUser(User user);
                int DeleteUser(User user);
		/*IList GetEnterprises(string session);
		IList GetGroups(string session);
		IList GetUsers(string session, 
		void CreateEnterprise(string session, Enterprise enterprise);
		void CreateGroup(string session, Group group);
		void CreateUser(string session, User user);
		void SaveGroup(string session, Group group);
		void SaveUser(string session, User user);
		void AddGroupToEnterprise(Group group, Enterprise enterprise);
		void AddUserToGroup(User user, Group group);
		void RemoveGroupFromEnterprise(Group group, Enterprise enterprise);
		void RemoveUserFromGroup(User user, Group group);
		void DeleteEnterprise(Enterprise enterprise);
		void DeleteGroup(Group group);
		void DeleteUser(User user);
		void 
		
		2. List Groups
				  3. List Users
				  4. Create Enterprise
				  5. Create Group
				  6. Create User
				  7. Set Group Permissions
				  8. Set User Permissions
				  9. Add a Group to an Enterprise
				  10. Add a User to a Group
				  11. Substract a Group from an Enterprise
				  12. Substract a User from a Group
				  13. Delete Enterprise
				  14. Delete Group
				  15. Delete User
				  16. Search Enterprise
				  17. Search Group
				  18. Search User

		*/		  
	}	
}
