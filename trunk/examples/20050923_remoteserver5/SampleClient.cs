using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
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

			public void envuelto()
			{
				long x = 0;
				Console.WriteLine("Thread haciendo llamada");
				ObjetoRemoto obj = (ObjetoRemoto) Activator.GetObject(
		    	typeof(TestCode.Remoting.ObjetoRemoto),
					"tcp://127.0.0.1:9191/ObjetoRemoto");
				Console.WriteLine("Finalizado contador, {0}", obj.contador(ref x));
			}
			
	    public static void Main(string [] args)
	    {
		    TcpChannel chan = new TcpChannel();
		    ChannelServices.RegisterChannel(chan);

				SampleClient client = new SampleClient();
				while (true){
						Console.WriteLine("Empezar a contar");
						Console.ReadLine();
       			ThreadStart delegate_metodo = new ThreadStart (client.envuelto);
			      Thread thread_m = new Thread (delegate_metodo);
						thread_m.Start();
						Thread.Sleep(1000);
						thread_m.Abort();	
				}
						Console.WriteLine("Ejecucion terminada");
			}
		}
}
