using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Collections.Generic;

namespace Ar.Gov.Anses.Microinformatica.DAT.Servicio
{
    [WebService(Namespace = "http://dat.anses.gov.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class EstadoWS : System.Web.Services.WebService
    {
        public EstadoWS()
        {
            InitializeComponent();
        }

        #region Component Designer generated code

        //Required by the Web Services Designer 
        private IContainer components = null;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion

        [WebMethod(Description = "Trae Todos los estados habilitados")]
        public List<Estado> Traer_Todos()
        {
            try
            {
                return  EstadoDAO.Traer_Todos();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        [WebMethod(Description = "Trae el estado dado")]
        public Estado Traer(int idEstado)
        {
            try
            {
                
                return EstadoDAO.Traer(idEstado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
       

        [WebMethod(Description = "Trae todos los modelos de impresion habilitados del estado dado")]
        public List<ModeloImpresion> ModeloImpresionTraer(int idEstado)
        {
            try
            {
                return EstadoReclamoDAO.ModeloImpresionTraer(idEstado);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [WebMethod(Description = "Trae Tipos de Estados de Documentacion")]
        public List<EstadoDocumentacion> Tipos_EstadosDocumentacion_Trae()
        {
            try
            {
                return EstadoDAO.Tipos_EstadosDocumentacion_Trae();
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        [WebMethod(Description = "Trae Tipos de Estados de Caratulacion")]
        public List<EstadoCaratulacion> Tipos_EstadosCaratulacion_Trae()
        {
            try
            {
                return EstadoDAO.Tipos_EstadosCaratulacion_Trae();
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        
    }
}