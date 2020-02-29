using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.ArgentaC.Contrato
{
[Serializable]
    public class Contacto
    {
        public string Mail { get; set; } 
        public string TelefonoFijo { get; set; }
        public string TelefonoCelular { get; set; }

        public Contacto() { }

        public Contacto(string mail, string telefonoFijo, string telefonoCelular)
        {
            this.Mail = mail;
            this.TelefonoFijo = telefonoFijo;
            this.TelefonoCelular = telefonoCelular;
        }
    }
}
