using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Reflection;
using System.Configuration;
using System.EnterpriseServices;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ANSES.Microinformatica.DATComPlus
{
	/// <summary>
	/// Summary description for EntidadPrivada.
	/// </summary>
	
	[Guid("C594E024-A929-4bcf-AEF1-8076B960BF88")]
	public interface IentidadPrivada
	{
		//long getIdPrestador();
		bool verDisponible(long idPrestador, long idBeneficiario, double monto, int mensualDesde, int mensualHasta);
		String[] altaDeNovedad(long idPrestador,long idBeneficiario, double monto, int mensualDesde,int mensualHasta, 
			string IP, string usuario);
		String[] modificarNovedad(long idPrestador,long nroDeTransaccion, long idBeneficiario, double monto, int mensualDesde,
			int mensualHasta, string IP, string usuario);
		void cancelarNovedad(long idPrestador,long nroDeTransaccion, long idBeneficiario, int mensualHasta);
		DataSet buscoNovedadesDe(long idPrestador,long idBeneficiario);
		DataSet buscoNovedadesPorFechas(long idPrestador,int mensual);
		DataSet buscoNovedadesPorTipo(long idPrestador,int tipo);
	}
	
	[Guid("772E4B4C-4036-4d68-B760-1822BEF6AF9D")] 
	[ ProgId( "EntidadPrivada" ) ]
	[ClassInterface(ClassInterfaceType.None)]
	[ ObjectPooling( MinPoolSize = 10) ]
	[JustInTimeActivation(true)]
	[Transaction(TransactionOption.Disabled)]
	[EventTrackingEnabled ( true )]
	[ComponentAccessControl(true)]
	[SecureMethod]

	public class EntidadPrivada : ServicedComponent,IentidadPrivada
	{
		//private long idPrestador;

		/*public EntidadPrivada(long idPrestador)
		{
			this.setIdPrestador(idPrestador);
		}*/
		public EntidadPrivada()
		{
			
		}
		/*public long getIdPrestador()
		{
			return idPrestador;
		}
		
		private void setIdPrestador(long idPrestador)
		{
			this.idPrestador=idPrestador;
		}*/

		[SecurityRole("OperadorEntidad")]
		public bool verDisponible(long idPrestador,long idBeneficiario, double monto, int mensualDesde, int mensualHasta)
		{
			string resultado;
			int cuotas;
			double importe;
			FechaMensual desde=new FechaMensual(mensualDesde);
			FechaMensual hasta=new FechaMensual(mensualHasta);
			//Creo un objeto tipo novedad para reutilizar el codigo ya existente de verificacion de disponibilidad
			
			
			Novedad_Trans nov = new Novedad_Trans();
			
			
			try
			{
				//VALIDO LOS DATOS INGRESADOS
				validoDatos(desde,hasta);
				
				//SACO LA CANTIDAD DE CUOTAS POR LA DIFERENCIAS DE LOS MENSUALES, EN MESES
				cuotas=desde.cantidadDeMesesCon(hasta);
				importe=monto/cuotas;
				
				//El metodo de verificacion de disponibilidad retorna un string informando el resultado
				resultado=nov.CtrolAlcanza(idBeneficiario,importe,idPrestador,0);			
			
				
				if (resultado==String.Empty)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			catch (NoValida e)
			{
				//RETORNO MENSAJE DE ERROR
				throw e;
			}
		}

		[SecurityRole("OperadorEntidad")]
		public String[] altaDeNovedad(long idPrestador,long idBeneficiario, double monto, int mensualDesde,int mensualHasta, 
			string IP, string usuario)
		{
			FechaMensual desde=new FechaMensual(mensualDesde);
			FechaMensual hasta=new FechaMensual(mensualHasta);
			// ME FIJO EL ULTIMO MENSUAL DE INHIBICION CARGADO, CASO DE Q NO TENGA NINGUNO RETORNA 0
			FechaMensual ultimoHasta=new FechaMensual(this.ultimoMensualDeInhibicion(idPrestador,idBeneficiario));
			Conexion objCnn = new Conexion();	
			SqlConnection objCon = new SqlConnection();
			SqlParameter [] objPar = new SqlParameter[9];
			String[] retorno = new String[2];
			//string MAC;
			
			try
			{
				//VALIDO LOS DATOS INGRESADOS
				validoDatos(desde,hasta);
				if (this.verDisponible(idPrestador, idBeneficiario, monto, mensualDesde, mensualHasta))
				{
					//genero la mac ****FALTA*****
			
					objCon = objCnn.Conectar();
					objPar[0] = new SqlParameter("@IdPrestador",SqlDbType.BigInt);
					objPar[0].Value = idPrestador;
	
					objPar[1] = new SqlParameter("@IdBeneficiario",SqlDbType.BigInt); 
					objPar[1].Value = idBeneficiario;

					objPar[2] = new SqlParameter("@monto",SqlDbType.Decimal);
					objPar[2].Value = monto;
			
					objPar[3] = new SqlParameter("@desde",SqlDbType.BigInt);
					objPar[3].Value = mensualDesde;

					objPar[4] = new SqlParameter("@hasta",SqlDbType.BigInt);
					objPar[4].Value = mensualHasta;
				
					objPar[5]  = new SqlParameter("@IP",SqlDbType.VarChar,20);
					objPar[5].Value = IP;
							
					objPar[6]  = new SqlParameter("@Usuario",SqlDbType.VarChar,50);
					objPar[6].Value = usuario;

					objPar[7]  = new SqlParameter("@MAC",SqlDbType.VarChar,100);
					objPar[7].Value = "";

					objPar[8]  = new SqlParameter("@IdNovedad",SqlDbType.BigInt);
					objPar[8].Direction = ParameterDirection.Output;
					objPar[8].Value = 0;

					//DOY DE ALTA EL REGISTRO EN LA BASE DE DATOS
					SqlHelper.ExecuteNonQuery(objCnn.ConectarString(), CommandType.StoredProcedure, "Novedades_A_EntidadesPrivadas", objPar);
				
					//LLAMO AL COMTI PARA LA TRANSACCION CICS SOLO SI EL MENSUAL HASTA ES MAYOR AL ULTIMO MENSUAL CARGADO
					if (ultimoHasta.esMenorQue(hasta))
					{
						//DOY ALTA EN EL COMTI CON EL MENSUAL Q ME VINO POR PARAMETRO
					
					}

					//RETORNO EL NUMERO DE TRANSACCION Y LA MAC
					retorno[0]=(string)objPar[8].Value.ToString();
					retorno[1] =(string) "";
					return retorno;				
				}
				else
				{
					throw new NoValida("Afectacion disponible insuficiente");
				}
				
			}
			catch (NoValida e)
			{
				//RETORNO MENSAJE DE ERROR
				throw e;
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

		[SecurityRole("OperadorEntidad")]
		public String[] modificarNovedad(long idPrestador,long nroDeTransaccion, long idBeneficiario, double monto, int mensualDesde,int mensualHasta, string IP, string usuario)
		{
			// LA MODIFICACION CONSISTE EN DAR DE BAJA LA TRANSACCION ACTUAL EN LA BASE DE DATOS Y DAR UN ALTA CON LOS NUEVOS DATOS INGRESADOS
			String[] retorno = new String[2];

			try
			{
				this.cancelarNovedad(idPrestador,nroDeTransaccion,idBeneficiario,mensualHasta);
				return this.altaDeNovedad(idPrestador,idBeneficiario,monto,mensualDesde,mensualHasta,IP,usuario);
			}
			catch(NoValida e)
			{
				//RETORNO MENSAJE DE ERROR
				throw e;
			}
			catch(Exception err)
			{
				throw err ;
			}
		}

		[SecurityRole("OperadorEntidad")]
		public void cancelarNovedad(long idPrestador,long nroDeTransaccion, long idBeneficiario, int mensualHasta)
		{
			Conexion objCnn = new Conexion();	
			SqlConnection objCon = new SqlConnection();
			SqlParameter [] objPar = new SqlParameter[1];
			FechaMensual ultimoMensual;
			FechaMensual hasta=new FechaMensual(mensualHasta);

			try
			{
				objCon = objCnn.Conectar();
				objPar[0] = new SqlParameter("@transaccion",SqlDbType.BigInt);
				objPar[0].Value = nroDeTransaccion;

				//DOY DE BAJA EL REGISTRO EN LA BASE DE DATOS
				SqlHelper.ExecuteNonQuery(objCnn.ConectarString(), CommandType.StoredProcedure, "Novedades_BAJA_EntidadesPrivadas", objPar);
				//AVERIGUO EL ULTIMO MENSUAL ACTIVO CARGADO EN LA BASE, SI NO HAY RETORNO 0
				ultimoMensual=new FechaMensual(this.ultimoMensualDeInhibicion(idPrestador,idBeneficiario));
				//LAMO AL COMTI PARA LA TRANSACCION CICS SOLO SI HAY OTRA NOVEDAD CON MENSUAL HASTA MENOR 
				if (ultimoMensual.esMenorQue(hasta))
				{
					//LAMO AL COMTI PARA DER DE BAJA EL ACTUAL 
					//ME FIJO SI HAY UN MENSUAL ANTERIOR ACTIVO CARGADO EN LA BASE
					if (!ultimoMensual.esMensualNulo())
					{
						//LLAMO AL COMTI PARA DAR EL ALTA CON EL MENSUAL ULTIMOMENSUAL, ULTIMO MENSUAL ACTIVO CARGADO EN LA BASE DE DATOS
					}
				}
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

		[SecurityRole("OperadorEntidad")]
		public DataSet buscoNovedadesDe(long idPrestador,long idBeneficiario)
		{
			Conexion objCnn = new Conexion();	
			SqlConnection objCon = new SqlConnection();
			SqlParameter[] objPar= new SqlParameter[2];
			DataSet novedades= new DataSet();
			
			try
			{
				objCon = objCnn.Conectar();
				objPar[0]=new SqlParameter("@idPrestador",SqlDbType.BigInt);
				objPar[0].Value=idPrestador;
				objPar[1]=new SqlParameter("@idBeneficiario",SqlDbType.BigInt);
				objPar[1].Value=idBeneficiario;
				
				//RETORNO UN DATASET CONTENIENDO TODAS LAS NOVEDADES DEL BENEFICIARIO PARA ESTA ENTIDAD PRIVADA
				novedades=SqlHelper.ExecuteDataset(objCnn.ConectarString(),CommandType.StoredProcedure,"BuscoNovedad_EntidadesPrivadas", objPar);
				return novedades;
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
		[SecurityRole("OperadorEntidad")]
		public DataSet buscoNovedadesPorFechas(long idPrestador,int mensual)
		{
			Conexion objCnn = new Conexion();	
			SqlConnection objCon = new SqlConnection();
			SqlParameter[] objPar= new SqlParameter[2];
			DataSet novedades= new DataSet();
			FechaMensual fecha=new FechaMensual(mensual);

			try
			{
				if (fecha.esMensualNulo())
				{
					throw new NoValida("Mensual incorrecto");
				}
				objCon = objCnn.Conectar();
				objPar[0]=new SqlParameter("@idPrestador",SqlDbType.BigInt);
				objPar[0].Value=idPrestador;
				objPar[1]=new SqlParameter("@mensual",SqlDbType.Int);
				objPar[1].Value=mensual;
				
				//RETORNO UN DATASET CONTENIENDO TODAS LAS NOVEDADES ACTIVAS EN EL MENSUAL DEL PARAMETRO
				novedades=SqlHelper.ExecuteDataset(objCnn.ConectarString(),CommandType.StoredProcedure,"BuscoNovedadPorFechas_EntidadesPrivadas", objPar);
				return novedades;
			}
			catch (NoValida e)
			{
				//RETORNO MENSAJE DE ERROR
				throw e;
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
		public DataSet buscoNovedadesPorTipo(long idPrestador,int tipo)
		{
			Conexion objCnn = new Conexion();	
			SqlConnection objCon = new SqlConnection();
			SqlParameter[] objPar= new SqlParameter[1];
			DataSet novedades= new DataSet();
			
			try
			{
				objCon = objCnn.Conectar();
				objPar[0]=new SqlParameter("@idPrestador",SqlDbType.BigInt);
				objPar[0].Value=idPrestador;
	
				switch (tipo)
				{
					case 0:
						//RETORNO UN DATASET CONTENIENDO TODAS LAS NOVEDADES ACTIVAS DEL PRESTADOR
						novedades=SqlHelper.ExecuteDataset(objCnn.ConectarString(),CommandType.StoredProcedure,"BuscoNovedadPorTipoActivas_EntidadesPrivadas", objPar);
						break;
					case 1:
						//RETORNO UN DATASET CONTENIENDO TODAS LAS NOVEDADES FINALIZADAS DEL PRESTADOR
						novedades=SqlHelper.ExecuteDataset(objCnn.ConectarString(),CommandType.StoredProcedure,"BuscoNovedadPorTipoFinalizadas_EntidadesPrivadas", objPar);
						break;
					case 2:
						//RETORNO UN DATASET CONTENIENDO TODAS LAS NOVEDADES CANCELADAS DEL PRESTADOR
						novedades=SqlHelper.ExecuteDataset(objCnn.ConectarString(),CommandType.StoredProcedure,"BuscoNovedadPorTipoCanceladas_EntidadesPrivadas", objPar);
						break;
					default:
						//LANZO UNA EXCEPCION
						throw new Exception ("Tipo erroneo");

				}
				return novedades;
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
		private int ultimoMensualDeInhibicion(long idPrestador,long idBeneficiario)
		{
			Conexion objCnn = new Conexion();	
			SqlConnection objCon = new SqlConnection();
			SqlParameter [] objPar = new SqlParameter[3];
			
			try
			{
				objCon = objCnn.Conectar();
				objPar[0] = new SqlParameter("@idBeneficiario",SqlDbType.BigInt);
				objPar[0].Value = idBeneficiario;
				objPar[1] = new SqlParameter("@idPrestador",SqlDbType.BigInt);
				objPar[1].Value = idPrestador;
				objPar[2]  = new SqlParameter("@ultimoMensual",SqlDbType.BigInt);
				objPar[2].Direction = ParameterDirection.Output;
				objPar[2].Value = 0;
				
				// DEVUELVO EL ULTIMO MENSUAL ACTIVO, CARGADO EN LA BASE DE DATOS PARA EL BENEFICIARIO. SI NO TIENE NINGUNO DEVUELVE EL VALOR 0
				SqlHelper.ExecuteNonQuery(objCnn.ConectarString(), CommandType.StoredProcedure, "UltimoMensual_EntidadesPrivadas", objPar);
				
				return int.Parse(objPar[2].Value.ToString());

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
		private void validoDatos(FechaMensual desde, FechaMensual hasta)
		{
			//ME FIJO SI LA FECHA DESDE ES MENOS Q LA FECHA HASTA Y LAS FECHAS TENGAS MESES VALIDOS
			
			if (desde.esMensualNulo())
			{
				throw new NoValida("Fecha desde invalida");
			}
			if (hasta.esMensualNulo())
			{
				throw new NoValida("Fecha hasta invalida");
			}
			if ((!desde.esMenorQue(hasta)))
			{
				throw new NoValida("Rango de mensuales incorrecto");
			}
		}
	}
}

