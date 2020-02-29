using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using System.Configuration;
using WSCodigoPreAprobacion;

/// <summary>
/// Summary description for CodigoPreAprobacion
/// </summary>

[Serializable]
public class Siniestro
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Siniestro).Name);
        
    public static List<WSSiniestro.TipoEstadoSiniestro> TipoEstadoSiniestro_Traer()
    {
        WSSiniestro.SiniestroWS oServicio = new WSSiniestro.SiniestroWS();
        oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
        oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
        List<WSSiniestro.TipoEstadoSiniestro> oList = new List<WSSiniestro.TipoEstadoSiniestro>();

        try
        {
            oList = new List<WSSiniestro.TipoEstadoSiniestro>(oServicio.TipoEstadoSiniestro_Traer());

            return oList;
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            throw ex;
        }
        finally
        {
            oServicio.Dispose();
        }
    }

    public static List<WSSiniestro.TipoPolizaSeguro> TipoPolizaSeguro_Traer()
    {
        WSSiniestro.SiniestroWS oServicio = new WSSiniestro.SiniestroWS();
        oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
        oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
        List<WSSiniestro.TipoPolizaSeguro> oList = new List<WSSiniestro.TipoPolizaSeguro>();

        try
        {
            oList = new List<WSSiniestro.TipoPolizaSeguro>(oServicio.TipoPolizaSeguro_Traer());

            return oList;
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            throw ex;
        }
        finally
        {
            oServicio.Dispose();
        }
    }
    
    public static List<WSSiniestro.Usuario> OperadorSiniestro_Traer(string idOperador)
    {
        WSSiniestro.SiniestroWS oServicio = new WSSiniestro.SiniestroWS();
        oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
        oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
        List<WSSiniestro.Usuario> oList = new List<WSSiniestro.Usuario>();

        try
        {
            oList = new List<WSSiniestro.Usuario>(oServicio.OperadorSiniestro_Traer(idOperador));

            return oList;
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            throw ex;
        }
        finally
        {
            oServicio.Dispose();
        }
    }

    public static List<WSSiniestro.NovedadSiniestro> NovedadSiniestrosCobrado_Traer(int? idEstado, int? idPolizaSeguro, bool? esGraciable, string operador, long idNovedad, string cuil, int idResumen,
                                                                                    DateTime? fFallecimientoDesde, DateTime? fFallecimientoHasta,int nroPagina,out int cantTotal, out int idUltimoResumen, 
                                                                                    out int cantUltimoResumen, out int cantPaginas)
    {
        WSSiniestro.SiniestroWS oServicio = new WSSiniestro.SiniestroWS();
        oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
        oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
        List<WSSiniestro.NovedadSiniestro> oList = new List<WSSiniestro.NovedadSiniestro>();

        try
        {
            oList = (List<WSSiniestro.NovedadSiniestro>)oServicio.NovedadSiniestrosCobrado_Traer(idEstado, idPolizaSeguro, esGraciable, operador, VariableSession.UsuarioLogeado.IdUsuario, 
                                                                                                 idNovedad, cuil, idResumen, fFallecimientoDesde, fFallecimientoHasta,
                                                                                                 nroPagina, out cantTotal, out idUltimoResumen, out cantUltimoResumen, out cantPaginas).ToList();

            return oList;
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            throw ex;
        }
        finally
        {
            oServicio.Dispose();
        }
    }

    public static List<WSSiniestro.NovedadSiniestroResumen> NovedadSiniestrosResumen_Traer(int idResumen)
    {
        WSSiniestro.SiniestroWS oServicio = new WSSiniestro.SiniestroWS();
        oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
        oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
        List<WSSiniestro.NovedadSiniestroResumen> oList = new List<WSSiniestro.NovedadSiniestroResumen>();

        try
        {
            oList = (List<WSSiniestro.NovedadSiniestroResumen>)oServicio.NovedadSiniestrosResumen_Traer(idResumen).ToList();

            return oList;
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            throw ex;
        }
        finally
        {
            oServicio.Dispose();
        }
    }

    public static List<string> NovedadSiniestrosResumen_TraerTXT(int idResumen)
    {
        WSSiniestro.SiniestroWS oServicio = new WSSiniestro.SiniestroWS();
        oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
        oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
        List<string> oList;

        try
        {
            oList = (List<string>)oServicio.NovedadSiniestroResumen_TraerTXT(idResumen).ToList();

            return oList;
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            throw ex;
        }
        finally
        {
            oServicio.Dispose();
        }
    }

    public static void NovedadSiniestrosCobrado_CambioEstado(string novAsignar, int idEstado, string idOperador, WSSiniestro.Usuario usuario)
    {
        WSSiniestro.SiniestroWS oServicio = new WSSiniestro.SiniestroWS();
        oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
        oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;      

        try
        {
            oServicio.NovedadSiniestrosCobrado_CambioEstado(novAsignar, idEstado, idOperador, usuario);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            throw ex;
        }
        finally
        {
            oServicio.Dispose();
        }
    }

    public static int NovedadSiniestrosResumen_Alta(List<WSSiniestro.NovedadSiniestro> novedades, string idOperador, WSSiniestro.Usuario usuario, int? idPolizaSeguro, bool? esGraciable, int idResumenAgregar, out string mensaje)
    {
        WSSiniestro.SiniestroWS oServicio = new WSSiniestro.SiniestroWS();
        oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
        oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
        int idResumen = 0;

        try
        {
            idResumen = oServicio.NovedadSiniestrosResumen_Alta(novedades.ToArray(), idOperador, usuario, idPolizaSeguro, esGraciable, idResumenAgregar, out mensaje);
            return idResumen;
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            throw ex;
        }
        finally
        {
            oServicio.Dispose();
        }
    }

    public static WSSiniestro.TipoCuentaBancariaSiniestro TipoCuentaBancariaSiniestro_Traer()
    {
        WSSiniestro.SiniestroWS oServicio = new WSSiniestro.SiniestroWS();
        oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
        oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

        try
        {
            return oServicio.TipoCuentaBancariaSiniestro_Traer();
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            throw ex;
        }
        finally
        {
            oServicio.Dispose();
        }
    }

    public static void NovedadSiniestrosImpresion_Alta(int idSiniestro, int idResumen, Constantes.TipoDocumentoImpreso tipoDocumentoImpreso)
    {
        WSSiniestro.SiniestroWS oServicio = new WSSiniestro.SiniestroWS();
        oServicio.Url = System.Configuration.ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
        oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;

        try
        {
            WSSiniestro.NovedadSiniestroImpresion novedad = new WSSiniestro.NovedadSiniestroImpresion();
            novedad.IdSiniestro = idSiniestro;
            novedad.IdResumen = idResumen;
            novedad.IdDocumentoImpreso = Convert.ToInt16(tipoDocumentoImpreso);

            WSSiniestro.Usuario usuario = new WSSiniestro.Usuario();
            usuario.Legajo = VariableSession.UsuarioLogeado.IdUsuario;
            usuario.OficinaCodigo = VariableSession.UsuarioLogeado.Oficina;
            usuario.Ip = VariableSession.UsuarioLogeado.DirIP;
            novedad.Usuario = usuario;
            
            oServicio.NovedadSiniestroImpresion_Alta(novedad);
        }
        catch (Exception ex)
        {
            log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
            throw ex;
        }
        finally
        {
            oServicio.Dispose();
        }
    }   
}