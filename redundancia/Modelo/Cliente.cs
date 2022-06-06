using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace redundancia.Modelo
{
    public class Cliente:Persona
    {
        public string Contrasenia { get; set; }
        public bool Estado { get; set; }
    }
}
