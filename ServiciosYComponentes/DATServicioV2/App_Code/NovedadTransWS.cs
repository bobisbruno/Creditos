using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Transactions;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.Servicio
{
    [WebService(Namespace = "http://dat.anses.gov.ar/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	
    public class NovedadTransWS : System.Web.Services.WebService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NovedadWS).Name);

        public NovedadTransWS()
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

        #region Alta Novedad
         
        [WebMethod(MessageName = "Novedades_T3_Alta_ConTasa_Sucursal")]
        public string Novedades_T3_Alta_ConTasa_Sucursal(long idPrestador, long idBeneficiario, long cuil, DateTime fecNovedad, byte tipoConcepto, int conceptoOPP,
                                                         double impTotal, byte cantCuotas, string nroComprobante, string ip, string usuario, int mensual, byte idEstadoReg,
                                                         decimal montoPrestamo, decimal cuotaTotalMensual, decimal TNA, decimal TEM,
                                                         decimal gastoOtorgamiento, decimal gastoAdmMensual, decimal cuotaSocial, decimal CFTEA,
                                                         decimal CFTNAReal, decimal CFTEAReal, decimal gastoAdmMensualReal, decimal TIRReal, string xmlCuotas,
                                                         int idItem, string nroFactura, string cbu, string nroTarjeta, string otro, string prestadorServicio, string poliza,
                                                         string nroSocio, string nroTicket, int idDomicilioBeneficiario, int idDomicilioPrestador,
                                                         string nroSucursal, DateTime fVto, DateTime fVtoHabilSiguiente, byte idTipoDocPresentado, DateTime fEstimadaEntrega, bool solicitaTarjetaNominada,
                                                         string codigoPreAprobado, List<DocumentacionScaneada> docScaneada, string codigoDeBanco, string codigoDeSucursal)
        {
            string error, idNovedad= string.Empty;
            string MyLog = String.Empty;

            try
            {
                using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    MyLog += " | voy a Novedades_T3_Alta_ConTasa ";
                    error= NovedadTransDAO.Novedades_T3_Alta_ConTasa(idPrestador, idBeneficiario, cuil, fecNovedad, tipoConcepto, conceptoOPP,
                                                                          impTotal, cantCuotas, nroComprobante, ip, usuario, mensual, idEstadoReg,
                                                                          montoPrestamo, cuotaTotalMensual, TNA, TEM,
                                                                          gastoOtorgamiento, gastoAdmMensual, cuotaSocial, CFTEA,
                                                                          CFTNAReal, CFTEAReal, gastoAdmMensualReal, TIRReal, xmlCuotas,
                                                                          idItem, nroFactura, cbu, nroTarjeta, otro, prestadorServicio, poliza,
                                                                          nroSocio, nroTicket, idDomicilioBeneficiario, idDomicilioPrestador, nroSucursal, fVto, fVtoHabilSiguiente, idTipoDocPresentado, fEstimadaEntrega, solicitaTarjetaNominada, codigoPreAprobado, docScaneada, codigoDeBanco, codigoDeSucursal);
                    idNovedad = error.Split(char.Parse("|"))[1].ToString();
                    MyLog += " | regreso  de Novedades_T3_Alta_ConTasa --> IdNovedad: {0}" + idNovedad;
                    if (!string.IsNullOrEmpty(idNovedad) && docScaneada != null && docScaneada.Count > 0)
                    {
                        MyLog += " | voy a DigiWeb_DirectorioDocScaneada";
                        string directorio=  DigiWeb_DirectorioDocScaneada();
                        foreach (DocumentacionScaneada item in docScaneada)
                        {
                            MyLog += " | voy a DigiWeb_GuardarArchivo";
                            item.Idnovedad = long.Parse(idNovedad);
                            item.IdImagen = DigiWeb_GuardarArchivo(item, directorio, idNovedad, usuario, ip);
                        }

                        MyLog += " | voy a DocumentacionScaneada_Alta";
                        NovedadDocumentacionDAO.DocumentacionScaneada_Alta(docScaneada, usuario, ip);
                    }

                    oTransactionScope.Complete();                    
                }

                return error;

            }
            catch (Exception err)
            {
                log.Error("MyLog :" + MyLog);
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                if (err.Message.ToString().Substring(0, 3) == "XX-")
                {
                    error = "Se produjo un error al guardar los archivo.<br>Reintente en otro momento.";
                    return error;
                }
                else
                    throw new Exception("Error en NovedadTransWS.Novedades_T3_Alta_ConTasa_Sucursal", err);
            }
            finally
            {}
        }

        #endregion

        #region Validaciones

        [WebMethod(MessageName = "Valido_Nov_T3_Gestion")]
        public string Valido_Nov_T3_Gestion(long idPrestador, long idBeneficiario, byte tipoConcepto, int conceptoOPP,
                                            double impTotal, byte cantCuotas, Single porcentaje, byte codMovimiento,
                                            String nroComprobante, DateTime fecNovedad, string ip, string usuario,
                                            int mensual, decimal montoPrestamo, decimal cuotaTotalMensual, decimal TNA,
                                            decimal TEM, decimal gastoOtorgamiento, decimal gastoAdmMensual,
                                            decimal cuotaSocial, decimal CFTEA, decimal CFTNAReal, decimal CFTEAReal,
                                            decimal gastoAdmMensualReal, decimal TIRReal, bool bGestionErrores)
        {
            try
            {
                return NovedadTransDAO.Valido_Nov_T3_Gestion(idPrestador, idBeneficiario, tipoConcepto, conceptoOPP, impTotal, cantCuotas,
                                                             porcentaje, codMovimiento, nroComprobante, fecNovedad, ip, usuario, mensual,
                                                             montoPrestamo, cuotaTotalMensual, TNA, TEM, gastoOtorgamiento, gastoAdmMensual,
                                                             cuotaSocial, CFTEA, CFTNAReal, CFTEAReal, gastoAdmMensualReal, TIRReal, bGestionErrores);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en Valido_Nov_T3_Gestion", err);
            }
            finally
            {}
        }

        #endregion

        #region Archivos

        private Guid DigiWeb_GuardarArchivo(DocumentacionScaneada documentoScaneado, string directorio, string idNovedad, string usuario, string ip)
        {
            Guid id;
            string MyLog = String.Empty;
            try
            {
                string rutaServidor_nombreDeArchivo = Path.Combine(directorio, documentoScaneado.Nombre);
                string cuil = documentoScaneado.Cuil.ToString();

                DigitalizacionWS.EDocumentoOriginal oEDocumentoOriginal = new DigitalizacionWS.EDocumentoOriginal();
                oEDocumentoOriginal.Id =id = Guid.NewGuid();

                oEDocumentoOriginal.PreCuil = byte.Parse(cuil.Substring(0, 2));
                oEDocumentoOriginal.NumeroDocumento = cuil.Substring(2, 8);
                oEDocumentoOriginal.DigitoVerificador = byte.Parse(cuil.Substring(10, 1));
                oEDocumentoOriginal.CodigoSistema = ConfigurationManager.AppSettings["DigiWebCodSistema"].ToString();
                oEDocumentoOriginal.Titulo = documentoScaneado.TipoImagen.DescripcionAbrev;
                oEDocumentoOriginal.FechaIndexacion = DateTime.Now;

                oEDocumentoOriginal.Nombre = documentoScaneado.Nombre;
                oEDocumentoOriginal.Ruta = directorio; // para que es esta linea?? Utils.GetPathFromAtCurretDate();

                oEDocumentoOriginal.Entidad = string.Empty;
                oEDocumentoOriginal.TipoTramite = null; //41;
                oEDocumentoOriginal.Secuencia = null; //0;               

                oEDocumentoOriginal.TipoEDocumentoId = documentoScaneado.TipoImagen.IdTipoImagenDW;
                oEDocumentoOriginal.EstadoEDocumentoId = int.Parse(ConfigurationManager.AppSettings["DigiWebCodEstado"].ToString());

                oEDocumentoOriginal.CodigoExterno = idNovedad;
                oEDocumentoOriginal.Metadata = string.Empty; //Utils.GetMetadata(asyncfuImajenCer.FileBytes);            
                oEDocumentoOriginal.Descripcion = string.Empty;

                if (System.IO.File.Exists(rutaServidor_nombreDeArchivo))
                {
                    File.Delete(rutaServidor_nombreDeArchivo);
                }

                DigitalizacionWS.DigitalizacionServicio srv = new DigitalizacionWS.DigitalizacionServicio();
                srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
                srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
                MyLog += " | voy a GuardarEDocumentoV2--> Id:" + oEDocumentoOriginal.Id.ToString() + " TipoImagen: " + oEDocumentoOriginal.TipoEDocumentoId + " CodSistema: " + oEDocumentoOriginal.CodigoSistema + " Estado: " + oEDocumentoOriginal.EstadoEDocumentoId;              
                srv.GuardarEDocumentoV2(oEDocumentoOriginal);
                documentoScaneado.IdImagen = oEDocumentoOriginal.Id;
                // Guardamos el archivo en el FS  
                MyLog += " | voy a Guardar en directorio: " + rutaServidor_nombreDeArchivo; 
                File.WriteAllBytes(rutaServidor_nombreDeArchivo, documentoScaneado.Imagen);
               
                return id;
            }
            catch (Exception err)
            {
                log.Error("MyLog :" + MyLog);
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("XX-Error al guardar Archivo");
                //return "Se produjo un error al guardar el archivo<br>Reintente en otro momento.";
            }
        }

        private string DigiWeb_DirectorioDocScaneada()
        {
            DigitalizacionWS.DigitalizacionServicio srv = new DigitalizacionWS.DigitalizacionServicio();
            srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
            srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
            
            return srv.CalcularRutaSistema(ConfigurationManager.AppSettings["DigiWebCodSistema"]);   
        }

        #endregion

    }    
}
