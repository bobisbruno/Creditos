using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Anses.ArgentaC.Contrato
{
    public class Libreta
    {
        public long Cuil { get; set; }
        public int? AnioUltimaLiquidacion { get; set; }
        public int? AnioUltimaLibretaPresentada { get; set; }

        public Libreta(int _Cuil, int? _AnioUltimaLiquidacion, int? _AnioUltimaLibretaPresentada)
        {
            this.Cuil = _Cuil;
            this.AnioUltimaLiquidacion = _AnioUltimaLiquidacion;
            this.AnioUltimaLibretaPresentada = _AnioUltimaLibretaPresentada;
        }
    }
}
