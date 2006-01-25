//
// HistoryObject.cs
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
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
using Boxerp.Errors;
using Boxerp.Debug;
using Boxerp.Exceptions;
using Boxerp.Database;

namespace Boxerp.Objects
{

 	public class HistoryObject : MarshalByRefObject
	{
		GenericDatabase db;
		
		// Constructor
		public HistoryObject ()
		{
//			db = new GenericDatabase();

		}
		///////////////////////////////////////////////////////////////////////////	
				/// <summary>
				/// 
				/// </summary>
		public void Register(string msg)
		{
			try
			{
				Console.WriteLine(msg);
			}
			catch (Exception e)
			{
				Exception wrappedEx = new Exception(
					ErrorManager.AddLayer(e, RunningClass.GetName(this), RunningClass.GetMethod())); 
				throw wrappedEx;
			}
		}
		////////////////////////////////////////////////////////////////////////
		///<summary>
		/// Este método pasa de un formato DateTime al formato con el que se 
		/// almacena la fecha en la bd
		///</summary>
		///<param name="dt">Fecha a convertir</param>
		///<returns>Devuelve un string con la fecha formateada</returns>
		private string FormatDate (DateTime dt)
		{
			int year = dt.Year;
			int month = dt.Month;
			int day = dt.Day;
			int hour = dt.Hour;
			int min = dt.Minute;
			int sec = dt.Second;

			string str_month = (month < 10) ? ("0" + month.ToString()) : month.ToString(); 
			string str_day = (day < 10) ? ("0" + day.ToString()) : day.ToString(); 
			string str_hour = (hour < 10) ? ("0" + hour.ToString()) : hour.ToString(); 
			string str_min = (min < 10) ? ("0" + min.ToString()) : min.ToString(); 
			string str_sec = (sec < 10) ? ("0" + sec.ToString()) : sec.ToString(); 
				
			string str_date = year.ToString() + str_month + str_day + str_hour + str_min + str_sec;

			return str_date;

		}
		
/*		///<summary>
		/// Este método registra en el historico de acciones sobre la base de 
		/// datos la accion indicada
		///</summary>
		///<param name="id_user">Usuario que realiza la acción (¿Quién?)</param>
		///<param name="id_client">Cliente desde el que se realiza la acción (¿Desde donde?)</param>
		///<param name="window_name">Ventana del cliente donde se encuentra el usuario</param>
		///<param name="table_name">Tabla sobre la que se realiza la acción</param>
		///<param name="id_tupla">Fila de la tabla sobre la que se realiza la acción</param>
		///<param name="action">Tipo de accion que se realiza (i-u-d, inser-update-drop)</param>
		///<returns>Devuelve "true" si la consulta se realiza con exito y "false" en 
		/// otro caso </returns>
		public bool RegisterDbAction(int id_user, int id_client, string window_name, string table_name, int id_tupla, string action)
		{
			db.Table_name = "history_db_actions";

			IDictionary dic = new Hashtable();

			dic["date"] = this.FormatDate(DateTime.Now);
			dic["id_user"] = id_user;
			dic["id_client"] = id_client;
			dic["window_name"] = window_name;
			dic["table_name"] = table_name;
			dic["id_tupla"] = id_tupla;
			dic["action"] = action;

			int id_history_db_actions = db.insert(dic);

			if (id_history_db_actions == 0)
			{
				Console.WriteLine ("ERROR: no se ha podido insertar el registro en el historico");
				return false;
			}
			return true;
		}

		///<summary>
		/// Este método registra en el historico de acciones sobre la base de datos la accion indicada
		///</summary>
		///<param name="id_user">Usuario que realiza la acción (¿Quién?)</param>
		///<param name="id_client">Cliente desde el que se realiza la acción (¿Desde donde?)</param>
		///<param name="window_name">Ventana del cliente donde se encuentra el usuario</param>
		///<param name="table_name">Tabla sobre la que se realiza la acción</param>
		///<param name="id_tupla">Fila de la tabla sobre la que se realiza la acción</param>
		///<param name="action">Tipo de accion que se realiza (i-u-d, inser-update-drop)</param>
		///<param name="fields">Nombre de los campo a modificar</param>
		///<returns>Devuelve "true" si la consulta se realiza con exito y "false" en otro caso </returns>
		public bool RegisterDbAction(int id_user, int id_client, string window_name, string table_name, int id_tupla, string action, IDictionary fields)
		{
			db.Table_name = "history_db_actions";

			IDictionary dic = new Hashtable();

			dic["date"] = this.FormatDate(DateTime.Now);
			dic["id_user"] = id_user;
			dic["id_client"] = id_client;
			dic["window_name"] = window_name;
			dic["table_name"] = table_name;
			dic["id_tupla"] = id_tupla;
			dic["action"] = action;

			int id_history_db_actions = db.insert(dic);

			if (id_history_db_actions > 0)
			{
				db.Table_name = "history_db_fields";

				IDictionary dic_fields = new Hashtable();
			
				ICollection keys = fields.Keys;

				dic_fields["id_history_db_actions"] = id_history_db_actions;

				foreach (string key in keys)
				{
					dic_fields["field_name"] = key;
					dic_fields["field_value"] = fields[key];

					if (db.insert(dic_fields) == 0)
						Console.WriteLine ("ERROR: no se ha podido insertar el registro en el historico");
				}
			}
			else
			{
				Console.WriteLine ("ERROR: no se ha podido insertar el registro en el historico");
				return false;
			}
			return true;
		}

		///<summary>
		/// Este método registra en el historico de sessiones cuando entran y salen los usuarios
		///</summary>
		///<param name="id_user">Usuario que realiza la acción (¿Quién?)</param>
		///<param name="id_client">Cliente desde el que se realiza la acción (¿Desde donde?)</param>
		///<param name="login">1-El usuario entra en el sistema; 0-El usuario sale del sistema</param>
		///<returns>Devuelve "true" si la consulta se realiza con exito y "false" en otro caso </returns>
		public bool RegisterSession(int id_user, int id_client, string login)
		{
			db.Table_name = "history_sessions";

			IDictionary dic = new Hashtable();

			dic["date"] = this.FormatDate(DateTime.Now);
			dic["id_user"] = id_user;
			dic["id_client"] = id_client;
			dic["login"] = login;

			if (db.insert(dic) == 0)
			{
				Console.WriteLine ("ERROR: no se ha podido insertar el registro en el historico");
				return false;
			}
			return true;

		}

		/*public DataSet GetDbActionsByDate()
		{

		}*/
/*
		///<summary>
		/// Este método devuelve el histórico de sesiones entre las fechas indicadas 
		///</summary>
		///<param name="ini_date">Fecha de inicio de la busqueda</param>
		///<param name="end_date">Fecha de final de la busqueda</param>
		///<returns>Devuelve un DataSet con los registros encontrados </returns>
		public DataSet GetSessionsByDate(string ini_date, string end_date)
		{
			string query = "SELECT * FROM history_sessions WHERE date >= " + ini_date + " and date <= " + end_date;
			db.setQuery(query);
			db.executeQuery();
			return db.activeDataset();
		}
	*/
	}	
}
