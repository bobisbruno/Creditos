using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Activos_ServiciosComplementarios.Entidades
{
    [Serializable]
    public class DatosDeConsultaCondenadoProcesado
    {
        public decimal Cuil {get;set;}
        public DateTime FechaCarga {get;set;}
        public short CodigoEstado {get;set;}
        public decimal PeriodoLiquidacion { get; set; }
        public string TipoInterno {get;set;}
        public Nullable<decimal> Cuit { get; set; }

    }
}
