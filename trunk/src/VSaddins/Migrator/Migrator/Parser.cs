using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Migrator
{
	public class Parser
	{

		public static bool IsOpenBracket(string line)
		{
			Regex regex = new Regex("{");
			return regex.IsMatch(line);
		}

		public static bool IsClassDefinition(string line)
		{
			Regex regex = new Regex(@"public\s+partial\s+class");
			return regex.IsMatch(line);
		}

		public static bool IsPublicMethodDefinition(string line)
		{
			Regex regex = new Regex(@"public\s+\w+\s+\w+\s*\(");
			return regex.IsMatch(line);
		}

		public static bool IsNamesSpaceDefinition(string line)
		{
			Regex regex = new Regex("namespace ");
			return regex.IsMatch(line);
		}

		public static string GetNamesSpace(string line)
		{
			return line.Substring(line.IndexOf("namespace") + 10);
		}
	}
}
