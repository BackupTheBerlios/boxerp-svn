/* Linea de compilacion:
mcs -r:System.Data -r:Npgsql postgres1.cs

*/
using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
namespace TestCode.Remoting
{

 	public class ServerObject : MarshalByRefObject
	{
		int valueX = 0;
		
		// Constructor
		public ServerObject ()
		{
			valueX = 1; 
		}  
		////////////////////////////////////////////////////////////////////////////////////////////////

		public int get_value()
		{
			return(valueX);
		}

		public void set_value(int x)
		{
			valueX = x;
		}
		////////////////////////////////////////////////////////////////////////////////////////////////
	}

}
