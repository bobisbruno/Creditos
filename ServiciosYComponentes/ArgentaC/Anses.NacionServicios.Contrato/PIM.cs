using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anses.NacionServicios.Contrato
{
    public class resultadoConsultaCBU
    {
        public subscriber subscriber { get; set; }
        public string codeResult { get; set; }
        public string descResult { get; set; }

        public resultadoConsultaCBU() { }

        public resultadoConsultaCBU(subscriber _subscriber, string _codeResult, string _descResult)
        {
            this.subscriber = _subscriber;
            this.codeResult = _codeResult;
            this.descResult = _descResult;
        }
    }

    public class subscriber
    {
        public string CBU { get; set; }

        public subscriber() { }

        public subscriber(string cbu)
        {
            this.CBU = cbu;
        }
    }
}
