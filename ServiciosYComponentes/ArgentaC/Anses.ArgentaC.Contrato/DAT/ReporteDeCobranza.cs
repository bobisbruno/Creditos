using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public class ReporteDeCobranza
    {
        public List<CobranzaReporte> listaCobranzas { get; set; }

        public ReporteDeCobranza()
        {
            listaCobranzas = new List<CobranzaReporte>();
        }

        public ReporteDeCobranza(List<CobranzaReporte> _lista)
        {
            this.listaCobranzas = _lista;
        }
    }

    [Serializable]
    public class CobranzaReporte
    {
        public int Mensual { get; set; }
        //public int SistemaOriginante { get; set; }
        public string SistemaOriginante { get; set; }
        public long DV_CantCreditos { get; set; }
        public decimal DV_Importe { get; set; }
        public long PANT_CantCreditos { get; set; }
        public decimal PANT_Importe { get; set; }
        public long F_CantCreditos { get; set; }
        public decimal F_Importe { get; set; }
        public long A_LIQ_CantCreditos { get; set; }
        public decimal A_LIQ_Importe { get; set; }
        public long AHU_CantCreditos { get; set; }
        public decimal AHU_Importe { get; set; }
        public long SUAF_CantCreditos { get; set; }
        public decimal SUAF_Importe { get; set; }
        public long PEND_CantCreditos { get; set; }
        public decimal PEND_Importe { get; set; }

        public CobranzaReporte()
        { }

        //public CobranzaReporte(int _mensual, int _sistemaOriginante, long _devengado_CantCreditos, decimal _devengado_Importe, long _pendientePeriodoAnterior_CantCreditos, decimal _pendientePeriodoAnterior_Importe, long _fallecidos_CantCreditos, decimal _fallecidos_Importe, long _totalLiquidado_CantCreditos, decimal _totalLiquidado_Importe, long _cobradosEnAUH_CantCreditos, decimal _cobradosEnAUH_Importe, long _cobradosEnSUAF_CantCreditos, decimal _cobradosEnSUAF_Importe, long _pendienteDeCobro_CantCreditos, decimal _pendienteDeCobro_Importe)
        public CobranzaReporte(int _mensual, string _sistemaOriginante, long _devengado_CantCreditos, decimal _devengado_Importe, long _pendientePeriodoAnterior_CantCreditos, decimal _pendientePeriodoAnterior_Importe, long _fallecidos_CantCreditos, decimal _fallecidos_Importe, long _totalLiquidado_CantCreditos, decimal _totalLiquidado_Importe, long _cobradosEnAUH_CantCreditos, decimal _cobradosEnAUH_Importe, long _cobradosEnSUAF_CantCreditos, decimal _cobradosEnSUAF_Importe, long _pendienteDeCobro_CantCreditos, decimal _pendienteDeCobro_Importe)

        {
            this.Mensual = _mensual;
            this.SistemaOriginante = _sistemaOriginante;
            this.DV_CantCreditos = _devengado_CantCreditos;
            this.DV_Importe = _devengado_Importe;
            this.PANT_CantCreditos = _pendientePeriodoAnterior_CantCreditos;
            this.PANT_Importe = _pendientePeriodoAnterior_Importe;
            this.F_CantCreditos = _fallecidos_CantCreditos;
            this.F_Importe = _fallecidos_Importe;
            this.A_LIQ_CantCreditos = _totalLiquidado_CantCreditos;
            this.A_LIQ_Importe = _totalLiquidado_Importe;
            this.AHU_CantCreditos = _cobradosEnAUH_CantCreditos;
            this.AHU_Importe = _cobradosEnAUH_Importe;
            this.SUAF_CantCreditos = _cobradosEnSUAF_CantCreditos;
            this.SUAF_Importe = _cobradosEnSUAF_Importe;
            this.PEND_CantCreditos = _pendienteDeCobro_CantCreditos;
            this.PEND_Importe = _pendienteDeCobro_Importe;
        }
    }
}
