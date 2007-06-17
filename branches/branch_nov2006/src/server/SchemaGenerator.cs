using System;
using System.Configuration;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Config;
//using Castle.Model;
//using NHibernate;
using Boxerp.Models;

public class SchemaGenerator
{
	public static void Main(){
		
		ActiveRecordStarter.Initialize( new XmlConfigurationSource("activeRecord.xml"), 
						typeof(Action),
						typeof(Section),
						typeof(SectionPermission),
						typeof(Enterprise),
						typeof(ErrorCode),
						typeof(Group),
						typeof(Session),
						typeof(User));
		ActiveRecordStarter.CreateSchema();
	}

}

