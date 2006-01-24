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
            RemotingConfiguration.Configure("client.config");
            ServerObject obj = new ServerObject();
				// Use the object
				if( obj.Equals(null) )
				{
					System.Console.WriteLine("Error: no se puede localizar el servidor");
				}
				else
				{
					int y;
					while (true){
						y = obj.get_value();
						Console.WriteLine("El valor de X es : {0}", y);
						Console.ReadLine();
						obj.set_value(5);
					}
			}
		}
	}
}
