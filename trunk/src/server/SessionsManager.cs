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
using Boxerp.Errors;
using Boxerp.Debug;
using Boxerp.Exceptions;
using Boxerp.Models;
using Castle.ActiveRecord;
using NHibernate;


namespace Boxerp.Objects
{

	public struct SessionStruct
	{
		public User user;
		public DateTime lastHit;
	}
	
 	public class SessionsManager 
	{
		private static SessionsManager instance = null;
		private static Hashtable sHash = Hashtable.Synchronized(new Hashtable());
		private static Random random = new Random( );
		private static double EXPIRE = 10;
		private SessionsManager(){}

		public static SessionsManager GetInstance()
		{
			if (instance == null)
			{
				instance = new SessionsManager();
		   }
			return instance;
		}

		public string GetSession(User u)
		{
			if (u != null)
			{
				string session = u.UserName + DateTime.Now.GetHashCode() + random.NextDouble().ToString();
				SessionStruct sstruct = new SessionStruct();
				sstruct.user = u;
				sstruct.lastHit = DateTime.Now;
				Console.WriteLine("new session=" + session);
				sHash[session] = sstruct;
				return session;
			}
			else
			{
				throw new NullReferenceException();
			}
		}

		// FIXME: Should I lock or Synchronized hashtable is enough?
		public bool IsValidSession(string session)
		{
			if (sHash.ContainsKey(session))
			{
				SessionStruct sstruct = (SessionStruct)sHash[session];
				DateTime dt = sstruct.lastHit;
				dt = dt.AddMinutes(EXPIRE);			
				if (DateTime.Compare(DateTime.Now, dt) <= 0)
					return true;			
				else
					return false;		// expire
			}
			else
				return false;
		}

		public bool IsValidSessionThenUpdate(string session)
		{
			if (sHash.ContainsKey(session))
			{
				SessionStruct sstruct = (SessionStruct)sHash[session];
				DateTime dt = sstruct.lastHit;
				dt = dt.AddMinutes(EXPIRE);			// 
				if (DateTime.Compare(DateTime.Now, dt) < 0)
				{
					sstruct.lastHit = DateTime.Now;	// FIXME: may I destroy the previous DateTime first?
					sHash[session] = sstruct;
					return true;			
				}
				else
					return false;		// expire
			}
			else
				return false;
		}


		
		// Just for test:
		/*
		public string GetAllSessions()
		{
			string result = "";
			foreach(Manager i in sHash.Keys)
			{
				result += (string)i + ",";
			}
			return result;
		}*/

		
	}
}
