using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.ArgentaC.Contrato
{
    public class Mensaje
    {
        public int? ID { get; set; }
        public string Descripcion { get; set; }

        public Mensaje()
        {
            this.ID = null;
            this.Descripcion = "";
        }

        public Mensaje(int _id, string _mensaje)
        {
            this.ID = _id;
            this.Descripcion = _mensaje;
        }
    }
}
