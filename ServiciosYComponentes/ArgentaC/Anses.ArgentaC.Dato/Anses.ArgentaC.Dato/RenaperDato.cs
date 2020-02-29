using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anses.ArgentaC.Dato
{
    public static class RenaperDato
    {
        public static bool Renaper_Habilitado()
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "tblDTSVariables_Buscar";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@batch", "RENAPER"));
                cmd.Parameters.Add(new SqlParameter("@variable", "HABILITADO"));
                cmd.Connection = oCnn;
                var retorno = cmd.ExecuteScalar();
                if (retorno == null)
                {
                    return false;
                }
                else
                {
                    return (retorno.ToString() == "1" ? true : false);
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                cmd.Dispose();
            }
        }
    }
}

