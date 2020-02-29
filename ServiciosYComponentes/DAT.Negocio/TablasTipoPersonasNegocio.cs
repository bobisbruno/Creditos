using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Anses.DAT.Negocio.Servicios;

namespace Anses.DAT.Negocio
{
    [Serializable]
    public class TablasTipoPersonasNegocio
    {
        private static DateTime _ultimaActualizacion = DateTime.MinValue;
        private static TablasTipoPersonasList _TTipoPersonas = null;
        static readonly object objectLock = new object();

        private static void TablasTipoPersonas_Buscar()
        {
            string mensajeError = string.Empty;
            if (_TTipoPersonas == null || (DateTime.Now - _ultimaActualizacion).TotalMinutes > 30)
            {
                lock (objectLock)
                {
                    _TTipoPersonas = new TablasTipoPersonasList();
                    _TTipoPersonas.lstEstadoGrupoControl = InvocaADPDescripciones.ObtenerEstadoGrupoControl();
                    _TTipoPersonas.lstTipoDocumento = InvocaADPDescripciones.ObtenerTipoDocumento();
                    _TTipoPersonas.lstEstadoCivil = InvocaADPDescripciones.ObtenerEstadoCivil();
                    _TTipoPersonas.lstIncapacidad = InvocaADPDescripciones.ObtenerIncapacidad();                    
                    _TTipoPersonas.lstComprobanteIngresoPais = InvocaADPDescripciones.ObtenerComprobanteIngresoPais();
                    _TTipoPersonas.lstTipoResidencia = InvocaADPDescripciones.ObtenerTipoResidencia();
                    _TTipoPersonas.lstEstadoFallecimiento = InvocaADPDescripciones.ObtenerEstadoFallecimiento();
                    _TTipoPersonas.lstTipoCuilCuit = InvocaADPDescripciones.ObtenerTipoCuilCuit();
                    _TTipoPersonas.lstEstadoRespectoAfip = InvocaADPDescripciones.ObtenerEstadoRespectoAfip();
                    _TTipoPersonas.lstPais = InvocaADPDescripciones.ObtenerPais();
                    _TTipoPersonas.lstProvincia = InvocaADPDescripciones.ObtenerProvincia();
                    _TTipoPersonas.lstTipoDomicilio = InvocaADPDescripciones.ObtenerTipoDomicilio();
                    _ultimaActualizacion = DateTime.Now;
                }
            }
            if (!string.IsNullOrEmpty(mensajeError))
                throw new Exception(mensajeError);
        }

        public static TablaTipoPersona EstadoGrupoControlXCodigo(string codigo)
        {
            TablasTipoPersonas_Buscar();
            return _TTipoPersonas.lstEstadoGrupoControl.Find(x => x.Codigo == codigo);
        }

        public static TablaTipoPersona TipoDocumentoXCodigo(string codigo)
        {
            TablasTipoPersonas_Buscar();
            return _TTipoPersonas.lstTipoDocumento.Find(x => x.Codigo == codigo);
        }

        public static TablaTipoPersona EstadoCivilXCodigo(string codigo)
        {
            TablasTipoPersonas_Buscar();
            return _TTipoPersonas.lstEstadoCivil.Find(x => x.Codigo == codigo);
        }

        public static TablaTipoPersona IncapacidadXCodigo(string codigo)
        {
            TablasTipoPersonas_Buscar();
            return _TTipoPersonas.lstIncapacidad.Find(x => x.Codigo == codigo);
        }

        public static TablaTipoPersona ComprobanteIngresoPaisXCodigo(string codigo)
        {
            TablasTipoPersonas_Buscar();
            return _TTipoPersonas.lstComprobanteIngresoPais.Find(x => x.Codigo == codigo);
        }

        public static TablaTipoPersona TipoResidenciaXCodigo(string codigo)
        {
            TablasTipoPersonas_Buscar();
            return _TTipoPersonas.lstTipoResidencia.Find(x => x.Codigo == codigo);
        }

        public static TablaTipoPersona TipoDomicilioXCodigo(string codigo)
        {
            TablasTipoPersonas_Buscar();
            return _TTipoPersonas.lstTipoDomicilio.Find(x => x.Codigo == codigo);
        }

        public static TablaTipoPersona EstadoFallecimientoXCodigo(string codigo)
        {
            TablasTipoPersonas_Buscar();
            return _TTipoPersonas.lstEstadoFallecimiento.Find(x => x.Codigo == codigo);
        }

        public static TablaTipoPersona TipoCuilCuitXCodigo(string codigo)
        {
            TablasTipoPersonas_Buscar();
            return _TTipoPersonas.lstTipoCuilCuit.Find(x => x.Codigo == codigo);
        }

        public static TablaTipoPersona EstadoRespectoAfipXCodigo(string codigo)
        {
            TablasTipoPersonas_Buscar();
            return _TTipoPersonas.lstEstadoRespectoAfip.Find(x => x.Codigo == codigo);
        }

        public static TablaTipoPersona PaisXCodigo(string codigo)
        {
            TablasTipoPersonas_Buscar();
            return _TTipoPersonas.lstPais.Find(x => x.Codigo == codigo);
        }

        public static TablaTipoPersona ProvinciaXCodigo(string codigo)
        {
            TablasTipoPersonas_Buscar();
            return _TTipoPersonas.lstProvincia.Find(x => x.Codigo == codigo);
        }       
    }
}
