using IBM.Data.DB2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Activos_ServiciosComplementarios.Dato
{
    public class AdministradorDeConexion
    {

        public static DB2Connection ObtenerConnexion()
        {
            DB2Connection cnn = new DB2Connection(ConfigurationManager.ConnectionStrings["DB2ConnectionString"].ConnectionString);
            return cnn;
        }

        public static DB2Command ObtenerComando()
        {
            DB2Command cmd = new DB2Command();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            return cmd;
        }


        public static string GetSchema() 
        {
            return System.Configuration.ConfigurationManager.AppSettings["Schema"];
        }


    }
}
