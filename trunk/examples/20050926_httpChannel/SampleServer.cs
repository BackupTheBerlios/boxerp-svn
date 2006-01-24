using System;
using System.Data;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
namespace TestCode.Remoting
{
	public class SampleServer
    {
	    public static void Main(string [] args)
	    {
       IDictionary channelProperties = new Hashtable();
			 channelProperties["port"] = 9090;
			 HttpChannel channel = new HttpChannel(channelProperties,
				                     new SoapClientFormatterSinkProvider(),
				                     new SoapServerFormatterSinkProvider());

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

