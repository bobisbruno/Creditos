using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using Anses.ArgentaC.Contrato;

namespace Anses.ArgentaC.Dato
{
    public static class LibretaDato
    {
        public static int RegistrarLibreta(Anses.ArgentaC.Contrato.Libreta unaLibreta)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    oCnn = Conexion.ObtenerConnexionSQL();
                    cmd.CommandText = "RegistrarLibreta";
                    oCnn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@IdBeneficioAsociado", unaLibreta.IdBeneficioAsociado));
                    cmd.Parameters.Add(new SqlParameter("@fecUltimaLibreta", unaLibreta.fecUltimaLibreta));
                    cmd.Parameters.Add(new SqlParameter("@CodError", "0"));

                    cmd.Connection = oCnn;

                    int codError = Convert.ToInt32(cmd.ExecuteScalar());

                    scope.Complete();

                    return codError;
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
