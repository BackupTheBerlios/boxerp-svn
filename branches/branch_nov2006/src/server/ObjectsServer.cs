//
// Server.cs
//
// Authors:
// 	Carlos Ble Jurado <carlosble@shidix.com>
//
// Copyright (C) 2005,2006 Shidix Technologies (www.shidix.com)
// 
// Redistribution and use in source and binary forms, with or
// without modification, are permitted provided that the following
// conditions are met:
// Redistributions of source code must retain the above
// copyright notice, this list of conditions and the following
// disclaimer.
// Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following
// disclaimer in the documentation and/or other materials
// provided with the distribution.
// 
// THIS SOFTWARE IS PROVIDED BY THE AUTHOR ``AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
// PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR
// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
// EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
// TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
// LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING
// IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
// THE POSSIBILITY OF SUCH DAMAGE.

using System;
using System.IO;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Mono.Unix;
using System.Configuration;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Config;
//using Castle.Model;
//using NHibernate;
using Boxerp.Models;
using log4net;
using log4net.Config;

namespace Boxerp.Objects
{
	public class Server
	{
		public static void Main(string [] args)
		{
				//Catalog.Init("boxerp", Boxerp.Defines.LOCALE_DIR);
				XmlConfigurator.Configure(new System.IO.FileInfo("./server.config"));
				RemotingConfiguration.Configure("./serverRemoting.config");
				ActiveRecordStarter.Initialize( new XmlConfigurationSource("./activeRecord.xml"), 
						typeof(Action),
						typeof(Section),
						typeof(SectionPermission),
						typeof(Enterprise),
						typeof(ErrorCode),
						typeof(Group),
						typeof(Session),
						typeof(User));
				System.Console.WriteLine("----------");
				System.Console.WriteLine("     Server started and running... (press key to exit)");
				System.Console.ReadLine();
		}
	}
}

