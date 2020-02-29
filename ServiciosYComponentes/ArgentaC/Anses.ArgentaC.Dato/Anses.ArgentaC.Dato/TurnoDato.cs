using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using Anses.ArgentaC.Contrato;
using Anses.ArgentaC.Comun.Domain;

namespace Anses.ArgentaC.Dato
{
    public static class TurnoDato
    {
        public static bool Turno_Hardcoded()
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "tblDTSVariables_Buscar";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@batch", "Turno"));
                cmd.Parameters.Add(new SqlParameter("@variable", "Hardcoded"));
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

        public static bool PNC_PUAM_Existe(long cuil)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter outparam_CodError = cmd.Parameters.Add("@existe", SqlDbType.Bit);
            outparam_CodError.Direction = ParameterDirection.Output;

            try
            {
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "PNC_PUAM_Existe ";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@cuil", cuil));
                cmd.Connection = oCnn;
                cmd.ExecuteNonQuery();
                bool existe = Convert.ToBoolean(cmd.Parameters["@existe"].Value);
                return existe;
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
