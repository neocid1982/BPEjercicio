using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static redundancia.Enumeradores;

namespace redundancia.Modelo
{
    public class Cuenta:Entidad
    {
        public int ClienteId { get; set; }
        public string NumeroCuenta { get; set; }
        public EnumTipoCuenta TipoCuenta { get; set; }
        public decimal SaldoInicial { get; set; }
        public bool Estado { get; set; }
        public decimal CupoDiario { get; set; }
    }
}
