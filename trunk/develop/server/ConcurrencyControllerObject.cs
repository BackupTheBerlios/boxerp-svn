//
// ConcurrencyControllerObject.cs
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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
using System.Collections;
using Boxerp.Database; 

namespace Boxerp.Objects
{

		///<summary>
		/// The Concurrency Controller is a Singleton that check what database tables are being
		/// acceded by every user, every moment. It must detect when one or more users are 
		/// trying to write the same table at same time.
		/// When a Web Service instance gonna access database invokes the Concurrency controller's
		/// StartModify method and then it call StopModify when finished.
		/// This class in not implemented yet. This code is only an idea  
		///</summary>
 	public class ConcurrencyControllerObject : MarshalByRefObject
	{
		const int BEING_MODIFIED = 1;
		const int WAS_MODIFIED = 2;
		const int WAS_AND_BEING_MODIF = 3;
		const int UNLOCKED = 0;
		// FIXME: Read TOTALUSERS and TOTALTABLES from database to create accessStatus matrix.
		const int TOTALUSERS = 5;
		const int TOTALTABLES = 3;
		private int [,] accessStatus;           // matrix to match users and database tables
		
				///<summary>
				///Set al tables unlocked
				///</summary>
		public ConcurrencyControllerObject ()
		{
			accessStatus = new int[TOTALUSERS,TOTALTABLES];
			for (int i = 0; i < TOTALUSERS; i++)
				for (int j = 0; j < TOTALTABLES; j++)
					accessStatus[i,j] = UNLOCKED;
		}  
		//////////////////////////////////////////////////////////////////////////
				///<summary>
				/// There is a group of tables related to every section and module of the application.
				/// This methos get the related tables
				///</summary>
		public ArrayList FindRelated(int sectionId, int moduleId)
		{
			// FIXME: This is not the algorithm, is only a poor example for understand the target.
			ArrayList tables = new ArrayList();
			if ((sectionId == 0) && (moduleId == 0))
			{
				tables.Add(0);
				tables.Add(3);
				tables.Add(6);
				tables.Add(1);
			}
			return tables;
		}
		//////////////////////////////////////////////////////////////////////////////////////	
				///<summary>
				///Web services notify the controller before modify thru this method
				///</summary>
		public void StartModify( int uid, int sectionId, int moduleId, ref string status) 
		{
			ArrayList relatedtables = this.FindRelated(sectionId, moduleId);
			int thistable = 0;
			IEnumerator tables = relatedtables.GetEnumerator(); 
			for (int u = 0; u < TOTALUSERS; u++)
				while (tables.MoveNext())
				{
					thistable = accessStatus[u, (int)tables.Current];
					if (u == uid)		
					{
						if (thistable == BEING_MODIFIED) 
						{
							// TODO: Check for differente sessions with same username. Is a conflict
						}
						else if (thistable == WAS_MODIFIED)
						{
							// TODO: Check session instances
							thistable = WAS_AND_BEING_MODIF;
						}
						else
							thistable = BEING_MODIFIED;   // TODO: Add information about session
					}
					else 
					{
						// If someone is modifying table, you must notice the user
						if (thistable == BEING_MODIFIED)
						{
							// FIXME: Generate and throw and exception to the caller 
							status += u + ",";	// check same user name in different sessions
						}
						else if (accessStatus[u, (int)tables.Current] == WAS_MODIFIED)
						{
							// Never mind. 
						}
					}
					accessStatus[u, (int)tables.Current] = thistable;
				}
		}
		////////////////////////////////////////////////////////////////////
				///<summary>
				/// Caller stop modifying, saving o discarding changes, but stop modifying
				///
				///</summary>
		public void StopModify(int uid, int sectionId, int moduleId, ref string status)
		{
			ArrayList relatedtables = this.FindRelated(sectionId, moduleId);
			int thistable = 0;
			IEnumerator tables = relatedtables.GetEnumerator(); 
			for (int u = 0; u < TOTALUSERS; u++)
				while (tables.MoveNext())
				{
					thistable = accessStatus[u, (int)tables.Current];
					if (u == uid)
					{
						if (thistable == BEING_MODIFIED)	// TODO:Check!Could be other session of same user
							{
								// If is the same user mark WAS_MODIFIED
								thistable = WAS_MODIFIED;
							}
							else if (thistable == WAS_AND_BEING_MODIF) // TODO: Do the same check
							{
								// If is the same user mark WAS_MODIFIED
								thistable = WAS_MODIFIED;
							}
							else // WAS_MODIFIED
							{
								// Must be other user session instance 
								// because the same user dont WAS_MODIFIED twice
							}
					}
					else 
					{
						if (thistable == BEING_MODIFIED)
						{
							// Other user is modifying, alert this user
							// FIXME: Generate and throw an exception
							status += "Caution:User {0} is already modifying table" + u;
						}
						else if (thistable == WAS_AND_BEING_MODIF)
						{
							// Data changed!!! 
							// FIXME: Generate and throw an exception
							status += "Danger!: User {0} changed data while you was working and is modifying again!" + u;
						}
						else // WAS_MODIFIED
						{
							// Data changed!!!
							// FIXME: Generate and throw an exception
							status += "Danger!: User {0}, changed data while you was working" + u;
						}
					}
					accessStatus[u, (int)tables.Current] = thistable;
				}
		}
		/////////////////////////////////////////////////////////////////////////
	}

}
