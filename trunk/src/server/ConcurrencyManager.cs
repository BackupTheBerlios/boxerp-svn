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

	public enum Sections
	{
		ADMIN
	}

	public enum Actions
	{
		SHOW,
		EDIT,
		DELETE
	}
	
	public struct ConcurrencyStruct
	{
		public Sections section;
		public Actions action;
		public User user;
		public DateTime lastHit;
	}
	
 	public class ConcurrencyManager //: MarshalByRefManager
	{
		private static ConcurrencyManager instance = null;
		private static Hashtable cHash = Hashtable.Synchronized(new Hashtable());
		private static double EXPIRE = 10;
		private ConcurrencyManager(){}

		public static ConcurrencyManager GetInstance()
		{
			if (instance == null)
			{
				instance = new ConcurrencyManager();
		   }
			return instance;
		}

		public bool StartAction(User u, Sections s, Actions a)
		{
			if (u != null)
			{
				if (!cHash.ContainsKey(s.ToString()+a.ToString()))
				{
					ConcurrencyStruct cstruct = new ConcurrencyStruct();
					cstruct.user = u;
					cstruct.lastHit = DateTime.Now;
					cstruct.section = s;
					cstruct.action = a;
					cHash[s.ToString()+a.ToString()] = cstruct;
					return true;
				}
				else
				{
					if (s == Sections.ADMIN)
					{
						return false;	// another user is inside administrator
					}
					return true;
				}
			}
			else
			{
				throw new NullReferenceException();
			}
		}

		public bool EndAction(User u, Sections s, Actions a)
		{
			if (u != null)
			{
				if (cHash.ContainsKey(s.ToString()+a.ToString()))
				{
					cHash.Remove(s.ToString()+a.ToString());
					return true;
				}
				else
				{
					return false;	// something is wrong
				}
			}
			else
			{
				throw new NullReferenceException();
			}
		}

	}
}
