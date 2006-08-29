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
using System.Collections;
using Castle.ActiveRecord;

namespace Boxerp.Models
{
	[ActiveRecord("ctrldActions")]
	public class CtrldAction : ActiveRecordBase
	{
		private int _id;
		private string _name;
		private string _description;

		[PrimaryKey(PrimaryKeyType.Native)]
		public int Id
		{
			get { return _id; }
			set { _id = value; }
		}

		[Property(Length=30)]
		public string Name
		{
			get { return _name; }
			set { _name = value; }
   	}

		[Property(Length=50)]
		public string Description
		{
			get { return _description; }
			set { _description = value; }
   	}
	}
}

