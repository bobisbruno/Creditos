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
    public static class BeneficioDato
    {
        public static int RegistrarBeneficiosRelacionados(Anses.ArgentaC.Contrato.Beneficio unBeneficio)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    oCnn = Conexion.ObtenerConnexionSQL();
                    cmd.CommandText = "RegistrarBeneficiosRelacionados";
                    oCnn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Cuil", unBeneficio.Cuil));
                    cmd.Parameters.Add(new SqlParameter("@NroBeneficio", unBeneficio.NroBeneficio));
                    cmd.Parameters.Add(new SqlParameter("@ApellidoNombre", unBeneficio.ApellidoNombre));
                    cmd.Parameters.Add(new SqlParameter("@TipoDoc", unBeneficio.TipoDoc));
                    cmd.Parameters.Add(new SqlParameter("@NroDoc", unBeneficio.NroDoc));
                    cmd.Parameters.Add(new SqlParameter("@sexo", unBeneficio.Sexo));
                    cmd.Parameters.Add(new SqlParameter("@fnacimiento", unBeneficio.FecNacimiento));
                    cmd.Parameters.Add(new SqlParameter("@ffallecimiento", unBeneficio.FecFallecimiento));
                    cmd.Parameters.Add(new SqlParameter("@SueldoBruto", unBeneficio.SueldoBruto));
                    cmd.Parameters.Add(new SqlParameter("@cbu", unBeneficio.CBU));
                    cmd.Parameters.Add(new SqlParameter("@codBanco", unBeneficio.CodBanco));
                    cmd.Parameters.Add(new SqlParameter("@codAgencia", unBeneficio.CodAgencia));
                    cmd.Parameters.Add(new SqlParameter("@periodoAlta", unBeneficio.PeriodoAlta));
                    cmd.Parameters.Add(new SqlParameter("@CodPrestacion", unBeneficio.CodPrestacion));
                    cmd.Parameters.Add(new SqlParameter("@RelCuil", unBeneficio.RelCuil));
                    cmd.Parameters.Add(new SqlParameter("@esDiscapacitado", unBeneficio.EsDiscapacitado));
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
