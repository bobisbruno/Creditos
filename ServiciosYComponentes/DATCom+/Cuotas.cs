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
	[Guid("9E2BEC2A-FBD7-489c-8311-EA7BC1867564")] 
	public interface DAT_ICuotas
	{
		DataSet  TraeCuotas ( long idNovedad, long idPrestador);
		

	}
	[Guid("66460206-1CB4-4eab-9420-C499F2EF6C8B")] 
	[ ProgId( "DAT_Cuotas" ) ]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ ObjectPooling( MinPoolSize = 3) ]
	[JustInTimeActivation(true)]
	[Transaction(TransactionOption.Required)]
	[EventTrackingEnabled ( true )]
	[ComponentAccessControl(true)]
	[SecureMethod]
	
	public class Cuotas:ServicedComponent,DAT_ICuotas
	{
		public Cuotas()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		#region TraeCuotas

		[SecurityRole("OperadorEntidad")]
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

