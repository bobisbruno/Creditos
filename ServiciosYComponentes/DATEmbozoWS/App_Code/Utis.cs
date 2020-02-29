using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Anses.Embozo.Dominio;

namespace Anses.Embozo.Utils
{
    public class Utils
    {
        public static TarjetaWS.TarjetaWS instancio_TarjetaWS
        {
            get
            {
                TarjetaWS.TarjetaWS srv = new TarjetaWS.TarjetaWS();

                srv.Url = System.Configuration.ConfigurationManager.AppSettings[srv.GetType().ToString()];
                srv.Credentials = System.Net.CredentialCache.DefaultCredentials;

                return srv;
            }
        }
	    
	   public static TarjetaWS.Auditoria mapToAuditoria(Usuario usuario)
       {
            TarjetaWS.Auditoria unaAuditoria = new TarjetaWS.Auditoria();
            unaAuditoria.Usuario = usuario.CodigoUsuario;
            unaAuditoria.IDOficina = int.Parse(usuario.Oficina);
            unaAuditoria.IP = usuario.IP;

            return unaAuditoria;
       }

       public static TarjetaWS.TarjetaEmbozado mapToTarjetaEmbozado(TarjetaEmbozado tarjeta, enum_TipoEstadoEmbozado idEstadoEmbozado)
       {
           TarjetaWS.TarjetaEmbozado unaTarjeta = new TarjetaWS.TarjetaEmbozado();
           unaTarjeta.Cuil = tarjeta.Persona.Cuil.ToString();
           unaTarjeta.ApellidoNombre = tarjeta.Persona.ApellidoNombre;
           unaTarjeta.BeneficioPrincipal = tarjeta.NroBeneficiario;
           unaTarjeta.FechaNovedad = tarjeta.FechaNovedad;
           unaTarjeta.IdEstadoEmbozado = int.Parse(idEstadoEmbozado.ToString());
           unaTarjeta.UnaAuditoria = mapToAuditoria(tarjeta.Usuario);
           unaTarjeta.Observaciones = tarjeta.Observacion;

           return unaTarjeta;
       }      
    }
}