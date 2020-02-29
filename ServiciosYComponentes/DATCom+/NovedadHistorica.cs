using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Reflection;
using System.Configuration;
using System.EnterpriseServices;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime;
using System.IO;
using System.Text;
using System.Collections;


namespace ANSES.Microinformatica.DATComPlus
{
	[Guid("ACE9C4A6-4905-4ba1-AD0C-5F9741454AF0")]
	public interface DAT_INovedadHistorica
	{
		DataSet NovedadHistorica_Trae(byte criterio, byte opcion, long prestador, long benefCuil, byte tipoConc, int concopp, string fecCierre,bool GeneraArchivo, out string rutaArchivoSal, string Usuario );
		
	}

	[Guid("0F687E75-D802-42b9-BD6A-F6E2449D8074")] 
	[ ProgId( "DAT_NovedadHistorica" ) ]
	[ClassInterface(ClassInterfaceType.None)]
	[ ObjectPooling( MinPoolSize = 3) ]
	[JustInTimeActivation(true)]
	[Transaction(TransactionOption.Disabled)]
	[ComponentAccessControl(true)]
	[EventTrackingEnabled ( true )]
	[SecureMethod]
	public class NovedadHistorica:ServicedComponent,DAT_INovedadHistorica
	{
		
		public NovedadHistorica ()
		{

		}

		#region DataSet NovedadHistorica_Trae_Consulta(byte criterio, byte opcion, long Prestador, 	long benefCuil, byte tipoConc, int concopp, string fecCierre)
		private DataSet NovedadHistorica_Trae_Consulta(byte criterio, byte opcion, long Prestador, 	long benefCuil, byte tipoConc, int concopp, string fecCierre)
		{
			DataSet ds = new DataSet();
			
			SqlConnection oCnn = new SqlConnection();
			SqlCommand cmd = new SqlCommand();
	
			try
			{
				/*Criterio: 
				 *		1 Descontado
				 *		2 Descuento Parcial
				 *		3 No Descontado
				 * */

				oCnn = new Conexion().Conectar();
				
				oCnn.Open();			
				cmd.Connection = oCnn;			
				cmd.CommandText = "Novedades_Historica_Trae";
				cmd.CommandType = CommandType.StoredProcedure;
				//cmd.CommandTimeout = 1000;

				cmd.Parameters.Add("@Criterio",SqlDbType.TinyInt).Value = criterio;
				cmd.Parameters.Add("@Opcion",SqlDbType.TinyInt).Value = opcion;
				cmd.Parameters.Add("@Prestador",SqlDbType.BigInt).Value = Prestador;
				cmd.Parameters.Add("@BenefCuil",SqlDbType.BigInt).Value = benefCuil;
				cmd.Parameters.Add("@TipoConc",SqlDbType.TinyInt).Value = tipoConc;
				cmd.Parameters.Add("@Conc",SqlDbType.Int).Value = concopp;
				cmd.Parameters.Add("@FechaCierre",SqlDbType.Int).Value = fecCierre;
				

				SqlDataAdapter dta = new SqlDataAdapter(cmd);
				dta.Fill(ds);

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
				//objCnn=null;
			}
		}
		#endregion
		
		#region public DataSet NovedadHistorica_Trae(byte criterio, byte opcion, long prestador, long benefCuil, byte tipoConc, int concopp, string fecCierre, out string rutaArchivoSal, string Usuario )
		
		[SecurityRole("OperadorEntidad")]	
		public DataSet NovedadHistorica_Trae(byte criterio, byte opcion, long prestador, long benefCuil, byte tipoConc, int concopp, string fecCierre,bool GeneraArchivo, out string rutaArchivoSal, string Usuario )
		{
			string rutaArchivo = string.Empty;
			string nombreArchivo = string.Empty;
			rutaArchivoSal =string.Empty;
			string rta =string.Empty;
			string nombreConsulta = "NovedadesLiquidadas";
			try
			{
				if (opcion != 1 || GeneraArchivo== true)
				{								 
					rta = new ConsultasBatch().ExisteConsulta ( prestador, nombreConsulta, criterio, opcion, fecCierre,tipoConc, concopp,benefCuil,string.Empty ,string.Empty );
					if ( rta != string.Empty)
					{
						throw new ApplicationException("MSG_ERROR"+rta+"FIN_MSG_ERROR");
					}
				}

				DataSet ds = NovedadHistorica_Trae_Consulta(criterio,opcion,prestador,benefCuil,tipoConc,concopp,fecCierre);

				if ((ds.Tables[0].Rows.Count != 0) && (opcion != 1  || GeneraArchivo== true))
				{
					int maxCantidad =new Conexion().MaxCantidadRegistros();
				
					if (ds.Tables[0].Rows.Count>=maxCantidad || GeneraArchivo== true)
					{
						nombreArchivo = Utilidades.GeneraNombreArchivo(nombreConsulta,prestador,out rutaArchivo);

						StreamWriter sw = new StreamWriter(rutaArchivo + nombreArchivo,false,System.Text.Encoding.UTF8);
						string separador = new Conexion().DelimitadorCampo();

						
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
						cabecera.Append ( "NroCuotaLiq"+separador);
						cabecera.Append ( "PeriodoLiq"+separador);
						cabecera.Append ( "ImporteCuota"+separador);											
						cabecera.Append ( "ImporteALiq"+separador);
						cabecera.Append ( "ImporteLiq"+separador);															
						cabecera.Append ( "NroComprobante"+separador);
						cabecera.Append ( "MAC"+separador);
						cabecera.Append ( "Usuario"+separador);								
						cabecera.Append ( "Stock");
						
						sw.WriteLine(cabecera.ToString());		
						*/
						
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
							linea.Append ( fila["NroCuotaLiq"].ToString()+separador);
							linea.Append ( fila["PeriodoLiq"].ToString()+separador);
							linea.Append ( fila["ImporteCuota"].ToString().Replace(",",".")+separador);
							linea.Append ( fila["ImporteALiq"].ToString().Replace(",",".")+separador);
							linea.Append ( fila["ImporteLiq"].ToString().Replace(",",".")+separador);															
							linea.Append ( fila["NroComprobante"].ToString()+separador);
							linea.Append ( fila["MAC"].ToString()+separador);
							linea.Append ( fila["Usuario"].ToString()+separador);								
							linea.Append ( fila["Stock"].ToString().ToString());
														
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
						rta = new ConsultasBatch().AltaNuevaConsulta(prestador,nombreConsulta,criterio,opcion,fecCierre,tipoConc,concopp,benefCuil,string.Empty,string.Empty,rutaArchivo,nombreArchivo,fGeneracion,true, Usuario);

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
					nombreArchivo = Utilidades.GeneraNombreArchivo(nombreConsulta,prestador,out rutaArchivo);
					rta = new ConsultasBatch().AltaNuevaConsulta(prestador,nombreConsulta,criterio,opcion,fecCierre,tipoConc,concopp,benefCuil,string.Empty,string.Empty,rutaArchivo,nombreArchivo,string.Empty,false, Usuario);
					
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
	
		
			
	
	}

	
}