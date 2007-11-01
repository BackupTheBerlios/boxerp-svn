using System;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Configuration;
using Boxerp.Client;

namespace RemotingTestServer
{
	public class Server
	{
		public static void Main(string [] args)
		{
				RemotingConfiguration.Configure("..\\..\\serverRemoting.config");
				
				System.Console.WriteLine("----------");
				System.Console.WriteLine("     Server started and running... (press key to exit)");
				System.Console.ReadLine();
		}
	}
}

