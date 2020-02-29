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
	/// Summary description for Cuotas.
	/// </summary>
	[Guid("76A10E82-8B99-42cd-8772-09981259A675")] 
	public interface DAT_IParametros
	{
		string  SitioHabilitado ( );
		byte MaxCantCuotas();
		Single  MaxPorcentaje();
		DataSet CostoFinanciero_Trae(byte cantcuotas);

	}
	[Guid("F1ADEB63-28AD-4230-B3F2-F9B168ABDD28")] 
	[ ProgId( "DAT_Parametros" ) ]
	[ClassInterface(ClassInterfaceType.None)]
	[ ObjectPooling( MinPoolSize = 3) ]
	[JustInTimeActivation(true)]
	[Transaction(TransactionOption.Disabled)]
	[EventTrackingEnabled ( true )]
	[ComponentAccessControl(true)]
	[SecureMethod]
	
	public class Parametros:ServicedComponent,DAT_IParametros
	{
		public Parametros()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region SITIO HABILITADO

		[SecurityRole("OperadorEntidad")]
		public string  SitioHabilitado( )
		{
		
			Conexion objCnn = new Conexion();
				
	
			try
			{
						
				return (string) SqlHelper.ExecuteScalar( objCnn.ConectarString(),CommandType.StoredProcedure,"Parametros_SitioHabilitado");
				
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

		#region MaxCantCuotas
		[SecurityRole("OperadorEntidad")]
		public byte  MaxCantCuotas( )
		{
		
			Conexion objCnn = new Conexion();
				
	
			try
			{
						
				return (byte) SqlHelper.ExecuteScalar( objCnn.ConectarString(),CommandType.StoredProcedure,"Parametros_MaxCantCuotas");
				
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

		#region MaxPorcentaje
		[SecurityRole("OperadorEntidad")]
		public Single  MaxPorcentaje( )
		{
		
			Conexion objCnn = new Conexion();
				
	
			try
			{
						
				return Single.Parse(SqlHelper.ExecuteScalar( objCnn.ConectarString(),CommandType.StoredProcedure,"Parametros_MaxPorcentaje").ToString());
				
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

		#region Trae Costo Financiero
		[SecurityRole("OperadorEntidad")]
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
				throw err ;
			}
			finally
			{
				objCnn=null;
			}
		}
		#endregion

	}
}

