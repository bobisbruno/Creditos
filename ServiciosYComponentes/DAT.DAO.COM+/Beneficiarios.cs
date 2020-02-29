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
using log4net;


namespace ANSES.Microinformatica.DATComPlus
{
	
	public class Beneficiarios
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(Beneficiarios).Name);

		public Beneficiarios()
		{
		}

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

				DataSet benef = SqlHelper.ExecuteDataset(objCnn.ConectarString(), "Beneficiarios_Traer",Beneficiario,Cuil );

				return benef;
			} 
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}-> IdBeneficiario:{2}  - Cuil:{3} - Error:{4}->{5}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), Beneficiario,Cuil, err.Source, err.Message));
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
