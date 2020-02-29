using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

/// <summary>
/// Clase encargada de "interfacear" la presentacion con el backend
/// </summary>
public class RecuperoService
{
    private static readonly ILog log = LogManager.GetLogger(typeof(RecuperoService).Name);

	public RecuperoService()
	{
	}

    /// <summary>
    /// El metodo se encarga de listar los Estados de recupero necesarios en la UI para construir el filtro
    /// </summary>
    /// <returns></returns>
    public List<RecuperoWS.ComboBoxItem> ListarTipoEstadoRecupero()
    {
        RecuperoWS.RecuperoWS service = new RecuperoWS.RecuperoWS();
        try
        {
            service.Url = System.Configuration.ConfigurationManager.AppSettings[service.GetType().ToString()];
            service.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<RecuperoWS.ComboBoxItem> tipoEstadoRecuperoList = service.ObtenerTipoEstadoRecupero_TT().Select(x => new RecuperoWS.ComboBoxItem { Id = x.idEstadorecupero, Texto = x.descripcionEstadoRecupero }).ToList();
            tipoEstadoRecuperoList.Insert(0, new RecuperoWS.ComboBoxItem { Id = -1, Texto = "Seleccione" });
            return tipoEstadoRecuperoList;
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ListarTipoEstadoRecupero", ex.Source, ex.Message));
            throw;
        }
    }


    /// <summary>
    /// el metodo se encarga de listar todos los motivos para utilizarse en la UI para construir el filtro
    /// </summary>
    /// <returns></returns>
    public List<RecuperoWS.ComboBoxItem> ListarTipoMotivoRecupero()
    {
        RecuperoWS.RecuperoWS service = new RecuperoWS.RecuperoWS();
        try
        {
            service.Url = System.Configuration.ConfigurationManager.AppSettings[service.GetType().ToString()];
            service.Credentials = System.Net.CredentialCache.DefaultCredentials;
            List<RecuperoWS.ComboBoxItem> tipoMotivoRecuperoList = service.ObtenerTipoMotivoRecupero_TT().Select(x => new RecuperoWS.ComboBoxItem { Id = x.Id, Texto = x.DescripcionMotivoRecupero }).ToList();
            tipoMotivoRecuperoList.Insert(0, new RecuperoWS.ComboBoxItem { Id = -1, Texto = "Seleccione" });
            return tipoMotivoRecuperoList;
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ListarTipoMotivoRecupero", ex.Source, ex.Message));
            throw;
        }
    }


    /// <summary>
    /// El metodo se encarga de listar los recuperos almacenados utililando el filtro del parametro
    /// </summary>
    /// <param name="filtro"></param>
    /// <returns></returns>
    public RecuperoWS.GestionRecuperoForm ListarRecuperosPorFiltro(RecuperoWS.FiltroDeRecuperos filtro)
    {
        RecuperoWS.RecuperoWS service = new RecuperoWS.RecuperoWS();
        try
        {
            service.Url = System.Configuration.ConfigurationManager.AppSettings[service.GetType().ToString()];
            service.Credentials = System.Net.CredentialCache.DefaultCredentials;
            return service.Buscar_Recupero_T(filtro);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ListarRecuperosPorFiltro", ex.Source, ex.Message));
            throw;
        }    
    }   
}

[Serializable]
public class PrestadorRecupero
{
    public long CUIT { get; set; }
    public string RazonSocial { get; set; }
    public int? RecuperaSobreConcepto { get; set; }
    public decimal? ValorResidual { get; set; }

    public PrestadorRecupero() { }

    public PrestadorRecupero(long _CUIT, string _RazonSocial, int? _RecuperaSobreConcepto, decimal? _ValorResidual) 
    {
        CUIT = _CUIT;
        RazonSocial = _RazonSocial;
        RecuperaSobreConcepto = _RecuperaSobreConcepto;
        ValorResidual = _ValorResidual;
    }
}