using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public class MensajeBDD
    {
        public long CuilRelacionado { get; set; }
        public string Mensaje { get; set; }
        public bool Salvable { get; set; }
        public MensajeBDD() { }

        public MensajeBDD(long _cuilRelacionado, string _mensaje, bool _salvable)
        {
            this.CuilRelacionado = _cuilRelacionado;
            this.Mensaje = _mensaje;
            this.Salvable = _salvable;
        }
    }
}
