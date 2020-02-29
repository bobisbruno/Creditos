using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    public class AdministradorDeConexion
    {
        private static SqlConnection conn;

        private static void InicializarConexion()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DAT_V01"].ConnectionString);
        }

        public static SqlCommand obtenerSqlComand()
        {
            InicializarConexion();
            SqlCommand command;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            command = conn.CreateCommand();
            command.Connection = conn;

            return command;
        }

        public static void CerrarConexion()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        public static void abrirConexion(SqlCommand command)
        {
            if (command.Connection.State != ConnectionState.Open)
            {
                command.Connection.Open();
            }
        }


    }
}
