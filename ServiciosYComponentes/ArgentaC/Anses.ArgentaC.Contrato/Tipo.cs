using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public class Tipo
    {
        public string ID { get; set; }
        public string Descripcion { get; set; }


        public Tipo() { }

        public Tipo(string id, string descripcion)
        {
            this.ID = id;
            this.Descripcion = descripcion;
        }
    }
}
