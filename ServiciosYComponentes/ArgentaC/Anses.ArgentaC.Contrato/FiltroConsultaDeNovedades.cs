using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anses.ArgentaC.Contrato
{
    public class FiltroConsultaDeNovedades
    {
        public long Mensual { get; set; }
        public long Cuil {get;set;} 
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string Oficina { get; set; }
        public long? IdNovedad { get; set; }
        public string Usuario { get; set; }
    }
}
