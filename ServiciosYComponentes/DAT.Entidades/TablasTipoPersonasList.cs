using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ar.Gov.Anses.Microinformatica.DAT.Entidades
{
    [Serializable]
    public class TablasTipoPersonasList
    {
        private List<TablaTipoPersona> _lstEstadoGrupoControl = new List<TablaTipoPersona>();
        public List<TablaTipoPersona> lstEstadoGrupoControl
        {
            get { return _lstEstadoGrupoControl; }
            set { _lstEstadoGrupoControl = value; }
        }  
 
        private List<TablaTipoPersona> _lstTipoDocumento = new List<TablaTipoPersona>();
        public List<TablaTipoPersona> lstTipoDocumento
        {
            get { return _lstTipoDocumento; }
            set { _lstTipoDocumento = value; }
        } 

        private List<TablaTipoPersona> _lstEstadoCivil = new List<TablaTipoPersona>();
        public List<TablaTipoPersona> lstEstadoCivil
        {
            get { return _lstEstadoCivil; }
            set { _lstEstadoCivil = value; }
        } 

        private List<TablaTipoPersona> _lstIncapacidad = new List<TablaTipoPersona>();
        public List<TablaTipoPersona> lstIncapacidad
        {
            get { return _lstIncapacidad; }
            set { _lstIncapacidad = value; }
        }
       
        private List<TablaTipoPersona> _lstComprobanteIngresoPais = new List<TablaTipoPersona>();
        public List<TablaTipoPersona> lstComprobanteIngresoPais
        {
            get { return _lstComprobanteIngresoPais; }
            set { _lstComprobanteIngresoPais = value; }
        }
 
        private List<TablaTipoPersona> _lstTipoResidencia = new List<TablaTipoPersona>();
        public List<TablaTipoPersona> lstTipoResidencia
        {
            get { return _lstTipoResidencia; }
            set { _lstTipoResidencia = value; }
        }

        private List<TablaTipoPersona> _lstEstadoFallecimiento = new List<TablaTipoPersona>();
        public List<TablaTipoPersona> lstEstadoFallecimiento
        {
            get { return _lstEstadoFallecimiento; }
            set { _lstEstadoFallecimiento = value; }
        }

        private List<TablaTipoPersona> _lstTipoCuilCuit = new List<TablaTipoPersona>();
        public List<TablaTipoPersona> lstTipoCuilCuit
        {
            get { return _lstTipoCuilCuit; }
            set { _lstTipoCuilCuit = value; }
        }
                    
        private List<TablaTipoPersona> _lstEstadoRespectoAfip = new List<TablaTipoPersona>();
        public List<TablaTipoPersona> lstEstadoRespectoAfip
        {
            get { return _lstEstadoRespectoAfip; }
            set { _lstEstadoRespectoAfip = value; }
        }

        private List<TablaTipoPersona> _lstPais = new List<TablaTipoPersona>();
        public List<TablaTipoPersona> lstPais
        {
            get { return _lstPais; }
            set { _lstPais = value; }
        } 


        private List<TablaTipoPersona> _lstProvincia = new List<TablaTipoPersona>();
        public List<TablaTipoPersona> lstProvincia
        {
            get { return _lstProvincia; }
            set { _lstProvincia = value; }
        }

        private List<TablaTipoPersona> _lstTipoDomicilio = new List<TablaTipoPersona>();
        public List<TablaTipoPersona> lstTipoDomicilio
        {
            get { return _lstTipoDomicilio; }
            set { _lstTipoDomicilio = value; }
        }
    }
}
