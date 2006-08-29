//
// ServerExceptions.cs
//
// Authors:
// 	Carlos Ble Jurado <carlosble@shidix.com>
// 	Zebenzui Perez Ramos <zebenperez@shidix.com>
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
using System.Runtime.Serialization;
using Mono.Unix;

namespace Boxerp.Exceptions
{
			///<summary>
			///Exeption to raise when trying to get inexistent user
			///</summary>
	[Serializable]
	public class NullDataSetException : ApplicationException
	{
		public NullDataSetException()
			: base ("DataSet is empty. Query is empty") { }

		public NullDataSetException(string Msg)
			: base (Msg) { }

		public NullDataSetException(string Msg, System.Exception e)
			: base (Msg, e) {	}
	
		protected NullDataSetException (SerializationInfo info, StreamingContext context)
			: base (info, context) { }
	}
	///////////////////////////////////////////////////////////////
			///<summary>
			///Exeption to raise when trying to get inexistent user
			///</summary>
	[Serializable]
	public class NullUserException : ApplicationException
	{
		public NullUserException()
			: base ("User doesnt exist") { }

		public NullUserException(string Msg)
			: base (Msg) { }

		public NullUserException(string Msg, System.Exception e)
			: base (Msg, e) {	}
	
		protected NullUserException (SerializationInfo info, StreamingContext context)
			: base (info, context) { }
	}
	///////////////////////////////////////////////////////////////
			///<summary>
			///Exeption to raise when trying to insert a user that already exist
			///</summary>
	[Serializable]
	public class UserAlreadyExistException : ApplicationException
	{
		public UserAlreadyExistException()
			: base ("User already exist")	{	}

		public UserAlreadyExistException(string Msg)
			: base (Msg)	{	}

		public UserAlreadyExistException(string Msg, System.Exception e)
			: base (Msg, e)	{	}
		
		protected UserAlreadyExistException (SerializationInfo info, StreamingContext context)
			: base (info, context) { }
	}
	/////////////////////////////////////////////////////////////////
			///<summary>
			///Exeption to raise when trying to get inexistent group
			///</summary>
	[Serializable]
	public class NullGroupException : ApplicationException
	{
		public NullGroupException()
			: base ("Group doesnt exist")	{	}

		public NullGroupException(string Msg)
			: base (Msg) { }

		public NullGroupException(string Msg, System.Exception e)
			: base (Msg, e)	{	}
		
		protected NullGroupException (SerializationInfo info, StreamingContext context)
			: base (info, context) { }
	}
	///////////////////////////////////////////////////////////////
			///<summary>
			///Exeption to raise when trying to insert a group that already exist
			///</summary>
	[Serializable]
	public class GroupAlreadyExistException : ApplicationException
	{
		public GroupAlreadyExistException()
			: base ("Group already exist") { }

		public GroupAlreadyExistException(string Msg)
			: base (Msg) { }

		public GroupAlreadyExistException(string Msg, System.Exception e)
			: base (Msg, e)	{	}

		protected GroupAlreadyExistException (SerializationInfo info, StreamingContext context)
			: base (info, context) { }
	}
	///////////////////////////////////////////////////////////////
			///<summary>
			///Exeption to raise when trying to access an invalid group.
			///
			///</summary>
	[Serializable]
	public class InvalidGroupException : ApplicationException
	{
		public InvalidGroupException()
			: base ("Group is invalid or is in other enterprise")	{	}

		public InvalidGroupException(string Msg)
			: base (Msg)	{	}

		public InvalidGroupException(string Msg, System.Exception e)
			: base (Msg, e)	{	}

		protected InvalidGroupException (SerializationInfo info, StreamingContext context)
			: base (info, context) { }
	}
	///////////////////////////////////////////////////////////////
			///<summary>
			///Exeption to raise when trying to get inexistent enterprise
			///</summary>
	[Serializable]
	public class NullEnterpriseException : ApplicationException
	{
		public NullEnterpriseException()
			: base ("Enterprise doesnt exist") { }

		public NullEnterpriseException(string Msg)
			: base (Msg) {	}

		public NullEnterpriseException(string Msg, System.Exception e)
			: base (Msg, e)	{	}
		
		protected NullEnterpriseException (SerializationInfo info, StreamingContext context)
			: base (info, context) { }
	}
	///////////////////////////////////////////////////////////////
			///<summary>
			///Exeption to raise when trying to insert an enterprise that already exist
			///</summary>
	[Serializable]
	public class EnterpriseAlreadyExistException : ApplicationException
	{
		public EnterpriseAlreadyExistException()
			: base ("Enterprise already exist") {}

		public EnterpriseAlreadyExistException(string Msg)
			: base (Msg)	{	}

		public EnterpriseAlreadyExistException(string Msg, System.Exception e)
			: base (Msg, e)	{	}

		protected EnterpriseAlreadyExistException (SerializationInfo info, StreamingContext context)
			: base (info, context) { }
	}
///////////////////////////////////////////////////////////////

}
