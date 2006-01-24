using System;
using NUnit.Framework;

namespace Banco
{
	[TestFixture] // Establecemos el atributo que índica la clase que contiene las pruebas.
	public class CuentaTest
	{
		public CuentaTest(){}
		[Test] //Especificamos cuál es el método de prueba.
		public void Transferencias()
		{
			//creamos la cuenta origen con un saldo de 200.00
			Cuenta origen = new Cuenta();
			origen.Deposito(200.00F);
			
			//creamos la cuenta destino con un saldo de 150.00
			Cuenta destino = new Cuenta();
			destino.Deposito(150.00F);
			
			//transferimos 100.00 de la cuenta origen a la destino
			origen.Transferencia(destino, 100.00F);
			
			//sí todo ha salido bien, debemos tener
			//un balance de 250.00 en la cuenta destino
			//y de 100.00 en la origen
			Assert.AreEqual(250.00F, destino.Balance);
			Assert.AreEqual(100.00F, origen.Balance);
		}
	}
}

