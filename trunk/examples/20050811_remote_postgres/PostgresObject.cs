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
	private NpgsqlConnection conexion;
	private NpgsqlTransaction transaccion;
	
	public PostgresObject (){}  // Constructor
		
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
 }

}
