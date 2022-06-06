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
    public class MovimientosController : ControllerBase
    {
        private int ClienteId
        {
            get
            {
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

        [HttpGet("/{cuenta}")]
        public IActionResult Get(Cuenta cuenta)
        {
            return Ok(RepositorioMovimientos.Enumerar(ClienteId, cuenta));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var cliente = RepositorioMovimientos.Obtener(id);

            if (cliente == null)
                return NotFound($"No se encontró cliente {id}");

            return Ok(cliente);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Movimiento movimiento)
        {
            if (id != movimiento.Id)
                return BadRequest();

            var MovimientoDb = RepositorioMovimientos.Obtener(id);

            if (MovimientoDb == null)
                return NotFound($"No se encontró movimiento {id}");

            try
            {

                RepositorioMovimientos.Actualizar(movimiento);

                return CreatedAtAction("Get", new { id = movimiento.Id }, movimiento);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex);
            }

        }

        [HttpPost]
        public IActionResult Post(Movimiento movimiento)
        {
            RepositorioMovimientos.Agregar(movimiento);

            return CreatedAtAction("Get", new { id = movimiento.Id }, movimiento);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            var movimientoDb = RepositorioMovimientos.Obtener(id);

            if (movimientoDb == null)
                return NotFound($"No se encontró movimiento {id}");

            RepositorioMovimientos.Eliminar(movimientoDb);

            return CreatedAtAction("Get", new { id });

        }

    }
}
