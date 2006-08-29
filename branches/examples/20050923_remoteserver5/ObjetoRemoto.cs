using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Messaging;
namespace TestCode.Remoting
{

 	public class ObjetoRemoto : MarshalByRefObject
	{

		// Constructor
		public ObjetoRemoto (){}  
		///////////////////////////////////////////////////////////////////////////////////////////////////	
	
		public int contador(ref long cuenta)
		{
			int x = 0;
			while (x < 99999)
			{
				System.Threading.Interlocked.Increment(ref x);
				Console.WriteLine("X = {0}", x);
			}
			cuenta += 10;
			return (0);
		}
	
	}

}
