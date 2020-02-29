using Activos_ServiciosComplementarios.Entidades;
using IBM.Data.DB2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Activos_ServiciosComplementarios.Dato
{
    public static class ActivosServiciosComplementariosDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ActivosServiciosComplementariosDAO).Name);

        public static List<DatosDeConsultaAltaTemprana> ConsultaAltaTemprana(decimal cuil) 
        {
            log.InfoFormat("Se ejecuta el método YH_ConsultaAltaTemprana con el parámetro {0}", cuil);

            List<DatosDeConsultaAltaTemprana> response = new List<DatosDeConsultaAltaTemprana>();
            DB2Command cmd = AdministradorDeConexion.ObtenerComando();
            using (DB2Connection conexion = AdministradorDeConexion.ObtenerConnexion())
            {
                try
                {
                    cmd.CommandText = string.Format("{0}.YH_ConsultaAltaTemprana", AdministradorDeConexion.GetSchema());
                    cmd.Parameters.Add(new DB2Parameter("@cuil", cuil));
                    cmd.Connection = conexion;
                    conexion.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        response.Add(Adapter.toConsultaAltaTempranaResponse(reader));
                    }
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("Se produjo un error en el método YH_ConsultaAltaTemprana {0} - {1} - {2}", ex.Message, ex.Source, ex.StackTrace);
                    throw ex;
                }
                finally
                {
                    if (conexion.IsOpen)
                    {
                        conexion.Close();
                    }
                    conexion.Dispose();
                    cmd.Dispose();
                }
            }
            return response;
        }

        public static List<DatosDeConsultaCondenadoProcesado> ConsultaCondenadoProcesado(decimal cuil) 
        {
            log.InfoFormat("Se ejecuta el método YH_ConsultaCondenadoProcesado con el parámetro {0}", cuil);
            List<DatosDeConsultaCondenadoProcesado> response = new List<DatosDeConsultaCondenadoProcesado>();
            DB2Command cmd = AdministradorDeConexion.ObtenerComando();
            using (DB2Connection conexion = AdministradorDeConexion.ObtenerConnexion())
            {
                try
                {
                    cmd.CommandText = string.Format("{0}.YH_ConsultaCondenadoProcesado", AdministradorDeConexion.GetSchema());
                    cmd.Parameters.Add(new DB2Parameter("@CUIL", cuil));
                    cmd.Connection = conexion;
                    conexion.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        response.Add(Adapter.toConsultaCondenadoProcesadoResponse(reader));
                    }
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("Se produjo un error en el método YH_ConsultaCondenadoProcesado {0} - {1} - {2}", ex.Message, ex.Source, ex.StackTrace);
                    throw ex;
                }
                finally
                {
                    if (conexion.IsOpen)
                    {
                        conexion.Close();
                    }
                    conexion.Dispose();
                    cmd.Dispose();
                }
            }
            return response;
        }

    }
}
