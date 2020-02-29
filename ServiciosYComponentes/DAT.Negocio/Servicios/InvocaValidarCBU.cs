using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Anses.DAT.Negocio.Servicios
{
    
    public static class InvocaValidarCBU
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(InvocaValidarCBU).Name);
        public static WSValidarCBU.Retorno ValidarCBU(string Cuil, string CBU)
        {
            WSValidarCBU.Retorno retorno = new WSValidarCBU.Retorno();
            try
            {
                WSValidarCBU.ValidarCBU srv = new WSValidarCBU.ValidarCBU();
                srv.Url = System.Configuration.ConfigurationManager.AppSettings["WSValidarCBU.ValidarCBU"];
                srv.Credentials = System.Net.CredentialCache.DefaultCredentials;
                var tiempo = Stopwatch.StartNew();
                log.DebugFormat("Ejecuto el servicio - Externo COELSA - WSValidarCBU ValidarCBU (Cuil:{0}, CBU:{1})", Cuil, CBU);

                retorno = srv.Validar(Cuil, CBU);

                tiempo.Stop();

                log.InfoFormat("el servicio {0} tardo {1} ", "ValidarCBU", tiempo.Elapsed);

                return retorno;

            }
            catch (Exception ex)
            {
                String.Format("Validar()  ->  cuil:{0}, CBU:{1}", Cuil, CBU);
                log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }

        }
    }
}
