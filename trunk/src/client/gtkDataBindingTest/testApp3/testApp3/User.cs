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
using System.Collections.Generic;
using System.Text;

namespace testApp3
{
	[Serializable]
	public class User 
	{
		private string _username;
		private string _password;
		private string _email;
		private int    _desk;
		
		public virtual int Desk 
		{
			get 
			{
				return _desk;
			}
			set
			{
				_desk = value;
			}
		}

		public virtual string Email 
		{
			get 
			{
				return _email;
			}
			set
			{
				_email = value;
			}
		}

		public virtual string Password 
		{
			get 
			{
				return _password;
			}
			set
			{
				_password = value;
			}
		}

		public virtual string Username 
		{
			get 
			{
				return _username;
			}
			set
			{
				_username = value;
			}
		}
			
		public User()
		{
		}
		
		private User(string username, string password, string email, int desk)
		{
			_username = username;
			_password = password;
			_email = email;
			_desk = desk;
		}		
	}
}
