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
		public static void Main(string [] args)
		{
			TcpChannel chan = new TcpChannel();
			ChannelServices.RegisterChannel(chan);
	
			ObjetoRemoto obj = (ObjetoRemoto) Activator.GetObject(
				typeof(TestCode.Remoting.ObjetoRemoto),
					"tcp://127.0.0.1:9191/ObjetoRemoto");
			try
			{
				long x = 0;
				obj.contador(ref x);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Capturada excepcion: {0}", ex.Message);
			}
		}
	}
}
