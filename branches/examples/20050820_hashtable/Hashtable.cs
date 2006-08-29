using System;
using System.Collections;

public class Hashing
{
	public static void Main()
	{
		Hashtable permi = new Hashtable();
		permi["hola"] = true;
		string despedida = "adios";
		permi[despedida] = false;

		Console.WriteLine("hola {0}", permi["hola"]);

		IDictionaryEnumerator enumerador = permi.GetEnumerator();
		while (enumerador.MoveNext())
			Console.WriteLine("\t{0}:\t{1}", enumerador.Key, enumerador.Value);
	}
}
