using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using Anses.DAT.Negocio;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Collections.Generic;
using System.EnterpriseServices;

namespace Ar.Gov.Anses.Microinformatica.DAT.Servicio
{
    [WebService(Namespace = "http://dat.anses.gov.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class TablasTipoPersonasWS : System.Web.Services.WebService
	{
        public TablasTipoPersonasWS()
		{}
         
        [WebMethod]
        public TablaTipoPersona TTP_EstadoGrupoControlXCodigo(string codigo)
        {
            return TablasTipoPersonasNegocio.EstadoGrupoControlXCodigo(codigo);
        }

        [WebMethod]
        public TablaTipoPersona TTP_TipoDocumentoXCodigo(string codigo)
        {
            return TablasTipoPersonasNegocio.TipoDocumentoXCodigo(codigo);
        }

        [WebMethod]
        public TablaTipoPersona TTP_EstadoCivilXCodigo(string codigo)
        {
            return TablasTipoPersonasNegocio.EstadoCivilXCodigo(codigo);
        }

        [WebMethod]
        public TablaTipoPersona TTP_IncapacidadXCodigo(string codigo)
        {
            return TablasTipoPersonasNegocio.IncapacidadXCodigo(codigo);
        }

        [WebMethod]
        public TablaTipoPersona TTP_ComprobanteIngresoPaisXCodigo(string codigo)
        {
            return TablasTipoPersonasNegocio.ComprobanteIngresoPaisXCodigo(codigo);
        }

        [WebMethod]
        public TablaTipoPersona TTP_TipoResidenciaXCodigo(string codigo)
        {
            return TablasTipoPersonasNegocio.TipoResidenciaXCodigo(codigo);
        }

        [WebMethod]
        public TablaTipoPersona TTP_TipoDomicilioXCodigo(string codigo)
        {
            return TablasTipoPersonasNegocio.TipoDomicilioXCodigo(codigo);
        }

        [WebMethod]
        public TablaTipoPersona TTP_EstadoFallecimientoXCodigo(string codigo)
        {
            return TablasTipoPersonasNegocio.EstadoFallecimientoXCodigo(codigo);
        }

        [WebMethod]
        public TablaTipoPersona TTP_TipoCuilCuitXCodigo(string codigo)
        {
            return TablasTipoPersonasNegocio.TipoCuilCuitXCodigo(codigo);
        }

        [WebMethod]
        public TablaTipoPersona TTP_EstadoRespectoAfipXCodigo(string codigo)
        {
            return TablasTipoPersonasNegocio.EstadoRespectoAfipXCodigo(codigo);
        }

        [WebMethod]
        public TablaTipoPersona TTP_PaisXCodigo(string codigo)
        {
            return TablasTipoPersonasNegocio.PaisXCodigo(codigo);
        }
        
        [WebMethod]
        public TablaTipoPersona TTP_ProvinciaXCodigo(string codigo)
        {
            return TablasTipoPersonasNegocio.ProvinciaXCodigo(codigo);
        }
	}
}
