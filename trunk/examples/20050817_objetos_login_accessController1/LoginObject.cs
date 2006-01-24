using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
namespace Boxerp.Objects
{

 	public class LoginObject : MarshalByRefObject
	{
		
		// Constructor
		public LoginObject ()
		{
			db = new Boxerp.Objects.Database("localhost", "mono", "5432", "conflux", "");

		}
		///////////////////////////////////////////////////////////////////////////////////////////////////	
	
		public int login(string user, string passwd, ref int gid, ref int[,] permissions, ref string error_code)
		{
			// Query username and password and return permissions if login success.
			db.setQuery("SELECT id, id_group FROM users WHERE username = " + user +" AND password = " + passwd);
			db.executeQuery();
			DataSet dataset = db.activeDataset();
			if (dataset.Tables[0].Count() != 0)
			{
				DataRow result = dataset.Tables[0].Rows[0];
				DataColumn col1 = dataset.Tables[0].Columns[0];
				DataColumn col2 = dataset.Tables[0].Columns[1];
				int userid = (int) result[col1];
				gid = (int) result[col2];
				string gettingperms;
				gettingperms  = "SELECT ctrld_actions.id, ctrld_sections.id FROM ctrld_actions AS acts, ";
				gettingperms += "ctrld_sections AS secs, permissions AS perm WHERE ";
				gettingperms += "acts.id = perm.id_ctrld_actions AND secs.id = perm.id_ctrld_sections AND perm.id_group IN ";
				gettingperms += "(SELECT id_group FROM users WHERE id = " + UID + ")";
				db.setQuery(gettingperms);
				// Put data in permissions array
				return(0);
			}
			else
			{
				db.setQuery("SELECT id FROM error_codes WHERE description = 'Login incorrect'");
				db.executeQuery();  
				error_code = dataset.Tables[0].Rows[0].Columns[0];
				return(-1); // Error Code
			}
			// El cliente, despues deberá llamar al objeto Access Controller para ver si ese usuario ya estaba 
			// en el sistema y qué se debe hacer. 
		}
	
	}

}
