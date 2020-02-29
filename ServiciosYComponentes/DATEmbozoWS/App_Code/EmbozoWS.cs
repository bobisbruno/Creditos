using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Anses.Embozo.Dominio;
using log4net;
using System.Transactions;
using Anses.Embozo.Utils;

[WebService(Namespace = "http://dat.anses.gov.ar/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class EmbozoWS : System.Web.Services.WebService
{
    private static ILog log = LogManager.GetLogger(typeof(EmbozoWS).Name);

    public EmbozoWS () {}

    [WebMethod(Description = "Trae Tarjetas Pendientes de Embozo")] //EmbozadoAnses_TraerPendientes
    public List<TarjetaEmbozado> EmbozadoAnses_AEmbozarTraer(out int cantTotal, out int cantMostrada, Usuario usuario)
    {
        List<TarjetaEmbozado> lista = new List<TarjetaEmbozado>();
        cantTotal = cantMostrada = 0;

        try
        {
           TarjetaWS.TarjetaWS srv = Utils.instancio_TarjetaWS;

            List<TarjetaWS.TarjetaEmbozado> listaT = srv.EmbozadoAnses_GeneraPendientesEmbozado(Utils.mapToAuditoria(usuario), out cantTotal, out cantMostrada).ToList();

            lista = listaT!=null && listaT.Count > 0? (from a in listaT select new TarjetaEmbozado(a.BeneficioPrincipal, new Persona(long.Parse(a.Cuil), a.ApellidoNombre), a.FechaNovedad)).ToList(): lista;
         
            return lista;
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
            lista = null;
            cantTotal = cantMostrada = 0;
        }

        return lista;
    }

    [WebMethod(Description = "Solicita Embozo tarjeta")]
    public Tarjeta EmbozadoAnses_SolicitudEmbozado(TarjetaEmbozado tarjetaE)
    {
        Tarjeta tarjeta = null;

        try
        {
            TarjetaWS.TarjetaWS srv = Utils.instancio_TarjetaWS;

            srv.EmbozadoAnses_Guardar(Utils.mapToTarjetaEmbozado(tarjetaE, enum_TipoEstadoEmbozado.BuscaDatosTarjetaAEmbozar));
            TarjetaWS.Tarjeta unaTarjeta =  srv.EmbozadoAnses_TraerXCuilEstado(tarjetaE.Persona.Cuil);
                  
            //Llamar servicio TS
            //Actualizar estado en tarjetaEmbozado srv.EmbozadoAnses_Guardar(Utils.mapToTarjetaEmbozado(tarjetaE, enum_TipoEstadoEmbozado.BuscaDatosTarjetaAEmbozar));

        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));           
        }

        return tarjeta;
    }

    [WebMethod(Description = "Solicita Embozo tarjeta")]
    public string EmbozadoAnses_Escaneo(TarjetaEmbozado tarjeta)
    {
        string mensaje = string.Empty;
      
        try
        {
            TarjetaWS.TarjetaWS srv = Utils.instancio_TarjetaWS;

            mensaje = srv.EmbozadoAnses_ValidoEscaneo(tarjeta.Persona.Cuil, tarjeta.NroTarjeta);

            if (string.IsNullOrEmpty(mensaje))
            {
                srv.EmbozadoAnses_Guardar(Utils.mapToTarjetaEmbozado(tarjeta, enum_TipoEstadoEmbozado.TarjetaEmbozadaOK));
                //b.	Actualiza estado tarjeta ABM_Tarjetas Estado 4 – En udai y le tiene q informar el nro de tarjeta.  (falta)
            }
            else 
            {   
                //ingreso observacion manual obligatorio - ver que onda
                srv.EmbozadoAnses_Guardar(Utils.mapToTarjetaEmbozado(tarjeta, enum_TipoEstadoEmbozado.TarjetaEmbozadaErrorManual));
            }
        }
        catch (Exception err)
        {
            log.Error(string.Format("{0} - Error:{1}->{2}", System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
        }

        return mensaje;
    }  
}
