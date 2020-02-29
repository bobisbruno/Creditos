using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class Parametros
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

        ~Parametros()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        public bool Habilitado { get; set; }
        public byte CantMaximaCuotas { get; set; }
        public float MaxPorcentaje { get; set; }
        public DateTime HoraCorte { get; set; }
        public DateTime HoraCierre { get; set; }
        public bool Nominada1A1 { get; set; }
        public byte CantDiasTarjetaUDAI { get; set; }
        public float MaxMontoCreditoFGSInundado { get; set; }
        public float MaxMontoCreditoFGSTotal { get; set; }
        public float MaxMontoCreditoFGS { get; set; }
        public bool ComercioFirma { get; set; }
        public bool ComercioDocumentacion { get; set; }
        public bool ComercioHuella { get; set; }
        public bool TSAltaAutomatica { get; set; }
        public DateTime? TSAltaAutomaticaDesde { get; set; }
        public string CantCuotasHabilitadaArgenta { get; set; }  
        public bool VlaidaEdadTipoDoc { get; set; }
        public DateTime? ValidaEdadTipoDocDesde { get; set; }
        public DateTime? ValidaEdadTipoDocFechaCorte { get; set; }
        public bool HabilitaLeyendaSoloDNITarjeta { get; set; }
        public DateTime? HabilitaLeyendaSoloDNITarjetaDesde { get; set; }
        public bool HabilitaAltaTurno { get; set; }
        public DateTime? HabilitaAltaTurnoDesde { get; set; }
        public bool HabilitaArgentaUVHI { get; set; }
        public DateTime? HabilitaArgentaUVHIDesde { get; set; }
        public bool HabilitaAltaPNC { get; set; }
        public DateTime? HabilitaAltaPNCDesde { get; set; }
        public bool HabilitaSBAHuella { get; set; }
        public DateTime? HabilitaSBAHuellaDesde { get; set; }
        public bool HabilitaANME { get; set; }
        public int SiniestroResumenTope { get; set; }
        public bool HabilitaCalculoMontoPrestamoTotal { get; set; }
        public bool HabilitaValidacionMadre7H { get; set; }
        public DateTime? HabilitaValidacionMadre7HDesde { get; set; }
        public bool HabilitaValidacionRiesgo { get; set; }
        public DateTime? HabilitaValidacionRiesgoDesde { get; set; }
        public bool HabilitaValidacionDomicilioExranjero { get; set; }
        public DateTime? HabilitaValidacionDomicilioExranjeroDesde { get; set; }
        public bool  HabilitaDeudaArgenta { get; set; }
        public DateTime? HabilitaDeudaArgentaDesde { get; set; }
        public int SiniestroTopeFilaXPagina { get; set; }
        public bool HabiltaCoelsaValidacionCBU { get; set; }
        public DateTime? HabiltaCoelsaValidacionCBUDesde { get; set; }
        public bool HabiltaCuotaCero { get; set; }


        public Parametros()
        { }

        #region Errores de Clase
        public class ParametrosException : System.ApplicationException
        {
            public ParametrosException(string mensaje)
                : base(mensaje)
            {
            }
        }
        #endregion
    }

    [Serializable]
    public class Parametros_CodConcepto_T3
    {
        public long Codconceptoliq {get;set;}
        public int CantMinCuotas { get; set; }
        public int CantMaxCuotas { get; set; }
        public double MontoMinCred { get; set; }
        public double MontoMaxCred { get; set; }
        public bool RequiereCBU { get; set; }

        public Parametros_CodConcepto_T3() { }
        public Parametros_CodConcepto_T3(long _Codconceptoliq, int _CantMinCuotas, int _CantMaxCuotas,
                                         double _MontoMinCred, double _MontoMaxCred, bool _RequiereCBU)
        { 
            
            Codconceptoliq = _Codconceptoliq;
            CantMinCuotas = _CantMinCuotas;
            CantMaxCuotas = _CantMaxCuotas;
            MontoMinCred = _MontoMinCred;
            MontoMaxCred = _MontoMaxCred;
            RequiereCBU = _RequiereCBU;

        }
    }

    public class Parametros_CostoFinaciero
    {        
        public DateTime FDesde { get; set; }
        public DateTime? FHasta { get; set; }
        public int CantCuotasDesde { get; set; }
        public int CantCuotasHasta { get; set; }
        public double CFTA { get; set; }
        public double PorcentajeError { get; set; }
        public double Total { get; set; }

        public Parametros_CostoFinaciero() { }

        public Parametros_CostoFinaciero(DateTime _FDesde, DateTime? _FHasta, int _CantCuotasDesde, int _CantCuotasHasta, double _CFTA, double _PorcentajeError, double _Total)
        {
            FDesde = _FDesde;
            FHasta = _FHasta;
            CantCuotasDesde = _CantCuotasDesde;
            CantCuotasHasta = _CantCuotasHasta;
            CFTA = _CFTA;
            PorcentajeError = _PorcentajeError;
            Total = _Total;
        }
    }
}
