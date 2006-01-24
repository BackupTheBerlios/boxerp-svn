using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
namespace TestCode.Remoting
{
	public class SampleServer
    {
	    public static void Main(string [] args)
	    {
		   TcpChannel channel = new TcpChannel(9191);
		   ChannelServices.RegisterChannel(channel);
		            
			RemotingConfiguration.RegisterWellKnownServiceType(
				typeof(ObjetoRemoto),
				"ObjetoRemoto",
				WellKnownObjectMode.Singleton );
			
			System.Console.WriteLine("Presiona una tecla para salir...");
			System.Console.ReadLine();
		}
	}
}

