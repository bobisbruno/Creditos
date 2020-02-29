using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using log4net;
using System.Net;

namespace ANSES.Microinformatica.DAT.Negocio
{
    [Serializable]
    public static class Sucursal
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Sucursal).Name);
	
        public static List<WSSucursales.Sucursal> SucursalCorreo_TXPrestador( long idPrestador)
        {

            WSSucursales.SucursalesWS oServicio = new WSSucursales.SucursalesWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSSucursales.SucursalesWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSSucursales.Sucursal> oListSucursal = null;

            try
            {
               oListSucursal = new List<WSSucursales.Sucursal>(oServicio.SucursalCorreo_TXPrestador(idPrestador));
                
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Error Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                return null;
            }
            finally
            {
                oServicio.Dispose();
            }

            return oListSucursal;
        }

        public static List<WSSucursales.Regional> RegionalUdaiExternoTraer()
        {

            WSSucursales.SucursalesWS oServicio = new WSSucursales.SucursalesWS();
            oServicio.Url = ConfigurationManager.AppSettings["WSSucursales.SucursalesWS"];
            oServicio.Credentials = CredentialCache.DefaultCredentials;
            List<WSSucursales.Regional> oListRegional = null;

            try
            {
                oListRegional = new List<WSSucursales.Regional>(oServicio.RegionalUdaiExternoTraer());

            }
            catch (Exception ex)
            {
                log.Error(string.Format("Error Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                return null;
            }
            finally
            {
                oServicio.Dispose();
            }

            return oListRegional;
        }

        public static List<WSSucursales.UDAI> ObtenerUdaiCercanaADomicilio(Int16 idProvincia, Int32 codigoPostal, out Int32 udaiCercanaDomicilio)
        {
            try
            {
                WSSucursales.SucursalesWS sucursalesWsClient = new WSSucursales.SucursalesWS();
                sucursalesWsClient.Url = ConfigurationManager.AppSettings["WSSucursales.SucursalesWS"];
                sucursalesWsClient.Credentials = CredentialCache.DefaultCredentials;
                return sucursalesWsClient.Traer_UdaiExterno_TXProvincia_CodPostal(idProvincia, codigoPostal,out udaiCercanaDomicilio).ToList();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Error Ejecución:{0}->{1} - Error:{2}->{3}", DateTime.Now, "ObtenerUdaiCercanaADomicilio({0},{1})", ex.Source, ex.Message, idProvincia, codigoPostal));
                throw;{}
            }
        
        }       
    }
}