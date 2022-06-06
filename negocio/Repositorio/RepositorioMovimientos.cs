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

        /*Reglas de negocio para movimientos*/
        public static void Agregar(Movimiento movimiento)
        {
            movimiento = ReglasNegocio(movimiento);
            new Repositorio<Movimiento>().Agregar(movimiento);
        }

        public static void Eliminar(Movimiento Movimiento)
        {
            new Repositorio<Movimiento>().Eliminar(Movimiento);
        }

        internal static decimal DebitoDia(int cuentaId)
        {
            var hoy = DateTime.Now.Date.ToString();
            decimal debito = 0;
            try
            {
                debito = new Repositorio<Movimiento>().Listar(e => e.CuentaId == cuentaId
                    && e.TipoMovimiento == redundancia.Enumeradores.EnumTipoMovimiento.Debito
                    && e.Fecha.Date.Equals(hoy)).Sum(e => e.Valor);
            }catch(Exception)
            {
                //No halla movimientos
            }

            return debito;

        }

        private static Movimiento ReglasNegocio(Movimiento movimiento)
        {
            if (movimiento.TipoMovimiento == redundancia.Enumeradores.EnumTipoMovimiento.Debito)
            {
                /*Los valores cuando son crédito son positivos, y los débitos son negativos. Debe 
                almacenarse el saldo disponible en cada transacción dependiendo del tipo de movimiento. 
                (suma o resta)*/
                if (movimiento.Valor > 0)
                    movimiento.Valor *= -1;

                /*Si el saldo es cero, y va a realizar una transacción débito, debe desplegar mensaje “Saldo no disponible” */
                if (movimiento.Saldo <= 0)
                    throw new InvalidOperationException("Saldo no disponible");

                /*Se debe tener un parámetro de limite diario de retiro (valor tope 1000$)*/
                /*Si el cupo disponible ya se cumplió no debe permitir realizar un debito y debe desplegar un mensaje “Cupo diario Excedido” */
                if (Repositorio.RepositorioCuentas.ValidarRetiroDiarioCupo(movimiento, out decimal topeDiarioCuenta))
                    throw new InvalidOperationException($"Cupo diario Excedido $ {topeDiarioCuenta}");
            }

            return movimiento;
        }
    }
}
