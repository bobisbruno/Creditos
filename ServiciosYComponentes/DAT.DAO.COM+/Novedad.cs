using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Reflection;
using System.Configuration;
using System.EnterpriseServices;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Transactions;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using log4net;

namespace ANSES.Microinformatica.DATComPlus
{
	
	public class Novedad_Trans
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(Novedad_Trans).Name); 
	 
		public Novedad_Trans()
		{
		}	

		#region ABM Novedad
	
		#region Novedades_Alta

		public string Novedades_Alta(long IdPrestador, long IdBeneficiario, byte TipoConcepto, 
			int ConceptoOPP,  	double ImpTotal, byte CantCuotas,Single Porcentaje,	 
			string  NroComprobante, string IP, string Usuario,int Mensual)
		{
                string mensaje;

                byte IdEstadoReg;
                DateTime FecNovedad;
                string retorno;
                string resp;
                Boolean EsAfiliacion = false;
            
                try
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        EsAfiliacion = true;
                        FecNovedad = DateTime.Now; 
                        IdEstadoReg = 1;
                        resp = Valido_Nov(IdPrestador, IdBeneficiario, TipoConcepto, ConceptoOPP, ImpTotal, CantCuotas, Porcentaje, 6, NroComprobante);
                        mensaje = resp.Split(char.Parse("|"))[0].ToString();

                        if (mensaje != String.Empty)
                        {
                            retorno = string.Concat(mensaje, "|0|");
                        }
                        else
                        {
                            EsAfiliacion = Boolean.Parse(resp.Split(char.Parse("|"))[1].ToString());

                            switch (TipoConcepto)
                            {
                                case 1:
                                case 6:
                                    if (EsAfiliacion == true)
                                        retorno = Novedades_T1o6_Alta_Afiliacion(IdPrestador, IdBeneficiario, FecNovedad, TipoConcepto, ConceptoOPP,
                                            ImpTotal, Porcentaje, NroComprobante, IP, Usuario, Mensual, IdEstadoReg);
                                    else
                                        retorno = Novedades_T1o6_Alta_No_Afiliacion(IdPrestador, IdBeneficiario, FecNovedad, TipoConcepto, ConceptoOPP,
                                            ImpTotal, Porcentaje, NroComprobante, IP, Usuario, Mensual, IdEstadoReg);
                                    break;
                                case 2: //ok
                                    retorno = Novedades_T2_Alta(IdPrestador, IdBeneficiario, FecNovedad, TipoConcepto, ConceptoOPP,
                                        ImpTotal, NroComprobante, IP, Usuario, Mensual, IdEstadoReg);
                                    break;
                                case 3:
                                    retorno = Novedades_T3_Alta(IdPrestador, IdBeneficiario, FecNovedad, TipoConcepto, ConceptoOPP,
                                        ImpTotal, CantCuotas, NroComprobante, IP, Usuario, Mensual, IdEstadoReg);
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
                            NovedadRechazada_Alta(IdPrestador, IdBeneficiario, FecNovedad, TipoConcepto, ConceptoOPP,
                                string.Empty, ImpTotal, CantCuotas, Porcentaje, 6, NroComprobante, IP, Usuario, Mensual, mensajeError);
                        }
                        else
                        {
                            scope.Complete(); // verif
                        }
                        return retorno;
                    }
                }
                catch (Exception err)
                {
                   log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                   throw new Exception("Error en servicio Novedades_Alta -  ERROR: " + err.Message + " - SRC: " + err.Source);
                }
                finally
                {}
		}
		#endregion

		#region Novedades_Modificacion

		public string Novedades_Modificacion(long IdNovedadAnt, double ImpTotalN, Single PorcentajeN, string  NroComprobanteN, 
			string IPN, string UsuarioN,int Mensual, Boolean Masiva)		
		{
			Novedad nov = new Novedad();
			DataSet onov_v = new DataSet();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    string mensaje = String.Empty;
                    string retorno;

                    long IdPrestador = 0;
                    long IdBeneficiario = 0;
                    byte TipoConcepto = 0;
                    double ImpTotalV = 0;
                    Single PorcentajeV = 0;
                    byte IdEstadoRegV = 0;
                    byte CodMovimientoV = 0;
                    int ConceptoOPP = 0;


                    string resp = String.Empty;
                    Boolean EsAfiliacion = false;

                    onov_v = nov.Novedades_TxIdNovedad_Sliq(IdNovedadAnt);

                    if (onov_v.Tables[0].Rows.Count == 0)
                    {
                        mensaje = "No existe la novedad a modificar";
                    }
                    else
                    {
                        IdPrestador = long.Parse(onov_v.Tables[0].Rows[0]["IdPrestador"].ToString());
                        IdBeneficiario = long.Parse(onov_v.Tables[0].Rows[0]["IdBeneficiario"].ToString());
                        TipoConcepto = byte.Parse(onov_v.Tables[0].Rows[0]["TipoConcepto"].ToString());
                        ImpTotalV = double.Parse(onov_v.Tables[0].Rows[0]["ImporteTotal"].ToString());
                        PorcentajeV = Single.Parse(onov_v.Tables[0].Rows[0]["Porcentaje"].ToString());
                        IdEstadoRegV = byte.Parse(onov_v.Tables[0].Rows[0]["IdEstadoReg"].ToString());
                        CodMovimientoV = byte.Parse(onov_v.Tables[0].Rows[0]["CodMovimiento"].ToString());
                        ConceptoOPP = int.Parse(onov_v.Tables[0].Rows[0]["CodConceptoLiq"].ToString());
                        EsAfiliacion = Boolean.Parse(onov_v.Tables[0].Rows[0]["EsAfiliacion"].ToString());
                    }

                    if (mensaje == String.Empty)
                    {
                        if (IdEstadoRegV == 12)
                        {
                            mensaje = "No existe la novedad";
                        }
                        else
                        {
                            resp = Valido_Nov(IdPrestador, IdBeneficiario, TipoConcepto, ConceptoOPP, ImpTotalN, 1, PorcentajeN, 5, NroComprobanteN);
                            mensaje = resp.Split(char.Parse("|"))[0].ToString();
                        }
                    }
                    if (mensaje == String.Empty)
                    {
                        switch (TipoConcepto)
                        {
                            case 1:
                            case 6:
                                retorno = Novedades_T1o6_Modificacion(IdNovedadAnt, IdPrestador, IdBeneficiario,
                                    TipoConcepto, ConceptoOPP, ImpTotalN, PorcentajeN, NroComprobanteN, IPN, UsuarioN,
                                    Mensual, IdEstadoRegV, ImpTotalV, PorcentajeV, CodMovimientoV, Masiva, EsAfiliacion);
                                break;
                            case 2:
                                retorno = Novedades_T2_Modificacion(IdNovedadAnt, IdPrestador, IdBeneficiario,
                                    TipoConcepto, ConceptoOPP, ImpTotalN, NroComprobanteN, IPN, UsuarioN,
                                    IdEstadoRegV, ImpTotalV);
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
                        NovedadRechazada_Alta(IdPrestador, IdBeneficiario, DateTime.Now, TipoConcepto, ConceptoOPP,
                            string.Empty, ImpTotalN, 0, PorcentajeN, 5, NroComprobanteN, IPN, UsuarioN, Mensual, mensajeError);
                    }
                    else
                        scope.Complete(); // verif

                    return retorno;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_Modificacion -  ERROR: " + err.Message + " - SRC: " + err.Source);
            }
			finally
			{
                nov = null;
                onov_v.Dispose();
		    }
		}
		#endregion

		#region Novedades_Baja

		public string Novedades_Baja(long IdNovedadAnt, string IP, string Usuario,int Mensual)
		{		
			string retorno;				
			DataSet onov_v = new DataSet();
			byte TipoConcepto;
			byte idEstadoReg;
			
			Novedad nov = new Novedad();
			try
			{
                using (TransactionScope scope = new TransactionScope())
                {
                    retorno = String.Empty;
                    idEstadoReg = 9;
                    // busco la novedad a modificar

                    onov_v = nov.Novedades_TxIdNovedad_Sliq(IdNovedadAnt);

                    if (onov_v.Tables[0].Rows.Count == 0)
                    {
                        retorno = "No existe la novedad|0| ";
                    }
                    else
                    {
                        byte IdEstadoRegV = byte.Parse(onov_v.Tables[0].Rows[0]["IdEstadoReg"].ToString());
                        if (IdEstadoRegV == 12)
                        {
                            retorno = "No existe la novedad|0| ";
                        }
                        else
                        {
                            TipoConcepto = byte.Parse(onov_v.Tables[0].Rows[0]["TipoConcepto"].ToString());
                            switch (TipoConcepto)
                            {
                                case 1:
                                case 6:
                                    retorno = Novedades_T1o6_Baja(idEstadoReg, IP, Usuario, onov_v);
                                    break;
                                case 2: //ok
                                    retorno = Novedades_T2_Baja(idEstadoReg, IP, Usuario, onov_v);
                                    break;
                                case 3:
                                    idEstadoReg = 5;
                                    retorno = Novedades_T3_Baja(idEstadoReg, IP, Usuario, Mensual, onov_v);
                                    break;
                                default:
                                    retorno = "Operación inválida|0| ";
                                    break;
                            }
                        }
                    }

                    //09/01/12 - $3b@
                    // NO GRABA RECHAZADO PORQUE NO HAY DATOS P/GRABAR NI POSIBLES ERRORES COPADOS 
                    string mensajeError = retorno.Split(char.Parse("|"))[0].ToString().Trim();
                    if (mensajeError == string.Empty)
                    {
                        scope.Complete(); // verif
                    }
                    return retorno;
                }
			}
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en servicio Novedades_Baja -  ERROR: " + err.Message + " - SRC: " + err.Source);
			}
			finally
			{
                onov_v.Dispose();
			}
		}
		#endregion

		#region Novedades_Baja_Cuotas

		public string Novedades_Baja_Cuotas(DataSet CuotasABajar, string Ip, string Usuario )
		{
			string mensaje = string.Empty;
		
			try
			{
                using (TransactionScope scope = new TransactionScope())
                {
                    if (CuotasABajar.Tables[0].Rows.Count > 0)
                    {
                        long IdNovedadAnt = 0;
                        int Mensual = 0;
                        foreach (DataRow CuotaInd in CuotasABajar.Tables[0].Rows)
                        {
                            IdNovedadAnt = long.Parse(CuotaInd["IdNovedad"].ToString());
                            Mensual = int.Parse(CuotaInd["Mensual"].ToString());
                            mensaje = Novedades_Baja(IdNovedadAnt, Ip, Usuario, Mensual);
                            if (!mensaje.StartsWith("|"))
                            {
                                mensaje = mensaje.Split(char.Parse("|"))[0].ToString();
                                break;
                            }
                            else
                            {
                                mensaje = string.Empty;
                            }
                        }
                    }
                    else
                    {
                        mensaje = "Se deben seleccionar las cuotas a dar de baja";
                    }

                    if(string.IsNullOrEmpty(mensaje))
                        scope.Complete(); // verif

                    return mensaje;
                }
			}
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en servicio Novedades_Baja_Cuotas -  ERROR: " + err.Message + " - SRC: " + err.Source);		
			}				
		}

		#endregion

		#region Modificacion_Masiva_Indeterminadas

		public DataSet Modificacion_Masiva_Indeterminadas(DataSet NovAMod,double Monto, string Ip, string Usuario,bool Masiva)
		{
			string mensaje = string.Empty;
			DataSet dsSalida = new DataSet();
            try
            {
                dsSalida = NovAMod.Clone();

                if (NovAMod.Tables[0].Rows.Count > 0)
                {
                    long IdNovedadAnt = 0;
                    int Mensual = 0;
                    byte TipoConc = 0;
                    double ImpTotN = 0;
                    Single PorcentajeN = 0;
                    string Comprobante = String.Empty;
                    string Mensaje = String.Empty;
                    string mens = string.Empty;

                    foreach (DataRow NovInd in NovAMod.Tables[0].Rows)
                    {
                        DataRow drNFila = dsSalida.Tables[0].NewRow();

                        TipoConc = byte.Parse(NovInd["TipoConcepto"].ToString());
                        switch (TipoConc)
                        {
                            case 1:
                                ImpTotN = double.Parse(NovInd["ImporteTotal"].ToString()) + Monto;
                                PorcentajeN = 0;
                                break;
                            case 6:
                                PorcentajeN = Single.Parse(NovInd["Porcentaje"].ToString()) + Single.Parse(Monto.ToString());
                                ImpTotN = 0;
                                break;
                            default:
                                mens = "Tipo de Concepto Erróneo para Modidicación Masiva|0|";
                                break;
                        }

                        IdNovedadAnt = long.Parse(NovInd["IdNovedad"].ToString());
                        Mensual = int.Parse(NovInd["Mensual"].ToString());
                        Comprobante = NovInd["Comprobante"].ToString();

                        if (mens == string.Empty)
                        {
                            mens = Novedades_Modificacion(IdNovedadAnt, ImpTotN, PorcentajeN, Comprobante, Ip, Usuario, Mensual, Masiva);
                        }

                        if (mens.StartsWith("|")) // no hubo mensaje de error al realizar la modificacion
                        {

                            drNFila["Mensaje"] = String.Empty;
                            drNFila["IdNovedad"] = int.Parse(mens.Split(char.Parse("|"))[1].ToString());
                            drNFila["MAC"] = mens.Split(char.Parse("|"))[2].ToString();
                            drNFila["FecNov"] = DateTime.Today;
                            drNFila["ImporteTotal"] = ImpTotN;
                            drNFila["Porcentaje"] = PorcentajeN;
                        }
                        else
                        {
                            // no se produjo la modificación por algun motivo
                            drNFila["Mensaje"] = mens.Split(char.Parse("|"))[0].ToString();
                            drNFila["IdNovedad"] = IdNovedadAnt;
                            drNFila["MAC"] = NovInd["MAC"].ToString();
                            drNFila["FecNov"] = DateTime.Parse(NovInd["FecNov"].ToString());
                            drNFila["ImporteTotal"] = double.Parse(NovInd["ImporteTotal"].ToString());
                            drNFila["Porcentaje"] = Single.Parse(NovInd["Porcentaje"].ToString());
                        }

                        drNFila["Mensual"] = Mensual;
                        drNFila["Comprobante"] = Comprobante;
                        drNFila["IdBeneficiario"] = long.Parse(NovInd["IdBeneficiario"].ToString());
                        drNFila["ApellidoNombre"] = NovInd["ApellidoNombre"].ToString();
                        drNFila["Cuil"] = NovInd["Cuil"].ToString();
                        drNFila["TipoDoc"] = byte.Parse(NovInd["TipoDoc"].ToString());
                        drNFila["NroDoc"] = long.Parse(NovInd["NroDoc"].ToString());
                        drNFila["IdPrestador"] = long.Parse(NovInd["IdPrestador"].ToString());
                        drNFila["CodConceptoLiq"] = byte.Parse(NovInd["CodConceptoLiq"].ToString());
                        drNFila["DescConceptoLiq"] = NovInd["DescConceptoLiq"].ToString();
                        drNFila["TipoConcepto"] = byte.Parse(NovInd["TipoConcepto"].ToString());

                        //Agrego la fila a la tabla
                        dsSalida.Tables[0].Rows.Add(drNFila);
                    }
                }

                return dsSalida;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en servicio Modificacion_Masiva_Indeterminadas -  ERROR: " + err.Message + " - SRC: " + err.Source);
            }
            finally {
                dsSalida = null;
            }				
		}
		
		#endregion

		#endregion
					
		#region TIPOS 1 y 6
		/* ****************************************
		 * 
		 *			TIpos 1 y 6
		 * 
		 * **************************************** */

		#region Novedades_T1o6_Alta

		#region Novedades_T1o6_Alta_Afiliacion

		private string Novedades_T1o6_Alta_Afiliacion(long IdPrestador, long IdBeneficiario, DateTime FecNovedad, 
			byte TipoConcepto, int ConceptoOPP, double ImpTotal, Single Porcentaje,	 
			string  NroComprobante, string IP, string Usuario,int Mensual,byte IdEstadoReg)	
		{
			
			string mensaje =String.Empty;
			string retorno;
			DataSet ds	 = new DataSet();
			double Importe;
			long IdNovedad;
			byte CodMovimiento = 6;
			byte CantCuotas = 0;
			string MAC = String.Empty;
			String[] alta = new String[2];

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    ds = Novedades_Trae_TCMov(IdPrestador, IdBeneficiario, ConceptoOPP);

                    if (ds.Tables[0].Rows.Count != 0 && (ds.Tables[0].Rows[0]["CodMovimiento"].ToString() == "5" || ds.Tables[0].Rows[0]["CodMovimiento"].ToString() == "6"))
                    {
                        mensaje = "Solo se puede ingresar una novedad para el concepto ingresado";
                    }

                    Importe = Calc_Importe_1o6(IdBeneficiario, TipoConcepto, Porcentaje, ImpTotal);

                    if (mensaje == String.Empty)
                    {
                        if (TipoConcepto == 6)
                        {
                            mensaje = CtrolTopeXCpto(IdPrestador, TipoConcepto, ConceptoOPP, Porcentaje);
                            ImpTotal = 0;
                        }
                        else
                        {
                            mensaje = CtrolTopeXCpto(IdPrestador, TipoConcepto, ConceptoOPP, Importe);
                            Porcentaje = 0;
                        }
                    }

                    if (mensaje == String.Empty)
                    {
                        mensaje = CtrolAlcanza(IdBeneficiario, Importe, IdPrestador, ConceptoOPP);
                    }

                    if (mensaje == String.Empty && ds.Tables[0].Rows.Count != 0)
                    {
                        if (ds.Tables[0].Rows[0]["CodMovimiento"].ToString() == "4")// esta en novedades
                        {
                            IdNovedad = long.Parse(ds.Tables[0].Rows[0]["IdNovedad"].ToString());

                            switch (byte.Parse(ds.Tables[0].Rows[0]["IdEstadoReg"].ToString()))
                            {
                                case 1:
                                    CodMovimiento = 5;
                                    Novedades_PasaAHist(IdNovedad, 0, 7, 3, 0, IP, Usuario);
                                    break;
                                case 2:
                                case 3:
                                    Novedades_Modifica_EstadoReg(IdNovedad, 12, IP, Usuario);
                                    break;
                                case 4:
                                    Novedades_PasaAHist(IdNovedad, 0, 7, 3, 0, IP, Usuario);
                                    break;
                            }
                        }

                        //Actualizo el total de novedades cargadas
                    }
                    if (mensaje == String.Empty)
                    {
                        ModificaSaldo(IdPrestador, IdBeneficiario, ConceptoOPP, Importe, Usuario);

                        //Doy alta la nueva novedad

                        alta = Novedades_Alta_Fisica(IdPrestador, IdBeneficiario, FecNovedad, TipoConcepto, ConceptoOPP,
                            ImpTotal, CantCuotas, Porcentaje, CodMovimiento, NroComprobante, IP, Usuario, IdEstadoReg);

                        retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                        scope.Complete(); // verif
                    }
                    else
                    {
                        retorno = mensaje + "|0|";
                    }
                    return (retorno);
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_T1o6_Alta_Afiliacion -  ERROR: " + err.Message + " - SRC: " + err.Source);
            }	
			finally
			{
				ds.Dispose();
			}
		}
		#endregion

		#region Novedades_T1o6_Alta_No_Afiliacion
		private string Novedades_T1o6_Alta_No_Afiliacion(long IdPrestador, long IdBeneficiario, DateTime FecNovedad, 
			byte TipoConcepto, int ConceptoOPP, double ImpTotal, Single Porcentaje,	 
			string  NroComprobante, string IP, string Usuario,int Mensual,byte IdEstadoReg)	
		{
			
			string mensaje =String.Empty;
			string retorno;
			DataSet ds	 = new DataSet();
			double Importe;
			byte CodMovimiento = 6;
			byte CantCuotas = 0;
			string MAC = String.Empty;
			String[] alta = new String[2];
			
			try
			{
                using (TransactionScope scope = new TransactionScope())
                {

                    Importe = Calc_Importe_1o6(IdBeneficiario, TipoConcepto, Porcentaje, ImpTotal);

                    if (mensaje == String.Empty)
                    {
                        if (TipoConcepto == 6)
                        {
                             ImpTotal = 0;
                        }
                        else
                        {
                             Porcentaje = 0;
                        }
                    }

                    if (mensaje == String.Empty)
                    {
                        mensaje = CtrolAlcanza(IdBeneficiario, Importe, IdPrestador, ConceptoOPP);
                    }

                    if (mensaje == String.Empty)
                    {
                        //Actualizo el total de novedades cargadas
                        ModificaSaldo(IdPrestador, IdBeneficiario, ConceptoOPP, Importe, Usuario);

                        //Doy alta la nueva novedad

                        alta = Novedades_Alta_Fisica(IdPrestador, IdBeneficiario, FecNovedad, TipoConcepto, ConceptoOPP,
                            ImpTotal, CantCuotas, Porcentaje, CodMovimiento, NroComprobante, IP, Usuario, IdEstadoReg);

                        retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                        scope.Complete(); // verif
                    }
                    else
                    {
                        retorno = mensaje + "|0|";
                    }
                    
                    return (retorno);
                }
			}
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_T1o6_Alta_No_Afiliacion -  ERROR: " + err.Message + " - SRC: " + err.Source);		
			}	
			finally
			{
				ds.Dispose();
			}
		}
		#endregion

		#endregion

		#region Novedades_T1o6_Baja
		private string Novedades_T1o6_Baja(byte idEstadoReg, string IP, string Usuario, DataSet NovVieja)
			// Se dio de baja durante el periodo vigente
		{
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    string retorno = String.Empty;
                    string mensaje = String.Empty;
                    string[] alta = new String[2];
                    byte CodMovimientoV = byte.Parse(NovVieja.Tables[0].Rows[0]["CodMovimiento"].ToString());

                    // Solo paso a historico. No genero alta nueva, nov. nunca fue a la liquidacion
                    if (CodMovimientoV == 4)
                    {
                        mensaje = "Novedad Inexistente";
                    }
                    else
                    {
                        byte EstRegistroV = byte.Parse(NovVieja.Tables[0].Rows[0]["IdEstadoReg"].ToString());
                        long IdNovedadV = long.Parse(NovVieja.Tables[0].Rows[0]["IdNovedad"].ToString());
                        byte TipoConcepto = byte.Parse(NovVieja.Tables[0].Rows[0]["TipoConcepto"].ToString());
                        double ImpTotal = double.Parse(NovVieja.Tables[0].Rows[0]["ImporteTotal"].ToString());
                        Single Porcentaje = Single.Parse(NovVieja.Tables[0].Rows[0]["Porcentaje"].ToString());
                        long IdPrestador = long.Parse(NovVieja.Tables[0].Rows[0]["IdPrestador"].ToString());
                        long IdBeneficiario = long.Parse(NovVieja.Tables[0].Rows[0]["IdBeneficiario"].ToString());
                        int ConceptoOPP = int.Parse(NovVieja.Tables[0].Rows[0]["CodConceptoLiq"].ToString());
                        string NroComprobante = NovVieja.Tables[0].Rows[0]["NroComprobante"].ToString();
                        Boolean EsAfiliacion = Boolean.Parse(NovVieja.Tables[0].Rows[0]["EsAfiliacion"].ToString());

                        //Modificación de saldo
                        double Importe = Calc_Importe_1o6(IdBeneficiario, TipoConcepto, Porcentaje, ImpTotal);

                        // Preparo el registro de baja segun corresponda.
                        byte CodMovimiento = 4;

                        Importe = Importe * -1;

                        ModificaSaldo(IdPrestador, IdBeneficiario, ConceptoOPP, Importe, Usuario);

                        if (CodMovimientoV == 6 && EstRegistroV == 1)
                        {
                            Novedades_PasaAHist(IdNovedadV, 0, 8, 3, 0, IP, Usuario);
                            alta[0] = "0";
                            alta[1] = string.Empty;
                        }
                        else
                        {
                            //Alguna vez fue a la liquidación
                            switch (CodMovimientoV)
                            {
                                //El archivo anterior fue modificado o es alta
                                case 5:
                                case 6:
                                    switch (EstRegistroV)
                                    {
                                        case 1:
                                        case 4:
                                        case 13:
                                            Novedades_PasaAHist(IdNovedadV, 0, 8, 3, 0, IP, Usuario);
                                            break;
                                        case 2:
                                        case 3:
                                        case 14:
                                        case 15:
                                            Novedades_Modifica_EstadoReg(IdNovedadV, 12, IP, Usuario);
                                            break;
                                    }
                                    //Para estas novedades se debe ingresar un nuevo registro para informar la baja a la 
                                    //liquidacion

                                    DateTime FecNovedad = DateTime.Today;
                                    alta = Novedades_Alta_Fisica(IdPrestador, IdBeneficiario, FecNovedad, TipoConcepto, ConceptoOPP, ImpTotal, 0, Porcentaje, CodMovimiento, NroComprobante, IP, Usuario, 1);
                                    break;
                            }
                        }
                    }
                    if (mensaje == String.Empty)
                    {
                        retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                        scope.Complete(); // verif
                    }
                    else
                    {
                        retorno = mensaje + "|0|";
                    }
    
                    return retorno;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en servicio Novedades_T1o6_Baja -  ERROR: " + err.Message + " - SRC: " + err.Source);
            }		
		}
		#endregion

		#region Novedades_T1o6_Modificacion
		private string Novedades_T1o6_Modificacion(long IdNovedadAnt,long IdPrestador, long IdBeneficiario, 
			byte TipoConcepto, int ConceptoOPP, double ImpTotalN, Single PorcentajeN, string NroComprobanteN,string IPN, string UsuarioN, 
			int Mensual, byte IdEstadoRegV, double  ImpTotalV, Single PorcentajeV, byte CodMovimientoV, Boolean Masiva, Boolean EsAfiliacion)
		{
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    string retorno = String.Empty;
                    string mensaje = String.Empty;
                    byte CodMovimientoN;
                    byte idEstadoRegN = 1;
                    double ImporteN = 0;
                    double ImporteV = 0;
                    DateTime FecNovedad = DateTime.Today;
                    double val = 0;
                    byte CantCuotas = 0;
                    String[] alta = new String[2];
                    CodMovimientoN = 5;

                    if (CodMovimientoV == 4)
                    {
                        mensaje = "Novedad inexistente";
                    }


                    if (mensaje == String.Empty)
                    {
                        if (EsAfiliacion == true)
                        {
                            if (TipoConcepto == 6)
                                mensaje = CtrolTopeXCpto(IdPrestador, TipoConcepto, ConceptoOPP, PorcentajeN);
                            else
                                mensaje = CtrolTopeXCpto(IdPrestador, TipoConcepto, ConceptoOPP, ImpTotalN);
                        }
                        else
                        {
                            CodMovimientoN = 6;
                        }
                    }


                    if (mensaje == String.Empty)
                    {
                        ImporteN = Calc_Importe_1o6(IdBeneficiario, TipoConcepto, PorcentajeN, ImpTotalN);
                        ImporteV = Calc_Importe_1o6(IdBeneficiario, TipoConcepto, PorcentajeV, ImpTotalV);
                        FecNovedad = DateTime.Today;
                        val = ImporteN - ImporteV;
                        CantCuotas = 0;

                        if (val > 0 && Masiva == false)
                        {
                             mensaje = CtrolAlcanza(IdBeneficiario, val, IdPrestador, ConceptoOPP);
                        }
                    }


                    if (mensaje == String.Empty)
                    {
                        ModificaSaldo(IdPrestador, IdBeneficiario, ConceptoOPP, val, UsuarioN);

                        switch (IdEstadoRegV)
                        {
                            case 2:
                            case 3:
                            case 14:
                            case 15:
                                Novedades_Modifica_EstadoReg(IdNovedadAnt, 12, IPN, UsuarioN);
                                break;
                            case 1:
                                if (CodMovimientoV == 6)
                                {
                                    CodMovimientoN = 6;
                                }
                                Novedades_PasaAHist(IdNovedadAnt, Mensual, 7, 3, 0, IPN, UsuarioN);
                                break;
                            case 4:
                            case 13:
                                Novedades_PasaAHist(IdNovedadAnt, Mensual, 7, 3, 0, IPN, UsuarioN);
                                break;
                        }

                        alta = Novedades_Alta_Fisica(IdPrestador, IdBeneficiario, FecNovedad, TipoConcepto, ConceptoOPP,
                            ImpTotalN, CantCuotas, PorcentajeN, CodMovimientoN, NroComprobanteN, IPN, UsuarioN, idEstadoRegN);


                        retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                        scope.Complete(); // verif
                    }
                    else
                    {
                        retorno = mensaje + "|0|";
                    }

                    return retorno;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_T1o6_Modificacion -  ERROR: " + err.Message + " - SRC: " + err.Source);
            }
			finally
			{
			 
			}
		}
		#endregion

		#region Novedades_Trae_TCMov
        //cambiar en  Novedades_Trae_TCMov(long IdPrestador, long IdBeneficiario,int ConceptoOPP) a private
		public DataSet Novedades_Trae_TCMov(long IdPrestador, long IdBeneficiario,int ConceptoOPP)
		{
		    Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_Trae_TCMov");

            try
            {
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, IdPrestador);
                db.AddInParameter(dbCommand, "@ConceptoLiq", DbType.Int32, ConceptoOPP);
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, IdBeneficiario);

                return db.ExecuteDataSet(dbCommand);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_Trae_TCMov -  ERROR: " + err.Message + " - SRC: " + err.Source);
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
		}	
		#endregion
		
		#region CtrolTopeXCpto
		private string CtrolTopeXCpto(long IdPrestador, byte TipoConcepto, int ConceptoOPP,double Importe)
									
		{
			string mensaje = String.Empty;
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("CtrolTopeXCpto");

			try
			{
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, IdPrestador);
                db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Byte, TipoConcepto);
                db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, ConceptoOPP);
                db.AddInParameter(dbCommand, "@Importe", DbType.Decimal, Importe);
                db.AddOutParameter(dbCommand, "@Alcanza", DbType.Byte, 0);

                db.ExecuteNonQuery(dbCommand);
								
				if ((Byte) db.GetParameterValue(dbCommand, "@Alcanza") == 0)
				{		
					mensaje = "Supera el Máximo permitido para el código de Liquidación " + ConceptoOPP.ToString();
				}
				
				return mensaje;
			}
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio CtrolTopeXCpto -  ERROR: " + err.Message + " - SRC: " + err.Source);
			}
			finally
			{
                dbCommand.Dispose();
                db = null;
			}
		}
		#endregion

		#region Calc_Importe_1o6
		private double Calc_Importe_1o6(long IdBeneficiario, byte TipoConcepto, Single Porcentaje, double ImpTotal)
		{	
			DataSet ds	 = new DataSet();
			double Importe = 0;
			Beneficiarios benef = new Beneficiarios();
			try
			{
				if (TipoConcepto == 6)
				{	
					ds = benef.Traer(IdBeneficiario.ToString(),string.Empty);
					Importe =(double.Parse(ds.Tables[0].Rows[0]["SueldoBruto"].ToString()) * Porcentaje)/100;
				}
				else
				{
					Importe = ImpTotal;
				}
				return Importe;
			}
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Calc_Importe_1o6 -  ERROR: " + err.Message + " - SRC: " + err.Source);
			}
			finally
			{
				ds.Dispose();	
			}
		}
	
		#endregion

		#endregion
		
		#region TIPO2
		/* ****************************************
		 * 
		 *				Tipo 2
		 * 
		 * **************************************** */

		#region Novedades_T2_Alta
		private string Novedades_T2_Alta(long IdPrestador, long IdBeneficiario, DateTime FecNovedad, 
			byte TipoConcepto, int ConceptoOPP,  double ImpTotal, 
			string  NroComprobante, string IP, string Usuario,int Mensual,byte IdEstadoReg)
		{		
			
			try
			{
                using (TransactionScope scope = new TransactionScope())
                {

                    string mensaje = String.Empty;
                    String[] alta = new String[2];
                    byte CodMovimiento = 6;
                    string retorno;

                    mensaje = CtrolAlcanza(IdBeneficiario, ImpTotal, IdPrestador, ConceptoOPP);

                    if (mensaje == String.Empty)
                    {
                        ModificaSaldo(IdPrestador, IdBeneficiario, ConceptoOPP, ImpTotal, Usuario);

                        alta = Novedades_Alta_Fisica(IdPrestador, IdBeneficiario, FecNovedad, TipoConcepto, ConceptoOPP, ImpTotal, 1, 0, CodMovimiento, NroComprobante, IP, Usuario, IdEstadoReg);
                        retorno = String.Format(" |{0}|{1}", alta[0].ToString(), alta[1].ToString());
                        scope.Complete(); // verif
                    }
                    else
                    {
                        retorno = mensaje + "|0| ";
                    }                    
                    return retorno;
                }
			}
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_T2_Alta -  ERROR: " + err.Message + " - SRC: " + err.Source);				
			}
			finally
			{}
		}
	
		#endregion

		#region  Novedades_T2_Modificacion()
		private string Novedades_T2_Modificacion(long IdNovedadAnt,long IdPrestador, long IdBeneficiario, 
			byte TipoConcepto, int ConceptoOPP, double ImpTotalN, string  NroComprobanteN, string IPN, 
			string UsuarioN, byte IdEstadoRegV,	double ImpTotalV)
		{
				
			try
			{
                using (TransactionScope scope = new TransactionScope())
                {

                    string mensaje = String.Empty;
                    double Importe = 0;
                    DateTime FecNovedad = DateTime.Today;
                    byte codMovimiento = 5;
                    string retorno = String.Empty;
                    String[] alta = new String[2];
                    byte IdEstadoRegN = 1;

                    // busco la novedad a modificar

                    if (IdEstadoRegV != 1)
                    {
                        // para novedades en proceso de liquidación o en transito a la misma
                        mensaje = "Novedad en proceso de liquidación. No puede modificarse";
                    }
                    if (mensaje == String.Empty)
                    {
                        // calculo el importe para ver si alcanza el disponible
                        Importe = ImpTotalN - ImpTotalV;

                        if (Importe > 0)
                        {
                            mensaje = CtrolAlcanza(IdBeneficiario, Importe, IdPrestador, ConceptoOPP);
                        }

                    }
                    if (mensaje == String.Empty)
                    {
                        ModificaSaldo(IdPrestador, IdBeneficiario, ConceptoOPP, Importe, UsuarioN);

                        Novedades_PasaAHist(IdNovedadAnt, 0, 7, 3, 0, IPN, UsuarioN);

                        alta = Novedades_Alta_Fisica(IdPrestador, IdBeneficiario, FecNovedad, TipoConcepto, ConceptoOPP, ImpTotalN, 1, 0, codMovimiento, NroComprobanteN, IPN, UsuarioN, IdEstadoRegN);

                        retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                        scope.Complete(); // verif
                    }
                    else
                    {
                        retorno = mensaje + "|0| ";
                    }
                    
                    return retorno;
                }
			}
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en servicio Novedades_T2_Modificacion -  ERROR: " + err.Message + " - SRC: " + err.Source);
			}			
			finally
			{
				
			}
		}	
		#endregion

		#region  Novedades_T2_Baja()
		private string Novedades_T2_Baja(byte IdEstadoReg,string IP, string Usuario,DataSet NovVieja)
		{				
			try	
			{
                using (TransactionScope scope = new TransactionScope())
                {
                    string mensaje = String.Empty;
                    long IdNovedadAnt;
                    long idPrestador;
                    long IdBeneficiario;
                    byte TipoConcepto;
                    int ConceptoOPP;
                    double ImpTotal;

                    if (NovVieja.Tables[0].Rows[0]["IdEstadoReg"].ToString() != "1")
                    {
                        // para novedades en proceso de liquidación o en transito a la misma
                        mensaje = "Novedad en proceso de liquidación. No puede darse de baja";
                    }
                    else
                    {
                        IdNovedadAnt = long.Parse(NovVieja.Tables[0].Rows[0]["IdNovedad"].ToString());
                        idPrestador = long.Parse(NovVieja.Tables[0].Rows[0]["IdPrestador"].ToString());
                        IdBeneficiario = long.Parse(NovVieja.Tables[0].Rows[0]["IdBeneficiario"].ToString());
                        TipoConcepto = byte.Parse(NovVieja.Tables[0].Rows[0]["TipoConcepto"].ToString());
                        ConceptoOPP = int.Parse(NovVieja.Tables[0].Rows[0]["CodConceptoLiq"].ToString());
                        ImpTotal = -1 * (double.Parse(NovVieja.Tables[0].Rows[0]["ImporteTotal"].ToString()));

                        ModificaSaldo(idPrestador, IdBeneficiario, ConceptoOPP, ImpTotal, Usuario);

                        Novedades_PasaAHist(IdNovedadAnt, 0, 9, 3, 0, IP, Usuario);

                    }

                    if(string.IsNullOrEmpty(mensaje))
                        scope.Complete(); // verif

                    return mensaje + "|0| ";
                }
			}
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en servicio Novedades_T2_Baja -  ERROR: " + err.Message + " - SRC: " + err.Source);
			}
			
		}
	
		#endregion
		#endregion

		#region TIPO3
		/* ****************************************
		 * 
		 * Tipo 3
		 * 
		 * **************************************** */

		public string Valido_Nov_T3(long IdPrestador, long IdBeneficiario, 
			byte TipoConcepto, int ConceptoOPP, 
			double ImpTotal, byte CantCuotas,
			Single Porcentaje, byte CodMovimiento, String NroComprobante,
			DateTime FecNovedad, string IP, string Usuario,int Mensual,
			decimal montoPrestamo,decimal CuotaTotalMensual,
			decimal TNA,decimal TEM,
			decimal gastoOtorgamiento,decimal gastoAdmMensual,
			decimal cuotaSocial,decimal CFTEA,
			decimal CFTNAReal,decimal CFTEAReal,
			decimal gastoAdmMensualReal,decimal TIRReal)
		{
			return Valido_Nov_T3_Gestion(IdPrestador, IdBeneficiario, TipoConcepto, ConceptoOPP,  ImpTotal, CantCuotas,
						  Porcentaje, CodMovimiento, NroComprobante, FecNovedad,  IP, Usuario, Mensual,
						  montoPrestamo, CuotaTotalMensual, TNA, TEM, gastoOtorgamiento, gastoAdmMensual,
						  cuotaSocial, CFTEA, CFTNAReal, CFTEAReal, gastoAdmMensualReal, TIRReal, true);
		}

		
		public string Valido_Nov_T3_Gestion(long IdPrestador, long IdBeneficiario, 
									byte TipoConcepto, int ConceptoOPP, 
									double ImpTotal, byte CantCuotas,
									Single Porcentaje, byte CodMovimiento, String NroComprobante,
									DateTime FecNovedad, string IP, string Usuario,int Mensual,
									decimal montoPrestamo,decimal CuotaTotalMensual,
									decimal TNA,decimal TEM,
									decimal gastoOtorgamiento,decimal gastoAdmMensual,
									decimal cuotaSocial,decimal CFTEA,
									decimal CFTNAReal,decimal CFTEAReal,
									decimal gastoAdmMensualReal,decimal TIRReal, bool bGestionErrores)
		{

            using (TransactionScope scope = new TransactionScope())
            {

                string mensaje = String.Empty;
                mensaje = Valido_Nov(IdPrestador, IdBeneficiario, TipoConcepto,
                                     ConceptoOPP, ImpTotal, CantCuotas, Porcentaje, CodMovimiento, NroComprobante, bGestionErrores, montoPrestamo);
                mensaje = mensaje.Split(char.Parse("|"))[0].ToString().Trim();

                if (mensaje == String.Empty)
                {
                    mensaje = CtrolAlcanza(IdBeneficiario, (double)CuotaTotalMensual, IdPrestador, ConceptoOPP);
                }

                if (mensaje != String.Empty )
                {
                    if(bGestionErrores)
                        Novedades_Rechazadas_A_ConTasas(IdBeneficiario, IdPrestador, CodMovimiento,
                            TipoConcepto, ConceptoOPP, ImpTotal, CantCuotas,
                            Porcentaje, NroComprobante, IP, Usuario, FecNovedad,
                            montoPrestamo, CuotaTotalMensual, TNA, TEM,
                            gastoOtorgamiento, gastoAdmMensual, cuotaSocial,
                            CFTEA, CFTNAReal, CFTEAReal, gastoAdmMensualReal, TIRReal, mensaje);

                }
                else
                    scope.Complete(); // verif

                return mensaje;
            }
		}

		
		public string Novedades_T3_Alta_ConTasa(long IdPrestador, long IdBeneficiario, DateTime FecNovedad,  byte TipoConcepto, int ConceptoOPP, 
			double ImpTotal, byte CantCuotas,  string NroComprobante, string IP, string Usuario, int Mensual, byte IdEstadoReg,
			decimal montoPrestamo,decimal CuotaTotalMensual,decimal TNA,decimal TEM,
			decimal gastoOtorgamiento,decimal gastoAdmMensual,decimal cuotaSocial,decimal CFTEA,
			decimal CFTNAReal,decimal CFTEAReal,decimal gastoAdmMensualReal,decimal TIRReal, string XMLCuotas, 
			int idItem, string nroFactura, string cbu, string nroTarjeta, string otro, string prestadorServicio, string poliza,
            string nroSocio, string nroTicket, int idDomicilioBeneficiario, int idDomicilioPrestador, string nroSucursal, DateTime fVto, DateTime fVtoHabilSiguiente, byte idTipoDocPresentado, DateTime fEntregaTarjeta, bool solicitaTarjetaNominada)
		{
			
			try
			{
                using (TransactionScope scope = new TransactionScope())
                {
                    String[] alta = new String[2];
                    string mensaje = String.Empty;
                    string retorno = String.Empty;
                    byte CodMovimiento = 6;
                    string MAC = string.Empty;

                    mensaje = Valido_Nov_T3(IdPrestador, IdBeneficiario,
                                           TipoConcepto, ConceptoOPP, ImpTotal,
                                           CantCuotas, 0, CodMovimiento, NroComprobante,
                                           FecNovedad, IP, Usuario, Mensual,
                                            montoPrestamo, CuotaTotalMensual, TNA, TEM,
                                            gastoOtorgamiento, gastoAdmMensual, cuotaSocial, CFTEA,
                                            CFTNAReal, CFTEAReal, gastoAdmMensualReal, TIRReal);


                    if (mensaje == String.Empty)
                    {
                        ModificaSaldo(IdPrestador, IdBeneficiario, ConceptoOPP, (double)CuotaTotalMensual, Usuario);

                        try
                        {
                            alta = Novedades_Alta_Fisica_Tipo3_ConTasa(IdBeneficiario, IdPrestador, ConceptoOPP, ImpTotal, montoPrestamo,
                                CantCuotas, CuotaTotalMensual, TNA, TEM, gastoOtorgamiento,
                                gastoAdmMensual, cuotaSocial, CFTEA, CFTNAReal,
                                CFTEAReal, gastoAdmMensualReal, TIRReal, NroComprobante, IP,
                                Usuario, Mensual, XMLCuotas, CodMovimiento, FecNovedad, TipoConcepto, IdEstadoReg,
                                idItem, nroFactura, cbu, nroTarjeta, otro, prestadorServicio, poliza,
                                nroSocio, nroTicket, idDomicilioBeneficiario, idDomicilioPrestador, nroSucursal, fVto, fVtoHabilSiguiente, idTipoDocPresentado, fEntregaTarjeta, solicitaTarjetaNominada);

                            retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                            scope.Complete(); // verif
                        }
                        catch (SqlException e)
                        {
                            log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), e.Source, e.Message));  
                            //Si es una excepcion especifica guardo el error y continuo
                            if (e.Number == 50000)
                            {
                                retorno = e.Message + "|0| ";
                            }
                            else
                                throw;
                        }
                    }
                    else
                    {
                        retorno = mensaje + "|0| ";
                    }

                    return retorno;
                }
			}
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_T3_Alta_ConTasa -  ERROR: " + err.Message + " - SRC: " + err.Source);
			}
			finally
			{}
		}
		
		#region Novedades_T3_Alta
		private string Novedades_T3_Alta(long IdPrestador, long IdBeneficiario, DateTime FecNovedad,  byte TipoConcepto, int ConceptoOPP, 
			double ImpTotal, byte CantCuotas,  string NroComprobante, string IP, string Usuario, int Mensual, byte IdEstadoReg)
		{

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    String[] alta = new String[2];
                    string mensaje = String.Empty;
                    string retorno = String.Empty;
                    double Importe = ImpTotal / CantCuotas;
                    byte CodMovimiento = 6;
                    string MAC = string.Empty;
                 
                    mensaje = CtrolAlcanza(IdBeneficiario, Importe, IdPrestador, ConceptoOPP);
				
                    if (mensaje == String.Empty)
                    {
                        ModificaSaldo(IdPrestador, IdBeneficiario, ConceptoOPP, Importe, Usuario);

                        alta = Novedades_Alta_Fisica_Tipo3(IdPrestador, IdBeneficiario, FecNovedad, TipoConcepto,
                            ConceptoOPP, ImpTotal, CantCuotas, 0, CodMovimiento, NroComprobante, IP, Usuario, IdEstadoReg, Mensual);

                        retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                        scope.Complete(); // verif
                    }
                    else
                    {
                        retorno = mensaje + "|0| ";
                    }
                    
                    return retorno;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_T3_Alta -  ERROR: " + err.Message + " - SRC: " + err.Source);
            }
			finally
			{}
		}
		
		#region Alta_Fisica_Novedades_Tipo3 
		
		private String[] Novedades_Alta_Fisica_Tipo3(long IdPrestador, long IdBeneficiario, DateTime FecNovedad, 
			byte TipoConcepto, int ConceptoOPP, double ImpTotal, byte CantCuotas,Single Porcentaje,	 
			byte CodMovimiento, string  NroComprobante, string IP, string Usuario, byte IdEstadoReg, int Mensual)
		{		

			string dato = Genera_Datos_para_MAC(IdBeneficiario, IdPrestador,FecNovedad,CodMovimiento,ConceptoOPP,TipoConcepto,
				ImpTotal,CantCuotas,Porcentaje,NroComprobante,IP,Usuario);
					
			string MAC = Utilidades.Calculo_MAC(dato);
            String[] retorno = new String[2];

            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_Tipo3_Alta");

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, IdBeneficiario);
                    db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, IdPrestador);
                    db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, ConceptoOPP);
                    db.AddInParameter(dbCommand, "@ImporteTotal", DbType.Decimal, ImpTotal);
                    db.AddInParameter(dbCommand, "@CantCuotas", DbType.Byte, CantCuotas);
                    db.AddInParameter(dbCommand, "@NroComprobante", DbType.AnsiString, NroComprobante); /* OJO en un futuro se va a exigir cargar el nro de comprobante */
                    db.AddInParameter(dbCommand, "@MAC", DbType.AnsiString, MAC);
                    db.AddInParameter(dbCommand, "@IP", DbType.AnsiString, IP);
                    db.AddInParameter(dbCommand, "@Usuario", DbType.AnsiString, Usuario);
                    db.AddInParameter(dbCommand, "@PrimerMensual", DbType.Int32, Mensual);
                    db.AddOutParameter(dbCommand, "@IdNovedad", DbType.Int64, 0);

                    db.ExecuteNonQuery(dbCommand);

                    retorno[0] = db.GetParameterValue(dbCommand, "@IdNovedad").ToString();
                    retorno[1] = MAC;
                    scope.Complete(); // verif
                    return retorno;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en Novedades_Tipo3_Alta - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
            finally {
                dbCommand.Dispose();
                db = null;
            }
		}
		
		private String[] Novedades_Alta_Fisica_Tipo3_ConTasa(
			
			long IdBeneficiario ,
			long IdPrestador , 
			int CodConceptoLiq, 
			double importeTotal,
			decimal montoPrestamo,
			byte CantCuotas,
			decimal CuotaTotalMensual,
			decimal TNA,
			decimal TEM,
			decimal gastoOtorgamiento,
			decimal gastoAdmMensual,
			decimal cuotaSocial,
			decimal CFTEA ,
			decimal CFTNAReal ,
			decimal CFTEAReal ,
			decimal gastoAdmMensualReal,
			decimal TIRReal ,
			string NroComprobante,
			string IP ,
			string Usuario,
			int PrimerMensual, 
			string cuotas,
			byte CodMovimiento,
			DateTime FecNovedad, 
			byte TipoConcepto,
			byte IdEstadoReg,
			int idItem,
			string nroFactura,
			string cbu,
			string nroTarjeta,
			string otro,
			string prestadorServicio,
			string poliza,
			string nroSocio,
			string nroTicket,
			int idDomicilioBeneficiario,
			int idDomicilioPrestador, 
			string nroSucursal, 
            DateTime fVto,
			DateTime fVtoHabilSiguiente,
            byte idTipoDocPresentado,
            DateTime fEntregaTarjeta,
            bool solicitaTarjetaNominada)
			{		

			string dato = Genera_Datos_para_MAC(IdBeneficiario, IdPrestador,FecNovedad,
												CodMovimiento,CodConceptoLiq,TipoConcepto,
												importeTotal,CantCuotas,0,NroComprobante,IP,Usuario);
					
			string MAC = Utilidades.Calculo_MAC(dato);

            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_Tipo3_AltaConTasas");

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    String[] retorno = new String[2];

                    db.AddInParameter(dbCommand, "@idbeneficiario", DbType.Int64, IdBeneficiario);
                    db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, IdPrestador);
                    db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, CodConceptoLiq);
                    db.AddInParameter(dbCommand, "@importeTotal", DbType.Decimal, importeTotal);
                    db.AddInParameter(dbCommand, "@montoPrestamo", DbType.Decimal, montoPrestamo);
                    db.AddInParameter(dbCommand, "@CantCuotas", DbType.Byte, CantCuotas);
                    db.AddInParameter(dbCommand, "@CuotaTotalMensual", DbType.Decimal, CuotaTotalMensual);
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
                    db.AddInParameter(dbCommand, "@NroComprobante", DbType.AnsiString, NroComprobante);
                    db.AddInParameter(dbCommand, "@MAC", DbType.AnsiString, MAC);
                    db.AddInParameter(dbCommand, "@IP", DbType.AnsiString, IP);
                    db.AddInParameter(dbCommand, "@Usuario", DbType.AnsiString, Usuario);
                    db.AddInParameter(dbCommand, "@PrimerMensual", DbType.Int32, PrimerMensual);
                    db.AddInParameter(dbCommand, "@cuotas", DbType.AnsiString, cuotas);
                    db.AddInParameter(dbCommand, "@idestadoReg", DbType.Int32, IdEstadoReg);
                    db.AddInParameter(dbCommand, "@idItem", DbType.Int32, idItem);
                    db.AddInParameter(dbCommand, "@nroFactura", DbType.AnsiString, nroFactura);
                    db.AddInParameter(dbCommand, "@cbu", DbType.AnsiString, cbu);
                    db.AddInParameter(dbCommand, "@otro", DbType.AnsiString, otro);
                    db.AddInParameter(dbCommand, "@prestadorServicio", DbType.AnsiString, prestadorServicio);
                    db.AddInParameter(dbCommand, "@poliza", DbType.AnsiString, poliza);
                    db.AddInParameter(dbCommand, "@nroSocio", DbType.AnsiString, nroSocio);
        
                    if (nroTarjeta == null || nroTarjeta == string.Empty)
                        db.AddInParameter(dbCommand, "@nroTarjeta", DbType.AnsiString, DBNull.Value);
                    else
                        db.AddInParameter(dbCommand, "@nroTarjeta", DbType.AnsiString, nroTarjeta);

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
                    
                    if (fEntregaTarjeta == DateTime.MinValue)
                        db.AddInParameter(dbCommand, "@fEntregaTarjeta", DbType.DateTime, DBNull.Value);
                    else
                        db.AddInParameter(dbCommand, "@fEntregaTarjeta", DbType.DateTime, fVto);

                   db.AddOutParameter(dbCommand, "@IdNovedad", DbType.Int64, 0);

                    db.ExecuteNonQuery(dbCommand);

                    retorno[0] = db.GetParameterValue(dbCommand, "@IdNovedad").ToString();
                    retorno[1] = MAC;

                    scope.Complete(); // verif
                    return retorno;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en Novedades_Tipo3_AltaConTasas - ERROR: " + err.Message + " - SRC: " + err.Source); 
            }
            finally {
                dbCommand.Dispose();
                db = null;
            }
		}
		#endregion

		#region GeneraCuotas
		private void GeneraCuotas (long idNovedad, byte CantCuotas, double Importe, string IP, string Usuario,int Mensual)
		{
			
			try
			{
                using (TransactionScope scope = new TransactionScope())
                {
                    int mes = int.Parse(Mensual.ToString().Substring(4, 2));

                    int ano = int.Parse(Mensual.ToString().Substring(0, 4));

                    for (byte i = 1; i <= CantCuotas; i++)
                    {
                        Mensual = int.Parse(ano.ToString() + (mes < 10 ? "0" + mes.ToString() : mes.ToString()));
                        if (mes == 12)
                        {
                            ano++;
                            mes = 1;
                        }
                        else
                        {
                            mes++;
                        }

                        object[] datos = new object[4];

                        datos[0] = idNovedad;
                        datos[1] = i;
                        datos[2] = Importe;
                        datos[3] = Mensual;

                        // No se generara Hash para las cuotas.
                     
                        AltaCuota(idNovedad, i, Importe, IP, Usuario, Mensual);
                     }
                    scope.Complete(); // verif
                    return;
                }
			}
	
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en servicio GeneraCuotas -  ERROR: " + err.Message + " - SRC: " + err.Source);
			}
			finally
			{			
			}
		}
		#endregion

		#region AltaCuota
		private void AltaCuota( long IdNovedad, byte NroCuota, double Importe,  string IP, string Usuario, int Mensual)
		{
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Cuotas_A");

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    db.AddInParameter(dbCommand, "@idNovedad", DbType.Int64, IdNovedad);
                    db.AddInParameter(dbCommand, "@NroCuota", DbType.Byte, NroCuota);
                    db.AddInParameter(dbCommand, "@Importe", DbType.Decimal, Importe);
                    db.AddInParameter(dbCommand, "@IP", DbType.AnsiString, IP);
                    db.AddInParameter(dbCommand, "@Usuario", DbType.AnsiString, Usuario);
                    db.AddInParameter(dbCommand, "@Mensual", DbType.Int32, Mensual);

                    db.ExecuteNonQuery(dbCommand);
                    scope.Complete(); // verif
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en servicio Cuotas_A -  ERROR: " + err.Message + " - SRC: " + err.Source);
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
		}

		#endregion
		#endregion

		#region Novedades_T3_Baja()
		private string Novedades_T3_Baja(byte idEstadoReg, string IP, string Usuario, int Mensual, DataSet NovVieja)
		{
			
			DataSet ds = new DataSet();
			Cierres cie = new Cierres();
			
			try
			{
                using (TransactionScope scope = new TransactionScope())
                {

                    string mensaje = String.Empty;

                    string filtro = "(MensualCuota >= " + Mensual.ToString() + ")";
                    NovVieja.Tables[0].DefaultView.RowFilter = filtro;
                    NovVieja.Tables[0].DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                    DataView dvNovViejas = NovVieja.Tables[0].DefaultView;

                    if (dvNovViejas.Count != 1 || (int.Parse(dvNovViejas[0]["MensualCuota"].ToString()) != Mensual))
                    {
                        mensaje = "Sólo se puede dar de baja a partir de la última cuota y en forma descendente.";
                    }

                    if (mensaje == string.Empty)
                    {
                        if (dvNovViejas[0]["IdEstadoReg"].ToString() != "1")
                        {
                           mensaje = "Novedad en proceso de liquidación. No puede darse de baja";
                        }
                    }
                    if (mensaje == string.Empty)
                    {

                        int MensualAct = int.Parse(cie.Trae_Fec_Prox_Cierre().Tables[0].Rows[0]["Mensual"].ToString());

                        long IdNovedad = long.Parse(dvNovViejas[0]["IdNovedad"].ToString());


                        if (Mensual == MensualAct)
                        {
                            long IdPrestador = long.Parse(dvNovViejas[0]["IdPrestador"].ToString());
                            long IdBeneficiario = long.Parse(dvNovViejas[0]["IdBeneficiario"].ToString());
                            int ConceptoOPP = int.Parse(dvNovViejas[0]["CodConceptoLiq"].ToString());
                            double Importe = double.Parse(dvNovViejas[0]["ImporteCuota"].ToString()) * -1;

                            ModificaSaldo(IdPrestador, IdBeneficiario, ConceptoOPP, Importe, Usuario);
                        }

                         Novedades_PasaAHist(IdNovedad, Mensual, idEstadoReg, 3, 0, IP, Usuario);					//Modificado por COK 09.08.2005

                        scope.Complete(); // verif
                    }
                    
                    return mensaje + "|0| ";
                }
			}
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_T3_Baja -  ERROR: " + err.Message + " - SRC: " + err.Source);
				
			}
			finally
			{
				ds.Dispose();
			}
		}
		
		#endregion
		#endregion

		#region ABM_Sobre_BD

		#region Novedades_Alta_Fisica
		private String[] Novedades_Alta_Fisica(long IdPrestador, long IdBeneficiario, DateTime FecNovedad, 
			byte TipoConcepto, int ConceptoOPP, double ImpTotal, byte CantCuotas,Single Porcentaje,	 
			byte CodMovimiento, string  NroComprobante, string IP, string Usuario, byte IdEstadoReg)
		{		

			string dato = Genera_Datos_para_MAC(IdBeneficiario, IdPrestador,FecNovedad,CodMovimiento,ConceptoOPP,TipoConcepto,
				ImpTotal,CantCuotas,Porcentaje,NroComprobante,IP,Usuario);
					
			string MAC = Utilidades.Calculo_MAC(dato);

            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_A");

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    String[] retorno = new String[2];

                    db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, IdBeneficiario);
                    db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, IdPrestador);
                    db.AddInParameter(dbCommand, "@FecNovedad", DbType.DateTime, FecNovedad);
                    db.AddInParameter(dbCommand, "@CodMovimiento", DbType.Byte, CodMovimiento);    
                    db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Byte, TipoConcepto);    
                    db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, ConceptoOPP);    
                    db.AddInParameter(dbCommand, "@ImporteTotal", DbType.Decimal, ImpTotal);   
                    db.AddInParameter(dbCommand, "@CantCuotas", DbType.Byte, CantCuotas);    
                    db.AddInParameter(dbCommand, "@Porcentaje", DbType.Decimal, Porcentaje);  
                    db.AddInParameter(dbCommand, "@MAC", DbType.AnsiString, MAC);   
                    db.AddInParameter(dbCommand, "@NroComprobante", DbType.AnsiString, NroComprobante);  /* OJO en un futuro se va a exigir cargar el nro de comprobante */ 
                    db.AddInParameter(dbCommand, "@Usuario", DbType.AnsiString, Usuario);   
                    db.AddInParameter(dbCommand, "@IP", DbType.AnsiString, IP);   
                    db.AddInParameter(dbCommand, "@IdEstadoReg", DbType.Byte, IdEstadoReg);   
                    db.AddOutParameter(dbCommand, "@IdNovedad", DbType.Int64, 0);   
                        
                   db.ExecuteNonQuery(dbCommand);

                   retorno[0] = db.GetParameterValue(dbCommand, "@IdNovedad").ToString();
                   retorno[1] = MAC;
                   scope.Complete(); // verif
                  
                    return retorno;
                }
			}
			catch(Exception err)
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
			
		#endregion
		
		#region Novedades Rechazadas A ConTasas

		
		public void Novedades_Rechazadas_A_ConTasas(
			long IdBeneficiario,
			long IdPrestador ,
			byte CodMovimiento ,
			byte TipoConcepto ,
			int CodConceptoLiq ,
			double ImporteTotal ,
			byte CantCuotas ,
			Single Porcentaje ,
			string NroComprobante ,
			string IP,
			string Usuario ,
			DateTime FecRechazo ,
			decimal montoPrestamo,
			decimal CuotaTotalMensual,
			decimal TNA,
			decimal TEM,
			decimal gastoOtorgamiento,
			decimal gastoAdmMensual,
			decimal cuotaSocial,
			decimal CFTEA,
			decimal CFTNAReal,
			decimal CFTEAReal,
			decimal gastoAdmMensualReal,
			decimal TIRReal,
			string mensajeError)
		{
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_Rechazadas_A_ConTasas");
        
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                     db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, IdBeneficiario);
                     db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, IdPrestador);
                     db.AddInParameter(dbCommand, "@CodMovimiento", DbType.Byte, CodMovimiento);
                     db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Byte, TipoConcepto);
                     db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, CodConceptoLiq);
                     db.AddInParameter(dbCommand, "@ImporteTotal", DbType.Decimal, ImporteTotal); 
                     db.AddInParameter(dbCommand, "@CantCuotas", DbType.Byte, CantCuotas);
                     db.AddInParameter(dbCommand, "@Porcentaje", DbType.Decimal, Porcentaje);
                     db.AddInParameter(dbCommand, "@NroComprobante", DbType.AnsiString, NroComprobante);//Como limito la cantidad a 50
                     db.AddInParameter(dbCommand, "@IP", DbType.AnsiString, IP);//Como limito la cantidad a 20
                     db.AddInParameter(dbCommand, "@Usuario", DbType.AnsiString, Usuario);//Como limito la cantidad a 50
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
                     scope.Complete(); // verif
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
		#endregion
	
		#region NovedadRechazada_Alta
		private void NovedadRechazada_Alta(long IdPrestador, long IdBeneficiario, DateTime FecNovedad, 
			byte TipoConcepto, int ConceptoOPP, string MAC, double ImpTotal, byte CantCuotas,Single Porcentaje,	 
			byte CodMovimiento, string  NroComprobante, string IP, string Usuario,int Mensual, string mensajeError)
		{
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_Rechazadas_A");

            try
            {
                using (TransactionScope scope = new TransactionScope())                
                {
                    db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, IdBeneficiario);
                    db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, IdPrestador);
                    db.AddInParameter(dbCommand, "@CodMovimiento", DbType.Byte, CodMovimiento); 
                    db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Byte, TipoConcepto); 
                    db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, ConceptoOPP);   
                    db.AddInParameter(dbCommand, "@ImporteTotal", DbType.Decimal, ImpTotal);  
                    db.AddInParameter(dbCommand, "@CantCuotas", DbType.Byte, CantCuotas);       
                    db.AddInParameter(dbCommand, "@Porcentaje", DbType.Decimal, Porcentaje);    
                    db.AddInParameter(dbCommand, "@NroComprobante", DbType.AnsiString, NroComprobante);//Como limito la cantidad a 50
                    db.AddInParameter(dbCommand, "@IP", DbType.AnsiString, IP);//Como limito la cantidad a 20
                    db.AddInParameter(dbCommand, "@Usuario", DbType.AnsiString, Usuario);//Como limito la cantidad a 15
                    db.AddInParameter(dbCommand, "@FecRechazo", DbType.DateTime, DateTime.Today);
                    db.AddInParameter(dbCommand, "@TipoRechazo", DbType.AnsiString, mensajeError);//Como limito la cantidad a 300
                       
                    db.ExecuteNonQuery(dbCommand);
                    
                    scope.Complete(); // verif
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en Novedades_Rechazadas_A - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
		#endregion
	
		#region Novedades_Modifica_EstadoReg
		private void Novedades_Modifica_EstadoReg(long IdNovedad,byte IdEstadoReg, string IP, string Usuario)
        {
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_M");

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, IdNovedad);
                    db.AddInParameter(dbCommand, "@IdEstadoReg", DbType.Int64, IdEstadoReg);
                    db.AddInParameter(dbCommand, "@Usuario", DbType.AnsiString, Usuario);//Como limito la cantidad a 50
                    db.AddInParameter(dbCommand, "@IP", DbType.AnsiString, IP);//Como limito la cantidad a 20

                    db.ExecuteNonQuery(dbCommand);

                    scope.Complete(); // verif
                 }
			}
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en Novedades_M - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }			
		}
		#endregion

		#region Novedades_PasaAHist		
		private static void Novedades_PasaAHist( long IdNovedad, int Mensual, byte IdEstadoReg, byte IdEstadoNov, 
			double ImporteLiq, string IPCuota, string Usuario)
		{
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_PaHist");

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, IdNovedad);
                    db.AddInParameter(dbCommand, "@Mensual", DbType.Int32, Mensual);
                    db.AddInParameter(dbCommand, "@IdEstadoReg", DbType.Byte, IdEstadoReg);
                    db.AddInParameter(dbCommand, "@IdEstadoNov", DbType.Byte, IdEstadoNov);
                    db.AddInParameter(dbCommand, "@ImporteLiq", DbType.Decimal, ImporteLiq);
                    db.AddInParameter(dbCommand, "@IP", DbType.AnsiString, IPCuota);//Como limito la cantidad a 20
                    db.AddInParameter(dbCommand, "@Usuario", DbType.AnsiString, Usuario);//Como limito la cantidad a 50
                     
                    db.ExecuteNonQuery(dbCommand);

                    scope.Complete(); // verif
                    return;
                }
			}
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en Novedades_PaHist - ERROR: " + err.Message + " - SRC: " + err.Source);
			}
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
		}

		#endregion

		#region ModificaSaldo
		private long ModificaSaldo(long IdPrestador, long IdBeneficiario, int ConceptoOPP,  double Importe,string Usuario)
		{
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Beneficiarios_MSaldo");

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, IdBeneficiario);
                    db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, IdPrestador);
                    db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, ConceptoOPP);
                    db.AddInParameter(dbCommand, "@Importe", DbType.Decimal, Importe); 
                    db.AddInParameter(dbCommand, "@Usuario", DbType.AnsiString, Usuario);//Como limito la cantidad a 50

                    db.ExecuteNonQuery(dbCommand);

                   scope.Complete(); // verif
                   return 0;
                }
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

		#endregion 

		#endregion ABM Novedad

		#region Validaciones Comunes a todos los tipos

		#region Valido Nov
		private string Valido_Nov(long IdPrestador, long IdBeneficiario, byte TipoConcepto, 
			int ConceptoOPP, double ImpTotal, byte CantCuotas,Single Porcentaje, byte CodMovimiento, String NroComprobante )
		{
			return Valido_Nov(IdPrestador, IdBeneficiario, TipoConcepto, ConceptoOPP, ImpTotal, CantCuotas, Porcentaje,
							CodMovimiento, NroComprobante,true, 0);
		}

		private string Valido_Nov(long IdPrestador, long IdBeneficiario, byte TipoConcepto,
            int ConceptoOPP, double ImpTotal, byte CantCuotas, Single Porcentaje, byte CodMovimiento, String NroComprobante, bool bGestionErrores, decimal montoPrestamo)
		{
			string mensaje = String.Empty;
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedad_Valido_Derecho");

            try
            {
                // CONTROLA MAXIMOS INTENTOS 
                if (bGestionErrores)
                    mensaje = CtrolIntentos(IdPrestador, IdBeneficiario);

                // CONTROLA QUE ESTE INFORMADO EL COMPROBANTE
                if ((mensaje == String.Empty) && (NroComprobante.Trim().Length < 4))
                {
                    mensaje = "El nro. de comprobante debe ser mayor a 3 dígitos.";
                }
                // CONTROLA TIPOS DE CAMPOS

                if (mensaje == String.Empty)
                {
                    mensaje = CtrolMontos(TipoConcepto, ImpTotal, CantCuotas, Porcentaje);
                }


                // valida la novedad
                if (mensaje == String.Empty)
                {
                   #region parametros

                    db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, IdPrestador);
                    db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, IdBeneficiario);
                    db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Byte, TipoConcepto);
                    db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, ConceptoOPP);
                    db.AddInParameter(dbCommand, "@CodMovimiento", DbType.Byte, CodMovimiento);
                    db.AddInParameter(dbCommand, "@ImporteTotal", DbType.Decimal, ImpTotal); //ESTE ES DECIMAL O DOUBLE????
                    db.AddInParameter(dbCommand, "@CantCuotas", DbType.Byte, CantCuotas);
                    db.AddInParameter(dbCommand, "@Porcentaje", DbType.Decimal, Porcentaje);//ESTE ES DECIMAL O DOUBLE????
                    db.AddInParameter(dbCommand, "@NroComprobante", DbType.AnsiString, NroComprobante);//como limito la longitud a 100
                    db.AddInParameter(dbCommand, "@MontoPrestamo", DbType.Decimal, montoPrestamo);//ESTE ES DECIMAL O DOUBLE????
                    db.AddOutParameter(dbCommand, "@Mensaje", DbType.AnsiString, 100);
                    db.AddOutParameter(dbCommand, "@EsAfiliacion", DbType.Boolean, 1);

                   #endregion parametros

                    db.ExecuteNonQuery(dbCommand);
                  
                    mensaje = db.GetParameterValue(dbCommand, "@Mensaje").ToString() + '|' + db.GetParameterValue(dbCommand, "@EsAfiliacion").ToString();

                }
                if (mensaje != String.Empty)
                {
                    mensaje += "|";
                }

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
	
		#endregion

		#region Controles

		#region ConceptoOPP_Habil_X_Prest
		private string ConceptoOPP_Habil_X_Prest( long IdPrestador, int ConceptoOPP, byte TipoConcepto )
		{		
			// Controla que exista la relacion y si existe trae si es afiliacion o no
			string mensaje= String.Empty;
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("ConceptoOPP_Habil_X_Prest");

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, IdPrestador);
                    db.AddInParameter(dbCommand, "@ConceptoOPP", DbType.Int32, ConceptoOPP);
                    db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Byte, TipoConcepto);
                    db.AddInParameter(dbCommand, "@Ok", DbType.Boolean, 0);
                    db.AddInParameter(dbCommand, "@EsAfiliacion", DbType.Boolean, 0);
                   
                    db.ExecuteNonQuery(dbCommand);

                    if (Boolean.Parse(db.GetParameterValue(dbCommand, "@Ok").ToString()) == false)
                    {
                        mensaje = "Concepto Liq - Tipo Concepto no habilitada para el Prestador|False";
                    }
                    else
                    {
                        mensaje = "|" + db.GetParameterValue(dbCommand, "EsAfiliacion").ToString();
                        scope.Complete(); //verif
                    }
                    return mensaje;
                }
			}
			catch(Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));    
                throw new Exception("Error en ConceptoOPP_Habil_X_Prest - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }

		#endregion
				
		#region Ctrol de Duplicados 
		private string CtrolDuplicados(long IdPrestador, long IdBeneficiario, byte TipoConcepto, 
			int ConceptoOPP, double ImpTotal, byte CantCuotas, Single Porcentaje,
			string NroComprobante)
		{

            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_Existe");

            try
            {
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, IdPrestador);
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, IdBeneficiario);
                db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Byte, TipoConcepto);
                db.AddInParameter(dbCommand, "@ConceptoOPP", DbType.Int32, ConceptoOPP);
                db.AddInParameter(dbCommand, "@ImpTotal", DbType.Int64, ImpTotal);
                db.AddInParameter(dbCommand, "@CantCuotas", DbType.Byte, CantCuotas);
                db.AddInParameter(dbCommand, "@Porcentaje", DbType.Int16, Porcentaje);
                db.AddInParameter(dbCommand, "@NroComprobante", DbType.AnsiString, NroComprobante);
                db.AddOutParameter(dbCommand, "@Existe", DbType.Boolean, 0);

                db.ExecuteNonQuery(dbCommand);

                return ((Boolean)db.GetParameterValue(dbCommand, "@Existe") == true) ? "Ud. está intentando re-ingresar una novedad ya existente. Proceso cancelado" : String.Empty;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en servicio Novedades_Existe - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
			finally
			{
                dbCommand.Dispose();
                db = null;
			}
        }
		#endregion

		#region CtrolAlcanza
		
		public string CtrolAlcanza( long IdBeneficiario, double Importe, long IdPrestador, int ConceptoOPP )
		{		
			// controla si alcanza el monto a ingresar - si no alcanza ingresa el monto en rechazados		
			string mensaje= String.Empty;

            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("AlcanzaAfectacion");

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, IdBeneficiario);
                db.AddInParameter(dbCommand, "@monto", DbType.Decimal, Importe);
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, IdPrestador);
                db.AddInParameter(dbCommand, "@ConceptoOPP", DbType.Int32, ConceptoOPP);
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

		#region CtrolCantRechazos
		private int CtrolCantRechazos(long IdPrestador, long IdBeneficiario)
		{
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("NovRechazadas_TCant");

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, IdBeneficiario);
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, IdPrestador);
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

		#region CtrolIntentos
		private string CtrolIntentos(long IdPrestador, long IdBeneficiario)
		{		
			string mensaje =String.Empty;
            try
            {

                int MaxIntentos = int.Parse(ConfigurationSettings.AppSettings["DAT_MaxIntentos"].ToString());
                int MaxCantRechazos = CtrolCantRechazos(IdPrestador, IdBeneficiario);

                mensaje = (MaxCantRechazos >= MaxIntentos) ? "Máxima cantidad de intentos permitidos" : String.Empty;
                return (mensaje);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio CtrolIntentos - ERROR: " + err.Message + " - SRC: " + err.Source);
            }			
		}
		#endregion

		#region CtrolBloqueado
		private string CtrolBloqueado ( long IdBeneficiario)
		{
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Beneficios_Bloqueados_Busca");

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, IdBeneficiario);
                db.AddOutParameter(dbCommand, "@Existe", DbType.Int64, 0);

                db.ExecuteNonQuery(dbCommand);

                return ((Boolean)db.GetParameterValue(dbCommand, "@Existe") == true) ? "Beneficio Bloqueado" : String.Empty;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Beneficios_Bloqueados_Busca - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
			finally
			{
                dbCommand.Dispose();
                db = null;
			}
        }
		#endregion

		#region CtrolInhibido
		private string CtrolInhibido (long IdPrestador, long IdBeneficiario, int ConceptoOPP)
		{
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Beneficios_Inhibido_Busca");

            try
            {
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, IdPrestador);
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, IdBeneficiario);
                db.AddInParameter(dbCommand, "@ConceptoLiq", DbType.Int32, ConceptoOPP);
                db.AddOutParameter(dbCommand, "@Existe", DbType.Boolean, 0);

                db.ExecuteNonQuery(dbCommand);

                return ((Boolean)db.GetParameterValue(dbCommand, "@Existe") == true) ? String.Concat("Código inhibido para el Beneficio:", IdBeneficiario.ToString()) : String.Empty;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Beneficios_Inhibido_Busca - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
			finally
			{
                dbCommand.Dispose();
                db = null;
			}
        }
		#endregion

		#region CtrolOcurrencias
		
		private string CtrolOcurrencias (byte CantOcurrDisp,long IdBeneficiario, int ConceptoOPP)
		{
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_Alcanza_Ocurrencia");

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, IdBeneficiario);
                db.AddInParameter(dbCommand, "@ConceptoLiq", DbType.Int32, ConceptoOPP);
                db.AddInParameter(dbCommand, "@CantOcurrDisp", DbType.Int64, CantOcurrDisp);
                db.AddOutParameter(dbCommand, "@Alcanza", DbType.Boolean, 0);

                db.ExecuteNonQuery(dbCommand);

                return ((Boolean)db.GetParameterValue(dbCommand, "@Alcanza") == false ? "La Liquidación no posee lugar para ingresar un nuevo descuento" : String.Empty);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_Alcanza_Ocurrencia - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
			finally
			{
                dbCommand.Dispose();
                db = null;
			}
        }
		#endregion

		#region CtrolOcurrenciasCancCuotas
		
		public Boolean CtrolOcurrenciasCancCuotas (byte CantOcurrDisp,long IdBeneficiario, int ConceptoOPP, long IdNovedadABorrar)
		{
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_Alcanza_Ocurrencia_Xa_CancCuotas");

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, IdBeneficiario);
                db.AddInParameter(dbCommand, "@ConceptoLiq", DbType.Int32, ConceptoOPP);
                db.AddInParameter(dbCommand, "@CantOcurrDisp", DbType.Int64, CantOcurrDisp);
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, IdNovedadABorrar);
                db.AddOutParameter(dbCommand, "@Alcanza", DbType.Boolean, 0);

                db.ExecuteNonQuery(dbCommand);

                return ((Boolean) db.GetParameterValue(dbCommand, "@Alcanza"));
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_Alcanza_Ocurrencia_Xa_CancCuotas - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
			finally
			{
                dbCommand.Dispose();
                db = null;
			}
		}
		#endregion
	
		#region CtrolRestricciones
		private string CtrolRestricciones (long IdPrestador, long IdBeneficiario, int ConceptoOPP)
		{
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("RestriccionesXCodConceptoLiq_TxExCaja");

            try
            {
                db.AddInParameter(dbCommand, "@ConceptoLiq", DbType.Int32, ConceptoOPP);
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, IdPrestador);
                db.AddInParameter(dbCommand, "@ExCaja", DbType.Byte, IdBeneficiario.ToString("00000000000").Substring(0, 2));
                db.AddOutParameter(dbCommand, "@Ok", DbType.Boolean, 0);

                db.ExecuteNonQuery(dbCommand);
               
                return ((Boolean) db.GetParameterValue(dbCommand, "@Ok") == false ? "Beneficio restringido para éste concepto" : String.Empty);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio RestriccionesXCodConceptoLiq_TxExCaja - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
			finally
			{
                dbCommand.Dispose();
                db = null;
			}
		}
		#endregion

		#region CtrolMontos
		private string CtrolMontos( byte TipoConcepto, double ImpTotal, byte CantCuotas, Single Porcentaje)
		{
			string mensajeMontos = String.Empty;
			switch (TipoConcepto)
			{			
				case 1:
				case 2:
					if ( ImpTotal <= 0 ) { mensajeMontos = @"El campo Importe debe ser mayor a 0" ;}
					break;
				
				case 3:
					if ( ImpTotal <= 0 ) { mensajeMontos = @"El importe resultante de resta la cuota total  y cuota afiliación debe ser mayor a CERO (0)" ;}

					if (mensajeMontos == String.Empty)
					{
						if (CantCuotas <= 0 || CantCuotas > 240) { mensajeMontos = @"El campo Cant. Cuotas debe estar comprendido entre 1 y 240" ;}
					}									
					break;
					
				case 6:
					if  (Porcentaje <= 0 || Porcentaje > 100) { mensajeMontos = @"El campo Porcentaje debe ser mayor que 0 y menor a 100"; }
					break;
					
				default:
					mensajeMontos = @"Opción no contemplada";
					break;
			}
			return mensajeMontos;
		}
		#endregion

		#region CtrolPuedeAltaNovXTipo
		private string CtrolPuedeAltaNovXTipo(long IdPrestador, byte TipoConcepto, int ConceptoOPP ,long IdBeneficiario, Boolean EsAfiliacion)
		{		
			/*
			 * Verifica que segun los tipo de concepto de las novedades cargadas se pueda cargar nueva novedad.
			 * */
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_CtrolPuedeAltaXTipo");

			string mensaje = string.Empty;
			Boolean resp;

            try
            {
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, IdPrestador);
                db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Byte, TipoConcepto);
                db.AddInParameter(dbCommand, "@ConceptoOPP", DbType.Int32, ConceptoOPP);
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, IdBeneficiario);
                db.AddInParameter(dbCommand, "@EsAfiliacion", DbType.Boolean, EsAfiliacion);
                db.AddOutParameter(dbCommand, "@Ok", DbType.Boolean, 0);

                db.ExecuteNonQuery(dbCommand);
                
                resp = (Boolean) db.GetParameterValue(dbCommand, "@Ok");
                if (resp == true)
                {
                    mensaje = string.Empty;
                }
                else
                {
                    if (EsAfiliacion == true)
                    {
                        mensaje = "Solo se puede cargar una novedad de Afiliación";
                    }
                    else
                    {
                        if (TipoConcepto == 6)
                        {
                            mensaje = "Existen novedades con monto fijo. No se puede cargar novedades con monto fijo y con porcentaje";
                        }
                        else
                        {
                            mensaje = "Existen novedades con porcentaje. No se puede cargar novedades con monto fijo y con porcentaje";
                        }
                    }
                }

                return mensaje;

            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_CtrolPuedeAltaXTipo - ERROR: " + err.Message + " - SRC: " + err.Source);
            }
			finally
			{
                dbCommand.Dispose();
                db = null;
			}
		}
		#endregion

		#endregion		
		
		#endregion

		#region Genera_Datos_para_MAC
		private string Genera_Datos_para_MAC (long IdBeneficiario,long IdPrestador, DateTime FecNovedad,  
			byte CodMovimiento, int ConceptoOPP, byte TipoConcepto,  double ImpTotal, byte CantCuotas, 
			Single Porcentaje, string NroComprobante, string IP,  string Usuario)
		{
			object[] datos = new object[12];
														
			datos[0]= IdBeneficiario;
			datos[1]= IdPrestador;
			datos[2]= FecNovedad;
			datos[3]= CodMovimiento;
			datos[4]= ConceptoOPP;
			datos[5]= TipoConcepto;
			datos[6]= ImpTotal;
			datos[7]= CantCuotas;
			datos[8]= Porcentaje;
			datos[9]= NroComprobante;
			datos[10]=IP;
			datos[11]=Usuario;

			return(Utilidades.Armo_String_MAC(datos));
		}
		#endregion

	}

	#region Listados - Novedad NO Transaccional

	
	public class Novedad
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(Novedad).Name); 
		public Novedad ()
		{
		}

		#region Novedades_TraerTodos

		
		public DataSet  Novedades_TraerTodos(long lintPrestador)
		{
			DataSet ds = new DataSet();
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_TT");

			try
			{
                db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, lintPrestador);
                ds = db.ExecuteDataSet(dbCommand);
			    return ds;
			} 
			catch(SqlException ErrSQL)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));  
                
                if ( ErrSQL.Number == 1205 || ErrSQL.Number == 1204 ) 
				{
					throw new ApplicationException("Interbloqueo");	
				}
				else
				{                    
                    throw new Exception("Error en servicio Novedades_TT - ERROR: " + ErrSQL.Message + " - SRC: " + ErrSQL.Source);
				}
            }
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_BajaT - ERROR: " + err.Message + " - SRC: " + err.Source);
			}
			finally
			{
				ds.Dispose();
                dbCommand.Dispose();
                db = null;
			}
		}

		#endregion

		#region Novedades_Traer 

		private DataSet  Novedades_TraerConsulta(byte opcion, long lintPrestador,long benefCuil, byte tipoConc, int concopp, int mensual, string fdesde, string fhasta )
		{
			DataSet ds = new DataSet();
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_TT_SinMigrar");
	
			try
			{
                db.AddInParameter(dbCommand, "@Opcion", DbType.Byte, opcion);
                db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, lintPrestador);
                db.AddInParameter(dbCommand, "@BenefCuil", DbType.Int64, benefCuil);
                db.AddInParameter(dbCommand, "@TipoConc", DbType.Byte, tipoConc);
                db.AddInParameter(dbCommand, "@Conc", DbType.Int32, concopp);
                db.AddInParameter(dbCommand, "@Mensual", DbType.Int32, mensual);
                db.AddInParameter(dbCommand, "@desde", DbType.AnsiString, fdesde);
                db.AddInParameter(dbCommand, "@hasta", DbType.AnsiString, fhasta);

                ds = db.ExecuteDataSet(dbCommand);
				return ds;
			} 
			catch(SqlException ErrSQL)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));   
              
                if ( ErrSQL.Number == 1205 || ErrSQL.Number == 1204 ) 
				{
					throw new ApplicationException("Interbloqueo");	
				}
				else
				{
                    throw new Exception("Error en servicio Novedades_TT_SinMigrar - ERROR: " + ErrSQL.Message + " - SRC: " + ErrSQL.Source);
				}
            }
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_TT_SinMigrar - ERROR: " + err.Message + " - SRC: " + err.Source);
			}
			finally
			{
				ds.Dispose();
                dbCommand.Dispose();
                db = null;
			}
		}
		
		public DataSet  Novedades_Traer(byte OpcionBusqueda, long idPrestador,long benefCuil, byte tipoConc, int concopp, int mensual,  string fdesde, string fhasta,bool GeneraArchivo, out string rutaArchivoSal)
		{

			string rutaArchivo = string.Empty;
			string nombreArchivo = string.Empty;
			rutaArchivoSal =string.Empty;
			string rta =string.Empty;
			string nombreConsulta = "Novedades_ingresadas";
            try
            {
                if (OpcionBusqueda != 1 || GeneraArchivo == true)
                {

                    rta = new ConsultasBatch().ExisteConsulta(idPrestador, nombreConsulta, 0, OpcionBusqueda, mensual.ToString(), tipoConc, concopp, benefCuil, fdesde, fhasta);
                    if (rta != string.Empty)
                    {
                        throw new ApplicationException("MSG_ERROR" + rta + "FIN_MSG_ERROR");
                    }
                }

                DataSet ds = Novedades_TraerConsulta(OpcionBusqueda, idPrestador, benefCuil, tipoConc, concopp, mensual, fdesde, fhasta);

                if ((ds.Tables[0].Rows.Count != 0) && (OpcionBusqueda != 1 || GeneraArchivo == true))
                {
                    int maxCantidad = new Conexion().MaxCantidadRegistros();

                    if (ds.Tables[0].Rows.Count >= maxCantidad || GeneraArchivo == true)
                    {
                        nombreArchivo = Utilidades.GeneraNombreArchivo(nombreConsulta, idPrestador, out rutaArchivo);

                        StreamWriter sw = new StreamWriter(rutaArchivo + nombreArchivo, false, System.Text.Encoding.UTF8);

                        string separador = new Conexion().DelimitadorCampo();

                        DataTable miTabla = ds.Tables[0];

                        #region codigo comentado
                        /*	
						StringBuilder cabecera = new StringBuilder();
						cabecera.Append ( "IdNovedad"+separador);
						cabecera.Append ( "IdBeneficiario"+separador);
						cabecera.Append ( "ApellidoNombre"+separador);
						cabecera.Append ( "FecNov"+separador);
						cabecera.Append ( "TipoConcepto"+separador);
						cabecera.Append ( "DescTipoConcepto"+separador);
						cabecera.Append ( "CodConceptoLiq"+separador);
						cabecera.Append ( "DescConceptoLiq"+separador);
						cabecera.Append ( "ImporteTotal"+separador);
						cabecera.Append ( "CantCuotas"+separador);
						cabecera.Append ( "Porcentaje"+separador);
						cabecera.Append ( "ImporteCuota"+separador);
						cabecera.Append ( "NroCuota"+separador);
						cabecera.Append ( "MensualCuota"+separador);
						cabecera.Append ( "Nrocomprobante"+separador);
						cabecera.Append ( "MAC"+separador);
						cabecera.Append ( "Usuario");
						
						sw.WriteLine(cabecera.ToString());																		
						*/
                        #endregion

                        foreach (DataRow fila in miTabla.Rows)
                        {
                            StringBuilder linea = new StringBuilder();
                            linea.Append(fila["IdNovedad"].ToString() + separador);
                            linea.Append(fila["IdBeneficiario"].ToString() + separador);
                            linea.Append(fila["ApellidoNombre"].ToString() + separador);
                            linea.Append(DateTime.Parse(fila["FecNov"].ToString()).ToString("dd/MM/yyyy HH:mm:ss") + separador);
                            linea.Append(fila["TipoConcepto"].ToString() + separador);
                            linea.Append(fila["DescTipoConcepto"].ToString() + separador);
                            linea.Append(fila["CodConceptoLiq"].ToString() + separador);
                            linea.Append(fila["DescConceptoLiq"].ToString() + separador);
                            linea.Append(fila["ImporteTotal"].ToString().Replace(",", ".") + separador);
                            linea.Append(fila["CantCuotas"].ToString() + separador);
                            linea.Append(fila["Porcentaje"].ToString().Replace(",", ".") + separador);
                            linea.Append(fila["ImporteCuota"].ToString().Replace(",", ".") + separador);
                            linea.Append(fila["NroCuota"].ToString() + separador);
                            linea.Append(fila["MensualCuota"].ToString() + separador);
                            linea.Append(fila["NroComprobante"].ToString() + separador);
                            linea.Append(fila["MAC"].ToString() + separador);
                            linea.Append(fila["Usuario"].ToString());

                            sw.WriteLine(linea.ToString());

                        }
                        sw.Close();

                        Utilidades.ComprimirArchivo(rutaArchivo, nombreArchivo);
                        Utilidades.BorrarArchivo(rutaArchivo, nombreArchivo);

                        nombreArchivo = nombreArchivo + ".zip";
                        rutaArchivoSal = rutaArchivo + nombreArchivo;

                        ds = new DataSet();
                        ds.Tables.Add(new DataTable());

                        string fGeneracion = DateTime.Now.ToString("yyyyMMdd hh:mm:ss:fff");

                        rta = new ConsultasBatch().AltaNuevaConsulta(idPrestador, nombreConsulta, 0, OpcionBusqueda, mensual.ToString(), tipoConc, concopp, benefCuil, fdesde, fhasta, rutaArchivo, nombreArchivo, fGeneracion, true);

                        if (rta != string.Empty)
                        {
                            rta = "MSG_ERROR" + rta + "FIN_MSG_ERROR";
                            throw new ApplicationException(rta);
                        }
                    }
                }
                return ds;

            }
            catch (SqlException errsql)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), errsql.Source, errsql.Message));

                if (errsql.Number == -2)
                {
                    nombreArchivo = Utilidades.GeneraNombreArchivo(nombreConsulta, idPrestador, out rutaArchivo);
                    rta = new ConsultasBatch().AltaNuevaConsulta(idPrestador, nombreConsulta, 0, OpcionBusqueda, mensual.ToString(), tipoConc, concopp, benefCuil, fdesde, fhasta, rutaArchivo, nombreArchivo, string.Empty, false);

                    throw new ApplicationException("MSG_ERROR Generando el archivo. Reingrese a la consulta en unos minutos.FIN_MSG_ERROR");
                }
                else
                {
                    throw errsql;
                }
            }
            catch (ApplicationException apperr)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), apperr.Source, apperr.Message));
                throw new ApplicationException(apperr.Message);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
            }	
		}
		#endregion
			
		#region Novedades_Trae_NoAplicadas
						
		
		public DataSet  Novedades_Trae_NoAplicadas(byte OpcionBusqueda, long idPrestador, long benefCuil, byte tipoConc, int concopp,string fdesde, string fhasta, bool GeneraArchivo, int mensual,out string rutaArchivoSal )
		{
			string rutaArchivo = string.Empty;
			string nombreArchivo = string.Empty;
			rutaArchivoSal =string.Empty;
			string rta =string.Empty;
			string nombreConsulta = "Novedades_NoAplicadas";
			try
			{
				if (OpcionBusqueda != 1 || GeneraArchivo== true)
				{			 
					rta = new ConsultasBatch().ExisteConsulta ( idPrestador, nombreConsulta, 0, OpcionBusqueda, mensual.ToString(),tipoConc, concopp,benefCuil,fdesde ,fhasta);
					if ( rta != string.Empty)
					{
						throw new ApplicationException("MSG_ERROR"+rta+"FIN_MSG_ERROR");
					}
				}
				
				DataSet ds = Novedades_Trae_NoAplicadasConsulta(OpcionBusqueda, idPrestador, benefCuil, tipoConc, concopp,fdesde,fhasta);

				if ((ds.Tables[0].Rows.Count != 0) && (OpcionBusqueda != 1  || GeneraArchivo== true))
				{
					int maxCantidad =new Conexion().MaxCantidadRegistros();
				
					if (ds.Tables[0].Rows.Count>=maxCantidad || GeneraArchivo== true)
					{
						nombreArchivo = Utilidades.GeneraNombreArchivo(nombreConsulta,idPrestador,out rutaArchivo);

						StreamWriter sw = new StreamWriter(rutaArchivo + nombreArchivo,false,System.Text.Encoding.UTF8);		
						
						string separador = new Conexion().DelimitadorCampo();

						DataTable miTabla = ds.Tables[0];							
						
						foreach(DataRow fila in miTabla.Rows)
						{
							StringBuilder linea = new StringBuilder();
								
							linea.Append ( fila["IdNovedad"].ToString()+separador);
							linea.Append ( fila["IdBeneficiario"].ToString()+separador);
							linea.Append ( fila["ApellidoNombre"].ToString()+separador);
							linea.Append ( DateTime.Parse(fila["FecNov"].ToString()).ToString("dd/MM/yyyy HH:mm:ss")+separador);
							linea.Append ( fila["TipoConcepto"].ToString()+separador);
							linea.Append ( fila["DescTipoConcepto"].ToString()+separador);
							linea.Append ( fila["CodConceptoLiq"].ToString()+separador);
							linea.Append ( fila["DescConceptoLiq"].ToString()+separador);
							linea.Append ( fila["ImporteTotal"].ToString().Replace(",",".")+separador);
							linea.Append ( fila["CantCuotas"].ToString()+separador);
							linea.Append ( fila["Porcentaje"].ToString().Replace(",",".")+separador);
							linea.Append ( fila["ImporteCuota"].ToString().Replace(",",".")+separador);
							linea.Append ( fila["NroCuota"].ToString()+separador);
							linea.Append ( fila["MensualCuota"].ToString()+separador);
							linea.Append ( fila["NroComprobante"].ToString()+separador);
							linea.Append ( fila["MAC"].ToString()+separador);							
							linea.Append ( fila["Usuario"].ToString());							
								
							sw.WriteLine(linea.ToString());
							
						}
						sw.Close();
												
						Utilidades.ComprimirArchivo(rutaArchivo, nombreArchivo);
						Utilidades.BorrarArchivo(rutaArchivo,nombreArchivo);

						nombreArchivo=nombreArchivo+".zip";
						rutaArchivoSal = rutaArchivo+nombreArchivo;
										
						ds = new DataSet();						
						ds.Tables.Add(new DataTable());

						string fGeneracion = DateTime.Now.ToString("yyyyMMdd hh:mm:ss:fff");
						
						rta = new ConsultasBatch().AltaNuevaConsulta(idPrestador,nombreConsulta,0,OpcionBusqueda,mensual.ToString(),tipoConc,concopp,benefCuil,fdesde,fhasta,rutaArchivo,nombreArchivo,fGeneracion,true);

						if (rta != string.Empty)
						{
							rta = "MSG_ERROR"+rta+"FIN_MSG_ERROR";
							throw new ApplicationException(rta);
						}
					}
				}
				return ds;
			}
			catch (SqlException errsql)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), errsql.Source, errsql.Message));  
                
                if (errsql.Number == -2)
				{
					nombreArchivo = Utilidades.GeneraNombreArchivo(nombreConsulta,idPrestador,out rutaArchivo);
					rta = new ConsultasBatch().AltaNuevaConsulta(idPrestador,nombreConsulta,0,OpcionBusqueda,mensual.ToString(),tipoConc,concopp,benefCuil,fdesde,fhasta,rutaArchivo,nombreArchivo,string.Empty,false);
					
					throw new ApplicationException("MSG_ERROR Generando el archivo. Reingrese a la consulta en unos minutos.FIN_MSG_ERROR");
				}
				else
				{
                    throw new Exception("Error en servicio Novedades_Trae_NoAplicadas - ERROR: " + errsql.Message + " - SRC: " + errsql.Source);		
				}

			}
			catch( ApplicationException apperr)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), apperr.Source, apperr.Message));  
                throw new ApplicationException(apperr.Message) ;						
			}
			catch( Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_Trae_NoAplicadas - ERROR: " + err.Message + " - SRC: " + err.Source);					
			}
			finally
			{
            }	
        }

		private DataSet  Novedades_Trae_NoAplicadasConsulta(byte opcion, long lintPrestador, long benefCuil, byte tipoConc, int concopp,string fdesde, string fhasta )
		{
			DataSet ds = new DataSet();
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_TNoAplicadas");
			
			try
			{

                db.AddInParameter(dbCommand, "@Opcion", DbType.Byte, opcion);
                db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, lintPrestador);
                db.AddInParameter(dbCommand, "@BenefCuil", DbType.Int64, benefCuil);
                db.AddInParameter(dbCommand, "@TipoConc", DbType.Byte, tipoConc);
                db.AddInParameter(dbCommand, "@Conc", DbType.Int32, concopp);
                db.AddInParameter(dbCommand, "@fDesde", DbType.AnsiString, fdesde);
                db.AddInParameter(dbCommand, "@fHasta", DbType.AnsiString, fhasta);

                ds = db.ExecuteDataSet(dbCommand);

				return ds;
			} 
			catch(SqlException ErrSQL)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));  

				if ( ErrSQL.Number == 1205 || ErrSQL.Number == 1204 ) 
				{
					throw new ApplicationException("Interbloqueo");	
				}
				else
				{
                    throw new Exception("Error en servicio Novedades_TNoAplicadas - ERROR: " + ErrSQL.Message + " - SRC: " + ErrSQL.Source);
				}
            }
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_TNoAplicadas - ERROR: " + err.Message + " - SRC: " + err.Source);
			}
			finally
			{
				ds.Dispose();
                dbCommand.Dispose();
                db = null;
			}
		}
	
		#endregion

		#region Novedades_Traer_X_Id
		
		
		public DataSet  Novedades_Traer_X_Id ( long idNovedad)
		{
			DataSet ds = new DataSet();
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_TxIdNovedad");
	
			try
			{
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, idNovedad);

                ds = db.ExecuteDataSet(dbCommand);
				
				return ds;
            } 
			catch(SqlException ErrSQL)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));  
				
                if ( ErrSQL.Number == 1205 || ErrSQL.Number == 1204 ) 
				{
					throw new ApplicationException("Interbloqueo");	
				}
				else
				{
                    throw new Exception("Error en servicio Novedades_TxIdNovedad - ERROR: " + ErrSQL.Message + " - SRC: " + ErrSQL.Source);
				}

			}
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_BajaT - ERROR: " + err.Message + " - SRC: " + err.Source);
			}
			finally
			{
				ds.Dispose();
                dbCommand.Dispose();
                db = null;
			}
        }
		#endregion

		#region Novedades_TxIdNovedad_Sliq
		
		
		public DataSet  Novedades_TxIdNovedad_Sliq ( long idNovedad)
		{
			DataSet ds = new DataSet();
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_TxIdNovedad_Sliq");
	
			try
			{
                db.AddInParameter(dbCommand, "@idNovedad", DbType.Int64, idNovedad);

                ds = db.ExecuteDataSet(dbCommand);
				return ds;
            } 
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_TxIdNovedad_Sliq -  ERROR: " + err.Message + " - SRC: " + err.Source);
			}
			finally
			{
                dbCommand.Dispose();
                ds.Dispose();
                db = null;
            }
		}
		#endregion
		
		#region Novedades_Trae_Creditos_Activos

		
		public DataSet  Novedades_Trae_Creditos_Activos(long Prestador,long Beneficiario)
		{
			DataSet ds = new DataSet();
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_Tipo3_Vigentes");
	
			try
			{
                db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, Prestador);
                db.AddInParameter(dbCommand, "@Beneficiario", DbType.Int64, Beneficiario);

                ds = db.ExecuteDataSet(dbCommand);
				return ds;
			} 
			catch(SqlException ErrSQL)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));  
				
                if ( ErrSQL.Number == 1205 || ErrSQL.Number == 1204 ) 
				{
					throw new ApplicationException("Interbloqueo");	
				}
				else
				{
                    throw new Exception("Error en servicio Novedades_Tipo3_Vigentes - ERROR: " + ErrSQL.Message + " - SRC: " + ErrSQL.Source);
				}
            }
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_Tipo3_Vigentes - ERROR: " + err.Message + " - SRC: " + err.Source);
			}
			finally
			{
				ds.Dispose();
                dbCommand.Dispose();
                db = null;
			}
		}
		#endregion

		#region Novedades_T1o6_Trae

		
		public DataSet Novedades_T1o6_Trae(long Prestador,byte TipoConcepto)
		{
			DataSet ds = new DataSet();
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_T1o6_Trae");

			try
			{
				if (TipoConcepto == 1 || TipoConcepto == 6)
				{
                    db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, Prestador);
                    db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Byte, TipoConcepto);

                    ds = db.ExecuteDataSet(dbCommand);
				}			
				return ds;
			} 
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_T1o6_Trae - ERROR: " + err.Message + " - SRC: " + err.Source);
			}
			finally
			{
				ds.Dispose();
                dbCommand.Dispose();
                db = null;
			}
		}
		#endregion

		#region Novedades_Trae_Xa_ABM

		
		public DataSet Novedades_Trae_Xa_ABM(long Prestador, long Beneficiario)
		{
			DataSet ds = new DataSet();
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_TT_Xa_ABM");

			try
			{
                db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, Prestador);
                db.AddInParameter(dbCommand, "@Beneficiario", DbType.Int64, Beneficiario);

                ds = db.ExecuteDataSet(dbCommand);

				return ds;
			} 
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_TT_Xa_ABM - ERROR: " + err.Message + " - SRC: " + err.Source);
			}
			finally
			{
				ds.Dispose();
                dbCommand.Dispose();
                db = null;
			}
		}
		#endregion

		#region Novedades_BajaTxIDNov_FecBaja
		
		
		public DataSet  Novedades_BajaTxIDNov_FecBaja( long IdNov, string FechaBaja )
		{
			DataSet ds = new DataSet();
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_BajaTxIdNov_FecBaja");
			
			try
			{
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, IdNov);
                db.AddInParameter(dbCommand, "@FechaBaja", DbType.AnsiString, FechaBaja);

                ds = db.ExecuteDataSet(dbCommand);
				
				return ds;
            } 
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_BajaTxIdNov_FecBaja - ERROR: " + err.Message + " - SRC: " + err.Source);
			}
			finally
			{
				ds.Dispose();
                dbCommand.Dispose();
                db = null;
			}

		}
		#endregion

		#region Novedades de baja Traer

		
		public DataSet  Novedades_BajaTraer( long IDPrestador, byte OpcionBusqueda, long BenefCuil, byte TipoConc, int ConcOpp )
		{
			DataSet ds = new DataSet();
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_BajaT");
	
			try
			{
                db.AddInParameter(dbCommand, "@IDPrestador", DbType.Int64, IDPrestador);
                db.AddInParameter(dbCommand, "@opcion", DbType.Byte, OpcionBusqueda);
                db.AddInParameter(dbCommand, "@BenefCuil", DbType.AnsiString, BenefCuil);
                db.AddInParameter(dbCommand, "@TipoConc", DbType.Byte, TipoConc);
                db.AddInParameter(dbCommand, "@Conc", DbType.Int32, ConcOpp);

                ds = db.ExecuteDataSet(dbCommand);

				return ds;
            } 
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_BajaT - ERROR: " + err.Message + " - SRC: " + err.Source);
			}
			finally
			{
				ds.Dispose();
                dbCommand.Dispose();
                db = null;
			}
		}
		#endregion

		#region Novedades de baja Traer Agrupada		
		private DataSet  Novedades_BajaTraerAgrupadaConsulta( long IDPrestador, byte OpcionBusqueda, long BenefCuil, byte TipoConc, int ConcOpp,string MesAplica )
		{
			DataSet ds = new DataSet();
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("Novedades_BajaT_Agrupadas");
				
			try
			{
                db.AddInParameter(dbCommand, "@IDPrestador", DbType.Int64, IDPrestador);
                db.AddInParameter(dbCommand, "@opcion", DbType.Byte, OpcionBusqueda);
                db.AddInParameter(dbCommand, "@BenefCuil", DbType.AnsiString, BenefCuil);
                db.AddInParameter(dbCommand, "@TipoConc", DbType.Byte, TipoConc);
                db.AddInParameter(dbCommand, "@Conc", DbType.Int32, ConcOpp);
                db.AddInParameter(dbCommand, "@MesAplica", DbType.AnsiString, MesAplica);

                ds = db.ExecuteDataSet(dbCommand);
                
				return ds;
			} 
			catch(SqlException ErrSQL)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));  
				
                if ( ErrSQL.Number == 1205 || ErrSQL.Number == 1204 ) 
				{
					throw new ApplicationException("Interbloqueo");	
				}
				else
				{
                    throw new Exception("Error en servicio Novedades_BajaT_Agrupadas - ERROR: " + ErrSQL.Message + " - SRC: " + ErrSQL.Source);
                }
			}
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio Novedades_BajaT_Agrupadas - ERROR: " + err.Message + " - SRC: " + err.Source);
			}
			finally
			{
				ds.Dispose();
				dbCommand.Dispose();
                db = null;
			}
		}
			
		
		public DataSet  Novedades_BajaTraerAgrupada( long IDPrestador, byte OpcionBusqueda, long BenefCuil, byte TipoConc, int ConcOpp,string MesAplica,bool GeneraArchivo, out string rutaArchivoSal )
		{
			string rutaArchivo = string.Empty;
			string nombreArchivo = string.Empty;
			rutaArchivoSal =string.Empty;
			string rta =string.Empty;
			string nombreConsulta = "Novedades_Canceladas";
			try
			{
				if (OpcionBusqueda != 1 || GeneraArchivo== true)
				{								 
					rta = new ConsultasBatch().ExisteConsulta ( IDPrestador, nombreConsulta, 0, OpcionBusqueda, MesAplica,TipoConc, ConcOpp,BenefCuil,string.Empty ,string.Empty );
					if ( rta != string.Empty)
					{
						throw new ApplicationException("MSG_ERROR"+rta+"FIN_MSG_ERROR");
					}
				}

				DataSet ds = Novedades_BajaTraerAgrupadaConsulta (IDPrestador,OpcionBusqueda,BenefCuil,TipoConc,ConcOpp,MesAplica);

				if ((ds.Tables[0].Rows.Count != 0) && (OpcionBusqueda != 1  || GeneraArchivo== true))
				{
					int maxCantidad =new Conexion().MaxCantidadRegistros();
				
					if (ds.Tables[0].Rows.Count>=maxCantidad || GeneraArchivo== true)
					{
						nombreArchivo = Utilidades.GeneraNombreArchivo(nombreConsulta,IDPrestador,out rutaArchivo);

						StreamWriter sw = new StreamWriter(rutaArchivo + nombreArchivo,false,System.Text.Encoding.UTF8);		
						
						string separador = new Conexion().DelimitadorCampo();

						DataTable miTabla = ds.Tables[0];							
					
						foreach(DataRow fila in miTabla.Rows)
						{
							
							StringBuilder linea = new StringBuilder();
								
							linea.Append ( fila["cuil"].ToString()+separador);
							linea.Append ( fila["IdNovedad"].ToString()+separador);
							linea.Append ( fila["IdBeneficiario"].ToString()+separador);
							linea.Append ( fila["ApellidoNombre"].ToString()+separador);
							linea.Append ( DateTime.Parse(fila["FecNov"].ToString()).ToString("dd/MM/yyyy")+separador);
							linea.Append ( fila["HoraNov"].ToString()+separador);
							linea.Append ( fila["TipoConcepto"].ToString()+separador);
							linea.Append ( fila["DescTipoConcepto"].ToString()+separador);
							linea.Append ( fila["CodConceptoLiq"].ToString()+separador);
							linea.Append ( fila["DescConceptoLiq"].ToString()+separador);
							linea.Append ( fila["ImporteTotal"].ToString().Replace(",",".")+separador);
							linea.Append ( fila["CantCuotas"].ToString()+separador);
							linea.Append ( fila["Porcentaje"].ToString().Replace(",",".")+separador);
							linea.Append ( fila["NroComprobante"].ToString()+separador);
							linea.Append ( fila["MAC"].ToString()+separador);
							linea.Append ( fila["ImporteCuota"].ToString().Replace(",",".")+separador);
							linea.Append ( fila["IdEstadoReg"].ToString()+separador);
							linea.Append ( fila["DescripcionEstadoReg"].ToString()+separador);
							linea.Append ( fila["ImporteLiq"].ToString().Replace(",",".")+separador);
							linea.Append ( fila["IdEstadoNov"].ToString()+separador);
							linea.Append ( fila["Documento"].ToString()+separador);
							linea.Append ( fila["Stock"].ToString()+separador);
							linea.Append ( DateTime.Parse(fila["FechaEliminacion"].ToString()).ToString("dd/MM/yyyy")+separador);
							linea.Append ( fila["CodMovimiento"].ToString()+separador);
							linea.Append ( fila["FechaOrder"].ToString());
								
							sw.WriteLine(linea.ToString());
							
						}
						sw.Close();
												
						Utilidades.ComprimirArchivo(rutaArchivo, nombreArchivo);
						Utilidades.BorrarArchivo(rutaArchivo,nombreArchivo);

						nombreArchivo=nombreArchivo+".zip";
						rutaArchivoSal = rutaArchivo+nombreArchivo;
										
						ds = new DataSet();						
						ds.Tables.Add(new DataTable());
						
						string fGeneracion = DateTime.Now.ToString("yyyyMMdd hh:mm:ss:fff");
						rta = new ConsultasBatch().AltaNuevaConsulta(IDPrestador,nombreConsulta,0,OpcionBusqueda,MesAplica,TipoConc,ConcOpp,BenefCuil,string.Empty,string.Empty,rutaArchivo,nombreArchivo,fGeneracion,true);

						if (rta != string.Empty)
						{
							rta = "MSG_ERROR"+rta+"FIN_MSG_ERROR";
							throw new ApplicationException(rta);
						}
					}
				}
					
				return ds;
			}
			catch (SqlException errsql)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), errsql.Source, errsql.Message));  
                if (errsql.Number == -2)
				{
					nombreArchivo = Utilidades.GeneraNombreArchivo(nombreConsulta,IDPrestador,out rutaArchivo);
					rta = new ConsultasBatch().AltaNuevaConsulta(IDPrestador,nombreConsulta,0,OpcionBusqueda,MesAplica,TipoConc,ConcOpp,BenefCuil,string.Empty,string.Empty,rutaArchivo,nombreArchivo,string.Empty,false);
					
					throw new ApplicationException("MSG_ERROR Generando el archivo. Reingrese a la consulta en unos minutos.FIN_MSG_ERROR");
				}
				else
				{
					throw errsql;	
				}
			}
			catch( ApplicationException apperr)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), apperr.Source, apperr.Message));  
                throw new ApplicationException(apperr.Message) ;						
			}
			catch( Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw err ;						
			}
			finally
			{}	
		}
		#endregion

		#region Trae cuota social por cuil
        		
		public DataSet CuotaSocial_TraeXCuil(long idbeneficiario, long idPrestador)
		{
			DataSet ds = new DataSet();
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand("CuotaSocial_TraeXCuil");
	
			try
			{
                db.AddInParameter(dbCommand, "@idbeneficiario", DbType.Int64, idbeneficiario);
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);

                ds = db.ExecuteDataSet(dbCommand);
				
				return ds;
            } 
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw new Exception("Error en servicio CuotaSocial_TraeXCuil - ERROR: " + err.Message + " - SRC: " + err.Source);
			}
			finally
			{
                dbCommand.Dispose();
                db = null;
			}
		}
		#endregion
    }
	
	#endregion
}

