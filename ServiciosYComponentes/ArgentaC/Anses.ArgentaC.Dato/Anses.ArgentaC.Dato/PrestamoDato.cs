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
    public static class PrestamoDato
    {
        public static Persona ObtenerNovedades(Persona _persona)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter outparam_CodError = cmd.Parameters.Add("@CodError", SqlDbType.Int);
            outparam_CodError.Direction = ParameterDirection.Output;
            SqlParameter outparam_MsgResultado = cmd.Parameters.Add("@MsgResultado", SqlDbType.NVarChar, 4000);
            outparam_MsgResultado.Direction = ParameterDirection.Output;
            try
            {
                List<Prestamo> lstPrestamos = new List<Prestamo>();
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "ObtenerNovedades ";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@cuil", _persona.Cuil));
                cmd.Parameters.Add(new SqlParameter("@idEstadoNovedad ", 1));
                cmd.Connection = oCnn;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lstPrestamos.Add(obtenerEntidad(dr));
                    }
                }
                int CodError = Convert.ToInt16(cmd.Parameters["@CodError"].Value);
                string MsgResultado = Convert.ToString(cmd.Parameters["@MsgResultado"].Value);
                _persona.Errores.Add(new Mensaje(CodError, MsgResultado));
                _persona.Prestamos = lstPrestamos;
                return _persona;
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
        private static Prestamo obtenerEntidad(SqlDataReader dr)
        {
            return new Prestamo(decimal.Parse(string.IsNullOrEmpty(dr["Monto"].ToString()) ? "0" : dr["Monto"].ToString()),
                                Int32.Parse(string.IsNullOrEmpty(dr["CantCuotas"].ToString()) ? "0" : dr["CantCuotas"].ToString()),
                                decimal.Parse(string.IsNullOrEmpty(dr["CuotaTotalMensual"].ToString()) ? "0" : dr["CuotaTotalMensual"].ToString()),
                                decimal.Parse(string.IsNullOrEmpty(dr["TNA"].ToString()) ? "0" : dr["TNA"].ToString()),
                                decimal.Parse(string.IsNullOrEmpty(dr["GastosAdministrativos"].ToString()) ? "0" : dr["GastosAdministrativos"].ToString()),
                                decimal.Parse(string.IsNullOrEmpty(dr["CFTNA"].ToString()) ? "0" : dr["CFTNA"].ToString()),
                                decimal.Parse(string.IsNullOrEmpty(dr["CFTEA"].ToString()) ? "0" : dr["CFTEA"].ToString()),
                                new List<Cuota>());
        }
    }
}
