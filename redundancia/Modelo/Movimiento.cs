using System;
using System.Collections.Generic;
using static redundancia.Enumeradores;

namespace redundancia.Modelo
{
    public class Movimiento:Entidad
    {
        public int Id { get; set; }
        public int CuentaId { get; set; }
        public DateTime Fecha { get; set; }
        public EnumTipoMovimiento TipoMovimiento { get; set; }
        public decimal Valor { get; set; }
        public decimal Saldo { get; set; }

    }
}
