using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using AdminInformes.Entidades;


namespace AdminInformes.AccesoDatos
{
    public class ObtenerDatosEjecucionTablero
    {
        private Visualizacion GetObjectVisualizacion(SqlDataReader dr)
        {
            Visualizacion mt = new Visualizacion();
            mt.idVisualizacion = SqlConvert.Convert<Int32>(dr["idVisualizacion"]);
            mt.idTipoVisualizacion = SqlConvert.Convert<Int32>(dr["idTipoVisualizacion"]);
            mt.NombreVisualizacion = SqlConvert.Convert<string>(dr["NombreVisualizacion"]);
            mt.NombreFuncion = SqlConvert.Convert<string>(dr["NombreFuncion"]);
            mt.Contenedor = SqlConvert.Convert<string>(dr["Contenedor"]);
            mt.ObjetoGrafico = SqlConvert.Convert<string>(dr["ObjetoGrafico"]);
            mt.Opciones = SqlConvert.Convert<string>(dr["Opciones"]);
            mt.Paqueterequerido = SqlConvert.Convert<string>(dr["PaqueteRequerido"]);
            mt.Proceso = ObtenerProcesoParaVizualizacion(mt.idVisualizacion);
            mt.lstScriptsRequeridos = ObtenerScriptsRequeridosPorViz(mt.idVisualizacion);

            return mt;
        }
        public TableroConDatos ObtenerTableroConDatos (ItemMenuTablero tbl)
        {
            TableroConDatos tcd = new TableroConDatos();
            tcd.idTablero = tbl.idTablero;
            tcd.NombreTablero = tbl.Nombre;
            tcd.lstVisualizaciones = new List<Visualizacion>();
            tcd.lstPaquetesRequeridos = new List<string>();
            tcd.IncludeScripts = new List<string>();
            //Esto (ObtenerMapeoDeParametros) podria ser un metodo de la clase TableroConDatos, 
            //charlar la diferencia de deseño con Carlos/Gaston
            tcd.MapaParametros = ObtenerMapeoDeParametros(tcd.idTablero);

            string comando = "ObtenerVisualizacionesTablero";
            SqlConnection oCnn = Conexion.ObtenerConnexionSQL();
            SqlCommand cmd = new SqlCommand(comando, oCnn);
            cmd.Parameters.AddWithValue("@idTablero", tcd.idTablero);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {
                oCnn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Visualizacion viz = GetObjectVisualizacion(dr);
                        tcd.lstVisualizaciones.Add(viz);
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
            tcd.Solicitud = tbl;
            return tcd;
        }
        private Query GetObjectQuery(SqlDataReader dr)
        {
            Query mt = new Query();
            mt.idVisualizacion = SqlConvert.Convert<Int32>(dr["idVisualizacion"]);
            mt.idQuery = SqlConvert.Convert<Int32>(dr["idQuery"]);
            mt.NombreProceso = SqlConvert.Convert<string>(dr["NombreProceso"]);
            mt.Servidor = SqlConvert.Convert<string>(dr["Servidor"]);
            mt.Base = SqlConvert.Convert<string>(dr["Base"]);
            mt.lstParametros = ObtenerParametrosQuery(mt.idQuery);

            return mt;
        }
        public Query ObtenerProcesoParaVizualizacion(Int32 idVisualizacion)
        {
            Query q = new Query();

            string comando = "ObtenerQueryParaVisualizacion";
            SqlConnection oCnn = Conexion.ObtenerConnexionSQL();
            SqlCommand cmd = new SqlCommand(comando, oCnn);
            cmd.Parameters.AddWithValue("@idVisualizacion", idVisualizacion);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {
                oCnn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        q = GetObjectQuery(dr);
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

            return q;

        }
        private ParametroTablero GetObjectParametro(SqlDataReader dr)
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
        public List<ParametroTablero> ObtenerParametrosQuery(int idQuery)
        {
            List<ParametroTablero> resultados = new List<ParametroTablero>();
            string comando = "ObtenerParametrosQuery";
            SqlConnection oCnn = Conexion.ObtenerConnexionSQL();
            SqlCommand cmd = new SqlCommand(comando, oCnn);
            cmd.Parameters.AddWithValue("@idQuery", idQuery);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {
                oCnn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ParametroTablero pt = GetObjectParametro(dr);
                        resultados.Add(pt);
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
        private MapeoParametrosTableroQuery GetObjectMapeo(SqlDataReader dr)
        {
            MapeoParametrosTableroQuery mt = new MapeoParametrosTableroQuery();
            mt.idTableroParametro = SqlConvert.Convert<Int32>(dr["idTableroParametro"]);
            mt.idParametro = SqlConvert.Convert<Int32>(dr["idParametro"]);
            return mt;
        }
        public List<MapeoParametrosTableroQuery> ObtenerMapeoDeParametros(Int32 idTablero)
        {
            List<MapeoParametrosTableroQuery> lstMapeo = new List<MapeoParametrosTableroQuery>();

            string comando = "ObtenerMapeoDeParametros";
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
                        MapeoParametrosTableroQuery map = GetObjectMapeo(dr);
                        lstMapeo.Add(map);
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

            return lstMapeo;
        }

        public List<string> ObtenerScriptsRequeridosPorViz (int idVisualizacion)
        {
            List<string> lstStrings = new List<string>();

            string comando = "ObtenerScriptsRequeridosPorViz";
            SqlConnection oCnn = Conexion.ObtenerConnexionSQL();
            SqlCommand cmd = new SqlCommand(comando, oCnn);
            cmd.Parameters.AddWithValue("@idVisualizacion", idVisualizacion);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {
                oCnn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lstStrings.Add(SqlConvert.Convert<string>(dr[1]));
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

            return lstStrings;
        }
    }
}