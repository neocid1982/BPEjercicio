using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using negocio.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace webapi.Seguridad
{
    public class AutenticacionClienteHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        public AutenticacionClienteHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock){
            }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("Authorization no encontrado en cabecera");

            try
            {
                var autenticacionCabecera = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

                string[] autorizacion = Encoding.UTF8.GetString(Convert.FromBase64String(autenticacionCabecera.Parameter)).Split(":");
                var usuario = autorizacion[0];
                var clave = autorizacion[1];

                var cliente = RepositorioClientes.IniciarSesion(usuario, clave);

                if (cliente == null)
                    throw new ArgumentException("Cliente no encontrado en el sistema");

                var claims = new[] { new Claim(ClaimTypes.Name, cliente.Id.ToString()) };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                AuthenticateResult.Success(ticket);
            }
            catch(ArgumentException ex)
            {
                return AuthenticateResult.Fail(ex);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail("Authorization no valida");
            }

            return AuthenticateResult.Fail("Falta implementar");
        }
    }
}
