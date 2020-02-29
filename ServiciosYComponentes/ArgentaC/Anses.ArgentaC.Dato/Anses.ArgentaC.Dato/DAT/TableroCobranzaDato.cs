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
    public static class TableroCobranzaDato
    {
        public static TableroCobranza TableroCobranza_Obtener(int? _mensual, int? _concepto)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand oCmd = new SqlCommand();
            TableroCobranza TableroCobranza_Retorno = new TableroCobranza();
            try
            {
                oCnn = Conexion.ObtenerConnexionSQL();
                oCmd.CommandText = "TC_ObtenerGrillas";
                oCnn.Open();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.Parameters.Add(new SqlParameter("@Mensual", _mensual));
                oCmd.Parameters.Add(new SqlParameter("@CodConceptoLiq", _concepto));
                oCmd.Connection = oCnn;
                using (SqlDataReader dr = oCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        TableroCobranza_Retorno.InfCobranza.listaCobranzas.Add (obtenerEntidadInfCobranza(dr));
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        TableroCobranza_Retorno.RepCobranza.listaCobranzas.Add(obtenerEntidadRepCobranza(dr));
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        TableroCobranza_Retorno.InfPendCobro.listaPendientesDeCobro.Add(obtenerEntidadPendienteDeCobro(dr));
                    }
                }
                return TableroCobranza_Retorno;
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
                oCmd.Dispose();
            }
        }
        public static Cobranza obtenerEntidadInfCobranza(SqlDataReader dr)
        {
            return new Cobranza(   Int32.Parse(dr["Mensual"].ToString()),
                                   dr["CodConceptoOriginante"].ToString(),
                                   Int32.Parse(dr["SistemaApropiador"].ToString()),
                                   long.Parse(dr["CantCreditos"].ToString()),
                                   decimal.Parse(dr["MontoCuotaTotal"].ToString()),
                                   decimal.Parse(dr["Amortizacion"].ToString()),
                                   decimal.Parse(dr["Intereses"].ToString()),
                                   decimal.Parse(dr["GastoAdministrativo"].ToString()),
                                   decimal.Parse(dr["SeguroVida"].ToString()),
                                   decimal.Parse(dr["InteresCuotaCero"].ToString()),
                                   decimal.Parse(dr["MontoCuotaSinRedondeo"].ToString()));
        }

        public static CobranzaReporte obtenerEntidadRepCobranza(SqlDataReader dr)
        {
            return new CobranzaReporte(    Int32.Parse(dr["Mensual"].ToString()),
                                           //Int32.Parse(dr["SistemaOriginante"].ToString()),
                                           dr["SistemaOriginante"].ToString(),
                                           long.Parse(dr["DV_CantCreditos"].ToString()),
                                           decimal.Parse(dr["DV_Importe"].ToString()),
                                           long.Parse(dr["PANT_CantCreditos"].ToString()),
                                           decimal.Parse(dr["PANT_Importe"].ToString()),
                                           long.Parse(dr["F_CantCreditos"].ToString()),
                                           decimal.Parse(dr["F_Importe"].ToString()),
                                           long.Parse(dr["A_LIQ_CantCreditos"].ToString()),
                                           decimal.Parse(dr["A_LIQ_Importe"].ToString()),
                                           long.Parse(dr["AHU_CantCreditos"].ToString()),
                                           decimal.Parse(dr["AHU_Importe"].ToString()),
                                           long.Parse(dr["SUAF_CantCreditos"].ToString()),
                                           decimal.Parse(dr["SUAF_Importe"].ToString()),
                                           long.Parse(dr["PEND_CantCreditos"].ToString()),
                                           decimal.Parse(dr["PEND_Importe"].ToString()));
        }

        public static PendientesDeCobro obtenerEntidadPendienteDeCobro(SqlDataReader dr)
        {
            return new PendientesDeCobro(   Int32.Parse(dr["Mensual"].ToString()),
                                            dr["Motivo"].ToString(),
                                            Int32.Parse(dr["CantCasos"].ToString()),
                                            decimal.Parse(dr["Importe"].ToString()));
        }
    }
}
