using System;

delegate void D(int valor);

class EjemploDelegado
{
	public string Nombre;

	EjemploDelegado(string nombre)
	{
		Nombre = nombre;
	}

	public static void Main()
	{
		EjemploDelegado obj1 = new EjemploDelegado("obj1");
		D objDelegado = new D(f);

		objDelegado +=  new D(obj1.g);
		objDelegado(3);	
		objDelegado -= new D(obj1.g);
		objDelegado(5);
	}

	public void g(int x)
	{
		Console.WriteLine("Pasado valor {0} a g() en objeto {1}", x, Nombre);
	}

	public static void f(int x)	
	{
		Console.WriteLine( "Pasado valor {0} a f()", x);
	}
}
