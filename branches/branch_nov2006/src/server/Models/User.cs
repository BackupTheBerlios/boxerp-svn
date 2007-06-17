//
// Authors:
//    Hector Rojas González <hecrogon@gmail.com>
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
using NHibernate.Expression;
using Iesi.Collections;
using System.Collections;
using Castle.ActiveRecord;

namespace Boxerp.Models
{
	[ActiveRecord("susers")]
	[Serializable]
	public class User : ActiveRecordBase, IBoxerpModel
	{
		private int _id;
		private ISet _groups; 
		private string _userName;
		private string _realName;
		private string _email;
		private string _password;
		private bool _active;
		private IDictionary _permissions;
		static System.Security.Cryptography.MD5 hasher = System.Security.Cryptography.MD5.Create();

		[PrimaryKey(PrimaryKeyType.Native)]
		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		[HasAndBelongsToMany( typeof(Group), RelationType.Set,
			Table="users_groups",
			ColumnRef="group_id",  ColumnKey="user_id", Cascade=ManyRelationCascadeEnum.SaveUpdate)]
		public ISet Groups
		{
			get { return _groups; }
			set { _groups = value; }
		}
	
		[HasMany(typeof(SectionPermission), Index="sectionpermission" ,IndexType="string",
			Cascade=ManyRelationCascadeEnum.SaveUpdate)]
		public IDictionary Permissions 
		{
			get { return _permissions; }
			set { _permissions = value; }
		}
		
		[Property(Length=20, Unique=true)]
		public string UserName
		{
			get { return _userName; }
			set { _userName = value; }
   	}

		[Property(Length=50)]
		public string RealName
		{
			get { return _realName; }
			set { _realName = value; }
   	}

		[Property(Length=40)]
		public string Email
		{
			get { return _email; }
			set { _email = value; }
   	}

		[Property(Length=50)]
		public string Password
		{
			get { return _password; }
			set { _password = value; }
   	}

		[Property]
		public bool Active
		{
			get { return _active; }
			set { _active = value; }
		}

		public static User[] FindAll()
		{
			return (User[]) ActiveRecordBase.FindAll(typeof(User));
		}

		public static User Find(int id)
		{
			return (User) ActiveRecordBase.FindByPrimaryKey( typeof(User), id );
		}
		
		public static User FindByUsernameAndPasswd(string username, string passwd)
		{	
			string hashedPassword = passwd; // Hash(passwd);
			
			return (User) ActiveRecordBase.FindOne(typeof(User), 
					Expression.Eq("UserName", username), Expression.Eq("Password", hashedPassword));
		}

		private static string Hash(string value)
		{
			if (value.Length < 10)
			{
				byte[] valueBytes = System.Text.Encoding.UTF8.GetBytes(value);
				byte[] hashedBytes = hasher.ComputeHash(valueBytes);
				return System.Convert.ToBase64String(hashedBytes);
			}
			else
				return value;
		}

		public override string ToString()
		{
			return UserName;
		}

		public override bool Equals(Object obj )
		{
                    User user = obj as User;
                    if (user != null)
                    {
                        return (user.Id == Id);    
                    }
                    return false;
		}

                public override int GetHashCode()
                {
                    return Id;
                }

	}
}

