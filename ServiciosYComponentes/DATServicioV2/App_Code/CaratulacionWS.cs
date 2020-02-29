using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using System.ComponentModel;
using System.Transactions;
using Ar.Gov.Anses.Microinformatica;
using Anses.Director.Session;

namespace Ar.Gov.Anses.Microinformatica.DAT.Servicio
{
    [WebService(Namespace = "http://dat.anses.gov.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CaratulacionWS : System.Web.Services.WebService
    {
        
        public enum enum_Cartula_Cambia_Estado
        {
            APROBAR = 40,
            RECHAZAR = 41,
            BAJA = 43
        }


        public CaratulacionWS()
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

        #region  Traer
        [WebMethod(Description = "Traer la novedad para caratular")]
        public List<NovedadCaratulada> Traer_xIdNovedad(long idNovedad)
        {

            try
            {
                return CaratulacionDAO.Traer_xIdNovedad(idNovedad);
            }
            catch (Exception err)
            {         
                throw err;
            }
        }
        #endregion

        #region  Alta
        [WebMethod(Description = "Alta de la novedad para caratular")]
        public void NovedadesCaratuladas_Alta(string Expediente, long IdNovedad, DateTime FecRecepcion, DateTime FecAlta,
                                        long cuil, long IdBeneficiario, int EstadoExpediente, byte faltaIngresarDocumentacion,
                                        string observaciones, string usuarios, string oficina, string ip, long IdPrestador)
        {

            try
            {
                CaratulacionDAO.NovedadesCaratuladas_Alta(Expediente, IdNovedad, FecRecepcion, FecAlta,
                           cuil, IdBeneficiario, EstadoExpediente, faltaIngresarDocumentacion, observaciones,
                                       usuarios, oficina, ip, IdPrestador);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        #endregion

        #region Caratula_Cambiar_Estado


        [WebMethod]
        public Boolean Novedades_Cartula_Cambia_Estado(enum_Cartula_Cambia_Estado CambiaEstado, long id_Novedad, string expediente, string observaciones, string nroResolucion, int? idTipoRechazo, out string Error)
        {
            try
            {
                Error = string.Empty;
                WSCambiarEstadoExpediente.TipoError ERR = new WSCambiarEstadoExpediente.TipoError();

                WSCambiarEstadoExpediente.CambiarEstadoExpedienteWS srv = new WSCambiarEstadoExpediente.CambiarEstadoExpedienteWS();
                srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
                srv.Credentials = System.Net.CredentialCache.DefaultCredentials;

                #region Creo Obj Expediente

                WSCambiarEstadoExpediente.ExpedienteIdDTO exp = new WSCambiarEstadoExpediente.ExpedienteIdDTO();

                exp.cuil = new WSCambiarEstadoExpediente.CuilDTO();
                exp.cuil.digCuil = expediente.Substring(13, 1).ToString();
                exp.cuil.docCuil = expediente.Substring(5, 8).ToString();
                exp.cuil.preCuil = expediente.Substring(3, 2).ToString();
                exp.organismo = expediente.Substring(0, 3);
                exp.secuencia = expediente.Substring(17).ToString();
                exp.tipoTramite = expediente.Substring(14, 3).ToString();

                #endregion

                #region Creo obj TipoSesion

                IUsuarioToken Token = new UsuarioToken();
                Token.ObtenerUsuarioEnWs();

                WSCambiarEstadoExpediente.TipoSesion ts = new WSCambiarEstadoExpediente.TipoSesion();
                ts.UsuarioRed = Token.IdUsuario;
                ts.Ip = ((InfoElement)(Token.Atributos[4])).Value;
                ts.Oficina = Token.Oficina;
                ts.Legajo = Token.IdUsuario.Substring(1);
                ts.CodAplicacion = "0004";
                ts.CasoDeUso = "EstExpDAT";
                ts.CuilOrganismo = ((string[])(Token.EntidadesAsociadas))[0];
                ts.Aplicacion = Token.Sistema;

                #endregion

                using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    switch (CambiaEstado)
                    {
                        case enum_Cartula_Cambia_Estado.APROBAR:
                            #region

                            Error = NovedadDAO.Novedades_AprobarCredito(id_Novedad, (int)enum_tipoestadoNovedad.Normal, Token.IdUsuario);

                            if (!string.IsNullOrEmpty(Error))
                                return false;

                            CaratulacionDAO.NovedadesCaratuladas_ModificarEstado(id_Novedad,
                                                                                (int)CambiaEstado,
                                                                                Token.IdUsuario,
                                                                                Token.Oficina,
                                                                                Token.DirIP,
                                                                                observaciones,
                                                                                nroResolucion,
                                                                                idTipoRechazo);

                            try
                            {
                                // codigo Sistema=41 -->ultimo parametro
                                ERR = srv.CambiarEstadoExpediente(exp, ((int)CambiaEstado).ToString(), null, ts, 41);
                                
                                if (ERR != null && !string.IsNullOrEmpty(ERR.descripcion))
                                {
                                    throw new Exception(ERR.descripcion);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (!ValidarANMEEstadoExp(exp, CambiaEstado))
                                {
                                    Error = ERR.descripcion;                                    
                                    //throw new Exception("Error en servicio CambiarEstadoExpediente - URL: " + srv.Url + " - ERROR: " + ex.Message + " - SRC: " + ex.Source);
                                }
                            }

                            Error = ERR.descripcion;

                            break;
                            #endregion

                        case enum_Cartula_Cambia_Estado.RECHAZAR:
                            #region

                            NovedadDAO.Novedades_B_Con_Desaf_Monto(id_Novedad, (int)enum_tipoestadoNovedad.BajaporrechazodelanovedadGciaControl, "Baja Control", Token.DirIP, Token.IdUsuario, false);

                            CaratulacionDAO.NovedadesCaratuladas_ModificarEstado(id_Novedad,
                                                                                (int)CambiaEstado,
                                                                                Token.IdUsuario,
                                                                                Token.Oficina,
                                                                                Token.DirIP,
                                                                                observaciones,
                                                                                nroResolucion,
                                                                                idTipoRechazo);

                            try
                            {
                                // codigo Sistema=41 -->ultimo parametro
                                ERR = srv.CambiarEstadoExpediente(exp, ((int)CambiaEstado).ToString(), null, ts, 41);

                                if (ERR != null && !string.IsNullOrEmpty(ERR.descripcion))
                                {
                                    throw new Exception(ERR.descripcion);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (!ValidarANMEEstadoExp(exp, CambiaEstado))
                                {
                                    Error = ERR.descripcion;
                                    //throw new Exception("Error en servicio CambiarEstadoExpediente - URL: " + srv.Url + " - ERROR: " + ex.Message + " - SRC: " + ex.Source);
                                }
                            }

                            Error = ERR.descripcion;

                            break;
                            #endregion

                        case enum_Cartula_Cambia_Estado.BAJA:
                            #region
                            CaratulacionDAO.NovedadesCaratuladas_ModificarEstado(id_Novedad,
                                                                                (int)CambiaEstado,
                                                                                Token.IdUsuario,
                                                                                Token.Oficina,
                                                                                Token.DirIP,
                                                                                observaciones,
                                                                                nroResolucion,
                                                                                idTipoRechazo);

                            try
                            {
                                // codigo Sistema=41 -->ultimo parametro
                                ERR = srv.CambiarEstadoExpediente(exp, ((int)CambiaEstado).ToString(), null, ts, 41);

                                  if (ERR != null && !string.IsNullOrEmpty(ERR.descripcion))
                                {
                                    throw new Exception(ERR.descripcion);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (!ValidarANMEEstadoExp(exp, CambiaEstado))
                                {
                                    Error = ERR.descripcion;
                                    //throw new Exception("Error en servicio CambiarEstadoExpediente - URL: " + srv.Url + " - ERROR: " + ex.Message + " - SRC: " + ex.Source);
                                }
                            }

                            Error = ERR.descripcion;
                            break;
                            #endregion
                    }

                    if (string.IsNullOrEmpty(Error))
                    {
                        oTransactionScope.Complete();
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception err)
            {
                throw err;
            }

        }

        private bool ValidarANMEEstadoExp(WSCambiarEstadoExpediente.ExpedienteIdDTO exp, enum_Cartula_Cambia_Estado cambiaEstado)
        {
            Boolean retorno = false;

            try
            {
                ANMEConsGral.BuscarExpedientePorPkWS srv = new ANMEConsGral.BuscarExpedientePorPkWS();
                srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
                srv.Credentials = System.Net.CredentialCache.DefaultCredentials;

                ANMEConsGral.TipoError error = new ANMEConsGral.TipoError();
                ANMEConsGral.TipoExpedientePorPk tipoExpedientePorPk = srv.BuscarExpedientePorPk(exp.organismo, exp.cuil.preCuil, exp.cuil.docCuil, exp.cuil.digCuil, int.Parse(exp.tipoTramite), int.Parse(exp.secuencia), new ANMEConsGral.TipoAuditoria(), out error);

                if (error != null &&
                    error.codigo == 0 &&
                    tipoExpedientePorPk != null &&
                    tipoExpedientePorPk.estado.Equals((int)cambiaEstado))
                {
                    retorno = true;
                }
            }
            catch
            {
                return retorno;
            }

            return retorno;
        }
        #endregion
        
        [WebMethod]
        public List<NovedadCaratulada> Novedades_Caratuladas_Traer
                                    (ConsultaBatch.enum_ConsultaBatch_NombreConsulta nombreConsulta,
                                     long idPrestador, DateTime? Fecha_Recepcion_desde, DateTime? Fecha_Recepcion_hasta, enum_EstadoCaratulacion? idEstado,
                                     int conErrores, long? id_Beneficiario,
                                     bool generaArchivo, bool generadoAdmin, out string rutaArchivoSal)
        {
            return CaratulacionDAO.Novedades_Caratuladas_Traer(nombreConsulta,
                                     idPrestador, Fecha_Recepcion_desde, Fecha_Recepcion_hasta, idEstado,
                                     conErrores, id_Beneficiario,
                                     generaArchivo, generadoAdmin, out rutaArchivoSal);
        }

        [WebMethod]
        public List<NovedadCaratuladaTotales> Novedades_Caratuladas_Traer_Por_Estado
                                    (long? idPrestador, DateTime? fDesde, DateTime? fHasta)
        {
            return CaratulacionDAO.Novedades_Caratuladas_Traer_Por_Estado(idPrestador, fDesde, fHasta);
        }

        [WebMethod]
        public List<NovedadCaratuladaTotales> Novedades_Caratuladas_Traer_Difiere_Estado()
        {
            return CaratulacionDAO.Novedades_Caratuladas_Traer_Difiere_Estado();
        }

        [WebMethod]
        public List<TipoRechazoExpediente> TipoRechazoExpediente_Traer()
        {
            return CaratulacionDAO.TipoRechazoExpediente_Traer();
        }

        [WebMethod(Description = "Traer Oficinas Sin Vencimiento")]
        public List<String> NovedadesCaratuladas_OficinasSinVencimiento_Traer() {
            return CaratulacionDAO.NovedadesCaratuladas_OficinasSinVencimiento_Traer();
        }

    }


}
