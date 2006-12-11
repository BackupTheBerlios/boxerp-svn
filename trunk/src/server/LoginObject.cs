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

 	public class LoginObject : MarshalByRefObject, ILogin
	{
		private static SessionsManager sessionsMgr = SessionsManager.GetInstance();
		private static Random rand = new Random( );
		
		// Constructor
		public LoginObject ()
		{
		}
		///////////////////////////////////////////////////////////////////////////	
				/// <summary>
				/// 
				/// </summary>
		public int Login(string user, string password)
 		{
         User u = User.FindByUsernameAndPasswd(user, password);
			if (u != null) {
            lock(this)
            {
				    string session = sessionsMgr.GetSession(u);
				    UserInformation.SetSessionToken(session); // put the session token in the CallContext
				    return 0;
            }
			}
			else
				return -1;
		}
	}	
}
