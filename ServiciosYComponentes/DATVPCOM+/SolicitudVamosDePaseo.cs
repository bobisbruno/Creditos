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
	/// <summary>
	/// Summary description for SolicitudVamosPaseo.
	/// </summary>
	[Guid("0880C1D9-30A5-4745-93C7-6A2B51856F78")] 
	public interface DAT_ISolicitudVamosDePaseo
	{
		DataSet Trae_SolicitudVamosPaseo(long beneficiario, int nroComprobante );
		
	}
	[Guid("3FB91C16-2669-4b6e-9B1D-48965712D322")] 
	[ ProgId( "DAT_SolicitudVamosDePaseo" ) ]
	[ClassInterface(ClassInterfaceType.None)]
	[ ObjectPooling( MinPoolSize = 3) ]
	[JustInTimeActivation(true)]
	[EventTrackingEnabled ( true )]
	[Transaction(TransactionOption.Disabled)]
	[ComponentAccessControl(true)]
	[SecureMethod]
	

	public class SolicitudVamosDePaseo :ServicedComponent,DAT_ISolicitudVamosDePaseo
	{
		public SolicitudVamosDePaseo()
		{
		}
		
		[SecurityRole("OperadorEntidad")]
		public DataSet Trae_SolicitudVamosPaseo(long beneficiario, int nroComprobante )
		{
				

			Conexion objCnn = new Conexion();	
			SqlConnection objCon = new SqlConnection();
					
			try
			{			
				objCon = objCnn.Conectar();

				DataSet solicitud = SqlHelper.ExecuteDataset(objCnn.ConectarString(), "VamosDePaseo_solicitud_Trae_Beneficiario_NroComprobante",beneficiario,nroComprobante);
				
				return solicitud;
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


