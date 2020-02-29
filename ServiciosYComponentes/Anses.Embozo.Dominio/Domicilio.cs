using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.Embozo.Dominio
{    
    [Serializable]
    public class Domicilio
    {
        public string Calle { get; set; }
        public string Numero { get; set; }
        public string Piso { get; set; }
        public string Depto { get; set; }
        public string CodPostal { get; set; }
        public string Localidad { get; set; }
        public string Provincia { get; set; }
        public string Referencia { get; set; }
      
        public Domicilio() {}

        public Domicilio(string _Calle, string _Numero, string _Piso, string _Depto,string _CodPostal,string _Localidad, string _Provincia,string _Referencia)
        {
            Calle = _Calle;
            Numero = _Numero;
            Piso = _Piso;
            Depto = _Depto;
            CodPostal = _CodPostal;
            Localidad = _Localidad;
            Provincia = _Provincia;
            Referencia = _Referencia;
        }
    }
}
