using System;
using System.Data;
using Npgsql;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Runtime.CompilerServices;  // for synchronized methods
namespace TestCode.Remoting
{

 	public class AccessController : MarshalByRefObject
	{
		// Atributos:	
		const int TOTALUSERS = 5;
		const int TOTALTABLES = 3;
		// Tendria que consultar el número total de usuarios en la base de datos al iniciar la aplicacion
		// y tambien el numero total de tablas, para crear la matriz de usuarios y tablas:
		// Para el acceso directo al array, se haría hashing del id del usuario y del id de la tabla
		// Por tanto deberia haber en la base de datos una tabla de tablas con su nombre y su id.
		private bool[,] access_status; 
		
		// Constructor
		public AccessController ()
		{
			access_status = new bool[TOTALUSERS,TOTALTABLES];
		}  
		///////////////////////////////////////////////////////////////////////////////////////////////////	
	
		[MethodImpl(MethodImplOptions.Synchronized)]		
		public int  start_modifying( int uid, int tid, ref string status) 
		/*
		 * Client sends user id ant table id that is modifying to prevent other users 
		 * Con el churro que hay antes de public le estoy diciendo que solo un hilo toque a la vez
		 */
		{
			bool conflict = false;
			for (int u = 0; u < TOTALUSERS; u++){
				if (access_status[u,tid] == true){
					conflict = true;
					status += u + ",";
				}
			}
			if (conflict)
				status = "This table is modifying by this users:" + status;
			access_status[uid,tid] = true;
			return 0;		
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////

		public int stop_modifying(int uid, int tid, ref string status)
		/*
		 * Client stop modifying, saving o discarding changes, but stop modifying
		 */
		{
			access_status[uid,tid] = false;
			return(0);
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////

		[OneWay]
		public void bucle()
		{
			int x = 0;
			while (x < 50000)
			{
				//System.Threading.Interlocked.Increment(ref x);	// Por seguridad se utiliza esta clase
				x++;                              // Asi tambien parece funcionar al menos en mi maquina
				Console.WriteLine("X = {0}", x);
				Console.WriteLine("Hilo: {0}", Thread.CurrentThread.Name);
			}
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////

		public void hilo_bucle(string thread_id)
		{
				ThreadStart delegate_bucle = new ThreadStart (this.bucle);
		    Thread thread_bucle = new Thread (delegate_bucle);
				thread_bucle.Name = thread_id;
	      thread_bucle.Start();
		}
	
	}

}
