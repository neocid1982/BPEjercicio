using redundancia.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace redundancia.Database
{
    public delegate object EntityDelegate(DataContext db);

    public class Repositorio<T> : IRepositorio<T> where T : Entidad, new()
    {
        #region CRUD

        public object Entity(EntityDelegate delegado)
        {
            var db = new DataContext();
            return delegado.Invoke(db);
        }

        public void Actualizar(T entidad)
        {
            using var db = new DataContext();
            db.Entry(entidad).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }

        public void Agregar(T entidad)
        {
            using var db = new DataContext();
            db.Entry(entidad).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();
        }

        public void Eliminar(T entidad)
        {
            using var db = new DataContext();
            db.Entry(entidad).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            db.SaveChanges();
        }

        public T ObtenerPorId(int id)
        {
            using var db = new DataContext();
            return db.Set<T>().FirstOrDefault(x => x.Id == id);
        }

        public T Buscar(Expression<Func<T, bool>> where)
        {
            using var db = new DataContext();
            return db.Set<T>().Where(where).FirstOrDefault();
        }

        public IEnumerable<T> Listar(Expression<Func<T, bool>> where)
        {
            IQueryable<T> retorno;
            using var db = new DataContext();

            retorno = db.Set<T>().Where(where);

            if (retorno.Any() == false)
                return new List<T>();

            return retorno.AsEnumerable<T>();
        }

        #endregion

    }
}
