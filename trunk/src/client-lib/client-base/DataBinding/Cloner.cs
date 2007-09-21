using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Boxerp.Client
{
	public static class Cloner
	{
		public static object Clone(object source)
		{
			using (MemoryStream buffer = new MemoryStream())
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(buffer, source);
				buffer.Position = 0;
				object clone = formatter.Deserialize(buffer);
				return clone;
			}
		}
	}
}
