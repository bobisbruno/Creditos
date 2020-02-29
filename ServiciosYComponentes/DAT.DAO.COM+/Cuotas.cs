using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Reflection;
using System.Configuration;
using System.EnterpriseServices;
using System.Diagnostics;
using System.Runtime.InteropServices;
using log4net;


namespace ANSES.Microinformatica.DATComPlus
{

	public class Cuotas
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(Cuotas).Name);
        
        public Cuotas()
		{
		}

		#region TraeCuotas

		
		public DataSet  TraeCuotas ( long idNovedad, long idPrestador)
		{
			DataSet ds = new DataSet();
		    Conexion objCnn = new Conexion();
				
			SqlParameter [] objPar = new SqlParameter[2];	
			try
			{						
				objPar[0] = new SqlParameter("@IdNovedad",SqlDbType.BigInt); 
				objPar[0].Value = idNovedad;
				objPar[1] = new SqlParameter("@IdPrestador",SqlDbType.BigInt); 
				objPar[1].Value = idPrestador;
					
				ds=SqlHelper.ExecuteDataset( objCnn.Conectar(), CommandType.StoredProcedure, "Cuotas_Traer", objPar );
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
		#endregion


		

	}
}

