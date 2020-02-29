using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;

namespace Anses.ArgentaC.Dato
{
    public class Conexion
    {
        public static SqlConnection ObtenerConnexionSQL()
        {
            string _conString = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
            return new SqlConnection(_conString);
        }
        public static void buildParameter(Microsoft.Practices.EnterpriseLibrary.Data.Database db, DbCommand dbCommand, string name, object value)
        {
            DbParameter p = dbCommand.CreateParameter();
            p.Value = value;
            db.AddInParameter(dbCommand, name, p.DbType, value);
        }
        public static bool HasColumn(SqlDataReader r, string columnName)
        {
            try
            {
                return r.GetOrdinal(columnName) >= 0;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }
        public static string ObtenerConnStringKey()
        {
            return "SQLServerConnectionString";
        }
    }
}
