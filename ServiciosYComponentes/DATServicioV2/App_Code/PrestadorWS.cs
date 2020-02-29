using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using System.ComponentModel;

namespace Ar.Gov.Anses.Microinformatica.DAT.Servicio
{
    [WebService(Namespace = "http://dat.anses.gov.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class PrestadorWS : System.Web.Services.WebService
    {
        public PrestadorWS()
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

        #region Prestadores Traer Prestador

        [WebMethod(Description = "TraePrestadoresXParametros")]
        public List<Prestador> TraerPrestadoresAdm(string CodigoSistema, int CodConcLiq, string RazonSocial)
        {
            try
            {
                return PrestadorDAO.TraerPrestadoresAdm(CodigoSistema, CodConcLiq, RazonSocial);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region TipoConcConLiq por Presatador Este metodo no Funciona, //La nueva version(V02) se encuentra en TipoConcConcLiqWS 

        //[WebMethod(Description = "TipoConcConLiqPorPresatador")]
        //public List<List<TipoConcepto>> TipoConcConLiqPorPresatador(long idPrestador)
        //{
        //    try
        //    {
        //        return PrestadorDAO.TipoConcConLiqPorPresatador(idPrestador);
        //    }
        //    catch (Exception err)
        //    {
        //        throw err;
        //    }            
        //}
        #endregion

        #region Trae Contactos

        [WebMethod(Description = "Trae Contactos")]
        public List<Prestador> ContactosTraer(long idPrestador, int idDomicilio)
        {         
            try
            {
                return PrestadorDAO.ContactosTraer(idPrestador, idDomicilio);  
            }
            catch (Exception err)
            {
                throw err;
            }           
        }
        #endregion

        #region Trae Domicilios

        [WebMethod(Description = "Trae Domicilios")]
        public List<Domicilio> DomiciliosTraer(long idPrestador)
        {
            try
            {
                return PrestadorDAO.DomiciliosTraer(idPrestador);
            }
            catch (Exception err)
            {
                throw err;
            }           
        }
        #endregion

        #region Trae Convenios

        [WebMethod(Description = "Trae Convenios")]
        public List<Prestador> ConveniosTraer(long idPrestador)
        {
            try
            {
                return PrestadorDAO.ConveniosTraer(idPrestador);
            }
            catch (Exception err)
            {
                throw err;
            }          
        }
        #endregion

        #region Traer Conceptos por Codigo de Sistema

        [WebMethod(Description = "Traer Conceptos por Codigo de Sistema")]
        public List<Prestador> TraerConceptosPorCodSistema(string codSistema)
        {
            try
            {
                return PrestadorDAO.TraerConceptosPorCodSistema(codSistema);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion

        #region Traer Conceptos por Codigo de Concepto

        [WebMethod(Description = "Traer Conceptos por Codigo de Concepto")]
        public List<Prestador> TraerConceptosPorCodConcepto(Int64 codConcepto)
        {
           try
            {
                return PrestadorDAO.TraerConceptosPorCodConcepto(codConcepto);
            }
            catch (Exception ex)
            {
                throw ex;                 
            }        
        }
        #endregion

        #region Trae Prestadores

        [WebMethod(Description = "Trae prestadores")]
        public List<Prestador> TraerPrestador(byte orden, long idPrestador)
        {
            try
            {
                return PrestadorDAO.TraerPrestador(orden,idPrestador) ;
            }
            catch (Exception err)
            {
                throw err;
            }
        }
       
        #endregion

        #region Trae Prestadores X CUIT

        [WebMethod(Description = "Trae prestador x CUIT")]
        public Prestador TraerPrestador_x_CUIT(long CUIT)
        {
            try
            {
                return PrestadorDAO.TraerPrestador_x_CUIT(CUIT);
            }
            catch (Exception err)
            {
                throw err;
            }
        }
        #endregion
        
        #region Trae Todos los Conceptos

        [WebMethod(Description = "Trae Todos los Conceptos")]
        public List<TipoConcepto> TraerConceptosTodo(long idPrestador)
        { 
            try
            {
                return PrestadorDAO.TraerConceptosTodo(idPrestador);
            }
            catch (Exception err)
            {
                throw err;
            }          
        }
        #endregion         

        #region Traer_Prestadores_Entrega_FGS

        [WebMethod(Description = "Trae_Prestadores_Entrega_FGS")]
        public List<Prestador> Traer_Prestadores_Entrega_FGS()
        {
            try
            {
                return PrestadorDAO.Traer_Prestadores_Entrega_FGS();
            }
            catch (Exception err)
            {
                throw err;
            }              
        }

        #endregion

        #region Metodos Para Tasas

        [WebMethod(Description = "Retorna una lista de Tasas por idPrestador y idComercializador")]
        public List<Tasa> TraerTasas_xidPrestadorIdComercializador(long idPrestador, long idComercializadora)
        {
            try
            {
                return TasasDAO.TraerTasas_xidPrestadorIdComercializador(idPrestador, idComercializadora);
            }
            catch (Exception err)
            {
                throw err;
            }
        }        

        [WebMethod(Description = "Retorna el top 20 de Tasas con su prestador y comercializador")]
        public List<Tasa> TasasAplicadas_TxTop20(int tipoTasa, double tasaDesde, double tasaHasta, int cuotaDesde, int cuotaHasta)
        {
            try
            {
                return TasasDAO.TasasAplicadas_TxTop20(tipoTasa, tasaDesde, tasaHasta, cuotaDesde, cuotaHasta);
            }
            catch (Exception err)
            {
                throw err;
            }
        }        
        
        [WebMethod(Description = "Trae una lista de Tasas asociadas a un Prestador por RazonSocial o fecha inicio fecha fin.")]
        public List<Tasa> TasasAplicadasParaGestionUCATxTop50(string codigoSistema,string razonSocial, DateTime fechaDesde, DateTime fechaHasta, bool habilita)
        {
            try
            {
                return TasasDAO.TasasAplicadasParaGestionUCATxTop50(codigoSistema, razonSocial, fechaDesde, fechaHasta, habilita);
            }
            catch (Exception err)
            {
                throw err;
            }
        }


        [WebMethod(Description = "Alta/Modificación de Tasa Aplicada")]
        public string TasasAplicadasMB(long idPrestador, long idComercializador, Tasa unaTasaAplicada)
        {
            try
            {

                    return TasasDAO.TasasAplicadasMB(idPrestador, idComercializador, unaTasaAplicada);

            }
            catch (Exception err)
            {
                throw err;
            }
        }

        [WebMethod(Description = "Alta/Modificación de Tasa Aplicada")]
        public string TasasAplicadasA(long idPrestador, long idComercializador, Tasa unaTasaAplicada)
        {
            try
            {
               return TasasDAO.TasasAplicadasA(idPrestador, idComercializador, unaTasaAplicada);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        [WebMethod(Description = "Aprobacion de Tasa Aplicada")]
        public string TasasAplicadasHabilitaDeshabilita(Tasa[] TasasAplicadas, bool habilita)
        {
            try
            {
               return TasasDAO.TasasAplicadasHabilitaDeshabilita(TasasAplicadas, habilita);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        #endregion
    }    
}

