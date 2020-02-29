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
    public class MutuoDato
    {
        public Mutuo obtenerDatosMutuo(Novedad novedad)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                Mutuo mutuo = new Mutuo();
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "ObtenerNovedadPorID";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@idNovedad", novedad.IdNovedad));
                cmd.Connection = oCnn;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        mutuo = obtenerEntidad(dr);
                    }
                    mutuo.BeneficiosAfectadosMutuo = new List<Beneficio>();
                    dr.NextResult();
                    while (dr.Read())
                    {
                        mutuo.BeneficiosAfectadosMutuo.Add(obtenerEntidadBeneficio(dr));
                    }
                }
                return mutuo;
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

        public int obtenerVersionActualMutuo(int idOrigen, DateTime fechaVersion)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                int id = 0;
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "ObtenerVersionActualMutuo";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@idOrigen", idOrigen));
                cmd.Parameters.Add(new SqlParameter("@FechaVersion", fechaVersion));
                cmd.Connection = oCnn;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        id = int.Parse(dr["idVersion"].ToString());
                    }
                }
                return id;
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

        private static Mutuo obtenerEntidad(SqlDataReader dr)
        {
            return new Mutuo(
                string.IsNullOrEmpty(dr["NombreYApellido"].ToString()) ? "" : dr["NombreYApellido"].ToString(),
                long.Parse(string.IsNullOrEmpty(dr["cuilTomador"].ToString()) ? "" : dr["CuilTomador"].ToString()),
                string.IsNullOrEmpty(dr["DNI_Nro_Tramite_Renaper"].ToString()) ? "" : dr["DNI_Nro_Tramite_Renaper"].ToString(),
                string.IsNullOrEmpty(dr["Banco"].ToString()) ? "" : dr["Banco"].ToString(),
                string.IsNullOrEmpty(dr["Agencia"].ToString()) ? "" : dr["Agencia"].ToString(),
                string.IsNullOrEmpty(dr["CBU"].ToString()) ? "" : dr["CBU"].ToString(),
                string.IsNullOrEmpty(dr["Provincia"].ToString()) ? "" : dr["Provincia"].ToString(),
                string.IsNullOrEmpty(dr["Calle"].ToString()) ? "" : dr["Calle"].ToString(),
                string.IsNullOrEmpty(dr["Altura"].ToString()) ? "" : dr["Altura"].ToString(),
                string.IsNullOrEmpty(dr["Piso"].ToString()) ? "" : dr["Piso"].ToString(),
                string.IsNullOrEmpty(dr["Depto"].ToString()) ? "" : dr["Depto"].ToString(),
                string.IsNullOrEmpty(dr["Codigo_postal"].ToString()) ? "" : dr["Codigo_postal"].ToString(),
                string.IsNullOrEmpty(dr["Localidad"].ToString()) ? "" : dr["Localidad"].ToString(),
                string.IsNullOrEmpty(dr["TelefonoFijo"].ToString()) ? "" : dr["TelefonoFijo"].ToString(),
                string.IsNullOrEmpty(dr["Celular"].ToString()) ? "" : dr["Celular"].ToString(),
                string.IsNullOrEmpty(dr["Mail"].ToString()) ? "" : dr["Mail"].ToString(),
                decimal.Parse(string.IsNullOrEmpty(dr["ImporteTotal"].ToString()) ? "" : dr["ImporteTotal"].ToString()),
                int.Parse(string.IsNullOrEmpty(dr["CantCuotas"].ToString()) ? "" : dr["CantCuotas"].ToString()),
                decimal.Parse(string.IsNullOrEmpty(dr["CFTEA"].ToString()) ? "" : dr["CFTEA"].ToString()),
                decimal.Parse(string.IsNullOrEmpty(dr["TNA"].ToString()) ? "" : dr["TNA"].ToString()),
                DateTime.Parse(dr["FechaCredito"].ToString()),
                long.Parse(string.IsNullOrEmpty(dr["idProducto"].ToString()) ? "" : dr["idProducto"].ToString()),
                long.Parse(string.IsNullOrEmpty(dr["IdNovedad"].ToString()) ? "" : dr["IdNovedad"].ToString()),
                int.Parse(string.IsNullOrEmpty(dr["idVersionMutuo"].ToString()) ? "" : dr["idVersionMutuo"].ToString()),
                long.Parse(string.IsNullOrEmpty(dr["idEstadoNovedad"].ToString()) ? "" : dr["idEstadoNovedad"].ToString()),
                long.Parse(string.IsNullOrEmpty(dr["OFICINA_CARGA"].ToString()) ? "" : dr["OFICINA_CARGA"].ToString()),
                bool.Parse(string.IsNullOrEmpty(dr["ImposibilidadFirma"].ToString()) ? "false" : dr["ImposibilidadFirma"].ToString())
                );
        }

        private static Beneficio obtenerEntidadBeneficio(SqlDataReader dr)
        {
            return new Beneficio(
                long.Parse(string.IsNullOrEmpty(dr["cuil"].ToString()) ? "" : dr["cuil"].ToString()),
                string.IsNullOrEmpty(dr["apellidoNombre"].ToString()) ? "" : dr["apellidoNombre"].ToString(),
                decimal.Parse(string.IsNullOrEmpty(dr["Importe"].ToString()) ? "" : dr["Importe"].ToString()),
                long.Parse(string.IsNullOrEmpty(dr["nroBeneficio"].ToString()) ? "" : dr["nroBeneficio"].ToString()),
                DateTime.Parse(dr["fecNacimiento"].ToString())
                );
        }
    }
}
