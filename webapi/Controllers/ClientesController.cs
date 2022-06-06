using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using negocio.Repositorio;
using redundancia.Modelo;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        // GET: api/Clientes
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(RepositorioClientes.Enumerar());
        }

        // GET: api/Clientes/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var cliente = RepositorioClientes.Obtener(id);

            if (cliente == null)
                return NotFound($"No se encontró cliente {id}");

            return Ok(cliente);    
        }

        // PUT: api/Clientes/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Cliente cliente)
        {
            if (id != cliente.Id)
                return BadRequest();

            var clienteDb = RepositorioClientes.Obtener(id);

            if (clienteDb == null)
                return NotFound($"No se encontró cliente {id}");

            try
            {

                RepositorioClientes.Actualizar(cliente);

                return CreatedAtAction("Get", new { id = cliente.Id }, cliente);

            }
            catch (DbUpdateConcurrencyException ex)
            {
                return BadRequest(ex);
            }

        }

        // POST: api/Clientes
        [HttpPost]
        public IActionResult Post(Cliente cliente)
        {
            RepositorioClientes.Agregar(cliente);

            return CreatedAtAction("Get", new { id = cliente.Id }, cliente);
        }

        // DELETE: api/Clientes/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            var clienteDb = RepositorioClientes.Obtener(id);

            if (clienteDb == null)
                return NotFound($"No se encontró cliente {id}");

            RepositorioClientes.Eliminar(clienteDb);

            return CreatedAtAction("Get", new { id });

        }

        public IActionResult Autorizar(string identificacion)
        {
            var cliente = RepositorioClientes.BuscarXIdentidicacion(identificacion);

            if (cliente == null)
                return NotFound($"Cliente {identificacion} no encontrado");

            if (string.IsNullOrEmpty(cliente.Contrasenia))
                return ValidationProblem($"Cliente {identificacion}, no tiene contraseña definida");

            try
            {

                var bytes = $"{cliente.Identificacion}:{cliente.Contrasenia}".ToCharArray();

                var autenticacion = Convert.FromBase64CharArray(bytes, 0, bytes.Length);

                return Ok($"Basic {autenticacion}");

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
