using System;
using System.Data;
using Npgsql;

namespace Boxerp.Database
{

	///<summary>
	/// Esta clase se encarga de la conexion directa con la base de datos. 
	/// Actualmente utiliza el conector de mono para postgresql "Npgsql"
	///</summary>
	public class Database
	{
		public NpgsqlConnection connection;
		NpgsqlCommand command;
		string connectionString;
		string serverip, dbname, portnumber, dbuser, dbpasswd;
		
		///<summary>
		///Constructor de la clase. Crea el string con los parametros necesarios para establecer una conexión con la bd
		///</summary>
		///<param name="server">Dirección del servidor que contiene la base de datos</param>
		///<param name="db_name">Nombre de la base de datos</param>
		///<param name="port">Puerto por el que se conecta con la base de datos</param>
		///<param name="db_user">Usuario de la base de datos</param>
		///<param name="passwd">Password de la base de datos</param>
		public Database (string server, string db_name, string port, string db_user, string passwd)
		{
			this.serverip   = server;
			this.dbname     = db_name;
			this.portnumber = port;
			this.dbuser     = db_user;
			this.dbpasswd   = passwd;
			connectionString = 
	   			"Server="   + this.serverip   + ";" +
	   			"Database=" + this.dbname     + ";" +
	   			"Port="     + this.portnumber + ";" +
	   			"User Id="  + this.dbuser     + ";" + 
	   			"Password=" + this.dbpasswd   + ";";      
		}
		//////////////////////////////////////////////////////////////////////////////////////////
		
		public void Connect ()
		{
			try
			{
	  			connection = new NpgsqlConnection (connectionString);
	  			connection.Open();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		//////////////////////////////////////////////////////////////////////////////////////////

		public void Reconnect(string server, string db_name, string port, string db_user, string passwd)
		{
			this.serverip   = server;
			this.dbname     = db_name;
			this.portnumber = port;
			this.dbuser     = db_user;
			this.dbpasswd   = passwd;
			connectionString = 
	   			"Server="   + this.serverip   + ";" +
	   			"Database=" + this.dbname     + ";" +
	   			"Port="     + this.portnumber + ";" +
	   			"User Id="  + this.dbuser     + ";" + 
	   			"Password=" + this.dbpasswd   + ";";      
			try 
			{
				connection.Close();
					//connection = null; connection.Finalize();  ???
				connection = new NpgsqlConnection (connectionString);
				connection.Open();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		//////////////////////////////////////////////////////////////////////////////////////////

		public void Reconnect()
		{
			try
			{
				connection.Close();
				//connection = null; connection.Finalize();  ???
	  		connection = new NpgsqlConnection (connectionString);
	  		connection.Open();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		//////////////////////////////////////////////////////////////////////////////////////////
		
		public void CloseConnection ()
		{
			try
			{
				connection.Close();
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		/////////////////////////////////////////////////////////////////////////////////////////

		public void ExecuteQuery (string str_query)
		{
			try
			{
		  		command = connection.CreateCommand();   	
				command.CommandText = str_query;
				command.ExecuteReader();
			}
			catch (Exception ex)
			{
				throw ex; 
			}
		}
		////////////////////////////////////////////////////////////////////////////////////////
		
		///<summary>
		/// Realiza una transaccion con la base de datos (inserción, actualización y borrado)
		///</summary>
		///<returns>Una cadena vacia o un codigo de error si ha habido problemas</returns>
		public void ExecuteTransaction ()
		{
	  		//Comenzamos una transaccion
	  		NpgsqlTransaction transaction = connection.BeginTransaction (); 
			command.Transaction = transaction; //determinamos que sera parte de una transacción
			try
			{
				command.ExecuteNonQuery ();
				transaction.Commit ();
			}
			catch (NpgsqlException ex)
			{
				transaction.Rollback ();
				throw ex;
			}
		}
		/////////////////////////////////////////////////////////////////////////////////////7
		
		/*public void ExecuteQuery ()
		{
			try
			{
				command.ExecuteReader();
			} 
			catch (NpgsqlException ex)
			{
				throw ex;
			}
		}*/
		///////////////////////////////////////////////////////////////////////////////
		
		///<summary>
		/// Activa la estructura de datos con los resultados de las consultas
		///</summary>
		///<returns>Un DataSet con los datos de la consulta</returns>
		public DataSet ActiveDataSet ()
		{
			try
			{
				DataSet dataset = new DataSet ();
				NpgsqlDataAdapter adaptador = new NpgsqlDataAdapter ();
				adaptador.SelectCommand = command;
				adaptador.Fill (dataset);
				return dataset;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
		//////////////////////////////////////////////////////////////////////////////////
		
	}
	
	
	public class Test
	{
		static void Main ()
		{
			Database db = new Database ("carlosble.dyndns.org", "mono", "5432", "conflux", "");
			db.Connect();
			db.ExecuteQuery("SELECT * FROM users");
			DataSet dataset = db.ActiveDataSet();
			foreach (DataRow fila in dataset.Tables[0].Rows)
			{
				int id = (int) fila[0];
				string user_name = (string) fila[2];
				string password = (string) fila[3];
         		Console.WriteLine("Nombre: {0},{1},{2}\n", id, user_name, password);

			}

			// Tirando de las interfaces IDb, me vale para cualquier base de datos porque todos los 
			// conectores implementan estas interfaces:
			IDbConnection conn = db.connection;
			IDbCommand comm = conn.CreateCommand();
			IDbCommand comm2 = conn.CreateCommand();
			IDbTransaction trans = conn.BeginTransaction();

			comm.CommandText = "Insert into users values (3,3,'xxx','xxx',true)";
			comm.ExecuteNonQuery();  // ¿ en que se diferencia esto
			comm.CommandText = "Insert into users values (4,4,'yyy','yyy',true)";
			comm.ExecuteReader();   //    de esto otro ?
			comm.CommandText = "Insert into users values (5,5,'yyy','yyy',true)";
			comm.ExecuteReader();
			comm.Transaction = trans;
			//trans.Commit();

			IDbTransaction trans2 = conn.BeginTransaction();
			//comm.CommandText = "Insert into users values (6,6,'aaa','aaa',true)";
			//comm.Transaction = trans2;
			//trans2.Commit();
			
    	}

	}

}
