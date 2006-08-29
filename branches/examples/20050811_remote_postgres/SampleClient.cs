using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
namespace TestCode.Remoting
{
	public class SampleClient
    {
	    public static void Main(string [] args)
	    {
			// Create a channel for communicating w/ the remote object
		    // Notice no port is specified on the client
		    TcpChannel chan = new TcpChannel();
		    ChannelServices.RegisterChannel(chan);

			// Create an instance of the remote object
          	PostgresObject obj = (PostgresObject) Activator.GetObject(
		    	typeof(TestCode.Remoting.PostgresObject),
				"tcp://127.0.0.1:9191/BaseDatos");
			// Use the object
			if( obj.Equals(null) )
			{
				System.Console.WriteLine("Error: no se puede localizar el servidor");
			}
			else
			{
			    Console.WriteLine("Conectando...");
				 obj.connect();
			    Console.WriteLine("Insertando...");
				 obj.insert();
			    Console.WriteLine("Seleccionando...");
				 obj.select();
			}
		}
	}
}
