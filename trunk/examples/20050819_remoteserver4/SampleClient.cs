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
			
			public delegate int RemoteAsyncDelegate(ref long cuenta);
		
			//[OneWay]    -> No entiendo la diferencia entre ponerlo y no ponerlo
			public void fin_contador(IAsyncResult ar)
			/* Este método se ejecuta cuando la llamada remota termina
			* y recoge los resultados a traves de del.EndInvoke
			*
			* */
			{
				RemoteAsyncDelegate del = (RemoteAsyncDelegate)((AsyncResult)ar).AsyncDelegate;
				Console.WriteLine("--- El servidor devolvió: "  + del.EndInvoke(ref this.cuenta, ar));
				Console.WriteLine("--- La cuenta es {0}", this.cuenta);
				return;
			}
	
	    public static void Main(string [] args)
	    {
		    TcpChannel chan = new TcpChannel();
		    ChannelServices.RegisterChannel(chan);

				ObjetoRemoto obj = (ObjetoRemoto) Activator.GetObject(
		    	typeof(TestCode.Remoting.ObjetoRemoto),
					"tcp://127.0.0.1:9191/ObjetoRemoto");
				if( obj.Equals(null) )
				{
					System.Console.WriteLine("Error: no se puede localizar el servidor");
				}
				else
				{
					SampleClient client = new SampleClient();
					AsyncCallback fin_llamada_asincrona = new AsyncCallback(client.fin_contador);
					RemoteAsyncDelegate RemoteDel = new RemoteAsyncDelegate(obj.contador);
					while (true){
						Console.WriteLine("Empezar a contar");
						Console.ReadLine();
						IAsyncResult RemAr = RemoteDel.BeginInvoke(ref client.cuenta, fin_llamada_asincrona, null);
						
						// Esperar a que la llamada termine: Si se quita while ya no espera 
						int y = 0;
						while(!RemAr.IsCompleted){
				      //Console.WriteLine("Enviando petición al servidor, espere por favor: ");
							//Console.WriteLine((++y));
				      Thread.Sleep(10);
						}
					}
				}
			}
		}
}
