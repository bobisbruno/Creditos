using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using AdminInformes.Entidades;


namespace AdminInformes.AccesoDatos
{
    public class ObtenerDataset
    {
        public void PonerDatosEnTablero(ref TableroConDatos tcd)
        {
            //TODO: pasar la logica de usar MapeoParametrosTableroQuery para asignar valores a los parametros a la capa de negocio.
            foreach (Visualizacion v in tcd.lstVisualizaciones)
            {
                foreach (ParametroTablero paramTablero in tcd.Solicitud.Parametros)
                {
                    foreach (MapeoParametrosTableroQuery mp in tcd.MapaParametros)
                    {
                        if (paramTablero.idParametro == mp.idTableroParametro)
                        {
                            foreach (ParametroTablero paramQuery in v.Proceso.lstParametros)
                            {
                                if (paramQuery.idParametro==mp.idParametro)
                                {
                                    paramQuery.ValorActual = paramTablero.ValorActual;
                                }
                            }
                        }

                    }
                }
                v.lstDatasets = EjecutarQuery(v.Proceso, v.idTipoVisualizacion);
            }


        }
        public List<string> EjecutarQuery(Query q, Int32 idTipoVisualizacion)
        {
            List<string> DataSets = new List<string>();
            DataTable dt = new DataTable();
            CultureInfo esAR = new CultureInfo("es-AR");
            int intVal;
            DateTime datetimeVal;
            
            bool r;
            //r = DateTime.TryParseExact("01/09/2017 12:00:00 a.m.", "YYYYMMDD", esAR, DateTimeStyles.None, out datetimeVal);
            //r = DateTime.TryParse("2017-09-02", esAR, DateTimeStyles.None, out datetimeVal);

            using (SqlConnection oCnn = Conexion.ObtenerConnexion(q.Base))
            {
                using (SqlCommand cmd = new SqlCommand(q.NombreProceso, oCnn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (ParametroTablero paramQuery in q.lstParametros)
                    {
                        SqlParameter par = new SqlParameter();
                        switch (paramQuery.TipoDeDato.ToUpper())
                        {
                            //TODO: Agregar todos los tipos de datos al SWITCH.
                            case "INT":
                                par.SqlDbType = SqlDbType.Int;
                                r = Int32.TryParse(paramQuery.ValorActual, out intVal);
                                par.Value = paramQuery.ValorActual;//intVal;
                                break;
                            case "DATE":
                                par.SqlDbType = SqlDbType.Date;
                                r = DateTime.TryParseExact(paramQuery.ValorActual, "YYYYMMDD", esAR, DateTimeStyles.None, out datetimeVal);
                                par.Value = paramQuery.ValorActual;//datetimeVal;
                                break;
                        }
                        par.ParameterName = paramQuery.NombreParametro;
                        //par.Value = paramQuery.ValorActual;
                        cmd.Parameters.Add(par);
                    }

                    oCnn.Open();
                    //SqlDataAdapter ada = new SqlDataAdapter(cmd);
                    //ada.Fill(dt);

                    SqlDataReader dataReader = cmd.ExecuteReader();
                    

                    switch (idTipoVisualizacion)
                    {
                        case 1:
                            DataSets.Add(ConvertirReaderEnJSONstring(dataReader));
                            //DataSets.Add(ConvertirDataTableEnJSONstring(dt));
                            break;
                    }
                }
            }
            return DataSets;
        }
        //TODO: Modificar para devolver mas de un dataset.
        private string ConvertirReaderEnJSONstring(SqlDataReader dr)
        {
            //TODO: Pasar a stringbuilder.
            string strData="";
            string strFilas = "";
            if (dr.HasRows)
            {
                strData = "{cols:[";
                for (int i=0;i<dr.FieldCount;i++)
                {
                    if (i > 0) { strData += ","; }
                    strData += "{id: '" + dr.GetName(i) + "',label:'" + dr.GetName(i) + "',type:'" + TipoSQLaJSONscriptGChart(dr.GetFieldType(i)) + "'}";
                }
                strData += "],rows:[";
                while (dr.Read())
                {
                    if (strFilas.Length > 0) { strFilas += ','; }
                    strFilas += "{c:[";
                    for (int i=0; i<dr.FieldCount;i++)
                    {
                        if (i > 0) { strFilas += ","; }

                        switch (TipoSQLaJSONscriptGChart(dr.GetFieldType(i)))
                        {
                            case "string":
                                strFilas += "{v:'" + dr.GetValue(i).ToString() + "'}";
                                break;
                            case "number":
                                strFilas += "{v:" + dr.GetValue(i).ToString() + "}";
                                break;
                            case "boolean":
                                strFilas += "{v:" + dr.GetValue(i).ToString() + "}";
                                break;
                            case "datetime":
                                strFilas += "{v: new Date("
                                    + dr.GetDateTime(i).Year.ToString() + ","
                                    + (dr.GetDateTime(i).Month - 1).ToString() + ","
                                    + dr.GetDateTime(i).Day.ToString() + ")}";
                                break;

                        }
                        
                    }
                    strFilas += "]}";
                }
                strData += strFilas+"]}";
            }

            return strData;
        }
        private string TipoSQLaJSONscriptGChart(Type t)
        {
            if (
                t.Equals(typeof(byte)) ||
                t.Equals(typeof(sbyte)) ||
                t.Equals(typeof(short)) ||
                t.Equals(typeof(ushort)) ||
                t.Equals(typeof(int)) ||
                t.Equals(typeof(uint)) ||
                t.Equals(typeof(long)) ||
                t.Equals(typeof(ulong)) ||
                t.Equals(typeof(float)) ||
                t.Equals(typeof(double)) ||
                t.Equals(typeof(decimal))
                )
            { return "number"; }
            else if (t.Equals(typeof(string)))
            { return "string"; }
            else if (t.Equals(typeof(bool)))
            { return "boolean"; }
            else if (t.Equals(typeof(DateTime)))
            { return "datetime"; }
            else
            { return ""; }
        }

    }
}