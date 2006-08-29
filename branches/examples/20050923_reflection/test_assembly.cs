using System;
using System.Reflection;
using System.Runtime.Remoting;

namespace Test
{
	interface IHolaMundo
	{ 
		string HolaMundo();
	}//Fin de definición de la interface

	class HolaMundoCastellano: IHolaMundo
	{
		public string HolaMundo()
		{ 
			return "Hola Mundo"; 
		}
	}//Fin de definición de HolaMundoCastellano

	class HolaMundoIngles:IHolaMundo
	{
		public string HolaMundo() 
		{
			return "Hello world"; 
		}
	}//Fin de definición de HolaMundoIngles

	class Principal
	 {
		///Punto de entrada de la aplicacion 
		//[STAThread] 
		static void Main(string [] args) 
		{
			IHolaMundo oTest; 
			ObjectHandle ManipularObjeto; 
			Console.WriteLine ("Introduzca el nombre de la clase que quiere crear:"); 
			Console.WriteLine ("HolaMundoCastellano"); 
			Console.WriteLine ("HolaMundoIngles"); 
			string DatoUsuario = Console.ReadLine();
			string CadenaObjeto = "Test." + DatoUsuario; 
			Assembly assembly = Assembly.GetExecutingAssembly(); 
			try { 
				ManipularObjeto = AppDomain.CurrentDomain.CreateInstance(assembly.FullName, CadenaObjeto); 
				oTest = (IHolaMundo) ManipularObjeto.Unwrap(); 
				Console.WriteLine( oTest.HolaMundo()); 
			} 
			catch (Exception e) 
			{
				Console.WriteLine ("No se ha podido crear el objeto." ); 
				Console.WriteLine (e.Message ); 
			}  
		} 
	}
}
