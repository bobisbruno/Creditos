using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using NullableReaders;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Transactions;
using System.Data.SqlClient;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using log4net;


namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class CaratulacionDAO
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CaratulacionDAO).Name);
        
        #region Traer Novedades por idNovedad

        public static List<NovedadCaratulada> Traer_xIdNovedad(long idNovedad)
        {
            string sql = "Novedades_TXIdNovedad_Xa_Caratular";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            List<NovedadCaratulada> lstCaratulacion = new List<NovedadCaratulada>();

            try
            {
                db.AddInParameter(dbCommand, "@idnovedad", DbType.Int64, idNovedad);
                dbParametros = dbCommand.Parameters;

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {

                        NovedadCaratulada nc = new NovedadCaratulada();
                        Novedad n = null;
                        if (!dr["FecNov"].Equals(DBNull.Value))
                        {
                            n = new Novedad();
                            n.IdNovedad = Convert.ToInt64(dr["IdNovedad"]);
                            n.FechaNovedad = DateTime.Parse(dr["FecNov"].ToString());
                            n.ImporteTotal = Convert.ToDouble(dr["ImporteTotal"]);
                            n.CantidadCuotas = Convert.ToByte(dr["CantCuotas"]);
                            n.Comprobante = dr["NroComprobante"].ToString();
                            n.IdEstadoReg = Convert.ToByte(dr["IdEstadoReg"]);
                            n.UnPrestador = new Prestador();
                            n.UnPrestador.RazonSocial = dr["RazonSocial"].ToString();
                            n.UnPrestador.ID = long.Parse(dr["IdPrestador"].ToString());
                            n.UnBeneficiario = new Beneficiario();
                            n.UnBeneficiario.IdBeneficiario = !string.IsNullOrEmpty(dr["IdBeneficiario"].ToString()) ? Convert.ToInt64(dr["IdBeneficiario"]) : 0;
                            n.UnBeneficiario.Cuil = !string.IsNullOrEmpty(dr["Cuil"].ToString()) ? Convert.ToInt64(dr["Cuil"]) : 0;
                            n.UnBeneficiario.ApellidoNombre = dr["ApellidoNombre"].ToString();
                            n.UnBeneficiario.TipoDoc = Convert.ToInt32(dr["TipoDoc"]);
                            n.UnBeneficiario.NroDoc = dr["NroDoc"].ToString();
                            n.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()), dr.GetString("DescConceptoLiq"));
                            n.UnTipoConcepto = new TipoConcepto(Byte.Parse(dr["TipoConcepto"].ToString()), dr.GetString("DescTipoConcepto"));
                            n.CBU = dr["cbu"].ToString();
                        }
                        if(dr["nroExpediente"].Equals(DBNull.Value))
                        {
                            nc = new NovedadCaratulada(null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        n,
                                                        null,
                                                        null,
                                                        null,
                                                        null,
                                                        null);
                        }
                        else
                        {
                            nc = new NovedadCaratulada(dr["nroExpediente"].ToString(),
                                                                    Convert.ToDateTime(dr["fAlta"]),
                                                                    Convert.ToDateTime(dr["frecepcion"]),
                                                                    (enum_EstadoCaratulacion)Enum.Parse(typeof(enum_EstadoCaratulacion), dr["IdEstado"].ToString()),
                                                                    dr["DescEstadoNov"].ToString(),
                                                                    Convert.ToInt32(dr["idEstadoExpediente"]),
                                                                    string.Empty,
                                                                    n,
                                                                    dr["usuarioAlta"].ToString(),
                                                                    dr["oficinaAlta"].ToString(),
                                                                    dr["observaciones"].ToString(),
                                                                    dr["nroResolucion"] == DBNull.Value ? null : dr["nroResolucion"].ToString(),
                                                                    dr["idTipoRechazo"] == DBNull.Value ? null : new TipoRechazoExpediente(
                                                                                                                        new Tipo(Convert.ToInt32(dr["idTipoRechazo"]),
                                                                                                                                 dr["descTipoRechazo"].ToString()), 
                                                                                                                                 false)
                                                                    );
                        }
                        
                        lstCaratulacion.Add(nc);

                        #region Codigo COMENTADO - verificar y borrar
                        /*
                        unaCaratulacion = new Caratulacion();

                        unaCaratulacion.IdNovedad=long.Parse(dr["IdNovedad"].ToString());
                        unaCaratulacion.Beneficio =dr["IdBeneficiario"].ToString();
                        unaCaratulacion.Cuil = dr["Cuil"].ToString();
                        unaCaratulacion.Nombre = dr["ApellidoNombre"].ToString();
                        unaCaratulacion.ImporteTotal = double.Parse( dr["ImporteTotal"].ToString());
                        unaCaratulacion.CantidadCuotas = Byte.Parse(dr["CantCuotas"].ToString());
                        unaCaratulacion.FechaPresentacion = DateTime.Now;
                        unaCaratulacion.FechaNovedad = DateTime.Parse(dr["FecNov"].ToString());
                        unaCaratulacion.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()), dr.GetString("DescConceptoLiq"));
                        unaCaratulacion.UnTipoConcepto = new TipoConcepto(Byte.Parse(dr["TipoConcepto"].ToString()), dr.GetString("DescTipoConcepto"));
                        unaCaratulacion.Entidad = dr["RazonSocial"].ToString();
                        unaCaratulacion.CBU = dr["cbu"].ToString();
                        unaCaratulacion.Documento = dr["NroDoc"].ToString();
                        unaCaratulacion.TipoDocumento = dr["TipoDoc"].ToString();
                        unaCaratulacion.NroComprobante = dr["nrocomprobante"].ToString();
                        unaCaratulacion.IdEstadoReg = dr["IdEstadoReg"].ToString();
                        unaCaratulacion.IdItem = dr["idItem"].ToString();
                        unaCaratulacion.DescripcionItem = dr["descripcionItem"].ToString();
                        unaCaratulacion.ModoPago = dr["ModoPago"].ToString();
                        unaCaratulacion.EstaCartulado = dr["EstaCaratulado"].ToString();
                        unaCaratulacion.NroExpediente = dr["nroExpediente"].ToString();
                        unaCaratulacion.FechaRecepcion = dr["frecepcion"].Equals(DBNull.Value) ? new DateTime?() : DateTime.Parse(dr["frecepcion"].ToString());
                        unaCaratulacion.FechaAlta = dr["fAlta"].Equals(DBNull.Value) ? new DateTime?() : DateTime.Parse(dr["fAlta"].ToString());
                        unaCaratulacion.IdEstadoExpediente = dr["idEstadoExpediente"].ToString();
		                unaCaratulacion.IdEstado = dr["idEstado"].ToString();
                        unaCaratulacion.DescEstadoNov = dr["DescEstadoNov"].ToString();
		                unaCaratulacion.IngresoDocumentacion = dr["faltaIngresarDocumentacion"].ToString();
		                unaCaratulacion.Observacion = dr["observaciones"].ToString();
		                unaCaratulacion.UsuarioAlta =  dr["usuarioAlta"].ToString();
		                unaCaratulacion.OficinaAlta =  dr["oficinaAlta"].ToString();
                        unaCaratulacion.IpAlta =  dr["ipAlta"].ToString();
                        unaCaratulacion.FechaProceso = dr["fproceso"].Equals(DBNull.Value) ? new DateTime?() : DateTime.Parse(dr["fproceso"].ToString());
                        unaCaratulacion.Usuario =  dr["usuario"].ToString();
                        unaCaratulacion.Oficina =  dr["oficina"].ToString();
                        unaCaratulacion.Ip =  dr["ip"].ToString();
                        unaCaratulacion.FechaUltActualizacion = dr["fultActualizacion"].Equals(DBNull.Value) ? new DateTime?() : DateTime.Parse(dr["fultActualizacion"].ToString());
                         
                        lstCaratulacion.Add(unaCaratulacion);
                        */

                        #endregion
                    }
                }
                return lstCaratulacion;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en Novedades_TXIdNovedad_Xa_Caratular", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region Alta de las Novedades Caratuladas

          public static void NovedadesCaratuladas_Alta(string Expediente, long IdNovedad, DateTime FecRecepcion, DateTime FecAlta, 
          long cuil, long IdBeneficiario, int EstadoExpediente, byte faltaIngresarDocumentacion, string observaciones,
          string usuarios, string oficina, string ip, long IdPrestador)
          {
            //String retorno = "";

            string sql = "NovedadesCaratuladas_Alta";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@nroExpediente", DbType.String, Expediente);
                db.AddInParameter(dbCommand, "@idNovedad", DbType.Int64, IdNovedad);
                db.AddInParameter(dbCommand, "@frecepcion", DbType.DateTime, FecRecepcion);
                db.AddInParameter(dbCommand, "@fAlta", DbType.DateTime, FecAlta );
                db.AddInParameter(dbCommand, "@cuilBeneficiario", DbType.Int64, cuil );
                db.AddInParameter(dbCommand, "@idBeneficiario", DbType.Int64, IdBeneficiario );
                db.AddInParameter(dbCommand, "@idEstadoExpediente", DbType.Byte, EstadoExpediente );
                db.AddInParameter(dbCommand, "@idEstado", DbType.Byte, faltaIngresarDocumentacion );
                db.AddInParameter(dbCommand, "@faltaIngresarDocumentacion", DbType.Byte, faltaIngresarDocumentacion);
                db.AddInParameter(dbCommand, "@observaciones", DbType.String, observaciones);
                db.AddInParameter(dbCommand, "@usuario", DbType.String, usuarios);
                db.AddInParameter(dbCommand, "@oficina", DbType.String, oficina );
                db.AddInParameter(dbCommand, "@ip", DbType.String, ip);
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, IdPrestador);                
                
                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }

            
        }

        #endregion

        #region  Novedades Caratuladas Modifica estado

        public static void NovedadesCaratuladas_ModificarEstado(long IdNovedad, int EstadoExpediente, 
                                                                string usuarios, string oficina, string ip, 
                                                                string observaciones, string nroResolucion, int? idTipoRechazo)
        {

            string sql = "NovedadesCaratuladas_ModificarEstado";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@idNovedad", DbType.Int64, IdNovedad);
                db.AddInParameter(dbCommand, "@idEstadoExpediente", DbType.Byte, EstadoExpediente);
                db.AddInParameter(dbCommand, "@observaciones", DbType.String,observaciones);
	            db.AddInParameter(dbCommand, "@nroResolucion", DbType.String,nroResolucion);
                db.AddInParameter(dbCommand, "@idTipoRechazo", DbType.Int32, idTipoRechazo);
                db.AddInParameter(dbCommand, "@usuario", DbType.String, usuarios);
                db.AddInParameter(dbCommand, "@oficina", DbType.String, oficina);
                db.AddInParameter(dbCommand, "@ip", DbType.String, ip);

                db.ExecuteNonQuery(dbCommand);

            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }


        }

        #endregion

        #region Novedades_Caratuladas_Traer

        public static List<NovedadCaratulada> Novedades_Caratuladas_Traer(ConsultaBatch.enum_ConsultaBatch_NombreConsulta nombreConsulta,
                                                                         long idPrestador, DateTime? Fecha_Recepcion_desde, DateTime? Fecha_Recepcion_hasta, enum_EstadoCaratulacion? idEstado,
                                                                         int conErrores, long? id_Beneficiario,
                                                                         bool generaArchivo, bool generadoAdmin, out string rutaArchivoSal)
        {
            string rutaArchivo = string.Empty;
            string nombreArchivo = string.Empty;
            rutaArchivoSal = string.Empty;
            string msgRta = string.Empty;           
            ConsultaBatch consultaBatch = new ConsultaBatch();
            consultaBatch.IDPrestador = idPrestador;
            consultaBatch.NombreConsulta = nombreConsulta;
            consultaBatch.NroBeneficio = id_Beneficiario.HasValue ? id_Beneficiario.Value : 0;
            consultaBatch.FechaDesde = Fecha_Recepcion_desde;
            consultaBatch.FechaHasta = Fecha_Recepcion_hasta;
            consultaBatch.GeneradoAdmin = generadoAdmin;
            consultaBatch.IdEstado_Documentacion = (int?)idEstado;

            try
            {               
                if (generaArchivo == true)
                {
                    msgRta = ConsultasBatchDAO.ExisteConsulta(consultaBatch);
                    if (!string.IsNullOrEmpty(msgRta))
                        throw new ApplicationException("MSG_ERROR" + msgRta + "FIN_MSG_ERROR");
                }
               
                List<NovedadCaratulada> listNovedades = Traer_Caratulacion(idPrestador, Fecha_Recepcion_desde, Fecha_Recepcion_hasta, conErrores, id_Beneficiario, (int?)idEstado);

                if (listNovedades.Count > 0 && generaArchivo)
                {
                    int maxCantidad = Settings.MaxCantidadRegistros();

                    if (listNovedades.Count >= maxCantidad || generaArchivo)
                    {
                        nombreArchivo = Utilidades.GeneraNombreArchivo(nombreConsulta.ToString(), idPrestador, out rutaArchivo);
                        rutaArchivoSal = Path.Combine(rutaArchivo, nombreArchivo);
                        StreamWriter sw = new StreamWriter(rutaArchivoSal, false, Encoding.UTF8);
                        string separador = Settings.DelimitadorCampo();

                        foreach (NovedadCaratulada oNovedad in listNovedades)
                        {
                            StringBuilder linea = new StringBuilder();

                            linea.Append(oNovedad.NroExpediente.ToString() + separador);
                            linea.Append(oNovedad.novedad.IdNovedad.ToString() + separador);
                            linea.Append(oNovedad.FInicioAfjp.ToString() + separador);
                            linea.Append(oNovedad.novedad.UnConceptoLiquidacion.CodConceptoLiq.ToString() + separador);
                            linea.Append(oNovedad.novedad.UnBeneficiario.IdBeneficiario.ToString() + separador);
                            linea.Append(oNovedad.novedad.UnBeneficiario.ApellidoNombre.ToString() + separador);
                            linea.Append(oNovedad.novedad.FechaNovedad.ToString() + separador);
                            linea.Append(oNovedad.Error.ToString() + separador);
                            linea.Append(oNovedad.idEstadoCaratulacion.ToString());

                            sw.WriteLine(linea.ToString());
                        }
                        sw.Close();

                        Utilidades.ComprimirArchivo(rutaArchivo, nombreArchivo);
                        Utilidades.BorrarArchivo(rutaArchivoSal);
                        nombreArchivo = nombreArchivo + ".zip";

                        consultaBatch.OpcionBusqueda = byte.Parse(conErrores.ToString());
                        consultaBatch.RutaArchGenerado = rutaArchivo;
                        consultaBatch.NomArchGenerado = nombreArchivo;
                        consultaBatch.FechaGenera = DateTime.Now;
                        consultaBatch.Vigente = true;

                        //  conErrores.HasValue? conErrores.Value ? 1 : 0 : 2 --> el valor=2 es para cuando seleccione todos en el combo 'Con errores'
                        msgRta = ConsultasBatchDAO.AltaNuevaConsulta(consultaBatch);
                        if (!string.IsNullOrEmpty(msgRta))
                        {
                            msgRta = "MSG_ERROR" + msgRta + "FIN_MSG_ERROR";
                            throw new ApplicationException(msgRta);
                        }
                        /* Se instacia el objeto para que no muestre los 
                        * registros y pueda ver solo el archivo generado. */
                        listNovedades = new List<NovedadCaratulada>();
                    }
                }

                return listNovedades;
            }
            catch (SqlException errsql)
            {
                if (errsql.Number == -2)
                {
                    // timeout
                    log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), errsql.Source, errsql.Message));
                    nombreArchivo = Utilidades.GeneraNombreArchivo(nombreConsulta.ToString(), idPrestador, out rutaArchivo);
                    consultaBatch.OpcionBusqueda = byte.Parse(conErrores.ToString());
                    consultaBatch.NomArchGenerado = nombreArchivo;
                    consultaBatch.RutaArchGenerado = rutaArchivo;
                    consultaBatch.FechaGenera = DateTime.MinValue;
                    consultaBatch.Vigente = false;

                    msgRta = ConsultasBatchDAO.AltaNuevaConsulta(consultaBatch);

                    throw new ApplicationException("MSG_ERROR Generando el archivo. Reingrese a la consulta en unos minutos.FIN_MSG_ERROR");
                }
                else throw errsql;
            }
            catch (ApplicationException apperr)
            {
                throw new ApplicationException(apperr.Message);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
        }

        public static List<NovedadCaratuladaTotales> Novedades_Caratuladas_Traer_Por_Estado(long? idPrestador, DateTime? fDesde, DateTime? fHasta)
        {
            string sql = "NovedadesCaratuladas_TotXEstado";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<NovedadCaratuladaTotales> rta = new List<NovedadCaratuladaTotales>();
           
            try
            {            

                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@fDesde", DbType.String, fDesde.HasValue ? fDesde.Value.ToString("yyyyMMdd") : null);
                db.AddInParameter(dbCommand, "@fHasta", DbType.String, fHasta.HasValue ? fHasta.Value.ToString("yyyyMMdd") : null);
           
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {

                        rta.Add(new NovedadCaratuladaTotales(dr["estado"].ToString(),
                                                             int.Parse(dr["Cantidad de novedades Sin duplicados por idnovedad"].ToString()),
                                                             int.Parse(dr["Cantidad de novedades"].ToString())));


                    }
                }

                return rta;             
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
            }
        }

        public static List<NovedadCaratuladaTotales> Novedades_Caratuladas_Traer_Difiere_Estado()
        {
            string sql = "NovedadesCaratuladas_DifiereEstado";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<NovedadCaratuladaTotales> rta = new List<NovedadCaratuladaTotales>();

            try
            {
                 using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {

                        rta.Add(new NovedadCaratuladaTotales(string.IsNullOrEmpty(dr["idestadoexpediente"].ToString())? (int?)null : int.Parse(dr["idestadoexpediente"].ToString()),
                                                             dr["EstadoExpedienteErroneo"].ToString(),
                                                             (enum_EstadoCaratulacion)Enum.Parse(typeof(enum_EstadoCaratulacion), dr["idEstado"].ToString()),
                                                             dr["EstadoDAT"].ToString(),                                                             
                                                             int.Parse(dr["cant"].ToString())));
                    }
                }

                return rta;
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
            }
        }

        public static List<NovedadCaratulada> Traer_Caratulacion(long idprestador, DateTime? fDesde, DateTime? fHasta, int conErrores,
                                                                    long? idBeneficiario, int? idEstadoNovCaratulacion)
        {
           
            string sql = "Novedades_Caratuladas_Traer";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<NovedadCaratulada> rta = new List<NovedadCaratulada>();

            db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idprestador);
            db.AddInParameter(dbCommand, "@fDesde", DbType.String, fDesde.HasValue ? fDesde.Value.ToString("yyyyMMdd") : null);
            db.AddInParameter(dbCommand, "@fHasta", DbType.String, fHasta.HasValue ? fHasta.Value.ToString("yyyyMMdd") : null);
            db.AddInParameter(dbCommand, "@conErrores", DbType.Int16, conErrores);
            db.AddInParameter(dbCommand, "@idBeneficiario", DbType.Int64, idBeneficiario.HasValue ? idBeneficiario.Value.ToString() : null);
            db.AddInParameter(dbCommand, "@idEstadoNovCaratulacion", DbType.Int16, idEstadoNovCaratulacion.HasValue ? idEstadoNovCaratulacion.Value.ToString() : null);

            try
            {
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {

                        string fecnov = string.Empty;

                        if (!string.IsNullOrEmpty(dr["FecNov"].ToString()) && !dr["FecNov"].ToString().Contains("/"))
                        {
                            fecnov = dr["FecNov"].ToString().Substring(0, 4) + "/" + dr["FecNov"].ToString().Substring(4, 2) + "/" + dr["FecNov"].ToString().Substring(6);
                        }

                        Novedad n = new Novedad(Convert.ToInt64(dr["idnovedad"]),
                                                                  Convert.ToDateTime(fecnov),
                                                                  Convert.ToDouble(dr["ImporteTotal"]),
                                                                  Convert.ToByte(dr["CantCuotas"]),
                                                                  (float)Convert.ToDouble(dr["Porcentaje"]),
                                                                  dr["NroComprobante"].ToString(),
                                                                  dr["MAC"].ToString());

                        n.UnConceptoLiquidacion.CodConceptoLiq = Convert.ToInt32(dr["CodConceptoLiq"]);
                        n.UnConceptoLiquidacion.DescConceptoLiq = dr["DescConceptoLiq"].ToString();
                        n.IdEstadoReg = Convert.ToByte(dr["IdEstadoReg"]);

                        n.MontoPrestamo = Convert.ToDouble(dr["montoPrestamo"]);
                        n.TNA = Convert.ToDouble(dr["TNA"]);
                        n.Gasto_Otorgamiento = Convert.ToDouble(dr["gastoOtorgamiento"]);
                        n.Cuota_Social = Convert.ToDouble(dr["cuotaSocial"]);
                        n.Gasto_Adm_Mensual = Convert.ToDouble(dr["gastoAdmMensual"]);
                        n.CFTEA = Convert.ToDouble(dr["CFTEA"]);

                        n.UnPrestador = new Prestador(idprestador,
                                                      dr["RazonSocial"].ToString(),
                                                      Convert.ToInt64(dr["CUIT"].ToString()));

                        Beneficiario UnBeneficiario = new Beneficiario();
                        
                        UnBeneficiario.IdBeneficiario = !string.IsNullOrEmpty(dr["IdBeneficiario"].ToString() ) ?  Convert.ToInt64(dr["IdBeneficiario"]) : 0;
                        UnBeneficiario.Cuil = !string.IsNullOrEmpty(dr["Cuil"].ToString() ) ?  Convert.ToInt64(dr["Cuil"]) : 0; 
                        UnBeneficiario.ApellidoNombre =  dr["ApellidoNombre"].ToString();

                        n.UnBeneficiario = UnBeneficiario;

                        rta.Add(new NovedadCaratulada(dr["nroExpediente"].ToString(),
                                                      Convert.ToDateTime(dr["fAltaExpediente"]),
                                                      Convert.ToDateTime(dr["finicioAfjp"]),
                                                      (enum_EstadoCaratulacion)Enum.Parse(typeof(enum_EstadoCaratulacion), dr["IdEstado"].ToString()),
                                                      string.Empty,
                                                      null,
                                                      dr["error"].ToString(),
                                                      n,
                                                      string.Empty,
                                                      string.Empty,
                                                      string.Empty,
                                                      string.Empty,
                                                      dr["idTipoRechazo"].Equals(DBNull.Value) ? null : 
                                                      new TipoRechazoExpediente(new Tipo(Convert.ToInt64(dr["idTipoRechazo"]),
                                                                                          dr["descTipoRechazo"].ToString()))));


                    }
                }
                return rta;
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
            }
        }
        #endregion Novedades_documentacion_AltaMasiva

        #region TipoRechazoExpediente_Traer

        public static List<TipoRechazoExpediente> TipoRechazoExpediente_Traer()
        {

            string sql = "TipoRechazoExpediente_Traer";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<TipoRechazoExpediente> rta = new List<TipoRechazoExpediente>();

            try
            {
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        rta.Add(new TipoRechazoExpediente(new Tipo(Convert.ToInt64(dr["idTipoRechazo"]),
                                                                   dr["descTipoRechazo"].ToString()),
                                                          Convert.ToBoolean(dr["pideNroResolucion"])));
                    }
                }
                return rta;
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
            }
        }
        #endregion

        #region Novedades_Caratuladas_OficinasSinVencimiento

        public static List<String> NovedadesCaratuladas_OficinasSinVencimiento_Traer()
        {
            string sql = "NovedadeCaratuladas_OficinasSinVencimiento_TT";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<String> rta = new List<String>();
            
            try
            {
               using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        rta.Add(dr["oficina"].ToString());
                    }
                }
                return rta;
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
            }
        }

        #endregion
    }
}
