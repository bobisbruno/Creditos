using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;
using System.EnterpriseServices;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.ApplicationBlocks.Data;
using log4net;


namespace ANSES.Microinformatica.DATComPlus
{
	public class TipoConcConcLiq 
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(TipoConcConcLiq).Name);

		public TipoConcConcLiq()
		{}
             

		public DataSet Traer (long Prestador)
		{
			DataSet ds = new DataSet();
		
			Conexion objCnn = new Conexion();
				
			SqlParameter [] objPar = new SqlParameter[1];
	
			try
			{
						
				objPar[0] = new SqlParameter("@Prestador",SqlDbType.BigInt); 
				objPar[0].Value = Prestador;
					
	
				ds=SqlHelper.ExecuteDataset( objCnn.Conectar(), CommandType.StoredProcedure, "TipoConc_ConcLiq_TxPrestador", objPar );

				
				return ds;
			} 
			
			catch(Exception err)
			{
               log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
               throw err ;
			}
			finally
			{
				ds.Dispose();
				objCnn=null;
			}
		}

		
		public DataSet TraerTipoServicio (int CodConceptoLiq, short TipoConcepto)
		{
			DataSet ds = new DataSet();
		
			Conexion objCnn = new Conexion();
				
			SqlParameter [] objPar = new SqlParameter[2];
	
			try
			{
				
				objPar[0] = new SqlParameter("@CodConceptoLiq",SqlDbType.Int); 
				objPar[0].Value = CodConceptoLiq;
				objPar[1] = new SqlParameter("@TipoConcepto",SqlDbType.TinyInt); 
				objPar[1].Value = TipoConcepto;
					
	
				ds=SqlHelper.ExecuteDataset( objCnn.Conectar(), CommandType.StoredProcedure, "RelacionCodLiqXTipoConcXPrestITem_Trae", objPar );

				
				return ds;
			} 
			
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err ;
			}
			finally
			{
				ds.Dispose();
				objCnn=null;
			}
		}
	}
}