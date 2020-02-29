using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using NullableReaders;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.IO;
using System.Data.SqlClient;
using System.Globalization;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class NovedadDocumentacionDAO
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NovedadDocumentacionDAO).Name);  

        public NovedadDocumentacionDAO() { }

        #region Novedades_documentacion_AltaMasiva

        public static List<NovedadDocumentacion> AltaMasiva(List<NovedadDocumentacion> lst, DateTime fRecepcion, 
                                                                     string usuario, string oficina, string ip)
        {
            string sql = "Novedades_documentacion_AltaMasiva";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<NovedadDocumentacion> rta = new List<NovedadDocumentacion>();

            string xml = string.Empty;
            lst.ForEach(delegate(NovedadDocumentacion n)
            {
                xml += "<Documentacion>" +
                       "<IdNovedad>" + n.IdNovedad + "</IdNovedad>" +
                       "<idEstadoDocumentacion>" + n.Estado.IdEstado + "</idEstadoDocumentacion>" +
                       "<nroCaja>" + n.NroCaja + "</nroCaja>" +
                       "</Documentacion>";
            });
            xml = "<NovedadesDocumentacion>" + xml + "</NovedadesDocumentacion>";

            db.AddInParameter(dbCommand, "@xmldatos", DbType.String, xml);
            db.AddInParameter(dbCommand, "@fRecepcion", DbType.DateTime, fRecepcion);
            db.AddInParameter(dbCommand, "@usuario", DbType.String, usuario);
            db.AddInParameter(dbCommand, "@oficina", DbType.String, oficina);
            db.AddInParameter(dbCommand, "@ip", DbType.String, ip);

            try
            {
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        rta.Add(new NovedadDocumentacion(Convert.ToInt64(dr["idNovedad"]),
                                                         new EstadoDocumentacion(Convert.ToInt32(dr["idEstadoDocumentacion"]), 
                                                                                string.Empty, false, false, false, false),
                                                         Convert.ToInt32(dr["nroCaja"]), 
                                                         dr["error"].ToString()));
                    }
                }
                return rta;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(),err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion Novedades_documentacion_AltaMasiva
        
        #region Traer_Novedades_documentacion_estado
        public static List<NovedadDocumentacion> Traer_Documentacion_X_Estado(ConsultaBatch.enum_ConsultaBatch_NombreConsulta nombreConsulta, 
                                                                             long idPrestador, DateTime? Fecha_Recepcion_desde, DateTime? Fecha_Recepcion_hasta, int? idEstado_documentacion, 
                                                                             long? id_Novedad, long? id_Beneficiario, 
                                                                             bool generaArchivo, bool generadoAdmin, out string rutaArchivoSal)
        { 
            string rutaArchivo = string.Empty;
            string nombreArchivo = string.Empty;
            rutaArchivoSal = string.Empty;
            string msgRta = string.Empty;            
            ConsultaBatch consultaBatch = new ConsultaBatch();
            consultaBatch.IDPrestador = idPrestador;
            consultaBatch.NombreConsulta = nombreConsulta;
            consultaBatch.NroBeneficio =  id_Beneficiario.HasValue ? id_Beneficiario.Value : 0;
            consultaBatch.FechaDesde = Fecha_Recepcion_desde;
            consultaBatch.FechaHasta = Fecha_Recepcion_hasta;
            consultaBatch.GeneradoAdmin = generadoAdmin;
            consultaBatch.IdEstado_Documentacion = idEstado_documentacion;
            consultaBatch.Idnovedad = id_Novedad;

            try
            {
                if (generaArchivo == true)
                {
                    /*idPrestador, nombreConsulta.ToString(), 0,
                                                              0, string.Empty, 0,
                                                              0, id_Beneficiario, Fecha_Recepcion_desde, Fecha_Recepcion_hasta, generadoAdmin,
                                                              null, null, null, null, null, idEstado_documentacion, null, null, id_Novedad,
                                                              false, false, null, null, null, null, null*/
                    msgRta = ConsultasBatchDAO.ExisteConsulta(consultaBatch);
                    if (!string.IsNullOrEmpty(msgRta))
                        throw new ApplicationException("MSG_ERROR" + msgRta + "FIN_MSG_ERROR");
                }

                List<NovedadDocumentacion> listNovedades = Traer_Documentacion(idPrestador, Fecha_Recepcion_desde, Fecha_Recepcion_hasta, 
                                                                                idEstado_documentacion, id_Novedad, id_Beneficiario);

                if (listNovedades.Count > 0 && generaArchivo)
                {
                    int maxCantidad = Settings.MaxCantidadRegistros();

                    if (listNovedades.Count >= maxCantidad || generaArchivo)
                    {
                        nombreArchivo = Utilidades.GeneraNombreArchivo(nombreConsulta.ToString(), idPrestador, out rutaArchivo);
                        rutaArchivoSal = Path.Combine(rutaArchivo, nombreArchivo);
                        StreamWriter sw = new StreamWriter(rutaArchivoSal, false, Encoding.UTF8);
                        string separador = Settings.DelimitadorCampo();

                        foreach (NovedadDocumentacion oNovedad in listNovedades)
                        {
                            StringBuilder linea = new StringBuilder();

                            linea.Append(oNovedad.IdNovedad.ToString() + separador);
                            linea.Append(oNovedad.unBeneficiario.IdBeneficiario.ToString() + separador);

                            if (nombreConsulta == ConsultaBatch.enum_ConsultaBatch_NombreConsulta.NOVEDADES_DOCUMENTACION)
                                linea.Append(oNovedad.unBeneficiario.Cuil.ToString() + separador);

                            linea.Append(oNovedad.unBeneficiario.ApellidoNombre.ToString().Trim() + separador);
                            linea.Append(oNovedad.Estado.DescEstado.ToString() + separador);
                            linea.Append((oNovedad.Fecha_Recepcion.HasValue ? oNovedad.Fecha_Recepcion.Value.ToString("dd/MM/yyyy") : string.Empty) + separador);

                            if (nombreConsulta == ConsultaBatch.enum_ConsultaBatch_NombreConsulta.NOVEDADES_DOCUMENTACION)
                            {
                                linea.Append(oNovedad.Cant_Cuotas.ToString() + separador);
                                linea.Append(oNovedad.NroCaja.ToString());
                            }
                            else
                                linea.Append(oNovedad.Cant_Cuotas.ToString());
                            
                            sw.WriteLine(linea.ToString());
                        }
                        sw.Close();

                        Utilidades.ComprimirArchivo(rutaArchivo, nombreArchivo);
                        Utilidades.BorrarArchivo(rutaArchivoSal);
                        nombreArchivo = nombreArchivo + ".zip";
                        consultaBatch.RutaArchGenerado = rutaArchivo;
                        consultaBatch.NomArchGenerado = nombreArchivo;
                        consultaBatch.FechaGenera = DateTime.Now;
                        consultaBatch.Vigente = true;

                        msgRta = ConsultasBatchDAO.AltaNuevaConsulta(consultaBatch);
                        if (!string.IsNullOrEmpty(msgRta))
                        {
                            msgRta = "MSG_ERROR" + msgRta + "FIN_MSG_ERROR";
                            throw new ApplicationException(msgRta);
                        }
                        /* Se instacia el objeto para que no muestre los 
                        * registros y pueda ver solo el archivo generado. */
                        listNovedades = new List<NovedadDocumentacion>(); 
                    }
                }

                return listNovedades;
            }
            catch (SqlException errsql)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), errsql.Source, errsql.Message)); 
                
                if (errsql.Number == -2)
                {                 
                    nombreArchivo = Utilidades.GeneraNombreArchivo(nombreConsulta.ToString(), idPrestador, out rutaArchivo);
                    consultaBatch.RutaArchGenerado = rutaArchivo;
                    consultaBatch.NomArchGenerado = nombreArchivo;
                    consultaBatch.FechaGenera = DateTime.MinValue;
                    consultaBatch.Vigente = false;

                    msgRta = ConsultasBatchDAO.AltaNuevaConsulta(consultaBatch);

                    throw new ApplicationException("MSG_ERROR Generando el archivo. Reingrese a la consulta en unos minutos.FIN_MSG_ERROR");
                }
                else  throw errsql;
            }
            catch (ApplicationException apperr)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), apperr.Source, apperr.Message));    
                throw new ApplicationException(apperr.Message);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }

        public static List<NovedadDocumentacion> Traer_Documentacion(long idPrestado, DateTime? F_Recep_Desde, DateTime? F_Recep_Hasta, int? idEstado_documentacion,
                                                                    long? id_Novedad, long? id_Beneficiario)
        {
            string sql = "NovedadesDocumentacion_TXEstado";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<NovedadDocumentacion> rta = new List<NovedadDocumentacion>();

            db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestado);

            db.AddInParameter(dbCommand, "@fRecepcion_desde", DbType.String, F_Recep_Desde.HasValue ? F_Recep_Desde.Value.ToString("yyyyMMdd") : null);
            db.AddInParameter(dbCommand, "@fRecepcion_Hasta", DbType.String, F_Recep_Hasta.HasValue ? F_Recep_Hasta.Value.ToString("yyyyMMdd") : null);
            db.AddInParameter(dbCommand, "@idEstadoDocumentacion", DbType.Int32, !idEstado_documentacion.HasValue ? null : idEstado_documentacion.Value.ToString());
            db.AddInParameter(dbCommand, "@idNovedad", DbType.Int64, id_Novedad.HasValue ? id_Novedad.Value.ToString(): null);
            db.AddInParameter(dbCommand, "@idBeneficiario", DbType.Int64, id_Beneficiario.HasValue ? id_Beneficiario.Value.ToString(): null);
            
            try
            {
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {


                        rta.Add(new NovedadDocumentacion(Convert.ToInt64(dr["idNovedad"]),
                                                         new EstadoDocumentacion(Convert.ToInt32(dr["idEstadoDocumentacion"]),
                                                                                dr["descripcion"].ToString(), false, false, false, false),
                                                         new Beneficiario(Convert.ToInt64(dr["IdBeneficiario"]), Convert.ToInt64(dr["Cuil"]),
                                                                          dr["ApellidoNombre"].ToString()),
                                                        Convert.ToDouble(dr["montoPrestamo"]), Convert.ToDateTime(dr["fRecepcion"]),
                                                        string.IsNullOrEmpty(dr["nroCaja"].ToString())? new int() : Convert.ToInt32(dr["nroCaja"]),
                                                        string.IsNullOrEmpty(dr["CantCuotas"].ToString()) ? new int() : Convert.ToInt32(dr["CantCuotas"])
                                                        ));


                        
                    }
                }
                return rta;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion Novedades_documentacion_AltaMasiva

        #region Documentacion Scaneada

        public static List<TipoImagen> TipoImagen_Traer()
        {
            string sql = "TipoImagen_Trae";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            List<TipoImagen> rdo = new List<TipoImagen>();

            try
            {

                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        TipoImagen tipoImagen = new TipoImagen(Int16.Parse(dr["IdTipoImagenDW"].ToString()),
                                                               dr["DescripcionAbrev"].ToString(),
                                                               dr["Descripcion"].ToString());
                        rdo.Add(tipoImagen);
                    }
                }

                return rdo;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
                rdo = null;
            }
        }
        
        public static void DocumentacionScaneada_Alta(List<DocumentacionScaneada> lst, string usuario, string ip)
        {
            string sql = "DocumentacionScaneada_Alta";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<NovedadDocumentacion> rta = new List<NovedadDocumentacion>();

            string xml = string.Empty;
            lst.ForEach(delegate(DocumentacionScaneada d)
            {               
                xml += "<Documentacion>" +
                       //"<IdGralImagen>" + idNovedad + "</IdGralImagen>" +
                       "<IdTipoImagen>" + d.TipoImagen.IdTipoImagenDW + "</IdTipoImagen>" +
                       "<IdImagen>" + d.IdImagen + "</IdImagen>" +
                       "<Idnovedad>" + d.Idnovedad + "</Idnovedad>" +
                       "</Documentacion>";
            });
            xml = "<DocumentacionScaneada>" + xml + "</DocumentacionScaneada>";

            db.AddInParameter(dbCommand, "@xmldatos", DbType.String, xml);
            db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, lst.Select(l => l.Idnovedad).First());
            db.AddInParameter(dbCommand, "@cantDocumentos", DbType.Int32, lst.Count);    
            db.AddInParameter(dbCommand, "@usuario", DbType.String, usuario);          
            db.AddInParameter(dbCommand, "@ip", DbType.String, ip);

            try
            {
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(),err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        
        #endregion Documentacion Scaneada
    }
}
