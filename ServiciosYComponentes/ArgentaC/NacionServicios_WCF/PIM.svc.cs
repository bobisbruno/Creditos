using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Configuration;
using System.Text;
using System.Net;
using System.Net.Security;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using log4net;


namespace NacionServicios_WCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PIM" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PIM.svc or PIM.svc.cs at the Solution Explorer and start debugging.
    [ServiceContract]
    public class PIM 
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PIM).Name);
        [OperationContract]
        public Anses.NacionServicios.Contrato.resultadoConsultaCBU PIM_Consulta(string cuil)
        {
            Anses.NacionServicios.Contrato.resultadoConsultaCBU unPIM = new Anses.NacionServicios.Contrato.resultadoConsultaCBU();
            string ruta = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["Config.log4Net"]);
            System.IO.FileInfo arch = new System.IO.FileInfo(ruta);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(arch);
            log.Info("App Started");

            try
            {
                log.DebugFormat("Metodo PIM_Consulta, CUIL --> {0}", cuil);
                log.DebugFormat("*** Valida Certificado ***");
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(new UTF8Encoding().GetBytes(ConfigurationManager.AppSettings["User"] + ":" +
                                                                                                                                                       ConfigurationManager.AppSettings["Pass"])));

                dynamic parametros = new JObject();
                parametros.cuil = cuil;
                parametros.invokerApp = ConfigurationManager.AppSettings["invokerApp"];
                HttpRequestMessage reqmsg = new HttpRequestMessage();
                reqmsg.Content = new StringContent(parametros.ToString(), Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync(new Uri(ConfigurationManager.AppSettings["URI_NacionServicios"]), reqmsg.Content).GetAwaiter().GetResult();
                log.DebugFormat("*** RESPONSE *** {0}", response);
                unPIM = JsonConvert.DeserializeObject<Anses.NacionServicios.Contrato.resultadoConsultaCBU>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult());
                log.Debug("*** Json ***");
                log.DebugFormat("{0}", unPIM.descResult);
                log.DebugFormat("{0}", unPIM.subscriber.CBU);
                return unPIM;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod().Name, err.Source, err.Message));
                return unPIM;
            }
        }
        public static bool ValidateServerCertificate(Object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
