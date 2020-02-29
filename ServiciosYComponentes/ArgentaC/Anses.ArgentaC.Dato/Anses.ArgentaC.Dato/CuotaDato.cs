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
    public static class CuotaDato
    {
        public static Persona calcularCuotas(Persona _unaPersona, Producto _unProducto, decimal _unMonto)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter outparam_CodError = cmd.Parameters.Add("@CodError", SqlDbType.Int);
            outparam_CodError.Direction = ParameterDirection.Output;
            SqlParameter outparam_MsgResultado = cmd.Parameters.Add("@MsgResultado", SqlDbType.NVarChar, 4000);
            outparam_MsgResultado.Direction = ParameterDirection.Output;
            try
            {
                List<Cuota> lstCuotas = new List<Cuota>();
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "CalcularCuotas";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@idProducto", _unProducto.Id));
                cmd.Parameters.Add(new SqlParameter("@MontoSolicitado", _unMonto));
                cmd.Connection = oCnn;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lstCuotas.Add(obtenerEntidad(dr));
                    }
                }
                int CodError = Convert.ToInt16(cmd.Parameters["@CodError"].Value);
                string MsgResultado = Convert.ToString(cmd.Parameters["@MsgResultado"].Value);
                _unaPersona.Errores.Add(new Mensaje(CodError, MsgResultado));
                _unaPersona.Productos.Find(x => x.Id == _unProducto.Id).Cuotas = lstCuotas;
                return _unaPersona;
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
        private static Cuota obtenerEntidad(SqlDataReader dr)
        {
            return new Cuota(0, //idPrestamo
                            dr.IsDBNull(dr.GetOrdinal("NroCta")) ? 0 : dr.GetInt32(dr.GetOrdinal("NroCta")),
                            decimal.Parse(string.IsNullOrEmpty(dr["Interes"].ToString()) ? "0" : dr["Interes"].ToString()),
                            decimal.Parse(string.IsNullOrEmpty(dr["Amortizacion"].ToString()) ? "0" : dr["Amortizacion"].ToString()),
                            decimal.Parse(string.IsNullOrEmpty(dr["TotalAmortizado"].ToString()) ? "0" : dr["TotalAmortizado"].ToString()),
                            decimal.Parse(string.IsNullOrEmpty(dr["AmortizacionRemanente"].ToString()) ? "0" : dr["AmortizacionRemanente"].ToString()),
                            decimal.Parse(string.IsNullOrEmpty(dr["CuotaPura"].ToString()) ? "0" : dr["CuotaPura"].ToString()),
                            decimal.Parse(string.IsNullOrEmpty(dr["GastosOperativos"].ToString()) ? "0" : dr["GastosOperativos"].ToString()),
                            decimal.Parse(string.IsNullOrEmpty(dr["TotalPagar"].ToString()) ? "0" : dr["TotalPagar"].ToString()),
                            0, //cuotaPagada
                            0, //cuotaImpaga
                            decimal.Parse(string.IsNullOrEmpty(dr["Seguro"].ToString()) ? "0" : dr["Seguro"].ToString())
                );
        }
    }
}
