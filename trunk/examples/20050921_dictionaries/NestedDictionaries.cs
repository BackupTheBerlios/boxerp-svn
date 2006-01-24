using System;
using System.Collections;



public class NestedDictionaries
{
		
		struct User
		{
			int id;
			public string name;
			string passwd;
			bool published;
			bool modified;
		}

		struct Group
		{
			int id;
			public string name;
			bool published;
			bool modified;
			public IDictionary users;
		}
		
		struct Enterprise
		{
			int id;
			public string name;
			bool published;
			string desc;
			bool modified;
			public IDictionary groups;
		}
		
		IDictionary enterprises;
		///////////////////////////////////////////
		
		public NestedDictionaries()
		{
			
			Enterprise e = new Enterprise();
			e.groups = new Hashtable();
			Group g = new Group();
			g.users = new Hashtable();
			User u = new User();
			enterprises = new Hashtable();

			u.name = "PACO";
			g.users["PACO"] = u;
			g.name = "Grupo1";
			e.groups["Grupo1"] = g;
			e.name = "EmpresaX";
			enterprises["EmpresaX"] = e;
		}


		public static void Main()
		{
			NestedDictionaries nd = new NestedDictionaries();
			
			Enterprise e;
			Group g;
			User u;

			// Leo los valores que metí en el constructor:
			e = (Enterprise)nd.enterprises["EmpresaX"];
			g = (Group)e.groups["Grupo1"];
			u = (User)g.users["PACO"];
			Console.WriteLine("Empresa: {0}, Grupo: {1}, Usuario: {2}", e.name, g.name, u.name);
			string name = ((User)((Group)((Enterprise)nd.enterprises["EmpresaX"]).groups["Grupo1"]).users["PACO"]).name;
			Console.WriteLine("Usuario: {0}", name);

			// Le meto otros nuevos 
			u.name = "LOLO";
			g.users["LOLO"] = u;
			g.name = "Grupo2";
			e.groups["Grupo2"] = g;
			e.name = "EmpresaZ";
			nd.enterprises["EmpresaZ"] = e;
			
			// Leo los viejos a ver si se han machacado valores o no
			e = (Enterprise)nd.enterprises["EmpresaX"];
			g = (Group)e.groups["Grupo1"];
			u = (User)g.users["PACO"];
			Console.WriteLine("Empresa: {0}, Grupo: {1}, Usuario: {2}", e.name, g.name, u.name);
			name = ((User)((Group)((Enterprise)nd.enterprises["EmpresaX"]).groups["Grupo1"]).users["PACO"]).name;
			Console.WriteLine("Usuario: {0}", name);
			
			// Leo los ultimos que meti
			e = (Enterprise)nd.enterprises["EmpresaZ"];
			g = (Group)e.groups["Grupo2"];
			u = (User)g.users["LOLO"];
			Console.WriteLine("Empresa: {0}, Grupo: {1}, Usuario: {2}", e.name, g.name, u.name);
			name = ((User)((Group)((Enterprise)nd.enterprises["EmpresaZ"]).groups["Grupo2"]).users["LOLO"]).name;
			Console.WriteLine("Usuario: {0}", name);
			
		}

}
