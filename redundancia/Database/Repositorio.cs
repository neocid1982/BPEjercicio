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
            //try
            //{
                var db = new DataContext();
                return delegado.Invoke(db);
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    string error = "";
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            error += $"{validationError.ErrorMessage};\r\n";
            //        }
            //    }
            //    throw new Exception(error);
            //}
        }

        public void Actualizar(T entidad)
        {
            //try
            //{
                using (var db = new DataContext())
                {
                    db.Entry(entidad).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                }
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    string error = "";
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            error += $"{validationError.ErrorMessage};\r\n";
            //        }
            //    }
            //    throw new Exception(error);
            //}
        }

        public void Agregar(T entidad)
        {
            //try
            //{
                using (var db = new DataContext())
                {
                    db.Entry(entidad).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    db.SaveChanges();
                }
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    string error = "";
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            error += $"{validationError.ErrorMessage};\r\n";
            //        }
            //    }
            //    throw new Exception(error);
            //}
        }

        public void Eliminar(T entidad)
        {
            //try
            //{
                using (var db = new DataContext())
                {
                    db.Entry(entidad).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    db.SaveChanges();
                }
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    string error = "";
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            error += $"{validationError.ErrorMessage};\r\n";
            //        }
            //    }
            //    throw new Exception(error);
            //}
        }

        public T ObtenerPorId(int id)
        {
            //try
            //{
                using (var db = new DataContext())
                {
                    return db.Set<T>().FirstOrDefault(x => x.Id == id);
                }
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    string error = "";
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            error += $"{validationError.ErrorMessage};\r\n";
            //        }
            //    }
            //    throw new Exception(error);
            //}
        }

        public T Buscar(Expression<Func<T, bool>> where)
        {
            //try
            //{
                using (var db = new DataContext())
                {
                    return db.Set<T>().Where(where).FirstOrDefault();
                }
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    string error = "";
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            error += $"{validationError.ErrorMessage};\r\n";
            //        }
            //    }
            //    throw new Exception(error);
            //}
        }

        public IEnumerable<T> Listar(Expression<Func<T, bool>> where)
        {
            //try
            //{
                IQueryable<T> retorno;
                using (var db = new DataContext())
                {
                
                    retorno = db.Set<T>().Where(where);

                    if (retorno.Any() == false)
                        return new List<T>();

                    return retorno.AsEnumerable<T>();
                }
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    string error = "";
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            error += $"{validationError.ErrorMessage};\r\n";
            //        }
            //    }
            //    throw new Exception(error);
            //}
        }

        public IQueryable<T> Listar(ParametrosQuery<T> parametrosQuery)
        {
            //try
            //{
                var orderByClass = ObtenerOrderBy(parametrosQuery);
                Expression<Func<T, bool>> whereTrue = x => true;
                var where = parametrosQuery.Where ?? whereTrue;
                using (var db = new DataContext())
                {
                    if (orderByClass.IsAscending)
                    {
                        return db.Set<T>().Where(where).OrderBy(orderByClass.OrderBy)
                        .Skip((parametrosQuery.Pagina - 1) * parametrosQuery.Top)
                        .Take(parametrosQuery.Top).AsQueryable();
                    }
                    else
                    {
                        return db.Set<T>().Where(where).OrderByDescending(orderByClass.OrderBy)
                        .Skip((parametrosQuery.Pagina - 1) * parametrosQuery.Top)
                        .Take(parametrosQuery.Top).AsQueryable();
                    }

                }
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    string error = "";
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            error += $"{validationError.ErrorMessage};\r\n";
            //        }
            //    }
            //    throw new Exception(error);
            //}
        }

        private OrderByClass ObtenerOrderBy(ParametrosQuery<T> parametrosDeQuery)
        {
            //try
            //{
                if (parametrosDeQuery.OrderBy == null && parametrosDeQuery.OrderByDescending == null)
                {
                    return new OrderByClass(x => x.Id, true);
                }

                return (parametrosDeQuery.OrderBy != null)
                    ? new OrderByClass(parametrosDeQuery.OrderBy, true) :
                    new OrderByClass(parametrosDeQuery.OrderByDescending, false);
            //}
            //catch (DbEntityValidationException dbEx)
            //{
            //    string error = "";
            //    foreach (var validationErrors in dbEx.EntityValidationErrors)
            //    {
            //        foreach (var validationError in validationErrors.ValidationErrors)
            //        {
            //            error += $"{validationError.ErrorMessage};\r\n";
            //        }
            //    }
            //    throw new Exception(error);
            //}
        }

        
        #endregion

        private class OrderByClass
        {

            public OrderByClass()
            {

            }

            public OrderByClass(Func<T, object> orderBy, bool isAscending)
            {
                OrderBy = orderBy;
                IsAscending = isAscending;
            }


            public Func<T, object> OrderBy { get; set; }
            public bool IsAscending { get; set; }
        }
    }
}
