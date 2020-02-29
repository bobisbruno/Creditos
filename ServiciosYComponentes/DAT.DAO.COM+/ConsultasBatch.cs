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
using System.Text;
using System.Collections;
using log4net;

namespace ANSES.Microinformatica.DATComPlus
{
	
	public class ConsultasBatch
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(ConsultasBatch).Name); 
       
        public ConsultasBatch()
		{
		}

		#region Traer Consultas x IDPrestador

		
		public DataSet  TraerxIdPrestador ( long idPrestador, string nombreConsulta)
		{
			DataSet ds = new DataSet();
		
			Conexion objCnn = new Conexion();

			String[] oParam = new String[2];

			oParam[0] = idPrestador.ToString();
			oParam[1] = nombreConsulta;

			try
			{				
				ds=SqlHelper.ExecuteDataset( objCnn.ConectarString(), "ConsultasBatch_TxIDPrestador", oParam );
				
				return ds;
			} 			
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err ;
			}
			finally
			{
				ds.Dispose();
				objCnn=null;
			}
		}
		#endregion

		#region Alta de Peticion de consulta

		
		public string  AltaNuevaConsulta ( long IDPrestador, string NombreConsulta, int CriterioLiq, int Opcion, string PeriodoCons,
			 byte TipoConsulta, int CodConceptoLiq,long nroBeneficio,string fechaDesde,string fechaHasta,string rutaArchGenerado, string NomArchGenerado, string fGeneracion, Boolean vigente )
		{
		
			string MsgRetorno = String.Empty;
			string Usuario = String.Empty;
			
			String[] oParam = new String[15];

			Conexion objCnn = new Conexion();

			Usuario =  SecurityCallContext.CurrentCall.OriginalCaller.AccountName ;			

			oParam[0] = IDPrestador.ToString();
			oParam[1] = NombreConsulta;
			oParam[2] = CriterioLiq.ToString();
			oParam[3] = Opcion.ToString();
			oParam[4] = PeriodoCons;
			oParam[5] = TipoConsulta.ToString();
			oParam[6] = CodConceptoLiq.ToString();
			oParam[7] = nroBeneficio.ToString();
			oParam[8] = fechaDesde==string.Empty? DBNull.Value.ToString():fechaDesde;
			oParam[9] = fechaHasta==string.Empty? DBNull.Value.ToString():fechaHasta;
			oParam[10] = rutaArchGenerado;
			oParam[11] = NomArchGenerado;
			oParam[12] = Usuario.ToString();
			oParam[13] = fGeneracion == string.Empty? DBNull.Value.ToString():fGeneracion;
			oParam[14] = vigente.ToString();

			try
			{				
				SqlHelper.ExecuteNonQuery( objCnn.Conectar(), "ConsultasBatch_A", oParam );
							
			} 			
			catch( SqlException sqlErr)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlErr.Source, sqlErr.Message));
				
                if( sqlErr.Number >= 50000 )
				{
					MsgRetorno = sqlErr.Message ; 
				}
				else
				{
					throw sqlErr;
				}
			}
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err ;
			}

			return MsgRetorno;

		}
		#endregion

		#region existe Peticion de consulta
		
		public string  ExisteConsulta ( long IDPrestador, string NombreConsulta, int CriterioLiq, int Opcion, string PeriodoCons,
			byte TipoConsulta, int CodConceptoLiq,long nroBeneficio,string fechaDesde,string fechaHasta  )
		{
		
			string MsgRetorno = String.Empty;
			string Usuario = String.Empty;
			
			String[] oParam = new String[10];

			Conexion objCnn = new Conexion();

			//Usuario =  SecurityCallContext.CurrentCall.OriginalCaller.AccountName ;			

			oParam[0] = IDPrestador.ToString();
			oParam[1] = NombreConsulta.ToString();
			oParam[2] = CriterioLiq.ToString();
			oParam[3] = Opcion.ToString();
			oParam[4] = PeriodoCons.ToString();
			oParam[5] = TipoConsulta.ToString();
			oParam[6] = CodConceptoLiq.ToString();
			oParam[7] = nroBeneficio.ToString();
			oParam[8] = fechaDesde==string.Empty? DBNull.Value.ToString():fechaDesde;
			oParam[9] = fechaHasta==string.Empty? DBNull.Value.ToString():fechaHasta;
			try
			{				
				//Boolean existe = Boolean.Parse(SqlHelper.ExecuteScalar( objCnn.Conectar(), "ConsultasBatch_Existe", oParam ).ToString());

				string existe = SqlHelper.ExecuteScalar( objCnn.Conectar(), "ConsultasBatch_Existe", oParam ).ToString();
				if (existe == "1")
				{
					MsgRetorno ="Existe una consulta con los mismos parametros generada recientemente.";
				}
				else
					if (existe == "2")
					{
						MsgRetorno ="Se esta generando una consulta con los mismos parametros.Reingrese en unos minutos";
					}						
			}			
			catch( SqlException sqlErr)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlErr.Source, sqlErr.Message));
                if( sqlErr.Number >= 50000 )
				{
					MsgRetorno = sqlErr.Message ; 
				}
				else
				{
                    throw sqlErr;
				}
			}
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
				throw err ;
			}

			return MsgRetorno;

		}
		#endregion

			
	}
}

