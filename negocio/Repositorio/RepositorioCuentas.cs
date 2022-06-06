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
    }
}
