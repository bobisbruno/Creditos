using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anses.ArgentaC.Contrato
{
    public class NovedadesPendientesDeAprobacionAgrupada
    {

        public int CantidadSinAprobar { get; set; }
        public DateTime MinimaFechaNovedad { get; set; }
        public DateTime MaxFechaNovedad { get; set; }

        public NovedadesPendientesDeAprobacionAgrupada()
        {
                
        }

        public NovedadesPendientesDeAprobacionAgrupada(int CantidadSinAprobar, DateTime MinimaFechaNovedad, DateTime MaxFechaNovedad)
        {
            this.CantidadSinAprobar = CantidadSinAprobar;
            this.MinimaFechaNovedad = MinimaFechaNovedad;
            this.MaxFechaNovedad = MaxFechaNovedad;
        }

    }
}
