using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades.EstadosNovedad
{
    public class CancelacionAnticipada
    {
        public DateTime FechaCobroFGS { get; set; }
        public decimal Importe { get; set; }
        public Int64 id { get; set; }
        public Int64 idNovedad { get; set; }
        
        #region constructores
        public CancelacionAnticipada() { }
        public CancelacionAnticipada(DateTime _FechaCobroFGS, decimal _Importe, Int64 _id, Int64 _idNovedad)
        {
            FechaCobroFGS = _FechaCobroFGS;
            Importe = _Importe;
            id = _id;
            idNovedad = _idNovedad;
        }
        #endregion

        internal string GetFechaCobro()
        {
            return FechaCobroFGS.ToString("dd/MM/yyyy");
        }

        internal string GetImporte()
        {
            return Importe.ToString();
        }

    }
}
