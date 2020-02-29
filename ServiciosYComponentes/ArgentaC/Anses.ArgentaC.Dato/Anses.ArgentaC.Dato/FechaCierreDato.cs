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
    public static class FechaCierreDato
    {
        public static Anses.ArgentaC.Contrato.FechaCierre Buscar(enum_TipoFecha _tipoFecha)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand oCmd = new SqlCommand();
            Anses.ArgentaC.Contrato.FechaCierre oFechaCierre = new Anses.ArgentaC.Contrato.FechaCierre();
            try
            {
                oCmd.CommandType = CommandType.StoredProcedure;
                switch (_tipoFecha)
                {
                    case enum_TipoFecha.CierreAnterior:
                        oCmd.CommandText = "Cierres_TFecCierreAnt";
                        break;
                    case enum_TipoFecha.CierreProximo:
                        oCmd.CommandText = "Cierres_TProxFecCierre";
                        break;
                    default:
                        return null;
                }
                oCnn = Conexion.ObtenerConnexionSQL();
                oCnn.Open();
                oCmd.Connection = oCnn;
                using (SqlDataReader dr = oCmd.ExecuteReader())
                {
                    while (dr.Read())
                        oFechaCierre = obtenerEntidad(dr);
                    dr.Close();
                }
                return oFechaCierre;
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                    oCnn.Close();
                oCnn.Dispose();
                oCmd.Dispose();
            }
        }

        public static Anses.ArgentaC.Contrato.FechaCierre obtenerEntidad(SqlDataReader dr)
        {
            Anses.ArgentaC.Contrato.FechaCierre oFechaCierre = new Anses.ArgentaC.Contrato.FechaCierre();
            oFechaCierre.Fecha = DateTime.Parse(dr["FecCierre"].ToString());
            oFechaCierre.Mensual = long.Parse(dr["Mensual"].ToString());
            return oFechaCierre;
        }
    }
}
