using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Anses.ArgentaC.Contrato;
using Anses.ArgentaC.Comun.Domain;
using Anses.ArgentaC.Dato;

namespace Anses.ArgentaC.Dato
{
    public static class ArchivoDato
    {
        public static string InformesPreparados_Buscar(int mensual, int? concepto, int? tipoInforme)
        {
            string pathSalida = "";
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "TC_InformesPreparados_Buscar";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Mensual", mensual.ToString()));
                cmd.Parameters.Add(new SqlParameter("@CodConcepto", concepto.ToString()));
                cmd.Parameters.Add(new SqlParameter("@TipoInforme", tipoInforme.ToString()));
                cmd.Connection = oCnn;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        pathSalida = obtenerEntidad(dr);
                    }
                }
                return pathSalida;
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

        private static string obtenerEntidad(SqlDataReader dr)
        {
            return dr["NombreArchivo"].ToString();
        }
    }
}
