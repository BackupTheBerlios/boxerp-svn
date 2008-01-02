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
using System.Collections.Generic;

namespace mvcSample1WinForms
{
	public static class Database
	{		
		public static List<User> GetUsers(Group group)
		{
			User u1 = new User();
			u1.Group = group;
			u1.Username = "Allan";
			u1.Desk = 1;
			u1.Email = "admin@testing.com";
			u1.IsActive = true;

			User u2 = new User();
			u2.Group = group;
			u2.Username = "Dave";
			u2.Desk = 2;
			u2.Email = "dave@testing.com";
			u2.IsActive = false;

			User u3 = new User();
			u3.Group = group;
			u3.Username = "Carlos";
			u3.Desk = 3;
			u3.Email = "carlos@testing.com";
			u3.IsActive = true;

			User u4 = new User();
			u4.Group = group;
			u4.Username = "Jeremy";
			u4.Desk = 4;
			u4.Email = "jwormy@testing.com";
			u4.IsActive = false;

			List<User> users = new List<User>();
			if (group.Id == 1)
			{
				users.Add(u1);
				users.Add(u2);
			}
			else
			{
				users.Add(u3);
				users.Add(u4);
			}
			
			return users;
		}
		
		public static List<Group> GetAllGroups()
		{
			Group g1 = new Group();
			g1.Id = 1;
			g1.Name = "Admins";
			
			Group g2 = new Group();
			g2.Id = 2;
			g2.Name = "Developers";
			
			List<Group> groups = new List<Group>();
			groups.Add(g1);
			groups.Add(g2);
			
			return groups;
		}
	}
}
