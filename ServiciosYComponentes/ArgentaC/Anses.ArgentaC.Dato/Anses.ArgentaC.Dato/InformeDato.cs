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
    public static class InformeDato
    {
        public static List<InformeDeDevolucionesCierreDiario> InformeDeDevolucionesCierreDiario_Obtener(DateTime? _FechaDesde, DateTime? _FechaHasta)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand oCmd = new SqlCommand();
            List<InformeDeDevolucionesCierreDiario> InformeDeDevolucionesCierreDiario_Retorno = new List<InformeDeDevolucionesCierreDiario>();
            try
            {
                oCnn = Conexion.ObtenerConnexionSQL();
                oCmd.CommandText = "InformeDeDevolucionesCierreDiario";
                oCnn.Open();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.Parameters.Add(new SqlParameter("@Fdesde", _FechaDesde));
                oCmd.Parameters.Add(new SqlParameter("@Fhasta", _FechaHasta));
                oCmd.Connection = oCnn;
                using (SqlDataReader dr = oCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        InformeDeDevolucionesCierreDiario_Retorno.Add(obtenerEntidad(dr));
                    }
                }
                return InformeDeDevolucionesCierreDiario_Retorno;
            }
            catch (Exception err)
            {
                throw (err);
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

        public static InformeDeDevolucionesCierreDiario obtenerEntidad(SqlDataReader dr)
        {
            return new InformeDeDevolucionesCierreDiario(dr["nombreArchivoCoelsaEnvio"].ToString(),
                                                         Int32.Parse(dr["NroInforme"].ToString()),
                                                         dr["Sistema"].ToString(),
                                                         Int64.Parse(dr["CUIL"].ToString()),
                                                         dr["NombreyApellido"].ToString(),
                                                         decimal.Parse(dr["ImporteTotal"].ToString()),
                                                         dr["MotivoDevolucion"].ToString(),
                                                         DateTime.Parse(dr["FechaDevolucion"].ToString()));
        }

        public static List<Mensual> Mensual_Obtener(enum_Proposito proposito)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand oCmd = new SqlCommand();
            List<Mensual> mensuales = new List<Mensual>();
            try
            {
                oCnn = Conexion.ObtenerConnexionSQL();
                oCmd.CommandText = "MensualesYConceptosObtener";
                oCnn.Open();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.Parameters.Add(new SqlParameter("@Proposito", proposito));
                oCmd.Connection = oCnn;
                using (SqlDataReader dr = oCmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        mensuales.Add(obtenerEntidadMensual(dr));
                    }
                }
                return mensuales;
            }
            catch (Exception err)
            {
                throw (err);
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

        public static List<Concepto> Concepto_Obtener(enum_Proposito proposito)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand oCmd = new SqlCommand();
            List<Concepto> conceptos = new List<Concepto>();
            try
            {
                oCnn = Conexion.ObtenerConnexionSQL();
                oCmd.CommandText = "MensualesYConceptosObtener";
                oCnn.Open();
                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.Parameters.Add(new SqlParameter("@Proposito", proposito));
                oCmd.Connection = oCnn;
                using (SqlDataReader dr = oCmd.ExecuteReader())
                {
                    dr.NextResult();
                    while (dr.Read())
                    {
                        conceptos.Add(obtenerEntidadConcepto(dr));
                    }
                }
                return conceptos;
            }
            catch (Exception err)
            {
                throw (err);
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

        public static Mensual obtenerEntidadMensual(SqlDataReader dr)
        {
            return new Mensual(Int32.Parse(dr["Mensual"].ToString()));
        }

        public static Concepto obtenerEntidadConcepto(SqlDataReader dr)
        {
            return new Concepto(Int32.Parse(dr["CodConceptoLiq"].ToString()), dr["DescConceptoLiq"].ToString());
        }

    }
}
