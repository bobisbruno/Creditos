using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Configuration;
using System.Data;
using System.Data.Common;
using NullableReaders;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class NovedadTransDAO
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NovedadTransDAO).Name);

        #region *** Transaccional ***

        #region Alta Novedad

        #region TIPO1 - TIPO6

        public static string Novedades_Alta(long idPrestador, long idBeneficiario, byte tipoConcepto, int conceptoOPP, double impTotal,
                                    byte cantCuotas, Single porcentaje, string nroComprobante, string ip, string usuario, int mensual)
        {
            byte idEstadoReg;
            DateTime fecNovedad;
            string mensaje = string.Empty;
            string retorno = string.Empty;
            string respta = string.Empty;
            Boolean esAfiliacion = false;

            try
            {
                esAfiliacion = true;
                fecNovedad = DateTime.Now;
                idEstadoReg = 1;

                using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    respta = Valido_Nov(idPrestador, idBeneficiario, tipoConcepto, conceptoOPP, impTotal, cantCuotas, porcentaje, 6, nroComprobante);

                    mensaje = respta.Split('|')[0].ToString();

                    if (mensaje != String.Empty)
                    {
                        retorno = string.Concat(mensaje, "|0|");
                    }
                    else
                    {
                        esAfiliacion = Boolean.Parse(respta.Split(char.Parse("|"))[1].ToString());

                        switch (tipoConcepto)
                        {
                            case 1:
                            case 6:
                                if (esAfiliacion == true)
                                    retorno = Novedades_T1o6_Alta_Afiliacion(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP,
                                                                            impTotal, porcentaje, nroComprobante, ip, usuario, mensual, idEstadoReg);
                                else
                                    retorno = Novedades_T1o6_Alta_No_Afiliacion(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP,
                                                                                impTotal, porcentaje, nroComprobante, ip, usuario, mensual, idEstadoReg);
                                break;
                            case 2: //ok
                                retorno = Novedades_T2_Alta(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP, impTotal,
                                                            nroComprobante, ip, usuario, mensual, idEstadoReg);
                                break;
                            case 3:
                                retorno = Novedades_T3_Alta(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP, impTotal,
                                                            cantCuotas, nroComprobante, ip, usuario, mensual, idEstadoReg);
                                break;
                            default:
                                retorno = "Operación inválida|0|";
                                break;
                        }
                    }

                    //09/01/12 - $3b@
                    string mensajeError = retorno.Split(char.Parse("|"))[0].ToString().Trim();
                    if (mensajeError != string.Empty)
                    {
                        NovedadRechazada_Alta(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP, string.Empty, impTotal,
                                              cantCuotas, porcentaje, 6, nroComprobante, ip, usuario, mensual, mensajeError);
                    }

                    oTransactionScope.Complete();
                }

                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        } 

        #region Novedades_T1o6_Alta_Afiliacion
        private static string Novedades_T1o6_Alta_Afiliacion(long idPrestador, long idBeneficiario, DateTime fecNovedad,
                                                             byte tipoConcepto, int conceptoOPP, double impTotal, Single porcentaje,
                                                             string nroComprobante, string ip, string usuario, int mensual, byte idEstadoReg)
        {
            double importe;
            long idNovedad;
            byte codMovimiento = 6;
            byte cantCuotas = 0;
            string mensaje = String.Empty;
            string retorno = string.Empty;
            string mac = String.Empty;
            String[] alta = new String[2];
            List<Novedad> oListNovedades = new List<Novedad>(); ;

            try
            {
                oListNovedades = Novedades_Trae_TCMov(idPrestador, idBeneficiario, conceptoOPP);

                if (oListNovedades.Count != 0 && (oListNovedades[0].UnCodMovimiento.CodMovimiento == 5 ||
                    oListNovedades[0].UnCodMovimiento.CodMovimiento == 6))
                    mensaje = "Solo se puede ingresar una novedad para el concepto ingresado";

                importe = Calc_Importe_1o6(idBeneficiario, tipoConcepto, porcentaje, impTotal);

                if (mensaje == String.Empty)
                {
                    if (tipoConcepto == 6)
                    {
                        mensaje = CtrolTopeXCpto(idPrestador, tipoConcepto, conceptoOPP, porcentaje);
                        impTotal = 0;
                    }
                    else
                    {
                        mensaje = CtrolTopeXCpto(idPrestador, tipoConcepto, conceptoOPP, importe);
                        porcentaje = 0;
                    }
                }

                if (mensaje == String.Empty)
                {
                    mensaje = CtrolAlcanza(idBeneficiario, importe, idPrestador, conceptoOPP);

                }
                if (mensaje == String.Empty && oListNovedades.Count != 0)
                {
                    if (oListNovedades[0].UnCodMovimiento.CodMovimiento == 4)// esta en novedades
                    {
                        idNovedad = oListNovedades[0].IdNovedad;  //long.Parse(ds.Tables[0].Rows[0]["IdNovedad"].ToString());

                        switch (oListNovedades[0].UnEstadoReg.IdEstado)
                        {
                            case 1:
                                codMovimiento = 5;
                                Novedades_PasaAHist(idNovedad, 0, 7, 3, 0, ip, usuario);
                                break;
                            case 2:
                            case 3:
                                Novedades_Modifica_EstadoReg(idNovedad, 12, ip, usuario);
                                break;
                            case 4:
                                Novedades_PasaAHist(idNovedad, 0, 7, 3, 0, ip, usuario);
                                break;
                        }
                    }
                    //Actualizo el total de novedades cargadas
                }
                if (mensaje == String.Empty)
                {                   
                        ModificaSaldo(idPrestador, idBeneficiario, conceptoOPP, importe, usuario);

                        //Doy alta la nueva novedad
                        alta = Novedades_Alta_Fisica(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP, impTotal,
                                                     cantCuotas, porcentaje, codMovimiento, nroComprobante, ip, usuario, idEstadoReg);
                     
                       retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                }
                else
                    retorno = mensaje + "|0|";

                return (retorno);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region Novedades_T1o6_Alta_No_Afiliacion
        private static string Novedades_T1o6_Alta_No_Afiliacion(long idPrestador, long idBeneficiario, DateTime fecNovedad,
                                                                byte tipoConcepto, int conceptoOPP, double impTotal, Single porcentaje,
                                                                string nroComprobante, string ip, string usuario, int mensual, byte idEstadoReg)
        {

            string mensaje = String.Empty;
            string retorno;
            DataSet ds = new DataSet();
            double importe;
            byte codMovimiento = 6;
            byte cantCuotas = 0;
            string mac = String.Empty;
            String[] alta = new String[2];

            try
            {

                importe = Calc_Importe_1o6(idBeneficiario, tipoConcepto, porcentaje, impTotal);

                if (mensaje == String.Empty)
                {
                    if (tipoConcepto == 6)
                    {
                        impTotal = 0;
                    }
                    else
                    {
                        porcentaje = 0;
                    }
                }

                if (mensaje == String.Empty)
                {
                    mensaje = CtrolAlcanza(idBeneficiario, importe, idPrestador, conceptoOPP);
                }

                if (mensaje == String.Empty)
                {                   
                        //Actualizo el total de novedades cargadas
                        ModificaSaldo(idPrestador, idBeneficiario, conceptoOPP, importe, usuario);

                        //Doy alta la nueva novedad
                        alta = Novedades_Alta_Fisica(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP, impTotal,
                                                    cantCuotas, porcentaje, codMovimiento, nroComprobante, ip, usuario, idEstadoReg);
                    
                        retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                }
                else
                {
                    retorno = mensaje + "|0|";
                }
                return (retorno);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                ds.Dispose();
            }
        }
        #endregion

        #endregion

        #region TIPO1 - TIPO2 - TIPO6 Alta Fisica Novedad
        private static String[] Novedades_Alta_Fisica(long idPrestador, long idBeneficiario, DateTime fecNovedad,
                                                      byte tipoConcepto, int conceptoOPP, double impTotal, byte cantCuotas, Single porcentaje,
                                                      byte codMovimiento, string nroComprobante, string ip, string usuario, byte idEstadoReg)
        {
           
            string dato = Genera_Datos_para_MAC(idBeneficiario, idPrestador, fecNovedad, codMovimiento, conceptoOPP, tipoConcepto,
                                                impTotal, cantCuotas, porcentaje, nroComprobante, ip, usuario);

            string MAC = Utilidades.Calculo_MAC(dato);
            
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_A");

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    String[] retorno = new String[2];

                    db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                    db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                    db.AddInParameter(dbCommand, "@FecNovedad", DbType.DateTime, fecNovedad);
                    db.AddInParameter(dbCommand, "@CodMovimiento", DbType.Byte, codMovimiento);
                    db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Byte, tipoConcepto);
                    db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, conceptoOPP);
                    db.AddInParameter(dbCommand, "@ImporteTotal", DbType.Decimal, impTotal);
                    db.AddInParameter(dbCommand, "@CantCuotas", DbType.Byte, cantCuotas);
                    db.AddInParameter(dbCommand, "@Porcentaje", DbType.Decimal, porcentaje);
                    db.AddInParameter(dbCommand, "@MAC", DbType.AnsiString, MAC);
                    db.AddInParameter(dbCommand, "@NroComprobante", DbType.AnsiString, nroComprobante);  /* OJO en un futuro se va a exigir cargar el nro de comprobante */
                    db.AddInParameter(dbCommand, "@Usuario", DbType.AnsiString, usuario);
                    db.AddInParameter(dbCommand, "@IP", DbType.AnsiString, ip);
                    db.AddInParameter(dbCommand, "@IdEstadoReg", DbType.Byte, idEstadoReg);
                    db.AddOutParameter(dbCommand, "@IdNovedad", DbType.Int64, 0);

                    db.ExecuteNonQuery(dbCommand);

                    retorno[0] = db.GetParameterValue(dbCommand, "@IdNovedad").ToString();
                    retorno[1] = MAC;
                    scope.Complete(); 
                    return retorno;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en Novedades_A - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }          
        }
        #endregion TIPO1 - TIPO6 Alta Fisica Novedad
        
        #region TIPO2
        private static string Novedades_T2_Alta(long idPrestador, long idBeneficiario, DateTime fecNovedad,        
                                                byte tipoConcepto, int conceptoOPP, double impTotal,
                                                string nroComprobante, string ip, string usuario,
                                                int mensual, byte idEstadoReg)
        {
            try
            {
                string mensaje = String.Empty;
                String[] alta = new String[2];
                byte codMovimiento = 6;
                string retorno;

                mensaje = CtrolAlcanza(idBeneficiario, impTotal, idPrestador, conceptoOPP);

                if (mensaje == String.Empty)
                {
                    ModificaSaldo(idPrestador, idBeneficiario, conceptoOPP, impTotal, usuario);

                    alta = Novedades_Alta_Fisica(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP, impTotal,
                                                 1, 0, codMovimiento, nroComprobante, ip, usuario, idEstadoReg);

                    retorno = String.Format(" |{0}|{1}", alta[0].ToString(), alta[1].ToString());                       
                 }
                else 
                {
                    retorno = mensaje + "|0| ";
                }

                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion TIPO2

        #region TIPO3

        #region TIPO3 - Alta Novedad
        private static string Novedades_T3_Alta(long idPrestador, long idBeneficiario, DateTime fecNovedad, byte tipoConcepto, int conceptoOPP,
                                         double impTotal, byte cantCuotas, string nroComprobante, string ip, string usuario, int mensual, byte idEstadoReg)
        {
            try
            {                
                    String[] alta = new String[2];
                    string mensaje = String.Empty;
                    string retorno = String.Empty;
                    double importe = impTotal / cantCuotas;
                    byte codMovimiento = 6;
                    string MAC = string.Empty;

                    mensaje = CtrolAlcanza(idBeneficiario, importe, idPrestador, conceptoOPP);

                    if (mensaje == String.Empty)
                    {
                        ModificaSaldo(idPrestador, idBeneficiario, conceptoOPP, importe, usuario);

                        alta = Novedades_Alta_Fisica_Tipo3(idPrestador, idBeneficiario, fecNovedad, tipoConcepto,
                                                           conceptoOPP, impTotal, cantCuotas, 0, codMovimiento, 
                                                           nroComprobante, ip, usuario, idEstadoReg, mensual);

                        retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                    }
                    else
                    {
                        retorno = mensaje + "|0| ";
                    }
                   
                    return retorno;               
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en Novedades_T3_Alta - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
            finally
            {}
        }
        #endregion TIPO3 - Alta Novedad

        #region TIPO3 - Alta Fisica Novedad
        private static String[] Novedades_Alta_Fisica_Tipo3(long idPrestador, long idBeneficiario, DateTime fecNovedad,
                                                            byte tipoConcepto, int conceptoOPP, double impTotal, byte cantCuotas, Single porcentaje,
                                                            byte codMovimiento, string nroComprobante, string ip, string usuario, byte idEstadoReg, int mensual)
        {            
            string dato = Genera_Datos_para_MAC(idBeneficiario, idPrestador, fecNovedad, codMovimiento, conceptoOPP, tipoConcepto,
                                                impTotal, cantCuotas, porcentaje, nroComprobante, ip, usuario);

            string MAC = Utilidades.Calculo_MAC(dato);
            String[] retorno = new String[2];

            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_Tipo3_Alta");

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                    db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                    db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, conceptoOPP);
                    db.AddInParameter(dbCommand, "@ImporteTotal", DbType.Decimal, impTotal);
                    db.AddInParameter(dbCommand, "@CantCuotas", DbType.Byte, cantCuotas);
                    db.AddInParameter(dbCommand, "@NroComprobante", DbType.AnsiString, nroComprobante); /* OJO en un futuro se va a exigir cargar el nro de comprobante */
                    db.AddInParameter(dbCommand, "@MAC", DbType.AnsiString, MAC);
                    db.AddInParameter(dbCommand, "@IP", DbType.AnsiString, ip);
                    db.AddInParameter(dbCommand, "@Usuario", DbType.AnsiString, usuario);
                    db.AddInParameter(dbCommand, "@PrimerMensual", DbType.Int32, mensual);
                    db.AddOutParameter(dbCommand, "@IdNovedad", DbType.Int64, 0);

                    db.ExecuteNonQuery(dbCommand);

                    retorno[0] = db.GetParameterValue(dbCommand, "@IdNovedad").ToString();
                    retorno[1] = MAC;
                    scope.Complete(); 
                    return retorno;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en Novedades_Tipo3_Alta - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }      
        }
        #endregion TIPO3 - Alta Fisica Novedad

        #region TIPO3 - Alta Novedad Con Tasa
        public static string Novedades_T3_Alta_ConTasa(long idPrestador, long idBeneficiario,long cuil, DateTime fecNovedad, byte tipoConcepto, int conceptoOPP,
                                                       double impTotal, byte cantCuotas, string nroComprobante, string ip, string usuario, int mensual, byte idEstadoReg,
                                                       decimal montoPrestamo, decimal cuotaTotalMensual, decimal TNA, decimal TEM,
                                                       decimal gastoOtorgamiento, decimal gastoAdmMensual, decimal cuotaSocial, decimal CFTEA,
                                                       decimal CFTNAReal, decimal CFTEAReal, decimal gastoAdmMensualReal, decimal TIRReal, string xmlCuotas,
                                                       int idItem, string nroFactura, string cbu, string nroTarjeta, string otro, string prestadorServicio, string poliza,
                                                       string nroSocio, string nroTicket, int idDomicilioBeneficiario, int idDomicilioPrestador, string nroSucursal, DateTime fVto, 
                                                       DateTime fVtoHabilSiguiente, byte idTipoDocPresentado, DateTime fEstimadaEntrega, bool solicitaTarjetaNominada, string codigoPreAprobado, List<DocumentacionScaneada> docScaneada, string codigoDeBanco, string codigoDeEntidad)
        {
            try
            {
                //using (TransactionScope scope = new TransactionScope())
                //{
                    String[] alta = new String[2];
                    string mensaje = String.Empty;
                    string retorno = String.Empty;
                    byte codMovimiento = 6;
                    string MAC = string.Empty;

                    mensaje = Valido_Nov_T3(idPrestador, idBeneficiario,
                                            tipoConcepto, conceptoOPP, impTotal,
                                            cantCuotas, 0, codMovimiento, nroComprobante,
                                            fecNovedad, ip, usuario, mensual,
                                            montoPrestamo, cuotaTotalMensual, TNA, TEM,
                                            gastoOtorgamiento, gastoAdmMensual, cuotaSocial, CFTEA,
                                            CFTNAReal, CFTEAReal, gastoAdmMensualReal, TIRReal);

                    if (mensaje == String.Empty)
                    {
                        try
                        {
                            alta = Novedades_Alta_Fisica_Tipo3_ConTasa(idBeneficiario, idPrestador,conceptoOPP, impTotal, montoPrestamo,
                                                                       cantCuotas, cuotaTotalMensual, TNA, TEM, gastoOtorgamiento,
                                                                       gastoAdmMensual, cuotaSocial, CFTEA, CFTNAReal,
                                                                       CFTEAReal, gastoAdmMensualReal, TIRReal, nroComprobante, ip,
                                                                       usuario, mensual, xmlCuotas, codMovimiento, fecNovedad, tipoConcepto, idEstadoReg,
                                                                       idItem, nroFactura, cbu, nroTarjeta, otro, prestadorServicio, poliza,
                                                                       nroSocio, nroTicket, idDomicilioBeneficiario, idDomicilioPrestador, nroSucursal, fVto, fVtoHabilSiguiente, idTipoDocPresentado, fEstimadaEntrega, solicitaTarjetaNominada, codigoDeBanco, codigoDeEntidad);

                            retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();                    

                            if (!string.IsNullOrEmpty(codigoPreAprobado))
                                retorno += "|" + CodigoPreAprobacionDAO.Novedades_CodigoPreAprobacion_Modificacion((new CodigoPreAprobado(cuil, codigoPreAprobado, long.Parse(alta[0]), TipoUsoCodPreAprobado.Alta, new Auditoria(usuario, ip, null))));                               

                            ModificaSaldo(idPrestador, idBeneficiario, conceptoOPP, (double)cuotaTotalMensual, usuario);
                            
                            //scope.Complete();
                        }
                        catch (SqlException err)
                        {
                            //Si es una excepcion especifica guardo el error y continuo
                            if (err.Number == 50000)
                            {
                                retorno = err.Message + "|0| ";                                
                            }
                            else
                            {
                                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                                throw new Exception("Error en Novedades_T3_Alta_ConTasa - ERROR: " + err.Message + " - SRC: " + err.Source);
                            }
                        }
                    }
                    else
                    {
                        retorno = mensaje + "|0| ";
                    }

                    return retorno;
                //}
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en Novedades_T3_Alta_ConTasa - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
            finally
            {}
        }
        #endregion TIPO3 - Alta Novedad Con tasa

        #region TIPO3 - Alta Fisica Novedad Con tasa
        private static String[] Novedades_Alta_Fisica_Tipo3_ConTasa(long idBeneficiario, long idPrestador, int codConceptoLiq,
                                                                    double importeTotal, decimal montoPrestamo, byte cantCuotas,
                                                                    decimal cuotaTotalMensual, decimal TNA, decimal TEM,
                                                                    decimal gastoOtorgamiento, decimal gastoAdmMensual, decimal cuotaSocial,
                                                                    decimal CFTEA, decimal CFTNAReal, decimal CFTEAReal, decimal gastoAdmMensualReal,
                                                                    decimal TIRReal, string nroComprobante, string ip, string usuario, int primerMensual,
                                                                    string cuotas, byte codMovimiento, DateTime fecNovedad, byte tipoConcepto, byte idEstadoReg,
                                                                    int idItem, string nroFactura, string cbu, string nroTarjeta, string otro, 
                                                                    string prestadorServicio, string poliza, string nroSocio, string nroTicket,
                                                                    int idDomicilioBeneficiario, int idDomicilioPrestador, string nroSucursal,
                                                                    DateTime fVto, DateTime fVtoHabilSiguiente, byte idTipoDocPresentado,
                                                                    DateTime fEstimadaEntrega, bool solicitaTarjetaNominada, string codigoDeBanco, string codigoDeSucursal)
        {           
            string dato = Genera_Datos_para_MAC(idBeneficiario, idPrestador,fecNovedad,
												codMovimiento,codConceptoLiq,tipoConcepto,
												importeTotal,cantCuotas,0,nroComprobante,ip,usuario);
					
			string MAC = Utilidades.Calculo_MAC(dato);
                      
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_Tipo3_AltaConTasas_V2");

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    String[] retorno = new String[2];
                    db.AddInParameter(dbCommand, "@idbeneficiario", DbType.Int64, idBeneficiario);
                    db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                    db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, codConceptoLiq);
                    db.AddInParameter(dbCommand, "@importeTotal", DbType.Decimal, importeTotal);
                    db.AddInParameter(dbCommand, "@montoPrestamo", DbType.Decimal, montoPrestamo);
                    db.AddInParameter(dbCommand, "@CantCuotas", DbType.Byte, cantCuotas);
                    db.AddInParameter(dbCommand, "@CuotaTotalMensual", DbType.Decimal, cuotaTotalMensual);
                    db.AddInParameter(dbCommand, "@TNA", DbType.Decimal, TNA);
                    db.AddInParameter(dbCommand, "@TEM", DbType.Decimal, TEM);
                    db.AddInParameter(dbCommand, "@gastoOtorgamiento", DbType.Decimal, gastoOtorgamiento);
                    db.AddInParameter(dbCommand, "@gastoAdmMensual", DbType.Decimal, gastoAdmMensual);
                    db.AddInParameter(dbCommand, "@cuotaSocial", DbType.Decimal, cuotaSocial);
                    db.AddInParameter(dbCommand, "@CFTEA", DbType.Decimal, CFTEA);
                    db.AddInParameter(dbCommand, "@CFTNAReal", DbType.Decimal, CFTNAReal);
                    db.AddInParameter(dbCommand, "@CFTEAReal", DbType.Decimal, CFTEAReal);
                    db.AddInParameter(dbCommand, "@gastoAdmMensualReal", DbType.Decimal, gastoAdmMensualReal);
                    db.AddInParameter(dbCommand, "@TIRReal", DbType.Decimal, TIRReal);
                    db.AddInParameter(dbCommand, "@NroComprobante", DbType.AnsiString, nroComprobante);
                    db.AddInParameter(dbCommand, "@MAC", DbType.AnsiString, MAC);
                    db.AddInParameter(dbCommand, "@IP", DbType.AnsiString, ip);
                    db.AddInParameter(dbCommand, "@Usuario", DbType.AnsiString, usuario);
                    db.AddInParameter(dbCommand, "@PrimerMensual", DbType.Int32, primerMensual);
                    db.AddInParameter(dbCommand, "@cuotas", DbType.AnsiString, cuotas);
                    db.AddInParameter(dbCommand, "@idestadoReg", DbType.Int32, idEstadoReg);
                    db.AddInParameter(dbCommand, "@idItem", DbType.Int32, idItem);
                    db.AddInParameter(dbCommand, "@nroFactura", DbType.AnsiString, nroFactura);
                    db.AddInParameter(dbCommand, "@cbu", DbType.AnsiString, cbu);
                    db.AddInParameter(dbCommand, "@otro", DbType.AnsiString, otro);
                    db.AddInParameter(dbCommand, "@prestadorServicio", DbType.AnsiString, prestadorServicio);
                    db.AddInParameter(dbCommand, "@poliza", DbType.AnsiString, poliza);
                    db.AddInParameter(dbCommand, "@nroSocio", DbType.AnsiString, nroSocio);

                    /*if (nroTarjeta == null || nroTarjeta == string.Empty)
                        db.AddInParameter(dbCommand, "@nroTarjeta", DbType.AnsiString, DBNull.Value);
                    else
                        db.AddInParameter(dbCommand, "@nroTarjeta", DbType.AnsiString, nroTarjeta);*/
                    db.AddInParameter(dbCommand, "@nroTarjeta", DbType.AnsiString, (nroTarjeta == null || nroTarjeta == string.Empty) ? null : nroTarjeta);
                    db.AddInParameter(dbCommand, "@nroTicket", DbType.AnsiString, (nroTicket == null || nroTicket == string.Empty) ? null : nroTicket);
                    db.AddInParameter(dbCommand, "@idDomicilioBeneficiario", DbType.Int32, idDomicilioBeneficiario);
                    db.AddInParameter(dbCommand, "@idDomicilioPrestador", DbType.Int32, idDomicilioPrestador);
                    db.AddInParameter(dbCommand, "@idSucursal", DbType.AnsiString, (nroSucursal == null || nroSucursal == string.Empty) ? null : nroSucursal);

                    if (fVto == DateTime.MinValue)
                        db.AddInParameter(dbCommand, "@fVto", DbType.DateTime, DBNull.Value);
                    else
                        db.AddInParameter(dbCommand, "@fVto", DbType.DateTime, fVto);

                    if (fVtoHabilSiguiente == DateTime.MinValue)
                        db.AddInParameter(dbCommand, "@fVtoHabilSiguiente", DbType.DateTime, DBNull.Value);
                    else
                        db.AddInParameter(dbCommand, "@fVtoHabilSiguiente", DbType.DateTime, fVtoHabilSiguiente);

                    db.AddInParameter(dbCommand, "@idTipoDocPresentado", DbType.Int16, idTipoDocPresentado);
                    db.AddInParameter(dbCommand, "@solicitaTarjetaNominada", DbType.Boolean, solicitaTarjetaNominada);

                    if (fEstimadaEntrega == DateTime.MinValue)
                        db.AddInParameter(dbCommand, "@fEstimadaEntrega", DbType.DateTime, DBNull.Value);
                    else
                        db.AddInParameter(dbCommand, "@fEstimadaEntrega", DbType.DateTime, fVto);

                    db.AddInParameter(dbCommand, "@codbanco", DbType.AnsiString, codigoDeBanco);
                    db.AddInParameter(dbCommand, "@codAgencia", DbType.AnsiString, codigoDeSucursal);


                    db.AddOutParameter(dbCommand, "@IdNovedad", DbType.Int64, 0);

                    db.ExecuteNonQuery(dbCommand);

                    retorno[0] = db.GetParameterValue(dbCommand, "@IdNovedad").ToString();
                    retorno[1] = MAC;
                    scope.Complete();
                    return retorno;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en Novedades_Alta_Fisica_Tipo3_ConTasa - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion TIPO3 - Alta Fisica Novedad Con Tasa

        #region Alta Novedad Seguros Banco Nacion
        public static string Novedades_Alta(long idPrestador, long idBeneficiario, short tipoConcepto, int conceptoOPP, double impTotal,
                                            byte cantCuotas, Single porcentaje, string nroComprobante, string ip, string usuario,
                                            int mensual, List<Adherente> unaLista_Adherentes)
        {
            try
            {
                string retorno = string.Empty;

                using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                {

                    retorno = Novedades_Alta(idPrestador, idBeneficiario, Convert.ToByte(tipoConcepto), conceptoOPP, impTotal, cantCuotas, porcentaje,
                                             nroComprobante, ip, usuario, mensual);

                    if (string.IsNullOrEmpty(retorno.Split(char.Parse("|"))[0].ToString().Trim()))
                    {
                        long idNovedad = long.Parse(retorno.Split(char.Parse("|"))[1].ToString().Trim());

                        foreach (Adherente unA in unaLista_Adherentes)
                        {
                            string sql = "Adherente_Alta";
                            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
                            DbCommand dbCommand = db.GetStoredProcCommand(sql);

                            db.AddInParameter(dbCommand, "@idNovedad", DbType.Int64, idNovedad);
                            db.AddInParameter(dbCommand, "@cuilAdherente", DbType.Int64, unA.CUIL);
                            db.AddInParameter(dbCommand, "@apellidoNombre", DbType.String, unA.Apellido_Nombre);
                            db.AddInParameter(dbCommand, "@ip", DbType.String, ip);
                            db.AddInParameter(dbCommand, "@usuario", DbType.String, usuario);

                            db.ExecuteNonQuery(dbCommand);

                            dbCommand.Dispose();
                        }
                    }
                    oTransactionScope.Complete();
                }
                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region Rechaza Novedades Con Tasas
        public static void Novedades_Rechazadas_A_ConTasas(long idBeneficiario, long idPrestador, byte codMovimiento,
			                                               byte tipoConcepto, int codConceptoLiq, double importeTotal,
			                                               byte cantCuotas, Single porcentaje, string nroComprobante, 
                                                           string ip, string usuario, DateTime fecRechazo, decimal montoPrestamo,
			                                               decimal CuotaTotalMensual, decimal TNA, decimal TEM, 
                                                           decimal gastoOtorgamiento, decimal gastoAdmMensual, decimal cuotaSocial,
			                                               decimal CFTEA, decimal CFTNAReal, decimal CFTEAReal,
			                                               decimal gastoAdmMensualReal, decimal TIRReal, string mensajeError)
		{			
			Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_Rechazadas_A_ConTasas");
        
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                    db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                    db.AddInParameter(dbCommand, "@CodMovimiento", DbType.Byte, codMovimiento);
                    db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Byte, tipoConcepto);
                    db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, codConceptoLiq);
                    db.AddInParameter(dbCommand, "@ImporteTotal", DbType.Decimal, importeTotal);
                    db.AddInParameter(dbCommand, "@CantCuotas", DbType.Byte, cantCuotas);
                    db.AddInParameter(dbCommand, "@Porcentaje", DbType.Decimal, porcentaje);
                    db.AddInParameter(dbCommand, "@NroComprobante", DbType.AnsiString, nroComprobante);//Como limito la cantidad a 50
                    db.AddInParameter(dbCommand, "@IP", DbType.AnsiString, ip);//Como limito la cantidad a 20
                    db.AddInParameter(dbCommand, "@Usuario", DbType.AnsiString, usuario);//Como limito la cantidad a 50
                    db.AddInParameter(dbCommand, "@montoPrestamo", DbType.Decimal, montoPrestamo);
                    db.AddInParameter(dbCommand, "@CuotaTotalMensual", DbType.Decimal, CuotaTotalMensual);
                    db.AddInParameter(dbCommand, "@TNA", DbType.Decimal, TNA);
                    db.AddInParameter(dbCommand, "@TEM", DbType.Decimal, TEM);
                    db.AddInParameter(dbCommand, "@gastoOtorgamiento", DbType.Decimal, gastoOtorgamiento);
                    db.AddInParameter(dbCommand, "@gastoAdmMensual", DbType.Decimal, gastoAdmMensual);
                    db.AddInParameter(dbCommand, "@cuotaSocial", DbType.Decimal, cuotaSocial);
                    db.AddInParameter(dbCommand, "@CFTEA", DbType.Decimal, CFTEA);
                    db.AddInParameter(dbCommand, "@CFTNAReal", DbType.Decimal, CFTNAReal);
                    db.AddInParameter(dbCommand, "@CFTEAReal", DbType.Decimal, CFTEAReal);
                    db.AddInParameter(dbCommand, "@gastoAdmMensualReal", DbType.Decimal, gastoAdmMensual);
                    db.AddInParameter(dbCommand, "@TIRReal", DbType.Decimal, TIRReal);
                    db.AddInParameter(dbCommand, "@TipoRechazo", DbType.AnsiString, mensajeError);//Como limito la cantidad a 300

                    db.ExecuteNonQuery(dbCommand);
                    scope.Complete();
                }
			}
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en Novedades_Rechazadas_A_ConTasas - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
		}
        #endregion Rechaza Novedades Con Tasas

        #region NovedadRechazada_Alta
        private static void NovedadRechazada_Alta(long idPrestador, long idBeneficiario, DateTime fecNovedad,
                                                  byte tipoConcepto, int codConceptoLiq, string mac, double importeTotal,
                                                  byte cantCuotas, Single porcentaje, byte codMovimiento,
                                                  string nroComprobante, string ip, string usuario, int mensual, string mensajeError)
        {
            string sql = "Novedades_Rechazadas_A";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@CodMovimiento", DbType.Int16, codMovimiento);
                db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Int16, tipoConcepto);
                db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, codConceptoLiq);
                db.AddInParameter(dbCommand, "@ImporteTotal", DbType.Decimal, importeTotal);
                db.AddInParameter(dbCommand, "@CantCuotas", DbType.Int16, cantCuotas);
                db.AddInParameter(dbCommand, "@Porcentaje", DbType.Decimal, porcentaje);
                db.AddInParameter(dbCommand, "@NroComprobante", DbType.String, nroComprobante);
                db.AddInParameter(dbCommand, "@IP", DbType.String, ip);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, usuario);
                db.AddInParameter(dbCommand, "@FecRechazo", DbType.DateTime, DateTime.Today);
                db.AddInParameter(dbCommand, "@TipoRechazo", DbType.String, mensajeError);

                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion

        #region TIPO3 - Valido Novedad
        public static string Valido_Nov_T3(long idPrestador, long idBeneficiario, byte tipoConcepto, int conceptoOPP,
                                           double impTotal, byte cantCuotas, Single porcentaje, byte codMovimiento, 
                                           String nroComprobante, DateTime fecNovedad, string ip, string usuario, 
                                           int mensual, decimal montoPrestamo, decimal cuotaTotalMensual, decimal TNA, 
                                           decimal TEM, decimal gastoOtorgamiento, decimal gastoAdmMensual,
                                           decimal cuotaSocial, decimal CFTEA, decimal CFTNAReal, decimal CFTEAReal,
                                           decimal gastoAdmMensualReal, decimal TIRReal)
        {
            return Valido_Nov_T3_Gestion(idPrestador, idBeneficiario, tipoConcepto, conceptoOPP, impTotal, cantCuotas,
                                         porcentaje, codMovimiento, nroComprobante, fecNovedad, ip, usuario, mensual,
                                         montoPrestamo, cuotaTotalMensual, TNA, TEM, gastoOtorgamiento, gastoAdmMensual,
                                         cuotaSocial, CFTEA, CFTNAReal, CFTEAReal, gastoAdmMensualReal, TIRReal, true);
        }

        public static string Valido_Nov_T3_Gestion(long idPrestador, long idBeneficiario, byte tipoConcepto, int conceptoOPP,
                                                   double  impTotal, byte cantCuotas, Single porcentaje, byte codMovimiento,
                                                   String nroComprobante, DateTime fecNovedad, string ip, string usuario,
                                                   int mensual, decimal montoPrestamo, decimal cuotaTotalMensual, decimal TNA,
                                                   decimal TEM, decimal gastoOtorgamiento, decimal gastoAdmMensual,
                                                   decimal cuotaSocial, decimal CFTEA, decimal CFTNAReal, decimal CFTEAReal, 
                                                   decimal gastoAdmMensualReal, decimal TIRReal, bool bGestionErrores)
        {

            try
            {
                string mensaje = String.Empty;
                mensaje = Valido_Nov(idPrestador, idBeneficiario, tipoConcepto, conceptoOPP, impTotal, cantCuotas, porcentaje, codMovimiento, nroComprobante, bGestionErrores, montoPrestamo);
                mensaje = mensaje.Split(char.Parse("|"))[0].ToString().Trim();

                if (mensaje == String.Empty)
                {
                    mensaje = CtrolAlcanza(idBeneficiario, (double)cuotaTotalMensual, idPrestador, conceptoOPP);             
                }
                      
                if (mensaje != String.Empty && bGestionErrores)
                {
                    Novedades_Rechazadas_A_ConTasas(idBeneficiario, idPrestador, codMovimiento,
                                                    tipoConcepto, conceptoOPP, impTotal, cantCuotas,
                                                    porcentaje, nroComprobante, ip, usuario, fecNovedad,
                                                    montoPrestamo, cuotaTotalMensual, TNA, TEM,
                                                    gastoOtorgamiento, gastoAdmMensual, cuotaSocial,
                                                    CFTEA, CFTNAReal, CFTEAReal, gastoAdmMensualReal, TIRReal, mensaje);
                }

                return mensaje;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en Valido_Nov_T3_Gestion - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
            finally
            {}
        }
        #endregion TIPO3 - Valido Novedad

        #endregion TIPO3               

        #endregion Alta Novedad

        #region Pasa a Historico Novedad
        private static void Novedades_PasaAHist(long idNovedad, int mensual, int idEstadoReg, byte idEstadoNov,
                                               double importeLiq, string ip, string usuario)
        {
            string sql = "Novedades_PaHist";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, idNovedad);
                db.AddInParameter(dbCommand, "@Mensual", DbType.Int32, mensual);
                db.AddInParameter(dbCommand, "@IdEstadoReg", DbType.Int16, idEstadoReg);
                db.AddInParameter(dbCommand, "@IdEstadoNov", DbType.Int16, idEstadoNov);
                db.AddInParameter(dbCommand, "@ImporteLiq", DbType.Decimal, importeLiq);
                db.AddInParameter(dbCommand, "@IP", DbType.String, ip);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, usuario);

                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion Pasa a Historico

        #region Modifica Novedad              

        #region Novedades_Modificacion
        public static string Novedades_Modificacion(long idNovedadAnt, double ImpTotalN, Single PorcentajeN, string NroComprobanteN,
                                                    string IPN, string UsuarioN, int mensual, Boolean masiva)
        {
            Novedad oNovedad = new Novedad();
            List<Novedad> oListNovedades = new List<Novedad>();

            try
            {

                string mensaje = String.Empty;
                string retorno;

                long IdPrestador = 0;
                long IdBeneficiario = 0;
                short TipoConcepto = 0;
                double ImpTotalV = 0;
                Single PorcentajeV = 0;
                int IdEstadoRegV = 0;
                byte CodMovimientoV = 0;
                int ConceptoOPP = 0;

                string respta = String.Empty;
                Boolean EsAfiliacion = false;

                oListNovedades = NovedadDAO.Novedades_TxIdNovedad_Sliq(idNovedadAnt);

                if (oListNovedades.Count == 0)
                    mensaje = "No existe la novedad a modificar";
                else
                {
                    IdPrestador = oListNovedades[0].UnPrestador.ID;
                    IdBeneficiario = oListNovedades[0].UnBeneficiario.IdBeneficiario;
                    TipoConcepto = oListNovedades[0].UnTipoConcepto.IdTipoConcepto;
                    ImpTotalV = oListNovedades[0].ImporteTotal;
                    PorcentajeV = oListNovedades[0].Porcentaje;
                    IdEstadoRegV = oListNovedades[0].UnEstadoReg.IdEstado;
                    CodMovimientoV = oListNovedades[0].UnCodMovimiento.CodMovimiento;
                    ConceptoOPP = oListNovedades[0].UnConceptoLiquidacion.CodConceptoLiq;
                    EsAfiliacion = oListNovedades[0].UnConceptoLiquidacion.EsAfiliacion;
                }

                if (mensaje == String.Empty)
                {
                    if (IdEstadoRegV == 12)
                        mensaje = "No existe la novedad";
                    else
                    {
                        respta = Valido_Nov(IdPrestador, IdBeneficiario, Convert.ToByte(TipoConcepto), ConceptoOPP, ImpTotalN, 1, PorcentajeN, 5, NroComprobanteN);
                        mensaje = respta.Split('|')[0].ToString();
                    }
                }
                if (mensaje == String.Empty)
                {
                    switch (TipoConcepto)
                    {
                        case 1:
                        case 6:
                            retorno = Novedades_T1o6_Modificacion(idNovedadAnt, IdPrestador, IdBeneficiario, TipoConcepto, ConceptoOPP, ImpTotalN,
                                                                    PorcentajeN, NroComprobanteN, IPN, UsuarioN, mensual, IdEstadoRegV, ImpTotalV,
                                                                    PorcentajeV, CodMovimientoV, masiva, EsAfiliacion);
                            break;
                        case 2:
                            retorno = Novedades_T2_Modificacion(idNovedadAnt, IdPrestador, IdBeneficiario, TipoConcepto, ConceptoOPP, ImpTotalN,
                                                                NroComprobanteN, IPN, UsuarioN, IdEstadoRegV, ImpTotalV);
                            break;
                        default:
                            retorno = "Operación inválida|0|";
                            break;
                    }
                }
                else
                {
                    retorno = mensaje + "|0| ";
                }

                //09/01/12 - $3b@
                string mensajeError = retorno.Split(char.Parse("|"))[0].ToString().Trim();
                if (mensajeError != string.Empty)
                {
                    NovedadRechazada_Alta(IdPrestador, IdBeneficiario, DateTime.Now, Convert.ToByte(TipoConcepto), ConceptoOPP, string.Empty, ImpTotalN, 0,
                                            PorcentajeN, 5, NroComprobanteN, IPN, UsuarioN, mensual, mensajeError);
                }

                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region Novedades_Modificacion (Con Adherente)
        public static string Novedades_Modificacion(long idNovedadAnt, double ImpTotalN, Single PorcentajeN, string NroComprobanteN, string IPN,
                                                    string UsuarioN, int mensual, Boolean masiva, List<Adherente> unaLista_Adherentes)
        {
            try
            {
                string retorno = string.Empty;

                using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                {

                    retorno = Novedades_Modificacion(idNovedadAnt, ImpTotalN, PorcentajeN, NroComprobanteN, IPN,
                                                     UsuarioN, mensual, masiva);

                    if (string.IsNullOrEmpty(retorno.Split(char.Parse("|"))[0].ToString().Trim()))
                    {
                        long idNovedad = long.Parse(retorno.Split(char.Parse("|"))[1].ToString().Trim());

                        foreach (Adherente unA in unaLista_Adherentes)
                        {
                            string sql = "Adherente_Alta";
                            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
                            DbCommand dbCommand = db.GetStoredProcCommand(sql);

                            db.AddInParameter(dbCommand, "@idNovedad", DbType.Int64, idNovedad);
                            db.AddInParameter(dbCommand, "@cuilAdherente", DbType.Int64, unA.CUIL);
                            db.AddInParameter(dbCommand, "@apellidoNombre", DbType.String, unA.Apellido_Nombre);
                            db.AddInParameter(dbCommand, "@ip", DbType.String, IPN);
                            db.AddInParameter(dbCommand, "@usuario", DbType.String, UsuarioN);

                            db.ExecuteNonQuery(dbCommand);

                            dbCommand.Dispose();
                        }
                    }
                    oTransactionScope.Complete();
                }
                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }

        #endregion

        #region Modificacion_Masiva_Indeterminadas
        public static List<Novedad> Modificacion_Masiva_Indeterminadas(List<Novedad> listNovedades, double monto, string ip, string usuario, bool masiva)
        {
            List<Novedad> oListNovedadesOut = new List<Novedad>();

            try
            {
                if (listNovedades.Count > 0)
                {
                    long idNovedadAnt = 0;
                    int mensual = 0;
                    short tipoConcepto = 0;
                    double impTotN = 0;
                    Single porcentajeN = 0;
                    string comprobante = String.Empty;
                    string mensaje = string.Empty;
                    string msj = string.Empty;


                    foreach (Novedad oNovedad in listNovedades)
                    {
                        //DataRow drNFila = dsSalida.Tables[0].NewRow();
                        Novedad oNovedadOut = new Novedad();

                        tipoConcepto = oNovedad.UnTipoConcepto.IdTipoConcepto;
                        switch (tipoConcepto)
                        {
                            case 1:
                                impTotN = oNovedad.ImporteTotal + monto;
                                porcentajeN = 0;
                                break;
                            case 6:
                                porcentajeN = oNovedad.Porcentaje + float.Parse(monto.ToString());
                                impTotN = 0;
                                break;
                            default:
                                msj = "Tipo de Concepto Erróneo para Modidicación Masiva|0|";
                                break;
                        }

                        idNovedadAnt = oNovedad.IdNovedad;
                        mensual = int.Parse(oNovedad.PrimerMensual);
                        comprobante = oNovedad.Comprobante;

                        if (msj == string.Empty)
                            msj = Novedades_Modificacion(idNovedadAnt, impTotN, porcentajeN, comprobante, ip, usuario,
                                                         mensual, masiva);

                        if (msj.StartsWith("|")) // no hubo mensaje de error al realizar la modificacion
                        {
                            //drNFila["Mensaje"] = String.Empty;                           
                            oNovedadOut.IdNovedad = int.Parse(msj.Split(char.Parse("|"))[1].ToString());
                            oNovedadOut.MAC = msj.Split(char.Parse("|"))[2].ToString();
                            oNovedadOut.FechaNovedad = DateTime.Today;
                            oNovedadOut.ImporteTotal = impTotN;
                            oNovedadOut.Porcentaje = porcentajeN;
                        }
                        else
                        {
                            // no se produjo la modificación por algun motivo
                            //drNFila["Mensaje"] = msj.Split(char.Parse("|"))[0].ToString(); (En caso de que se utilice el metodo ....Ver que onda este campo)
                            oNovedadOut.IdNovedad = idNovedadAnt;
                            oNovedadOut.MAC = oNovedad.MAC;
                            oNovedadOut.FechaNovedad = oNovedad.FechaNovedad;
                            oNovedadOut.ImporteTotal = oNovedad.ImporteTotal;
                            oNovedadOut.Porcentaje = oNovedad.Porcentaje;
                        }

                        oNovedadOut.PrimerMensual = mensual.ToString();
                        oNovedadOut.Comprobante = comprobante;
                        oNovedadOut.UnBeneficiario.IdBeneficiario = oNovedad.UnBeneficiario.IdBeneficiario;
                        oNovedadOut.UnBeneficiario.ApellidoNombre = oNovedad.UnBeneficiario.ApellidoNombre;
                        oNovedadOut.UnBeneficiario.Cuil = oNovedad.UnBeneficiario.Cuil;
                        oNovedadOut.UnBeneficiario.TipoDoc = oNovedad.UnBeneficiario.TipoDoc;
                        oNovedadOut.UnBeneficiario.NroDoc = oNovedad.UnBeneficiario.NroDoc;
                        oNovedadOut.UnPrestador.ID = oNovedad.UnPrestador.ID;
                        oNovedadOut.UnConceptoLiquidacion.CodConceptoLiq = oNovedad.UnConceptoLiquidacion.CodConceptoLiq;
                        oNovedadOut.UnConceptoLiquidacion.DescConceptoLiq = oNovedad.UnConceptoLiquidacion.DescConceptoLiq;
                        oNovedadOut.UnTipoConcepto.IdTipoConcepto = oNovedad.UnTipoConcepto.IdTipoConcepto;

                        //Agrego la fila a la tabla
                        oListNovedadesOut.Add(oNovedadOut);
                    }
                }

                return oListNovedadesOut;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region TIPO1 - TIPO6 Modificacion Novedad
        private static string Novedades_T1o6_Modificacion(long idNovedadAnt, long idPrestador, long idBeneficiario, short tipoConcepto,
                                                   int conceptoOPP, double impTotalN, Single porcentajeN, string nroComprobanteN,
                                                   string ip, string usuarioN, int mensual, int idEstadoRegV, double impTotalV,
                                                   Single porcentajeV, int codMovimientoV, Boolean masiva, Boolean esAfiliacion)
        {
            try
            {
                string retorno = String.Empty;
                string mensaje = String.Empty;
                byte codMovimientoN;
                byte idEstadoRegN = 1;
                double importeN = 0;
                double importeV = 0;
                DateTime fecNovedad = DateTime.Today;
                double val = 0;
                byte cantCuotas = 0;
                String[] alta = new String[2];
                codMovimientoN = 5;

                if (codMovimientoV == 4)
                    mensaje = "Novedad inexistente";

                if (mensaje == String.Empty)
                {
                    if (esAfiliacion == true)
                    {
                        if (tipoConcepto == 6)
                            mensaje = CtrolTopeXCpto(idPrestador, tipoConcepto, conceptoOPP, porcentajeN);
                        else
                            mensaje = CtrolTopeXCpto(idPrestador, tipoConcepto, conceptoOPP, impTotalN);
                    }
                    else
                        codMovimientoN = 6;
                }

                using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (mensaje == String.Empty)
                    {
                        importeN = Calc_Importe_1o6(idBeneficiario, tipoConcepto, porcentajeN, impTotalN);
                        importeV = Calc_Importe_1o6(idBeneficiario, tipoConcepto, porcentajeV, impTotalV);
                        fecNovedad = DateTime.Today;
                        val = importeN - importeV;
                        cantCuotas = 0;
                        if (val > 0 && masiva == false)
                        {
                            mensaje = CtrolAlcanza(idBeneficiario, val, idPrestador, conceptoOPP);

                            #region
                            //09/01/12 - $3b@ SE COMENTA LO SIG PORQUE SE GRABAN TODOS LOS ERRORES
                            //if (mensaje != String.Empty)
                            //{
                            //    NovedadRechazada_Alta(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP,
                            //                          String.Empty, impTotalN, cantCuotas, porcentajeN, codMovimientoN, nroComprobanteN, ip, usuarioN, mensual);
                            //}
                            #endregion
                        }
                    }
                    if (mensaje == String.Empty)
                    {
                        ModificaSaldo(idPrestador, idBeneficiario, conceptoOPP, val, usuarioN);

                        switch (idEstadoRegV)
                        {
                            case 2:
                            case 3:
                            case 14:
                            case 15:
                                Novedades_Modifica_EstadoReg(idNovedadAnt, 12, ip, usuarioN);
                                break;
                            case 1:
                                if (codMovimientoV == 6)
                                {
                                    codMovimientoN = 6;
                                }
                                Novedades_PasaAHist(idNovedadAnt, mensual, 7, 3, 0, ip, usuarioN);
                                break;
                            case 4:
                            case 13:
                                Novedades_PasaAHist(idNovedadAnt, mensual, 7, 3, 0, ip, usuarioN);
                                break;
                        }

                        alta = Novedades_Alta_Fisica(idPrestador, idBeneficiario, fecNovedad, Convert.ToByte(tipoConcepto), conceptoOPP,
                                                     impTotalN, cantCuotas, porcentajeN, codMovimientoN, nroComprobanteN, ip, usuarioN, idEstadoRegN);


                        retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                    }
                    else
                        retorno = mensaje + "|0|";

                    oTransactionScope.Complete();
                }
                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region  TIPO2 Modificacion Novedad
        private static string Novedades_T2_Modificacion(long idNovedadAnt, long idPrestador, long idBeneficiario,
                                                 short tipoConcepto, int conceptoOPP, double impTotalN,
                                                 string nroComprobanteN, string ip, string usuarioN,
                                                 int idEstadoRegV, double impTotalV)
        {
            try
            {
                string mensaje = String.Empty;
                string retorno = String.Empty;
                String[] alta = new String[2];
                DateTime fecNovedad = DateTime.Today;
                double importe = 0;
                byte codMovimiento = 5;
                byte idEstadoRegN = 1;

                // busco la novedad a modificar
                if (idEstadoRegV != 1)
                    // para novedades en proceso de liquidación o en transito a la misma
                    mensaje = "Novedad en proceso de liquidación. No puede modificarse";

                using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (mensaje == String.Empty)
                    {
                        // calculo el importe para ver si alcanza el disponible
                        importe = impTotalN - impTotalV;

                        if (importe > 0)
                        {
                            mensaje = CtrolAlcanza(idBeneficiario, importe, idPrestador, conceptoOPP);

                            //09/01/12 - $3b@ SE COMENTA LO SIG PORQUE SE GRABAN TODOS LOS ERRORES
                            //if (mensaje != String.Empty)
                            //{
                            //    NovedadRechazada_Alta(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP, string.Empty,
                            //                          impTotalN, 0, 0, codMovimiento, nroComprobanteN, ip, usuarioN, string.Empty);
                            //}
                        }
                    }
                    if (mensaje == String.Empty)
                    {
                        ModificaSaldo(idPrestador, idBeneficiario, conceptoOPP, importe, usuarioN);
                        Novedades_PasaAHist(idNovedadAnt, 0, 7, 3, 0, ip, usuarioN);
                        alta = Novedades_Alta_Fisica(idPrestador, idBeneficiario, fecNovedad, Convert.ToByte(tipoConcepto), conceptoOPP, impTotalN, 1, 0, codMovimiento, nroComprobanteN, ip, usuarioN, idEstadoRegN);
                        retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                    }
                    else
                        retorno = mensaje + "|0| ";

                    oTransactionScope.Complete();
                }

                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region Novedades_Modifica_EstadoReg
        private static void Novedades_Modifica_EstadoReg(long idNovedad, byte idEstadoReg, string ip, string usuario)
        {
            string sql = "Novedades_M";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, idNovedad);
                db.AddInParameter(dbCommand, "@IdEstadoReg", DbType.Int16, idEstadoReg);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, usuario);
                db.AddInParameter(dbCommand, "@IP", DbType.String, ip);

                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion

        #region Modifica Beneficiarios - Saldo
        private static long ModificaSaldo(long idPrestador, long idBeneficiario, int conceptoOPP, double importe, string usuario)
        {
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Beneficiarios_MSaldo");

            try
            {               
                    db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                    db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                    db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, conceptoOPP);
                    db.AddInParameter(dbCommand, "@Importe", DbType.Decimal, importe);
                    db.AddInParameter(dbCommand, "@Usuario", DbType.AnsiString, usuario);

                    db.ExecuteNonQuery(dbCommand);
                               
                    return 0;
               
            }
            catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en Beneficiarios_MSaldo - ERROR: " + err.Message + " - SRC: " + err.Source);
			}
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion Modifica Beneficiarios - Saldo

        #endregion Modifica

        #region Baja Novedad
        public static string Novedades_Baja(long idNovedadAnt, string ip, string usuario, int mensual)
        {
            string retorno;
            int idEstadoReg;
            short tipoConcepto;

            try
            {
                retorno = String.Empty;
                idEstadoReg = 9;
                // busco la novedad a modificar

                List<Novedad> oListNovedades = NovedadDAO.Novedades_TxIdNovedad_Sliq(idNovedadAnt);

                if (oListNovedades.Count == 0)
                    retorno = "No existe la novedad|0| ";
                else
                {
                    int IdEstadoRegV = oListNovedades[0].UnEstadoReg.IdEstado;
                    if (IdEstadoRegV == 12)
                        retorno = "No existe la novedad|0| ";
                    else
                    {
                        using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                        {
                            tipoConcepto = oListNovedades[0].UnTipoConcepto.IdTipoConcepto;

                            switch (tipoConcepto)
                            {
                                case 1:
                                case 6:
                                    retorno = Novedades_T1o6_Baja(idEstadoReg, ip, usuario, oListNovedades);
                                    break;
                                case 2: //ok
                                    retorno = Novedades_T2_Baja(idEstadoReg, ip, usuario, oListNovedades);
                                    break;
                                case 3:
                                    idEstadoReg = 5;
                                    retorno = Novedades_T3_Baja(idEstadoReg, ip, usuario, mensual, oListNovedades[0]);
                                    break;
                                default:
                                    retorno = "Operación inválida|0| ";
                                    break;
                            }

                            //oTransactionScope.Complete();
                            //09/01/12 - $3b@
                            // NO GRABA RECHAZADO PORQUE NO HAY DATOS P/GRABAR NI POSIBLES ERRORES COPADOS 
                            //06 de junio 2017 - Verificar con Flavia si tiene que validar esto para hacer commit -  en DATWS no esta
                            string mensajeError = retorno.Split(char.Parse("|"))[0].ToString().Trim();
                            if (mensajeError == string.Empty)
                            {
                                oTransactionScope.Complete(); // verif
                            }
                            return retorno;
                        }
                    }
                }

                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }

        #region  Novedades_T2_Baja
        private static string Novedades_T2_Baja(int idEstadoReg, string ip, string usuario, List<Novedad> listNovedades)
        {
            try
            {
                string mensaje = String.Empty;
                long idNovedadAnt;
                long idPrestador;
                long idBeneficiario;
                short tipoConcepto;
                int conceptoOPP;
                double impTotal;

                if (listNovedades[0].UnEstadoReg.IdEstado != 1)
                    // para novedades en proceso de liquidación o en transito a la misma
                    mensaje = "Novedad en proceso de liquidación. No puede darse de baja";
                else
                {
                    idNovedadAnt = listNovedades[0].IdNovedad;
                    idPrestador = listNovedades[0].UnPrestador.ID;
                    idBeneficiario = listNovedades[0].UnBeneficiario.IdBeneficiario;
                    tipoConcepto = listNovedades[0].UnTipoConcepto.IdTipoConcepto;
                    conceptoOPP = listNovedades[0].UnConceptoLiquidacion.CodConceptoLiq;
                    impTotal = (-1 * listNovedades[0].ImporteTotal);

                    string NroComprobante = listNovedades[0].Comprobante;
                    Boolean EsAfiliacion = listNovedades[0].UnConceptoLiquidacion.EsAfiliacion;

                    ModificaSaldo(idPrestador, idBeneficiario, conceptoOPP, impTotal, usuario);

                    //Novedades_PasaAHist(idNovedadAnt, string.Empty, 9, 3, 0, ip, usuario);
                    Novedades_PasaAHist(idNovedadAnt, 0, 9, 3, 0, ip, usuario);

                }
                return mensaje + "|0| ";
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region Novedades_T3_Baja
        private static string Novedades_T3_Baja(int idEstadoReg, string ip, string usuario, int mensual, Novedad unaNovedad)
        {
            DataSet ds = new DataSet();
            try
            {
                string mensaje = String.Empty;    
                List<Cuota> lstCuotas = new List<Cuota>();

                lstCuotas = unaNovedad.unaLista_Cuotas.FindAll(delegate(Cuota C)
                                                    {
                                                        if (int.Parse(C.Mensual_Cuota) >= mensual)
                                                        {
                                                            return true;
                                                        }
                                                        else
                                                        {
                                                            return false;
                                                        }
                                                    });
                           
                if (lstCuotas.Count != 1 || lstCuotas[0].Mensual_Cuota != mensual.ToString())
                    mensaje = "Sólo se puede dar de baja a partir de la última cuota y en forma descendente.";

                if (mensaje == string.Empty)
                {                  
                    if (unaNovedad.UnEstadoReg.IdEstado != 1)
                        // para novedades en proceso de liquidación o en transito a la misma
                        mensaje = "Novedad en proceso de liquidación. No puede darse de baja";
                }
                if (mensaje == string.Empty)
                {
                    Cierre oCierre = CierreDAO.TraerFechaCierreProx();

                    string mensualAct = oCierre.Mensual;
                    long idNovedad = unaNovedad.IdNovedad;

                    if (mensual == int.Parse(mensualAct))
                    {                      
                        long idPrestador = unaNovedad.UnPrestador.ID; //long.Parse(dvNovViejas[0]["IdPrestador"].ToString());
                        long idBeneficiario = unaNovedad.UnBeneficiario.IdBeneficiario; //long.Parse(dvNovViejas[0]["IdBeneficiario"].ToString());
                        int conceptoOPP = unaNovedad.UnConceptoLiquidacion.CodConceptoLiq; //int.Parse(dvNovViejas[0]["CodConceptoLiq"].ToString());

                        double importe = unaNovedad.ImporteCuota * -1; //double.Parse(dvNovViejas[0]["ImporteCuota"].ToString()) * -1;

                        ModificaSaldo(idPrestador, idBeneficiario, conceptoOPP, importe, usuario);
                    }
                  				
                    Novedades_PasaAHist(idNovedad, mensual, idEstadoReg, 3, 0, ip, usuario);					//Modificado por COK 09.08.2005

                }
                return mensaje + "|0| ";
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region Novedades_BAJA_T3_ControlVencimiento
        public static void Novedades_BAJA_T3_ControlVencimiento(long idNovedad,
                                                                int MensualDesde,
                                                                enum_tipoestadoNovedad idEstadoReg,
                                                                string Mac,
                                                                string ip,
                                                                string usuario,
                                                                out string mensaje)
        {
            string sql = "Novedades_BAJA_T3_ControlVencimiento";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;

            try
            {
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, idNovedad);
                db.AddInParameter(dbCommand, "@MensualDesde", DbType.Int32, (int)MensualDesde);
                db.AddInParameter(dbCommand, "@IdEstadoReg", DbType.Int16, (int)idEstadoReg);
                db.AddInParameter(dbCommand, "@MAC", DbType.String, Mac);
                db.AddInParameter(dbCommand, "@IP", DbType.String, ip);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, usuario);
                db.AddOutParameter(dbCommand, "@mensaje", DbType.String, 100);

                using (TransactionScope scope = new TransactionScope())
                {
                    dbParametros = dbCommand.Parameters;
                    db.ExecuteNonQuery(dbCommand);
                    scope.Complete();
                    mensaje = dbParametros[06].Value.ToString();
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }        
        #endregion

        #region Novedades_T1o6_Baja                
        private static string Novedades_T1o6_Baja(int idEstadoReg, string ip, string usuario, List<Novedad> listNovedades)
        {
            try
            {
                string retorno = String.Empty;
                string mensaje = String.Empty;
                string[] alta = new String[2];
                byte codMovimientoV = listNovedades[0].UnCodMovimiento.CodMovimiento;

                // Solo paso a historico. No genero alta nueva, nov. nunca fue a la liquidacion
                if (codMovimientoV == 4)
                    mensaje = "Novedad Inexistente";
                else
                {
                    int estRegistroV = listNovedades[0].UnEstadoReg.IdEstado;
                    long idNovedadV = listNovedades[0].IdNovedad;
                    short tipoConcepto = listNovedades[0].UnTipoConcepto.IdTipoConcepto;
                    double impTotal = listNovedades[0].ImporteTotal;
                    Single porcentaje = listNovedades[0].Porcentaje;
                    long idPrestador = listNovedades[0].UnPrestador.ID;
                    long idBeneficiario = listNovedades[0].UnBeneficiario.IdBeneficiario;
                    int conceptoOPP = listNovedades[0].UnConceptoLiquidacion.CodConceptoLiq;
                    string nroComprobante = listNovedades[0].Comprobante;
                    Boolean esAfiliacion = listNovedades[0].UnConceptoLiquidacion.EsAfiliacion;

                    //Modificación de saldo
                    double importe = Calc_Importe_1o6(idBeneficiario, tipoConcepto, porcentaje, impTotal);

                    // Preparo el registro de baja segun corresponda.
                    byte codMovimiento = 4;

                    importe = importe * -1;

                    ModificaSaldo(idPrestador, idBeneficiario, conceptoOPP, importe, usuario);

                    if (codMovimientoV == 6 && estRegistroV == 1)
                    {
                        //Novedades_PasaAHist(idNovedadV, string.Empty, 8, 3, 0, ip, usuario);
                        Novedades_PasaAHist(idNovedadV, 0, 8, 3, 0, ip, usuario);
                        alta[0] = "0";
                        alta[1] = string.Empty;
                    }
                    else
                    {
                        //Alguna vez fue a la liquidación
                        switch (codMovimientoV)
                        {
                            //El archivo anterior fue modificado o es alta
                            case 5:
                            case 6:
                                switch (estRegistroV)
                                {
                                    case 1:
                                    case 4:
                                    case 13:
                                        Novedades_PasaAHist(idNovedadV, 0, 8, 3, 0, ip, usuario);
                                        break;
                                    case 2:
                                    case 3:
                                    case 14:
                                    case 15:
                                        Novedades_Modifica_EstadoReg(idNovedadV, 12, ip, usuario);
                                        break;
                                }
                                //Para estas novedades se debe ingresar un nuevo registro para informar la baja a la 
                                //liquidacion

                                DateTime fecNovedad = DateTime.Today;
                                alta = Novedades_Alta_Fisica(idPrestador, idBeneficiario, fecNovedad, Convert.ToByte(tipoConcepto), conceptoOPP, impTotal, 0, porcentaje, codMovimiento, nroComprobante, ip, usuario, 1);
                                break;
                        }
                    }
                }
                if (mensaje == String.Empty)
                {
                    retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                }
                else
                {
                    retorno = mensaje + "|0|";
                }
                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region Novedades_B_Con_Desaf_Monto
        public static void Novedades_B_Con_Desaf_Monto(long idNovedad, int idEstadoReg, string Mac, string ip, string usuario, bool cierre)
        {
            string sql;
            if (cierre)
                sql = "Novedades_B_Con_Desaf_Monto_Al_Cierre";
            else
                sql = "Novedades_B_Con_Desaf_Monto";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, idNovedad);
                db.AddInParameter(dbCommand, "@IdEstadoReg", DbType.Int16, idEstadoReg);
                db.AddInParameter(dbCommand, "@MAC", DbType.String, Mac);
                db.AddInParameter(dbCommand, "@IP", DbType.String, ip);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, usuario);

                using (TransactionScope scope = new TransactionScope())
                {
                    db.ExecuteNonQuery(dbCommand);
                    scope.Complete();
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }

        #endregion

        #region Novedades_Baja_Cuotas
        public static string Novedades_Baja_Cuotas(Novedad unaNovedad, string ip, string usuario)
        {
            string mensaje = string.Empty;

            try
            {
                if (unaNovedad.unaLista_Cuotas.Count > 0)
                {
                    using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        long idNovedadAnt = 0;
                        int mensual = 0;
                        foreach (Cuota oNovedadCuotaInd in unaNovedad.unaLista_Cuotas)
                        {
                            idNovedadAnt = oNovedadCuotaInd.IdNovedad;
                            mensual = int.Parse(oNovedadCuotaInd.Mensual_Cuota);

                            mensaje = Novedades_Baja(idNovedadAnt, ip, usuario, mensual);
                            if (!mensaje.StartsWith("|"))
                            {
                                mensaje = mensaje.Split(char.Parse("|"))[0].ToString();
                                break;
                            }
                            else
                                mensaje = string.Empty;
                        }

                        oTransactionScope.Complete();
                    }
                }
                else
                    mensaje = "Se deben seleccionar las cuotas a dar de baja";

                return mensaje;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion
        #endregion

        #region Aprobar Novedad
        public static List<KeyValue<long, string>> Novedades_Aprobacion(List<long> listNovedadesAAprobar, int idEstadoReg, string usuario)
        {
            try
            {
                string retorno = string.Empty;
                List<KeyValue<long, string>> listNovedadesNoAprobadas = new List<KeyValue<long, string>>();

                foreach (long idNovedad in listNovedadesAAprobar)
                {
                    try
                    {
                        retorno = Novedades_AprobarCredito(idNovedad, idEstadoReg, usuario);

                        if (!retorno.Equals(string.Empty))
                        {
                            listNovedadesNoAprobadas.Add(new KeyValue<long, string>(idNovedad, retorno));
                        }
                        else
                        {
                            listNovedadesNoAprobadas.Add(new KeyValue<long, string>(idNovedad, "OK"));
                        }
                    }
                    catch (Exception err)
                    {
                        listNovedadesNoAprobadas.Add(new KeyValue<long, string>(idNovedad, err.Message));
                    }
                }
                return listNovedadesNoAprobadas;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en NovedadDAO.Novedades_Aprobacion", err);
            }
        }

        #region  Novedades_AprobarCredito
        public static string Novedades_AprobarCredito(long id_Novedad, int id_EstadoReg, string Usuario)
        {
            string sql = "Novedades_AprobarCredito";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;

            try
            {
                db.AddInParameter(dbCommand, "@idnovedad", DbType.Int64, id_Novedad);
                db.AddInParameter(dbCommand, "@idestadoreg", DbType.Int64, id_EstadoReg);
                db.AddInParameter(dbCommand, "@usuario", DbType.String, Usuario);
                db.AddOutParameter(dbCommand, "@error", DbType.String, 100);
                dbParametros = dbCommand.Parameters;               

                using (TransactionScope scope = new TransactionScope())
                {
                    db.ExecuteNonQuery(dbCommand);
                    scope.Complete();
                }
                return dbParametros[03].Value.ToString();
            }
            catch (SqlException sqlex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlex.Source, sqlex.Message));
                throw sqlex;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en NovedadDAO.Novedades_AprobarCredito", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #endregion

        #region  Confirmar Novedad

        public static string Novedades_Confirmacion(long idNovedad, int idEstadoReg, string ip, string usuario, string oficina)
        {
            string sql = "Novedades_ConfirmaCallCenter";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            string rdo = string.Empty;

            try
            {
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, idNovedad);
                db.AddInParameter(dbCommand, "@IdEstadoReg", DbType.Int16, idEstadoReg);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, usuario);
                db.AddInParameter(dbCommand, "@IP", DbType.String, ip);
                db.AddInParameter(dbCommand, "@Oficina", DbType.String, oficina);

                dbParametros = dbCommand.Parameters;
               
                using (TransactionScope scope = new TransactionScope())
                {
                    db.ExecuteNonQuery(dbCommand);
                    scope.Complete();
                }
                return rdo;
            }
            catch (DbException sqlErr)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlErr.Source, sqlErr.Message));
                if (((System.Data.SqlClient.SqlException)(sqlErr)).Number >= 50000)
                    return ((System.Exception)(sqlErr)).Message;
                else
                    throw sqlErr;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #endregion *** Transaccional ***

        #region *** No Transaccional***             
        
        #region Genera_Datos_para_MAC
        private static string Genera_Datos_para_MAC (long idBeneficiario,long idPrestador, DateTime fecNovedad,  
			                                         byte codMovimiento, int conceptoOPP, byte tipoConcepto,  double impTotal, byte cantCuotas, 
			                                         Single porcentaje, string nroComprobante, string ip,  string usuario)
		{
			object[] datos = new object[12];
														
			datos[0]= idBeneficiario;
			datos[1]= idPrestador;
			datos[2]= fecNovedad;
			datos[3]= codMovimiento;
			datos[4]= conceptoOPP;
			datos[5]= tipoConcepto;
			datos[6]= impTotal;
			datos[7]= cantCuotas;
			datos[8]= porcentaje;
			datos[9]= nroComprobante;
			datos[10]=ip;
			datos[11]=usuario;

			return(Utilidades.Armo_String_MAC(datos));
		}
		#endregion	

        #region Validaciones y Controles

        #region Valido Novedad
        private static string Valido_Nov(long idPrestador, long idBeneficiario, byte tipoConcepto, int conceptoOPP, 
                                         double impTotal, byte cantCuotas, Single porcentaje, byte codMovimiento, String nroComprobante)
        {
            return Valido_Nov(idPrestador, idBeneficiario, tipoConcepto, conceptoOPP, impTotal, cantCuotas, porcentaje,
                              codMovimiento, nroComprobante, true, 0);
        }

        private static string Valido_Nov(long idPrestador, long idBeneficiario, byte tipoConcepto, int conceptoOPP, 
                                         double impTotal, byte cantCuotas, Single porcentaje, byte codMovimiento, 
                                         String nroComprobante, bool bGestionErrores, decimal montoPrestamo)
        {            
            string mensaje = String.Empty;
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedad_Valido_Derecho");

            try
            {
                // CONTROLA MAXIMOS INTENTOS 
                if (bGestionErrores)
                    mensaje = CtrolIntentos(idPrestador, idBeneficiario);

                // CONTROLA QUE ESTE INFORMADO EL COMPROBANTE
                if ((mensaje == String.Empty) && (nroComprobante.Trim().Length < 4))
                {
                    mensaje = "El nro. de comprobante debe ser mayor a 3 dígitos.";
                }
                // CONTROLA TIPOS DE CAMPOS

                if (mensaje == String.Empty)
                {
                    mensaje = CtrolMontos(tipoConcepto, impTotal, cantCuotas, porcentaje);
                }

                // valida la novedad
                if (mensaje == String.Empty)
                {
                    #region parametros

                    db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                    db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                    db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Byte, tipoConcepto);
                    db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, conceptoOPP);
                    db.AddInParameter(dbCommand, "@CodMovimiento", DbType.Byte, codMovimiento);
                    db.AddInParameter(dbCommand, "@ImporteTotal", DbType.Decimal, impTotal); //ESTE ES DECIMAL O DOUBLE????
                    db.AddInParameter(dbCommand, "@CantCuotas", DbType.Byte, cantCuotas);
                    db.AddInParameter(dbCommand, "@Porcentaje", DbType.Decimal, porcentaje);//ESTE ES DECIMAL O DOUBLE????
                    db.AddInParameter(dbCommand, "@NroComprobante", DbType.AnsiString, nroComprobante);//como limito la longitud a 100
                    db.AddInParameter(dbCommand, "@MontoPrestamo", DbType.Decimal, montoPrestamo);//ESTE ES DECIMAL O DOUBLE????
                    db.AddOutParameter(dbCommand, "@Mensaje", DbType.AnsiString, 100);
                    db.AddOutParameter(dbCommand, "@EsAfiliacion", DbType.Boolean, 1);

                    #endregion parametros

                    db.ExecuteNonQuery(dbCommand);

                    mensaje = db.GetParameterValue(dbCommand, "@Mensaje").ToString() + '|' + db.GetParameterValue(dbCommand, "@EsAfiliacion").ToString();    
                }

                if (mensaje != String.Empty) { mensaje += "|"; }

                return mensaje;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en Novedad_Valido_Derecho - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion Valido Novedad

        #region CtrolIntentos
        private static string CtrolIntentos(long idPrestador, long idBeneficiario)
        {
            string mensaje = String.Empty;
            try
            {
                int maxIntentos = int.Parse(ConfigurationManager.AppSettings["DAT_MaxIntentos"].ToString());
                int maxCantRechazos = CtrolCantRechazos(idPrestador, idBeneficiario);

                mensaje = (maxCantRechazos >= maxIntentos) ? "Máxima cantidad de intentos permitidos" : String.Empty;
                return (mensaje);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en CtrolIntentos", err);
            }
        }
        #endregion

        #region CtrolCantRechazos
        private static int CtrolCantRechazos(long idPrestador, long idBeneficiario)
        {
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("NovRechazadas_TCant");

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddOutParameter(dbCommand, "@CantRech", DbType.Byte, 0);

                db.ExecuteNonQuery(dbCommand);

                return (int.Parse(db.GetParameterValue(dbCommand, "@CantRech").ToString()));
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en NovRechazadas_TCant - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            } 
        }
        #endregion

        #region CtrolMontos
        private static string CtrolMontos(byte tipoConcepto, double impTotal, byte cantCuotas, Single porcentaje)
        {
            string mensajeMontos = String.Empty;
            switch (tipoConcepto)
            {
                case 1:
                case 2:
                    if (impTotal <= 0) { mensajeMontos = @"El campo Importe debe ser mayor a 0"; }
                    break;
                case 3:
                    if (impTotal <= 0) { mensajeMontos = @"El importe resultante de resta la cuota total  y cuota afiliación debe ser mayor a CERO (0)"; }
                    if (mensajeMontos == String.Empty)
                    {
                        if (cantCuotas <= 0 || cantCuotas > 240) { mensajeMontos = @"El campo Cant. Cuotas debe estar comprendido entre 1 y 240"; }
                    }
                    break;     
                case 6:                 	
                    if (porcentaje <= 0 || porcentaje > 100) { mensajeMontos = @"El campo Porcentaje debe ser mayor que 0 y menor a 100"; }
                    break;

                default:
                    mensajeMontos = @"Opción no contemplada";
                    break;
            }
            return mensajeMontos;
        }
        #endregion

        #region CtrolAlcanza      
        public static string CtrolAlcanza(long idBeneficiario, double importe, long idPrestador, int conceptoOPP)
        {
            // controla si alcanza el monto a ingresar - si no alcanza ingresa el monto en rechazados		
            string mensaje = String.Empty;

            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("AlcanzaAfectacion");

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@monto", DbType.Decimal, importe);
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@ConceptoOPP", DbType.Int32, conceptoOPP);
                db.AddOutParameter(dbCommand, "@alcanza", DbType.Byte, 0);

                db.ExecuteNonQuery(dbCommand);

                mensaje = Byte.Parse(db.GetParameterValue(dbCommand, "@alcanza").ToString()) == 0 ? "Afectación Disponible Insuficiente" : String.Empty;

                return mensaje;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en AlcanzaAfectacion - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }           
        }
        #endregion

        #region Calc_Importe_1o6
        private static double Calc_Importe_1o6(long idBeneficiario, short tipoConcepto, Single porcentaje, double impTotal)
        {
            double importe = 0;
            List<Beneficiario> oListBeneficiarios = new List<Beneficiario>();

            try
            {
                if (tipoConcepto == 6)
                {
                    oListBeneficiarios = BeneficiarioDAO.Traer(idBeneficiario, string.Empty);
                    importe = (oListBeneficiarios[0].SueldoBruto * porcentaje) / 100;
                }
                else
                    importe = impTotal;

                return importe;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region CtrolTopeXCpto
        private static string CtrolTopeXCpto(long idPrestador, short tipoConcepto, int conceptoOPP, double importe)
        {
            string sql = "CtrolTopeXCpto";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string mensaje = string.Empty;

            try
            {
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Int16, tipoConcepto);
                db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, conceptoOPP);
                db.AddInParameter(dbCommand, "@Importe", DbType.Decimal, importe);
                db.AddOutParameter(dbCommand, "@Alcanza", DbType.Boolean, 1);

                db.ExecuteNonQuery(dbCommand);

                if (!bool.Parse(db.GetParameterValue(dbCommand, "@Alcanza").ToString()))
                    mensaje = "Supera el Máximo permitido para el código de Liquidación " + conceptoOPP.ToString();

                return mensaje;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region Novedades_Trae_TCMov
        private static List<Novedad> Novedades_Trae_TCMov(long idPrestador, long idBeneficiario, int conceptoOPP)
        {
            string sql = "Novedades_Trae_TCMov";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad> lstNovedades = new List<Novedad>();
            Novedad unaNovedad;

            try
            {
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@ConceptoLiq", DbType.Int32, conceptoOPP);
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        unaNovedad = new Novedad();

                        unaNovedad.IdNovedad = long.Parse(dr["IdNovedad"].ToString());
                        unaNovedad.FechaNovedad = DateTime.Parse(dr["FecNov"].ToString());
                        unaNovedad.FechaImportacion = dr["FecImportacion"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecImportacion"].ToString());
                        unaNovedad.ImporteCuota = double.Parse(string.IsNullOrEmpty(dr["ImporteCuota"].ToString()) ? "0" : dr["ImporteCuota"].ToString());
                        unaNovedad.ImporteTotal = double.Parse(dr.GetValue("ImporteTotal").ToString());
                        unaNovedad.CantidadCuotas = byte.Parse(dr.GetValue("CantCuotas").ToString());
                        unaNovedad.Porcentaje = Single.Parse(dr["Porcentaje"].ToString());
                        unaNovedad.Comprobante = dr["NroComprobante"].ToString();
                        unaNovedad.MAC = dr["MAC"].ToString();
                        unaNovedad.NroCuotaLiquidada = dr["NroCuota"].Equals(DBNull.Value) ? 0 : int.Parse(dr["NroCuota"].ToString());
                        unaNovedad.MensualCuota = dr["MensualCuota"].Equals(DBNull.Value) ? "" : dr["MensualCuota"].ToString();


                        unaNovedad.UnPrestador.ID = long.Parse(dr["IdPrestador"].ToString());

                        unaNovedad.UnBeneficiario.IdBeneficiario = long.Parse(dr["IdBeneficiario"].ToString());

                        unaNovedad.UnTipoConcepto = new TipoConcepto(Byte.Parse(dr["TipoConcepto"].ToString()), string.Empty);

                        unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()), string.Empty);

                        unaNovedad.UnCodMovimiento = new CodigoMovimiento(Byte.Parse(dr["CodMovimiento"].ToString()), string.Empty);

                        unaNovedad.UnEstadoReg.IdEstado = dr["IdEstadoReg"].Equals(DBNull.Value) ? 0 : int.Parse(dr["IdEstadoReg"].ToString());
                        unaNovedad.UnAuditoria.IP = dr.GetString("IP");

                        lstNovedades.Add(unaNovedad);
                    }
                }

                return lstNovedades;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion
                 
        #endregion Validaciones y Controles
        
        #endregion No transaccional
    }
}
