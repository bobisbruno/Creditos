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
	[Guid("0880C1D9-30A5-4745-93C7-6A2B51856F68")] 
	public interface DAT_ICierres
	{
			DataSet Trae_Fec_Prox_Cierre();
			DataSet Trae_Fec_Cierre_Ant();
			DataSet Trae_Cierres_Anteriores();
			//int Trae_Mensual();
			void Cierres_AM ( string FecCierre, string Mensual,string Usuario);		    

	}
	[Guid("3FB91C16-2669-4b6e-9B1D-48965712D321")] 
	[ ProgId( "DAT_Cierres" ) ]
	[ClassInterface(ClassInterfaceType.None)]
	[ ObjectPooling( MinPoolSize = 3) ]
	[JustInTimeActivation(true)]
	[EventTrackingEnabled ( true )]
	[Transaction(TransactionOption.Disabled)]
	[ComponentAccessControl(true)]
	[SecureMethod]
	public class Cierres:ServicedComponent,DAT_ICierres
	{
		public Cierres()
		{
			
		}

		#region Trae_Fec_Prox_Cierre

		[SecurityRole("OperadorEntidad")]
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

		[SecurityRole("OperadorEntidad")]
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

		[SecurityRole("OperadorEntidad")]
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

		[SecurityRole("OperadorEntidad")]
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
