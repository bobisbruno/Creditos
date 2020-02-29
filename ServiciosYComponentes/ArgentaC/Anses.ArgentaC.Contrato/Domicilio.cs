using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public class Domicilio
    {
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Piso { get; set; }
        public string Depto { get; set; }
        public string Codigo_Postal { get; set; }
        public string Localidad { get; set; }
        public Tipo Provincia { get; set; }

        public Domicilio() { }

        public Domicilio(string calle, string numero, string piso, string depto, string codigo_postal, Tipo provincia, string localidad)
        {
            this.Calle = calle;
            this.Numero = numero;
            this.Piso = piso;
            this.Depto = depto;
            this.Codigo_Postal = codigo_postal;
            this.Localidad = localidad;
            this.Provincia = provincia;
        }

    }

    
}
