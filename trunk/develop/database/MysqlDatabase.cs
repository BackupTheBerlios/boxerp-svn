/*using System;
using System.Data;
using ByteFX.Data.MySqlClient;

namespace Boxerp.Objects
{

public class MySqlDatabase
{
	MySqlConnection connection;
	MySqlCommand command;
		
	public MySqlDatabase (string server, string db_name, string port, string db_user, string passwd)
	{
		//Definición de la conexión
		string connectionString = 
	   		"Server=" + server + ";" +
	   		"Database=" + db_name + ";" +
	   		//"Port=" + port + ";" +
	   		"User Id=" + db_user + ";" + // Aquí debes de indicar algún usuario existente en la BD
	   		"Password=" + passwd + ";";      // que tenga permisos, ademas de su contraseña
	  	//MySqlConnection conexion;
	  	connection = new MySqlConnection (connectionString);
	  	connection.Open();
	}

	public void closeConnection ()
	{
		connection.Close();
	}

	public void setQuery (string str_query)
	{
	  	//Definición de comando a ejecutar
	  	command = connection.CreateCommand();   	
		command.CommandText = str_query;
	}

	public bool executeTransaction ()
	{
	  	//Comenzamos una transaccion
	  	MySqlTransaction transaction = connection.BeginTransaction (); 
		command.Transaction = transaction; //determinamos que sera parte de una transacción
		try
		{
			command.ExecuteNonQuery ();
			transaction.Commit ();
			return true;
			//Console.WriteLine ("Inserción lograda con éxito");
		}
		catch (MySqlException ex)
		{
			Console.WriteLine ("No se pudo realizar transaccion: "+ex.Message);
			transaction.Rollback ();
			return false;
		}

	}

	public MySqlDataReader executeQuery ()
	{
		try{
			return command.ExecuteReader();
		} 
		catch (MySqlException ex)
		{
			Console.WriteLine ("No se pudo realizar transaccion: "+ex.Message);
			return null;
		}
	}

	public DataSet activeDataset ()
	{
		DataSet dataset = new DataSet ();
		MySqlDataAdapter adaptador = new MySqlDataAdapter ();
		adaptador.SelectCommand = command;
		adaptador.Fill (dataset);


		return dataset;

	}


}
/*
public class Test
{
	static void Main ()
	{
		Database db = new Database ("127.0.0.1", "mono", "5432", "conflux", "");
		//db.setQuery ("INSERT INTO users (id, user_name, password, published) VALUES (0, 'carlos', 'clave', true)");
		//db.executeTransaction();
		db.setQuery ("SELECT * FROM users");
		MySqlDataReader lector = db.executeQuery();
		while (lector.Read ()) 
		{
			int id = (int) lector["id"];
			string user_name = (string) lector["user_name"];
			string password = (string) lector["password"];
         	Console.WriteLine("T-uplas: {0},{1},{2}\n", id, user_name, password);
		}
		lector.Close();

		DataSet dataset = db.activeDataset();

		foreach (DataRow fila in dataset.Tables[0].Rows)
		{
			int id = (int) fila[0];
			string user_name = (string) fila[2];
			string password = (string) fila[3];
         	Console.WriteLine("Nombre: {0},{1},{2}\n", id, user_name, password);

		}
    }

}
*/
}*/
