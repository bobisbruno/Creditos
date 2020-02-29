using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades.EstadosNovedad
{
    public class SiniestroCobrado
    {
        public DateTime? FechaCobroFGS { get; set; }
        public decimal Importe { get; set; }
        public Int64 idSiniestro { get; set; }
        public Int64 idNovedad { get; set; }
        public Int32? idLote { get; set; }
        public DateTime? fSolicitudCobro { get; set; }
        
        #region constructores
        public SiniestroCobrado() { }
        public SiniestroCobrado(DateTime? _FechaCobroFGS, decimal _Importe, Int64 _idSiniestro, Int64 _idNovedad, Int32? _idLote, DateTime? _fSolicitudCobro)
        {
            FechaCobroFGS = _FechaCobroFGS;
            Importe = _Importe;
            idSiniestro = _idSiniestro;
            idNovedad = _idNovedad;
            idLote = _idLote;
            fSolicitudCobro = _fSolicitudCobro;
        }
        #endregion

        //internal string GetFechaCobro()
        //{
        //    return FechaCobroFGS == null? "Sin Información": ((DateTime)FechaCobroFGS).ToString("dd/MM/yyyy");
        //}

        internal string GetImporte()
        {
            return Importe.ToString();
        }
    }
}
