using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Reflection;
using System.Configuration;
using System.EnterpriseServices;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace ANSES.Microinformatica.DATVPComPlus
{
	[Guid("0880C1D9-30A5-4745-93C7-6A2B51856F79")] 
	public interface DAT_IAgencia
	{
		DataSet Trae_Agencia(int nroLegajo );
	}
	[Guid("3FB91C16-2669-4b6e-9B1D-48965712D324")] 
	[ ProgId( "DAT_Agencia" ) ]
	[ClassInterface(ClassInterfaceType.None)]
	[ ObjectPooling( MinPoolSize = 3) ]
	[JustInTimeActivation(true)]
	[EventTrackingEnabled ( true )]
	[Transaction(TransactionOption.Disabled)]
	[ComponentAccessControl(true)]
	[SecureMethod]

	/// <summary>
	/// Summary description for Agencia.
	/// </summary>
	
	public class Agencia:ServicedComponent,DAT_IAgencia
	{
	
		[SecurityRole("OperadorEntidad")]
			

		public DataSet Trae_Agencia(int nroLegajo )
		{

			Conexion objCnn = new Conexion();	
			SqlConnection objCon = new SqlConnection();
					
			try
			{			
				objCon = objCnn.Conectar();

				DataSet agencia = SqlHelper.ExecuteDataset(objCnn.ConectarString(), "VamosDePaseo_Agencia_Traer",1,0,nroLegajo);
								
				
				return agencia;
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

	}
}
