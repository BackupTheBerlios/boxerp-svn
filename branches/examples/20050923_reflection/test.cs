using
System;
using System.Collections;
using System.Security.Principal;
using System.Reflection;

public
class P
{
	public P()
	{
	}
	
	public int Suma(int a, int b)
	{
		return a + b;
	}
}

class Prueba
{
	public static void Main()
	{
		P miP = new P();
		Type t = miP.GetType();
		Object[] arr = new Object[] {2, 2};
		Object o = t.InvokeMember("Suma", BindingFlags.InvokeMethod, null, miP, arr);
		Console.WriteLine( "lalalalal --> {0}", (int)o );
	}

}
