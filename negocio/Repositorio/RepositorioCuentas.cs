using redundancia.Database;
using redundancia.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio.Repositorio
{

    public static class RepositorioCuentas
    {
        public static IEnumerable<Cuenta> Enumerar(int clienteId)
        {
            return new Repositorio<Cuenta>().Listar(e => e.ClienteId == clienteId);
        }

        public static Cuenta Obtener(int id)
        {
            return new Repositorio<Cuenta>().ObtenerPorId(id);
        }

        public static void Actualizar(Cuenta cuenta)
        {
            new Repositorio<Cuenta>().Actualizar(cuenta);
        }

        public static void Agregar(Cuenta cuenta)
        {
            new Repositorio<Cuenta>().Agregar(cuenta);
        }

        public static void Eliminar(Cuenta cuenta)
        {
            new Repositorio<Cuenta>().Eliminar(cuenta);
        }

        internal static bool ValidarRetiroDiarioCupo(Movimiento movimiento, out decimal topeDiarioCuenta)
        {
            var cuenta = RepositorioCuentas.Obtener(movimiento.CuentaId);
            topeDiarioCuenta = cuenta.CupoDiario;
            var movimientosDia = RepositorioMovimientos.DebitoDia(movimiento.CuentaId);
            return movimientosDia >= topeDiarioCuenta;
                
        }

        public static IEnumerable<Movimiento> MovimientosXRango(int id, DateTime? desde, DateTime? hasta)
        {
            return new Repositorio<Movimiento>().Listar(e => e.CuentaId == id && e.Fecha >= desde && e.Fecha <= hasta);
        }
    }
}
