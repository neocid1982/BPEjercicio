using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace redundancia.Database
{
    public interface IRepositorio<T>
    {
        void Agregar(T entidad);
        void Eliminar(T entidad);
        void Actualizar(T entidad);
        T ObtenerPorId(int id);
        IQueryable<T> Listar(ParametrosQuery<T> parametrosQuery);
    }
}