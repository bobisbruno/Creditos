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
public class CodigoPreAprobacion
{
    private static readonly ILog log = LogManager.GetLogger(typeof(CodigoPreAprobacion).Name);

    public static string Novedades_CodigoPreAprobacion_Alta(long Cuil)
    {

         CodigoPreAprobacionWS oServicio = new WSCodigoPreAprobacion.CodigoPreAprobacionWS();
         oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
         oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;        

         try
         {
             return oServicio.Novedades_CodigoPreAprobacion_Alta(Cuil, VariableSession.UsuarioLogeado.DirIP, VariableSession.UsuarioLogeado.IdUsuario);
         }
         catch (Exception ex)
         {
             log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
             throw ex;
         }
     }

    public static string Novedades_CodigoPreAprobacion_Modificacion(WSCodigoPreAprobacion.CodigoPreAprobado unCodigoPreAprobado)
    {

         WSCodigoPreAprobacion.CodigoPreAprobacionWS oServicio = new WSCodigoPreAprobacion.CodigoPreAprobacionWS();
         oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
         oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;
         string MsgError = string.Empty;

         try
         {
             MsgError = oServicio.Novedades_CodigoPreAprobacion_Modificacion(unCodigoPreAprobado);
             if(!string.IsNullOrEmpty(MsgError))
                 MsgError = String.Format("\n No se pudo relacionar la Novedad ({0}) con el código de Pre Aprobación ({1})", unCodigoPreAprobado.IdNovedad, unCodigoPreAprobado.CodigoAValidar);
             return MsgError;
         }
         catch (Exception ex)
         {
             log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
             MsgError = String.Format("No se pudo relacionar la Novedad ({0}) con el código de Pre Aprobación ({1}). El mensaje original es : {2} ", unCodigoPreAprobado.IdNovedad, unCodigoPreAprobado.CodigoAValidar, ex.Message);
             throw new Exception(MsgError);
         }
     }

    public static string Novedades_CodigoPreAprobacion_Valida(WSCodigoPreAprobacion.CodigoPreAprobado unCodigoPreAprobado)
    {

         WSCodigoPreAprobacion.CodigoPreAprobacionWS oServicio = new WSCodigoPreAprobacion.CodigoPreAprobacionWS();
         oServicio.Url = ConfigurationManager.AppSettings[oServicio.GetType().ToString()];
         oServicio.Credentials = System.Net.CredentialCache.DefaultCredentials;        

         try
         {
             return oServicio.Novedades_CodigoPreAprobacion_Valida(unCodigoPreAprobado);
         }
         catch (Exception ex)
         {
             log.Error(string.Format("ERROR Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
             throw ex;
         }
     }

    public static WSCodigoPreAprobacion.CodigoPreAprobado mapCodigoPreAprobado(long cuil, string codigoAValidar, long? idNovedad, WSCodigoPreAprobacion.TipoUsoCodPreAprobado tipoUso)
    {
        WSCodigoPreAprobacion.CodigoPreAprobado codigo = new WSCodigoPreAprobacion.CodigoPreAprobado();
        codigo.Cuil = cuil;
        codigo.CodigoAValidar = codigoAValidar;
        codigo.IdNovedad = idNovedad;
        codigo.unTipoUso = tipoUso;
        codigo.UnAuditoria = new WSCodigoPreAprobacion.Auditoria();
        codigo.UnAuditoria.Usuario = VariableSession.UsuarioLogeado.IdUsuario;
        codigo.UnAuditoria.IP = VariableSession.UsuarioLogeado.DirIP;

        return codigo;
    }

    public static WSTarjeta.CodigoPreAprobado mapCodigoPreAprobado(long cuil, string codigoAValidar, long? idNovedad, WSTarjeta.TipoUsoCodPreAprobado tipoUso)
    {
        WSTarjeta.CodigoPreAprobado codigo = new WSTarjeta.CodigoPreAprobado();
        codigo.Cuil = cuil;
        codigo.CodigoAValidar = codigoAValidar;
        codigo.IdNovedad = idNovedad;
        codigo.unTipoUso = tipoUso;
        codigo.UnAuditoria = new WSTarjeta.Auditoria();
        codigo.UnAuditoria.Usuario = VariableSession.UsuarioLogeado.IdUsuario;
        codigo.UnAuditoria.IP = VariableSession.UsuarioLogeado.DirIP;

        return codigo;
    }
    public static WSCodigoPreAprobacion.CodigoPreAprobado mapCodigoPreAprobado(long cuil, string codigoAValidar)
    {
        WSCodigoPreAprobacion.CodigoPreAprobado codigo = new WSCodigoPreAprobacion.CodigoPreAprobado();
        codigo.Cuil = cuil;
        codigo.CodigoAValidar = codigoAValidar;

        return codigo;
    }  
}