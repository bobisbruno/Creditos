using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    public enum enum_EstadoCaratulacion
    {
        Pendiente_de_verificación = 0,
        Confirmado_Control = 1,
        Rechazado_Control = 2,
        Expediente_dado_baja = 3
    }

    [Serializable]
    public class NovedadCaratulada
    {
        public string NroExpediente { get; set; }
        public DateTime? FAlta { get; set; }
        public DateTime? FInicioAfjp { get; set; } // frecepcion
        public int? idEstadoExpediente { get; set; }
        public enum_EstadoCaratulacion? idEstadoCaratulacion { get; set; }
        public string DesEstadoCaratulacion { get; set; }
        public string Error { get; set; }
        public Novedad novedad { get; set; }
        public string UsuarioAlta { get; set; }
        public string OficinaAlta { get; set; }
        public string Observaciones { get; set; }
        public string NroResolucion { get; set; }
        public TipoRechazoExpediente Tiporechazo { get; set; }

        public NovedadCaratulada() { }

        public NovedadCaratulada(string nroExpediente,
                       DateTime? fAlta,
                       DateTime? fInicioAfjp,
                       enum_EstadoCaratulacion? estadoCaratulacion,
                       string DesEstadoCaratulacion,
                       int? idEstadoExpediente,
                       string error,
                       Novedad novedad,
                       string usuarioAlta,
                       string oficinaAlta,
                       string observaciones,
                       string nroResolucion,
                       TipoRechazoExpediente tiporechazo)
        {
            this.NroExpediente = nroExpediente;
            this.FAlta = fAlta;
            this.FInicioAfjp = fInicioAfjp;
            this.idEstadoCaratulacion = estadoCaratulacion;
            this.idEstadoExpediente = idEstadoExpediente;
            this.DesEstadoCaratulacion = DesEstadoCaratulacion;

            if (string.IsNullOrEmpty(DesEstadoCaratulacion))
                if (estadoCaratulacion.HasValue)
                    this.DesEstadoCaratulacion = estadoCaratulacion.Value.ToString().Replace("_", " ");

            this.Error = error;
            this.novedad = novedad;
            this.UsuarioAlta = usuarioAlta;
            this.OficinaAlta = oficinaAlta;
            this.Observaciones = observaciones;
            this.NroResolucion = nroResolucion;
            this.Tiporechazo = tiporechazo;
        }
    }

    [Serializable]
    public class NovedadCaratuladaTotales
    {
        public int? IdEstadoExpediente { get; set; }
        public string DesEstadoExpediente { get; set; }
        public enum_EstadoCaratulacion? IdEstadoCaratulacion { get; set; }
        public string DesEstadoCaratulacion { get; set; }
        public int TotalDifiere { get; set; }
        public int TotalSinDuplicado { get; set; }
        public int TotalNovedades { get; set; }

        public NovedadCaratuladaTotales(int? idEstadoExpediente,
                                        string desEstadoExpediente,
                                        enum_EstadoCaratulacion? idEstadoCaratulacion,
                                        string desEstadoCaratulacion,
                                        int totalDifiere)
        {
            this.IdEstadoExpediente = idEstadoExpediente;
            this.IdEstadoCaratulacion = idEstadoCaratulacion;
            this.DesEstadoExpediente = desEstadoExpediente;
            this.DesEstadoCaratulacion = desEstadoCaratulacion;
            this.TotalDifiere = totalDifiere;
        }

        public NovedadCaratuladaTotales(string desEstadoCaratulacion,
                                        int totalSinDuplicado,
                                        int totalNovedades)
        {            
            this.DesEstadoCaratulacion = desEstadoCaratulacion;
            this.TotalSinDuplicado = totalSinDuplicado;
            this.TotalNovedades = totalNovedades;
        }

        public NovedadCaratuladaTotales() { }
    }
}

