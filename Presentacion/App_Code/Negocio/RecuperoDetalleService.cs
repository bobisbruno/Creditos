using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Net;
using log4net;
using RecuperoWS;

/// <summary>
/// Summary description for RecuperoDetalleService
/// </summary>
public class RecuperoDetalleService
{
    ILog log = LogManager.GetLogger(typeof(RecuperoDetalleService).Name);
    public RecuperoDetalleService()
    { }

    public decimal ObtenerMontoMinimoDeRecupero(int idPrestador)
    {
        try
        {
            RecuperoWS.RecuperoWS clientRecuperoWs = new RecuperoWS.RecuperoWS();
            clientRecuperoWs.Url = System.Configuration.ConfigurationManager.AppSettings[clientRecuperoWs.GetType().ToString()];
            clientRecuperoWs.Credentials = System.Net.CredentialCache.DefaultCredentials;
            return clientRecuperoWs.ObtenerValorMinimoDeRecupero(idPrestador);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ObtenerMontoMinimoDeRecupero", ex.Source, ex.Message));
            throw;
        }
    }

    public List<ComboBoxItem> ListarFormasDePago()
    {
        try
        {
            RecuperoWS.RecuperoWS clientRecuperoWs = new RecuperoWS.RecuperoWS();
            clientRecuperoWs.Url = System.Configuration.ConfigurationManager.AppSettings[clientRecuperoWs.GetType().ToString()];
            clientRecuperoWs.Credentials = System.Net.CredentialCache.DefaultCredentials;
            return clientRecuperoWs.ObtenerModalidadDePago().ToList().Select(x => new ComboBoxItem { Id = x.Id, Texto = x.Descripcion }).ToList();
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ListarFormasDePago()", ex.Source, ex.Message));
            throw;
        }
    }
    public WSAltaANME.ExpedienteAG CaratularExpediente(string[] numeroDeBeneficio, DateTime fechaDePresentacion, string tipoDocumento, string numeroDocumento)
    {
        WSAltaANME.AltaGenericaExpteWS wsAltaAnmeClient = new WSAltaANME.AltaGenericaExpteWS();
        try
        {
            wsAltaAnmeClient.Url = ConfigurationManager.AppSettings["WSAltaANME.AltaGenericaExptews"];
            wsAltaAnmeClient.Credentials = CredentialCache.DefaultCredentials;
            WSAltaANME.ExpedienteAG expediente = new WSAltaANME.ExpedienteAG();
            log.DebugFormat("Inicio Ejecución:{0} - GeneraAltaGenericaExpte({1})", DateTime.Now, VariableSession.Cuil);
            expediente = wsAltaAnmeClient.GeneraAltaGenericaExpte("004", "ANS40211", "GE6GENP", "ge07", "33637617449",
                                                     VariableSession.UsuarioLogeado.IdUsuario, "41", "S", "20", "N",
                                                    "024",
                                                    VariableSession.Cuil.Substring(0, 2).ToString(),
                                                    VariableSession.Cuil.Substring(2, 8).ToString(),
                                                    VariableSession.Cuil.Substring(10, 1).ToString(), "435",
                                                    "000000", "", "", numeroDeBeneficio[(int)Constantes.PartesDelBeneficio.excaja].ToString(),
                                                    numeroDeBeneficio[(int)Constantes.PartesDelBeneficio.tipo],
                                                    numeroDeBeneficio[(int)Constantes.PartesDelBeneficio.numero],
                                                    numeroDeBeneficio[(int)Constantes.PartesDelBeneficio.coparticipacion],
                                                    tipoDocumento, numeroDocumento, "",
                                                    fechaDePresentacion.ToShortDateString().Replace('/', '.'),
                                                    VariableSession.UsuarioLogeado.IdUsuario,
                                                    VariableSession.UsuarioLogeado.Oficina,
                                                    "14", "00",
                                                    "", "", "", "", "", "", // Clave del Expediente Relacionado  
                                                    "", //Código de Agregación de la relación entre el Expediente Relacionado y el Expediente a generar
                                                    "", "", "", "", "", //beneficio de causante
                                                    "", "", // tipo doc y causante
                                                    "", //cuil causante
                                                    "", // oficina de entrada
                                                    "", "", "", "", "", "", //actuacion medica
                                                    "", "", "", "01",
                                                     "S", "S", "N", "N",
                                                    "N", "N", "N", "N", "N",
                                                    "N");
            log.DebugFormat("Fin Ejecución:{0} - CaratularExpediente", DateTime.Now);
            return expediente;
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "CaratularExpediente", ex.Source, ex.Message));
            throw;
        }
    }

    public RecuperoWS.RecuperoDetalleForm ObtenerRecuperosPorId(decimal idRecupero,out decimal valorResidualTotal)
    {
        try
        {
            RecuperoWS.RecuperoWS clientRecuperoWs = new RecuperoWS.RecuperoWS();
            clientRecuperoWs.Url = System.Configuration.ConfigurationManager.AppSettings[clientRecuperoWs.GetType().ToString()];
            clientRecuperoWs.Credentials = System.Net.CredentialCache.DefaultCredentials;
            return clientRecuperoWs.ObtenerDatosDeRecupero_TXId(idRecupero, out valorResidualTotal);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ObtenerRecuperosPorId", ex.Source, ex.Message));
            throw;
        }
    }
}