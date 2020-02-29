using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public class TableroCobranza
    {
        public InformeDeCobranza InfCobranza { get; set; }
        public ReporteDeCobranza RepCobranza { get; set; }
        public InformePendientesDeCobro InfPendCobro { get; set; }

        public TableroCobranza()
        {
            InfCobranza = new InformeDeCobranza();
            RepCobranza = new ReporteDeCobranza();
            InfPendCobro = new InformePendientesDeCobro();
        }
    }
}
