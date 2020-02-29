using Anses.ArgentaC.Contrato;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anses.ArgentaC.Dato
{
    public static class ParametroDato
    {
        private static Parametro GetObject(System.Data.SqlClient.SqlDataReader dr)
        {
            Parametro result = new Parametro();
            result.batch = string.IsNullOrEmpty(dr["batch"].ToString()) ? "" : dr["batch"].ToString();
            result.Variable = string.IsNullOrEmpty(dr["Variable"].ToString()) ? "" : dr["Variable"].ToString(); 
            result.Valor = string.IsNullOrEmpty(dr["Valor"].ToString()) ? "" : dr["Valor"].ToString(); 

            return result;

        }

        public static bool Parametros_SitioHabilitado()
        {
            bool result = false;

            SqlConnection oCnn = Conexion.ObtenerConnexionSQL();
            SqlCommand cmd = new SqlCommand("Parametros_SitioHabilitado", oCnn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            oCnn.Open();
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                if (dr.Read())
                {
                    result = (dr["VALOR"].ToString() == "1" ? true: false);
                }
            }

            if (oCnn.State != ConnectionState.Closed)
            {
                oCnn.Close();
            }
            oCnn.Dispose();
            cmd.Dispose();

            return result;
        }

        public static string TblDTSVariables_Buscar(string batch, string variable)
        {
            string valor = null;
            using (SqlConnection con = Conexion.ObtenerConnexionSQL())
            {
                SqlCommand cmd = new SqlCommand("tblDTSVariables_Buscar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter("@batch", batch);
                cmd.Parameters.Add(param);
                param = new SqlParameter("@Variable", variable);
                cmd.Parameters.Add(param);
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.HasRows)
                    {
                        dr.Read();
                        valor = dr["Valor"] as string;
                    }
                }
            }
            return valor;
        }
    }
}
