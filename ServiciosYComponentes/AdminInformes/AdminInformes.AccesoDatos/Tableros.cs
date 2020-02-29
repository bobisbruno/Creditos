using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using AdminInformes.Entidades;

namespace AdminInformes.AccesoDatos
{

    public class Tableros
    {
        private ParametrosTablero parametros;

        public Tableros ()
        {
            parametros = new ParametrosTablero();
        }
        private ItemMenuTablero GetObject(SqlDataReader dr)
        {
            ItemMenuTablero mt = new ItemMenuTablero();
            mt.idTablero = SqlConvert.Convert<Int32>(dr["idTablero"]);
            mt.Nombre = SqlConvert.Convert<string>(dr["NombreTablero"]);
            mt.Parametros = this.parametros.ObtenerParametrosTablero(mt.idTablero);
            return mt;
        }

        public List<ItemMenuTablero> ObtenerTableros()
        {
            List<ItemMenuTablero> resultados = new List<ItemMenuTablero>();
            string comando = "ObtenerTableros";
            SqlConnection oCnn = Conexion.ObtenerConnexionSQL();
            SqlCommand cmd = new SqlCommand(comando, oCnn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            try
            {
                oCnn.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ItemMenuTablero resul = GetObject(dr);
                        resultados.Add(resul);
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