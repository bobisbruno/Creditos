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
	public class Cierres
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(Cierres).Name);
        
        public Cierres()
		{
			
		}

		#region Trae_Fec_Prox_Cierre

		public DataSet Trae_Fec_Prox_Cierre()
		{

			Conexion objCnn = new Conexion();	
			SqlConnection objCon = new SqlConnection();
					
			try
			{			
				objCon = objCnn.Conectar();
				DataSet FecProxCierre = SqlHelper.ExecuteDataset(objCon, CommandType.StoredProcedure, "Cierres_TProxFecCierre");
				
				return FecProxCierre;
			} 
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err ;
			}
			finally
			{
				objCon.Dispose();
				objCnn=null;
			}

		}
		#endregion

		#region Trae_Fec_Cierre_Ant

		
		public DataSet Trae_Fec_Cierre_Ant()
		{
			Conexion objCnn = new Conexion();	
			SqlConnection objCon = new SqlConnection();
		
			try
			{			
				objCon = objCnn.Conectar();
				DataSet FecProxCierre = SqlHelper.ExecuteDataset(objCon, CommandType.StoredProcedure, "Cierres_TFecCierreAnt");
				return FecProxCierre;
			} 
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err ;
			}
			finally
			{
				objCon.Dispose();
				objCnn=null;
			}
		}
		#endregion

		#region Trae_Cierres_Anteriores

		
		public DataSet Trae_Cierres_Anteriores()
		{
			DataSet ds = new DataSet();
			
			Conexion objCnn = new Conexion();
	
			try
			{
				ds=SqlHelper.ExecuteDataset( objCnn.Conectar(), CommandType.StoredProcedure, "Cierres_TT" );
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
							
		#region Cierres_AM

		
		public void Cierres_AM ( string FecCierre, string Mensual,	string Usuario)
		{
			DataSet ds = new DataSet();
			
			Conexion objCnn = new Conexion();	
			
			SqlConnection objCon = new SqlConnection();
			try
			{
				
				objCon = objCnn.Conectar();
				SqlParameter [] objPar = new SqlParameter[3];
			
				objPar[0] = new SqlParameter("@FecCierre",SqlDbType.VarChar,8); 
				objPar[0].Value = FecCierre;

				objPar[1] = new SqlParameter("@Mensual",SqlDbType.VarChar,8); 
				objPar[1].Value = Mensual;

				objPar[2] = new SqlParameter("@Usuario",SqlDbType.VarChar,50);
				objPar[2].Value = Usuario;
			
				SqlHelper.ExecuteNonQuery(objCon,CommandType.StoredProcedure,"dbo.Cierres_AM",objPar);
				
				return ;
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
