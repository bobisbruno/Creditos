using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentoWS;
using System.Configuration;
using System.Net;
using log4net;

/// <summary>
/// Summary description for DocumentoService
/// </summary>
public class DocumentoService
{
    ILog log = LogManager.GetLogger(typeof(RecuperoDetalleService).Name);
	public DocumentoService()
	{
	}

    public List<Documento> ListarTiposDeDocumentos(Nullable<int> id)
    {
        DocumentoWS.DocumentoWS documentoWsClient = new DocumentoWS.DocumentoWS();
        try
        {
            documentoWsClient.Url = ConfigurationManager.AppSettings[documentoWsClient.GetType().ToString()];
            documentoWsClient.Credentials = CredentialCache.DefaultCredentials;
            log.DebugFormat("Inicio Ejecución:{0} - ListarTiposDeDocumentos({1})", DateTime.Now, id);
            return documentoWsClient.ListarTiposDeDocumentos(id).ToList();
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ListarTiposDeDocumentos", ex.Source, ex.Message));
            throw;
        }
    }
}