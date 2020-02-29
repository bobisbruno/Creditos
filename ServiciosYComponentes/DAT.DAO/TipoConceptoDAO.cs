using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;
using System.EnterpriseServices;
using System.Diagnostics;
using System.Data.Common;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NullableReaders;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
	public class TipoConceptoDAO
	{

        public TipoConceptoDAO()
		{
		}
		
		public DataSet Traer (long lintPrestador)
		{
            DataSet dss = new DataSet();

            string sql = "TipoConcepto_TxPrestador";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;

            try
            {
                db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, lintPrestador);
                dbParametros = dbCommand.Parameters;

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {

                    }
                }
				
				return dss;
			} 
			
			catch(Exception err)
			{
				throw err ;
			}
			finally
			{
				dss.Dispose();
			}
		}
	}
}