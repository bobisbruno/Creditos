using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class TipoServicio
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

        ~TipoServicio()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        public string Id { get; set; }
        public string Descripcion { get; set; }
        public short PideFactura { get; set; }
        public short PidePrestadorServicio { get; set; }
        public short PideCBU { get; set; }
        public short PideOtroMedioPago { get; set; }
        public short PidePoliza { get; set; }
        public short PideNroSocio { get; set; }
        public short PideDetalleServicio { get; set; }
        public short PideTarjeta { get; set; }
        public short PideSucursal { get; set; }
        public short PideTicket { get; set; }
        public short PideTipoDocPresentado { get; set; }      

        public TipoServicio()
        { }

        public TipoServicio(string _Id, string _Descripcion,short _PideFactura,short _PidePrestadorServicio,short _PideCBU,short _PideOtroMedioPago,short _PidePoliza,
                            short _PideNroSocio,short _PideDetalleServicio,short _PideTarjeta,short _PideSucursal,short _PideTicket,short _PideTipoDocPresentado )
        {
            Id = _Id;
            Descripcion = _Descripcion;
            PideFactura = _PideFactura;
            PidePrestadorServicio = _PidePrestadorServicio;
            PideCBU = _PideCBU;
            PideOtroMedioPago = _PideOtroMedioPago;
            PidePoliza = _PidePoliza;
            PideNroSocio = _PideNroSocio;
            PideDetalleServicio = _PideDetalleServicio;
            PideTarjeta = _PideTarjeta;
            PideSucursal = _PideSucursal;
            PideTicket = _PideTicket;
            PideTipoDocPresentado = _PideTipoDocPresentado;

        }

        #region Errores de Clase
        public class TipoServicioException : System.ApplicationException
        {
            public TipoServicioException(string mensaje)
                : base(mensaje)
            {
            }
        }
        #endregion
    }
}
