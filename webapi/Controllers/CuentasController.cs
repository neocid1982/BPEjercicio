using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using negocio.Repositorio;
using redundancia.Modelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webapi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private int ClienteId
        {
            get {
                try
                {
                    return Convert.ToInt32(HttpContext.User.Identity.Name);
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(RepositorioCuentas.Enumerar(ClienteId));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var cuenta = RepositorioMovimientos.Obtener(id);

            if (cuenta == null)
                return NotFound($"No se encontró cuenta {id}");

            return Ok(cuenta);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Cuenta cuenta)
        {
            if (id != cuenta.Id)
                return BadRequest();

            var cuentaDb = RepositorioMovimientos.Obtener(id);

            if (cuentaDb == null)
                return NotFound($"No se encontró cuenta {id}");

            try
            {

                RepositorioCuentas.Actualizar(cuenta);

                return CreatedAtAction("Get", new { id = cuenta.Id }, cuenta);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpPost]
        public IActionResult Post(Cuenta cuenta)
        {
            RepositorioCuentas.Agregar(cuenta);

            return CreatedAtAction("Get", new { id = cuenta.Id }, cuenta);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            var cuentaDb = RepositorioMovimientos.Obtener(id);

            if (cuentaDb == null)
                return NotFound($"No se encontró cuenta {id}");

            RepositorioMovimientos.Eliminar(cuentaDb);

            return CreatedAtAction("Get", new { id });

        }

        /*Generar reporte (Estado de cuenta) especificando un rango de fechas y un cliente, 
            visualice las cuentas asociadas con sus respectivos saldos y el total de débitos y créditos 
            realizados durante las fechas de ese cliente. (resultado en Json) */
        [HttpGet("/reporte/{id},{fecha}")]
        public IActionResult Reporte(int id, string fecha)
        {
            if (string.IsNullOrEmpty(fecha))
                return BadRequest("Se requiere el parametro fecha (desde,hasta)");

            if(!fecha.Contains(','))
                return BadRequest("Se requiere rango de fecha (desde,hasta)");

            var rango = fecha.Split(',');

            DateTime? desde = Convert.ToDateTime(rango[0]);

            if(desde == null)
                return BadRequest("Rango de inicio no es fecha valida");

            DateTime? hasta = Convert.ToDateTime(rango[1]);
            
            if (hasta == null)
                return BadRequest("Rango de fin no es fecha valida");

            return Ok(RepositorioCuentas.MovimientosXRango(id, desde, hasta));
        }
    }
}
