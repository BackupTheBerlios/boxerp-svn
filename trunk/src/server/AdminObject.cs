//
// LoginObject.cs
//
// Authors:
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
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
using Boxerp.Errors;
using Boxerp.Debug;
using Boxerp.Exceptions;
using Boxerp.Models;
using Castle.ActiveRecord;
using NHibernate;

namespace Boxerp.Objects
{

 	public class AdminObject : MarshalByRefObject, IAdmin
	{
		private static SessionsManager sessionsMgr = SessionsManager.GetInstance();
		
		// Constructor
		public AdminObject ()
		{
		}

		public User[] GetUsers()
		{
			try
			{
				Console.WriteLine("user session=" + UserInformation.GetSessionToken());
				if (sessionsMgr.IsValidSessionThenUpdate(UserInformation.GetSessionToken()))
				{
					Console.WriteLine("valid user");
					return User.FindAll();
				}
				else
				{
					Console.WriteLine("not valid user");
					return null;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR:" + ex.Message + ":" + ex.StackTrace);
				return null;
			}
		}

		public Group[] GetGroups()
		{
			try
			{
				if (sessionsMgr.IsValidSessionThenUpdate(UserInformation.GetSessionToken()))
				{
					return Group.FindAll();
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR:" + ex.Message + ":" + ex.StackTrace);
				return null;
			}
		}

		public Enterprise[] GetEnterprises()
		{
			try
			{
				if (sessionsMgr.IsValidSessionThenUpdate(UserInformation.GetSessionToken()))
				{
					return Enterprise.FindAll();
				}
				else
				{
					return null;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("ERROR:" + ex.Message + ":" + ex.StackTrace);
				return null;
			}
		}
		
		public User GetUser(string username, string password)
		{
			/*Console.WriteLine("User information=" + UserInformation.GetUser());
			Console.WriteLine("Haciendo login= " + username +"," + password);
         User u = User.FindByUsernameAndPasswd(username, password);
			return u;	*/
			return null;
		}

		public void SaveUser(User u)
		{
			if (u.UserName.Length > 0)
				u.Save();
			Console.WriteLine("User:"+ u.UserName+" saved");
		}
	}	
}
