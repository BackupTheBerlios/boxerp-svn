using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
using System.Collections;
namespace Boxerp.Objects
{

 	public class AccessController : MarshalByRefObject
	{
		// Atributos:	
		const int BEING_MODIFIED = 1;
		const int WAS_MODIFIED = 2;
		const int WAS_AND_BEING_MODIF = 3;
		const int UNLOCKED = 0;
		const int TOTALUSERS = 5;
		const int TOTALTABLES = 3;
		// Tendria que consultar el número total de usuarios en la base de datos al iniciar la aplicacion
		// y tambien el numero total de tablas, para crear la matriz de usuarios y tablas:
		// Para el acceso directo al array, se haría hashing del id del usuario y del id de la tabla
		// Por tanto deberia haber en la base de datos una tabla de tablas con su nombre y su id.
		private int [,] access_status; 
		
		// Constructor
		public AccessController ()
		{
			access_status = new int[TOTALUSERS,TOTALTABLES];
			for (int i = 0; i < TOTALUSERS; i++)
				for (int j = 0; j < TOTALTABLES; j++)
					access_status[i,j] = UNLOCKED;
		}  
		///////////////////////////////////////////////////////////////////////////////////////////////////

		public ArrayList findrelated(int sectionid, int moduleid)
		{
			ArrayList tables = new ArrayList();
			if ((sectionid == 0) && (moduleid == 0))
			{
				tables.Add(0);
				tables.Add(3);
				tables.Add(6);
				tables.Add(1);
			}
			return tables;
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////	
	
		public int  start_modifying( int uid, int sectionid, int moduleid, ref string status) 
		/*
		 * Client sends user id ant table id that is modifying to prevent other users 
		 */
		{
			ArrayList relatedtables = this.findrelated(sectionid, moduleid);
			bool conflict = false;
			int thistable = 0;
			IEnumerator tables = relatedtables.GetEnumerator(); 
			for (int u = 0; u < TOTALUSERS; u++)
				while (tables.MoveNext())
				{
					thistable = access_status[u, (int)tables.Current];
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
							conflict = true;
							status += u + ",";	// comprobar que el usuario no se metió ya por conflicto en otra tabla
						}
						else if (access_status[u, (int)tables.Current] == WAS_MODIFIED)
						{
							// Never mind. 
						}
					}
					access_status[u, (int)tables.Current] = thistable;
				}
			if (conflict){
				status = "This table is modifying by this users:" + status;
				return 1;		
			}
			else return(0);
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////

		public int stop_modifying(int uid, int sectionid, int moduleid, ref string status)
		/**
			* Client stop modifying, saving o discarding changes, but stop modifying
			*/
		{
			ArrayList relatedtables = this.findrelated(sectionid, moduleid);
			bool conflict = false;
			int thistable = 0;
			IEnumerator tables = relatedtables.GetEnumerator(); 
			for (int u = 0; u < TOTALUSERS; u++)
				while (tables.MoveNext())
				{
					thistable = access_status[u, (int)tables.Current];
					if (u == uid)
					{
						if (thistable == BEING_MODIFIED)	// TODO: Check! Could be other instance of same user
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
									// Must be other user session instance because the same user dont WAS_MODIFIED twice
							}
					}
					else 
					{
							if (thistable == BEING_MODIFIED)
							{
								// Other user is modifying, alert this user
								conflict = true;
								status += "Caution:User {0} is already modifying table" + u;
							}
							else if (thistable == WAS_AND_BEING_MODIF)
							{
								// Data changed!!! 
								conflict = true;
								status += "Danger!: User {0} changed data while you was working and is modifying again!" + u;
							}
							else // WAS_MODIFIED
							{
								// Data changed!!!
								conflict = true;
								status += "Danger!: User {0}, changed data while you was working" + u;
							}
					}
					access_status[u, (int)tables.Current] = thistable;
				}
			if (conflict){
				status = "There are conflicts with other users. Do you really want to save?: " + status;
				return 1;		
			}
			else return(0);
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////
	
	}

}
