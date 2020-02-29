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

namespace ANSES.Microinformatica.DATComPlus
{
	[Guid("8DB1A006-DAEC-42c6-BB21-0A99B3D76A4B")] 
	public interface DAT_INovedad_Trans
	{
		string Novedades_Alta(long IdPrestador, long IdBeneficiario, byte TipoConcepto, 
			int ConceptoOPP,  	double ImpTotal, byte CantCuotas,Single Porcentaje,	 
			string  NroComprobante, string IP, string Usuario,int Mensual);
	
	
		string Novedades_Baja(long IdNovedadAnt, string IP, string Usuario,int Mensual);


		string Novedades_Modificacion(long IdNovedadAnt, double ImpTotalN, Single PorcentajeN, string  NroComprobanteN, 
			string IPN, string UsuarioN,int Mensual, Boolean Masiva);

		string Novedades_Baja_Cuotas(DataSet CuotasABajar, string Ip, string Usuario );

		DataSet Modificacion_Masiva_Indeterminadas(DataSet NovAMod,double Monto, string Ip, string Usuario,bool Masiva);
		
		Boolean CtrolOcurrenciasCancCuotas (byte CantOcurrDisp,long IdBeneficiario, int ConceptoOPP, long IdNovedadABorrar);

		string CtrolAlcanza( long IdBeneficiario, double Importe, long IdPrestador, int ConceptoOPP );

		string Valido_Nov_T3(long IdPrestador, long IdBeneficiario, 
			byte TipoConcepto, int ConceptoOPP, 
			double ImpTotal, byte CantCuotas,
			Single Porcentaje, byte CodMovimiento, String NroComprobante,
			DateTime FecNovedad, string IP, string Usuario,int Mensual,
			decimal montoPrestamo,decimal CuotaTotalMensual,
			decimal TNA,decimal TEM,
			decimal gastoOtorgamiento,decimal gastoAdmMensual,
			decimal cuotaSocial,decimal CFTEA,
			decimal CFTNAReal,decimal CFTEAReal,
			decimal gastoAdmMensualReal,decimal TIRReal);

		string Valido_Nov_T3_Gestion(long IdPrestador, long IdBeneficiario, 
			byte TipoConcepto, int ConceptoOPP, 
			double ImpTotal, byte CantCuotas,
			Single Porcentaje, byte CodMovimiento, String NroComprobante,
			DateTime FecNovedad, string IP, string Usuario,int Mensual,
			decimal montoPrestamo,decimal CuotaTotalMensual,
			decimal TNA,decimal TEM,
			decimal gastoOtorgamiento,decimal gastoAdmMensual,
			decimal cuotaSocial,decimal CFTEA,
			decimal CFTNAReal,decimal CFTEAReal,
			decimal gastoAdmMensualReal,decimal TIRReal, bool bGestionErrores);

		string Novedades_T3_Alta_ConTasa(long IdPrestador, long IdBeneficiario, DateTime FecNovedad,  byte TipoConcepto, int ConceptoOPP, 
			double ImpTotal, byte CantCuotas,  string NroComprobante, string IP, string Usuario, int Mensual, byte IdEstadoReg,
			decimal montoPrestamo,decimal CuotaTotalMensual,decimal TNA,decimal TEM,
			decimal gastoOtorgamiento,decimal gastoAdmMensual,decimal cuotaSocial,decimal CFTEA,
			decimal CFTNAReal,decimal CFTEAReal,decimal gastoAdmMensualReal,decimal TIRReal, string XMLCuotas, 
			int idItem, string nroFactura, string cbu, string nroTarjeta, string otro, string prestadorServicio, string poliza,
			string nroSocio, string nroTicket, int idDomicilioBeneficiario,int idDomicilioPrestador,string nroSucursal, DateTime fVto, DateTime fVtoHabilSiguiente, byte idTipoDocPresentado, DateTime fEstimadaEntrega, bool solicitaTarjetaNominada);

		void Novedades_Rechazadas_A_ConTasas(
			long IdBeneficiario,long IdPrestador ,byte CodMovimiento ,byte TipoConcepto ,int CodConceptoLiq ,double ImporteTotal ,
			byte CantCuotas ,Single Porcentaje ,string NroComprobante ,string IP,string Usuario ,DateTime FecRechazo ,
			decimal montoPrestamo,decimal CuotaTotalMensual,decimal TNA,decimal TEM,decimal gastoOtorgamiento,decimal gastoAdmMensual,
			decimal cuotaSocial,decimal CFTEA,decimal CFTNAReal,decimal CFTEAReal,decimal gastoAdmMensualReal,decimal TIRReal, string mensajeError);
	}


	[Guid("60794880-A41B-4887-8891-EF6DFC5B7724")] 
	[ ProgId( "DAT_Novedad_Trans" ) ]
	[ClassInterface(ClassInterfaceType.AutoDual)]
	[ ObjectPooling( MinPoolSize = 10) ]
	[JustInTimeActivation(true)]
	//[ MustRunInClientContext( true ) ]	
	[Transaction(TransactionOption.Required)]
	[ComponentAccessControl(true)]
	[EventTrackingEnabled ( true )]
	[SecureMethod]
	public class Novedad_Trans:ServicedComponent,DAT_INovedad_Trans
	{
		 
		public Novedad_Trans()
		{
		}	

		#region ABM Novedad
	
		#region Novedades_Alta

		[SecurityRole("OperadorEntidad")]
		[AutoComplete]
		public string Novedades_Alta(long IdPrestador, long IdBeneficiario, byte TipoConcepto, 
			int ConceptoOPP,  	double ImpTotal, byte CantCuotas,Single Porcentaje,	 
			string  NroComprobante, string IP, string Usuario,int Mensual)
		{
			string mensaje;

			byte IdEstadoReg ;
			DateTime FecNovedad;
			string retorno;
			string resp;
			Boolean EsAfiliacion = false;

											
			try
			{	
				EsAfiliacion = true;
				FecNovedad = DateTime.Now; //.Today;
				IdEstadoReg = 1;
				resp = Valido_Nov(IdPrestador,IdBeneficiario,TipoConcepto,ConceptoOPP,ImpTotal,CantCuotas,Porcentaje,6,NroComprobante);  			
				mensaje = resp.Split(char.Parse("|"))[0].ToString();
					
				if (mensaje!=String.Empty)
				{
					retorno = string.Concat( mensaje , "|0|" ) ;										
				}
				else
				{
					EsAfiliacion =  Boolean.Parse(resp.Split(char.Parse("|"))[1].ToString());

					switch (TipoConcepto)
					{
						case 1:
						case 6:
							if (EsAfiliacion == true)
								retorno = Novedades_T1o6_Alta_Afiliacion(IdPrestador, IdBeneficiario, FecNovedad,  TipoConcepto, ConceptoOPP, 
									ImpTotal, Porcentaje, NroComprobante, IP, Usuario, Mensual, IdEstadoReg);
							else
								retorno = Novedades_T1o6_Alta_No_Afiliacion(IdPrestador, IdBeneficiario, FecNovedad,  TipoConcepto, ConceptoOPP, 
									ImpTotal, Porcentaje, NroComprobante, IP, Usuario, Mensual, IdEstadoReg);
							break;
						case 2: //ok
							retorno = Novedades_T2_Alta (IdPrestador, IdBeneficiario, FecNovedad,  TipoConcepto, ConceptoOPP, 
								ImpTotal,NroComprobante, IP, Usuario,Mensual,IdEstadoReg);
							break;
						case 3:					
							retorno = Novedades_T3_Alta(IdPrestador, IdBeneficiario, FecNovedad,  TipoConcepto, ConceptoOPP, 
								ImpTotal, CantCuotas,  NroComprobante, IP, Usuario, Mensual,IdEstadoReg);
							break;						
						default:
							retorno = "Operación inválida|0|";										
							break;
					}								
				}
				
				//09/01/12 - $3b@
				string mensajeError = retorno.Split(char.Parse("|"))[0].ToString().Trim();
				if(mensajeError != string.Empty)
				{
					NovedadRechazada_Alta(IdPrestador, IdBeneficiario, FecNovedad,  TipoConcepto, ConceptoOPP, 
						string.Empty, ImpTotal,CantCuotas, Porcentaje, 6, NroComprobante, IP, Usuario, Mensual,mensajeError);	
				}

				return retorno;
			}					
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
		
			}
		}
		#endregion

		#region Novedades_Modificacion

		[SecurityRole("OperadorEntidad")]
		[AutoComplete]
		public string Novedades_Modificacion(long IdNovedadAnt, double ImpTotalN, Single PorcentajeN, string  NroComprobanteN, 
			string IPN, string UsuarioN,int Mensual, Boolean Masiva)		
		{
			Novedad nov = new Novedad();
			DataSet onov_v = new DataSet();	

			try
			{
				string mensaje =String.Empty;
				string retorno ;

				long IdPrestador = 0;
				long IdBeneficiario = 0;
				byte TipoConcepto = 0;
				double ImpTotalV = 0;
				Single PorcentajeV =0;
				byte IdEstadoRegV = 0;
				byte CodMovimientoV =0;
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
					IdPrestador	=	long.Parse(onov_v.Tables[0].Rows[0]["IdPrestador"].ToString());
					IdBeneficiario =	long.Parse(onov_v.Tables[0].Rows[0]["IdBeneficiario"].ToString());
					TipoConcepto	=	byte.Parse(onov_v.Tables[0].Rows[0]["TipoConcepto"].ToString());
					ImpTotalV	=	double.Parse(onov_v.Tables[0].Rows[0]["ImporteTotal"].ToString());
					PorcentajeV	=	Single.Parse(onov_v.Tables[0].Rows[0]["Porcentaje"].ToString());
					IdEstadoRegV	=	byte.Parse(onov_v.Tables[0].Rows[0]["IdEstadoReg"].ToString());
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
						resp = Valido_Nov(IdPrestador,IdBeneficiario,TipoConcepto,ConceptoOPP,ImpTotalN,1,PorcentajeN,5,NroComprobanteN);
						mensaje = resp.Split(char.Parse("|"))[0].ToString();
					}
				}
				if (mensaje == String.Empty)
				{
					switch (TipoConcepto)
					{
						case 1:
						case 6:
							retorno = Novedades_T1o6_Modificacion(IdNovedadAnt,IdPrestador,IdBeneficiario, 
								TipoConcepto, ConceptoOPP, ImpTotalN,PorcentajeN, NroComprobanteN, IPN, UsuarioN, 
								Mensual, IdEstadoRegV, ImpTotalV,PorcentajeV,CodMovimientoV, Masiva, EsAfiliacion);
							break;
						case 2:
							retorno = Novedades_T2_Modificacion(IdNovedadAnt,IdPrestador,IdBeneficiario, 
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
				if(mensajeError != string.Empty)
				{
					NovedadRechazada_Alta(IdPrestador, IdBeneficiario, DateTime.Now,  TipoConcepto, ConceptoOPP, 
						string.Empty, ImpTotalN,0, PorcentajeN, 5, NroComprobanteN, IPN, UsuarioN, Mensual,mensajeError);	
				}

				return retorno;
			}					
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				nov.Dispose();
				onov_v.Dispose();
		
			}
		}
		#endregion

		#region Novedades_Baja

