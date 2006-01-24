using System;
using System.Data;
using Npgsql;

/* Linea de compilacion:
mcs -r:System.Data -r:Npgsql postgres1.cs

*/

public class PostgreSQL
{
	public static void Main(string[] args)
	{
		//Definición de la conexión
		string connectionString =
	   		"Server=127.0.0.1;" +
	   		"Database=mono;" +
	   		"Port=5432;" +
	   		"User Id=conflux;" + // Aquí debes de indicar algún usuario existente en la BD
	   		"Password=;";      // que tenga permisos, ademas de su contraseña
	  	NpgsqlConnection conexion;
	  	conexion = new NpgsqlConnection (connectionString);
	  	conexion.Open();
	  	//Comenzamos una transaccion
	  	NpgsqlTransaction transaccion = conexion.BeginTransaction (); 
	  	//Definición de comando a ejecutar
	  	NpgsqlCommand comando = conexion.CreateCommand();   	
		comando.CommandText = "INSERT INTO users (id, user_name, password, published) VALUES (0, 'carlos', 'clave', true)";
		comando.Transaction = transaccion; //determinamos que sera parte de una transacción
		NpgsqlCommand comando_param = conexion.CreateCommand();
		comando_param.CommandText = "INSERT INTO groups (id, group_name, published) VALUES (:nombres, :apellidos, :correo)";
		comando_param.Transaction = transaccion;
		//comando_param.Parameters.Add (new NpgsqlParameter ("id", 0));
		//comando_param.Parameters.Add (new NpgsqlParameter ("group_name", "Grupo1"));
		//comando_param.Parameters.Add (new NpgsqlParameter ("published", true)); 
		//Intentamos realizar el comando... 
		try
		{
			comando.ExecuteNonQuery ();
		//	comando_param.ExecuteNonQuery ();
			//comando.E
			transaccion.Commit ();
			Console.WriteLine ("Inserción lograda con éxito");
		}
		catch (NpgsqlException ex)
		{
			Console.WriteLine ("No se pudo realizar transaccion: "+ex.Message);
			transaccion.Rollback ();
		}
		
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
		//Limpiando...
		lector.Close();
		//Un dataset para usarse en modo desconectado
		DataSet dataset = new DataSet ();
		NpgsqlDataAdapter adaptador = new NpgsqlDataAdapter ();
		adaptador.SelectCommand = comando_lector;
		adaptador.Fill (dataset);
		//
		lector = null;
		comando.Dispose();
		comando = null;
		conexion.Close();
		conexion = null;
		//Consulta desconectada
		foreach (DataRow fila in dataset.Tables[0].Rows)
		{
			int id = (int) fila[1];
			string user_name = (string) fila[2];
			string password = (string) fila[3];
         Console.WriteLine("Nombre: {0},{1},{2}\n", id, user_name, password);

		}
    }
}
