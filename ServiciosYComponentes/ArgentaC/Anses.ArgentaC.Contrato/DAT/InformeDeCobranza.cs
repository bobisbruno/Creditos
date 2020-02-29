using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public enum enum_SistemaApropiador { SUAF = 14, AUH = 60 }

    [Serializable]
    public class InformeDeCobranza
    {
        public List<Cobranza> listaCobranzas { get; set; }

        public InformeDeCobranza()
        {
            listaCobranzas = new List<Cobranza>();
        }

        public InformeDeCobranza(List<Cobranza> _lista)
        {
            this.listaCobranzas = _lista;
        }
    }

    [Serializable]
    public class Cobranza
    {
        public int Mensual { get; set; }
        public string CodConceptoOriginante { get; set; }
        public int SistemaApropiador { get; set; }
        public long CantCreditos { get; set; }
        public decimal MontoCuotaTotal { get; set; }
        public decimal Amortizacion { get; set; }
        public decimal Intereses { get; set; }
        public decimal GastoAdministrativo { get; set; }
        public decimal SeguroVida { get; set; }
        public decimal InteresCuotaCero { get; set; }
        public decimal MontoCuotaSinRedondeo { get; set; }

        public Cobranza()
        { }

        public Cobranza(int _mensual, string _codConceptoOriginante, int _sistemaApropiador, long _cantCreditos, decimal _montoCuotaTotal, decimal _amortizacion, decimal _intereses, decimal _gastoAdministrativo, decimal _seguroVida, decimal _interesCuotaCero, decimal _montoCuotaSinRedondeo)
        {
            this.Mensual = _mensual;
            this.CodConceptoOriginante = _codConceptoOriginante;
            this.SistemaApropiador = _sistemaApropiador;
            this.CantCreditos = _cantCreditos;
            this.MontoCuotaTotal = _montoCuotaTotal;
            this.Amortizacion = _amortizacion;
            this.Intereses = _intereses;
            this.GastoAdministrativo = _gastoAdministrativo;
            this.SeguroVida = _seguroVida;
            this.InteresCuotaCero = _interesCuotaCero;
            this.MontoCuotaSinRedondeo = _montoCuotaSinRedondeo;
        }
    }
}
