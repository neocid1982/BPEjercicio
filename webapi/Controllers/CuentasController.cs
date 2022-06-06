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
            var cuenta = RepositorioCuentas.Obtener(id);

            if (cuenta == null)
                return NotFound($"No se encontró cuenta {id}");

            return Ok(cuenta);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Cuenta cuenta)
        {
            if (id != cuenta.Id)
                return BadRequest();

            var cuentaDb = RepositorioCuentas.Obtener(id);

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

            var cuentaDb = RepositorioCuentas.Obtener(id);

            if (cuentaDb == null)
                return NotFound($"No se encontró cuenta {id}");

            RepositorioCuentas.Eliminar(cuentaDb);

            return CreatedAtAction("Get", new { id = id });

        }

    }
}
