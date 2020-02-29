using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public enum enum_TipoEstadoPin
    {
        NoTienePedidosPin = 0,
        AltaPin = 1,
        BlanqueoPin = 2,
        BajaPin = 3,
        DesisteOperacion = 4,
    }
    
    [Serializable]
    public class PinLink
    {
        public long NroTarjeta { set; get; }
        public long Cuil { set; get; }
        public DateTime FechaNovedad { set; get; }
        public TipoEstadoPin UnTipoEstadoPin { set; get; }
        public Auditoria UnAuditoria { set; get; }
        public DateTime FechaProceso { set; get; }
        public String NombreArchivo { set; get; }
        public Boolean ResultadoProcesoTS { set; get; }
        public String MensajeResultadoProceso { set; get;}
        public enum_TipoEstadoPin ? enum_TipoEstadoPin { set; get; }
        public Entidad_Prest_Comer UnPrestador { set; get; }
        public Int16 TipoDocPresentado { set; get; }
        public CodigoPreAprobado UnCodigoPreAprobado { set; get; }

        public PinLink() { }
        public PinLink(long _NroTarjeta, long _Cuil, TipoEstadoPin _unTipoEstadoPin, DateTime _FechaNovedad, string _NombreArchivo, Entidad_Prest_Comer unPrestador) 
        {
            this.NroTarjeta = _NroTarjeta;
            this.Cuil = _Cuil;
            this.UnTipoEstadoPin = _unTipoEstadoPin;
            this.FechaNovedad = _FechaNovedad;
            this.NombreArchivo = _NombreArchivo;
            this.UnPrestador = unPrestador;
        }

        public PinLink(long _NroTarjeta, long _Cuil, TipoEstadoPin _unTipoEstadoPin, string _NombreArchivo, Entidad_Prest_Comer unPrestador, Int16 tipoDocPresentado, CodigoPreAprobado unCodigoPreAprobado)
        {
            this.NroTarjeta = _NroTarjeta;
            this.Cuil = _Cuil;
            this.UnTipoEstadoPin = _unTipoEstadoPin;
            this.NombreArchivo = _NombreArchivo;
            this.UnPrestador = unPrestador;
            this.TipoDocPresentado = tipoDocPresentado;
            this.UnCodigoPreAprobado = unCodigoPreAprobado;
        }
    }

    [Serializable]
    public class TipoEstadoPin
    {
        public int IdEstadoPin { set; get; }
        public String CodigoEstadoPin { set; get; }
        public String DescripcionEstadoPin { set; get; }
        public Boolean Habilitado { set; get; }
        public Boolean SeEnviaATS { set; get; }

        public TipoEstadoPin() { }

        public TipoEstadoPin(int _IdEstadoPin ,String _DescripcionEstadoPin) 
        {
            this.IdEstadoPin = _IdEstadoPin;
            this.DescripcionEstadoPin = _DescripcionEstadoPin;
        }
            
    }


}
