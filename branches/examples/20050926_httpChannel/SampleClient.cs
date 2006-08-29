using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Threading;
namespace TestCode.Remoting
{
	public class SampleClient
	{
			public long cuenta = 0;
	
			public SampleClient()
			{
			}

			public void llamada_remota()
			{
				long x = 0;
				Console.WriteLine("Thread haciendo llamada");
				ObjetoRemoto obj = (ObjetoRemoto) Activator.GetObject(
		    	typeof(TestCode.Remoting.ObjetoRemoto),
					"http://127.0.0.1:9090/ObjetoRemoto");
				Console.WriteLine("Finalizado contador, {0}", obj.contador(ref x));
			}
			
	    public static void Main(string [] args)
	    {
				IDictionary channelProperties = new Hashtable();
				channelProperties["timeout"] = 2000;
				HttpChannel channel = new HttpChannel(channelProperties,
						new SoapClientFormatterSinkProvider(),
						new SoapServerFormatterSinkProvider());

				ChannelServices.RegisterChannel(channel);

				SampleClient client = new SampleClient();
				Console.WriteLine("Empezar a contar");
				Console.ReadLine();
				client.llamada_remota();
				Console.WriteLine("Ejecucion terminada");
			}
		}
}
