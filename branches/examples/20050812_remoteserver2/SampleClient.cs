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
				string status = "";
				int user = System.Int32.Parse(args[0]);
				int table = System.Int32.Parse(args[1]);
				// Create a channel for communicating w/ the remote object
		    // Notice no port is specified on the client
		    TcpChannel chan = new TcpChannel();
		    ChannelServices.RegisterChannel(chan);

				// Create an instance of the remote object
          	AccessController obj = (AccessController) Activator.GetObject(
		    			typeof(TestCode.Remoting.AccessController),
							"tcp://127.0.0.1:9191/AccessController");
				// Use the object
				if( obj.Equals(null) )
				{
					System.Console.WriteLine("Error: no se puede localizar el servidor");
				}
				else
				{
					while (true){
						Console.WriteLine("Modificando tabla {0}, con usuario {1}", args[1], args[0]);
						obj.start_modifying(user, table, ref status);
						Console.WriteLine(status);
						Console.ReadLine();
						obj.hilo_bucle(args[2]);
						Console.WriteLine("Terminada de modificar tabla {0}, con usuario {1}", args[1], args[0]);
						status = "";
						obj.stop_modifying(user, table, ref status);
						Console.WriteLine(status);
						Console.ReadLine();
					}
			}
		}
	}
}
