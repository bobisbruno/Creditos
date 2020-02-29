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
	/// <summary>
	/// Summary description for Cuotas.
	/// </summary>
		
	public class Parametros
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(Parametros).Name); 
       
        public Parametros()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region SITIO HABILITADO

		
		public string  SitioHabilitado( )
		{
		
			Conexion objCnn = new Conexion();				
	
			try
			{
			 return (string) SqlHelper.ExecuteScalar( objCnn.ConectarString(),CommandType.StoredProcedure,"Parametros_SitioHabilitado");
				
			}			
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw err ;
			}
			finally
			{
				objCnn=null;
			}
		}
		#endregion

		#region MaxCantCuotas
		
		public byte  MaxCantCuotas( )
		{	
			Conexion objCnn = new Conexion();
		
			try
			{						
				return (byte) SqlHelper.ExecuteScalar( objCnn.ConectarString(),CommandType.StoredProcedure,"Parametros_MaxCantCuotas");
				
			}			
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw err;
			}
			finally
			{
				objCnn=null;
			}
		}
		#endregion

		#region MaxPorcentaje
		
		public Single  MaxPorcentaje( )
		{		
		   Conexion objCnn = new Conexion();
		   try
			{
			    return Single.Parse(SqlHelper.ExecuteScalar( objCnn.ConectarString(),CommandType.StoredProcedure,"Parametros_MaxPorcentaje").ToString());
			} 
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw err ;
			}
			finally
			{
				objCnn=null;
			}
		}
		#endregion

		#region Trae Costo Financiero
		
		public DataSet CostoFinanciero_Trae(byte cantcuotas)
		{
			DataSet ds = new DataSet();
		
			Conexion objCnn = new Conexion();

			SqlParameter [] objPar = new SqlParameter[1];
				
			try
			{

				objPar[0] = new SqlParameter("@cantcuotas",SqlDbType.TinyInt); 
				objPar[0].Value = cantcuotas;
                						
//				return SqlHelper.ExecuteDataset( objCnn.ConectarString(),
//													CommandType.StoredProcedure,
//													"Parametros_costoFinanciero_Trae",
//													new SqlParameter("@cantcuotas",cantcuotas));

				ds=SqlHelper.ExecuteDataset( objCnn.Conectar(), CommandType.StoredProcedure, "Parametros_costoFinanciero_Trae", objPar );
				
				return ds;
				
			} 
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));  
                throw err;
			}
			finally
			{
				objCnn=null;
			}
		}
		#endregion

	}
}

