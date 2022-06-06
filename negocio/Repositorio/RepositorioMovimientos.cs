using redundancia.Database;
using redundancia.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace negocio.Repositorio
{
    public static class RepositorioMovimientos
    {
        public static IEnumerable<Movimiento> Enumerar(int clienteId, Cuenta cuenta)
        {
            var cuentas = RepositorioCuentas.Enumerar(clienteId);

            if(cuentas.Contains(cuenta))
                return new Repositorio<Movimiento>().Listar(e => e.CuentaId == cuenta.Id);

            throw new Exception($"No se encontró la cuenta {cuenta.Id} asociada al cliente {clienteId}");
        }

        public static Movimiento Obtener(int id)
        {
            return new Repositorio<Movimiento>().ObtenerPorId(id);
        }

        public static void Actualizar(Movimiento movimiento)
        {
            new Repositorio<Movimiento>().Actualizar(movimiento);
        }

        public static void Agregar(Movimiento Movimiento)
        {
            new Repositorio<Movimiento>().Agregar(Movimiento);
        }

        public static void Eliminar(Movimiento Movimiento)
        {
            new Repositorio<Movimiento>().Eliminar(Movimiento);
        }
    }
}
