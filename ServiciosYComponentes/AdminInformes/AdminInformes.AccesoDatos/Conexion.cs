using System.Data.SqlClient;
using System.Configuration;
using AdminInformes.Entidades;
using System.Collections.Generic;

namespace AdminInformes.AccesoDatos
{
    internal class Conexion
    {
        public static SqlConnection ObtenerConnexionSQL()
        {
            string _conString = ConfigurationManager.ConnectionStrings["SQLServerConnectionString"].ConnectionString;
            return new SqlConnection(_conString);
        }

        public static SqlConnection ObtenerConnexion(string Base)
        {
            string _conString = ConfigurationManager.ConnectionStrings[Base].ConnectionString;
            return new SqlConnection(_conString);
        }
    }

  
}

