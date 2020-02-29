using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
     public enum enum_enviadoLiquidar { 
        P = 'P', 
        S = 'S', 
        N = 'N',
        B = 'B'
    }

     public enum enum_IdenPago
     {
         PA = 'P',
         IM = 'I',
         RE = 'R',
         AS = 'A',
         SI = 'S',
         NL = 'N'
     }

    [Serializable]
    public class Cuota : IDisposable
    {
        #region Dispose

        private bool disposing;

        public void Dispose()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        protected virtual void Dispose(bool b)
        {
            // Si no se esta destruyendo ya…
            if (!disposing)
            {
                // La marco como desechada ó desechandose,
                // de forma que no se puede ejecutar este código
                // dos veces.
                disposing = true;

                // Indico al GC que no llame al destructor
                // de esta clase al recolectarla.
                GC.SuppressFinalize(this);

                // … libero los recursos… 
            }
        }

        ~Cuota()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        private int nroCuota;
        private double importeCuota;
        private string mensualCuota;

        public int NroCuota
        {
            get { return nroCuota; }
            set { nroCuota = value; }
        }

        public double Importe_Cuota
        {
            get { return importeCuota; }
            set { importeCuota = value; }
        }

        public string Mensual_Cuota
        {
            get { return mensualCuota; }
            set { mensualCuota = value; }
        }

        public long IdNovedad { get; set; }
        public double Intereses {get;set;}
        public double Amortizacion { get; set; }
        public double Gasto_Adm { get; set; }
        public double Gasto_Adm_Tarjeta { get; set; }
        public double Seguro_Vida { get; set; }
        public double Interes_Cuota_0 { get; set; }

        public enum_enviadoLiquidar? EnviadoALiquidar { get; set; }
        //CTA CTE
        public String IdMensaje { get; set; }
        public String Mensaje { get; set; }
        public int MensualEmision { get; set; }
        public String TipoLiq { get; set; }
        public Double SaldoAmortizacion { get; set; }

        public Double ImporteCuotaLiq { get; set; }
        public Int64 IdBeneficiario { get; set; }
        public Int64 CodConceptoLiq { get; set; }
        public string DesEstado_E { get; set; }
        public string IdentPago { get; set; }
        public string descEstadoRub { get; set; }
        public string daEstadoRub { get; set; }
        public int idEstadoRub { get; set; }
        public decimal AmortizacionDescontadaCuota { get; set; }
        public DateTime FecCierreCuota { get; set; }
        public DateTime FecCierreUltEmision { get; set; }

        public Cuota() { }

        public Cuota(int NroCuota, double ImporteCuota, string Mensual_Cuota)
        {
            this.NroCuota = NroCuota;
            this.Importe_Cuota = ImporteCuota;
            this.Mensual_Cuota = Mensual_Cuota;
        }
        public Cuota(long _idNovedad, string _mensual_Cuota, int _nroCuota, double _importeCuota,
                     double _amortizacion, double _intereses, double _gasto_Adm,
                     double _gasto_Adm_Tarjeta, double _seguro_Vida)
        {
            this.IdNovedad = _idNovedad;
            this.Mensual_Cuota = _mensual_Cuota;
            this.NroCuota = _nroCuota;
            this.Importe_Cuota = _importeCuota;
            this.Amortizacion = _amortizacion;
            this.Intereses = _intereses;
            this.Gasto_Adm = _gasto_Adm;
            this.Gasto_Adm_Tarjeta = _gasto_Adm_Tarjeta;
            this.Seguro_Vida = _seguro_Vida;
        }
        public Cuota(long _idNovedad, string _mensual_Cuota, int _nroCuota, double _importeCuota,
                     double _amortizacion, double _intereses, double _gasto_Adm,
                     double _gasto_Adm_Tarjeta, double _seguro_Vida,enum_enviadoLiquidar enviadoALiquidar,
                     string idMensaje, string mensaje, int mensualEmision, String tipoLiq,
                     double saldoAmortizacion, double importeCuotaLiq, Int64 idBeneficiario, Int32 codConceptoLiq, string DesEstado_E , string IdentPago,
                     int idEstadoRub, string descEstadoRub, string daEstadoRub, decimal amortizacionDescontadaCuota, DateTime fecCierreCuota, DateTime fecCierreUltEmision,
                     double interes_Cuota_0)
        {
            this.IdNovedad = _idNovedad;
            this.Mensual_Cuota = _mensual_Cuota;
            this.NroCuota = _nroCuota;
            this.Importe_Cuota = _importeCuota;
            this.Amortizacion = _amortizacion;
            this.Intereses = _intereses;
            this.Gasto_Adm = _gasto_Adm;
            this.Gasto_Adm_Tarjeta = _gasto_Adm_Tarjeta;
            this.Seguro_Vida = _seguro_Vida;
            this.EnviadoALiquidar = enviadoALiquidar;
            //Cta Cte
            this.IdMensaje = idMensaje;
            this.Mensaje = mensaje;
            this.MensualEmision = mensualEmision;
            this.TipoLiq = tipoLiq;
            this.SaldoAmortizacion = saldoAmortizacion;
            this.ImporteCuotaLiq = importeCuotaLiq;
            this.CodConceptoLiq = codConceptoLiq;
            this.IdBeneficiario = idBeneficiario;
            this.DesEstado_E = DesEstado_E;
            this.idEstadoRub = idEstadoRub;
            this.descEstadoRub = descEstadoRub;
            this.daEstadoRub = daEstadoRub;
            this.IdentPago = IdentPago;
            this.AmortizacionDescontadaCuota = amortizacionDescontadaCuota;
            this.FecCierreCuota = fecCierreCuota;
            this.FecCierreUltEmision = fecCierreUltEmision;
            this.Interes_Cuota_0 = interes_Cuota_0;
        }
    }

    [Serializable]
    public class RelacionConceptoCantCuotas: IDisposable
    {
         #region Dispose

        private bool disposing;

        public void Dispose()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        protected virtual void Dispose(bool b)
        {
            // Si no se esta destruyendo ya…
            if (!disposing)
            {
                // La marco como desechada ó desechandose,
                // de forma que no se puede ejecutar este código
                // dos veces.
                disposing = true;

                // Indico al GC que no llame al destructor
                // de esta clase al recolectarla.
                GC.SuppressFinalize(this);

                // … libero los recursos… 
            }
        }

        ~RelacionConceptoCantCuotas()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        public long IDPrestador { get; set; }
        public int CodConceptoLiq { get; set; }
        public int CantCuotas { get; set; }
        public DateTime  FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
     
        public RelacionConceptoCantCuotas() { }

        public RelacionConceptoCantCuotas(int _CantCuotas)
        {
            CantCuotas = _CantCuotas;
        }
    }
}
