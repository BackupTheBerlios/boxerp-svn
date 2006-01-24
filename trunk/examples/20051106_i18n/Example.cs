// Secuencia de pasos a seguir:
// xgettext -o i18n.pot Example.cs
// msginit -l es -i i18n.pot
// Traducir las cadenas
// msgfmt es.po -o i18n.mo
// mkdir -p locale/es/LC_MESSAGES 
// mv i18n.mo locale/es/LC_MESSAGES
// export LANGUAGE=es
// mcs -r:Mono.Posix Example.cs
// mono ./Example.exe

using System;
using Mono.Unix;
 
public class i18n
{
	public static void Main(string[] argv)
	{
		Catalog.Init("i18n","./locale");
		Console.WriteLine(Catalog.GetString("My name is") + " Carlos");
		int i = 24;
		Console.WriteLine(Catalog.GetPluralString("I'm {0} year old.","I'm {0} years old.",i),i);
	}
}
