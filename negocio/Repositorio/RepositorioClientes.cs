using redundancia.Database;
using redundancia.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace negocio.Repositorio
{
    public static class RepositorioClientes
    {
        public static IEnumerable<Cliente> Enumerar()
        {
            var lista = new Repositorio<Cliente>().Listar(e => e.Estado);

            /*Quita datos sensibles*/
            foreach (Cliente cliente in lista)
            {
                cliente.Contrasenia = null;
            }

            return lista;
        }

        public static Cliente Obtener(int id)
        {
            return new Repositorio<Cliente>().ObtenerPorId(id);
        }

        public static void Actualizar(Cliente cliente)
        {
            new Repositorio<Cliente>().Actualizar(cliente);
        }

        public static void Agregar(Cliente cliente)
        {
            new Repositorio<Cliente>().Agregar(cliente);
        }

        public static void Eliminar(Cliente cliente)
        {
            new Repositorio<Cliente>().Eliminar(cliente);
        }

        public static Cliente IniciarSesion(string identificacion, string clave)
        {
            if (string.IsNullOrEmpty(identificacion) || string.IsNullOrEmpty(clave))
                throw new ArgumentException("No se ha definido identificación o contraseña.");

            var cliente = BuscarXIdentidicacion(identificacion);

            if (cliente == null)
                throw new ArgumentException($"No se encuentra cliente {identificacion}");

            if (!cliente.Estado)
                throw new ArgumentException("Cliente inactivo");

            if (String.IsNullOrEmpty(cliente.Contrasenia))
                throw new ArgumentException("El cliente no tiene acceso");

            if (cliente.Contrasenia.Equals(clave))
                throw new ArgumentException("Contraseña no valida");

            return cliente;

        }

        public static Cliente BuscarXIdentidicacion(string identificacion)
        {
            return new Repositorio<Cliente>().Buscar(c => c.Identificacion.Equals(identificacion));
        }
    }
}
