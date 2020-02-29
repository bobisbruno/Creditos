using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Data.Common;
using System.Reflection;
using System.Configuration;
using System.EnterpriseServices;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace ANSES.Microinformatica.DATComPlus
{
	[Guid("82A9C67A-5D7A-4055-ABE9-D52120FB35BC")] 
	public interface DAT_IBeneficiarios
	{
		 DataSet Traer (string Beneficiario, string Cuil );

	}


	[Guid("467E5045-9C46-4e28-B9AD-D4559A734581")] 
	[ProgId( "DAT_Beneficiarios" ) ]
	[ClassInterface(ClassInterfaceType.AutoDual)]
	[ ObjectPooling( MinPoolSize = 10) ]
	//[JustInTimeActivation(true)]
	[EventTrackingEnabled ( true )]
	[Transaction(TransactionOption.Supported )]
	[ComponentAccessControl(true)]
	[SecureMethod]
	public class Beneficiarios:ServicedComponent, DAT_IBeneficiarios
	{

		public Beneficiarios()
		{
			// 
			// TODO: Add constructor logic here
			//
		
		}

		[SecurityRole("OperadorEntidad")]
		public DataSet Traer (string Beneficiario, string Cuil )
		{
			Conexion objCnn = new Conexion();	
			SqlConnection objCon = new SqlConnection();
			
			try
			{			
				
				objCon = objCnn.Conectar();
				SqlParameter [] objPar = new SqlParameter[2];
			
				objPar[0] = new SqlParameter("@IdBeneficiario",SqlDbType.VarChar,20); 
				objPar[0].Value = Beneficiario;
				
				objPar[1] = new SqlParameter("@Cuil",SqlDbType.VarChar,11); 
				objPar[1].Value = Cuil;

				//DataSet benef = SqlHelper.ExecuteDataset(objCon, CommandType.StoredProcedure, "Beneficiarios_Traer",objPar );
				DataSet benef = SqlHelper.ExecuteDataset(objCnn.ConectarString(), "Beneficiarios_Traer",Beneficiario,Cuil );

				return benef;
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
