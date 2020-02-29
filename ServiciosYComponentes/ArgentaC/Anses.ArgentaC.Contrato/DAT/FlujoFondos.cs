using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anses.ArgentaC.Contrato
{
    [Serializable]
    public class FlujoFondos
    {
        public string Prestador { get; set; }
        public string CodSistema { get; set; }
        public string Sistema { get; set; }
        public int MensualCobranza { get; set; }
        public int CantCreditosCuilito { get; set; }
        public int CantCreditosTitular { get; set; }
        public decimal Amortizacion { get; set; }
        public decimal Intereses { get; set; }
        public decimal InteresCuotaCero { get; set; }
        public decimal GastoAdmin { get; set; }
        public decimal SeguroVida { get; set; }
        public decimal Total { get; set; }

        public FlujoFondos()
        { }

        public FlujoFondos(string _Prestador, string _CodSistema, string _Sistema, int _MensualCobranza, int _CantCreditosCuilito, int _CantCreditosTitular, decimal _Amortizacion, decimal _Intereses, 
                           decimal _InteresCuotaCero, decimal _GastoAdmin, decimal _SeguroVida, decimal _Total)
        {
            this.Prestador = _Prestador;
            this.CodSistema = _CodSistema;
            this.Sistema = _Sistema;
            this.MensualCobranza = _MensualCobranza;
            this.CantCreditosCuilito = _CantCreditosCuilito;
            this.CantCreditosTitular = _CantCreditosTitular;
            this.Amortizacion = _Amortizacion;
            this.Intereses = _Intereses;
            this.InteresCuotaCero = _InteresCuotaCero;
            this.GastoAdmin = _GastoAdmin;
            this.SeguroVida = _SeguroVida;
            this.Total = _Total;
        }

    }
}