		[SecurityRole("OperadorEntidad")]
		[AutoComplete]
		public string Novedades_Baja(long IdNovedadAnt, string IP, string Usuario,int Mensual)
		{		
			string retorno;				
			DataSet onov_v = new DataSet();
			byte TipoConcepto;
			byte idEstadoReg;
			
			Novedad nov = new Novedad();
			try
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

				return retorno;
			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				nov.Dispose();
				onov_v.Dispose();
			}
		}
		#endregion

		#region Novedades_Baja_Cuotas

		[SecurityRole("OperadorEntidad")]
		[AutoComplete]
		public string Novedades_Baja_Cuotas(DataSet CuotasABajar, string Ip, string Usuario )
		{
			string mensaje = string.Empty;
		
			try
			{
				if(CuotasABajar.Tables[0].Rows.Count > 0)
				{
					long IdNovedadAnt =0;
					int Mensual=0;
					foreach (DataRow CuotaInd in CuotasABajar.Tables[0].Rows)
					{
						IdNovedadAnt = long.Parse(CuotaInd["IdNovedad"].ToString());
						Mensual = int.Parse(CuotaInd["Mensual"].ToString());
						mensaje = Novedades_Baja(IdNovedadAnt,Ip,Usuario,Mensual);
						if (! mensaje.StartsWith("|"))
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
			
				return mensaje;
			}
			catch(Exception err)
			{
				throw err ;			
			}				
		}

		#endregion

		#region Modificacion_Masiva_Indeterminadas
		[AutoComplete]
		public DataSet Modificacion_Masiva_Indeterminadas(DataSet NovAMod,double Monto, string Ip, string Usuario,bool Masiva)
		{
			string mensaje = string.Empty;
			DataSet dsSalida = new DataSet();
			try
			{
				dsSalida = NovAMod.Clone();


				if(NovAMod.Tables[0].Rows.Count > 0)
				{
					long IdNovedadAnt =0;
					int Mensual=0;
					byte TipoConc = 0;
					double ImpTotN = 0;
					Single PorcentajeN =0;
					string Comprobante = String.Empty;
					string Mensaje = String.Empty;
					string mens = string.Empty;
					
					foreach (DataRow NovInd in NovAMod.Tables[0].Rows)
					{
						DataRow drNFila = dsSalida.Tables[0].NewRow();
        
						TipoConc = byte.Parse(NovInd["TipoConcepto"].ToString());
						switch(TipoConc)
						{
							case 1:
								ImpTotN=double.Parse(NovInd["ImporteTotal"].ToString())+Monto;
								PorcentajeN = 0;							
								break;
							case 6:
								PorcentajeN=Single.Parse(NovInd["Porcentaje"].ToString())+Single.Parse(Monto.ToString());
								ImpTotN = 0;							
								break;
							default:
								mens="Tipo de Concepto Erróneo para Modidicación Masiva|0|";
								break;
						}
						
						IdNovedadAnt = long.Parse(NovInd["IdNovedad"].ToString());
						Mensual = int.Parse(NovInd["Mensual"].ToString());
						Comprobante = NovInd["Comprobante"].ToString();

						if (mens == string.Empty)
						{	
							mens = Novedades_Modificacion(IdNovedadAnt,ImpTotN,PorcentajeN,Comprobante,Ip,Usuario,Mensual,Masiva);
						}
						
						if (mens.StartsWith("|")) // no hubo mensaje de error al realizar la modificacion
						{

							drNFila["Mensaje"] =String.Empty;
							drNFila["IdNovedad"] = int.Parse(mens.Split(char.Parse("|"))[1].ToString());
							drNFila["MAC"] = mens.Split(char.Parse("|"))[2].ToString();
							drNFila["FecNov"]= DateTime.Today;
							drNFila["ImporteTotal"]= ImpTotN;
							drNFila["Porcentaje"]= PorcentajeN;
						}
						else
						{
							// no se produjo la modificación por algun motivo
							drNFila["Mensaje"] = mens.Split(char.Parse("|"))[0].ToString();
							drNFila["IdNovedad"] = IdNovedadAnt;
							drNFila["MAC"] =NovInd["MAC"].ToString();
							drNFila["FecNov"]=DateTime.Parse(NovInd["FecNov"].ToString());
							drNFila["ImporteTotal"]= double.Parse(NovInd["ImporteTotal"].ToString());
							drNFila["Porcentaje"]= Single.Parse(NovInd["Porcentaje"].ToString());
						}
							
						drNFila["Mensual"] = Mensual;
						drNFila["Comprobante"]= Comprobante;
						drNFila["IdBeneficiario"]= long.Parse(NovInd["IdBeneficiario"].ToString());
						drNFila["ApellidoNombre"]= NovInd["ApellidoNombre"].ToString();
						drNFila["Cuil"]= NovInd["Cuil"].ToString();
						drNFila["TipoDoc"]= byte.Parse(NovInd["TipoDoc"].ToString());
						drNFila["NroDoc"]= long.Parse(NovInd["NroDoc"].ToString());
						drNFila["IdPrestador"]= long.Parse(NovInd["IdPrestador"].ToString());
						drNFila["CodConceptoLiq"]= byte.Parse(NovInd["CodConceptoLiq"].ToString());
						drNFila["DescConceptoLiq"]= NovInd["DescConceptoLiq"].ToString();
						drNFila["TipoConcepto"]= byte.Parse(NovInd["TipoConcepto"].ToString());
                        					  
						//Agrego la fila a la tabla
						dsSalida.Tables[0].Rows.Add( drNFila );
					}
				
				}
			
				return dsSalida;
			}
			catch(Exception err)
			{
				throw err ;			
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
				ds = Novedades_Trae_TCMov (IdPrestador, IdBeneficiario,ConceptoOPP);

				if (ds.Tables[0].Rows.Count != 0 && (ds.Tables[0].Rows[0]["CodMovimiento"].ToString() == "5" || ds.Tables[0].Rows[0]["CodMovimiento"].ToString() == "6"))
				{
					mensaje ="Solo se puede ingresar una novedad para el concepto ingresado";
				}
				
				Importe= Calc_Importe_1o6(IdBeneficiario,TipoConcepto,Porcentaje,ImpTotal);
				
				if (mensaje ==String.Empty)			
				{
					if (TipoConcepto == 6)
					{
						mensaje = CtrolTopeXCpto(IdPrestador, TipoConcepto, ConceptoOPP,Porcentaje);
						ImpTotal = 0;
					}
					else
					{
						mensaje = CtrolTopeXCpto(IdPrestador, TipoConcepto, ConceptoOPP,Importe);
						Porcentaje = 0;
					}
				}
		
				if (mensaje==String.Empty)
				{					
					mensaje = CtrolAlcanza(IdBeneficiario, Importe, IdPrestador, ConceptoOPP);
					
					//09/01/12 - $3b@ SE COMENTA LO SIG PORQUE SE GRABAN TODOS LOS ERRORES
//					if (mensaje!=String.Empty)
//					{
//						NovedadRechazada_Alta(IdPrestador, IdBeneficiario, FecNovedad,  TipoConcepto, ConceptoOPP, 
//							MAC, ImpTotal,CantCuotas, Porcentaje, CodMovimiento, NroComprobante, IP, Usuario, Mensual);
//					}
				}

				if (mensaje==String.Empty && ds.Tables[0].Rows.Count != 0 ) 
				{
					if (ds.Tables[0].Rows[0]["CodMovimiento"].ToString() == "4")// esta en novedades
					{ 
						IdNovedad= long.Parse(ds.Tables[0].Rows[0]["IdNovedad"].ToString());

						switch(byte.Parse(ds.Tables[0].Rows[0]["IdEstadoReg"].ToString()) )
						{
							case 1: 
								CodMovimiento = 5;
								Novedades_PasaAHist(IdNovedad,0,7,3,0,IP,Usuario);
								break;
							case 2:
							case 3:
								Novedades_Modifica_EstadoReg(IdNovedad,12,IP,Usuario);
								break;
							case 4:
								Novedades_PasaAHist(IdNovedad,0,7,3,0,IP,Usuario);
								break;
						}
					}

					//Actualizo el total de novedades cargadas
				}
				if (mensaje==String.Empty)
				{
					ModificaSaldo(IdPrestador,IdBeneficiario,ConceptoOPP,Importe,Usuario);
											
					//Doy alta la nueva novedad
					
					alta = Novedades_Alta_Fisica(IdPrestador, IdBeneficiario, FecNovedad,  TipoConcepto, ConceptoOPP, 
						ImpTotal, CantCuotas, Porcentaje, CodMovimiento, NroComprobante, IP, Usuario, IdEstadoReg);
										
					retorno = " |"+alta[0].ToString()+"|"+alta[1].ToString();
				}
				else
				{
					retorno = mensaje+"|0|";
				}							
				return (retorno);
			}
			catch(Exception err)
			{
				throw err ;			
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
				
				Importe= Calc_Importe_1o6(IdBeneficiario,TipoConcepto,Porcentaje,ImpTotal);
				
				if (mensaje ==String.Empty)			
				{
					if (TipoConcepto == 6)
					{
						//				mensaje = CtrolTopeXCpto(IdPrestador, TipoConcepto, ConceptoOPP,Porcentaje);
						ImpTotal = 0;
					}
					else
					{
						//				mensaje = CtrolTopeXCpto(IdPrestador, TipoConcepto, ConceptoOPP,Importe);
						Porcentaje = 0;
					}
				}
		
				if (mensaje==String.Empty)
				{					
					mensaje = CtrolAlcanza(IdBeneficiario, Importe, IdPrestador, ConceptoOPP);

					//09/01/12 - $3b@ SE COMENTA LO SIG PORQUE SE GRABAN TODOS LOS ERRORES
//					if (mensaje!=String.Empty)
//					{
//						NovedadRechazada_Alta(IdPrestador, IdBeneficiario, FecNovedad,  TipoConcepto, ConceptoOPP, 
//							MAC, ImpTotal,CantCuotas, Porcentaje, CodMovimiento, NroComprobante, IP, Usuario, Mensual);
//					}
				}
				
				if (mensaje==String.Empty)
				{
					//Actualizo el total de novedades cargadas
					ModificaSaldo(IdPrestador,IdBeneficiario,ConceptoOPP,Importe,Usuario);
											
					//Doy alta la nueva novedad
					
					alta = Novedades_Alta_Fisica(IdPrestador, IdBeneficiario, FecNovedad,  TipoConcepto, ConceptoOPP, 
						ImpTotal, CantCuotas, Porcentaje, CodMovimiento, NroComprobante, IP, Usuario, IdEstadoReg);
										
					retorno = " |"+alta[0].ToString()+"|"+alta[1].ToString();
				}
				else
				{
					retorno = mensaje+"|0|";
				}							
				return (retorno);
			}
			catch(Exception err)
			{
				throw err ;			
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
				string retorno = String.Empty;
				string mensaje =String.Empty;
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
					double Importe = Calc_Importe_1o6(IdBeneficiario,TipoConcepto,Porcentaje,ImpTotal);
									
					// Preparo el registro de baja segun corresponda.
					byte CodMovimiento = 4;
					
					Importe = Importe * -1;

					ModificaSaldo(IdPrestador,IdBeneficiario,ConceptoOPP,Importe,Usuario);

					if (CodMovimientoV == 6 && EstRegistroV == 1)
					{
						Novedades_PasaAHist(IdNovedadV,0,8,3,0,IP,Usuario);
						alta[0] = "0";
						alta[1] = string.Empty;
					}
					else
					{
						//Alguna vez fue a la liquidación
						switch ( CodMovimientoV)
						{
								//El archivo anterior fue modificado o es alta
							case 5:
							case 6:
							switch (EstRegistroV)
							{
								case 1:
								case 4:
								case 13:
									Novedades_PasaAHist(IdNovedadV,0,8,3,0,IP,Usuario);
									break;
								case 2:
								case 3:
								case 14:
								case 15:
									Novedades_Modifica_EstadoReg(IdNovedadV,12,IP,Usuario);
									break;
							}
								//Para estas novedades se debe ingresar un nuevo registro para informar la baja a la 
								//liquidacion
								
								DateTime FecNovedad = DateTime.Today;										
								alta = Novedades_Alta_Fisica(IdPrestador,IdBeneficiario,FecNovedad,TipoConcepto, ConceptoOPP,ImpTotal,0,Porcentaje,CodMovimiento,NroComprobante,IP,Usuario,1);
								break;									
						}
					}
				}
				if (mensaje == String.Empty)
				{
					retorno = " |"+alta[0].ToString()+"|"+alta[1].ToString();
				}
				else
				{
					retorno = mensaje+"|0|";
				}
				return retorno;
			}
			
			catch(Exception err)
			{
				throw err ;
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
				string retorno = String.Empty;
				string mensaje = String.Empty;
				byte CodMovimientoN ;
				byte idEstadoRegN = 1;
				double	ImporteN =0;
				double	ImporteV =0;
				DateTime FecNovedad = DateTime.Today;
				double  val = 0;
				byte CantCuotas = 0;
				String[] alta = new String[2];
				CodMovimientoN = 5;

				if (CodMovimientoV == 4)
				{
					mensaje = "Novedad inexistente";
				}
				
			
				if (mensaje ==String.Empty)			
				{
					if (EsAfiliacion == true)
					{								
						if (TipoConcepto == 6)
							mensaje = CtrolTopeXCpto(IdPrestador, TipoConcepto, ConceptoOPP,PorcentajeN);
						else
							mensaje = CtrolTopeXCpto(IdPrestador, TipoConcepto, ConceptoOPP,ImpTotalN);
					}
					/*else
					{
						CodMovimientoN = 6;
					}*/					
				}
				
			
				if (mensaje ==String.Empty)			
				{
					ImporteN = Calc_Importe_1o6(IdBeneficiario,TipoConcepto,PorcentajeN,ImpTotalN);
					ImporteV = Calc_Importe_1o6(IdBeneficiario,TipoConcepto,PorcentajeV,ImpTotalV);
					FecNovedad = DateTime.Today;
					val = ImporteN -ImporteV;
					CantCuotas = 0;
					
					if (val > 0 && Masiva == false)
					{
                        
						mensaje = CtrolAlcanza(IdBeneficiario, val, IdPrestador, ConceptoOPP);
						
						//09/01/12 - $3b@ SE COMENTA LO SIG PORQUE SE GRABAN TODOS LOS ERRORES				
//						if (mensaje!=String.Empty)
//						{
//							NovedadRechazada_Alta(IdPrestador, IdBeneficiario, FecNovedad,  TipoConcepto, ConceptoOPP, 
//								String.Empty, ImpTotalN,CantCuotas, PorcentajeN, CodMovimientoN, NroComprobanteN, IPN, UsuarioN, Mensual);
//						}
					}
				}

				
				if (mensaje == String.Empty)
				{
					ModificaSaldo(IdPrestador,IdBeneficiario,ConceptoOPP,val,UsuarioN);

					switch (IdEstadoRegV)
					{
						case 2:
						case 3:
						case 14:
						case 15:
							Novedades_Modifica_EstadoReg(IdNovedadAnt,12,IPN,UsuarioN);
							break;
						case 1:
							if (CodMovimientoV == 6)
							{
								CodMovimientoN=6;
							}
							Novedades_PasaAHist(IdNovedadAnt,Mensual,7,3,0,IPN,UsuarioN);
							break;
						case 4:
						case 13:
							Novedades_PasaAHist(IdNovedadAnt,Mensual,7,3,0,IPN,UsuarioN);
							break;								 
					}
					
					alta = Novedades_Alta_Fisica(IdPrestador,IdBeneficiario,FecNovedad,TipoConcepto,ConceptoOPP,
						ImpTotalN,CantCuotas,PorcentajeN,CodMovimientoN,NroComprobanteN,IPN,UsuarioN,idEstadoRegN);

													
					retorno = " |"+alta[0].ToString()+"|"+alta[1].ToString();
				}
				else
				{
					retorno = mensaje+"|0|";
				}			

				return retorno;
			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
			 
			}
		}
		#endregion

		#region Novedades_Trae_TCMov
		private DataSet Novedades_Trae_TCMov(long IdPrestador, long IdBeneficiario,int ConceptoOPP)
		{
			Conexion objCnn = new Conexion();	
			
			SqlConnection objCon = new SqlConnection();

			try
			{
				objCon = objCnn.Conectar();
				SqlParameter[] objPar = new SqlParameter[3];
		
				objPar[0] = new SqlParameter("@IdPrestador",SqlDbType.BigInt); 
				objPar[0].Value = IdPrestador;

				objPar[1] = new SqlParameter("@ConceptoLiq",SqlDbType.Int); 
				objPar[1].Value = ConceptoOPP;

				objPar[2] = new SqlParameter("@IdBeneficiario",SqlDbType.BigInt); 
				objPar[2].Value = IdBeneficiario;
		

				return (SqlHelper.ExecuteDataset(objCon, CommandType.StoredProcedure, "Novedades_Trae_TCMov", objPar));
			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCon.Dispose();
				objCnn=null;
			}
		}	
		#endregion
		
		#region CtrolTopeXCpto
		private string CtrolTopeXCpto(long IdPrestador, byte TipoConcepto, int ConceptoOPP,double Importe)
									
		{
			Conexion objCnn = new Conexion();	
			
			string mensaje = String.Empty;
			try
			{
				SqlParameter [] objPar = new SqlParameter[5];
		

				objPar[0] = new SqlParameter("@IdPrestador",SqlDbType.BigInt); 
				objPar[0].Value = IdPrestador;

				objPar[1] = new SqlParameter("@TipoConcepto",SqlDbType.TinyInt); 
				objPar[1].Value = TipoConcepto;

				objPar[2] = new SqlParameter("@CodConceptoLiq",SqlDbType.Int); 
				objPar[2].Value = ConceptoOPP;

				objPar[3] = new SqlParameter("@Importe",SqlDbType.Decimal); 
				objPar[3].Value = Importe;
		
				objPar[4] = new SqlParameter("@Alcanza",SqlDbType.TinyInt);
				objPar[4].Direction = ParameterDirection.Output;
				objPar[4].Value = 0;

				SqlHelper.ExecuteNonQuery(objCnn.Conectar(), CommandType.StoredProcedure, "CtrolTopeXCpto", objPar );
							
				
				if ((Byte) objPar[4].Value == 0)
				{		
					mensaje = "Supera el Máximo permitido para el código de Liquidación " + ConceptoOPP.ToString();
				}
				
				return mensaje;

			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCnn=null;
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
				throw err ;
			}
			finally
			{
				ds.Dispose();
				benef.Dispose();	
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
				string mensaje =String.Empty;
				String[] alta = new String[2];
				byte CodMovimiento = 6;
				string retorno;
								

				mensaje = CtrolAlcanza(IdBeneficiario,ImpTotal, IdPrestador, ConceptoOPP);

				//09/01/12 - $3b@ SE COMENTA LO SIG PORQUE SE GRABAN TODOS LOS ERRORES
//				if (mensaje!=String.Empty)
//				{
//					NovedadRechazada_Alta(IdPrestador, IdBeneficiario, FecNovedad,  TipoConcepto, ConceptoOPP,
//						String.Empty, ImpTotal,0, 0, CodMovimiento, NroComprobante, IP, Usuario, Mensual);
//						
//				}

				if (mensaje == String.Empty)
				{
				
					ModificaSaldo(IdPrestador,IdBeneficiario,ConceptoOPP,ImpTotal,Usuario);
					
					alta = Novedades_Alta_Fisica(IdPrestador,IdBeneficiario,FecNovedad,TipoConcepto,ConceptoOPP,ImpTotal,1,0,CodMovimiento,NroComprobante,IP,Usuario,IdEstadoReg);	
					retorno = String.Format(" |{0}|{1}", alta[0].ToString(),alta[1].ToString() ) ;				
				}
				else
				{
					retorno = mensaje + "|0| ";				
				}
				return retorno;
			}
			catch(Exception err)
			{
				throw err ;				
			}
			finally
			{
				
			}
		}
	
		#endregion

		#region  Novedades_T2_Modificacion()
		private string Novedades_T2_Modificacion(long IdNovedadAnt,long IdPrestador, long IdBeneficiario, 
			byte TipoConcepto, int ConceptoOPP, double ImpTotalN, string  NroComprobanteN, string IPN, 
			string UsuarioN, byte IdEstadoRegV,	double ImpTotalV)
		{
				
			try
			{
				string mensaje =String.Empty;
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
				if (mensaje==String.Empty)
				{				
					// calculo el importe para ver si alcanza el disponible
					Importe = ImpTotalN - ImpTotalV;
				
					if (Importe > 0)
					{
						mensaje = CtrolAlcanza(IdBeneficiario,Importe, IdPrestador, ConceptoOPP);

						//09/01/12 - $3b@ SE COMENTA LO SIG PORQUE SE GRABAN TODOS LOS ERRORES
//						if (mensaje!=String.Empty)
//						{
//							NovedadRechazada_Alta(IdPrestador, IdBeneficiario, FecNovedad,  TipoConcepto, ConceptoOPP,
//								String.Empty, ImpTotalN,0, 0, codMovimiento, NroComprobanteN, IPN, UsuarioN, 0);
//						}
					}

				}				
				if (mensaje==String.Empty)
				{
					ModificaSaldo(IdPrestador,IdBeneficiario,ConceptoOPP,Importe,UsuarioN);
					
					Novedades_PasaAHist(IdNovedadAnt,0,7,3,0,IPN,UsuarioN);
					
					alta = Novedades_Alta_Fisica(IdPrestador,IdBeneficiario,FecNovedad,TipoConcepto,ConceptoOPP,ImpTotalN,1,0,codMovimiento,NroComprobanteN,IPN,UsuarioN,IdEstadoRegN);
					
					retorno = " |"+alta[0].ToString()+"|"+alta[1].ToString();				
				}
				else
				{
					retorno = mensaje + "|0| ";				
				}
				return retorno;

			}
			catch(Exception err)
			{
				throw err ;
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
				string mensaje = String.Empty;				
				long IdNovedadAnt;
				long idPrestador;
				long IdBeneficiario;
				byte TipoConcepto ;
				int ConceptoOPP ;
				double ImpTotal ;							

				if ( NovVieja.Tables[0].Rows[0]["IdEstadoReg"].ToString() != "1" )
				{
					// para novedades en proceso de liquidación o en transito a la misma
					mensaje = "Novedad en proceso de liquidación. No puede darse de baja";
				}
				else
				{
					IdNovedadAnt = long.Parse(NovVieja.Tables[0].Rows[0]["IdNovedad"].ToString());
					idPrestador = long.Parse(NovVieja.Tables[0].Rows[0]["IdPrestador"].ToString());
					IdBeneficiario = long.Parse(NovVieja.Tables[0].Rows[0]["IdBeneficiario"].ToString());
					TipoConcepto =  byte.Parse(NovVieja.Tables[0].Rows[0]["TipoConcepto"].ToString());
					ConceptoOPP = int.Parse(NovVieja.Tables[0].Rows[0]["CodConceptoLiq"].ToString());
					ImpTotal = -1 * (double.Parse(NovVieja.Tables[0].Rows[0]["ImporteTotal"].ToString())) ;							

					ModificaSaldo(idPrestador,IdBeneficiario,ConceptoOPP,ImpTotal,Usuario);
					
					Novedades_PasaAHist(IdNovedadAnt,0,9,3,0,IP,Usuario);      	

				}			
				return mensaje + "|0| ";				
			}
			catch(Exception err)
			{
				throw err ;
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
		[SecurityRole("OperadorEntidad")]
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

		[SecurityRole("OperadorEntidad")]
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
			
			
			string mensaje = String.Empty;
			mensaje = Valido_Nov(IdPrestador, IdBeneficiario, TipoConcepto, 
								 ConceptoOPP, ImpTotal, CantCuotas,Porcentaje, CodMovimiento, NroComprobante, bGestionErrores,montoPrestamo);
			mensaje = mensaje.Split(char.Parse("|"))[0].ToString().Trim();
					
			if (mensaje==String.Empty)
			{
				mensaje= CtrolAlcanza(IdBeneficiario,(double) CuotaTotalMensual, IdPrestador, ConceptoOPP);
//				if (mensaje!=String.Empty)
//				{
//					Novedades_Rechazadas_A_ConTasas(IdBeneficiario, IdPrestador, CodMovimiento, 
//													TipoConcepto, ConceptoOPP, ImpTotal, CantCuotas, 
//													Porcentaje, NroComprobante, IP, Usuario, FecNovedad,
//													montoPrestamo, CuotaTotalMensual, TNA, TEM,
//													gastoOtorgamiento, gastoAdmMensual,cuotaSocial, 
//													CFTEA,CFTNAReal,CFTEAReal, gastoAdmMensualReal,TIRReal);
//						
//				}		
			}

			//09/01/12 - $3b@
			if (mensaje!=String.Empty && bGestionErrores)
			{
				Novedades_Rechazadas_A_ConTasas(IdBeneficiario, IdPrestador, CodMovimiento, 
					TipoConcepto, ConceptoOPP, ImpTotal, CantCuotas, 
					Porcentaje, NroComprobante, IP, Usuario, FecNovedad,
					montoPrestamo, CuotaTotalMensual, TNA, TEM,
					gastoOtorgamiento, gastoAdmMensual,cuotaSocial, 
					CFTEA,CFTNAReal,CFTEAReal, gastoAdmMensualReal,TIRReal, mensaje);
						
			}
			
			return mensaje;
			
		}

		[SecurityRole("OperadorEntidad")]
		[AutoComplete]
		public string Novedades_T3_Alta_ConTasa(long IdPrestador, long IdBeneficiario, DateTime FecNovedad,  byte TipoConcepto, int ConceptoOPP, 
			double ImpTotal, byte CantCuotas,  string NroComprobante, string IP, string Usuario, int Mensual, byte IdEstadoReg,
			decimal montoPrestamo,decimal CuotaTotalMensual,decimal TNA,decimal TEM,
			decimal gastoOtorgamiento,decimal gastoAdmMensual,decimal cuotaSocial,decimal CFTEA,
			decimal CFTNAReal,decimal CFTEAReal,decimal gastoAdmMensualReal,decimal TIRReal, string XMLCuotas, 
			int idItem, string nroFactura, string cbu, string nroTarjeta, string otro, string prestadorServicio, string poliza,
			string nroSocio, string nroTicket, int idDomicilioBeneficiario,int idDomicilioPrestador, string nroSucursal, DateTime fVto,DateTime fVtoHabilSiguiente, byte idTipoDocPresentado, DateTime fEstimadaEntrega, bool solicitaTarjetaNominada)
		{
			
			
			try
			{
				String[] alta = new String[2];
				string mensaje = String.Empty;
				string retorno = String.Empty;
				byte CodMovimiento  = 6;
				string MAC = string.Empty;

				mensaje= Valido_Nov_T3(IdPrestador, IdBeneficiario,
									   TipoConcepto,ConceptoOPP,ImpTotal, 
									   CantCuotas, 0,CodMovimiento, NroComprobante, 
									   FecNovedad, IP, Usuario,Mensual,
										montoPrestamo,CuotaTotalMensual,TNA,TEM,
										gastoOtorgamiento,gastoAdmMensual,cuotaSocial,CFTEA,
										CFTNAReal,CFTEAReal,gastoAdmMensualReal,TIRReal);

				
				if (mensaje==String.Empty)
				{
					try
					{
						alta= Novedades_Alta_Fisica_Tipo3_ConTasa(IdBeneficiario, IdPrestador, ConceptoOPP, ImpTotal, montoPrestamo, 
							CantCuotas,CuotaTotalMensual,TNA, TEM, gastoOtorgamiento,
							gastoAdmMensual, cuotaSocial, CFTEA, CFTNAReal, 
							CFTEAReal, gastoAdmMensualReal,TIRReal, NroComprobante, IP, 
							Usuario,Mensual ,XMLCuotas, CodMovimiento, FecNovedad,TipoConcepto,IdEstadoReg, 
							idItem, nroFactura, cbu, nroTarjeta , otro, prestadorServicio, poliza,
							nroSocio, nroTicket, idDomicilioBeneficiario,idDomicilioPrestador, nroSucursal, fVto,fVtoHabilSiguiente, idTipoDocPresentado, fEstimadaEntrega, solicitaTarjetaNominada);
			
						retorno = " |"+alta[0].ToString()+"|"+alta[1].ToString();	
									
						ModificaSaldo(IdPrestador,IdBeneficiario,ConceptoOPP, (double) CuotaTotalMensual,Usuario);
					}
					catch (SqlException e)
					{
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
			catch(Exception err)
			{
				throw err ;
				
			}
			finally
			{
				
				
			}
		}
		
		#region Novedades_T3_Alta
		private string Novedades_T3_Alta(long IdPrestador, long IdBeneficiario, DateTime FecNovedad,  byte TipoConcepto, int ConceptoOPP, 
			double ImpTotal, byte CantCuotas,  string NroComprobante, string IP, string Usuario, int Mensual, byte IdEstadoReg)
		{
			
			
			try
			{
				String[] alta = new String[2];
				string mensaje = String.Empty;
				string retorno = String.Empty;
				double Importe = ImpTotal/CantCuotas;
				byte CodMovimiento  = 6;
				string MAC = string.Empty;
				//long IdNovedad = 0;
					
						
				mensaje= CtrolAlcanza(IdBeneficiario,Importe, IdPrestador, ConceptoOPP);
				
				if (mensaje==String.Empty)
				{
					ModificaSaldo(IdPrestador,IdBeneficiario,ConceptoOPP,Importe,Usuario);
					
					/*alta= Novedades_Alta_Fisica(IdPrestador, IdBeneficiario, FecNovedad, TipoConcepto,
						ConceptoOPP, ImpTotal, CantCuotas,0,CodMovimiento, NroComprobante, IP, Usuario,IdEstadoReg);

					IdNovedad = long.Parse(alta[0].ToString());

					GeneraCuotas (IdNovedad,CantCuotas, Importe, IP, Usuario,Mensual);				*/

					alta= Novedades_Alta_Fisica_Tipo3(IdPrestador, IdBeneficiario, FecNovedad, TipoConcepto,
						ConceptoOPP, ImpTotal, CantCuotas,0,CodMovimiento, NroComprobante, IP, Usuario,IdEstadoReg,Mensual);
					
					retorno = " |"+alta[0].ToString()+"|"+alta[1].ToString();				
				}
				else
				{
					retorno = mensaje + "|0| ";				
				}
				return retorno;
			}
			catch(Exception err)
			{
				throw err ;
				
			}
			finally
			{
				
				
			}
		}
		
		#region Alta_Fisica_Novedades_Tipo3 
		
		private String[] Novedades_Alta_Fisica_Tipo3(long IdPrestador, long IdBeneficiario, DateTime FecNovedad, 
			byte TipoConcepto, int ConceptoOPP, double ImpTotal, byte CantCuotas,Single Porcentaje,	 
			byte CodMovimiento, string  NroComprobante, string IP, string Usuario, byte IdEstadoReg, int Mensual)
		{		

			string dato = Genera_Datos_para_MAC(IdBeneficiario, IdPrestador,FecNovedad,CodMovimiento,ConceptoOPP,TipoConcepto,
				ImpTotal,CantCuotas,Porcentaje,NroComprobante,IP,Usuario);
					
			string MAC = Utilidades.Calculo_MAC(dato);							

			Conexion objCnn = new Conexion();	
			
			SqlConnection objCon = new SqlConnection();

			String[] retorno = new String[2];
			try
			{

				objCon = objCnn.Conectar();
				SqlParameter [] objPar = new SqlParameter[11];
			
				objPar[0] = new SqlParameter("@IdBeneficiario",SqlDbType.BigInt); 
				objPar[0].Value = IdBeneficiario;

				objPar[1] = new SqlParameter("@IdPrestador",SqlDbType.BigInt);
				objPar[1].Value = IdPrestador;
	
				objPar[2] = new SqlParameter("@CodConceptoLiq",SqlDbType.Int);
				objPar[2].Value = ConceptoOPP;
					
				objPar[3] = new SqlParameter("@ImporteTotal",SqlDbType.Decimal);
				objPar[3].Value = ImpTotal;
			
				objPar[4] = new SqlParameter("@CantCuotas",SqlDbType.TinyInt);
				objPar[4].Value = CantCuotas;					
	
				/* OJO en un futuro se va a exigir cargar el nro de comprobante */
				objPar[5] = new SqlParameter("@NroComprobante",SqlDbType.VarChar,50);
				objPar[5].Value = NroComprobante;

				objPar[6] = new SqlParameter("@MAC",SqlDbType.VarChar,100);
				objPar[6].Value = MAC;
				
				objPar[7]  = new SqlParameter("@IP",SqlDbType.VarChar,20);
				objPar[7].Value = IP;
							
				objPar[8]  = new SqlParameter("@Usuario",SqlDbType.VarChar,50);
				objPar[8].Value = Usuario;

			
				objPar[9]  = new SqlParameter("@PrimerMensual",SqlDbType.Int);
				objPar[9].Value = Mensual;

				objPar[10]  = new SqlParameter("@IdNovedad",SqlDbType.BigInt);
				objPar[10].Direction = ParameterDirection.Output;
				objPar[10].Value = 0;


				//SqlHelper.ExecuteNonQuery(objCon, CommandType.StoredProcedure, "Novedades_A", objPar);
				SqlHelper.ExecuteNonQuery(objCnn.ConectarString(), CommandType.StoredProcedure, "Novedades_Tipo3_Alta", objPar);
				
				retorno[0]= objPar[10].Value.ToString();
				retorno[1] = MAC;

				return retorno;
			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCon.Dispose();
				objCnn=null;
				
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
			DateTime fEstimadaEntrega,
			bool solicitaTarjetaNominada)
			{		

			string dato = Genera_Datos_para_MAC(IdBeneficiario, IdPrestador,FecNovedad,
												CodMovimiento,CodConceptoLiq,TipoConcepto,
												importeTotal,CantCuotas,0,NroComprobante,IP,Usuario);
					
			string MAC = Utilidades.Calculo_MAC(dato);							

			Conexion objCnn = new Conexion();	
			
			SqlConnection objCon = new SqlConnection();

			String[] retorno = new String[2];
			try
			{

				objCon = objCnn.Conectar();

				SqlParameter [] objPar = new SqlParameter[42];				
				
				objPar[0] = new SqlParameter("@idbeneficiario",SqlDbType.BigInt); 
				objPar[0].Value = IdBeneficiario;
				objPar[1] = new SqlParameter("@IdPrestador",SqlDbType.BigInt); 
				objPar[1].Value = IdPrestador ;
				objPar[2] = new SqlParameter("@CodConceptoLiq",SqlDbType.Int); 
				objPar[2].Value = CodConceptoLiq ;
				objPar[3] = new SqlParameter("@importeTotal",SqlDbType.Decimal); 
				objPar[3].Value = importeTotal;
				objPar[4] = new SqlParameter("@montoPrestamo",SqlDbType.Decimal); 
				objPar[4].Value = montoPrestamo;
				objPar[5] = new SqlParameter("@CantCuotas",SqlDbType.TinyInt); 
				objPar[5].Value = CantCuotas;
				objPar[6] = new SqlParameter("@CuotaTotalMensual",SqlDbType.Decimal); 
				objPar[6].Value = CuotaTotalMensual;
				objPar[7] = new SqlParameter("@TNA",SqlDbType.Decimal); 
				objPar[7].Value = TNA;
				objPar[8] = new SqlParameter("@TEM",SqlDbType.Decimal); 
				objPar[8].Value = TEM;
				objPar[9] = new SqlParameter("@gastoOtorgamiento",SqlDbType.Decimal); 
				objPar[9].Value = gastoOtorgamiento;
				objPar[10] = new SqlParameter("@gastoAdmMensual",SqlDbType.Decimal); 
				objPar[10].Value = gastoAdmMensual;
				objPar[11] = new SqlParameter("@cuotaSocial" ,SqlDbType.Decimal); 
				objPar[11].Value = cuotaSocial;
				objPar[12] = new SqlParameter("@CFTEA",SqlDbType.Decimal); 
				objPar[12].Value = CFTEA;
				objPar[13] = new SqlParameter("@CFTNAReal",SqlDbType.Decimal); 
				objPar[13].Value = CFTNAReal;
				objPar[14] = new SqlParameter("@CFTEAReal",SqlDbType.Decimal); 
				objPar[14].Value = CFTEAReal;
				objPar[15] = new SqlParameter("@gastoAdmMensualReal",SqlDbType.Decimal); 
				objPar[15].Value = gastoAdmMensualReal;
				objPar[16] = new SqlParameter("@TIRReal",SqlDbType.Decimal); 
				objPar[16].Value = TIRReal;
				objPar[17] = new SqlParameter("@NroComprobante",SqlDbType.VarChar,50); 
				objPar[17].Value = NroComprobante;
				objPar[18] = new SqlParameter("@MAC",SqlDbType.VarChar,50); 
				objPar[18].Value = MAC;
				objPar[19] = new SqlParameter("@IP",SqlDbType.VarChar,15); 
				objPar[19].Value = IP;
				objPar[20] = new SqlParameter("@Usuario",SqlDbType.VarChar,50); 
				objPar[20].Value = Usuario;
				objPar[21] = new SqlParameter("@PrimerMensual",SqlDbType.Int); 
				objPar[21].Value = PrimerMensual ;
				objPar[22] = new SqlParameter("@cuotas",SqlDbType.Text); 
				objPar[22].Value = cuotas;

				objPar[23] = new SqlParameter("@idestadoReg",SqlDbType.Int); 
				objPar[23].Value = IdEstadoReg;
				objPar[24] = new SqlParameter("@idItem",SqlDbType.Int); 
				objPar[24].Value = idItem;
				objPar[25] = new SqlParameter("@nroFactura",SqlDbType.VarChar,20); 
				objPar[25].Value = nroFactura;
				objPar[26] = new SqlParameter("@cbu",SqlDbType.VarChar,22); 
				objPar[26].Value = cbu;
				objPar[27] = new SqlParameter("@otro",SqlDbType.VarChar,300); 
				objPar[27].Value = otro;
				objPar[28] = new SqlParameter("@prestadorServicio",SqlDbType.VarChar,100); 
				objPar[28].Value = prestadorServicio;
				objPar[29] = new SqlParameter("@poliza",SqlDbType.VarChar,50); 
				objPar[29].Value = poliza;
				objPar[30] = new SqlParameter("@nroSocio",SqlDbType.VarChar,50); 
				objPar[30].Value = nroSocio;

				objPar[31] = new SqlParameter("@nroTarjeta",SqlDbType.VarChar,50); 
				if(nroTarjeta == null || nroTarjeta == string.Empty)
					objPar[31].Value =  DBNull.Value;
				else
					objPar[31].Value =  nroTarjeta;
			
				objPar[32] = new SqlParameter("@idDomicilioBeneficiario",SqlDbType.Int); 
				objPar[32].Value = idDomicilioBeneficiario;
				objPar[33] = new SqlParameter("@idDomicilioPrestador",SqlDbType.Int); 
				objPar[33].Value = idDomicilioPrestador;

				objPar[34] = new SqlParameter("@idSucursal",SqlDbType.VarChar,10); 
				if(nroSucursal == null || nroSucursal == string.Empty)
					objPar[34].Value = DBNull.Value;
				else
					objPar[34].Value = nroSucursal;

				objPar[35] = new SqlParameter("@fVto",SqlDbType.DateTime); 
				if(fVto == DateTime.MinValue)
                    objPar[35].Value = DBNull.Value;
				else
					objPar[35].Value = fVto;

				objPar[36] = new SqlParameter("@fVtoHabilSiguiente",SqlDbType.DateTime); 
				if(fVtoHabilSiguiente == DateTime.MinValue)
					objPar[36].Value = DBNull.Value;
				else
					objPar[36].Value = fVtoHabilSiguiente;
				
				objPar[37] = new SqlParameter("@nroTicket",SqlDbType.VarChar,500); 
				if(nroTicket== null || nroTicket== string.Empty)
					objPar[37].Value =  DBNull.Value;
				else
					objPar[37].Value =  nroTicket;

				objPar[38] = new SqlParameter("@idTipoDocPresentado",SqlDbType.TinyInt); 
				objPar[38].Value = idTipoDocPresentado;

				objPar[39] = new SqlParameter("@solicitaTarjetaNominada",SqlDbType.Bit); 
				objPar[39].Value = solicitaTarjetaNominada;

				objPar[40] = new SqlParameter("@fEstimadaEntrega",SqlDbType.DateTime); 
				if(fEstimadaEntrega == DateTime.MinValue)
					objPar[40].Value = DBNull.Value;
				else
					objPar[40].Value = fEstimadaEntrega;
			
				objPar[41]  = new SqlParameter("@IdNovedad",SqlDbType.BigInt);
				objPar[41].Direction = ParameterDirection.Output;
				objPar[41].Value = 0;

				SqlHelper.ExecuteNonQuery( objCon, CommandType.StoredProcedure, "Novedades_Tipo3_AltaConTasas", objPar);

				retorno[0]= objPar[41].Value.ToString();
				retorno[1] = MAC;		

				return retorno;
			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCon.Dispose();
				objCnn=null;
				
			}
			
			
		}
		#endregion

		#region GeneraCuotas
		private void GeneraCuotas (long idNovedad, byte CantCuotas, double Importe, string IP, string Usuario,int Mensual)
		{
			
			try
			{			
				int mes = int.Parse(Mensual.ToString().Substring(4,2));
								
				int ano = int.Parse(Mensual.ToString().Substring(0,4));
				
				for (byte i=1; i<= CantCuotas; i++)
				{
					Mensual = int.Parse(ano.ToString()+ (mes<10?"0"+mes.ToString():mes.ToString()));	
					if (mes==12)
					{
						ano++;
						mes = 1;
					}
					else
					{
						mes ++;
					}

					object[] datos = new object[4];
					
					datos[0]= idNovedad;
					datos[1]= i;
					datos[2]=Importe;
					datos[3]= Mensual;
		
					// No se generara Hash para las cuotas.
					//string dato = Utilidades.Armo_String_MAC(datos);
					//MAC = Utilidades.Calculo_MAC(dato);		
					
					AltaCuota(idNovedad, i, Importe, IP, Usuario, Mensual);			

				}
			
				return;
			}
	
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				
			}
		
		}
		#endregion

		#region AltaCuota
		private void AltaCuota( long IdNovedad, byte NroCuota, double Importe,  string IP, string Usuario, int Mensual)
		{
			
			Conexion objCnn = new Conexion();	
			
			SqlConnection objCon = new SqlConnection();

			try
			{

				objCon = objCnn.Conectar();
				SqlParameter [] objPar = new SqlParameter[6];
			
				objPar[0] = new SqlParameter("@IdNovedad",SqlDbType.BigInt); 
				objPar[0].Value = IdNovedad;

				objPar[1] = new SqlParameter("@NroCuota",SqlDbType.TinyInt);
				objPar[1].Value = NroCuota;
			
				objPar[2] = new SqlParameter("@Importe",SqlDbType.Decimal);
				objPar[2].Value = Importe;
			
				objPar[3]  = new SqlParameter("@IP",SqlDbType.VarChar,20);
				objPar[3].Value = IP;

				objPar[4]  = new SqlParameter("@Usuario",SqlDbType.VarChar,50);
				objPar[4].Value = Usuario;

				objPar[5]  = new SqlParameter("@Mensual",SqlDbType.Int);
				objPar[5].Value = Mensual;

				SqlHelper.ExecuteNonQuery(objCon, CommandType.StoredProcedure, "Cuotas_A", objPar);

				return ;	
			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCon.Dispose();
				objCnn=null;
			
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
				string mensaje = String.Empty;
						
				string filtro = "(MensualCuota >= "+Mensual.ToString()+")";
				NovVieja.Tables[0].DefaultView.RowFilter= filtro;
				NovVieja.Tables[0].DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

				DataView dvNovViejas =  NovVieja.Tables[0].DefaultView ;

				if ( dvNovViejas.Count  !=1 || ( int.Parse(dvNovViejas[0]["MensualCuota"].ToString()) != Mensual))
				{
					mensaje = "Sólo se puede dar de baja a partir de la última cuota y en forma descendente.";
				}

				//				if (NovVieja.Tables[0].Rows.Count != 1 || (int.Parse(NovVieja.Tables[0].Rows[0]["MensualCuota"].ToString())!= Mensual))
				//				{
				//					mensaje = "Sólo se puede dar de baja la última cuota sin liquidar";
				//				}
				
				if (mensaje == string.Empty)
				{
					if ( dvNovViejas[0]["IdEstadoReg"].ToString() != "1" )
					{
						// para novedades en proceso de liquidación o en transito a la misma
						mensaje = "Novedad en proceso de liquidación. No puede darse de baja";
					}
				}
				if (mensaje == string.Empty)
				{

					int MensualAct = int.Parse(cie.Trae_Fec_Prox_Cierre().Tables[0].Rows[0]["Mensual"].ToString());

					long IdNovedad = long.Parse( dvNovViejas[0]["IdNovedad"].ToString());
					

					if (Mensual == MensualAct)
					{
						long IdPrestador = long.Parse( dvNovViejas[0]["IdPrestador"].ToString());
						long IdBeneficiario  = long.Parse( dvNovViejas[0]["IdBeneficiario"].ToString());
						int ConceptoOPP = int.Parse( dvNovViejas[0]["CodConceptoLiq"].ToString());
						double Importe = double.Parse( dvNovViejas[0]["ImporteCuota"].ToString()) * -1;
			
						ModificaSaldo(IdPrestador,IdBeneficiario,ConceptoOPP,Importe,Usuario);
					}

					//Novedades_PasaAHist(IdNovedad,Mensual,7,3,0,IP,Usuario);					
					Novedades_PasaAHist(IdNovedad,Mensual,idEstadoReg,3,0,IP,Usuario);					//Modificado por COK 09.08.2005

				}			

				return mensaje + "|0| ";
			}
			catch(Exception err)
			{
				throw err ;
				
			}
			finally
			{
				cie.Dispose();
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

			Conexion objCnn = new Conexion();	
			
			SqlConnection objCon = new SqlConnection();

			String[] retorno = new String[2];
			try
			{

				objCon = objCnn.Conectar();
				SqlParameter [] objPar = new SqlParameter[15];
			
				objPar[0] = new SqlParameter("@IdBeneficiario",SqlDbType.BigInt); 
				objPar[0].Value = IdBeneficiario;

				objPar[1] = new SqlParameter("@IdPrestador",SqlDbType.BigInt);
				objPar[1].Value = IdPrestador;
	
				objPar[2] = new SqlParameter("@FecNovedad",SqlDbType.DateTime);
				objPar[2].Value = FecNovedad;
		
				objPar[3] = new SqlParameter("@CodMovimiento",SqlDbType.TinyInt);
				objPar[3].Value = CodMovimiento;

				objPar[4] = new SqlParameter("@TipoConcepto",SqlDbType.TinyInt);
				objPar[4].Value = TipoConcepto;

				objPar[5] = new SqlParameter("@CodConceptoLiq",SqlDbType.Int);
				objPar[5].Value = ConceptoOPP;
					
				objPar[6] = new SqlParameter("@ImporteTotal",SqlDbType.Decimal);
				objPar[6].Value = ImpTotal;
			
				objPar[7] = new SqlParameter("@CantCuotas",SqlDbType.TinyInt);
				objPar[7].Value = CantCuotas;					
	
				objPar[8] = new SqlParameter("@Porcentaje",SqlDbType.Decimal);
				objPar[8].Value = Porcentaje;
			
				objPar[9] = new SqlParameter("@MAC",SqlDbType.VarChar,100);
				objPar[9].Value = MAC;
			
				/* OJO en un futuro se va a exigir cargar el nro de comprobante */
				objPar[10] = new SqlParameter("@NroComprobante",SqlDbType.VarChar,50);
				objPar[10].Value = NroComprobante;
				
			
				objPar[11]  = new SqlParameter("@Usuario",SqlDbType.VarChar,50);
				objPar[11].Value = Usuario;

				objPar[12]  = new SqlParameter("@IP",SqlDbType.VarChar,20);
				objPar[12].Value = IP;
			
				objPar[13]  = new SqlParameter("@IdEstadoReg",SqlDbType.TinyInt);
				objPar[13].Value = IdEstadoReg;

				objPar[14]  = new SqlParameter("@IdNovedad",SqlDbType.BigInt);
				objPar[14].Direction = ParameterDirection.Output;
				objPar[14].Value = 0;


				//SqlHelper.ExecuteNonQuery(objCon, CommandType.StoredProcedure, "Novedades_A", objPar);
				SqlHelper.ExecuteNonQuery(objCnn.ConectarString(), CommandType.StoredProcedure, "Novedades_A", objPar);
				
				retorno[0]= objPar[14].Value.ToString();
				retorno[1] = MAC;

				return retorno;
			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCon.Dispose();
				objCnn=null;
				
			}
			
			
		}
		
			
		#endregion
		
		#region Novedades Rechazadas A ConTasas

		[SecurityRole("OperadorEntidad")]
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
			Conexion objCnn = new Conexion();	
			
			SqlConnection objCon = new SqlConnection();

			try
			{
				
				objCon = objCnn.Conectar();
				SqlParameter [] objPar = new SqlParameter[24];

				objPar[0] = new SqlParameter("@IdBeneficiario",SqlDbType.BigInt); 

				objPar[0] = new SqlParameter("@IdBeneficiario",SqlDbType.BigInt);
				objPar[0].Value = IdBeneficiario;
				objPar[1] = new SqlParameter("@IdPrestador", SqlDbType.BigInt);
				objPar[1].Value = IdPrestador;
				objPar[2] = new SqlParameter("@CodMovimiento",SqlDbType.TinyInt);
				objPar[2].Value = CodMovimiento;
				objPar[3] = new SqlParameter("@TipoConcepto",SqlDbType.TinyInt);
				objPar[3].Value = TipoConcepto;
				objPar[4] = new SqlParameter("@CodConceptoLiq",SqlDbType.Int);
				objPar[4].Value = CodConceptoLiq;
				objPar[5] = new SqlParameter("@ImporteTotal",SqlDbType.Decimal);
				objPar[5].Value = ImporteTotal;
				objPar[6] = new SqlParameter("@CantCuotas",SqlDbType.TinyInt);
				objPar[6].Value = CantCuotas;
				objPar[7] = new SqlParameter("@Porcentaje",SqlDbType.Decimal);
				objPar[7].Value = Porcentaje;
				objPar[8] = new SqlParameter("@NroComprobante",SqlDbType.VarChar,50);
				objPar[8].Value = NroComprobante;
				objPar[9] = new SqlParameter("@IP",SqlDbType.VarChar, 20);
				objPar[9].Value = IP;
				objPar[10] = new SqlParameter("@Usuario",SqlDbType.VarChar,50);
				objPar[10].Value = Usuario;
				objPar[11] = new SqlParameter("@montoPrestamo",SqlDbType.Decimal);
				objPar[11].Value = montoPrestamo;
				objPar[12] = new SqlParameter("@CuotaTotalMensual",SqlDbType.Decimal);
				objPar[12].Value = CuotaTotalMensual;
				objPar[13] = new SqlParameter("@TNA",SqlDbType.Decimal);
				objPar[13].Value = TNA;
				objPar[14] = new SqlParameter("@TEM",SqlDbType.Decimal);
				objPar[14].Value = TEM;
				objPar[15] = new SqlParameter("@gastoOtorgamiento",SqlDbType.Decimal);
				objPar[15].Value = gastoOtorgamiento;
				objPar[16] = new SqlParameter("@gastoAdmMensual",SqlDbType.Decimal);
				objPar[16].Value = gastoAdmMensual;
				objPar[17] = new SqlParameter("@cuotaSocial",SqlDbType.Decimal);
				objPar[17].Value = cuotaSocial;
				objPar[18] = new SqlParameter("@CFTEA",SqlDbType.Decimal);
				objPar[18].Value = CFTEA;
				objPar[19] = new SqlParameter("@CFTNAReal",SqlDbType.Decimal);
				objPar[19].Value = CFTNAReal;
				objPar[20] = new SqlParameter("@CFTEAReal",SqlDbType.Decimal);
				objPar[20].Value = CFTEAReal;
				objPar[21] = new SqlParameter("@gastoAdmMensualReal",SqlDbType.Decimal);
				objPar[21].Value = gastoAdmMensual;
				objPar[22] = new SqlParameter("@TIRReal",SqlDbType.Decimal);
				objPar[22].Value = TIRReal;
				objPar[23] = new SqlParameter("@TipoRechazo",SqlDbType.VarChar,300);
				objPar[23].Value = mensajeError;

				SqlHelper.ExecuteNonQuery(objCon, CommandType.StoredProcedure, "Novedades_Rechazadas_A_ConTasas", objPar);
			
			} 
			
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCnn=null;
			}
		}
		#endregion
	
		#region NovedadRechazada_Alta
		private void NovedadRechazada_Alta(long IdPrestador, long IdBeneficiario, DateTime FecNovedad, 
			byte TipoConcepto, int ConceptoOPP, string MAC, double ImpTotal, byte CantCuotas,Single Porcentaje,	 
			byte CodMovimiento, string  NroComprobante, string IP, string Usuario,int Mensual, string mensajeError)
		{		
			Conexion objCnn = new Conexion();	
			
			SqlConnection objCon = new SqlConnection();

			try
			{
				
				objCon = objCnn.Conectar();
				SqlParameter [] objPar = new SqlParameter[13];
			
				objPar[0] = new SqlParameter("@IdBeneficiario",SqlDbType.BigInt); 
				objPar[0].Value = IdBeneficiario;

				objPar[1] = new SqlParameter("@IdPrestador",SqlDbType.BigInt);
				objPar[1].Value = IdPrestador;
				
				objPar[2] = new SqlParameter("@CodMovimiento",SqlDbType.TinyInt);
				objPar[2].Value = CodMovimiento;

				objPar[3] = new SqlParameter("@TipoConcepto",SqlDbType.TinyInt);
				objPar[3].Value = TipoConcepto;

				objPar[4] = new SqlParameter("@CodConceptoLiq",SqlDbType.Int);
				objPar[4].Value = ConceptoOPP;
				
				objPar[5] = new SqlParameter("@ImporteTotal",SqlDbType.Decimal);
				objPar[5].Value = ImpTotal;
			
				objPar[6] = new SqlParameter("@CantCuotas",SqlDbType.TinyInt);
				objPar[6].Value = CantCuotas;
			
				objPar[7] = new SqlParameter("@Porcentaje",SqlDbType.Decimal);
				objPar[7].Value = Porcentaje;
			
				objPar[8] = new SqlParameter("@NroComprobante",SqlDbType.VarChar,50);
				objPar[8].Value = NroComprobante;

				objPar[9]  = new SqlParameter("@IP",SqlDbType.VarChar,20);
				objPar[9].Value = IP;
						
				objPar[10] = new SqlParameter("@Usuario",SqlDbType.VarChar,15);
				objPar[10].Value = Usuario;

				objPar[11] = new SqlParameter("@FecRechazo",SqlDbType.DateTime);
				objPar[11].Value = DateTime.Today;

				objPar[12] = new SqlParameter("@TipoRechazo",SqlDbType.VarChar,300);
				objPar[12].Value = mensajeError;
				
				SqlHelper.ExecuteNonQuery(objCon, CommandType.StoredProcedure, "Novedades_Rechazadas_A", objPar);
				
			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCon.Dispose();
				objCnn=null;
			}
			
			
		}
		#endregion
	
		#region Novedades_Modifica_EstadoReg
		private void Novedades_Modifica_EstadoReg(long IdNovedad,byte IdEstadoReg, string IP, string Usuario)
		{		
			Conexion objCnn = new Conexion();	
			
			SqlConnection objCon = new SqlConnection();

			try
			{
				
				objCon = objCnn.Conectar();
				SqlParameter [] objPar = new SqlParameter[4];
			
				objPar[0] = new SqlParameter("@IdNovedad",SqlDbType.BigInt); 
				objPar[0].Value = IdNovedad;

				objPar[1] = new SqlParameter("@IdEstadoReg",SqlDbType.BigInt);
				objPar[1].Value = IdEstadoReg;
				
				objPar[2] = new SqlParameter("@Usuario",SqlDbType.VarChar,50);
				objPar[2].Value = Usuario;
				
				objPar[3]  = new SqlParameter("@IP",SqlDbType.VarChar,20);
				objPar[3].Value = IP;											
				
				SqlHelper.ExecuteNonQuery(objCon, CommandType.StoredProcedure, "Novedades_M", objPar);
				
			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCon.Dispose();
				objCnn=null;
			}
			
		}
		#endregion

		#region Novedades_PasaAHist		
		private static void Novedades_PasaAHist( long IdNovedad, int Mensual, byte IdEstadoReg, byte IdEstadoNov, 
			double ImporteLiq, string IPCuota, string Usuario)
		{

			Conexion objCnn = new Conexion();	
			
			SqlConnection objCon = new SqlConnection();

			try
			{
				objCon = objCnn.Conectar();
				SqlParameter [] objPar = new SqlParameter[7];
		
				objPar[0] = new SqlParameter("@IdNovedad",SqlDbType.BigInt); 
				objPar[0].Value = IdNovedad;

				objPar[1]  = new SqlParameter("@Mensual",SqlDbType.Int);
				objPar[1].Value = Mensual;

				objPar[2] = new SqlParameter("@IdEstadoReg",SqlDbType.TinyInt);
				objPar[2].Value = IdEstadoReg;

				objPar[3] = new SqlParameter("@IdEstadoNov",SqlDbType.TinyInt);
				objPar[3].Value = IdEstadoNov;
			
				objPar[4] = new SqlParameter("@ImporteLiq",SqlDbType.Decimal);
				objPar[4].Value = ImporteLiq;
											
				objPar[5]  = new SqlParameter("@IP",SqlDbType.VarChar,20);
				objPar[5].Value = IPCuota;
			
				objPar[6]  = new SqlParameter("@Usuario",SqlDbType.VarChar,50);
				objPar[6].Value = Usuario;
		
				SqlHelper.ExecuteNonQuery(objCon, CommandType.StoredProcedure, "Novedades_PaHist", objPar);

				return ;	
			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCon.Dispose();
				objCnn=null;
			
			}
			
		}

		#endregion

		#region ModificaSaldo
		private long ModificaSaldo(long IdPrestador, long IdBeneficiario, int ConceptoOPP,  double Importe,string Usuario)
		{		
			Conexion objCnn = new Conexion();	
			
			SqlConnection objCon = new SqlConnection();

			try
			{

				objCon = objCnn.Conectar();
				SqlParameter [] objPar = new SqlParameter[5];
			
				objPar[0] = new SqlParameter("@IdBeneficiario",SqlDbType.BigInt); 
				objPar[0].Value = IdBeneficiario;

				objPar[1] = new SqlParameter("@IdPrestador",SqlDbType.BigInt);
				objPar[1].Value = IdPrestador;		

				objPar[2] = new SqlParameter("@CodConceptoLiq",SqlDbType.Int);
				objPar[2].Value = ConceptoOPP;

				objPar[3]  = new SqlParameter("@Importe",SqlDbType.Decimal);
				objPar[3].Value = Importe;
				
				objPar[4]  = new SqlParameter("@Usuario",SqlDbType.VarChar,50);
				objPar[4].Value = Usuario;

			
				//SqlHelper.ExecuteNonQuery(objCon, CommandType.StoredProcedure, "Beneficiarios_MSaldo", objPar);
				
				SqlHelper.ExecuteNonQuery(objCnn.ConectarString(),"Beneficiarios_MSaldo", IdBeneficiario,IdPrestador,ConceptoOPP,Importe,Usuario);
				
				
				return 0 ;
			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCon.Dispose();
				objCnn=null;
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
							CodMovimiento, NroComprobante,true,0 );
		}

		private string Valido_Nov(long IdPrestador, long IdBeneficiario, byte TipoConcepto, 
			int ConceptoOPP, double ImpTotal, byte CantCuotas,Single Porcentaje, byte CodMovimiento, String NroComprobante, bool bGestionErrores, decimal montoPrestamo)
		{
			string mensaje = String.Empty;
			Conexion objCnn = new Conexion();	
									
			try
			{				
				SqlParameter [] objPar = new SqlParameter[12];
				
				// CONTROLA MAXIMOS INTENTOS 
				if(bGestionErrores)
					mensaje =  CtrolIntentos(IdPrestador,IdBeneficiario) ;
			
				// CONTROLA QUE ESTE INFORMADO EL COMPROBANTE
				if ((mensaje ==String.Empty) && (NroComprobante.Trim().Length < 4))
				{
					mensaje = "El nro. de comprobante debe ser mayor a 3 dígitos.";
				}
				// CONTROLA TIPOS DE CAMPOS
			
				if (mensaje ==String.Empty)
				{	
					mensaje = CtrolMontos(TipoConcepto,ImpTotal,CantCuotas,Porcentaje);
				}

						
				// valida la novedad
				if (mensaje == String.Empty)
				{
					#region parametros
					objPar[0] = new SqlParameter("@IdPrestador",SqlDbType.BigInt); 
					objPar[0].Value = IdPrestador;
		
					objPar[1] = new SqlParameter("@IdBeneficiario",SqlDbType.BigInt); 
					objPar[1].Value = IdBeneficiario;

					objPar[2] = new SqlParameter("@TipoConcepto",SqlDbType.TinyInt); 
					objPar[2].Value = TipoConcepto;

					objPar[3] = new SqlParameter("@CodConceptoLiq",SqlDbType.Int); 
					objPar[3].Value = ConceptoOPP;

					objPar[4] = new SqlParameter("@CodMovimiento",SqlDbType.TinyInt); 
					objPar[4].Value = CodMovimiento;

					objPar[5] = new SqlParameter("@ImporteTotal",SqlDbType.Decimal); 
					objPar[5].Value = ImpTotal;

					objPar[6] = new SqlParameter("@CantCuotas",SqlDbType.TinyInt); 
					objPar[6].Value = CantCuotas;

					objPar[7] = new SqlParameter("@Porcentaje",SqlDbType.Decimal); 
					objPar[7].Value = Porcentaje;

					objPar[8] = new SqlParameter("@NroComprobante",SqlDbType.VarChar,100); 
					objPar[8].Value = NroComprobante;

					objPar[9] = new SqlParameter("@MontoPrestamo", SqlDbType.Decimal);
					objPar[9].Value = montoPrestamo;

					objPar[10] = new SqlParameter("@Mensaje",SqlDbType.VarChar,100); 
					objPar[10].Direction = ParameterDirection.Output;
					objPar[10].Value = mensaje;

					objPar[11] = new SqlParameter("@EsAfiliacion",SqlDbType.Bit);
					objPar[11].Direction = ParameterDirection.Output;
					objPar[11].Value = 1;

					#endregion parametros

					SqlHelper.ExecuteNonQuery(objCnn.ConectarString(),CommandType.StoredProcedure,"Novedad_Valido_Derecho",objPar);
					
					mensaje = objPar[10].Value.ToString()+'|'+objPar[11].Value.ToString();
				
				}
				if (mensaje != String.Empty ){ mensaje += "|" ;}
				
				return  mensaje;
			}
		
			catch( Exception err)
			{
				throw err ;
			}
			finally
			{
				objCnn = null;			
			}
		}
	
		#endregion

		#region Valido_Nov viejo
		/* // validacion en com+ - se elimina y se pasa al store
				region Valido_Nov
				private string Valido_Nov(long IdPrestador, long IdBeneficiario, byte TipoConcepto, 
					int ConceptoOPP, double ImpTotal, byte CantCuotas,Single Porcentaje, byte CodMovimiento, String NroComprobante)
				{
					string mensaje = String.Empty;
					DataSet ds = new DataSet();
			
					Beneficiarios benef	= new Beneficiarios();						
					
					try
					{
				
						// CONTROLA MAXIMOS INTENTOS 
						mensaje = CtrolIntentos(IdPrestador,IdBeneficiario);
					
				
						// CONTROLA EXISTENCIA DEL BENEFICIARIO 
						if (mensaje == String.Empty)
						{
					
							ds = benef.Traer(IdBeneficiario.ToString(),String.Empty);
					
							if ((ds.Tables[0].Rows.Count== 0) || (long.Parse(ds.Tables[0].Rows[0]["IdBeneficiario"].ToString()) == 0))
							{
								mensaje =  "No se encontraron datos para el Nro. de beneficio.";
							}				

						}							

						//CONTROLA SI YA EXISTE EL REGISTRO
						if (mensaje ==String.Empty)
						{

							mensaje= CtrolDuplicados( IdPrestador,IdBeneficiario,TipoConcepto, ConceptoOPP , ImpTotal, CantCuotas, Porcentaje, NroComprobante );
						}
				

						// CONTROLA QUE ESTE INFORMADO EL COMPROBANTE
						if ((mensaje ==String.Empty) && (NroComprobante.Trim().Length < 4))
						{
							mensaje = "El nro. de comprobante debe ser mayor a 3 dígitos.";
						}

								
						// CONTROLA  BENEFICIO BLOQUEADO
						if (mensaje ==String.Empty)
						{
							mensaje= CtrolBloqueado(IdBeneficiario);
						}

						// CONTROLA INHIBIDO
						if (mensaje ==String.Empty)
						{
							mensaje= CtrolInhibido(IdPrestador,IdBeneficiario,ConceptoOPP);
						}
						// CONTROLA TIPOS DE CAMPOS
			
						if (mensaje ==String.Empty)
						{	
							mensaje = CtrolMontos(TipoConcepto,ImpTotal,CantCuotas,Porcentaje);
						}
                               			
						//CONTROLES PARA ALTA Y MODIFICACION. NO BAJA
						if (mensaje==String.Empty && CodMovimiento != 4 )
						{																										
							//CONTROLA SI ALCANZAN LAS OCURRENCIAS PARA INGRESAR UNA MAS
							mensaje= CtrolOcurrencias (byte.Parse(ds.Tables[0].Rows[0]["CantOcurrenciasDisp"].ToString()),IdBeneficiario,ConceptoOPP);

							if (mensaje == String.Empty)
							{
								//CONTROLA SI SE MIRA SI CUMPLE CON RESTRICCIONES POR EXCAJA. 
								mensaje= CtrolRestricciones(IdPrestador,IdBeneficiario,ConceptoOPP);
							}
						}
				
						return mensaje;
					}
					catch( Exception err)
					{
						throw err ;
					}
					finally
					{
						ds.Dispose();		
						benef.Dispose();
					}

				}
				endregion
		*/
		#endregion


		#region Controles

		#region ConceptoOPP_Habil_X_Prest
		private string ConceptoOPP_Habil_X_Prest( long IdPrestador, int ConceptoOPP, byte TipoConcepto )
		{		
			// Controla que exista la relacion y si existe trae si es afiliacion o no
			string mensaje= String.Empty;
			
			Conexion objCnn = new Conexion();	
			
			try
			{
				SqlParameter [] objPar = new SqlParameter[5];
			
				objPar[0] = new SqlParameter("@IdPrestador",SqlDbType.BigInt);
				objPar[0].Value = IdPrestador;				

				objPar[1] = new SqlParameter("@ConceptoOPP",SqlDbType.Int);
				objPar[1].Value = ConceptoOPP;				

				objPar[2] = new SqlParameter("@TipoConcepto",SqlDbType.TinyInt);
				objPar[2].Value = TipoConcepto;				

				objPar[3] = new SqlParameter("@Ok",SqlDbType.Bit);
				objPar[3].Direction = ParameterDirection.Output;
				objPar[3].Value = 0;

				objPar[4] = new SqlParameter("@EsAfiliacion",SqlDbType.Bit);
				objPar[4].Direction = ParameterDirection.Output;
				objPar[4].Value = 0;

				SqlHelper.ExecuteNonQuery(objCnn.Conectar(), CommandType.StoredProcedure, "ConceptoOPP_Habil_X_Prest", objPar );
							
				
				if ((Boolean) objPar[3].Value == false)
				{
					mensaje = "Concepto Liq - Tipo Concepto no habilitada para el Prestador|False";
				}
				else 
				{
					mensaje ="|" + objPar[4].Value.ToString();
				}
				return mensaje;

			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCnn=null;
			}

		}

		#endregion
				
		#region Ctrol de Duplicados 
		private string CtrolDuplicados(long IdPrestador, long IdBeneficiario, byte TipoConcepto, 
			int ConceptoOPP, double ImpTotal, byte CantCuotas, Single Porcentaje,
			string NroComprobante)
		{		
		
			Conexion objCnn = new Conexion();	
			

			try
			{

				SqlParameter [] objPar = new SqlParameter[9];
		
				objPar[0] = new SqlParameter("@IdPrestador",SqlDbType.BigInt ); 
				objPar[0].Value = IdPrestador;

				objPar[1] = new SqlParameter("@IdBeneficiario",SqlDbType.BigInt ); 
				objPar[1].Value = IdBeneficiario;
		
				objPar[2] = new SqlParameter("@TipoConcepto",SqlDbType.TinyInt); 
				objPar[2].Value = TipoConcepto;

				objPar[3] = new SqlParameter("@ConceptoOPP",SqlDbType.Int);
				objPar[3].Value = ConceptoOPP;

				objPar[4] = new SqlParameter("@ImpTotal",SqlDbType.BigInt ); 
				objPar[4].Value = ImpTotal;

				objPar[5] = new SqlParameter("@CantCuotas",SqlDbType.TinyInt ); 
				objPar[5].Value = CantCuotas;

				objPar[6] = new SqlParameter("@Porcentaje",SqlDbType.SmallInt ); 
				objPar[6].Value = Porcentaje;

				objPar[7] = new SqlParameter("@NroComprobante",SqlDbType.VarChar,50 ); 
				objPar[7].Value = NroComprobante;

				objPar[8] = new SqlParameter("@Existe",SqlDbType.Bit);
				objPar[8].Direction = ParameterDirection.Output;
				objPar[8].Value = 0;

				SqlHelper.ExecuteNonQuery( objCnn.Conectar(), CommandType.StoredProcedure, "Novedades_Existe", objPar );
				
				return ((Boolean) objPar[8].Value == true) ? "Ud. está intentando re-ingresar una novedad ya existente. Proceso cancelado":String.Empty;
				
			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCnn=null;
			}

		}
		#endregion

		#region CtrolAlcanza
		[SecurityRole("OperadorEntidad")]
		public string CtrolAlcanza( long IdBeneficiario, double Importe, long IdPrestador, int ConceptoOPP )
		{		
			// controla si alcanza el monto a ingresar - si no alcanza ingresa el monto en rechazados		
			string mensaje= String.Empty;
			
			Conexion objCnn = new Conexion();	
			
			try
			{
				SqlParameter [] objPar = new SqlParameter[5];
			
				objPar[0] = new SqlParameter("@IdBeneficiario",SqlDbType.BigInt); 
				objPar[0].Value = IdBeneficiario;

				objPar[1] = new SqlParameter("@monto",SqlDbType.Decimal);
				objPar[1].Value = Importe;				

				objPar[2] = new SqlParameter("@IdPrestador",SqlDbType.BigInt);
				objPar[2].Value = IdPrestador;				

				objPar[3] = new SqlParameter("@ConceptoOPP",SqlDbType.Int);
				objPar[3].Value = ConceptoOPP;				

				objPar[4] = new SqlParameter("@alcanza",SqlDbType.TinyInt);
				objPar[4].Direction = ParameterDirection.InputOutput;
				objPar[4].Value = 0;

				SqlHelper.ExecuteNonQuery(objCnn.Conectar(), CommandType.StoredProcedure, "AlcanzaAfectacion", objPar );
							
				
				if ((Byte) objPar[4].Value == 0)
				{
					mensaje = "Afectación Disponible Insuficiente";
				}
				else 
				{
					mensaje =String.Empty;
				}
				return mensaje;

			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCnn=null;
			}

		}

		#endregion

		#region CtrolCantRechazos
		private int CtrolCantRechazos(long IdPrestador, long IdBeneficiario)
		{		
		
			Conexion objCnn = new Conexion();	
			
			try
			{
			
				SqlParameter [] objPar = new SqlParameter[3];
		
			
				objPar[0] = new SqlParameter("@IdBeneficiario",SqlDbType.BigInt); 
				objPar[0].Value = IdBeneficiario;

				objPar[1] = new SqlParameter("@IdPrestador",SqlDbType.BigInt); 
				objPar[1].Value = IdPrestador;

				objPar[2] = new SqlParameter("@CantRech",SqlDbType.TinyInt); 
				objPar[2].Direction = ParameterDirection.Output;
				objPar[2].Value = 0;
				
				SqlHelper.ExecuteNonQuery( objCnn.Conectar(), CommandType.StoredProcedure, "NovRechazadas_TCant", objPar);
				

				return (int.Parse(objPar[2].Value.ToString()));

			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
			
				objCnn=null;
			}

		}
		#endregion

		#region CtrolIntentos
		private string CtrolIntentos(long IdPrestador, long IdBeneficiario)
		{		
			string mensaje =String.Empty;
			try
			{
				int MaxIntentos= int.Parse(ConfigurationSettings.AppSettings["DAT_MaxIntentos"].ToString());
				int MaxCantRechazos = CtrolCantRechazos(IdPrestador,IdBeneficiario);
				
				mensaje=(MaxCantRechazos >= MaxIntentos) ? "Máxima cantidad de intentos permitidos":String.Empty;
				return (mensaje);
			}		
			catch( Exception err)
			{
				throw err ;
			}			
		}
		#endregion

		#region CtrolBloqueado
		private string CtrolBloqueado ( long IdBeneficiario)
		{		
		
			Conexion objCnn = new Conexion();	
			
			SqlConnection objCon = new SqlConnection();

			try
			{
				objCon = objCnn.Conectar();
				SqlParameter [] objPar = new SqlParameter[2];
		
			
				objPar[0] = new SqlParameter("@IdBeneficiario",SqlDbType.BigInt); 
				objPar[0].Value = IdBeneficiario;

				
				objPar[1] = new SqlParameter("@Existe",SqlDbType.Bit);
				objPar[1].Direction = ParameterDirection.Output;
				objPar[1].Value = 0;

				SqlHelper.ExecuteNonQuery(objCon, CommandType.StoredProcedure, "Beneficios_Bloqueados_Busca", objPar );
				
				return ((Boolean) objPar[1].Value == true) ? "Beneficio Bloqueado":String.Empty;
				
			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCon.Dispose();
				objCnn=null;
			}

		}
		#endregion

		#region CtrolInhibido
		private string CtrolInhibido (long IdPrestador, long IdBeneficiario, int ConceptoOPP)
		{		
		
			Conexion objCnn = new Conexion();	
			
			SqlConnection objCon = new SqlConnection();

			try
			{
				objCon = objCnn.Conectar();
				SqlParameter [] objPar = new SqlParameter[4];
		
			
				objPar[0] = new SqlParameter("@IdPrestador",SqlDbType.BigInt); 
				objPar[0].Value = IdPrestador;

				objPar[1] = new SqlParameter("@IdBeneficiario",SqlDbType.BigInt); 
				objPar[1].Value = IdBeneficiario;

				objPar[2] = new SqlParameter("@ConceptoLiq",SqlDbType.Int); 
				objPar[2].Value = ConceptoOPP;
		
				objPar[3] = new SqlParameter("@Existe",SqlDbType.Bit);
				objPar[3].Direction = ParameterDirection.InputOutput;
				objPar[3].Value = 0;


				SqlHelper.ExecuteNonQuery(objCon, CommandType.StoredProcedure, "Beneficios_Inhibido_Busca", objPar );
				return ((Boolean) objPar[3].Value== true) ? String.Concat( "Código inhibido para el Beneficio:" , IdBeneficiario.ToString() ) : String.Empty;
				
			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCon.Dispose();
				objCnn=null;
			}

		}
		#endregion

		#region CtrolOcurrencias
		[SecurityRole("OperadorEntidad")]
		private string CtrolOcurrencias (byte CantOcurrDisp,long IdBeneficiario, int ConceptoOPP)
		{		
		
			Conexion objCnn = new Conexion();	
			
			SqlConnection objCon = new SqlConnection();

			try
			{
				objCon = objCnn.Conectar();
				SqlParameter [] objPar = new SqlParameter[4];
		
				objPar[0] = new SqlParameter("@IdBeneficiario",SqlDbType.BigInt); 
				objPar[0].Value = IdBeneficiario;

				objPar[1] = new SqlParameter("@ConceptoLiq",SqlDbType.Int); 
				objPar[1].Value = ConceptoOPP;
		
				objPar[2] = new SqlParameter("@CantOcurrDisp",SqlDbType.BigInt); 
				objPar[2].Value = CantOcurrDisp;

				objPar[3] = new SqlParameter("@Alcanza",SqlDbType.Bit);
				objPar[3].Direction = ParameterDirection.InputOutput;
				objPar[3].Value = 0;

				SqlHelper.ExecuteNonQuery(objCon, CommandType.StoredProcedure, "Novedades_Alcanza_Ocurrencia", objPar );
				return ((Boolean) objPar[3].Value  == false  ? "La Liquidación no posee lugar para ingresar un nuevo descuento":String.Empty);
				
			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCon.Dispose();
				objCnn=null;
			}

		}
		#endregion

		#region CtrolOcurrenciasCancCuotas
		[SecurityRole("OperadorEntidad")]
		public Boolean CtrolOcurrenciasCancCuotas (byte CantOcurrDisp,long IdBeneficiario, int ConceptoOPP, long IdNovedadABorrar)
		{		
		
			Conexion objCnn = new Conexion();	
			
			SqlConnection objCon = new SqlConnection();

			try
			{
				objCon = objCnn.Conectar();
				SqlParameter [] objPar = new SqlParameter[5];
		
				objPar[0] = new SqlParameter("@IdBeneficiario",SqlDbType.BigInt); 
				objPar[0].Value = IdBeneficiario;

				objPar[1] = new SqlParameter("@ConceptoLiq",SqlDbType.Int); 
				objPar[1].Value = ConceptoOPP;
		
				objPar[2] = new SqlParameter("@CantOcurrDisp",SqlDbType.BigInt); 
				objPar[2].Value = CantOcurrDisp;

				objPar[3] = new SqlParameter("@IdNovedad",SqlDbType.BigInt); 
				objPar[3].Value = IdNovedadABorrar;

				objPar[4] = new SqlParameter("@Alcanza",SqlDbType.Bit);
				objPar[4].Direction = ParameterDirection.InputOutput;
				objPar[4].Value = 0;

				SqlHelper.ExecuteNonQuery(objCon, CommandType.StoredProcedure, "Novedades_Alcanza_Ocurrencia_Xa_CancCuotas", objPar );
				return ((Boolean) objPar[4].Value);
				
			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCon.Dispose();
				objCnn=null;
			}

		}
		#endregion
	
		#region CtrolRestricciones
		private string CtrolRestricciones (long IdPrestador, long IdBeneficiario, int ConceptoOPP)
		{		
		
			Conexion objCnn = new Conexion();	
			
			SqlConnection objCon = new SqlConnection();
			try
			{

	
				objCon = objCnn.Conectar();
				SqlParameter [] objPar = new SqlParameter[4];
		
				objPar[0] = new SqlParameter("@ConceptoLiq",SqlDbType.Int); 
				objPar[0].Value = ConceptoOPP;
		
				objPar[1] = new SqlParameter("@IdPrestador",SqlDbType.BigInt); 
				objPar[1].Value = IdPrestador;

				objPar[2] = new SqlParameter("@ExCaja",SqlDbType.TinyInt); 
				objPar[2].Value = IdBeneficiario.ToString("00000000000").Substring(0,2);
				
				objPar[3] = new SqlParameter("@Ok",SqlDbType.Bit);
				objPar[3].Direction = ParameterDirection.InputOutput;
				objPar[3].Value = 0;

				SqlHelper.ExecuteNonQuery(objCon, CommandType.StoredProcedure, "RestriccionesXCodConceptoLiq_TxExCaja", objPar );
				return ((Boolean) objPar[3].Value== false ? "Beneficio restringido para éste concepto":String.Empty);
				
			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCon.Dispose();
				objCnn=null;
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
					//mensaje = (ImpTotal <= Double.Parse( "0" ) ) ? "El campo Importe debe ser mayor a 0":String.Empty;
					if ( ImpTotal <= 0 ) { mensajeMontos = @"El campo Importe debe ser mayor a 0" ;}
					break;
				
				case 3:
					//mensaje = (ImpTotal <= 0) ? "El campo Importe debe ser mayor a 0":String.Empty;
					if ( ImpTotal <= 0 ) { mensajeMontos = @"El importe resultante de resta la cuota total  y cuota afiliación debe ser mayor a CERO (0)" ;}

					if (mensajeMontos == String.Empty)
					{
						//mensaje = (CantCuotas <= 0 || CantCuotas > 255) ? "El campo Cant. Cuotas debe estar comprendido entre 1 y 255":String.Empty;
						//mensaje = (CantCuotas <= 0 || CantCuotas > 120) ? "El campo Cant. Cuotas debe estar comprendido entre 1 y 120":String.Empty;
						if (CantCuotas <= 0 || CantCuotas > 240) { mensajeMontos = @"El campo Cant. Cuotas debe estar comprendido entre 1 y 240" ;}
					}									
					break;
					/*case 4:
								mensaje = Val_Importe(_ImpTotal,"Importe Total",0);
								if (mensaje == null ||mensaje == String.Empty)
								{
									mensaje = Val_Importe(Porcentaje,"Porcentaje",1);							
								}
							
								break;
							case 5:
								 mensaje = Val_Importe(Porcentaje,"Porcentaje",1);							
													
								break; */
				case 6:
					//mensaje = (Porcentaje <= 0 || Porcentaje > 100) ? "El campo Porcentaje debe ser mayor que 0 y menor a 100":String.Empty;				
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
			Conexion objCnn = new Conexion();	
			
			SqlConnection objCon = new SqlConnection();
			string mensaje = string.Empty;
			Boolean resp;

			try
			{

				objCon = objCnn.Conectar();
				SqlParameter [] objPar = new SqlParameter[6];

				objPar[0] = new SqlParameter("@IdPrestador",SqlDbType.BigInt); 
				objPar[0].Value = IdPrestador;

				objPar[1] = new SqlParameter("@TipoConcepto",SqlDbType.TinyInt); 
				objPar[1].Value = TipoConcepto;
	
				objPar[2] = new SqlParameter("@ConceptoOPP",SqlDbType.Int); 
				objPar[2].Value = ConceptoOPP;

				objPar[3] = new SqlParameter("@IdBeneficiario",SqlDbType.BigInt); 
				objPar[3].Value = IdBeneficiario;

				objPar[4] = new SqlParameter("@EsAfiliacion",SqlDbType.Bit); 
				objPar[4].Value = EsAfiliacion;

				objPar[5] = new SqlParameter("@Ok",SqlDbType.Bit);
				objPar[5].Direction = ParameterDirection.InputOutput;
				objPar[5].Value = 0;

				SqlHelper.ExecuteNonQuery(objCon, CommandType.StoredProcedure, "Novedades_CtrolPuedeAltaXTipo", objPar );
				resp = (Boolean) objPar[5].Value;
				if (resp== true)
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
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				objCon.Dispose();
				objCnn=null;
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

	[Guid("0EE8C72F-1634-4abc-A62A-67B9592DE7DC")] 
	
	public interface DAT_INovedad
	{
		// Listados
		//DataSet  Novedades_Trae_NoAplicadas(byte opcion, long lintPrestador, long benefCuil, byte tipoConc, int concopp );
		DataSet  Novedades_Trae_NoAplicadas(byte opcion, long lintPrestador, long benefCuil, byte tipoConc, int concopp,string fdesde, string fhasta, bool GeneraArchivo,int mensual, out string rutaArchivoSal,string Usuario );
		DataSet  Novedades_TraerTodos(long lintPrestador);
//		DataSet  Novedades_Traer(byte opcion, long lintPrestador, long benefCuil, byte tipoConc, int concopp, int mensual );
		DataSet  Novedades_Traer(byte opcion, long lintPrestador,long benefCuil, byte tipoConc, int concopp, int mensual,  string fdesde, string fhasta,bool GeneraArchivo, out string rutaArchivoSal,string Usuario);
		DataSet  Novedades_Traer_X_Id ( long idNovedad);
		DataSet  Novedades_TxIdNovedad_Sliq ( long idNovedad);
		DataSet  Novedades_Trae_Creditos_Activos(long Prestador,long Beneficiario);
		DataSet	 Novedades_T1o6_Trae(long Prestador,byte TipoConcepto);
		DataSet  Novedades_Trae_Xa_ABM(long Prestador, long Beneficiario);
		DataSet  Novedades_BajaTxIDNov_FecBaja( long IdNov, string FechaBaja );
		DataSet  Novedades_BajaTraer( long IDPrestador, byte OpcionBusqueda, long BenefCuil, byte TipoConc, int ConcOpp );
		DataSet  Novedades_BajaTraerAgrupada( long IDPrestador, byte OpcionBusqueda, long BenefCuil, byte TipoConc, int ConcOpp,string MesAplica,bool GeneraArchivo, out string rutaArchivoSal, string Usuario );
		DataSet CuotaSocial_TraeXCuil(long idbeneficiario, long idPrestador);

	}

	[Guid("F2F8B533-52BE-47dd-AD72-6C83CF2EFA0F")] 
	[ ProgId( "DAT_Novedad" ) ]
	[ClassInterface(ClassInterfaceType.None)]
	[ ObjectPooling( MinPoolSize = 10) ]
	//[JustInTimeActivation(true)]
	[Transaction(TransactionOption.Supported)]
	[ComponentAccessControl(true)]
	[EventTrackingEnabled ( true )]
	[SecureMethod]

	public class Novedad:ServicedComponent,DAT_INovedad
	{
	
		public Novedad ()
		{
		}



		
		#region Novedades_TraerTodos

		[SecurityRole("OperadorEntidad")]
		public DataSet  Novedades_TraerTodos(long lintPrestador)
		{
			DataSet ds = new DataSet();
			Conexion objCnn = new Conexion();	
			SqlConnection objCon = new SqlConnection();
			SqlCommand objCmd = new SqlCommand();
	
			try
			{
				
				/*				
				SqlParameter objPar = objCmd.Parameters.Add ("@Prestador",SqlDbType.BigInt);
				objPar.Direction = ParameterDirection.Input;
				objPar.Value = lintPrestador;
				*/

				ds=SqlHelper.ExecuteDataset( objCnn.ConectarString(),"Novedades_TT",lintPrestador );
				
				return ds;


			} 
			catch(SqlException ErrSQL)
			{
				
				if ( ErrSQL.Number == 1205 || ErrSQL.Number == 1204 ) 
				{
					throw new ApplicationException("Interbloqueo");	
				}
				else
				{
					throw ErrSQL;
				}

			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				ds.Dispose();
				objCmd.Dispose();			
				objCon.Dispose();
				objCnn=null;
			}
		}

		#endregion

		#region Novedades_Traer 

		private DataSet  Novedades_TraerConsulta(byte opcion, long lintPrestador,long benefCuil, byte tipoConc, int concopp, int mensual, string fdesde, string fhasta )
		{
			DataSet ds = new DataSet();
			
			Conexion objCnn = new Conexion();
			SqlParameter [] objPar = new SqlParameter[8];
	
			try
			{
				objPar[0] = new SqlParameter("@Opcion",SqlDbType.TinyInt); 
				objPar[0].Value = opcion;
				objPar[1] = new SqlParameter("@Prestador",SqlDbType.BigInt);
				objPar[1].Value = lintPrestador;
				objPar[2] = new SqlParameter("@BenefCuil",SqlDbType.BigInt);
				objPar[2].Value = benefCuil;
				objPar[3] = new SqlParameter("@TipoConc",SqlDbType.TinyInt);
				objPar[3].Value = tipoConc;
				objPar[4] = new SqlParameter("@Conc",SqlDbType.Int);
				objPar[4].Value = concopp;
				objPar[5] = new SqlParameter("@Mensual",SqlDbType.Int);
				objPar[5].Value =mensual;
				objPar[6] = new SqlParameter("@desde",SqlDbType.VarChar,21);
				objPar[6].Value =fdesde;
				objPar[7] = new SqlParameter("@hasta",SqlDbType.VarChar,21);
				objPar[7].Value =fhasta;

				ds=SqlHelper.ExecuteDataset( objCnn.Conectar(), CommandType.StoredProcedure, "Novedades_TT_SinMigrar", objPar );
				return ds;
			} 
			catch(SqlException ErrSQL)
			{
				
				if ( ErrSQL.Number == 1205 || ErrSQL.Number == 1204 ) 
				{
					throw new ApplicationException("Interbloqueo");	
				}
				else
				{
					throw ErrSQL;
				}

			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				ds.Dispose();
				objCnn=null;
			}
		}
		[SecurityRole("OperadorEntidad")]
		public DataSet  Novedades_Traer(byte OpcionBusqueda, long idPrestador,long benefCuil, byte tipoConc, int concopp, int mensual,  string fdesde, string fhasta,bool GeneraArchivo, out string rutaArchivoSal, string Usuario)
		{

			string rutaArchivo = string.Empty;
			string nombreArchivo = string.Empty;
			rutaArchivoSal =string.Empty;
			string rta =string.Empty;
			string nombreConsulta = "Novedades_ingresadas";
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

				DataSet ds = Novedades_TraerConsulta( OpcionBusqueda, idPrestador,benefCuil, tipoConc,concopp, mensual,fdesde,fhasta );

				if ((ds.Tables[0].Rows.Count != 0) && (OpcionBusqueda != 1  || GeneraArchivo== true))
				{
					int maxCantidad =new Conexion().MaxCantidadRegistros();
				
					if (ds.Tables[0].Rows.Count>=maxCantidad || GeneraArchivo== true)
					{
						nombreArchivo = Utilidades.GeneraNombreArchivo(nombreConsulta,idPrestador,out rutaArchivo);

						StreamWriter sw = new StreamWriter(rutaArchivo + nombreArchivo,false,System.Text.Encoding.UTF8);		
						
						string separador = new Conexion().DelimitadorCampo();

						DataTable miTabla = ds.Tables[0];							
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
						
						rta = new ConsultasBatch().AltaNuevaConsulta(idPrestador,nombreConsulta,0,OpcionBusqueda,mensual.ToString(),tipoConc,concopp,benefCuil,fdesde,fhasta,rutaArchivo,nombreArchivo,fGeneracion,true, Usuario);

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
				if (errsql.Number == -2)
				{
					nombreArchivo = Utilidades.GeneraNombreArchivo(nombreConsulta,idPrestador,out rutaArchivo);
					rta = new ConsultasBatch().AltaNuevaConsulta(idPrestador,nombreConsulta,0,OpcionBusqueda,mensual.ToString(),tipoConc,concopp,benefCuil,fdesde,fhasta,rutaArchivo,nombreArchivo,string.Empty,false, Usuario);
					
					throw new ApplicationException("MSG_ERROR Generando el archivo. Reingrese a la consulta en unos minutos.FIN_MSG_ERROR");
				}
				else
				{
					throw errsql;	
				}

			}
			catch( ApplicationException apperr)
			{
				throw new ApplicationException(apperr.Message) ;						
			}
			catch( Exception err)
			{
				throw err ;						
			}
			finally
			{
			
			
			}	
		}
		#endregion
	
		
		#region Novedades_Trae_NoAplicadas
						
		[SecurityRole("OperadorEntidad")]
		public DataSet  Novedades_Trae_NoAplicadas(byte OpcionBusqueda, long idPrestador, long benefCuil, byte tipoConc, int concopp,string fdesde, string fhasta, bool GeneraArchivo, int mensual,out string rutaArchivoSal, string Usuario )
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
						/*StringBuilder cabecera = new StringBuilder();
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
						
						rta = new ConsultasBatch().AltaNuevaConsulta(idPrestador,nombreConsulta,0,OpcionBusqueda,mensual.ToString(),tipoConc,concopp,benefCuil,fdesde,fhasta,rutaArchivo,nombreArchivo,fGeneracion,true, Usuario);

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
				if (errsql.Number == -2)
				{
					nombreArchivo = Utilidades.GeneraNombreArchivo(nombreConsulta,idPrestador,out rutaArchivo);
					rta = new ConsultasBatch().AltaNuevaConsulta(idPrestador,nombreConsulta,0,OpcionBusqueda,mensual.ToString(),tipoConc,concopp,benefCuil,fdesde,fhasta,rutaArchivo,nombreArchivo,string.Empty,false, Usuario);
					
					throw new ApplicationException("MSG_ERROR Generando el archivo. Reingrese a la consulta en unos minutos.FIN_MSG_ERROR");
				}
				else
				{
					throw errsql;	
				}

			}
			catch( ApplicationException apperr)
			{
				throw new ApplicationException(apperr.Message) ;						
			}
			catch( Exception err)
			{
				throw err ;						
			}
			finally
			{
			
			
			}	

		}

		private DataSet  Novedades_Trae_NoAplicadasConsulta(byte opcion, long lintPrestador, long benefCuil, byte tipoConc, int concopp,string fdesde, string fhasta )
		{
			DataSet ds = new DataSet();
			
			Conexion objCnn = new Conexion();
			SqlParameter [] objPar = new SqlParameter[7];
	
			try
			{
				objPar[0] = new SqlParameter("@Opcion",SqlDbType.TinyInt); 
				objPar[0].Value = opcion;
				objPar[1] = new SqlParameter("@Prestador",SqlDbType.BigInt);
				objPar[1].Value = lintPrestador;
				objPar[2] = new SqlParameter("@BenefCuil",SqlDbType.BigInt);
				objPar[2].Value = benefCuil;
				objPar[3] = new SqlParameter("@TipoConc",SqlDbType.TinyInt);
				objPar[3].Value = tipoConc;
				objPar[4] = new SqlParameter("@Conc",SqlDbType.Int);
				objPar[4].Value = concopp;
				objPar[5] = new SqlParameter("@fDesde",SqlDbType.VarChar,21);
				objPar[5].Value = fdesde;
				objPar[6] = new SqlParameter("@fHasta",SqlDbType.VarChar,21);
				objPar[6].Value = fhasta;

				ds=SqlHelper.ExecuteDataset( objCnn.Conectar(), CommandType.StoredProcedure, "Novedades_TNoAplicadas", objPar );
				return ds;
			} 
			catch(SqlException ErrSQL)
			{
				
				if ( ErrSQL.Number == 1205 || ErrSQL.Number == 1204 ) 
				{
					throw new ApplicationException("Interbloqueo");	
				}
				else
				{
					throw ErrSQL;
				}

			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				ds.Dispose();
				objCnn=null;
			}
		}
	
		#endregion

		#region Novedades_Traer_X_Id
		
		[SecurityRole("OperadorEntidad")]
		public DataSet  Novedades_Traer_X_Id ( long idNovedad)
		{
			DataSet ds = new DataSet();
		
			Conexion objCnn = new Conexion();
				
			SqlParameter [] objPar = new SqlParameter[1];
	
			try
			{
						
				objPar[0] = new SqlParameter("@IdNovedad",SqlDbType.BigInt); 
				objPar[0].Value = idNovedad;
					
	
				ds=SqlHelper.ExecuteDataset( objCnn.Conectar(), CommandType.StoredProcedure, "Novedades_TxIdNovedad", objPar );

				
				return ds;


			} 
			catch(SqlException ErrSQL)
			{
				
				if ( ErrSQL.Number == 1205 || ErrSQL.Number == 1204 ) 
				{
					throw new ApplicationException("Interbloqueo");	
				}
				else
				{
					throw ErrSQL;
				}

			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				ds.Dispose();
				objCnn=null;
			}

		}
		#endregion

		#region Novedades_TxIdNovedad_Sliq
		
		[SecurityRole("OperadorEntidad")]
		public DataSet  Novedades_TxIdNovedad_Sliq ( long idNovedad)
		{
			DataSet ds = new DataSet();
		
			Conexion objCnn = new Conexion();
				
			SqlParameter [] objPar = new SqlParameter[1];
	
			try
			{
						
				objPar[0] = new SqlParameter("@IdNovedad",SqlDbType.BigInt); 
				objPar[0].Value = idNovedad;
					
	
				ds=SqlHelper.ExecuteDataset( objCnn.Conectar(), CommandType.StoredProcedure, "Novedades_TxIdNovedad_Sliq", objPar );

				
				return ds;


			} 
			
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				ds.Dispose();
				objCnn=null;
			}

		}
		#endregion
		
		#region Novedades_Trae_Creditos_Activos

		[SecurityRole("OperadorEntidad")]
		public DataSet  Novedades_Trae_Creditos_Activos(long Prestador,long Beneficiario)
		{
			DataSet ds = new DataSet();
			
			Conexion objCnn = new Conexion();
			SqlParameter [] objPar = new SqlParameter[2];
	
			try
			{
				objPar[0] = new SqlParameter("@Prestador",SqlDbType.BigInt);
				objPar[0].Value = Prestador;
				objPar[1] = new SqlParameter("@Beneficiario",SqlDbType.BigInt);
				objPar[1].Value = Beneficiario;
				
				ds=SqlHelper.ExecuteDataset( objCnn.Conectar(), CommandType.StoredProcedure, "Novedades_Tipo3_Vigentes", objPar );
				return ds;
			} 
			catch(SqlException ErrSQL)
			{
				
				if ( ErrSQL.Number == 1205 || ErrSQL.Number == 1204 ) 
				{
					throw new ApplicationException("Interbloqueo");	
				}
				else
				{
					throw ErrSQL;
				}

			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				ds.Dispose();
				objCnn=null;
			}
		}
		#endregion

		#region Novedades_T1o6_Trae

		[SecurityRole("OperadorEntidad")]
		public DataSet Novedades_T1o6_Trae(long Prestador,byte TipoConcepto)
		{
			
			DataSet ds = new DataSet();
			Conexion objCnn = new Conexion();
			SqlParameter [] objPar = new SqlParameter[2];
	
			try
			{
			//	ds = ds.BeginInit();
				if (TipoConcepto == 1 || TipoConcepto == 6)
				{

					objPar[0] = new SqlParameter("@Prestador",SqlDbType.BigInt);
					objPar[0].Value = Prestador;
					objPar[1] = new SqlParameter("@TipoConcepto",SqlDbType.TinyInt);
					objPar[1].Value = TipoConcepto;
				
					ds=SqlHelper.ExecuteDataset( objCnn.Conectar(), CommandType.StoredProcedure, "Novedades_T1o6_Trae", objPar );

				}
				
				return ds;
			} 
			
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				ds.Dispose();
				objCnn=null;
			}
		}
		#endregion

		#region Novedades_Trae_Xa_ABM

		[SecurityRole("OperadorEntidad")]
		public DataSet Novedades_Trae_Xa_ABM(long Prestador, long Beneficiario)
		{
			
			DataSet ds = new DataSet();
			Conexion objCnn = new Conexion();
			SqlParameter [] objPar = new SqlParameter[2];
	
			try
			{
					objPar[0] = new SqlParameter("@Prestador",SqlDbType.BigInt);
					objPar[0].Value = Prestador;
					objPar[1] = new SqlParameter("@Beneficiario",SqlDbType.BigInt);
					objPar[1].Value = Beneficiario;
				
					ds=SqlHelper.ExecuteDataset( objCnn.Conectar(), CommandType.StoredProcedure, "Novedades_TT_Xa_ABM", objPar );

				
				
				return ds;
			} 
			
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				ds.Dispose();
				objCnn=null;
			}
		}
		#endregion

		#region Novedades_BajaTxIDNov_FecBaja
		
		[SecurityRole("OperadorEntidad")]
		public DataSet  Novedades_BajaTxIDNov_FecBaja( long IdNov, string FechaBaja )
		{
			DataSet ds = new DataSet();
		
			Conexion objCnn = new Conexion();

			try
			{
						
				ds= SqlHelper.ExecuteDataset(objCnn.ConectarString(),"Novedades_BajaTxIdNov_FecBaja",IdNov,FechaBaja);
				
				return ds;

			} 
			
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				ds.Dispose();
				objCnn=null;
			}

		}
		#endregion

		#region Novedades de baja Traer

		[SecurityRole("OperadorEntidad")]
		public DataSet  Novedades_BajaTraer( long IDPrestador, byte OpcionBusqueda, long BenefCuil, byte TipoConc, int ConcOpp )
		{
			DataSet ds = new DataSet();
			
			Conexion objCnn = new Conexion();
	
			try
			{
				
				ds = SqlHelper.ExecuteDataset( objCnn.ConectarString(),"Novedades_BajaT", IDPrestador,  OpcionBusqueda,  BenefCuil,  TipoConc,  ConcOpp ) ;

				return ds;

			} 
			
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				ds.Dispose();
				objCnn=null;
			}
		}
		#endregion

		#region Novedades de baja Traer Agrupada		
		private DataSet  Novedades_BajaTraerAgrupadaConsulta( long IDPrestador, byte OpcionBusqueda, long BenefCuil, byte TipoConc, int ConcOpp,string MesAplica )
		{
			DataSet ds = new DataSet();
			
			Conexion objCnn = new Conexion();
	
			try
			{
				
				ds = SqlHelper.ExecuteDataset( objCnn.ConectarString(),"Novedades_BajaT_Agrupadas", IDPrestador,  OpcionBusqueda,  BenefCuil,  TipoConc,  ConcOpp,MesAplica ) ;

				return ds;

			} 
			catch(SqlException ErrSQL)
			{
				
				if ( ErrSQL.Number == 1205 || ErrSQL.Number == 1204 ) 
				{
					throw new ApplicationException("Interbloqueo");	
				}
				else
				{
					throw ErrSQL;
				}

			}
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				ds.Dispose();
				objCnn=null;
			}
		}
			
		[SecurityRole("OperadorEntidad")]
		public DataSet  Novedades_BajaTraerAgrupada( long IDPrestador, byte OpcionBusqueda, long BenefCuil, byte TipoConc, int ConcOpp,string MesAplica,bool GeneraArchivo, out string rutaArchivoSal,string Usuario )
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
						/*StringBuilder cabecera = new StringBuilder();
						cabecera.Append ( "cuil"+separador);
						cabecera.Append ( "IdNovedad"+separador);
						cabecera.Append ( "IdBeneficiario"+separador);
						cabecera.Append ( "ApellidoNombre"+separador);
						cabecera.Append ( "FecNov"+separador);
						cabecera.Append ( "HoraNov"+separador);
						cabecera.Append ( "TipoConcepto"+separador);
						cabecera.Append ( "DescTipoConcepto"+separador);
						cabecera.Append ( "CodConceptoLiq"+separador);
						cabecera.Append ( "DescConceptoLiq"+separador);
						cabecera.Append ( "ImporteTotal"+separador);
						cabecera.Append ( "CantCuotas"+separador);
						cabecera.Append ( "Porcentaje"+separador);
						cabecera.Append ( "NroComprobante"+separador);
						cabecera.Append ( "MAC"+separador);
						cabecera.Append ( "ImporteCuota"+separador);
						cabecera.Append ( "IdEstadoReg"+separador);
						cabecera.Append ( "DescripcionEstadoReg"+separador);
						cabecera.Append ( "ImporteLiq"+separador);
						cabecera.Append ( "IdEstadoNov"+separador);
						cabecera.Append ( "Documento"+separador);
						cabecera.Append ( "Stock"+separador);
						cabecera.Append ( "FechaEliminacion"+separador);
						cabecera.Append ( "CodMovimiento"+separador);
						cabecera.Append ( "FechaOrder");
						
						sw.WriteLine(cabecera.ToString());																		
						*/
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
						rta = new ConsultasBatch().AltaNuevaConsulta(IDPrestador,nombreConsulta,0,OpcionBusqueda,MesAplica,TipoConc,ConcOpp,BenefCuil,string.Empty,string.Empty,rutaArchivo,nombreArchivo,fGeneracion,true, Usuario);

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
				if (errsql.Number == -2)
				{
					nombreArchivo = Utilidades.GeneraNombreArchivo(nombreConsulta,IDPrestador,out rutaArchivo);
					rta = new ConsultasBatch().AltaNuevaConsulta(IDPrestador,nombreConsulta,0,OpcionBusqueda,MesAplica,TipoConc,ConcOpp,BenefCuil,string.Empty,string.Empty,rutaArchivo,nombreArchivo,string.Empty,false,Usuario);
					
					throw new ApplicationException("MSG_ERROR Generando el archivo. Reingrese a la consulta en unos minutos.FIN_MSG_ERROR");
				}
				else
				{
					throw errsql;	
				}

			}
			catch( ApplicationException apperr)
			{
				throw new ApplicationException(apperr.Message) ;						
			}
			catch( Exception err)
			{
				throw err ;						
			}
			finally
			{
			
			
			}	
		}
		#endregion

		#region Trae cuota social por cuil

		[SecurityRole("OperadorEntidad")]
		public DataSet CuotaSocial_TraeXCuil(long idbeneficiario, long idPrestador)
		{
			DataSet ds = new DataSet();
			
			Conexion objCnn = new Conexion();

			SqlParameter [] objPar = new SqlParameter[2];
	
			try
			{				
				objPar[0] = new SqlParameter("@idbeneficiario",SqlDbType.BigInt); 
				objPar[0].Value = idbeneficiario;
				objPar[1] = new SqlParameter("@idPrestador",SqlDbType.BigInt); 
				objPar[1].Value = idPrestador;

				
				ds = SqlHelper.ExecuteDataset( objCnn.Conectar(),CommandType.StoredProcedure,"CuotaSocial_TraeXCuil", objPar) ;

				return ds;

			} 
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				ds.Dispose();
				objCnn=null;
			}
		}
		#endregion

	}
	
	#endregion
	
}

