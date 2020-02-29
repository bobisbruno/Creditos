using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using AdminInformes.Entidades;

namespace AdminInformes.AccesoDatos
{
    public class ParametrosTablero
    {
        private ParametroTablero GetObject(SqlDataReader dr)
        {
            ParametroTablero mt = new ParametroTablero();
            mt.idParametro = SqlConvert.Convert<Int32>(dr["idParametro"]);
            mt.NombreParametro = SqlConvert.Convert<string>(dr["NombreParametro"]);
            mt.AliasParametro = SqlConvert.Convert<string>(dr["AliasParametro"]);
            mt.TipoDeDato = SqlConvert.Convert<string>(dr["TipoDeDato"]);
            mt.Presicion = SqlConvert.Convert<int>(dr["Presicion"]);
            mt.Escala = SqlConvert.Convert<int>(dr["Escala"]);
            mt.ValorMinimo = SqlConvert.Convert<string>(dr["ValorMinimo"]);
            mt.ValorMaximo = SqlConvert.Convert<string>(dr["ValorMaximo"]);
            mt.ValMinTipoMetodoObtencion = SqlConvert.Convert<byte>(dr["ValMinTipoMetodoObtencion"]);
            mt.ValMaxTipoMetodoObtencion = SqlConvert.Convert<byte>(dr["ValMaxTipoMetodoObtencion"]);
            mt.QueryDominio = SqlConvert.Convert<string>(dr["QueryDominio"]);
            mt.InterfaseIngreso = SqlConvert.Convert<string>(dr["InterfaseIngreso"]);
            return mt;
        }

        public List<ParametroTablero> ObtenerParametrosTablero(int idTablero)
        {
            List<ParametroTablero> resultados = new List<ParametroTablero>();
            string comando = "ObtenerParametros";
            SqlConnection oCnn = Conexion.ObtenerConnexionSQL();
            SqlCommand cmd = new SqlCommand(comando, oCnn);
            cmd.Parameters.AddWithValue("@idTablero", idTablero);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {
                oCnn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ParametroTablero resultado = GetObject(dr);
                        resultados.Add(resultado);
                    }
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
            return resultados;
        }

       
    }
}