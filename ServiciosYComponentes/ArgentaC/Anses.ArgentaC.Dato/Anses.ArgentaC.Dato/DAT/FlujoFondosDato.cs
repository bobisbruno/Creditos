using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Anses.ArgentaC.Contrato;
using System.Data.SqlClient;
using System.Data;

namespace Anses.ArgentaC.Dato
{
    public static class FlujoFondosDato
    {
        public static List<FlujoFondos> FlujoFondos_Obtener(int? _IdSistema, int? _MensualCobranzaDesde, int? _MensualCobranzaHasta)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand oCmd = new SqlCommand();
            List<FlujoFondos> FlujoFondos_Retorno = new List<FlujoFondos>();
            try
            {
                oCnn = Conexion.ObtenerConnexionSQL();
                oCmd.CommandText = "FlujoFondos_Obtener";
                oCnn.Open();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.Parameters.Add(new SqlParameter("@idSistema", (_IdSistema == 0? null : _IdSistema)));
                oCmd.Parameters.Add(new SqlParameter("@PrimerMensualDesde", _MensualCobranzaDesde));
                oCmd.Parameters.Add(new SqlParameter("@PrimerMensualHasta", _MensualCobranzaHasta));
                oCmd.Connection = oCnn;
                using (SqlDataReader dr = oCmd.ExecuteReader())
                {
                    while(dr.Read())
                    {
                        FlujoFondos_Retorno.Add(obtenerEntidad(dr));
                    }
                }
                return FlujoFondos_Retorno;
            }
            catch(Exception err)
            {
                throw err;
            }
            finally
            {
                if(oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                oCmd.Dispose();
            }
        }
        public static FlujoFondos obtenerEntidad(SqlDataReader dr)
        {
            return new FlujoFondos(dr["Prestador"].ToString(),
                                   dr["CodSistema"].ToString(),
                                   dr["Sistema"].ToString(),
                                   Int32.Parse(dr["MensualCobranza"].ToString()),
                                   Int32.Parse(dr["CantCreditosCuilito"].ToString()),
                                   Int32.Parse(dr["CantCreditosTitular"].ToString()),
                                   decimal.Parse(dr["Amortizacion"].ToString()),
                                   decimal.Parse(dr["Intereses"].ToString()),
                                   decimal.Parse(dr["InteresCuotaCero"].ToString()),
                                   decimal.Parse(dr["GastoAdmin"].ToString()),
                                   decimal.Parse(dr["SeguroVida"].ToString()),
                                   decimal.Parse(dr["Total"].ToString()));
        }
    }
}
