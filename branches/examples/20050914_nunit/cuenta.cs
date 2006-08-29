using System;

namespace Banco
{
	public class Cuenta
	{
		private float balance;
        public Cuenta(){}
        public void Deposito(float Cantidad)
        {
	        balance += Cantidad;
	    }
        public void Retiro(float Cantidad)
        {
			balance -= Cantidad;
		}
        public void Transferencia(Cuenta Destino, float Cantidad)
        {
        }
        public float Balance
        {        
	        get
	        {
		        return balance;
		    }
		}
	}
}
