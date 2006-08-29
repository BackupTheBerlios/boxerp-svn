/* Linea de compilacion:
mcs -r:System.Data -r:Npgsql postgres1.cs

*/
using System;
using System.Data;
using Npgsql;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
namespace TestCode.Remoting
{

 	public class PostgresObject : MarshalByRefObject
	{
		// Atributos:	
		private NpgsqlConnection conexion;
		private NpgsqlTransaction transaccion;
		const int TOTALUSERS = 5;
		const int TOTALTABLES = 3;
		// Tendria que consultar el número total de usuarios en la base de datos al iniciar la aplicacion
		// y tambien el numero total de tablas, para crear la matriz de usuarios y tablas:
		// Para el acceso directo al array, se haría hashing del id del usuario y del id de la tabla
		// Por tanto deberia haber en la base de datos una tabla de tablas con su nombre y su id.
		private bool[,] access_status; 

		// Constructor
		public PostgresObject ()
		{
			access_status = new bool[TOTALUSERS,TOTALTABLES];
		}  
		////////////////////////////////////////////////////////////////////////////////////////////////

		public void connect()
		{
			//Definición de la conexión
			string connectionString =
				"Server=127.0.0.1;" +
				"Database=mono;" +
				"Port=5432;" +
				"User Id=conflux;" + // Aquí debes de indicar algún usuario existente en la BD
				"Password=;";      // que tenga permisos, ademas de su contraseña
			conexion = new NpgsqlConnection (connectionString);
			conexion.Open();
			//Comenzamos una transaccion
			transaccion = conexion.BeginTransaction (); 
		}
		////////////////////////////////////////////////////////////////////////////////////////////////
		
		public void insert ()
		{
			//Definición de comando a ejecutar
			NpgsqlCommand comando = conexion.CreateCommand();   	
			comando.CommandText = "INSERT INTO users (id, user_name, password, published) VALUES (0, 'carlos', 'clave', true)";
			comando.Transaction = transaccion; //determinamos que sera parte de una transacción
			//Intentamos realizar el comando... 
			try
			{
				comando.ExecuteNonQuery ();
				transaccion.Commit ();
				Console.WriteLine ("Inserción lograda con éxito");
			}
			catch (NpgsqlException ex)
			{
				Console.WriteLine ("No se pudo realizar transaccion: "+ex.Message);
				transaccion.Rollback ();
			}
		}
		////////////////////////////////////////////////////////////////////////////////////////////////////
		
		public void select ()
		{
			//Definición de comando a ejecutar
			NpgsqlCommand comando_lector = conexion.CreateCommand();
			comando_lector.CommandText = "SELECT * FROM users";
			//Lector de datos
			NpgsqlDataReader lector = comando_lector.ExecuteReader();
			while (lector.Read ()) 
			{
				int id = (int) lector["id"];
				string user_name = (string) lector["user_name"];
				string password = (string) lector["password"];
				Console.WriteLine("T-uplas: {0},{1},{2}\n", id, user_name, password);
			}
			lector.Close();
		}
		///////////////////////////////////////////////////////////////////////////////////////////////////	
	
		public int start_modifying(int uid, int tid, ref string status)
		/*
		 * Client sends user id ant table id that is modifying to prevent other users 
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

		public void bucle()
		{
			int x = 0;
			while (x < 150000)
			{
				x++;
				Console.WriteLine("X = {0}", x);
			}
		}
	
	
	
	}

}
