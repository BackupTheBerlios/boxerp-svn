// created on 12/10/2006 at 13:57
using Boxerp.Client;
using System;

	[AttributeUsage(AttributeTargets.Method, AllowMultiple=false)]
	public class ResponsiveAttribute : Attribute
	{
		private ResponsiveEnum respType;
	
		public ResponsiveEnum RespType
		{
			get { return respType; }
			set { respType = value; }
		}
		
		public ResponsiveAttribute(ResponsiveEnum rType)
		{
			respType = rType;
		}
	}