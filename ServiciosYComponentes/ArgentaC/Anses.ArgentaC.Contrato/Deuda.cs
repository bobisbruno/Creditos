using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.ArgentaC.Contrato
{
    public class Deuda
    {
        public int idSistema { get; set; }
        public String Sistema { get; set; }
        public decimal ImporteDeuda { get; set; }

        public Deuda()
        { }

        public Deuda(int _idSistema, String _Sistema, decimal _ImporteDeuda)
        {
            this.idSistema = _idSistema;
            this.Sistema = _Sistema;
            this.ImporteDeuda = _ImporteDeuda;
        }
    }
}
