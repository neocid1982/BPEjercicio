using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace redundancia
{
    public static class Enumeradores
    {

        public enum EnumGeneroPersona
        {
            Ninguno = ' ',
            Masculino = 'M',
            Femenino = 'F'
        }

        public enum EnumTipoCuenta
        {
            Ninguno = ' ',
            Corriente = 'C',
            Ahorros = 'A'
        }

        public enum EnumTipoMovimiento
        {
            Credito = 'C',
            Debito = 'D'
        }
    }
}
