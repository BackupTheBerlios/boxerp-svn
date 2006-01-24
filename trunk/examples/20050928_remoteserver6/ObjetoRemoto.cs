using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
namespace TestCode.Remoting
{

 	public class ObjetoRemoto : MarshalByRefObject
	{
		string texto;
		
		// Constructor
		public ObjetoRemoto ()
		{
			texto = "hola";
		}  
		///////////////////////////////////////////////////////////////////////////////////////////////////	
	
		public int contador(ref long cuenta)
		{
			Console.WriteLine(texto);
			Exception ex = new Exception("probando");
			throw ex;
		}
	
	}

}
