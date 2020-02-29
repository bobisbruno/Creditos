using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Data.SqlClient;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    public class DeudaArgentaDAO
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DeudaArgentaDAO).Name);

        public static void BeneficiarioDeuda_Traer(long cuil, out bool tieneDeuda, out decimal montoDeuda)
        {
            string sql = "Beneficiario_Deuda_T";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            tieneDeuda = false;

            try
            {
                db.AddInParameter(dbCommand, "@cuil", DbType.Int64, cuil);
                db.AddOutParameter(dbCommand, "@tieneDeuda", DbType.Boolean, 0);
                db.AddOutParameter(dbCommand, "@montoDeuda", DbType.Decimal, 0);

                db.ExecuteNonQuery(dbCommand);

                tieneDeuda = string.IsNullOrEmpty(db.GetParameterValue(dbCommand, "@tieneDeuda").ToString()) ? tieneDeuda : Convert.ToBoolean(db.GetParameterValue(dbCommand, "@tieneDeuda").ToString());
                montoDeuda = string.IsNullOrEmpty(db.GetParameterValue(dbCommand, "@montoDeuda").ToString()) ? 0 :  Math.Round(Convert.ToDecimal(db.GetParameterValue(dbCommand, "@montoDeuda").ToString()),2);

            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
    }
}
      