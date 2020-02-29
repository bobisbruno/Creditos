using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Web;
using System.Web.Services;
using System.EnterpriseServices;
using ANSES.Microinformatica.DATComPlus;


namespace ANSES.Microinformatica.DATws
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class NovedadWS : System.Web.Services.WebService
	{
		public NovedadWS()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
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
		protected override void Dispose( bool disposing )
		{
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		#region Alta Novedad 
		//[WebMethod (MessageName="Alta_de_Novedad")]
		[WebMethod]
		public string Novedades_Alta(long IdPrestador, long IdBeneficiario, 
			byte TipoConcepto, int ConceptoOPP,  double ImpTotal, byte CantCuotas,Single Porcentaje,	 
			string  NroComprobante, string IP, string Usuario,int Mensual)
		{
			Novedad_Trans oNov  = new Novedad_Trans();
		
			try
			{
				return  (oNov.Novedades_Alta(IdPrestador, IdBeneficiario, TipoConcepto, ConceptoOPP, ImpTotal,
					CantCuotas, Porcentaje,NroComprobante,IP, Usuario,Mensual));
			
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
			}
		}
		#endregion

		#region Modificacion Novedad 
		//[WebMethod (MessageName="Modificacion_de_Novedad")]
		[WebMethod]
		public string Novedades_Modificacion(long IdNovedadAnt, double ImpTotalN, Single PorcentajeN, string  NroComprobanteN, 
			string IPN, string UsuarioN,int Mensual,bool Masiva)
		{
			Novedad_Trans oNov  = new Novedad_Trans();
		
			try
			{			
				return  oNov.Novedades_Modificacion(IdNovedadAnt, ImpTotalN, PorcentajeN, NroComprobanteN, 
					IPN, UsuarioN,Mensual,Masiva);
			
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				
			}
		}
		#endregion

		#region Baja Novedad 
		//[WebMethod (MessageName="Baja_de_Novedad")]
		[WebMethod]
		public string Novedades_Baja(long IdNovedadAnt, string IP, string Usuario,int Mensual)
		{
			Novedad_Trans oNov  = new Novedad_Trans();
		
			try
			{
			
				return  (oNov.Novedades_Baja(IdNovedadAnt, IP, Usuario, Mensual));			
			
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				
			}
		}
		#endregion

		#region Novedades_Baja_Cuotas 
		//[WebMethod (MessageName="Novedades Baja Cuotas")]
		[WebMethod]
		public string Novedades_Baja_Cuotas(DataSet CuotasABajar, string Ip, string Usuario )
			{
				Novedad_Trans oNov  = new Novedad_Trans();
		
				try
				{		
					return  (oNov.Novedades_Baja_Cuotas(CuotasABajar,  Ip, Usuario));			
			
				}
				catch (Exception err)
				{
					throw err;
				}
				finally
				{
				}
			}
		#endregion

		#region Modificacion_Masiva_Indeterminadas
		//[WebMethod (MessageName="Modificacion_Masiva_Indeterminadas")]
		[WebMethod]			
		public DataSet Modificacion_Masiva_Indeterminadas(DataSet NovAMod,double Monto, string Ip, string Usuario,bool Masiva )
		{
			Novedad_Trans oNov  = new Novedad_Trans();
		
			try
			{		
				return  (oNov.Modificacion_Masiva_Indeterminadas(NovAMod,Monto,Ip,Usuario,Masiva ));
			
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
			}
		}
		#endregion

		#region Novedades_Traer_Todos
		//[WebMethod (MessageName="Novedades_Trae_Todas")]
		[WebMethod]
		public DataSet  Novedades_TraerTodos(long lintPrestador)
		{
			Novedad oNov = new Novedad();
			try
			{
				DataSet ds = new DataSet();
				ds = oNov.Novedades_TraerTodos(lintPrestador);
				return ds;
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
			}
		}
		#endregion		

		#region Novedades Traer 
		//[WebMethod (MessageName="Novedades_Trae")]
		[WebMethod]
		public DataSet Novedades_Traer(byte opcion, long lintPrestador, long benefCuil, byte tipoConc, int concopp,int mensual, string fdesde, string fhasta,bool GeneraArchivo, out string rutaArchivoSal)

		{
			Novedad oNov = new Novedad();
			try
			{
				return oNov.Novedades_Traer(opcion, lintPrestador, benefCuil,tipoConc, concopp,mensual,fdesde,fhasta,GeneraArchivo, out rutaArchivoSal );
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				
			}
		}
		#endregion		

		#region Novedades_Traer_X_IdNovedad
		//[WebMethod (MessageName="Novedades_Traer_X_IdNovedad")]
		[WebMethod]
		public DataSet Novedades_Traer_X_Id ( long idNovedad)

		{
			Novedad oNov = new Novedad();
			DataSet ds  = new DataSet();
			try
			{
				ds =oNov.Novedades_Traer_X_Id (idNovedad);
				return  ds;
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				ds.Dispose();
				
				
			}
		}
		#endregion		

		#region Novedades Trae Novedades no Aplicadas
		//[WebMethod (MessageName="Novedades_Trae_No_Aplicadas")]
		//public DataSet  Novedades_Trae_NoAplicadas(byte opcion, long Prestador,long benefCuil, byte tipoConc, int concopp )
		[WebMethod]
		public DataSet  Novedades_Trae_NoAplicadas(byte OpcionBusqueda, long idPrestador, long benefCuil, byte tipoConc, int concopp,string fdesde, string fhasta, bool GeneraArchivo, int mensual,out string rutaArchivoSal)

		{
			Novedad oNov = new Novedad();
			try
			{
				//return oNov.Novedades_Trae_NoAplicadas(opcion, Prestador, benefCuil,tipoConc, concopp );
				return oNov.Novedades_Trae_NoAplicadas(OpcionBusqueda, idPrestador, benefCuil, tipoConc, concopp,fdesde, fhasta, GeneraArchivo,mensual,out rutaArchivoSal);
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				
			}
		}
		#endregion	
	
		#region Novedades Trae Creditos Activos
		//[WebMethod (MessageName="Novedades_Creditos_Activos")]
		[WebMethod]
		public DataSet  Novedades_Trae_Creditos_Activos(long Prestador,long Beneficiario )

		{
			Novedad oNov = new Novedad();
			try
			{
				return oNov.Novedades_Trae_Creditos_Activos(Prestador,Beneficiario);
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				
			}
		}
		#endregion	

		#region Novedades_T1o6_Trae
		//[WebMethod (MessageName="Novedades_T1o6_Trae")]
		[WebMethod]
		public DataSet Novedades_T1o6_Trae(long Prestador,byte TipoConcepto)

		{
			Novedad oNov = new Novedad();
			try
			{
				return oNov.Novedades_T1o6_Trae(Prestador,TipoConcepto);
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				
			}
		}
		#endregion	
	
		#region Novedades_Trae_Xa_ABM
		//[WebMethod (MessageName="Novedades_Trae_Xa_ABM")]
		[WebMethod]
		public DataSet Novedades_Trae_Xa_ABM(long Prestador, long Beneficiario)

	{
		Novedad oNov = new Novedad();
		try
		{
			return oNov.Novedades_Trae_Xa_ABM(Prestador, Beneficiario);
		}
		catch (Exception err)
		{
			throw err;
		}
		finally
		{
			
		}
	}
		#endregion	

		#region Novedades de Baja Traer x IDNov y FecBaja
		//[WebMethod (MessageName="Novedades Bajas Traer X IdNov y Fecha de Baja")]
		[WebMethod]
		public DataSet Novedades_BajaTxIDNov_FecBaja ( long IdNov, string FechaBaja ) 
		{
			Novedad oNov = new Novedad();
			DataSet ds  = new DataSet();
			try
			{
				ds =oNov.Novedades_BajaTxIDNov_FecBaja ( IdNov, FechaBaja ) ;
				return  ds;
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				ds.Dispose();
				
				
			}
		}
		#endregion		

		#region Novedades de Baja Traer 
		//[WebMethod (MessageName="Novedades de Baja Traer")]
		[WebMethod]
		public DataSet Novedades_BajaTraer( long IDPrestador, byte OpcionBusqueda, long BenefCuil, byte TipoConc, int ConcOpp )
		{

			Novedad oNov = new Novedad();
			try
			{
				return oNov.Novedades_BajaTraer(  IDPrestador,  OpcionBusqueda,  BenefCuil,  TipoConc,  ConcOpp );
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				
			}

		}
		#endregion		

		#region Novedades de Baja Traer Agrupadas
		//[WebMethod (MessageName="Novedades de Baja Traer Agrupadas")]
		[WebMethod]
        public DataSet Novedades_BajaTraerAgrupadas( long IDPrestador, byte OpcionBusqueda, long BenefCuil, byte TipoConc, int ConcOpp,string MesAplica,bool GeneraArchivo, out string rutaArchivoSal  )
		{

			Novedad oNov = new Novedad();
			try
			{
				return oNov.Novedades_BajaTraerAgrupada(  IDPrestador,  OpcionBusqueda,  BenefCuil,  TipoConc,  ConcOpp,MesAplica,GeneraArchivo,out rutaArchivoSal );
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				
			}

		}
		#endregion		

		#region Control de ocurrencias
		//[WebMethod (MessageName="Control de ocurrencias Para Cancelar Cuotas")]
		[WebMethod]
		public Boolean CtrolOcurrenciasCancCuotas (byte CantOcurrDisp,long IdBeneficiario, int ConceptoOPP, long IdNovedadABorrar)
		{

			Novedad_Trans oNov = new Novedad_Trans();
			try
			{
				return oNov.CtrolOcurrenciasCancCuotas(  CantOcurrDisp,  IdBeneficiario,  ConceptoOPP,IdNovedadABorrar);
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				
			}

		}
		#endregion	

		#region Trae cuota social por cuil
		//[WebMethod (MessageName="CuotaSocial_TraeXCuil")]
		[WebMethod]
		public DataSet CuotaSocial_TraeXCuil(long idbeneficiario, long idPrestador)
		{
			Novedad oNov = new Novedad();
			try
			{
				return oNov.CuotaSocial_TraeXCuil(idbeneficiario, idPrestador);
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				
			}

		}

		#endregion 

		#region Valida Novedad
		//[WebMethod (MessageName="Valido_Nov_T3")]
		[WebMethod]
		public string Valido_Nov_T3(long IdPrestador, long IdBeneficiario, 
			byte TipoConcepto, int ConceptoOPP, 
			double ImpTotal, byte CantCuotas,
			Single Porcentaje, byte CodMovimiento, String NroComprobante,
			string IP, string Usuario,int Mensual,
			decimal montoPrestamo,decimal CuotaTotalMensual,
			decimal TNA,decimal TEM,
			decimal gastoOtorgamiento,decimal gastoAdmMensual,
			decimal cuotaSocial,decimal CFTEA,
			decimal CFTNAReal,decimal CFTEAReal,
			decimal gastoAdmMensualReal,decimal TIRReal)
		{
			Novedad_Trans oNov = new Novedad_Trans();
			try
			{
				return oNov.Valido_Nov_T3(IdPrestador, IdBeneficiario,
					TipoConcepto,ConceptoOPP,ImpTotal, 
					CantCuotas, 0,CodMovimiento, NroComprobante, 
					DateTime.Now, IP, Usuario,Mensual,
					montoPrestamo,CuotaTotalMensual,TNA,TEM,
					gastoOtorgamiento,gastoAdmMensual,cuotaSocial,CFTEA,
					CFTNAReal,CFTEAReal,gastoAdmMensualReal,TIRReal);
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				
			}

			
		}

		//[WebMethod (MessageName="Valido_Nov_T3_Gestion")]
		[WebMethod]
		public string Valido_Nov_T3_Gestion(long IdPrestador, long IdBeneficiario, 
			byte TipoConcepto, int ConceptoOPP, 
			double ImpTotal, byte CantCuotas,
			Single Porcentaje, byte CodMovimiento, String NroComprobante,
			string IP, string Usuario,int Mensual,
			decimal montoPrestamo,decimal CuotaTotalMensual,
			decimal TNA,decimal TEM,
			decimal gastoOtorgamiento,decimal gastoAdmMensual,
			decimal cuotaSocial,decimal CFTEA,
			decimal CFTNAReal,decimal CFTEAReal,
			decimal gastoAdmMensualReal,decimal TIRReal, bool bGestionErrores)
		{
			Novedad_Trans oNov = new Novedad_Trans();
			try
			{
				return oNov.Valido_Nov_T3_Gestion(IdPrestador, IdBeneficiario,
										TipoConcepto,ConceptoOPP,ImpTotal, 
										CantCuotas, 0,CodMovimiento, NroComprobante, 
										DateTime.Now, IP, Usuario,Mensual,
										montoPrestamo,CuotaTotalMensual,TNA,TEM,
										gastoOtorgamiento,gastoAdmMensual,cuotaSocial,CFTEA,
										CFTNAReal,CFTEAReal,gastoAdmMensualReal,TIRReal, bGestionErrores);
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				
			}
		}
		#endregion

		#region Novedades_T3_Alta_ConTasa
		//[WebMethod (MessageName="Novedades_T3_Alta_ConTasa")]
		[WebMethod]
		public string Novedades_T3_Alta_ConTasa(long IdPrestador, long IdBeneficiario, DateTime FecNovedad,  byte TipoConcepto, int ConceptoOPP, 
			double ImpTotal, byte CantCuotas,  string NroComprobante, string IP, string Usuario, int Mensual, byte IdEstadoReg,
			decimal montoPrestamo,decimal CuotaTotalMensual,decimal TNA,decimal TEM,
			decimal gastoOtorgamiento,decimal gastoAdmMensual,decimal cuotaSocial,decimal CFTEA,
			decimal CFTNAReal,decimal CFTEAReal,decimal gastoAdmMensualReal,decimal TIRReal, string XMLCuotas,
			int idItem, string nroFactura, string cbu, string nroTarjeta, string otro, string prestadorServicio, string poliza,
            string nroSocio, int idDomicilioBeneficiario, int idDomicilioPrestador, byte idTipoDocPresentado, DateTime fEntregaTarjeta, bool solicitaTarjetaNominada)
		{
			
			return Novedades_T3_Alta_ConTasa_Sucursal(IdPrestador, IdBeneficiario, FecNovedad, TipoConcepto, ConceptoOPP, 
													ImpTotal, CantCuotas,  NroComprobante, IP, Usuario, Mensual, IdEstadoReg,
													montoPrestamo,CuotaTotalMensual,TNA, TEM,
													gastoOtorgamiento,gastoAdmMensual,cuotaSocial,CFTEA,
													CFTNAReal,CFTEAReal,gastoAdmMensualReal,TIRReal, XMLCuotas,
													idItem, nroFactura, cbu, nroTarjeta, otro, prestadorServicio, poliza,
													nroSocio, string.Empty, idDomicilioBeneficiario,idDomicilioPrestador,
                                                    string.Empty, DateTime.MinValue, DateTime.MinValue, idTipoDocPresentado, fEntregaTarjeta, solicitaTarjetaNominada);
		
		}
		#endregion

		#region Novedades_T3_Alta_ConTasa_Sucursal
		//[WebMethod (MessageName="Novedades_T3_Alta_ConTasa_Sucursal")]
		[WebMethod]
		public string Novedades_T3_Alta_ConTasa_Sucursal(long IdPrestador, long IdBeneficiario, DateTime FecNovedad,  byte TipoConcepto, int ConceptoOPP, 
			double ImpTotal, byte CantCuotas,  string NroComprobante, string IP, string Usuario, int Mensual, byte IdEstadoReg,
			decimal montoPrestamo,decimal CuotaTotalMensual,decimal TNA,decimal TEM,
			decimal gastoOtorgamiento,decimal gastoAdmMensual,decimal cuotaSocial,decimal CFTEA,
			decimal CFTNAReal,decimal CFTEAReal,decimal gastoAdmMensualReal,decimal TIRReal, string XMLCuotas,
			int idItem, string nroFactura, string cbu, string nroTarjeta, string otro, string prestadorServicio, string poliza,
			string nroSocio, string nroTicket, int idDomicilioBeneficiario,int idDomicilioPrestador,
            string nroSucursal, DateTime fVto, DateTime fVtoHabilSiguiente, byte idTipoDocPresentado, DateTime fEntregaTarjeta, bool solicitaTarjetaNominada)
		{
			
			Novedad_Trans oNov = new Novedad_Trans();
			try
			{
				return oNov.Novedades_T3_Alta_ConTasa(IdPrestador, IdBeneficiario, FecNovedad, TipoConcepto, ConceptoOPP, 
					ImpTotal, CantCuotas,  NroComprobante, IP, Usuario, Mensual, IdEstadoReg,
					montoPrestamo,CuotaTotalMensual,TNA,TEM,
					gastoOtorgamiento,gastoAdmMensual,cuotaSocial,CFTEA,
					CFTNAReal,CFTEAReal,gastoAdmMensualReal,TIRReal, XMLCuotas,
					idItem, nroFactura, cbu, nroTarjeta, otro, prestadorServicio, poliza,
					nroSocio, nroTicket, idDomicilioBeneficiario,idDomicilioPrestador,nroSucursal, fVto, fVtoHabilSiguiente, idTipoDocPresentado, fEntregaTarjeta, solicitaTarjetaNominada);
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				
			}
		
		}
		#endregion

		#region Novedades_Rechazadas_A_ConTasas
		//[WebMethod (MessageName="Novedades_Rechazadas_A_ConTasas")]
		[WebMethod]
		public void Novedades_Rechazadas_A_ConTasas(
			long IdBeneficiario,long IdPrestador ,byte CodMovimiento ,byte TipoConcepto ,int CodConceptoLiq ,double ImporteTotal ,
			byte CantCuotas ,Single Porcentaje ,string NroComprobante ,string IP,string Usuario ,DateTime FecRechazo ,
			decimal montoPrestamo,decimal CuotaTotalMensual,decimal TNA,decimal TEM,decimal gastoOtorgamiento,decimal gastoAdmMensual,
			decimal cuotaSocial,decimal CFTEA,decimal CFTNAReal,decimal CFTEAReal,decimal gastoAdmMensualReal,decimal TIRReal, string mensajeError)
		{
			
			Novedad_Trans oNov = new Novedad_Trans();
			try
			{
				oNov.Novedades_Rechazadas_A_ConTasas(IdBeneficiario,IdPrestador ,CodMovimiento ,TipoConcepto ,CodConceptoLiq ,ImporteTotal ,
													CantCuotas ,Porcentaje ,NroComprobante ,IP,Usuario ,FecRechazo ,
													montoPrestamo,CuotaTotalMensual,TNA,TEM,gastoOtorgamiento,gastoAdmMensual,
													cuotaSocial,CFTEA,CFTNAReal,CFTEAReal,gastoAdmMensualReal,TIRReal, mensajeError);
			}
			catch (Exception err)
			{
				throw err;
			}
			finally
			{
				
			}
		
		}
		#endregion

	}
}
