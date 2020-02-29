using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Configuration;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Ar.Gov.Anses.Microinformatica.DAT.DAO;
using System.Data.SqlClient;
using log4net;
using LoggingAnses.Servicio.Entidad;
using LoggingAnses.Servicio;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using System.Linq;

namespace Ar.Gov.Anses.Microinformatica.AuditoriaLog
{
    public static class SeguridadLogDAO
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SeguridadLogDAO).Name);

        #region Log Anterior
        public static string GuardarLog(string database,
                                        string pip_origen,
                                        string pcodigo_sistema,
                                        decimal pcuit_organismo,
                                        decimal pdependencia,
                                        string pautenticador,
                                        string pcodigo_usuario,
                                        string ptabla,
                                        string ptipo_accion,
                                        string pentorno_ejecucion,
                                        string pnombre_servicio,
                                        string pnombre_metodo,
                                        DateTime pfecha_hora,
                                        params object[] datosAguardar)
        {
            string sRta = "";

            Database db = DatabaseFactory.CreateDatabase(database);
            DbCommand dbCommand = db.GetStoredProcCommand("Auditoria_Grabar");

            try
            {
                string strDatosAguardar = string.Empty;
                if (datosAguardar.Length > 0 &&
                    datosAguardar[0].GetType() == typeof(SqlParameterCollection))
                {
                    DbParameterCollection dbParameterCollection = (DbParameterCollection)datosAguardar[0];
                    foreach (DbParameter itemParameter in dbParameterCollection)
                    {
                        string parameterName = itemParameter.ParameterName.Remove(0, 1);
                        strDatosAguardar += string.Concat(parameterName, ":", itemParameter.Value.ToString(), "|").Trim();
                    }
                }
                else
                {
                    foreach (var i in datosAguardar)
                    {
                        strDatosAguardar += string.Concat(strDatosAguardar, string.IsNullOrEmpty(strDatosAguardar) ? "" : "|", i).Trim();
                    }
                }

                db.AddInParameter(dbCommand, "@codigo_sistema", DbType.String, pcodigo_sistema);
                //db.AddInParameter(dbCommand, "@clave_principal", DbType.String, pclave_principal);
                db.AddInParameter(dbCommand, "@ip_origen", DbType.String, pip_origen);
                db.AddInParameter(dbCommand, "@cuit_organismo", DbType.Decimal, pcuit_organismo);
                db.AddInParameter(dbCommand, "@dependencia", DbType.Decimal, pdependencia);
                db.AddInParameter(dbCommand, "@autenticador", DbType.String, pautenticador);
                db.AddInParameter(dbCommand, "@codigo_usuario", DbType.String, pcodigo_usuario);
                db.AddInParameter(dbCommand, "@tabla", DbType.String, ptabla);
                db.AddInParameter(dbCommand, "@tipo_accion", DbType.String, ptipo_accion);
                db.AddInParameter(dbCommand, "@entorno_ejecucion", DbType.String, pentorno_ejecucion);
                db.AddInParameter(dbCommand, "@nombre_servicio", DbType.String, pnombre_servicio);
                db.AddInParameter(dbCommand, "@nombre_metodo", DbType.String, pnombre_metodo);
                db.AddInParameter(dbCommand, "@fecha_hora", DbType.DateTime, pfecha_hora);
                db.AddInParameter(dbCommand, "@datos", DbType.String, strDatosAguardar);

                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));   
                //sRta = ex.Message;
                throw ex;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return sRta;
        }

        public static void GuardarLog(string ptabla, string ptipo_accion,
                                      DbParameterCollection sqlParameterCollection)
        {
            try
            {
                IUsuarioToken oUsuarioEnDirector = new UsuarioToken();
                oUsuarioEnDirector.ObtenerUsuarioEnWs();

                Auditoria unaAuditoria = InstanciarAuditoria(oUsuarioEnDirector);

                if (oUsuarioEnDirector.VerificarTokenWs)
                {
                    //string pcodigo_sistema = Settings.CodigoSistema();
                    string database = "DAT_V01";
                    object[] pObject = { sqlParameterCollection };
                    unaAuditoria.EntornoEjecucion = "0";
                    unaAuditoria.FechaHora = DateTime.Now;

                    GuardarLog(database,
                               unaAuditoria.IP,
                               unaAuditoria.CodigoSistema,
                               unaAuditoria.CuitOrganismo,
                               unaAuditoria.DependenciaOficina,
                               unaAuditoria.Autenticacion,
                               unaAuditoria.CodigoUsuario,
                               ptabla,
                               ptipo_accion,
                               unaAuditoria.EntornoEjecucion,
                               unaAuditoria.NombreServicio,
                               unaAuditoria.NombreMetodo,
                               unaAuditoria.FechaHora,
                               pObject);
                }
                else
                {
                    throw new UsuarioTokenException("No se pudo obtener el UsuarioToken del ws " + unaAuditoria.NombreServicio);
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));   
                throw ex;
            }
        }

        static public Auditoria InstanciarAuditoria(IUsuarioToken UT)
        {
            try
            {
                string nombreServicio = HttpContext.Current.Request.Params["PATH_INFO"];
                string nombreMetodo = HttpContext.Current.Request.Params["HTTP_SOAPACTION"];

                // saco el formato
                if (nombreServicio.LastIndexOf(".") != -1 && nombreServicio.LastIndexOf("/") != -1)
                    nombreServicio = nombreServicio.Substring(0, nombreServicio.LastIndexOf(".")).Substring(nombreServicio.LastIndexOf("/") + 1);
                if (nombreMetodo.LastIndexOf("\"") != -1 && nombreMetodo.LastIndexOf("/") != -1)
                    nombreMetodo = nombreMetodo.Substring(0, nombreMetodo.LastIndexOf("\"")).Substring(nombreMetodo.LastIndexOf("/") + 1);
                decimal cuitANSES = decimal.Parse(string.IsNullOrEmpty(UT.EntidadesAsociadas[0]) ? "0" : UT.EntidadesAsociadas[0]); //decimal.Parse(ConfigurationManager.AppSettings["cuitANSES"].ToString());  

                string appNombreSistema = string.Empty;
                if (!string.IsNullOrEmpty(Settings.CodigoSistema()))
                    appNombreSistema = Settings.CodigoSistema();
                else
                    appNombreSistema = UT.Sistema;

                Auditoria unaAuditoria = new Auditoria(appNombreSistema,
                                                       cuitANSES,
                                                       UT.MetodoAuth,
                                                       UT.MetodoAuth,
                                                       nombreServicio,
                                                       nombreMetodo,
                                                       UT.IdUsuario,
                                                       UT.Nombre,
                                                       UT.DirIP,
                                                       string.IsNullOrEmpty(UT.Oficina) ? 0 : int.Parse(UT.Oficina));

                return unaAuditoria;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));   
                throw ex;
            }
        }
        #endregion
                
        public static string Serialize(object obj)
        {
          try
            {
                String stringXML = null;
                MemoryStream oMemoryStream = new MemoryStream();

                XmlSerializer oXmlSerializer = new XmlSerializer(obj.GetType());
                XmlTextWriter oXmlTextWriter = new XmlTextWriter(oMemoryStream, Encoding.UTF8);

                XmlSerializerNamespaces oXmlSerializerNamespaces = new XmlSerializerNamespaces();
                oXmlSerializerNamespaces.Add(string.Empty, string.Empty);

                oXmlSerializer.Serialize(oXmlTextWriter, obj, oXmlSerializerNamespaces);

                oMemoryStream = (MemoryStream)oXmlTextWriter.BaseStream;
                UTF8Encoding oUTF8Encoding = new UTF8Encoding();

                stringXML = oUTF8Encoding.GetString(oMemoryStream.ToArray());

                return stringXML;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Parametros(params object[] datosAguardar)
        {            
           string strDatosAguardar = string.Empty;
           try
            {               
                if (datosAguardar.Length > 0 &&
                    datosAguardar[0].GetType() == typeof(SqlParameterCollection))
                {
                    DbParameterCollection dbParameterCollection = (DbParameterCollection)datosAguardar[0];
                    foreach (DbParameter itemParameter in dbParameterCollection)
                    {
                        string parameterName = itemParameter.ParameterName.Remove(0, 1);
                        strDatosAguardar += string.Concat(parameterName, ":", itemParameter.Value.ToString(), "|").Trim();
                    }
                }
                else
                {
                    foreach (var i in datosAguardar)
                    {
                        strDatosAguardar += string.Concat(strDatosAguardar, string.IsNullOrEmpty(strDatosAguardar) ? "" : "|", i).Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
                  
            }

           string datosXML = Serialize(strDatosAguardar);
           return datosXML;
        }            

        
        public static void AuditarOnlineLog(string clavePrincipal,
                                            Object datos,
                                            string tabla,
                                            TipoAction tipoAction)
         {
            try
            {
                LogAplicaciones oLogging = new LoggingAnses.Servicio.LogAplicaciones();
                            
                if (datos.GetType().Namespace == "System.Collections.Generic")
                {
                    string[] arrayDummy = new string[1];
                    Type objType = datos.GetType();

                    if (objType.IsGenericType && objType.GetGenericArguments().Count() > 0)
                        objType = objType.GetGenericArguments().First<Type>();

                    string datosXML = Serialize(datos);
                    string xmlDescType = string.Format("Lista de Tipo {0} Serilizada en XML", objType.Name);
                    var newDatos = (from o in arrayDummy
                                    select new { XMLDescType = xmlDescType, DatosXML = datosXML }).ToList();

                    if (newDatos.Count > 0)
                        datos = newDatos[0];
                }                      
                
                oLogging.Log(new OnlineLog
                {
                    ClavePrincipal = clavePrincipal,
                    Datos = datos,
                    Tabla = tabla,
                    TipoAccion = tipoAction
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}