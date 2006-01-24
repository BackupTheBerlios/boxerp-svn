using System;
using System.Data;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using NUnit.Framework;
using Boxerp.Facade;
using Boxerp.Tools;

namespace Boxerp.Test
{

	[TestFixture]
	public class AdminTest
	{
		AdminFacade adminFacade;
		TcpChannel chan;
		string serveraddress = "tcp://localhost:9191/";
		
		public AdminTest() 
		{
			chan = new TcpChannel();
			ChannelServices.RegisterChannel(chan);

			adminFacade = (AdminFacade) Activator.GetObject(
			typeof(Boxerp.Facade.AdminFacade), serveraddress + "AdminFacade");
			if (adminFacade.Equals(null))
			{
				throw new System.NullReferenceException("Imposible Conectar");
			}
		}
		////////////////////////////////////////////////////////////////
		
		[Test]
		public void ShowEnterprises()
		{
			DataSet ds = adminFacade.GetEnterprisesList(0,0);
			DataRow[] rows = DataTools.GetRows(ds);
			for (int i = 0; i < rows.Length; i++)
			{
				Console.WriteLine("Id Empresa: {0}", rows[i][0]);
				Console.WriteLine("Nombre: {0}",     rows[i][1]);
				Console.WriteLine("Description: {0}",rows[i][2]);
				Console.WriteLine("Published: {0}",  rows[i][3]);
				Console.WriteLine("--------------------------------------");
			}
		}
		///////////////////////////////////////////////////////////////
		
		[Test]
		public void ShowGroups()
		{
			string epriseName = "Pruebas";
			DataSet ds = adminFacade.GetGroupsList(epriseName, 0,0);
			DataRow[] rows = DataTools.GetRows(ds);
			for (int i = 0; i < rows.Length; i++)
			{
				Console.WriteLine("Id Grupo: {0}", rows[i][0]);
				Console.WriteLine("Nombre: {0}",   rows[i][1]);
				Console.WriteLine("Published: {0}",rows[i][2]);
				Console.WriteLine("--------------------------------------");
			}
		}
		////////////////////////////////////////////////////////////////
		
	}
}
