//
// GenericDatabase.cs
//
// Authors:
// 	Zebenzui Perez Ramos <zebenperez@shidix.com>
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
using System.Data;
using Npgsql;
using Boxerp.Debug;
using Boxerp.Errors;

namespace Boxerp.Database
{

	///<summary>
	/// Esta clase se encarga de la conexion directa con la base de datos. 
	/// Actualmente utiliza el conector de mono para postgresql "Npgsql"
	///</summary>
	public class Database
	{
		NpgsqlConnection connection;
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
				Exception wrappedEx = new Exception(ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
        throw wrappedEx;
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
				Exception wrappedEx = new Exception(ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
        throw wrappedEx;
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
				Exception wrappedEx = new Exception(ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
        throw wrappedEx;
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
				Exception wrappedEx = new Exception(ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
        throw wrappedEx;
			}
		}
		/////////////////////////////////////////////////////////////////////////////////////////

		public void ExecuteQuery (string str_query)
		{
			try
			{
				if (connection.State == ConnectionState.Open)
				{
					command = connection.CreateCommand();   	
					command.CommandText = str_query;
					command.ExecuteReader();
				}
				else
					Reconnect();
			}
			catch (Exception ex)
			{
				CloseConnection();	// FIXME: Capturar exactamente la excepcion que precisa un cierra de conexion. 
				Exception wrappedEx = new Exception(ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
			
        throw wrappedEx;
			}
		}
		////////////////////////////////////////////////////////////////////////////////////////
		/*	
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
		}*/
		/////////////////////////////////////////////////////////////////////////////////////
				
		public IDbTransaction BeginTransaction ()
		{
			try
			{
				return connection.BeginTransaction();
			} 
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
        throw wrappedEx;
			}
		}
		/////////////////////////////////////////////////////////////////////////////////////

		public void Commit(IDbTransaction transaction)
		{
			try
			{
				command.Transaction = (NpgsqlTransaction)transaction;
				transaction.Commit();
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
        throw wrappedEx;
			}
		}
		/////////////////////////////////////////////////////////////////////////////////////

		public void Rollback(IDbTransaction transaction)
		{
			try
			{
				command.Transaction = (NpgsqlTransaction)transaction;
				transaction.Rollback();
			}
			catch (Exception ex)
			{
				Exception wrappedEx = new Exception(ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
        throw wrappedEx;
			}
		}
		/////////////////////////////////////////////////////////////////////////////////////
		
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
				Exception wrappedEx = new Exception(ErrorManager.AddLayer(ex, RunningClass.GetName(this), RunningClass.GetMethod()));
        throw wrappedEx;
			}
		}
		//////////////////////////////////////////////////////////////////////////////////
		
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
			NpgsqlDataReader lector = db.executeQuery();
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
}
