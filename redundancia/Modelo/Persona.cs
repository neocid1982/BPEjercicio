using System;
using System.Collections.Generic;
using System.Linq;
using static redundancia.Enumeradores;

namespace redundancia.Modelo
{
    public class Persona:Entidad
    {
        public string Nombres { get; set; }
        public EnumGeneroPersona Genero { get; set; }
        public int Edad { get; set; }
        public string Identificacion { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

    }
}
