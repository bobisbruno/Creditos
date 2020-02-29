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
using System.Globalization;
using log4net;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades.EstadosNovedad;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades.NovedadesHistoricas;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class NovedadDAO
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NovedadDAO).Name);

        #region Trae ID de Novedad por Beneficio - Fecha Novedad - concepto

        public static List<Novedad> TraerIDNovedadesPorBenef(string beneficiario, string fechaInicio, int codConcLiq, bool soloArgenta, bool soloEntidades)
        {
            string sql = "Admin_IDNovedades_Trae";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = null;
            List<Novedad> lstNovedades = new List<Novedad>();
            Novedad unaNovedad;

            try
            {
                sql = "Admin_IDNovedades_Trae";
                dbCommand = db.GetStoredProcCommand(sql);
                db.AddInParameter(dbCommand, "@Beneficiario", DbType.Int64, beneficiario);
                db.AddInParameter(dbCommand, "@CodConcLiq", DbType.Int32, codConcLiq);
                db.AddInParameter(dbCommand, "@FechaInicio", DbType.DateTime, fechaInicio);
                db.AddInParameter(dbCommand, "@SoloArgenta", DbType.Boolean, soloArgenta);
                db.AddInParameter(dbCommand, "@SoloEntidades", DbType.Boolean, soloEntidades);


                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        unaNovedad = new Novedad(long.Parse(dr["IdNovedad"].ToString()),
                                                 DateTime.Parse(dr["FecNov"].ToString()),
                                                 double.Parse(dr["ImporteTotal"].ToString()),
                                                 byte.Parse(dr["CantCuotas"].ToString()),
                                                 float.Parse(dr["Porcentaje"].ToString()),
                                                 string.Empty,
                                                 string.Empty,
                                                 null,
                                                 string.Empty,
                                                 false, null);

                        unaNovedad.MontoPrestamo = string.IsNullOrEmpty(dr["montoPrestamo"].ToString()) ? 0 : double.Parse(dr["montoPrestamo"].ToString());
                        unaNovedad.UnPrestador = new Prestador(dr.GetInt64("IdPrestador"), dr.GetString("RazonSocial"), 0);
                        unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(dr.GetInt32("CodConceptoLiq"),
                                                                                   dr.GetString("DescConceptoLiq"));

                        unaNovedad.UnTipoConcepto = new TipoConcepto(short.Parse(dr["TipoConcepto"].ToString()),
                                                                                   dr.GetString("DescTipoConcepto"));

                        unaNovedad.UnEstadoNovedad = new Estado(int.Parse(dr["IdEstadoReg"].ToString()), dr["DescripcionEstadoReg"].ToString());
                        lstNovedades.Add(unaNovedad);
                    }
                }
                return lstNovedades;
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
        #endregion

        #region Traer Novedades por idBeneficiario

        public static List<Novedad> Traer_Novedades_xIdBeneficiario(long? idBeneficiario, string cuil)
        {
            string sql = "Admin_Novedades_TT_X_Beneficiario";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            List<Novedad> lstNovedades = new List<Novedad>();
            Novedad unaNovedad;

            try
            {
                db.AddInParameter(dbCommand, "@Beneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@Cuil", DbType.String, string.IsNullOrEmpty(cuil) ? null : cuil);

                dbParametros = dbCommand.Parameters;

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {

                        unaNovedad = new Novedad(long.Parse(dr["IdNovedad"].ToString()),
                                                 DateTime.Parse(dr["FecNov"].ToString()),
                                                 double.Parse(dr.GetValue("ImporteTotal").ToString()),
                                                 byte.Parse(dr.GetValue("CantCuotas").ToString()),
                                                 Single.Parse(dr["Porcentaje"].ToString()),
                                                 dr["NroComprobante"].ToString(),
                                                 dr["MAC"].ToString(),
                                                 dr["FecImportacion"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecImportacion"].ToString()),
                                                 dr["PrimerMensual"].ToString(),
                                                 false, null,
                                                 dr["usuarioAlta"].ToString(),
                                                 dr["nroSucursal"].ToString() + " - " + dr["OficinaAlta"].ToString(),
                                                 dr["usuarioSupervision"].ToString(),
                                                 dr["fSupervision"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["fSupervision"].ToString()),
                                                 dr["NombreArchivo"].ToString());

                        unaNovedad.SaldoCredito = dr["Saldocredito"].Equals(DBNull.Value) ? 0 : double.Parse(dr.GetValue("Saldocredito").ToString());
                        unaNovedad.MontoPrestamo = dr["montoPrestamo"].Equals(DBNull.Value) ? 0 : double.Parse(dr.GetValue("montoPrestamo").ToString());
                        unaNovedad.TotalAmortizado = dr["TotalAmortizado"].Equals(DBNull.Value) ? 0 : double.Parse(dr.GetValue("TotalAmortizado").ToString());

                        unaNovedad.CantidadCuotasRestantes = dr.GetNullableInt32("CantidadCuotasRestantes");
                        unaNovedad.CuotasLiquidadas = dr.GetNullableInt32("CuotasLiquidadas");
                        unaNovedad.UnTipoConcepto = new TipoConcepto(Byte.Parse(dr["TipoConcepto"].ToString()),
                                                                     dr.GetString("DescTipoConcepto"));
                        unaNovedad.UnPrestador = new Prestador(long.Parse(dr["IdPrestador"].ToString()),
                                                               dr.GetString("RazonSocial"),
                                                               long.Parse(dr["CUIT_Prestador"].ToString()),
                                                               dr["entregaDocumentacionEnFGS"].Equals(DBNull.Value) ? false : bool.Parse(dr["entregaDocumentacionEnFGS"].ToString()));
                        unaNovedad.UnBeneficiario = new Beneficiario(long.Parse(dr["IdBeneficiario"].ToString()), 0,
                                                                     dr.GetString("ApellidoNombre"));
                        unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                                   dr.GetString("DescConceptoLiq"), bool.Parse(dr["EsAfiliacion"].ToString()));

                        unaNovedad.MensualCarga = dr["MensualCarga"].ToString();
                        unaNovedad.UltimaMensualCuota = dr["UltimaMensualCuota"].ToString();
                        unaNovedad.UnEstadoNovedad = new Estado(int.Parse(dr["idestado"].ToString()), dr["descripcionEstado"].ToString());
                        unaNovedad.ImporteCuota = dr["importeCuota"].Equals(DBNull.Value) ? 0 : double.Parse(dr.GetValue("importeCuota").ToString());
                        lstNovedades.Add(unaNovedad);
                    }
                }
                return lstNovedades;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en NovedadDAO.Traer_Novedades_xIdBeneficiario", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region
        public static List<Novedad> Traer_Novedades_HistoricoArgenta(long? idBeneficiario, string cuil)
        {
            string sql = "Admin_Novedades_TT_X_Beneficiario"; // cambiar por SP Novedades_TT_X_Beneficiario_Argenta  20180425 
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            List<Novedad> lstNovedades = new List<Novedad>();
            Novedad unaNovedad;

            try
            {

                db.AddInParameter(dbCommand, "@Beneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@Cuil", DbType.String, string.IsNullOrEmpty(cuil) ? null : cuil);
                dbParametros = dbCommand.Parameters;

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {

                        unaNovedad = new Novedad(long.Parse(dr["IdNovedad"].ToString()),
                                                 DateTime.Parse(dr["FecNov"].ToString()),
                                                 double.Parse(dr.GetValue("ImporteTotal").ToString()),
                                                 byte.Parse(dr.GetValue("CantCuotas").ToString()),
                                                 Single.Parse(dr["Porcentaje"].ToString()),
                                                 dr["NroComprobante"].ToString(),
                                                 dr["MAC"].ToString(),
                                                 dr["FecImportacion"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecImportacion"].ToString()),
                                                 dr["PrimerMensual"].ToString(),
                                                 false, null,
                                                 dr["usuarioAlta"].ToString(),
                                                 dr["nroSucursal"].ToString() + " - " + dr["OficinaAlta"].ToString(),
                                                 dr["usuarioSupervision"].ToString(),
                                                 dr["fSupervision"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["fSupervision"].ToString()),
                                                 dr["NombreArchivo"].ToString());

                        unaNovedad.SaldoCredito = dr["Saldocredito"].Equals(DBNull.Value) ? 0 : double.Parse(dr.GetValue("Saldocredito").ToString());
                        unaNovedad.MontoPrestamo = dr["montoPrestamo"].Equals(DBNull.Value) ? 0 : double.Parse(dr.GetValue("montoPrestamo").ToString());
                        unaNovedad.TotalAmortizado = dr["TotalAmortizado"].Equals(DBNull.Value) ? 0 : double.Parse(dr.GetValue("TotalAmortizado").ToString());

                        unaNovedad.CantidadCuotasRestantes = dr.GetNullableInt32("CantidadCuotasRestantes");
                        unaNovedad.CuotasLiquidadas = dr.GetNullableInt32("CuotasLiquidadas");
                        unaNovedad.UnTipoConcepto = new TipoConcepto(Byte.Parse(dr["TipoConcepto"].ToString()),
                                                                     dr.GetString("DescTipoConcepto"));
                        unaNovedad.UnPrestador = new Prestador(long.Parse(dr["IdPrestador"].ToString()),
                                                               dr.GetString("RazonSocial"),
                                                               long.Parse(dr["CUIT_Prestador"].ToString()),
                                                               dr["entregaDocumentacionEnFGS"].Equals(DBNull.Value) ? false : bool.Parse(dr["entregaDocumentacionEnFGS"].ToString()));
                        unaNovedad.UnBeneficiario = new Beneficiario(long.Parse(dr["IdBeneficiario"].ToString()), 0,
                                                                     dr.GetString("ApellidoNombre"));
                        unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                                   dr.GetString("DescConceptoLiq"), bool.Parse(dr["EsAfiliacion"].ToString()));

                        unaNovedad.MensualCarga = dr["MensualCarga"].ToString();
                        unaNovedad.UltimaMensualCuota = dr["UltimaMensualCuota"].ToString();
                        unaNovedad.UnEstadoNovedad = new Estado(int.Parse(dr["idestado"].ToString()), dr["descripcionEstado"].ToString());
                        unaNovedad.ImporteCuota = dr["importeCuota"].Equals(DBNull.Value) ? 0 : double.Parse(dr.GetValue("importeCuota").ToString());
                        lstNovedades.Add(unaNovedad);
                    }
                }

                return lstNovedades;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en NovedadDAO -> Traer_Novedades_HistoricoArgenta", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }    
       
        #endregion
        
        #region Traer Novedades por Reclamos

        public static List<Novedad> Traer_Novedades_xa_Reclamos(long idBeneficiario)
        {
            string sql = "Novedades_Para_Reclamos";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                List<Novedad> lstNovedades = new List<Novedad>();

                db.AddInParameter(dbCommand, "@Beneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@codConceptoLiq", DbType.Int64, 0);
                Novedad unaNovedad;

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        #region
                        /*                            
                           Novedades.IdNovedad,   
                           Novedades.FecNov, 
                           Novedades.ImporteTotal, 
                           Novedades.Porcentaje,
                           Novedades.nrocomprobante,
                           Novedades.idBeneficiario,  
                           Beneficiarios.ApellidoNombre, 
                           Beneficiarios.Cuil
                           Novedades.TipoConcepto, 
                           TipoConcepto.DescTipoConcepto, 
                           Novedades.CodConceptoLiq, 
                           CodConceptoLiquidacion.DescConceptoLiq,
                           Prestadores.IdPrestador, 
                           Prestadores.RazonSocial,
                           Prestadores.CUIT as CUIT_Prestador, 
                        */
                        #endregion

                        unaNovedad = new Novedad(long.Parse(dr["IdNovedad"].ToString()),
                                                 DateTime.Parse(dr["FecNov"].ToString()),
                                                 double.Parse(dr.GetValue("ImporteTotal").ToString()),
                                                 byte.Parse(dr.GetValue("CantCuotas").ToString()),
                                                 Single.Parse(dr["Porcentaje"].ToString()),
                                                 dr["NroComprobante"].ToString(),
                                                 string.Empty, null,
                                                 dr["PrimerMensual"].ToString(),
                                                 false, null);

                        unaNovedad.UnTipoConcepto = new TipoConcepto(Byte.Parse(dr["TipoConcepto"].ToString()),
                                                                     dr.GetString("DescTipoConcepto"));
                        unaNovedad.UnPrestador = new Prestador(long.Parse(dr["IdPrestador"].ToString()),
                                                               dr.GetString("RazonSocial"),
                                                               long.Parse(dr["CUIT_Prestador"].ToString()));
                        unaNovedad.UnBeneficiario = new Beneficiario(long.Parse(dr["IdBeneficiario"].ToString()),
                                                                     long.Parse(dr["Cuil"].ToString()),
                                                                     dr.GetString("ApellidoNombre"));
                        unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                                   dr.GetString("DescConceptoLiq"));

                        lstNovedades.Add(unaNovedad);
                    }

                    dr.Close();
                    dr.Dispose();

                }
                return lstNovedades;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en NovedadDAO.Traer_Novedades_xa_Reclamos", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

        }

        public static List<Novedad> Traer_Novedades_xa_Reclamos(long idBeneficiario, int codConceptoLiq)
        {
            string sql = "Novedades_Para_Reclamos";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                List<Novedad> lstNovedades = new List<Novedad>();

                db.AddInParameter(dbCommand, "@Beneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@codConceptoLiq", DbType.Int64, codConceptoLiq);
                Novedad unaNovedad;

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        unaNovedad = new Novedad(long.Parse(dr["IdNovedad"].ToString()),
                                                 DateTime.Parse(dr["FecNov"].ToString()),
                                                 double.Parse(dr.GetValue("ImporteTotal").ToString()),
                                                 byte.Parse(dr.GetValue("CantCuotas").ToString()),
                                                 Single.Parse(dr["Porcentaje"].ToString()),
                                                 dr["NroComprobante"].ToString(),
                                                 string.Empty, null,
                                                 dr["PrimerMensual"].ToString(),
                                                 false, null);

                        unaNovedad.UnTipoConcepto = new TipoConcepto(Byte.Parse(dr["TipoConcepto"].ToString()),
                                                                     dr.GetString("DescTipoConcepto"));
                        unaNovedad.UnPrestador = new Prestador(long.Parse(dr["IdPrestador"].ToString()),
                                                               dr.GetString("RazonSocial"),
                                                               long.Parse(dr["CUIT_Prestador"].ToString()));
                        unaNovedad.UnBeneficiario = new Beneficiario(long.Parse(dr["IdBeneficiario"].ToString()),
                                                                     long.Parse(dr["Cuil"].ToString()),
                                                                     dr.GetString("ApellidoNombre"));
                        unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                                   dr.GetString("DescConceptoLiq"));

                        lstNovedades.Add(unaNovedad);
                    }

                    dr.Close();
                    dr.Dispose();
                }
                return lstNovedades;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en NovedadDAO.Traer_Novedades_xa_Reclamos", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

        }
        #endregion

        #region Traer Novedades por Cuenta Corriente

        public static List<Novedades_CTACTE> Traer_Novedades_TT_XA_CTACTE(long? idBeneficiario, long? cuilBeneficiario, long? nroNovedad, out string ApellidoNombre, out string CuilRta)
        {
            string sql = "Novedades_TT_XA_CTACTE";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            ApellidoNombre = string.Empty;
            CuilRta = string.Empty;

            try
            {
                List<Novedades_CTACTE> lstNovedades = new List<Novedades_CTACTE>();

                db.AddInParameter(dbCommand, "@idBeneficiario", DbType.Int64, idBeneficiario == null ? 0 : idBeneficiario);
                db.AddInParameter(dbCommand, "@cuil", DbType.Int64, cuilBeneficiario == null ? 0 : cuilBeneficiario);
                db.AddInParameter(dbCommand, "@idnovedad", DbType.Int64, nroNovedad == null ? 0 : nroNovedad);

                db.AddOutParameter(dbCommand, "@cuilRta", DbType.Int64, 1);
                db.AddOutParameter(dbCommand, "@ApellidoNombre", DbType.String, 25);

                Novedades_CTACTE unaNovedad;

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        unaNovedad = new Novedades_CTACTE(dr["idnovedad"] == null ? (long?)null : long.Parse(dr["idnovedad"].ToString()),
                                                          string.IsNullOrEmpty(dr["idBeneficiario"].ToString()) ? (long?)null : long.Parse(dr["idBeneficiario"].ToString()),
                                                          dr["idEstadoSC"] == null ? null : (int?)int.Parse(dr["idEstadoSC"].ToString()),
                                                          dr["DescripcionEstadoSC"] == null ? null : dr["DescripcionEstadoSC"].ToString());

                        lstNovedades.Add(unaNovedad);
                    }
                    dr.Close();
                    dr.Dispose();
                }

                ApellidoNombre = db.GetParameterValue(dbCommand, "@ApellidoNombre").ToString();
                CuilRta = db.GetParameterValue(dbCommand, "@cuilRta").ToString();

                //Para evitar multiples output del servicio
                if (lstNovedades.Count > 0)
                {
                    lstNovedades[0].ApellidoNombre = ApellidoNombre;
                    lstNovedades[0].CuilRta = CuilRta;
                }

                return lstNovedades;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en NovedadDAO.Traer_Novedades_TT_XA_CTACTE", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        public static List<NovedadInventario> Traer_Novedades_CTACTE_Inventario(Int64? _cuil, DateTime? _fAltaDesde, DateTime? _fAltaHasta,
                                                                                DateTime? _fCambioEstadoSC_Desde, DateTime? _fCambioEstadoSC_hasta,
                                                                                Int32 _idEstadoSC, Int32 _canCuotas, Int32 _idprestador, Int32 _codConceptoliq, Int64 _idnovedad,
                                                                                Decimal ? _saldoAmortizacionDesde, Decimal ? _saldoAmortizacionHasta,
                                                                                int _nroPagina,
                                                                                bool _generaArchivo, bool _generadoAdmin, out string _mensajeError, out Int32 _cantNovedades,
                                                                                out string _rutaArchivoSal, out int _cantPaginas)
        {
            string rutaArchivo = string.Empty;
            string nombreArchivo =_rutaArchivoSal = _mensajeError =string.Empty;
            string msgRta = string.Empty;          
            ConsultaBatch consultaBatch = new ConsultaBatch();
            consultaBatch.NombreConsulta = ConsultaBatch.enum_ConsultaBatch_NombreConsulta.NOVEDADES_CTACTE_INVENTARIO;
            consultaBatch.IDPrestador = _idprestador;
            consultaBatch.UnConceptoLiquidacion = new ConceptoLiquidacion(_codConceptoliq, string.Empty);
            consultaBatch.CUIL_Usuario = _cuil.HasValue? _cuil.ToString(): string.Empty;
            consultaBatch.FechaDesde = _fAltaDesde;
            consultaBatch.FechaHasta = _fAltaHasta;
            consultaBatch.FechaCambioEstadoDesde = _fCambioEstadoSC_Desde;
            consultaBatch.FechaCambioEstadoHasta = _fCambioEstadoSC_hasta;
            consultaBatch.IdEstado_Documentacion = _idEstadoSC;
            consultaBatch.Cuotas = _canCuotas;
            consultaBatch.Idnovedad =_idnovedad;
            consultaBatch.GeneraArchivo = _generaArchivo;
            consultaBatch.GeneradoAdmin = _generadoAdmin;
            consultaBatch.SaldoAmortizacionDesde = _saldoAmortizacionDesde;
            consultaBatch.SaldoAmortizacionHasta = _saldoAmortizacionHasta;
            try
            {           

                if (consultaBatch.OpcionBusqueda != 1 || consultaBatch.GeneraArchivo == true)
                {                    
                    msgRta = ConsultasBatchDAO.ExisteConsulta(consultaBatch);

                    log.Info(String.Format("Existe Consulta: {0}", msgRta));

                    if (!string.IsNullOrEmpty(msgRta))
                        throw new ApplicationException("MSG_ERROR" + msgRta + "FIN_MSG_ERROR");

                }
               _nroPagina =  consultaBatch.GeneraArchivo == true ? - 1: _nroPagina;
                
               List<NovedadInventario> listNovedades = Novedades_CTACTE_Inventario(string.IsNullOrEmpty(consultaBatch.CUIL_Usuario) ? (long?)null : long.Parse(consultaBatch.CUIL_Usuario),
                                                                                    consultaBatch.FechaDesde,
                                                                                    consultaBatch.FechaHasta,
                                                                                    consultaBatch.FechaCambioEstadoDesde,
                                                                                    consultaBatch.FechaCambioEstadoHasta,
                                                                                    consultaBatch.IdEstado_Documentacion.GetValueOrDefault(),
                                                                                    consultaBatch.Cuotas.GetValueOrDefault(),
                                                                                    Convert.ToInt32(consultaBatch.IDPrestador.ToString()),
                                                                                    consultaBatch.UnConceptoLiquidacion != null ? consultaBatch.UnConceptoLiquidacion.CodConceptoLiq : 0,
                                                                                    string.IsNullOrEmpty(consultaBatch.Idnovedad.ToString()) ? 0 :(long)consultaBatch.Idnovedad,
                                                                                    consultaBatch.SaldoAmortizacionDesde,consultaBatch.SaldoAmortizacionHasta,
                                                                                    _nroPagina,
                                                                                    out _cantNovedades, out _cantPaginas);
                
                if (listNovedades.Count > 0 && (consultaBatch.OpcionBusqueda != 1 || consultaBatch.GeneraArchivo == true))
                {                    
                    int maxCantidad = Settings.MaxCantidadRegistros();

                    if (listNovedades.Count >= maxCantidad || consultaBatch.GeneraArchivo == true)
                    {
                        nombreArchivo = Utilidades.GeneraNombreArchivo(consultaBatch.NombreConsulta.ToString(), consultaBatch.IDPrestador, out rutaArchivo);
                        _rutaArchivoSal = Path.Combine(rutaArchivo, nombreArchivo);
                        StreamWriter sw = new StreamWriter(_rutaArchivoSal, false, Encoding.UTF8);
                        string separador = Settings.DelimitadorCampo();
                                                
                        var value = Utilidades.Encabezado_Archivo_CTACTE_Inventario();
                        sw.WriteLine(new StringBuilder(value));

                        log.Info(string.Format(" INICIO - GeneraArchivo {0}  -> cant Novedades: {1}", nombreArchivo, _cantNovedades));

                       foreach (NovedadInventario oNovedad in listNovedades)
                        {
                            StringBuilder linea = new StringBuilder();                            
                            linea.Append(oNovedad.cuil.ToString() + separador);
                            linea.Append(oNovedad.beneficio.ToString() + separador);
                            linea.Append(oNovedad.apellidoNombre.ToString() + separador);
                            linea.Append(oNovedad.idnovedad.ToString() + separador);
                            linea.Append(oNovedad.fechaAlta.ToString("yyyyMMdd HH:mm:ss") + separador);
                            linea.Append(oNovedad.codconceptoliq.ToString() + separador);
                            linea.Append(oNovedad.montoPrestamo.ToString().Replace(",", ".") + separador); 
                            linea.Append(oNovedad.cantCuotas.ToString() + separador);
                            linea.Append(oNovedad.tna.ToString().Replace(",", ".") + separador);
                            linea.Append(oNovedad.totAmortizado.ToString().Replace(",", ".") + separador);
                            linea.Append(oNovedad.totResidual.ToString().Replace(",", ".") + separador);
                            linea.Append(oNovedad.idestadosc.ToString() + separador);
                            linea.Append(oNovedad.descripcionDelEstado.ToString() + separador);
                            linea.Append(oNovedad.fechaCambioEstado.ToString("yyyyMMdd HH:mm:ss") + separador );
                            linea.Append(oNovedad.cantCuotasSinLiq);
                            sw.WriteLine(linea.ToString());
                        }
                        sw.Close();

                        log.Info(string.Format(" FIN - GeneraArchivo {0}  -> cant Novedades: {1}", nombreArchivo, _cantNovedades));

                        Utilidades.ComprimirArchivo(rutaArchivo, nombreArchivo);
                        Utilidades.BorrarArchivo(_rutaArchivoSal);
                        nombreArchivo = nombreArchivo + ".zip";

                        consultaBatch.NomArchGenerado = nombreArchivo;
                        consultaBatch.RutaArchGenerado = rutaArchivo;
                        consultaBatch.FechaGenera = DateTime.Now;
                        consultaBatch.Vigente = true;

                        log.Info(string.Format("Inicio AltaNuevaConsulta - Generación de fin de archivo OK - Nueva Consulta {0} - FechaGenera", nombreArchivo, consultaBatch.FechaGenera));

                        msgRta = ConsultasBatchDAO.AltaNuevaConsulta(consultaBatch);

                        if (!string.IsNullOrEmpty(msgRta))
                        {
                            log.Info(String.Format("Salio por {0}", msgRta));
                            msgRta = "MSG_ERROR" + msgRta + "FIN_MSG_ERROR";
                            throw new ApplicationException(msgRta);
                        }
                        /* Se instacia el objeto para que no muestre los 
                        * registros y pueda ver solo el archivo generado. */
                        listNovedades = new List<NovedadInventario>();
                    }
                }

                return listNovedades;
            }          
            catch (ApplicationException apperr)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), apperr.Source, apperr.Message));
                throw new ApplicationException(apperr.Message);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));

                consultaBatch.NomArchGenerado = Utilidades.GeneraNombreArchivo(consultaBatch.NombreConsulta.ToString(), consultaBatch.IDPrestador, out rutaArchivo);
                consultaBatch.RutaArchGenerado = rutaArchivo;
                consultaBatch.FechaGenera = DateTime.MinValue;
                consultaBatch.Vigente = false;
                log.Error(string.Format("NomArchGenerado: {0}-> FechaGenera:{1}: Vigente :{2}", consultaBatch.NomArchGenerado, consultaBatch.FechaGenera, consultaBatch.Vigente));

                msgRta = ConsultasBatchDAO.AltaNuevaConsulta(consultaBatch);

                throw new ApplicationException("MSG_ERROR Generando el archivo. Reingrese a la consulta en unos minutos.FIN_MSG_ERROR");          
              }
        }

        public static List<NovedadInventario> Novedades_CTACTE_Inventario(Int64? _cuil, DateTime? _fAltaDesde, DateTime? _fAltaHasta,
                                                                          DateTime? _fCambioEstadoSC_Desde, DateTime? _fCambioEstadoSC_hasta,
                                                                          Int32 _idEstadoSC, Int32 _canCuotas, Int32 _idprestador,
                                                                          Int32 _codConceptoliq,Int64  _idnovedad,
                                                                          Decimal? _saldoAmortizacionDesde , Decimal? _saldoAmortizacionHasta,
                                                                          int _nroPagina,out Int32 _cantNovedades, out int _cantPaginas)
        {
            List<NovedadInventario> lstNovedades = new List<NovedadInventario>();

            string sql = "Novedades_CTACTE_Inventario";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            _cantPaginas = _cantNovedades = 0;


            try
            {
                db.AddInParameter(dbCommand, "@cuil", DbType.Int64, _cuil == null ? null : _cuil);
                db.AddInParameter(dbCommand, "@fAltaDesde", DbType.Date, _fAltaDesde == null ? null : _fAltaDesde);
                db.AddInParameter(dbCommand, "@fAltaHasta", DbType.Date, _fAltaHasta == null ? null : _fAltaHasta);
                db.AddInParameter(dbCommand, "@fCambioEstadoSC_Desde", DbType.Date, _fCambioEstadoSC_Desde == null ? null : _fCambioEstadoSC_Desde);
                db.AddInParameter(dbCommand, "@fCambioEstadoSC_hasta", DbType.Date, _fCambioEstadoSC_hasta == null ? null : _fCambioEstadoSC_hasta);
                db.AddInParameter(dbCommand, "@idEstadoSC", DbType.Int32, _idEstadoSC);
                db.AddInParameter(dbCommand, "@canCuotas", DbType.Int32, _canCuotas);
                db.AddInParameter(dbCommand, "@idprestador", DbType.Int32, _idprestador);
                db.AddInParameter(dbCommand, "@codConceptoliq", DbType.Int32, _codConceptoliq);
                db.AddInParameter(dbCommand, "@idnovedad", DbType.Int64, _idnovedad);
                db.AddInParameter(dbCommand, "@saldoAmortizacionDesde", DbType.Decimal, _saldoAmortizacionDesde == null ? null : _saldoAmortizacionDesde);
                db.AddInParameter(dbCommand, "@saldoAmortizacionHasta", DbType.Decimal, _saldoAmortizacionHasta == null ? null : _saldoAmortizacionHasta);
                db.AddInParameter(dbCommand, "@nroPagina", DbType.Int32, _nroPagina);
                db.AddOutParameter(dbCommand, "@cantNov", DbType.Int32, 1);
                db.AddOutParameter(dbCommand, "@cantPaginas", DbType.Int32, 1);
                NovedadInventario unaNovedad;

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        unaNovedad = new NovedadInventario(dr["cuil"] == null ? 0 : long.Parse(dr["cuil"].ToString()),
                                                            dr["idbeneficiario"] == null ? 0 : long.Parse(dr["idbeneficiario"].ToString()),
                                                            dr["ApellidoNombre"] == null ? string.Empty : dr["ApellidoNombre"].ToString(),
                                                            dr["idnovedad"] == null ? 0 : int.Parse(dr["idnovedad"].ToString()),
                                                            dr["fAlta"] == null ? new DateTime() : DateTime.Parse(dr["fAlta"].ToString()),
                                                            dr["codconceptoliq"] == null ? 0 : Int32.Parse(dr["codconceptoliq"].ToString()),
                                                            dr["montoPrestamo"] == null ? 0 : float.Parse(dr["montoPrestamo"].ToString()),
                                                            dr["cantcuotas"] == null ? 0 : Int32.Parse(dr["cantcuotas"].ToString()),
                                                            dr["tna"] == null ? 0 : float.Parse(dr["tna"].ToString()),
                                                            dr["idestadosc"] == null ? 0 : Int32.Parse(dr["idestadosc"].ToString()),
                                                            dr["desEstadosc"] == null ? string.Empty : dr["desEstadosc"].ToString(),
                                                            dr["fCambioEstado"] == null ? new DateTime() : DateTime.Parse(dr["fCambioEstado"].ToString()),
                                                            dr["totAmortizado"] == null ? 0 : float.Parse(dr["totAmortizado"].ToString()),
                                                            dr["totResidual"] == null ? 0 : float.Parse(dr["totResidual"].ToString()),
                                                            dr["CantCuotasSinLiq"].Equals(DBNull.Value) ? 0 : Int32.Parse(dr["CantCuotasSinLiq"].ToString())
                                                            );

                        lstNovedades.Add(unaNovedad);
                    }

                    dr.Close();
                    dr.Dispose();
                }
                _cantNovedades = Int32.Parse(db.GetParameterValue(dbCommand, "@cantNov").ToString());
                _cantPaginas = int.Parse(db.GetParameterValue(dbCommand, "@cantPaginas").ToString());
                return lstNovedades;
            }
            catch (SqlException ErrSQL)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}-{4}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message, ErrSQL.Number));
                throw ErrSQL;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en NovedadDAO.Traer_Novedades_CTACTE_Inventario", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        public static List<NovedadTotal> Traer_Novedades_CTACTE_Total(Int64? _cuil, DateTime? _fAltaDesde, DateTime? _fAltaHasta,
                                                                      DateTime? _fCambioEstadoSC_Desde, DateTime? _fCambioEstadoSC_hasta,
                                                                      Int32 _idEstadoSC, Int32 _canCuotas, Int32 _idPrestador, Int32 _codConceptoliq,
                                                                       Decimal? _saldoAmortizacionDesde, Decimal? _saldoAmortizacionHasta
                                                                     )
        {
            List<NovedadTotal> lstNovedades = new List<NovedadTotal>();

            string sql = "Novedades_CTACTE_Totales";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            
            try
            {
                db.AddInParameter(dbCommand, "@cuil", DbType.Int64, _cuil == null ? null : _cuil);
                db.AddInParameter(dbCommand, "@fAltaDesde", DbType.Date, _fAltaDesde == null ? null : _fAltaDesde);
                db.AddInParameter(dbCommand, "@fAltaHasta", DbType.Date, _fAltaHasta == null ? null : _fAltaHasta);
                db.AddInParameter(dbCommand, "@fCambioEstadoSC_Desde", DbType.Date, _fCambioEstadoSC_Desde == null ? null : _fCambioEstadoSC_Desde);
                db.AddInParameter(dbCommand, "@fCambioEstadoSC_hasta", DbType.Date, _fCambioEstadoSC_hasta == null ? null : _fCambioEstadoSC_hasta);
                db.AddInParameter(dbCommand, "@idEstadoSC", DbType.Int32, _idEstadoSC);
                db.AddInParameter(dbCommand, "@canCuotas", DbType.Int32, _canCuotas);
                db.AddInParameter(dbCommand, "@idprestador", DbType.Int32, _idPrestador);
                db.AddInParameter(dbCommand, "@codConceptoliq", DbType.Int32, _codConceptoliq);
                db.AddInParameter(dbCommand, "@saldoAmortizacionDesde", DbType.Decimal, _saldoAmortizacionDesde == null ? null : _saldoAmortizacionDesde);
                db.AddInParameter(dbCommand, "@saldoAmortizacionHasta", DbType.Decimal, _saldoAmortizacionHasta == null ? null : _saldoAmortizacionHasta);

                NovedadTotal unaNovedad;

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    unaNovedad = new NovedadTotal();
                    lstNovedades.Add(unaNovedad);

                    while (dr.Read())
                    {
                        int idEstadoSc = dr["idestadosc"] == null ? 0 : Int32.Parse(dr["idestadosc"].ToString());
                        NovedadTotal novedadExistente = traerNovedadTotalExistente(lstNovedades, idEstadoSc);
                        if (novedadExistente != null)
                        {
                            ContenedorDeCuotas contenedorDeCuotas = new ContenedorDeCuotas(
                                dr["codconceptoliq"] == null ? 0 : Int32.Parse(dr["codconceptoliq"].ToString()),
                                dr["Cant_1Cuotas"] == null ? 0 : Int32.Parse(dr["Cant_1Cuotas"].ToString()),
                                dr["Cant_12Cuotas"] == null ? 0 : Int32.Parse(dr["Cant_12Cuotas"].ToString()),
                                dr["Cant_24Cuotas"] == null ? 0 : Int32.Parse(dr["Cant_24Cuotas"].ToString()),
                                dr["Cant_36Cuotas"] == null ? 0 : Int32.Parse(dr["Cant_36Cuotas"].ToString()),
                                dr["Cant_40Cuotas"] == null ? 0 : Int32.Parse(dr["Cant_40Cuotas"].ToString()),
                                dr["Cant_48Cuotas"] == null ? 0 : Int32.Parse(dr["Cant_48Cuotas"].ToString()),
                                dr["Cant_60Cuotas"] == null ? 0 : Int32.Parse(dr["Cant_60Cuotas"].ToString())
                                
                            );
                            novedadExistente.AgregarContenedorDeCuotas(contenedorDeCuotas);
                        }
                        else
                        {
                            unaNovedad = new NovedadTotal(
                                idEstadoSc, 1,
                                //dr["idbeneficiario"] == null ? 0 : Int32.Parse(dr["idbeneficiario"].ToString()),
                                dr["descripcion"] == null ? string.Empty : dr["descripcion"].ToString(),
                                dr["codconceptoliq"] == null ? 0 : Int32.Parse(dr["codconceptoliq"].ToString()),
                                dr["Cant_1Cuotas"] == null ? 0 : Int32.Parse(dr["Cant_1Cuotas"].ToString()),
                                dr["Cant_12Cuotas"] == null ? 0 : Int32.Parse(dr["Cant_12Cuotas"].ToString()),
                                dr["Cant_24Cuotas"] == null ? 0 : Int32.Parse(dr["Cant_24Cuotas"].ToString()),
                                dr["Cant_36Cuotas"] == null ? 0 : Int32.Parse(dr["Cant_36Cuotas"].ToString()),
                                dr["Cant_40Cuotas"] == null ? 0 : Int32.Parse(dr["Cant_40Cuotas"].ToString()),
                                dr["Cant_48Cuotas"] == null ? 0 : Int32.Parse(dr["Cant_48Cuotas"].ToString()),
                                dr["Cant_60Cuotas"] == null ? 0 : Int32.Parse(dr["Cant_60Cuotas"].ToString())
                            );
                            lstNovedades.Add(unaNovedad);
                        }
                    }
                    dr.Close();
                    dr.Dispose();
                }

                lstNovedades = CalcularTotalizador(lstNovedades);


                return lstNovedades;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en NovedadDAO.Traer_Novedades_CTACTE_Total", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        private static List<NovedadTotal> CalcularTotalizador(List<NovedadTotal> lstNovedades)
        {
            List<NovedadTotal> result = lstNovedades;

            if (lstNovedades == null || lstNovedades.Count == 0)
                lstNovedades = new List<NovedadTotal>();

            result[0].Descripcion = "Total General";

            foreach (NovedadTotal item in lstNovedades)
            {
                result[0].Total1Cuotas += item.Total1Cuotas;
                result[0].Total12Cuotas += item.Total12Cuotas;
                result[0].Total24Cuotas += item.Total24Cuotas;
                result[0].Total36Cuotas += item.Total36Cuotas;
                result[0].Total40Cuotas += item.Total40Cuotas;
                result[0].Total48Cuotas += item.Total48Cuotas;
                result[0].Total60Cuotas += item.Total60Cuotas;
                result[0].TotalAcumulado += item.TotalAcumulado;
            }

            return result;
        }

        private static NovedadTotal traerNovedadTotalExistente(List<NovedadTotal> novedades, int id)
        {
            return novedades.Exists(n => n.NumeroEstado == id) ? novedades.Find(n => n.NumeroEstado == id) : null;
        }

        #endregion

        # region Traer Novedades por Flujo de Fondos

        public static List<FlujoFondo> Traer_Novedades_Flujo_Fondos_TT(long idPrestador, long codConceptoLiq, int primerMensualDesde, int primerMensualHasta)
        {
            string sql = "Novedades_Flujo_Fondos_TT";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                List<FlujoFondo> lstFlujoFondo = new List<FlujoFondo>();

                db.AddInParameter(dbCommand, "@idprestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@codconceptoliq", DbType.Int64, codConceptoLiq);
                db.AddInParameter(dbCommand, "@primermensualdesde", DbType.Int32, primerMensualDesde);
                db.AddInParameter(dbCommand, "@primermensualHasta", DbType.Int32, primerMensualHasta);

                FlujoFondo unaFlujoFondo;

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        unaFlujoFondo = new FlujoFondo(dr["RazonSocial"] == null ? "" : dr["RazonSocial"].ToString(),
                                                       dr["mensual"] == null ? (Int32)0 : Int32.Parse(dr["mensual"].ToString()),
                                                       dr["cantCreditos"] == null ? (Int64)0 : Int64.Parse(dr["cantCreditos"].ToString()),
                                                       dr["TotalMontoPrestamo"] is DBNull ? (Decimal)0 : Decimal.Parse(dr["TotalMontoPrestamo"].ToString()),
                                                       dr["TotalImporteCuota"] is DBNull ? (Decimal)0 : Decimal.Parse(dr["TotalImporteCuota"].ToString()),
                                                       dr["TotalGastoAdministrativo"] is DBNull ? (Decimal)0 : Decimal.Parse(dr["TotalGastoAdministrativo"].ToString()),
                                                       dr["TotalGastoAdmTarjeta"] is DBNull ? (Decimal)0 : Decimal.Parse(dr["TotalGastoAdmTarjeta"].ToString()),
                                                       dr["TotalSeguroVida"] is DBNull ? (Decimal)0 : Decimal.Parse(dr["TotalSeguroVida"].ToString()),
                                                       dr["TotalAmortizacion"] is DBNull ? (Decimal)0 : Decimal.Parse(dr["TotalAmortizacion"].ToString()),
                                                       dr["TotalInteres"] is DBNull ? (Decimal)0 : Decimal.Parse(dr["TotalInteres"].ToString()),
                                                       dr["totalInteresCuotaCero"] is DBNull ? (Decimal)0 : Decimal.Parse(dr["totalInteresCuotaCero"].ToString())
                                                       );

                        lstFlujoFondo.Add(unaFlujoFondo);
                    }
                    dr.Close();
                    dr.Dispose();
                }

                return lstFlujoFondo;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en NovedadDAO.Traer_Novedades_Flujo_Fondos_TT", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        # endregion

        #region

        public static List<FlujoFondo> Novedades_Flujo_Fondos_TMensuales(long idPrestador, long codConceptoLiq)
        {
            string sql = "Novedades_Flujo_Fondos_TMensuales";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<FlujoFondo> lista_FF_M = new List<FlujoFondo>();

            try
            {
                db.AddInParameter(dbCommand, "@idprestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@codconceptoliq", DbType.Int64, codConceptoLiq);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        lista_FF_M.Add(new FlujoFondo(dr["primermensual"] == null ? (Int32)0 : int.Parse(dr["primermensual"].ToString())));
                    }

                    dr.Close();
                    dr.Dispose();
                }

                return lista_FF_M;
              
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en NovedadDAO.Novedades_Flujo_Fondos_TMensuales", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        
        }

        #endregion
        
        #region Traer Novedades Por CambioEstadoSC
        public static List<NovedadCambioEstado> Novedades_CambioEstadoSC_Histo_TT(Int64 idnovedad)
        {
            string sql = "Novedades_CambioEstadoSC_Histo_TT";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                List<NovedadCambioEstado> lstNovedades = new List<NovedadCambioEstado>();

                db.AddInParameter(dbCommand, "@idnovedad", DbType.Int64, idnovedad);
                NovedadCambioEstado unaNovedad;

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        unaNovedad = new NovedadCambioEstado(
                            dr["idnovedad"] == DBNull.Value ? 0 : Int64.Parse(dr["idnovedad"].ToString()),
                            dr["fCambioEstadoSC"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(dr["fCambioEstadoSC"].ToString()),
                            dr["idestadoSC"] == DBNull.Value ? 0 : Int32.Parse(dr["idestadoSC"].ToString()),
                            dr["DescripcionEstadoSC"] == DBNull.Value ? string.Empty : dr["DescripcionEstadoSC"].ToString(),
                            dr["usuarioCambioEstado"] == DBNull.Value ? string.Empty : dr["usuarioCambioEstado"].ToString(),
                            dr["ipCambioEstado"] == DBNull.Value ? string.Empty : dr["ipCambioEstado"].ToString(),
                            dr["oficinaCambioEstado"] == DBNull.Value ? string.Empty : dr["oficinaCambioEstado"].ToString()
                        );

                        lstNovedades.Add(unaNovedad);
                    }
                    dr.Close();
                    dr.Dispose();
                }

                return lstNovedades;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, "Novedades_CambioEstadoSC_Histo_TT", ex.Source, ex.Message));
                throw new Exception("Error en NovedadDAO.Novedades_CambioEstadoSC_Histo_TT", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion
        #region Traer Motivo Baja
        public static string Traer_Motivo_Baja(long idBeneficiario, int codConceptoLiq)
        {
            string sql = "Reclamos_NovedadesBaja_Motivo_Trae";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            string strMotivo = "";
            try
            {
                List<Ar.Gov.Anses.Microinformatica.DAT.Entidades.Novedad> lstNovedades = new List<Ar.Gov.Anses.Microinformatica.DAT.Entidades.Novedad>();

                db.AddInParameter(dbCommand, "@idbeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@codConceptoLiq", DbType.Int64, codConceptoLiq);

                dbParametros = dbCommand.Parameters;

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    if (dr.Read())
                    {
                        strMotivo = dr.GetString("DescripcionEstadoReg");
                    }
                    dr.Close();
                    dr.Dispose();

                }
                return strMotivo;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en NovedadDAO.Traer_Motivo_Baja", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

        }
        #endregion

        #region *** Transaccional

        #region ABM Novedad

        #region Novedades_Alta

        public static string Novedades_Alta(long idPrestador, long idBeneficiario, short tipoConcepto, int conceptoOPP, double impTotal,
                                             byte cantCuotas, Single porcentaje, string nroComprobante, string ip, string usuario,
                                            int mensual, List<Adherente> unaLista_Adherentes)
        {
            try
            {
                string retorno = string.Empty;

                using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                {

                    retorno = Novedades_Alta(idPrestador, idBeneficiario, tipoConcepto, conceptoOPP, impTotal, cantCuotas, porcentaje,
                                             nroComprobante, ip, usuario, mensual);

                    if (string.IsNullOrEmpty(retorno.Split(char.Parse("|"))[0].ToString().Trim()))
                    {
                        long idNovedad = long.Parse(retorno.Split(char.Parse("|"))[1].ToString().Trim());

                        foreach (Adherente unA in unaLista_Adherentes)
                        {
                            string sql = "Adherente_Alta";
                            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
                            DbCommand dbCommand = db.GetStoredProcCommand(sql);

                            db.AddInParameter(dbCommand, "@idNovedad", DbType.Int64, idNovedad);
                            db.AddInParameter(dbCommand, "@cuilAdherente", DbType.Int64, unA.CUIL);
                            db.AddInParameter(dbCommand, "@apellidoNombre", DbType.String, unA.Apellido_Nombre);
                            db.AddInParameter(dbCommand, "@ip", DbType.String, ip);
                            db.AddInParameter(dbCommand, "@usuario", DbType.String, usuario);

                            db.ExecuteNonQuery(dbCommand);

                            dbCommand.Dispose();
                        }
                    }
                    oTransactionScope.Complete();
                }
                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }


        public static string Novedades_Alta(long idPrestador, long idBeneficiario, short tipoConcepto, int conceptoOPP, double impTotal,
                                            byte cantCuotas, Single porcentaje, string nroComprobante, string ip, string usuario, int mensual)
        {
            byte idEstadoReg;
            DateTime fecNovedad;
            string mensaje = string.Empty;
            string retorno = string.Empty;
            string respta = string.Empty;
            Boolean esAfiliacion = false;

            try
            {
                esAfiliacion = true;
                fecNovedad = DateTime.Now;
                idEstadoReg = 1;

                respta = Valido_Novedad(idPrestador, idBeneficiario, tipoConcepto, conceptoOPP, impTotal, cantCuotas, porcentaje, 6, nroComprobante);

                mensaje = respta.Split('|')[0].ToString();

                if (mensaje != String.Empty)
                {
                    retorno = string.Concat(mensaje, "|0|");
                }
                else
                {
                    esAfiliacion = Boolean.Parse(respta.Split(char.Parse("|"))[1].ToString());

                    switch (tipoConcepto)
                    {
                        case 1:
                        case 6:
                            if (esAfiliacion == true)
                                retorno = Novedades_T1o6_Alta_Afiliacion(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP,
                                                                        impTotal, porcentaje, nroComprobante, ip, usuario, mensual, idEstadoReg);
                            else
                                retorno = Novedades_T1o6_Alta_No_Afiliacion(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP,
                                                                            impTotal, porcentaje, nroComprobante, ip, usuario, mensual, idEstadoReg);
                            break;
                        case 2: //ok
                            retorno = Novedades_T2_Alta(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP, impTotal,
                                                        nroComprobante, ip, usuario, mensual, idEstadoReg);
                            break;
                        case 3:
                            retorno = Novedades_T3_Alta(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP, impTotal,
                                                        cantCuotas, nroComprobante, ip, usuario, mensual, idEstadoReg);
                            break;
                        default:
                            retorno = "Operación inválida|0|";
                            break;
                    }
                }

                //09/01/12 - $3b@
                string mensajeError = retorno.Split(char.Parse("|"))[0].ToString().Trim();
                if (mensajeError != string.Empty)
                {
                    NovedadRechazada_Alta(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP, string.Empty, impTotal,
                                          cantCuotas, porcentaje, 6, nroComprobante, ip, usuario, mensual, mensajeError);
                }

                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region Novedades_Modificacion
        public static string Novedades_Modificacion(long idNovedadAnt, double ImpTotalN, Single PorcentajeN, string NroComprobanteN, string IPN,
                                                    string UsuarioN, int mensual, Boolean masiva, List<Adherente> unaLista_Adherentes)
        {
            try
            {
                string retorno = string.Empty;

                using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                {

                    retorno = Novedades_Modificacion(idNovedadAnt, ImpTotalN, PorcentajeN, NroComprobanteN, IPN,
                                                     UsuarioN, mensual, masiva);

                    if (string.IsNullOrEmpty(retorno.Split(char.Parse("|"))[0].ToString().Trim()))
                    {
                        long idNovedad = long.Parse(retorno.Split(char.Parse("|"))[1].ToString().Trim());

                        foreach (Adherente unA in unaLista_Adherentes)
                        {
                            string sql = "Adherente_Alta";
                            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
                            DbCommand dbCommand = db.GetStoredProcCommand(sql);

                            db.AddInParameter(dbCommand, "@idNovedad", DbType.Int64, idNovedad);
                            db.AddInParameter(dbCommand, "@cuilAdherente", DbType.Int64, unA.CUIL);
                            db.AddInParameter(dbCommand, "@apellidoNombre", DbType.String, unA.Apellido_Nombre);
                            db.AddInParameter(dbCommand, "@ip", DbType.String, IPN);
                            db.AddInParameter(dbCommand, "@usuario", DbType.String, UsuarioN);

                            db.ExecuteNonQuery(dbCommand);

                            dbCommand.Dispose();
                        }
                    }
                    oTransactionScope.Complete();
                }
                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }

        public static string Novedades_Modificacion(long idNovedadAnt, double ImpTotalN, Single PorcentajeN, string NroComprobanteN,
                                                    string IPN, string UsuarioN, int mensual, Boolean masiva)
        {
            Novedad oNovedad = new Novedad();
            List<Novedad> oListNovedades = new List<Novedad>();

            try
            {
                string mensaje = String.Empty;
                string retorno;

                long IdPrestador = 0;
                long IdBeneficiario = 0;
                short TipoConcepto = 0;
                double ImpTotalV = 0;
                Single PorcentajeV = 0;
                int IdEstadoRegV = 0;
                byte CodMovimientoV = 0;
                int ConceptoOPP = 0;

                string respta = String.Empty;
                Boolean EsAfiliacion = false;

                oListNovedades = NovedadDAO.Novedades_TxIdNovedad_Sliq(idNovedadAnt);

                if (oListNovedades.Count == 0)
                    mensaje = "No existe la novedad a modificar";
                else
                {
                    IdPrestador = oListNovedades[0].UnPrestador.ID;
                    IdBeneficiario = oListNovedades[0].UnBeneficiario.IdBeneficiario;
                    TipoConcepto = oListNovedades[0].UnTipoConcepto.IdTipoConcepto;
                    ImpTotalV = oListNovedades[0].ImporteTotal;
                    PorcentajeV = oListNovedades[0].Porcentaje;
                    IdEstadoRegV = oListNovedades[0].UnEstadoReg.IdEstado;
                    CodMovimientoV = oListNovedades[0].UnCodMovimiento.CodMovimiento;
                    ConceptoOPP = oListNovedades[0].UnConceptoLiquidacion.CodConceptoLiq;
                    EsAfiliacion = oListNovedades[0].UnConceptoLiquidacion.EsAfiliacion;
                }

                if (mensaje == String.Empty)
                {
                    if (IdEstadoRegV == 12)
                        mensaje = "No existe la novedad";
                    else
                    {
                        respta = Valido_Novedad(IdPrestador, IdBeneficiario, TipoConcepto, ConceptoOPP, ImpTotalN, 1, PorcentajeN, 5, NroComprobanteN);
                        mensaje = respta.Split('|')[0].ToString();
                    }
                }
                if (mensaje == String.Empty)
                {
                    switch (TipoConcepto)
                    {
                        case 1:
                        case 6:


                            retorno = Novedades_T1o6_Modificacion(idNovedadAnt, IdPrestador, IdBeneficiario, TipoConcepto, ConceptoOPP, ImpTotalN,
                                                                    PorcentajeN, NroComprobanteN, IPN, UsuarioN, mensual, IdEstadoRegV, ImpTotalV,
                                                                    PorcentajeV, CodMovimientoV, masiva, EsAfiliacion);
                            break;
                        case 2:
                            retorno = Novedades_T2_Modificacion(idNovedadAnt, IdPrestador, IdBeneficiario, TipoConcepto, ConceptoOPP, ImpTotalN,
                                                                NroComprobanteN, IPN, UsuarioN, IdEstadoRegV, ImpTotalV);
                            break;
                        default:
                            retorno = "Operación inválida|0|";
                            break;
                    }
                }
                else
                {
                    retorno = mensaje + "|0| ";
                }

                //09/01/12 - $3b@
                string mensajeError = retorno.Split(char.Parse("|"))[0].ToString().Trim();
                if (mensajeError != string.Empty)
                {
                    NovedadRechazada_Alta(IdPrestador, IdBeneficiario, DateTime.Now, TipoConcepto, ConceptoOPP, string.Empty, ImpTotalN, 0,
                                            PorcentajeN, 5, NroComprobanteN, IPN, UsuarioN, mensual, mensajeError);
                }

                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region Novedades_Baja

        public static string Novedades_Baja(long idNovedadAnt, string ip, string usuario, int mensual)
        {
            string retorno;
            int idEstadoReg;
            short tipoConcepto;

            try
            {
                retorno = String.Empty;
                idEstadoReg = 9;
                // busco la novedad a modificar

                List<Novedad> oListNovedades = NovedadDAO.Novedades_TxIdNovedad_Sliq(idNovedadAnt);

                if (oListNovedades.Count == 0)
                    retorno = "No existe la novedad|0| ";
                else
                {
                    int IdEstadoRegV = oListNovedades[0].UnEstadoReg.IdEstado;
                    if (IdEstadoRegV == 12)
                        retorno = "No existe la novedad|0| ";
                    else
                    {
                        using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                        {
                            tipoConcepto = oListNovedades[0].UnTipoConcepto.IdTipoConcepto;

                            switch (tipoConcepto)
                            {
                                case 1:
                                case 6:
                                    retorno = Novedades_T1o6_Baja(idEstadoReg, ip, usuario, oListNovedades);
                                    break;
                                case 2: //ok
                                    retorno = Novedades_T2_Baja(idEstadoReg, ip, usuario, oListNovedades);
                                    break;
                                case 3:
                                    idEstadoReg = 5;
                                    retorno = Novedades_T3_Baja(idEstadoReg, ip, usuario, mensual, oListNovedades[0]);
                                    break;
                                default:
                                    retorno = "Operación inválida|0| ";
                                    break;
                            }

                            oTransactionScope.Complete();
                        }
                    }
                }

                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region Novedades_Baja_Cuotas

        public static string Novedades_Baja_Cuotas(Novedad unaNovedad, string ip, string usuario)
        {
            string mensaje = string.Empty;

            try
            {
                if (unaNovedad.unaLista_Cuotas.Count > 0)
                {
                    using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        long idNovedadAnt = 0;
                        int mensual = 0;
                        foreach (Cuota oNovedadCuotaInd in unaNovedad.unaLista_Cuotas)
                        {
                            idNovedadAnt = unaNovedad.IdNovedad;
                            mensual = int.Parse(oNovedadCuotaInd.Mensual_Cuota);

                            mensaje = Novedades_Baja(idNovedadAnt, ip, usuario, mensual);
                            if (!mensaje.StartsWith("|"))
                            {
                                mensaje = mensaje.Split(char.Parse("|"))[0].ToString();
                                break;
                            }
                            else
                                mensaje = string.Empty;
                        }

                        oTransactionScope.Complete();
                    }
                }
                else
                    mensaje = "Se deben seleccionar las cuotas a dar de baja";

                return mensaje;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region Modificacion_Masiva_Indeterminadas

        public static List<Novedad> Modificacion_Masiva_Indeterminadas(List<Novedad> listNovedades, double monto, string ip, string usuario, bool masiva)
        {
            List<Novedad> oListNovedadesOut = new List<Novedad>();

            try
            {
                //dsSalida = listNovedades.Clone();

                if (listNovedades.Count > 0)
                {
                    long idNovedadAnt = 0;
                    int mensual = 0;
                    short tipoConcepto = 0;
                    double impTotN = 0;
                    Single porcentajeN = 0;
                    string comprobante = String.Empty;
                    string mensaje = string.Empty;
                    string msj = string.Empty;


                    foreach (Novedad oNovedad in listNovedades)
                    {
                        //DataRow drNFila = dsSalida.Tables[0].NewRow();
                        Novedad oNovedadOut = new Novedad();

                        tipoConcepto = oNovedad.UnTipoConcepto.IdTipoConcepto;
                        switch (tipoConcepto)
                        {
                            case 1:
                                impTotN = oNovedad.ImporteTotal + monto;
                                porcentajeN = 0;
                                break;
                            case 6:
                                porcentajeN = oNovedad.Porcentaje + float.Parse(monto.ToString());
                                impTotN = 0;
                                break;
                            default:
                                msj = "Tipo de Concepto Erróneo para Modidicación Masiva|0|";
                                break;
                        }

                        idNovedadAnt = oNovedad.IdNovedad;
                        mensual = int.Parse(oNovedad.PrimerMensual);
                        comprobante = oNovedad.Comprobante;

                        if (msj == string.Empty)
                            msj = Novedades_Modificacion(idNovedadAnt, impTotN, porcentajeN, comprobante, ip, usuario,
                                                         mensual, masiva);

                        if (msj.StartsWith("|")) // no hubo mensaje de error al realizar la modificacion
                        {
                            //drNFila["Mensaje"] = String.Empty;
                            oNovedadOut.IdNovedad = int.Parse(msj.Split(char.Parse("|"))[1].ToString());
                            oNovedadOut.MAC = msj.Split(char.Parse("|"))[2].ToString();
                            oNovedadOut.FechaNovedad = DateTime.Today;
                            oNovedadOut.ImporteTotal = impTotN;
                            oNovedadOut.Porcentaje = porcentajeN;

                        }
                        else
                        {
                            // no se produjo la modificación por algun motivo
                            //drNFila["Mensaje"] = msj.Split(char.Parse("|"))[0].ToString();
                            oNovedadOut.IdNovedad = idNovedadAnt;
                            oNovedadOut.MAC = oNovedad.MAC;
                            oNovedadOut.FechaNovedad = oNovedad.FechaNovedad;
                            oNovedadOut.ImporteTotal = oNovedad.ImporteTotal;
                            oNovedadOut.Porcentaje = oNovedad.Porcentaje;
                        }

                        oNovedadOut.PrimerMensual = mensual.ToString();
                        oNovedadOut.Comprobante = comprobante;
                        oNovedadOut.UnBeneficiario.IdBeneficiario = oNovedad.UnBeneficiario.IdBeneficiario;
                        oNovedadOut.UnBeneficiario.ApellidoNombre = oNovedad.UnBeneficiario.ApellidoNombre;
                        oNovedadOut.UnBeneficiario.Cuil = oNovedad.UnBeneficiario.Cuil;
                        oNovedadOut.UnBeneficiario.TipoDoc = oNovedad.UnBeneficiario.TipoDoc;
                        oNovedadOut.UnBeneficiario.NroDoc = oNovedad.UnBeneficiario.NroDoc;
                        oNovedadOut.UnPrestador.ID = oNovedad.UnPrestador.ID;
                        oNovedadOut.UnConceptoLiquidacion.CodConceptoLiq = oNovedad.UnConceptoLiquidacion.CodConceptoLiq;
                        oNovedadOut.UnConceptoLiquidacion.DescConceptoLiq = oNovedad.UnConceptoLiquidacion.DescConceptoLiq;
                        oNovedadOut.UnTipoConcepto.IdTipoConcepto = oNovedad.UnTipoConcepto.IdTipoConcepto;

                        //Agrego la fila a la tabla
                        oListNovedadesOut.Add(oNovedadOut);
                    }
                }

                return oListNovedadesOut;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #endregion

        #region TIPOS 1 y 6
        //*************************
        //* 	TIpos 1 y 6
        //*************************

        #region Novedades_T1o6_Alta

        #region Novedades_T1o6_Alta_Afiliacion

        private static string Novedades_T1o6_Alta_Afiliacion(long idPrestador, long idBeneficiario, DateTime fecNovedad,
                                                              short tipoConcepto, int conceptoOPP, double impTotal, Single porcentaje,
                                                              string nroComprobante, string ip, string usuario, int mensual, byte idEstadoReg)
        {
            double importe;
            long idNovedad;
            byte codMovimiento = 6;
            byte cantCuotas = 0;
            string mensaje = String.Empty;
            string retorno = string.Empty;
            string mac = String.Empty;
            String[] alta = new String[2];
            List<Novedad> oListNovedades = new List<Novedad>(); ;

            try
            {
                oListNovedades = Novedades_Trae_TCMov(idPrestador, idBeneficiario, conceptoOPP);

                if (oListNovedades.Count != 0 && (oListNovedades[0].UnCodMovimiento.CodMovimiento == 5 ||
                    oListNovedades[0].UnCodMovimiento.CodMovimiento == 6))
                    mensaje = "Solo se puede ingresar una novedad para el concepto ingresado";

                importe = Calc_Importe_1o6(idBeneficiario, tipoConcepto, porcentaje, impTotal);

                if (mensaje == String.Empty)
                {
                    if (tipoConcepto == 6)
                    {
                        mensaje = CtrolTopeXCpto(idPrestador, tipoConcepto, conceptoOPP, porcentaje);
                        impTotal = 0;
                    }
                    else
                    {
                        mensaje = CtrolTopeXCpto(idPrestador, tipoConcepto, conceptoOPP, importe);
                        porcentaje = 0;
                    }
                }

                if (mensaje == String.Empty)
                {
                    mensaje = CtrolAlcanza(idBeneficiario, importe, idPrestador, conceptoOPP);

                }
                if (mensaje == String.Empty && oListNovedades.Count != 0)
                {
                    if (oListNovedades[0].UnCodMovimiento.CodMovimiento == 4)// esta en novedades
                    {
                        idNovedad = oListNovedades[0].IdNovedad;  //long.Parse(ds.Tables[0].Rows[0]["IdNovedad"].ToString());

                        //switch (byte.Parse(ds.Tables[0].Rows[0]["IdEstadoReg"].ToString()))
                        using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                        {
                            switch (oListNovedades[0].UnEstadoReg.IdEstado)
                            {
                                case 1:
                                    codMovimiento = 5;
                                    Novedades_PasaAHist(idNovedad, 0, 7, 3, 0, ip, usuario);
                                    break;
                                case 2:
                                case 3:
                                    Novedades_Modifica_EstadoReg(idNovedad, 12, ip, usuario);
                                    break;
                                case 4:
                                    Novedades_PasaAHist(idNovedad, 0, 7, 3, 0, ip, usuario);
                                    break;
                            }

                            oTransactionScope.Complete();
                        }
                    }
                    //Actualizo el total de novedades cargadas
                }
                if (mensaje == String.Empty)
                {
                    using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        ModificaSaldo(idPrestador, idBeneficiario, conceptoOPP, importe, usuario);

                        //Doy alta la nueva novedad

                        alta = Novedades_Alta_Fisica(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP, impTotal,
                                                     cantCuotas, porcentaje, codMovimiento, nroComprobante, ip, usuario, idEstadoReg);

                        oTransactionScope.Complete();
                    }
                    retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                }
                else
                    retorno = mensaje + "|0|";

                return (retorno);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region Novedades_T1o6_Alta_No_Afiliacion

        private static string Novedades_T1o6_Alta_No_Afiliacion(long idPrestador, long idBeneficiario, DateTime fecNovedad,
                                                         short tipoConcepto, int conceptoOPP, double impTotal, Single porcentaje,
                                                         string nroComprobante, string ip, string usuario, int mensual, byte idEstadoReg)
        {

            string mensaje = String.Empty;
            string retorno;
            DataSet ds = new DataSet();
            double importe;
            byte codMovimiento = 6;
            byte cantCuotas = 0;
            string mac = String.Empty;
            String[] alta = new String[2];

            try
            {

                importe = Calc_Importe_1o6(idBeneficiario, tipoConcepto, porcentaje, impTotal);

                if (mensaje == String.Empty)
                {
                    if (tipoConcepto == 6)
                    {
                        //mensaje = CtrolTopeXCpto(idPrestador, tipoConcepto, conceptoOPP,Porcentaje);
                        impTotal = 0;
                    }
                    else
                    {
                        //mensaje = CtrolTopeXCpto(idPrestador, tipoConcepto, conceptoOPP,Importe);
                        porcentaje = 0;
                    }
                }

                if (mensaje == String.Empty)
                {
                    mensaje = CtrolAlcanza(idBeneficiario, importe, idPrestador, conceptoOPP);

                    #region
                    //09/01/12 - $3b@ SE COMENTA LO SIG PORQUE SE GRABAN TODOS LOS ERRORES
                    //if (mensaje != String.Empty)
                    //{
                    //    using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                    //    {
                    //        NovedadRechazada_Alta(idPrestador, idBeneficiario,
                    //                             fecNovedad, tipoConcepto,
                    //                             conceptoOPP, mac,
                    //                             impTotal, cantCuotas,
                    //                             porcentaje, codMovimiento,
                    //                             nroComprobante, ip,
                    //                             usuario, mensual);

                    //        oTransactionScope.Complete();
                    //    }
                    //}
                    #endregion
                }

                if (mensaje == String.Empty)
                {
                    using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                    {
                        //Actualizo el total de novedades cargadas
                        ModificaSaldo(idPrestador, idBeneficiario, conceptoOPP, importe, usuario);

                        //Doy alta la nueva novedad
                        alta = Novedades_Alta_Fisica(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP, impTotal,
                                                    cantCuotas, porcentaje, codMovimiento, nroComprobante, ip, usuario, idEstadoReg);

                        oTransactionScope.Complete();
                    }
                    retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                }
                else
                {
                    retorno = mensaje + "|0|";
                }
                return (retorno);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                ds.Dispose();
            }
        }
        #endregion

        #endregion

        #region Novedades_T1o6_Baja

        //private static string Novedades_T1o6_Baja(byte idEstadoReg, string ip, string usuario, DataSet NovVieja)
        private static string Novedades_T1o6_Baja(int idEstadoReg, string ip, string usuario, List<Novedad> listNovedades)
        {
            try
            {
                string retorno = String.Empty;
                string mensaje = String.Empty;
                string[] alta = new String[2];
                byte codMovimientoV = listNovedades[0].UnCodMovimiento.CodMovimiento;

                // Solo paso a historico. No genero alta nueva, nov. nunca fue a la liquidacion
                if (codMovimientoV == 4)
                    mensaje = "Novedad Inexistente";
                else
                {
                    int estRegistroV = listNovedades[0].UnEstadoReg.IdEstado;
                    long idNovedadV = listNovedades[0].IdNovedad;
                    short tipoConcepto = listNovedades[0].UnTipoConcepto.IdTipoConcepto;
                    double impTotal = listNovedades[0].ImporteTotal;
                    Single porcentaje = listNovedades[0].Porcentaje;
                    long idPrestador = listNovedades[0].UnPrestador.ID;
                    long idBeneficiario = listNovedades[0].UnBeneficiario.IdBeneficiario;
                    int conceptoOPP = listNovedades[0].UnConceptoLiquidacion.CodConceptoLiq;
                    string nroComprobante = listNovedades[0].Comprobante;
                    Boolean esAfiliacion = listNovedades[0].UnConceptoLiquidacion.EsAfiliacion;

                    //Modificación de saldo
                    double importe = Calc_Importe_1o6(idBeneficiario, tipoConcepto, porcentaje, impTotal);

                    // Preparo el registro de baja segun corresponda.
                    byte codMovimiento = 4;

                    importe = importe * -1;

                    ModificaSaldo(idPrestador, idBeneficiario, conceptoOPP, importe, usuario);

                    if (codMovimientoV == 6 && estRegistroV == 1)
                    {
                        //Novedades_PasaAHist(idNovedadV, string.Empty, 8, 3, 0, ip, usuario);
                        Novedades_PasaAHist(idNovedadV, 0, 8, 3, 0, ip, usuario);
                        alta[0] = "0";
                        alta[1] = string.Empty;
                    }
                    else
                    {
                        //Alguna vez fue a la liquidación
                        switch (codMovimientoV)
                        {
                            //El archivo anterior fue modificado o es alta
                            case 5:
                            case 6:
                                switch (estRegistroV)
                                {
                                    case 1:
                                    case 4:
                                    case 13:
                                        Novedades_PasaAHist(idNovedadV, 0, 8, 3, 0, ip, usuario);
                                        break;
                                    case 2:
                                    case 3:
                                    case 14:
                                    case 15:
                                        Novedades_Modifica_EstadoReg(idNovedadV, 12, ip, usuario);
                                        break;
                                }
                                //Para estas novedades se debe ingresar un nuevo registro para informar la baja a la 
                                //liquidacion

                                DateTime fecNovedad = DateTime.Today;
                                alta = Novedades_Alta_Fisica(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP, impTotal, 0, porcentaje, codMovimiento, nroComprobante, ip, usuario, 1);
                                break;
                        }
                    }
                }
                if (mensaje == String.Empty)
                {
                    retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                }
                else
                {
                    retorno = mensaje + "|0|";
                }
                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region Novedades_T1o6_Modificacion

        private static string Novedades_T1o6_Modificacion(long idNovedadAnt, long idPrestador, long idBeneficiario, short tipoConcepto,
                                                   int conceptoOPP, double impTotalN, Single porcentajeN, string nroComprobanteN,
                                                   string ip, string usuarioN, int mensual, int idEstadoRegV, double impTotalV,
                                                   Single porcentajeV, int codMovimientoV, Boolean masiva, Boolean esAfiliacion)
        {
            try
            {
                string retorno = String.Empty;
                string mensaje = String.Empty;
                byte codMovimientoN;
                byte idEstadoRegN = 1;
                double importeN = 0;
                double importeV = 0;
                DateTime fecNovedad = DateTime.Today;
                double val = 0;
                byte cantCuotas = 0;
                String[] alta = new String[2];
                codMovimientoN = 5;

                if (codMovimientoV == 4)
                    mensaje = "Novedad inexistente";

                if (mensaje == String.Empty)
                {
                    if (esAfiliacion == true)
                    {
                        if (tipoConcepto == 6)
                            mensaje = CtrolTopeXCpto(idPrestador, tipoConcepto, conceptoOPP, porcentajeN);
                        else
                            mensaje = CtrolTopeXCpto(idPrestador, tipoConcepto, conceptoOPP, impTotalN);
                    }
                    else
                        codMovimientoN = 6;
                }

                using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (mensaje == String.Empty)
                    {
                        importeN = Calc_Importe_1o6(idBeneficiario, tipoConcepto, porcentajeN, impTotalN);
                        importeV = Calc_Importe_1o6(idBeneficiario, tipoConcepto, porcentajeV, impTotalV);
                        fecNovedad = DateTime.Today;
                        val = importeN - importeV;
                        cantCuotas = 0;
                        if (val > 0 && masiva == false)
                        {
                            mensaje = CtrolAlcanza(idBeneficiario, val, idPrestador, conceptoOPP);

                            #region
                            //09/01/12 - $3b@ SE COMENTA LO SIG PORQUE SE GRABAN TODOS LOS ERRORES
                            //if (mensaje != String.Empty)
                            //{
                            //    NovedadRechazada_Alta(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP,
                            //                          String.Empty, impTotalN, cantCuotas, porcentajeN, codMovimientoN, nroComprobanteN, ip, usuarioN, mensual);
                            //}
                            #endregion
                        }
                    }
                    if (mensaje == String.Empty)
                    {
                        ModificaSaldo(idPrestador, idBeneficiario, conceptoOPP, val, usuarioN);

                        switch (idEstadoRegV)
                        {
                            case 2:
                            case 3:
                            case 14:
                            case 15:
                                Novedades_Modifica_EstadoReg(idNovedadAnt, 12, ip, usuarioN);
                                break;
                            case 1:
                                if (codMovimientoV == 6)
                                {
                                    codMovimientoN = 6;
                                }
                                Novedades_PasaAHist(idNovedadAnt, mensual, 7, 3, 0, ip, usuarioN);
                                break;
                            case 4:
                            case 13:
                                Novedades_PasaAHist(idNovedadAnt, mensual, 7, 3, 0, ip, usuarioN);
                                break;
                        }

                        alta = Novedades_Alta_Fisica(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP,
                                                     impTotalN, cantCuotas, porcentajeN, codMovimientoN, nroComprobanteN, ip, usuarioN, idEstadoRegN);


                        retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                    }
                    else
                        retorno = mensaje + "|0|";

                    oTransactionScope.Complete();
                }
                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region Novedades_Trae_TCMov

        private static List<Novedad> Novedades_Trae_TCMov(long idPrestador, long idBeneficiario, int conceptoOPP)
        {
            string sql = "Novedades_Trae_TCMov";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad> lstNovedades = new List<Novedad>();
            Novedad unaNovedad;

            try
            {
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@ConceptoLiq", DbType.Int32, conceptoOPP);
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        unaNovedad = new Novedad();

                        unaNovedad.IdNovedad = long.Parse(dr["IdNovedad"].ToString());
                        unaNovedad.FechaNovedad = DateTime.Parse(dr["FecNov"].ToString());
                        unaNovedad.FechaImportacion = dr["FecImportacion"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecImportacion"].ToString());
                        unaNovedad.ImporteCuota = double.Parse(string.IsNullOrEmpty(dr["ImporteCuota"].ToString()) ? "0" : dr["ImporteCuota"].ToString());
                        unaNovedad.ImporteTotal = double.Parse(dr.GetValue("ImporteTotal").ToString());
                        unaNovedad.CantidadCuotas = byte.Parse(dr.GetValue("CantCuotas").ToString());
                        unaNovedad.Porcentaje = Single.Parse(dr["Porcentaje"].ToString());
                        unaNovedad.Comprobante = dr["NroComprobante"].ToString();
                        unaNovedad.MAC = dr["MAC"].ToString();
                        unaNovedad.NroCuotaLiquidada = dr["NroCuota"].Equals(DBNull.Value) ? 0 : int.Parse(dr["NroCuota"].ToString());
                        unaNovedad.MensualCuota = dr["MensualCuota"].Equals(DBNull.Value) ? "" : dr["MensualCuota"].ToString();


                        unaNovedad.UnPrestador.ID = long.Parse(dr["IdPrestador"].ToString());

                        unaNovedad.UnBeneficiario.IdBeneficiario = long.Parse(dr["IdBeneficiario"].ToString());

                        unaNovedad.UnTipoConcepto = new TipoConcepto(Byte.Parse(dr["TipoConcepto"].ToString()), "");

                        unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()), "");

                        unaNovedad.UnEstadoReg.IdEstado = dr["IdEstadoReg"].Equals(DBNull.Value) ? 0 : int.Parse(dr["IdEstadoReg"].ToString());
                        unaNovedad.UnAuditoria.IP = dr.GetString("IP");

                        #region
                        //CODIGO ANTERIOR

                        //unaNovedad = new Novedad(long.Parse(dr["IdNovedad"].ToString()),
                        //                         DateTime.Parse(dr["FecNov"].ToString()),
                        //                         dr["FecImportacion"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecImportacion"].ToString()),
                        //    //double.Parse(dr["ImporteCuota"].ToString()),
                        //                         double.Parse(string.IsNullOrEmpty(dr["ImporteCuota"].ToString()) ? "0" : dr["ImporteCuota"].ToString()),
                        //                         double.Parse(dr.GetValue("ImporteTotal").ToString()),
                        //                         0, 0,
                        //                         byte.Parse(dr.GetValue("CantCuotas").ToString()),
                        //                         Single.Parse(dr["Porcentaje"].ToString()),
                        //                         dr["NroComprobante"].ToString(),
                        //                         "",//dr["PrimerMensual"].ToString(),
                        //                         dr["MensualCuota"].Equals(DBNull.Value) ? "" : dr["MensualCuota"].ToString(), //dr["MensualCuota"].ToString(),
                        //                         0,
                        //                         dr["MAC"].ToString(),
                        //                         false, 0, 0,
                        //                         0,//int.Parse(dr["NroCuota"].ToString()),
                        //                         null);

                        //unaNovedad.UnTipoConcepto = new TipoConcepto(Byte.Parse(dr["TipoConcepto"].ToString()),
                        //                                             dr.GetString("DescTipoConcepto"));

                        //unaNovedad.UnPrestador.ID = long.Parse(dr["IdPrestador"].ToString());
                        //unaNovedad.UnBeneficiario.IdBeneficiario = long.Parse(dr["IdBeneficiario"].ToString());

                        //unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                        //                                                           dr.GetString("DescConceptoLiq"));

                        //unaNovedad.UnEstadoReg.IdEstado = dr["IdEstadoReg"].Equals(DBNull.Value) ? 0: int.Parse(dr["IdEstadoReg"].ToString());
                        //unaNovedad.UnAuditoria.IP = dr.GetString("IP");
                        #endregion

                        lstNovedades.Add(unaNovedad);
                    }
                }

                return lstNovedades;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion

        #region CtrolTopeXCpto

        private static string CtrolTopeXCpto(long idPrestador, short tipoConcepto, int conceptoOPP, double importe)
        {
            //string sql = "Novedades_Trae_TCMov";
            string sql = "CtrolTopeXCpto";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string mensaje = string.Empty;

            try
            {
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Int16, tipoConcepto);
                db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, conceptoOPP);
                db.AddInParameter(dbCommand, "@Importe", DbType.Decimal, importe);
                db.AddOutParameter(dbCommand, "@Alcanza", DbType.Boolean, 1);

                db.ExecuteNonQuery(dbCommand);

                if (!bool.Parse(db.GetParameterValue(dbCommand, "@Alcanza").ToString()))
                    mensaje = "Supera el Máximo permitido para el código de Liquidación " + conceptoOPP.ToString();

                return mensaje;
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
        #endregion

        #region Calc_Importe_1o6

        private static double Calc_Importe_1o6(long idBeneficiario, short tipoConcepto, Single porcentaje, double impTotal)
        {
            double importe = 0;
            List<Beneficiario> oListBeneficiarios = new List<Beneficiario>();

            try
            {
                if (tipoConcepto == 6)
                {
                    oListBeneficiarios = BeneficiarioDAO.Traer(idBeneficiario, string.Empty);
                    importe = (oListBeneficiarios[0].SueldoBruto * porcentaje) / 100;
                }
                else
                    importe = impTotal;

                return importe;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }

        #endregion

        #endregion

        #region TIPO 2
        //***********************
        //*	    Tipo 2
        //***********************

        #region Novedades_T2_Alta

        private static string Novedades_T2_Alta(long idPrestador, long idBeneficiario, DateTime fecNovedad,
                                         short tipoConcepto, int conceptoOPP, double impTotal,
                                         string nroComprobante, string ip, string usuario,
                                         int mensual, byte idEstadoReg)
        {

            try
            {
                string mensaje = String.Empty;
                String[] alta = new String[2];
                byte codMovimiento = 6;
                string retorno;

                mensaje = CtrolAlcanza(idBeneficiario, impTotal, idPrestador, conceptoOPP);

                using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    //09/01/12 - $3b@ SE COMENTA LO SIG PORQUE SE GRABAN TODOS LOS ERRORES
                    //if (mensaje != String.Empty)
                    //{
                    //    NovedadRechazada_Alta(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP, string.Empty,
                    //                          impTotal, 0, 0, codMovimiento, nroComprobante, ip, usuario, mensual);

                    //    oTransactionScope.Complete();
                    //}
                    if (mensaje == String.Empty)
                    {
                        ModificaSaldo(idPrestador, idBeneficiario, conceptoOPP, impTotal, usuario);

                        alta = Novedades_Alta_Fisica(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP, impTotal,
                                                     1, 0, codMovimiento, nroComprobante, ip, usuario, idEstadoReg);

                        retorno = String.Format(" |{0}|{1}", alta[0].ToString(), alta[1].ToString());

                        oTransactionScope.Complete();
                    }
                    else
                        retorno = mensaje + "|0| ";
                }
                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }

        #endregion

        #region  Novedades_T2_Modificacion

        private static string Novedades_T2_Modificacion(long idNovedadAnt, long idPrestador, long idBeneficiario,
                                                 short tipoConcepto, int conceptoOPP, double impTotalN,
                                                 string nroComprobanteN, string ip, string usuarioN,
                                                 int idEstadoRegV, double impTotalV)
        {
            try
            {
                string mensaje = String.Empty;
                string retorno = String.Empty;
                String[] alta = new String[2];
                DateTime fecNovedad = DateTime.Today;
                double importe = 0;
                byte codMovimiento = 5;
                byte idEstadoRegN = 1;

                // busco la novedad a modificar
                if (idEstadoRegV != 1)
                    // para novedades en proceso de liquidación o en transito a la misma
                    mensaje = "Novedad en proceso de liquidación. No puede modificarse";

                using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (mensaje == String.Empty)
                    {
                        // calculo el importe para ver si alcanza el disponible
                        importe = impTotalN - impTotalV;

                        if (importe > 0)
                        {
                            mensaje = CtrolAlcanza(idBeneficiario, importe, idPrestador, conceptoOPP);

                            //09/01/12 - $3b@ SE COMENTA LO SIG PORQUE SE GRABAN TODOS LOS ERRORES
                            //if (mensaje != String.Empty)
                            //{
                            //    NovedadRechazada_Alta(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP, string.Empty,
                            //                          impTotalN, 0, 0, codMovimiento, nroComprobanteN, ip, usuarioN, string.Empty);
                            //}
                        }
                    }
                    if (mensaje == String.Empty)
                    {
                        ModificaSaldo(idPrestador, idBeneficiario, conceptoOPP, importe, usuarioN);
                        Novedades_PasaAHist(idNovedadAnt, 0, 7, 3, 0, ip, usuarioN);
                        alta = Novedades_Alta_Fisica(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP, impTotalN, 1, 0, codMovimiento, nroComprobanteN, ip, usuarioN, idEstadoRegN);
                        retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                    }
                    else
                        retorno = mensaje + "|0| ";

                    oTransactionScope.Complete();
                }

                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region  Novedades_T2_Baja

        //private static string Novedades_T2_Baja(int IdEstadoReg, string ip, string usuario, DataSet NovVieja)
        private static string Novedades_T2_Baja(int idEstadoReg, string ip, string usuario, List<Novedad> listNovedades)
        {
            try
            {
                string mensaje = String.Empty;
                long idNovedadAnt;
                long idPrestador;
                long idBeneficiario;
                short tipoConcepto;
                int conceptoOPP;
                double impTotal;

                if (listNovedades[0].UnEstadoReg.IdEstado != 1)
                    // para novedades en proceso de liquidación o en transito a la misma
                    mensaje = "Novedad en proceso de liquidación. No puede darse de baja";
                else
                {
                    idNovedadAnt = listNovedades[0].IdNovedad;
                    idPrestador = listNovedades[0].UnPrestador.ID;
                    idBeneficiario = listNovedades[0].UnBeneficiario.IdBeneficiario;
                    tipoConcepto = listNovedades[0].UnTipoConcepto.IdTipoConcepto;
                    conceptoOPP = listNovedades[0].UnConceptoLiquidacion.CodConceptoLiq;
                    impTotal = (-1 * listNovedades[0].ImporteTotal);

                    string NroComprobante = listNovedades[0].Comprobante;
                    Boolean EsAfiliacion = listNovedades[0].UnConceptoLiquidacion.EsAfiliacion;

                    ModificaSaldo(idPrestador, idBeneficiario, conceptoOPP, impTotal, usuario);

                    //Novedades_PasaAHist(idNovedadAnt, string.Empty, 9, 3, 0, ip, usuario);
                    Novedades_PasaAHist(idNovedadAnt, 0, 9, 3, 0, ip, usuario);

                }
                return mensaje + "|0| ";
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #endregion

        #region TIPO 3

        //***********************
        //* Tipo 3
        //***********************
        #region Novedades_T3_Alta

        private static string Novedades_T3_Alta(long idPrestador, long idBeneficiario, DateTime fecNovedad, short tipoConcepto,
                                         int conceptoOPP, double impTotal, byte cantCuotas, string nroComprobante,
                                         string ip, string usuario, int mensual, byte idEstadoReg)
        {
            try
            {
                String[] alta = new String[2];
                string mensaje = String.Empty;
                string retorno = String.Empty;
                string mac = string.Empty;
                double importe = 0;
                byte codMovimiento = 6;
                //long IdNovedad = 0;

                using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    //if (mensaje != String.Empty)
                    //{
                    //    NovedadRechazada_Alta(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP,
                    //                          String.Empty, impTotal, cantCuotas, 0, codMovimiento, nroComprobante, ip,
                    //                          usuario, mensual);
                    //}
                    if (mensaje == String.Empty)
                    {
                        ModificaSaldo(idPrestador, idBeneficiario, conceptoOPP, importe, usuario);

                        //alta = Novedades_Alta_Fisica(idPrestador, idBeneficiario, fecNovedad, tipoConcepto,
                        //                             ConceptoOPP, impTotal, cantCuotas,0,CodMovimiento, nroComprobante, ip, usuario,IdEstadoReg);
                        //IdNovedad = long.Parse(alta[0].ToString());
                        //GeneraCuotas (IdNovedad,cantCuotas, Importe, ip, usuario,Mensual);

                        alta = Novedades_Alta_Fisica_Tipo3(idPrestador, idBeneficiario, fecNovedad, tipoConcepto, conceptoOPP, impTotal,
                                                           cantCuotas, 0, codMovimiento, nroComprobante, ip, usuario, idEstadoReg, mensual);

                        retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                    }
                    else
                        retorno = mensaje + "|0| ";

                    oTransactionScope.Complete();
                }

                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }

        #region Alta_Fisica_Novedades_Tipo3

        private static String[] Novedades_Alta_Fisica_Tipo3(long idPrestador, long idBeneficiario, DateTime fecNovedad, short tipoConcepto,
                                                     int codConceptoLiq, double importeTotal, byte cantCuotas, Single porcentaje, byte codMovimiento,
                                                     string nroComprobante, string ip, string usuario, byte idEstadoReg, int mensual)
        {

            string dato = Genera_Datos_para_MAC(idBeneficiario, idPrestador, fecNovedad, codMovimiento, codConceptoLiq, tipoConcepto,
                                                importeTotal, cantCuotas, porcentaje, nroComprobante, ip, usuario);

            string mac = Utilidades.Calculo_MAC(dato);
            string sql = "Novedades_Tipo3_Alta";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string[] retorno = new string[2];

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, codConceptoLiq);
                db.AddInParameter(dbCommand, "@ImporteTotal", DbType.Decimal, importeTotal);
                db.AddInParameter(dbCommand, "@CantCuotas", DbType.Int16, cantCuotas);
                // OJO en un futuro se va a exigir cargar el nro de comprobante 
                db.AddInParameter(dbCommand, "@NroComprobante", DbType.String, nroComprobante);
                db.AddInParameter(dbCommand, "@MAC", DbType.String, mac);
                db.AddInParameter(dbCommand, "@IP", DbType.String, ip);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, usuario);
                db.AddInParameter(dbCommand, "@PrimerMensual", DbType.Int32, mensual);
                db.AddOutParameter(dbCommand, "@IdNovedad", DbType.Int64, 8);

                db.ExecuteNonQuery(dbCommand);

                retorno[0] = db.GetParameterValue(dbCommand, "@IdNovedad").ToString();
                retorno[1] = mac;

                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion

        #region Genera Cuotas

        private static void GeneraCuotas(long idNovedad, byte cantCuotas, double importe, string ip, string usuario, string mensual)
        {
            try
            {
                int mes = int.Parse(mensual.Substring(4, 2));
                int anio = int.Parse(mensual.Substring(0, 4));

                for (byte i = 1; i <= cantCuotas; i++)
                {
                    mensual = anio.ToString() + (mes < 10 ? "0" + mes.ToString() : mes.ToString());
                    if (mes == 12)
                    {
                        anio++;
                        mes = 1;
                    }
                    else
                        mes++;

                    object[] datos = new object[4];
                    datos[0] = idNovedad;
                    datos[1] = i;
                    datos[2] = importe;
                    datos[3] = mensual;

                    // No se generara Hash para las cuotas.
                    // string dato = Utilidades.Armo_String_MAC(datos);
                    // MAC = Utilidades.Calculo_MAC(dato);		

                    AltaCuota(idNovedad, i, importe, ip, usuario, mensual);
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #region AltaCuota

        private static void AltaCuota(long idNovedad, byte nroCuota, double importe, string ip, string usuario, string mensual)
        {
            string sql = "Cuotas_A";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;

            try
            {
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, idNovedad);
                db.AddInParameter(dbCommand, "@NroCuota", DbType.Int16, nroCuota);
                db.AddInParameter(dbCommand, "@Importe", DbType.Decimal, importe);
                db.AddInParameter(dbCommand, "@IP", DbType.String, ip);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, usuario);
                db.AddInParameter(dbCommand, "@Mensual", DbType.Int32, mensual);
                dbParametros = dbCommand.Parameters;

                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion

        #endregion

        #region Novedades_T3_Baja

        //private static string Novedades_T3_Baja(byte idEstadoReg, string ip, string usuario,
        //                                        string mensual, DataSet NovVieja)
        private static string Novedades_T3_Baja(int idEstadoReg, string ip, string usuario, int mensual, Novedad unaNovedad)
        {
            DataSet ds = new DataSet();
            try
            {
                string mensaje = String.Empty;
                //string filtro = "(MensualCuota >= " + mensual.ToString() + ")";

                //NovVieja.Tables[0].DefaultView.RowFilter = filtro;
                //NovVieja.Tables[0].DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
                //DataView dvNovViejas = NovVieja.Tables[0].DefaultView;

                List<Cuota> lstCuotas = new List<Cuota>();
                lstCuotas = unaNovedad.unaLista_Cuotas.FindAll(delegate(Cuota C)
                                                    {
                                                        if (int.Parse(C.Mensual_Cuota) >= mensual)
                                                        {
                                                            return true;
                                                        }
                                                        else
                                                        {
                                                            return false;
                                                        }
                                                    });

                // if (listNovedades.Count != 1 || (listNovedades[0].PrimerMensual != mensual))
                // string elmensual = mensual.Substring(4,2) + "-" + mensual.Substring(0,4);
                //if (lstCuotas.Count != 1 || (unaNovedad.PrimerMensual != elmensual))
                if (lstCuotas.Count != 1 || lstCuotas[0].Mensual_Cuota != mensual.ToString())
                    mensaje = "Sólo se puede dar de baja a partir de la última cuota y en forma descendente.";

                // if (NovVieja.Tables[0].Rows.Count != 1 || (int.Parse(NovVieja.Tables[0].Rows[0]["MensualCuota"].ToString())!= Mensual))
                //     mensaje = "Sólo se puede dar de baja la última cuota sin liquidar";

                if (mensaje == string.Empty)
                {
                    //if (listNovedades[0].UnEstadoReg.IdEstado != 1)
                    if (unaNovedad.UnEstadoReg.IdEstado != 1)
                        // para novedades en proceso de liquidación o en transito a la misma
                        mensaje = "Novedad en proceso de liquidación. No puede darse de baja";
                }
                if (mensaje == string.Empty)
                {
                    Cierre oCierre = CierreDAO.TraerFechaCierreProx();

                    string mensualAct = oCierre.Mensual;
                    //long idNovedad = listNovedades[0].IdNovedad; //long.Parse(dvNovViejas[0]["IdNovedad"].ToString());
                    long idNovedad = unaNovedad.IdNovedad;

                    if (mensual == int.Parse(mensualAct))
                    {
                        //long idPrestador = listNovedades[0].UnPrestador.ID; //long.Parse(dvNovViejas[0]["IdPrestador"].ToString());
                        //long idBeneficiario = listNovedades[0].UnBeneficiario.IdBeneficiario; //long.Parse(dvNovViejas[0]["IdBeneficiario"].ToString());
                        //int conceptoOPP = listNovedades[0].UnConceptoLiquidacion.CodConceptoLiq; //int.Parse(dvNovViejas[0]["CodConceptoLiq"].ToString());

                        long idPrestador = unaNovedad.UnPrestador.ID; //long.Parse(dvNovViejas[0]["IdPrestador"].ToString());
                        long idBeneficiario = unaNovedad.UnBeneficiario.IdBeneficiario; //long.Parse(dvNovViejas[0]["IdBeneficiario"].ToString());
                        int conceptoOPP = unaNovedad.UnConceptoLiquidacion.CodConceptoLiq; //int.Parse(dvNovViejas[0]["CodConceptoLiq"].ToString());

                        double importe = unaNovedad.ImporteCuota * -1; //double.Parse(dvNovViejas[0]["ImporteCuota"].ToString()) * -1;

                        ModificaSaldo(idPrestador, idBeneficiario, conceptoOPP, importe, usuario);
                    }

                    //Novedades_PasaAHist(IdNovedad, mensual, 7, 3, 0, ip, Usuario);					
                    Novedades_PasaAHist(idNovedad, mensual, idEstadoReg, 3, 0, ip, usuario);					//Modificado por COK 09.08.2005

                }
                return mensaje + "|0| ";
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }

        #endregion

        #endregion

        #region ABM Sobre BD

        #region Novedades_Alta_Fisica

        private static String[] Novedades_Alta_Fisica(long idPrestador, long idBeneficiario, DateTime fecNovedad,
                                               short tipoConcepto, int codConceptoLiq, double importeTotal, byte cantCuotas, Single porcentaje,
                                               byte codMovimiento, string nroComprobante, string ip, string usuario, byte idEstadoReg)
        {

            string dato = Genera_Datos_para_MAC(idBeneficiario, idPrestador, fecNovedad, codMovimiento, codConceptoLiq, tipoConcepto,
                                                importeTotal, cantCuotas, porcentaje, nroComprobante, ip, usuario);

            string mac = Utilidades.Calculo_MAC(dato);

            string sql = "Novedades_A";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string[] retorno = new string[2];

            try
            {

                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@FecNovedad", DbType.DateTime, fecNovedad);
                db.AddInParameter(dbCommand, "@CodMovimiento", DbType.Int16, codMovimiento);
                db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Int16, tipoConcepto);
                db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, codConceptoLiq);
                db.AddInParameter(dbCommand, "@ImporteTotal", DbType.Decimal, importeTotal);
                db.AddInParameter(dbCommand, "@CantCuotas", DbType.Int16, cantCuotas);
                db.AddInParameter(dbCommand, "@Porcentaje", DbType.Decimal, porcentaje);
                db.AddInParameter(dbCommand, "@MAC", DbType.String, mac);
                // OJO en un futuro se va a exigir cargar el nro de comprobante
                db.AddInParameter(dbCommand, "@NroComprobante", DbType.String, nroComprobante);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, usuario);
                db.AddInParameter(dbCommand, "@IP", DbType.String, ip);
                db.AddInParameter(dbCommand, "@IdEstadoReg", DbType.Int16, idEstadoReg);
                db.AddOutParameter(dbCommand, "@IdNovedad", DbType.Int64, 8);

                db.ExecuteNonQuery(dbCommand);

                retorno[0] = db.GetParameterValue(dbCommand, "@IdNovedad").ToString();
                retorno[1] = mac;

                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion

        #region NovedadRechazada_Alta

        private static void NovedadRechazada_Alta(long idPrestador, long idBeneficiario, DateTime fecNovedad,
                                            short tipoConcepto, int codConceptoLiq, string mac, double importeTotal,
                                            byte cantCuotas, Single porcentaje, byte codMovimiento,
                                            string nroComprobante, string ip, string usuario, int mensual, string mensajeError)
        {
            string sql = "Novedades_Rechazadas_A";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@CodMovimiento", DbType.Int16, codMovimiento);
                db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Int16, tipoConcepto);
                db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, codConceptoLiq);
                db.AddInParameter(dbCommand, "@ImporteTotal", DbType.Decimal, importeTotal);
                db.AddInParameter(dbCommand, "@CantCuotas", DbType.Int16, cantCuotas);
                db.AddInParameter(dbCommand, "@Porcentaje", DbType.Decimal, porcentaje);
                db.AddInParameter(dbCommand, "@NroComprobante", DbType.String, nroComprobante);
                db.AddInParameter(dbCommand, "@IP", DbType.String, ip);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, usuario);
                db.AddInParameter(dbCommand, "@FecRechazo", DbType.DateTime, DateTime.Today);
                db.AddInParameter(dbCommand, "@TipoRechazo", DbType.String, mensajeError);

                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion

        #region Novedades_Modifica_EstadoReg

        private static void Novedades_Modifica_EstadoReg(long idNovedad, byte idEstadoReg, string ip, string usuario)
        {
            string sql = "Novedades_M";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, idNovedad);
                db.AddInParameter(dbCommand, "@IdEstadoReg", DbType.Int16, idEstadoReg);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, usuario);
                db.AddInParameter(dbCommand, "@IP", DbType.String, ip);

                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion

        #region Novedades_PasaAHist

        private static void Novedades_PasaAHist(long idNovedad, int mensual, int idEstadoReg, byte idEstadoNov,
                                                double importeLiq, string ip, string usuario)
        {
            string sql = "Novedades_PaHist";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, idNovedad);
                db.AddInParameter(dbCommand, "@Mensual", DbType.Int32, mensual);
                db.AddInParameter(dbCommand, "@IdEstadoReg", DbType.Int16, idEstadoReg);
                db.AddInParameter(dbCommand, "@IdEstadoNov", DbType.Int16, idEstadoNov);
                db.AddInParameter(dbCommand, "@ImporteLiq", DbType.Decimal, importeLiq);
                db.AddInParameter(dbCommand, "@IP", DbType.String, ip);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, usuario);

                db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }

        #endregion

        #region ModificaSaldo

        private static long ModificaSaldo(long idPrestador, long idBeneficiario, int codConceptoLiq, double importe, string usuario)
        {
            string sql = "Beneficiarios_MSaldo";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, codConceptoLiq);
                db.AddInParameter(dbCommand, "@Importe", DbType.Decimal, importe);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, usuario);

                db.ExecuteNonQuery(dbCommand);

                return 0;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion

        #endregion ABM Sobre BD

        #region Genera datos para MAC

        private static string Genera_Datos_para_MAC(long idBeneficiario, long idPrestador, DateTime fecNovedad,
                                             byte codMovimiento, int conceptoOPP, short tipoConcepto,
                                             double impTotal, byte cantCuotas, Single porcentaje,
                                             string nroComprobante, string ip, string usuario)
        {
            //object[] datos = new object[12];
            //datos[0] = idBeneficiario;
            //datos[1] = idPrestador;
            //datos[2] = fecNovedad;
            //datos[3] = codMovimiento;
            //datos[4] = conceptoOPP;
            //datos[5] = tipoConcepto;
            //datos[6] = impTotal;
            //datos[7] = cantCuotas;
            //datos[8] = porcentaje;
            //datos[9] = nroComprobante;
            //datos[10] = ip;
            //datos[11] = usuario;
            //return (Utilidades.Armo_String_MAC(datos));

            StringBuilder sDatosMac = new StringBuilder();

            sDatosMac.Append(idBeneficiario);
            sDatosMac.Append(idPrestador);
            sDatosMac.Append(fecNovedad);
            sDatosMac.Append(codMovimiento);
            sDatosMac.Append(conceptoOPP);
            sDatosMac.Append(tipoConcepto);
            sDatosMac.Append(impTotal);
            sDatosMac.Append(cantCuotas);
            sDatosMac.Append(porcentaje);
            sDatosMac.Append(nroComprobante);
            sDatosMac.Append(ip);
            sDatosMac.Append(usuario);

            return sDatosMac.ToString();
        }
        #endregion

        #region Controles - Validaciones Comunes a todos los tipos

        #region ConceptoOPP_Habil_X_Prest

        private static string ConceptoOPP_Habil_X_Prest(long idPrestador, int conceptoOPP, short tipoConcepto)
        {
            string sql = "ConceptoOPP_Habil_X_Prest";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            string mensaje;
            //bool ok;

            try
            {
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@ConceptoOPP", DbType.Int32, conceptoOPP);
                db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Int16, tipoConcepto);
                db.AddOutParameter(dbCommand, "@OK", DbType.Boolean, 1);
                db.AddOutParameter(dbCommand, "@EsAfiliacion", DbType.Boolean, 1);
                dbParametros = dbCommand.Parameters;

                db.ExecuteNonQuery(dbCommand);

                if (bool.Parse(db.GetParameterValue(dbCommand, "@OK").ToString()))
                    mensaje = "|" + db.GetParameterValue(dbCommand, "@EsAfiliacion").ToString();
                else
                    mensaje = "Concepto Liq - Tipo Concepto no habilitada para el Prestador|False";

                return mensaje;
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

        #endregion

        #region Ctrol de Duplicados

        private static string CtrolDuplicados(long idPrestador, long idBeneficiario, short tipoConcepto,
                                       int conceptoOPP, double impTotal, byte cantCuotas,
                                       Single porcentaje, string nroComprobante)
        {

            string sql = "Novedades_Existe";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            //bool existe;

            try
            {
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Int16, tipoConcepto);
                db.AddInParameter(dbCommand, "@ConceptoOPP", DbType.Int32, conceptoOPP);
                db.AddInParameter(dbCommand, "@ImpTotal", DbType.Int64, impTotal);
                db.AddInParameter(dbCommand, "@CantCuotas", DbType.Int16, cantCuotas);
                db.AddInParameter(dbCommand, "@Porcentaje", DbType.Int16, porcentaje);
                db.AddInParameter(dbCommand, "@NroComprobante", DbType.String, nroComprobante);
                db.AddOutParameter(dbCommand, "@Existe", DbType.Boolean, 1);
                dbParametros = dbCommand.Parameters;

                db.ExecuteNonQuery(dbCommand);

                //if (bool.Parse(db.GetParameterValue(dbCommand, "@Existe").ToString()))
                //    return "Ud. está intentando re-ingresar una novedad ya existente. Proceso cancelado";

                //return String.Empty;

                return bool.Parse(db.GetParameterValue(dbCommand, "@Existe").ToString()) ? "Ud. está intentando re-ingresar una novedad ya existente. Proceso cancelado" : String.Empty;
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
        #endregion

        #region CtrolBloqueado

        private static string CtrolBloqueado(long idBeneficiario)
        {
            string sql = "Beneficios_Bloqueados_Busca";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            //bool existe;

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                db.AddOutParameter(dbCommand, "@Existe", DbType.Boolean, 1);
                dbParametros = dbCommand.Parameters;

                db.ExecuteNonQuery(dbCommand);

                if (bool.Parse(db.GetParameterValue(dbCommand, "@Existe").ToString()))
                    return "Beneficio Bloqueado.";
                return String.Empty;
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
        #endregion

        #region CtrolInhibido

        private static string CtrolInhibido(long idPrestador, long idBeneficiario, int conceptoOPP)
        {
            string sql = "Beneficios_Inhibido_Busca";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            //bool existe;

            try
            {
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@ConceptoLiq", DbType.Int32, conceptoOPP);
                db.AddOutParameter(dbCommand, "@Existe", DbType.Boolean, 1);
                dbParametros = dbCommand.Parameters;

                db.ExecuteNonQuery(dbCommand);

                if (bool.Parse(db.GetParameterValue(dbCommand, "@Existe").ToString()))
                    return String.Concat("Código inhibido para el Beneficio: ", idBeneficiario.ToString());
                return String.Empty;
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
        #endregion

        #region CtrolOcurrencias

        private static string CtrolOcurrencias(byte cantOcurrDisp, long idBeneficiario, int conceptoOPP)
        {
            string sql = "Novedades_Alcanza_Ocurrencia";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@ConceptoLiq", DbType.Int32, conceptoOPP);
                db.AddInParameter(dbCommand, "@CantOcurrDisp", DbType.Int16, cantOcurrDisp);
                db.AddOutParameter(dbCommand, "@Alcanza", DbType.Boolean, 1);
                dbParametros = dbCommand.Parameters;

                db.ExecuteNonQuery(dbCommand);

                //if (bool.Parse(db.GetParameterValue(dbCommand, "@Alcanza").ToString()))
                //    return "La Liquidación no posee lugar para ingresar un nuevo descuento.";
                //return String.Empty;

                return bool.Parse(db.GetParameterValue(dbCommand, "@Alcanza").ToString()) ? string.Empty : "La Liquidación no posee lugar para ingresar un nuevo descuento.";


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
        #endregion

        #region CtrolOcurrenciasCancCuotas

        public static Boolean CtrolOcurrenciasCancCuotas(byte cantOcurrDisp, long idBeneficiario, int codConceptoLiq, long idNovedadABorrar)
        {
            string sql = "Novedades_Alcanza_Ocurrencia_Xa_CancCuotas";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@ConceptoLiq", DbType.Int32, codConceptoLiq);
                db.AddInParameter(dbCommand, "@CantOcurrDisp", DbType.Int16, cantOcurrDisp);
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, idNovedadABorrar);
                db.AddOutParameter(dbCommand, "@Alcanza", DbType.Boolean, 1);
                dbParametros = dbCommand.Parameters;

                db.ExecuteNonQuery(dbCommand);

                return bool.Parse(db.GetParameterValue(dbCommand, "@Alcanza").ToString());
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
        #endregion

        #region CtrolRestricciones

        private static string CtrolRestricciones(long idPrestador, long idBeneficiario, int codConceptoLiq)
        {
            string sql = "RestriccionesXCodConceptoLiq_TxExCaja";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            //bool existe;

            try
            {
                db.AddInParameter(dbCommand, "@ConceptoLiq", DbType.Int32, codConceptoLiq);
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@ExCaja", DbType.Int16, idBeneficiario.ToString("00000000000").Substring(0, 2));
                db.AddOutParameter(dbCommand, "@Ok", DbType.Boolean, 1);
                dbParametros = dbCommand.Parameters;

                db.ExecuteNonQuery(dbCommand);

                //if (bool.Parse(db.GetParameterValue(dbCommand, "@Alcanza").ToString()))
                //    return "Beneficio restringido para éste concepto.";
                //return String.Empty;

                return bool.Parse(db.GetParameterValue(dbCommand, "@Ok").ToString()) ? String.Empty : "Beneficio restringido para éste concepto";

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
        #endregion

        
        #region CtrolPuedeAltaNovXTipo

        private static string CtrolPuedeAltaNovXTipo(long idPrestador, short tipoConcepto, int conceptoOPP, long idBeneficiario, Boolean esAfiliacion)
        {
            // * Verifica que segun los tipo de concepto de las novedades cargadas se pueda cargar nueva novedad.
            string sql = "Novedades_CtrolPuedeAltaXTipo";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            string mensaje = string.Empty;

            try
            {
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Int16, tipoConcepto);
                db.AddInParameter(dbCommand, "@ConceptoOPP", DbType.Int32, conceptoOPP);
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@EsAfiliacion", DbType.Boolean, esAfiliacion);
                db.AddOutParameter(dbCommand, "@OK", DbType.Boolean, 1);
                dbParametros = dbCommand.Parameters;

                db.ExecuteNonQuery(dbCommand);

                if (bool.Parse(db.GetParameterValue(dbCommand, "@OK").ToString()))
                {
                    //return "Beneficio restringido para éste concepto.";
                    mensaje = string.Empty;
                }
                else
                {
                    if (bool.Parse(db.GetParameterValue(dbCommand, "@EsAfiliacion").ToString()))
                    {
                        mensaje = "Solo se puede cargar una novedad de Afiliación";
                    }
                    else
                    {
                        if (tipoConcepto == 6)
                            mensaje = "Existen novedades con monto fijo. No se puede cargar novedades con monto fijo y con porcentaje";
                        else
                            mensaje = "Existen novedades con porcentaje. No se puede cargar novedades con monto fijo y con porcentaje";
                    }
                }

                return mensaje;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }

        #endregion

        #region CtrolMontos todo Comentado
        //private static string CtrolMontos(byte TipoConcepto, double impTotal, byte cantCuotas, Single porcentaje)
        //{
        //    string mensajeMontos = String.Empty;
        //    switch (tipoConcepto)
        //    {
        //        case 1:
        //        case 2:
        //            //mensaje = (ImpTotal <= Double.Parse( "0" ) ) ? "El campo Importe debe ser mayor a 0":String.Empty;
        //            if (ImpTotal <= 0) { mensajeMontos = @"El campo Importe debe ser mayor a 0"; }
        //            break;

        //        case 3:
        //            //mensaje = (ImpTotal <= 0) ? "El campo Importe debe ser mayor a 0":String.Empty;
        //            if (ImpTotal <= 0) { mensajeMontos = @"El campo Importe debe ser mayor a 0"; }

        //            if (mensajeMontos == String.Empty)
        //            {
        //                //mensaje = (CantCuotas <= 0 || CantCuotas > 255) ? "El campo Cant. Cuotas debe estar comprendido entre 1 y 255":String.Empty;
        //                //mensaje = (CantCuotas <= 0 || CantCuotas > 120) ? "El campo Cant. Cuotas debe estar comprendido entre 1 y 120":String.Empty;
        //                if (CantCuotas <= 0 || CantCuotas > 240) { mensajeMontos = @"El campo Cant. Cuotas debe estar comprendido entre 1 y 240"; }
        //            }
        //            break;
        //        //case 4:
        //        //            mensaje = Val_Importe(_ImpTotal,"Importe Total",0);
        //        //            if (mensaje == null ||mensaje == String.Empty)
        //        //            {
        //        //                mensaje = Val_Importe(Porcentaje,"Porcentaje",1);							
        //        //            }
        //        //            break;
        //        //        case 5:
        //        //             mensaje = Val_Importe(Porcentaje,"Porcentaje",1);							
        //        //            break;
        //        case 6:
        //            //mensaje = (Porcentaje <= 0 || porcentaje > 100) ? "El campo porcentaje debe ser mayor que 0 y menor a 100":String.Empty;				
        //            if (Porcentaje <= 0 || porcentaje > 100) { mensajeMontos = @"El campo porcentaje debe ser mayor que 0 y menor a 100"; }
        //            break;

        //        default:
        //            mensajeMontos = @"Opción no contemplada";
        //            break;
        //    }
        //    return mensajeMontos;
        //}
        //#endregion



        //public static string CtrolAlcanza(long idBeneficiario, double Importe, long idPrestador, int conceptoOPP)
        //{
        //    // controla si alcanza el monto a ingresar - si no alcanza ingresa el monto en rechazados		
        //    string mensaje = String.Empty;

        //    Conexion objCnn = new Conexion();

        //    try
        //    {
        //        SqlParameter[] objPar = new SqlParameter[5];

        //        objPar[0] = new SqlParameter("@IdBeneficiario", SqlDbType.BigInt);
        //        objPar[0].Value = idBeneficiario;

        //        objPar[1] = new SqlParameter("@monto", SqlDbType.Decimal);
        //        objPar[1].Value = Importe;

        //        objPar[2] = new SqlParameter("@IdPrestador", SqlDbType.BigInt);
        //        objPar[2].Value = idPrestador;

        //        objPar[3] = new SqlParameter("@ConceptoOPP", SqlDbType.Int);
        //        objPar[3].Value = ConceptoOPP;

        //        objPar[4] = new SqlParameter("@alcanza", SqlDbType.TinyInt);
        //        objPar[4].Direction = ParameterDirection.InputOutput;
        //        objPar[4].Value = 0;

        //        SqlHelper.ExecuteNonQuery(objCnn.Conectar(), CommandType.StoredProcedure, "AlcanzaAfectacion", objPar);


        //        if ((Byte)objPar[4].Value == 0)
        //        {
        //            mensaje = "Afectación Disponible Insuficiente";
        //        }
        //        else
        //        {
        //            mensaje = String.Empty;
        //        }
        //        return mensaje;

        //    }
        //    catch (Exception err)
        //    {
        //        throw err;
        //    }
        //    finally
        //    {
        //        objCnn = null;
        //    }

        //}



        //#region CtrolCantRechazos
        //private static int CtrolCantRechazos(long idPrestador, long idBeneficiario)
        //{

        //    Conexion objCnn = new Conexion();

        //    try
        //    {

        //        SqlParameter[] objPar = new SqlParameter[3];


        //        objPar[0] = new SqlParameter("@IdBeneficiario", SqlDbType.BigInt);
        //        objPar[0].Value = idBeneficiario;

        //        objPar[1] = new SqlParameter("@IdPrestador", SqlDbType.BigInt);
        //        objPar[1].Value = idPrestador;

        //        objPar[2] = new SqlParameter("@CantRech", SqlDbType.TinyInt);
        //        objPar[2].Direction = ParameterDirection.Output;
        //        objPar[2].Value = 0;

        //        SqlHelper.ExecuteNonQuery(objCnn.Conectar(), CommandType.StoredProcedure, "NovRechazadas_TCant", objPar);


        //        return (int.Parse(objPar[2].Value.ToString()));

        //    }
        //    catch (Exception err)
        //    {
        //        throw err;
        //    }
        //    finally
        //    {

        //        objCnn = null;
        //    }

        //}
        //#endregion

        //#region CtrolIntentos

        //private static string CtrolIntentos(long idPrestador, long idBeneficiario)
        //{
        //    string mensaje = String.Empty;
        //    try
        //    {
        //        int MaxIntentos = int.Parse(ConfigurationSettings.AppSettings["DAT_MaxIntentos"].ToString());
        //        int MaxCantRechazos = CtrolCantRechazos(idPrestador, idBeneficiario);

        //        mensaje = (MaxCantRechazos >= MaxIntentos) ? "Máxima cantidad de intentos permitidos" : String.Empty;
        //        return (mensaje);
        //    }
        //    catch (Exception err)
        //    {
        //        throw err;
        //    }
        //}
        #endregion

        #region Valido_Nov viejo

        //validacion en com+ - se elimina y se pasa al store
        //    #region Valido_Nov
        //    private static string Valido_Nov(long idPrestador, long idBeneficiario, short tipoConcepto, 
        //        int conceptoOPP, double impTotal, byte cantCuotas,Single porcentaje, byte codMovimiento, String NroComprobante)
        //    {
        //        string mensaje = String.Empty;
        //        DataSet ds = new DataSet();
        //        Beneficiarios benef	= new Beneficiarios();						
        //        try
        //        {
        //            // CONTROLA MAXIMOS INTENTOS 
        //            mensaje = CtrolIntentos(idPrestador,IdBeneficiario);
        //            // CONTROLA EXISTENCIA DEL BENEFICIARIO 
        //            if (mensaje == String.Empty)
        //            {
        //                ds = benef.Traer(IdBeneficiario.ToString(),String.Empty);
        //                if ((ds.Tables[0].Rows.Count== 0) || (long.Parse(ds.Tables[0].Rows[0]["IdBeneficiario"].ToString()) == 0))
        //                {
        //                    mensaje =  "No se encontraron datos para el Nro. de beneficio.";
        //                }				
        //            }							
        //            //CONTROLA SI YA EXISTE EL REGISTRO
        //            if (mensaje ==String.Empty)
        //            {
        //                mensaje= CtrolDuplicados( idPrestador,idBeneficiario,TipoConcepto, ConceptoOPP , impTotal, cantCuotas, porcentaje, nroComprobante); );
        //            }
        //            // CONTROLA QUE ESTE INFORMADO EL COMPROBANTE
        //            if ((mensaje ==String.Empty) && (NroComprobante.Trim().Length < 4))
        //            {
        //                mensaje = "El nro. de comprobante debe ser mayor a 3 dígitos.";
        //            }
        //            // CONTROLA  BENEFICIO BLOQUEADO
        //            if (mensaje ==String.Empty)
        //            {
        //                mensaje= CtrolBloqueado(IdBeneficiario);
        //            }
        //            // CONTROLA INHIBIDO
        //            if (mensaje ==String.Empty)
        //            {
        //                mensaje= CtrolInhibido(idPrestador,idBeneficiario,ConceptoOPP);
        //            }
        //            // CONTROLA TIPOS DE CAMPOS
        //            if (mensaje ==String.Empty)
        //            {	
        //                mensaje = CtrolMontos(TipoConcepto,impTotal,cantCuotas,Porcentaje);
        //            }
        //            //CONTROLES PARA ALTA Y MODIFICACION. NO BAJA
        //            if (mensaje==String.Empty && CodMovimiento != 4 )
        //            {																										
        //                //CONTROLA SI ALCANZAN LAS OCURRENCIAS PARA INGRESAR UNA MAS
        //                mensaje= CtrolOcurrencias (byte.Parse(ds.Tables[0].Rows[0]["CantOcurrenciasDisp"].ToString()),idBeneficiario,ConceptoOPP);
        //                if (mensaje == String.Empty)
        //                {
        //                    //CONTROLA SI SE MIRA SI CUMPLE CON RESTRICCIONES POR EXCAJA. 
        //                    mensaje= CtrolRestricciones(idPrestador,idBeneficiario,ConceptoOPP);
        //                }
        //            }
        //            return mensaje;
        //        }
        //        catch( Exception err)
        //        {
        //            throw err ;
        //        }
        //        finally
        //        {
        //            ds.Dispose();		
        //            benef.Dispose();
        //        }
        //    }
        //#endregion

        #endregion

        #endregion Controles - Validaciones Comunes a todos los tipos

        #endregion Transaccional

        #region *** No Transaccional

        #region Novedades_TraerTodos

        public static List<Novedad> Novedades_TraerTodos(long idPrestador)
        {
            string sql = "Novedades_TT";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad> lstNovedades = new List<Novedad>();
            Novedad unaNovedad;

            try
            {
                db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, idPrestador);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        //Int64.Parse(dr["IdNovedad"].ToString()),
                        //Int64.Parse(dr["IdBeneficiario"].ToString()),
                        //dr["ApellidoNombre"].ToString(),
                        //Int64.Parse(dr["IdPrestador"].ToString()),
                        //dr["RazonSocial"].ToString(),

                        //DateTime.Parse(dr["FecNov"].ToString()),

                        //byte.Parse(dr["CodMovimiento"].ToString()),
                        //dr["DescMovimiento"].ToString(),
                        //byte.Parse(dr["TipoConcepto"].ToString()),
                        //dr["DescTipoConcepto"].ToString(),

                        //int.Parse(dr["CodConceptoLiq"].ToString()),
                        //dr["DescConceptoLiq"].ToString(),
                        //decimal.Parse(dr["ImporteTotal"].ToString()),
                        //byte.Parse(dr["CantCuotas"].ToString()),
                        //decimal.Parse(dr["Porcentaje"].ToString()),
                        //dr["NroComprobante"].Equals(DBNull.Value) ? "" : dr["NroComprobante"].ToString(),
                        //
                        //dr["IdEstadoReg"].Equals(DBNull.Value) ? 0 : byte.Parse(dr["IdEstadoReg"].ToString()),
                        //dr["DescripcionEstadoReg"].Equals(DBNull.Value) ? "" : dr["DescripcionEstadoReg"].ToString(),

                        unaNovedad = new Novedad(long.Parse(dr["IdNovedad"].ToString()),
                                                 DateTime.Parse(dr["FecNov"].ToString()),
                                                 double.Parse(dr.GetValue("ImporteTotal").ToString()),
                                                 byte.Parse(dr.GetValue("CantCuotas").ToString()),
                                                 Single.Parse(dr["Porcentaje"].ToString()),
                                                 dr["NroComprobante"].ToString(),
                                                 string.Empty, null,
                                                 dr["PrimerMensual"].ToString(),
                                                 false, null
                                                 );

                        unaNovedad.MAC = dr.GetString("MAC");
                        unaNovedad.FechaImportacion = dr.GetNullableDateTime("FecImportacion");

                        unaNovedad.UnTipoConcepto = new TipoConcepto(Byte.Parse(dr["TipoConcepto"].ToString()),
                                                                     dr.GetString("DescTipoConcepto"));
                        unaNovedad.UnPrestador = new Prestador(long.Parse(dr["IdPrestador"].ToString()),
                                                               dr.GetString("RazonSocial"),
                                                               0);
                        unaNovedad.UnBeneficiario = new Beneficiario(long.Parse(dr["IdBeneficiario"].ToString()), 0,
                                                                     dr.GetString("ApellidoNombre"));
                        unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                                   dr.GetString("DescConceptoLiq"));
                        unaNovedad.UnEstadoReg = new Estado(int.Parse(dr["IdEstadoReg"].ToString()),
                                                            dr.GetString("DescripcionEstadoReg"));
                        unaNovedad.UnAuditoria.FecUltimaModificacion = DateTime.Parse(dr["FecUltModificacion"].ToString());

                        lstNovedades.Add(unaNovedad);
                    }
                }

                return lstNovedades;
            }
            catch (SqlException ErrSQL)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));

                if (ErrSQL.Number == 1205 || ErrSQL.Number == 1204)
                    throw new ApplicationException("Interbloqueo");
                else
                    throw ErrSQL;
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

        #endregion

        #region Novedades_Traer

        public static List<Novedad> Novedades_TraerConsulta(byte opcion, long idPrestador, long benefCuil,
                                                            byte tipoConc, int concopp, int mensual,
                                                            DateTime? fdesde, DateTime? fhasta)
        {
            string sql = "Novedades_TT_SinMigrar_V2";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad> lstNovedades = new List<Novedad>();

            try
            {
                db.AddInParameter(dbCommand, "@Opcion", DbType.Int16, opcion);
                db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@BenefCuil", DbType.Int64, benefCuil);
                db.AddInParameter(dbCommand, "@TipoConc", DbType.Int16, tipoConc);
                db.AddInParameter(dbCommand, "@Conc", DbType.Int32, concopp);
                db.AddInParameter(dbCommand, "@Mensual", DbType.Int32, mensual);
                db.AddInParameter(dbCommand, "@desde", DbType.String, !fdesde.HasValue ? null : fdesde.Value.ToString("yyyyMMdd"));
                db.AddInParameter(dbCommand, "@hasta", DbType.String, !fhasta.HasValue ? null : fhasta.Value.ToString("yyyyMMdd"));
                //db.AddInParameter(dbCommand, "@desde", DbType.String, !fdesde.HasValue ? null : fdesde.Value.ToShortDateString());
                //db.AddInParameter(dbCommand, "@hasta", DbType.String, !fhasta.HasValue ? null : fhasta.Value.ToShortDateString());

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        //Int64.Parse(dr["IdNovedad"].ToString()),
                        //Int64.Parse(dr["IdBeneficiario"].ToString()),
                        //dr["Cuil"].Equals(DBNull.Value) ? "" : dr["Cuil"].ToString(),
                        //dr["ApellidoNombre"].ToString(),
                        //DateTime.Parse(dr["FecNov"].ToString()),

                        //byte.Parse(dr["TipoConcepto"].ToString()),
                        //dr["DescTipoConcepto"].ToString(),

                        //int.Parse(dr["CodConceptoLiq"].ToString()),
                        //dr["DescConceptoLiq"].ToString(),

                        //decimal.Parse(dr["ImporteTotal"].ToString()),
                        //byte.Parse(dr["CantCuotas"].ToString()),
                        //decimal.Parse(dr["Porcentaje"].ToString()),
                        //dr["Importe"].Equals(DBNull.Value) ? 0 : decimal.Parse(dr["Importe"].ToString()),
                        //dr["NroCuota"].Equals(DBNull.Value) ? 0 : byte.Parse(dr["NroCuota"].ToString()),

                        //dr["NroComprobante"].Equals(DBNull.Value) ? "" : dr["NroComprobante"].ToString(),
                        //dr["MAC"].ToString(),
                        //dr["Usuario"].ToString(),

                        lstNovedades.Add(new Novedad(Int64.Parse(dr["IdNovedad"].ToString()),
                                                    dr["FecNov"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecNov"].ToString()),
                                                    null,//dr["FecImportacion"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecImportacion"].ToString()),
                                                    dr["ImporteCuota"].Equals(DBNull.Value) ? 0 : double.Parse(dr["ImporteCuota"].ToString()),
                                                    double.Parse(dr["ImporteTotal"].ToString()),
                                                    0, 0,
                                                    byte.Parse(dr["CantCuotas"].ToString()),
                                                    float.Parse(dr["Porcentaje"].ToString()),
                                                    dr["NroComprobante"].Equals(DBNull.Value) ? "" : dr["NroComprobante"].ToString(),
                                                    dr["PrimerMensual"].Equals(DBNull.Value) ? string.Empty : dr["PrimerMensual"].ToString(),
                                                    dr["MensualCuota"].Equals(DBNull.Value) ? string.Empty : dr["MensualCuota"].ToString(),
                                                    0,
                                                    dr["MAC"].ToString(),
                                                    false, //bool.Parse(dr["Stock"].ToString()),
                                                    0, 0,
                                                    dr["NroCuota"].Equals(DBNull.Value) ? 0 : int.Parse(dr["NroCuota"].ToString()),
                                                    new Beneficiario(long.Parse(dr["IdBeneficiario"].ToString()),
                                                                     long.Parse(dr["Cuil"].Equals(DBNull.Value) ? "0" : dr["Cuil"].ToString()),
                                                                     dr.GetString("ApellidoNombre")),
                                                    new Prestador(),
                                                    new Estado(dr["IdEstadoReg"].Equals(DBNull.Value) ? 0 : int.Parse(dr["IdEstadoReg"].ToString())),
                                                    new CodigoMovimiento(byte.Parse(dr["CodMovimiento"].ToString()), string.Empty),
                                                    new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                            dr.GetString("DescConceptoLiq")),
                                                    new TipoConcepto(short.Parse(dr["TipoConcepto"].ToString()),
                                                                     dr.GetString("DescTipoConcepto"), true),
                                                    new ModeloPC(),
                                                    new Auditoria(dr["Usuario"].ToString()))
                                        );

                    }
                }

                return lstNovedades;
            }
            catch (SqlException ErrSQL)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));
                if (ErrSQL.Number == 1205 || ErrSQL.Number == 1204)
                    throw new ApplicationException("Interbloqueo");
                else
                    throw ErrSQL;
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


        public static List<Novedad> Novedades_Traer(byte opcionBusqueda, long idPrestador, long benefCuil,
                                                    byte tipoConc, int concopp, int mensual, DateTime? fdesde, DateTime? fhasta,
                                                    bool generaArchivo, Boolean generadoAdmin, out string rutaArchivoSal)
        {
            string rutaArchivo = string.Empty;
            string nombreArchivo = string.Empty;
            rutaArchivoSal = string.Empty;
            string msgRta = string.Empty;
            ConsultaBatch consultaBatch = new ConsultaBatch();
            consultaBatch.NombreConsulta = ConsultaBatch.enum_ConsultaBatch_NombreConsulta.NOVEDADES_INGRESADAS;
            consultaBatch.IDPrestador = idPrestador;          
            consultaBatch.OpcionBusqueda = opcionBusqueda;
            consultaBatch.PeriodoCons = mensual.ToString();
            consultaBatch.UnConceptoLiquidacion = new ConceptoLiquidacion(concopp, string.Empty, new TipoConcepto(tipoConc, string.Empty));
            consultaBatch.NroBeneficio = benefCuil;
            consultaBatch.GeneradoAdmin = generadoAdmin;
            consultaBatch.FechaDesde = fdesde;
            consultaBatch.FechaHasta = fhasta;         

            try
            {
                if (opcionBusqueda != 1 || generaArchivo == true)
                {
                    msgRta = ConsultasBatchDAO.ExisteConsulta(consultaBatch);
                    if (!string.IsNullOrEmpty(msgRta))
                        throw new ApplicationException("MSG_ERROR" + msgRta + "FIN_MSG_ERROR");
                }

                List<Novedad> listNovedades = Novedades_TraerConsulta(opcionBusqueda, idPrestador, benefCuil,
                                                                      tipoConc, concopp, mensual, fdesde, fhasta);

                if (listNovedades.Count > 0 && (opcionBusqueda != 1 || generaArchivo == true))
                {
                    int maxCantidad = Settings.MaxCantidadRegistros();

                    if (listNovedades.Count >= maxCantidad || generaArchivo == true)
                    {
                        nombreArchivo = Utilidades.GeneraNombreArchivo(consultaBatch.NombreConsulta.ToString(), idPrestador, out rutaArchivo);
                        rutaArchivoSal = Path.Combine(rutaArchivo, nombreArchivo);
                        StreamWriter sw = new StreamWriter(rutaArchivoSal, false, Encoding.UTF8);
                        string separador = Settings.DelimitadorCampo();

                        foreach (Novedad oNovedad in listNovedades)
                        {
                            StringBuilder linea = new StringBuilder();

                            linea.Append(oNovedad.IdNovedad.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.IdBeneficiario.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.ApellidoNombre.ToString() + separador);
                            linea.Append(oNovedad.FechaNovedad.ToString("dd/MM/yyyy HH:mm:ss") + separador);
                            linea.Append(oNovedad.UnTipoConcepto.IdTipoConcepto.ToString() + separador);
                            linea.Append(oNovedad.UnTipoConcepto.DescTipoConcepto.ToString() + separador);
                            linea.Append(oNovedad.UnConceptoLiquidacion.CodConceptoLiq.ToString() + separador);
                            linea.Append(oNovedad.UnConceptoLiquidacion.DescConceptoLiq.ToString() + separador);
                            linea.Append(oNovedad.ImporteTotal.ToString().Replace(",", ".") + separador);
                            linea.Append(oNovedad.CantidadCuotas.ToString() + separador);
                            linea.Append(oNovedad.Porcentaje.ToString().Replace(",", ".") + separador);
                            linea.Append(oNovedad.ImporteCuota.ToString().Replace(",", ".") + separador);
                            linea.Append(oNovedad.NroCuotaLiquidada.ToString() + separador);
                            linea.Append(oNovedad.MensualCuota.ToString() + separador);
                            linea.Append(oNovedad.Comprobante.ToString() + separador);
                            linea.Append(oNovedad.MAC.ToString() + separador);
                            linea.Append(oNovedad.UnAuditoria.Usuario.ToString());

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
                        listNovedades = new List<Novedad>();
                    }
                }

                return listNovedades;
            }
            catch (SqlException errsql)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), errsql.Source, errsql.Message));

                if (errsql.Number == -2)
                {
                    nombreArchivo = Utilidades.GeneraNombreArchivo(consultaBatch.NombreConsulta.ToString(), idPrestador, out rutaArchivo);
                    consultaBatch.NomArchGenerado = nombreArchivo;
                    consultaBatch.RutaArchGenerado = rutaArchivo;
                    consultaBatch.FechaGenera = DateTime.MinValue;
                    consultaBatch.Vigente = false;

                    msgRta = ConsultasBatchDAO.AltaNuevaConsulta(consultaBatch);

                    throw new ApplicationException("MSG_ERROR Generando el archivo. Reingrese a la consulta en unos minutos.FIN_MSG_ERROR");
                }
                else
                    throw errsql;
            }
            catch (ApplicationException apperr)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), apperr.Source, apperr.Message));
                throw new ApplicationException(apperr.Message);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        #endregion

        #region Novedades_Trae_NoAplicadas

        public static List<Novedad> Novedades_Trae_NoAplicadas(byte opcionBusqueda, long idPrestador,
                                                               long benefCuil, byte tipoConc, int concopp,
                                                               DateTime? fdesde, DateTime? fhasta, string mensual,
                                                               bool generaArchivo, bool generadoAdmin, out string rutaArchivoSal)
        {
            string rutaArchivo = string.Empty;
            string nombreArchivo = string.Empty;
            rutaArchivoSal = string.Empty;
            string msgRta = string.Empty;
          
            ConsultaBatch consultaBatch = new ConsultaBatch();
            consultaBatch.NombreConsulta = ConsultaBatch.enum_ConsultaBatch_NombreConsulta.NOVEDADES_NOAPLICADAS;
            consultaBatch.IDPrestador = idPrestador;      
            consultaBatch.OpcionBusqueda = opcionBusqueda;
            consultaBatch.PeriodoCons = mensual;
            consultaBatch.UnConceptoLiquidacion = new ConceptoLiquidacion(concopp, string.Empty, new TipoConcepto(tipoConc, string.Empty));
            consultaBatch.NroBeneficio = benefCuil;
            consultaBatch.GeneradoAdmin = generadoAdmin;
            consultaBatch.FechaDesde = fdesde;
            consultaBatch.FechaHasta = fhasta;

            try
            {
                if (opcionBusqueda != 1 || generaArchivo == true)
                {
                    msgRta = ConsultasBatchDAO.ExisteConsulta(consultaBatch);
                    if (!string.IsNullOrEmpty(msgRta))
                    {
                        throw new ApplicationException("MSG_ERROR" + msgRta + "FIN_MSG_ERROR");
                    }
                }

                List<Novedad> listNovedades = Novedades_Trae_NoAplicadasConsulta(opcionBusqueda, idPrestador, benefCuil,
                                                                                 tipoConc, concopp, fdesde, fhasta);

                if ((listNovedades.Count > 0) && (opcionBusqueda != 1 || generaArchivo == true))
                {
                    int maxCantidad = Settings.MaxCantidadRegistros();

                    if (listNovedades.Count >= maxCantidad || generaArchivo == true)
                    {
                        nombreArchivo = Utilidades.GeneraNombreArchivo(consultaBatch.NombreConsulta.ToString(), idPrestador, out rutaArchivo);
                        rutaArchivoSal = Path.Combine(rutaArchivo, nombreArchivo);

                        StreamWriter sw = new StreamWriter(rutaArchivoSal, false, Encoding.UTF8);
                        string separador = Settings.DelimitadorCampo();

                        foreach (Novedad oNovedad in listNovedades)
                        {
                            StringBuilder linea = new StringBuilder();

                            linea.Append(oNovedad.IdNovedad.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.IdBeneficiario.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.ApellidoNombre.ToString() + separador);
                            linea.Append(oNovedad.FechaNovedad.ToString("dd/MM/yyyy HH:mm:ss") + separador);
                            linea.Append(oNovedad.UnTipoConcepto.IdTipoConcepto.ToString() + separador);
                            linea.Append(oNovedad.UnTipoConcepto.DescTipoConcepto.ToString() + separador);
                            linea.Append(oNovedad.UnConceptoLiquidacion.CodConceptoLiq.ToString() + separador);
                            linea.Append(oNovedad.UnConceptoLiquidacion.DescConceptoLiq.ToString() + separador);
                            linea.Append(oNovedad.ImporteTotal.ToString().Replace(",", ".") + separador);
                            linea.Append(oNovedad.CantidadCuotas.ToString() + separador);
                            linea.Append(oNovedad.Porcentaje.ToString().Replace(",", ".") + separador);
                            linea.Append(oNovedad.ImporteCuota.ToString().Replace(",", ".") + separador);
                            linea.Append(oNovedad.NroCuotaLiquidada.ToString() + separador);
                            linea.Append(oNovedad.MensualCuota.ToString() + separador);
                            linea.Append(oNovedad.Comprobante.ToString() + separador);
                            linea.Append(oNovedad.MAC.ToString() + separador);
                            linea.Append(oNovedad.UnAuditoria.Usuario.ToString());

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
                        listNovedades = new List<Novedad>();
                    }
                }

                return listNovedades;
            }
            catch (SqlException errsql)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), errsql.Source, errsql.Message));

                if (errsql.Number == -2)
                {
                    nombreArchivo = Utilidades.GeneraNombreArchivo(consultaBatch.NombreConsulta.ToString(), idPrestador, out rutaArchivo);
                    consultaBatch.NomArchGenerado = nombreArchivo;
                    consultaBatch.RutaArchGenerado = rutaArchivo;
                    consultaBatch.FechaGenera = DateTime.MinValue;
                    consultaBatch.Vigente = false;

                    msgRta = ConsultasBatchDAO.AltaNuevaConsulta(consultaBatch);

                    throw new ApplicationException("MSG_ERROR Generando el archivo. Reingrese a la consulta en unos minutos.FIN_MSG_ERROR");
                }
                else
                {
                    throw errsql;
                }
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

        public static List<Novedad> Novedades_Trae_NoAplicadasConsulta(byte opcion, long lintPrestador, long benefCuil,
                                                                        byte tipoConc, int concopp, DateTime? fdesde, DateTime? fhasta)
        {
            string sql = "Novedades_TNoAplicadas_V2";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad> listNovedades = new List<Novedad>();

            try
            {
                db.AddInParameter(dbCommand, "@Opcion", DbType.Int16, opcion);
                db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, lintPrestador);
                db.AddInParameter(dbCommand, "@BenefCuil", DbType.Int64, benefCuil);
                db.AddInParameter(dbCommand, "@TipoConc", DbType.Int16, tipoConc);
                db.AddInParameter(dbCommand, "@Conc", DbType.Int32, concopp);
                db.AddInParameter(dbCommand, "@fdesde", DbType.String, !fdesde.HasValue ? null : fdesde.Value.ToShortDateString());
                db.AddInParameter(dbCommand, "@fhasta", DbType.String, !fhasta.HasValue ? null : fhasta.Value.ToShortDateString());

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        //Int64.Parse(dr["IdNovedad"].ToString()),
                        //Int64.Parse(dr["IdBeneficiario"].ToString()),
                        //dr["ApellidoNombre"].ToString(),
                        //dr["FecNov"].Equals(DBNull.Value) ? "" : dr["FecNov"].ToString(),
                        //byte.Parse(dr["TipoConcepto"].ToString()),
                        //dr["DescTipoConcepto"].ToString(),
                        //int.Parse(dr["CodConceptoLiq"].ToString()),
                        //dr["DescConceptoLiq"].ToString(),
                        //decimal.Parse(dr["ImporteTotal"].ToString()),
                        //byte.Parse(dr["CantCuotas"].ToString()),
                        //decimal.Parse(dr["Porcentaje"].ToString()),
                        //dr["ImporteCuota"].Equals(DBNull.Value) ? 0 : decimal.Parse(dr["ImporteCuota"].ToString()),
                        //dr["NroCuota"].Equals(DBNull.Value) ? 0 : byte.Parse(dr["NroCuota"].ToString()),
                        //dr["MensualCuota"].Equals(DBNull.Value) ? 0 : int.Parse(dr["MensualCuota"].ToString()),
                        //dr["NroComprobante"].Equals(DBNull.Value) ? "" : dr["NroComprobante"].ToString(),
                        //dr["MAC"].ToString(),
                        //dr["Usuario"].ToString(),

                        listNovedades.Add(new Novedad(Int64.Parse(dr["IdNovedad"].ToString()),
                                                      dr["FecNov"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecNov"].ToString()),
                                                      null,
                                                      dr["ImporteCuota"].Equals(DBNull.Value) ? 0 : double.Parse(dr["ImporteCuota"].ToString()),
                                                      double.Parse(dr["ImporteTotal"].ToString()),
                                                      0, 0,
                                                      byte.Parse(dr["CantCuotas"].ToString()),
                                                      float.Parse(dr["Porcentaje"].ToString()),
                                                      dr["NroComprobante"].Equals(DBNull.Value) ? "" : dr["NroComprobante"].ToString(),
                                                      string.Empty, //dr["PrimerMensual"].Equals(DBNull.Value) ? string.Empty : dr["PrimerMensual"].ToString(),
                                                      dr["MensualCuota"].Equals(DBNull.Value) ? string.Empty : dr["MensualCuota"].ToString(),
                                                      0, dr["MAC"].ToString(),
                                                      false, 0, 0,
                                                      dr["NroCuota"].Equals(DBNull.Value) ? 0 : int.Parse(dr["NroCuota"].ToString()),
                                                      new Beneficiario(long.Parse(dr["IdBeneficiario"].ToString()),
                                                                       0, dr.GetString("ApellidoNombre")),
                                                      new Prestador(),
                                                      new Estado(), //dr["IdEstadoReg"].Equals(DBNull.Value) ? 0 : int.Parse(dr["IdEstadoReg"].ToString())),
                                                      new CodigoMovimiento(), //byte.Parse(dr["CodMovimiento"].ToString()), string.Empty),
                                                      new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                              dr.GetString("DescConceptoLiq")),
                                                      new TipoConcepto(short.Parse(dr["TipoConcepto"].ToString()),
                                                                       dr.GetString("DescTipoConcepto"), true),
                                                      new ModeloPC(),
                                                      new Auditoria(dr["Usuario"].ToString()))
                                          );
                    }
                }
                return listNovedades;
            }
            catch (SqlException ErrSQL)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));

                if (ErrSQL.Number == 1205 || ErrSQL.Number == 1204)
                {
                    throw new ApplicationException("Interbloqueo");
                }
                else
                {
                    throw ErrSQL;
                }
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

        #endregion

        #region Novedades de Baja Traer Agrupada
       
        private static List<Novedad> Novedades_BajaTraerAgrupadaConsulta(byte opcionBusqueda, long idPrestador, long benefCuil, long idNovedad, byte tipoConc, int concOpp, string mesAplica, DateTime? fechaDesde, DateTime? fechaHasta, bool soloArgenta, bool soloEntidades)
        {
            string sql = "Novedades_BajaT_Agrupadas_V2";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad> listaNovedades = new List<Novedad>();

            try
            {
                db.AddInParameter(dbCommand, "@IDPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@opcion", DbType.Int16, opcionBusqueda);
                db.AddInParameter(dbCommand, "@BenefCuil", DbType.String, benefCuil);
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.String, idNovedad);
                db.AddInParameter(dbCommand, "@TipoConc", DbType.Int16, tipoConc);
                db.AddInParameter(dbCommand, "@Conc", DbType.Int32, concOpp);
                db.AddInParameter(dbCommand, "@MesAplica", DbType.String, mesAplica);
                db.AddInParameter(dbCommand, "@SoloArgenta", DbType.Boolean, soloArgenta);
                db.AddInParameter(dbCommand, "@SoloEntidades", DbType.Boolean, soloEntidades);
                db.AddInParameter(dbCommand, "@FechaDesde", DbType.DateTime, fechaDesde);
                db.AddInParameter(dbCommand, "@FechaHasta", DbType.DateTime, fechaHasta);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {

                        listaNovedades.Add(new Novedad(Int64.Parse(dr["IdNovedad"].ToString()),
                                              dr["FecNov"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecNov"].ToString()),
                                              null,
                                              dr["ImporteCuota"].Equals(DBNull.Value) ? 0 : double.Parse(dr["ImporteCuota"].ToString()),
                                              double.Parse(dr["ImporteTotal"].ToString()),
                                              dr["montoPrestamo"].Equals(DBNull.Value) ? 0 : double.Parse(dr["montoPrestamo"].ToString()),
                                              0,
                                              dr["ImporteLiq"].Equals(DBNull.Value) ? 0 : double.Parse(dr["ImporteLiq"].ToString()),
                                              dr["CantCuotas"].Equals(DBNull.Value) ? byte.Parse("0") : byte.Parse(dr["CantCuotas"].ToString()),
                                              dr["Porcentaje"].Equals(DBNull.Value) ? 0 : float.Parse(dr["Porcentaje"].ToString()),
                                              dr["NroComprobante"].Equals(DBNull.Value) ? "" : dr["NroComprobante"].ToString(),
                                              null,
                                              string.Empty,
                                              0,
                                              dr["MAC"].ToString(),
                                              bool.Parse(dr["Stock"].ToString()),
                                              0, 0,
                                              0,
                                              DateTime.Parse(dr["FechaEliminacion"].ToString()),
                                              new Beneficiario(long.Parse(dr["IdBeneficiario"].ToString()),
                                                               long.Parse(dr["Cuil"].Equals(DBNull.Value) ? "0" : dr["Cuil"].ToString()),
                                                               dr["Documento"].ToString(),
                                                               dr.GetString("ApellidoNombre")),
                                              new Prestador(),
                                              new Estado(dr["IdEstadoReg"].Equals(DBNull.Value) ? 0 : int.Parse(dr["IdEstadoReg"].ToString()),
                                                         dr["DescripcionEstadoReg"].Equals(DBNull.Value) ? "" : dr["DescripcionEstadoReg"].ToString()),
                                              new Estado(dr["IdEstadoNov"].Equals(DBNull.Value) ? 0 : int.Parse(dr["IdEstadoNov"].ToString())),
                                              new CodigoMovimiento(),
                                              new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                      dr.GetString("DescConceptoLiq")),
                                              new TipoConcepto(short.Parse(dr["TipoConcepto"].ToString()),
                                                               dr.GetString("DescTipoConcepto"), true),
                                              new ModeloPC(),
                                              new Auditoria())
                                        );
                    }
                }

                return listaNovedades;
            }
            catch (SqlException ErrSQL)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));

                if (ErrSQL.Number == 1205 || ErrSQL.Number == 1204)
                {
                    throw new ApplicationException("Interbloqueo");
                }
                else
                {
                    throw ErrSQL;
                }

            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {

            }
        }

        public static List<Novedad> Novedades_BajaTraerAgrupada(byte opcionBusqueda, long idPrestador, long benefCuil, long idNovedad,
                                                                byte tipoConc, int concopp, string mensual,
                                                                DateTime? fechaDesde, DateTime? fechaHasta, bool soloArgenta, bool soloEntidades,
                                                                bool generaArchivo, bool generadoAdmin, out string rutaArchivoSal)
        {
            string rutaArchivo = string.Empty;
            string nombreArchivo = string.Empty;
            rutaArchivoSal = string.Empty;
            string msgRta = string.Empty;         
            
            Usuario usuario = Utilidades.GetUsuario();
            ConsultaBatch consultaBatch = new ConsultaBatch();
            consultaBatch.IDPrestador = idPrestador;
            consultaBatch.NombreConsulta = ConsultaBatch.enum_ConsultaBatch_NombreConsulta.NOVEDADES_CANCELADASV2;
            consultaBatch.OpcionBusqueda = opcionBusqueda;
            consultaBatch.PeriodoCons = mensual;
            consultaBatch.UnConceptoLiquidacion = new ConceptoLiquidacion(concopp, string.Empty, new TipoConcepto(tipoConc, string.Empty));
            consultaBatch.NroBeneficio = benefCuil;
            consultaBatch.FechaDesde = fechaDesde;
            consultaBatch.FechaHasta = fechaHasta;
            consultaBatch.GeneradoAdmin = generadoAdmin;
            consultaBatch.Nro_Sucursal = usuario.OficinaCodigo;
            consultaBatch.Usuario_Logeado = usuario.Legajo;
            consultaBatch.Perfil = usuario.Grupo;
            consultaBatch.SoloArgenta = soloArgenta;
            consultaBatch.SoloEntidades = soloEntidades;
            consultaBatch.Idnovedad = idNovedad;

            try
            {


                if ((opcionBusqueda != 1 && opcionBusqueda != 2) || generaArchivo == true)
                {
                    msgRta = ConsultasBatchDAO.ExisteConsulta(consultaBatch);

                    if (!string.IsNullOrEmpty(msgRta))
                        throw new ApplicationException("MSG_ERROR" + msgRta + "FIN_MSG_ERROR");
                }

                List<Novedad> listNovedades = Novedades_BajaTraerAgrupadaConsulta(opcionBusqueda, idPrestador, benefCuil, idNovedad,
                                                                                  tipoConc, concopp, mensual,
                                                                                  fechaDesde, fechaHasta, soloArgenta, soloEntidades);

                if (listNovedades.Count > 0 && (opcionBusqueda != 1 || generaArchivo == true))
                {
                    int maxCantidad = Settings.MaxCantidadRegistros();

                    if (listNovedades.Count >= maxCantidad || generaArchivo == true)
                    {
                        nombreArchivo = Utilidades.GeneraNombreArchivo(consultaBatch.NombreConsulta.ToString(), idPrestador, out rutaArchivo);
                        rutaArchivoSal = Path.Combine(rutaArchivo, nombreArchivo);

                        StreamWriter sw = new StreamWriter(rutaArchivoSal, false, Encoding.UTF8);
                        string separador = Settings.DelimitadorCampo();

                        foreach (Novedad oNovedad in listNovedades)
                        {
                            StringBuilder linea = new StringBuilder();

                            linea.Append(oNovedad.UnBeneficiario.Cuil.ToString() + separador);
                            linea.Append(oNovedad.IdNovedad.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.IdBeneficiario.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.ApellidoNombre.ToString() + separador);
                            linea.Append(oNovedad.FechaNovedad.ToString("dd/MM/yyyy HH:mm:ss") + separador);
                            linea.Append(oNovedad.UnTipoConcepto.IdTipoConcepto.ToString() + separador);
                            linea.Append(oNovedad.UnTipoConcepto.DescTipoConcepto.ToString() + separador);
                            linea.Append(oNovedad.UnConceptoLiquidacion.CodConceptoLiq.ToString() + separador);
                            linea.Append(oNovedad.UnConceptoLiquidacion.DescConceptoLiq.ToString() + separador);
                            linea.Append(oNovedad.ImporteTotal.ToString().Replace(",", ".") + separador);
                            linea.Append(oNovedad.CantidadCuotas.ToString() + separador);
                            linea.Append(oNovedad.Porcentaje.ToString().Replace(",", ".") + separador);
                            linea.Append(oNovedad.ImporteCuota.ToString().Replace(",", ".") + separador);
                            linea.Append(oNovedad.Comprobante.ToString() + separador);
                            linea.Append(oNovedad.MAC.ToString() + separador);
                            linea.Append(oNovedad.UnEstadoReg.IdEstado.ToString() + separador);
                            linea.Append(oNovedad.UnEstadoReg.DescEstado + separador);
                            linea.Append(oNovedad.ImporteLiquidado.ToString() + separador);
                            linea.Append(oNovedad.UnEstadoNovedad.IdEstado.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.NroDoc.ToString() + separador);
                            linea.Append(oNovedad.Stock.ToString() + separador);
                            linea.Append(oNovedad.FechaBaja.Value.ToString("dd/MM/yyyy") + separador);
                            linea.Append(oNovedad.UnCodMovimiento.CodMovimiento.ToString() + separador);

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
                        listNovedades = new List<Novedad>();
                    }
                }

                return listNovedades;
            }
            catch (SqlException errsql)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), errsql.Source, errsql.Message));

                if (errsql.Number == -2)
                {
                    nombreArchivo = Utilidades.GeneraNombreArchivo(consultaBatch.NombreConsulta.ToString(), idPrestador, out rutaArchivo);
                    consultaBatch.NomArchGenerado = nombreArchivo;
                    consultaBatch.RutaArchGenerado = rutaArchivo;
                    consultaBatch.FechaGenera = DateTime.MinValue;
                    consultaBatch.Vigente = false;

                    msgRta = ConsultasBatchDAO.AltaNuevaConsulta(consultaBatch);

                    throw new ApplicationException("MSG_ERROR Generando el archivo. Reingrese a la consulta en unos minutos.FIN_MSG_ERROR");
                }
                else
                    throw errsql;
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

        #endregion

        #region Novedades de Baja Traer
        /// <summary>
        /// Trae el detalle de las bajas agrupadas, consulta de novedades canceladas.
        /// </summary>
        /// <param name="idPrestador"></param>
        /// <param name="opcionBusqueda"></param>
        /// <param name="benefCuil"></param>
        /// <param name="tipoConc"></param>
        /// <param name="concOpp"></param>
        /// <returns></returns>                
        public static List<Novedad> Novedades_BajaTraer(long idPrestador, byte opcionBusqueda,
                                                         long benefCuil, byte tipoConc, int concOpp)
        {
            string sql = "Novedades_BajaT_V2";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad> listNovedades = new List<Novedad>();

            try
            {
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@Opcion", DbType.Byte, opcionBusqueda);
                db.AddInParameter(dbCommand, "@BenefCuil", DbType.Int64, benefCuil);
                db.AddInParameter(dbCommand, "@TipoConc", DbType.Byte, tipoConc);
                db.AddInParameter(dbCommand, "@Conc", DbType.Int64, concOpp);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        listNovedades.Add(new Novedad(Int64.Parse(dr["IdNovedad"].ToString()),
                                          dr["FecNov"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecNov"].ToString()),
                                          null,
                                          dr["ImporteCuota"].Equals(DBNull.Value) ? 0 : double.Parse(dr["ImporteCuota"].ToString()),
                                          double.Parse(dr["ImporteTotal"].ToString()),
                                          0, 0, 0,
                                          byte.Parse(dr["CantCuotas"].ToString()),
                                          float.Parse(dr["Porcentaje"].ToString()),
                                          dr["NroComprobante"].Equals(DBNull.Value) ? "" : dr["NroComprobante"].ToString(),
                                          dr["PrimerMensual"].Equals(DBNull.Value) ? string.Empty : dr["PrimerMensual"].ToString(),
                                          dr["MensualCuota"].Equals(DBNull.Value) ? string.Empty : dr["MensualCuota"].ToString(),
                                          0,
                                          dr["MAC"].ToString(),
                                          bool.Parse(dr["Stock"].ToString()),
                                          0, 0,
                                          dr["NroCuota"].Equals(DBNull.Value) ? 0 : int.Parse(dr["NroCuota"].ToString()),
                                          DateTime.Parse(dr["FechaEliminacion"].ToString()),
                                          new Beneficiario(long.Parse(dr["IdBeneficiario"].ToString()),
                                                           long.Parse(dr["Cuil"].Equals(DBNull.Value) ? "0" : dr["Cuil"].ToString()),
                                                           dr["Documento"].ToString(),
                                                           dr.GetString("ApellidoNombre")),
                                          new Prestador(),
                                          new Estado(dr["IdEstadoReg"].Equals(DBNull.Value) ? 0 : int.Parse(dr["IdEstadoReg"].ToString()),
                                                     dr["DescripcionEstadoReg"].Equals(DBNull.Value) ? "" : dr["DescripcionEstadoReg"].ToString()),
                                          new Estado(dr["IdEstadoNov"].Equals(DBNull.Value) ? 0 : int.Parse(dr["IdEstadoNov"].ToString())),
                                          new CodigoMovimiento(),
                                          new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                  dr.GetString("DescConceptoLiq")),
                                          new TipoConcepto(short.Parse(dr["TipoConcepto"].ToString()),
                                                           dr.GetString("DescTipoConcepto"), true),
                                          new ModeloPC(),
                                          new Auditoria(dr["Usuario"].ToString()))
                                    );
                    }
                }

                return listNovedades;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion

        #region Novedades Traer X ID

        public static List<Novedad> Novedades_Traer_X_Id(long idNovedad)
        {
            string sql = "Novedades_TxIdNovedad";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad> lstNovedades = new List<Novedad>();
            List<Cuota> lst_Cuota = new List<Cuota>();
            Novedad unaNovedad = null;

            try
            {
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, idNovedad);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        unaNovedad = new Novedad(long.Parse(dr["IdNovedad"].ToString()),
                                                 DateTime.Parse(dr["FecNov"].ToString() + " " + dr["HoraNov"].ToString()),
                                                 dr["ImporteTotal"].Equals(DBNull.Value) ? 0 : double.Parse(dr.GetValue("ImporteTotal").ToString()),
                                                 dr["CantCuotas"].Equals(DBNull.Value) ? byte.Parse("0") : byte.Parse(dr.GetValue("CantCuotas").ToString()),
                                                 dr["Porcentaje"].Equals(DBNull.Value) ? 0 : Single.Parse(dr["Porcentaje"].ToString()),
                                                 dr["NroComprobante"].ToString(),
                                                 dr["MAC"].ToString(), null,
                                                 dr["PrimerMensual"].ToString(),
                                                 false, null
                                                 );

                        unaNovedad.Otro = dr["otro"].ToString();
                        unaNovedad.CBU = dr["cbu"].ToString();
                        unaNovedad.Nro_Ticket = dr["nroTicket"].ToString();
                        unaNovedad.MontoPrestamo = dr["MontoPrestamo"].Equals(DBNull.Value) ? 0 : double.Parse(dr["MontoPrestamo"].ToString());
                        unaNovedad.MensualCuota = dr["cuotatotalmensual"].ToString();
                        unaNovedad.TNA = dr["TNA"].Equals(DBNull.Value) ? 0 : double.Parse(dr["TNA"].ToString());
                        unaNovedad.CFTEAReal = dr["CFTEAReal"].Equals(DBNull.Value) ? 0 : double.Parse(dr["CFTEAReal"].ToString());
                        unaNovedad.Gasto_Otorgamiento = dr["gastootorgamiento"].Equals(DBNull.Value) ? 0 : double.Parse(dr["gastootorgamiento"].ToString());
                        unaNovedad.Gasto_Adm_Mensual = dr["gastoAdmMensual"].Equals(DBNull.Value) ? 0 : double.Parse(dr["gastoAdmMensual"].ToString());
                        unaNovedad.Cuota_Social = dr["cuotasocial"].Equals(DBNull.Value) ? 0 : double.Parse(dr["cuotasocial"].ToString());
                        unaNovedad.Nro_Tarjeta = dr["nroTarjeta"].ToString();
                        unaNovedad.Nro_Sucursal = dr["nroSucursal"].ToString();

                        unaNovedad.UnTipoConcepto = new TipoConcepto(Byte.Parse(dr["TipoConcepto"].ToString()),
                                                                     dr.GetString("DescTipoConcepto"));
                        unaNovedad.UnPrestador = new Prestador(long.Parse(dr["IdPrestador"].ToString()),
                                                               dr.GetString("RazonSocial"), 0);

                        unaNovedad.UnCodMovimiento = new CodigoMovimiento(byte.Parse(dr["CodMovimiento"].ToString()),
                                                                          dr.GetString("DescMovimiento"));

                        unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                                   dr.GetString("DescConceptoLiq"));
                        unaNovedad.UnEstadoReg = new Estado(int.Parse(dr["IdEstadoReg"].ToString()),
                                                            dr.GetString("DescripcionEstadoReg"));
                        unaNovedad.UnAuditoria = new Auditoria(string.Empty, dr.GetString("IP"), null);

                        unaNovedad.UnBeneficiario = new Beneficiario(long.Parse(dr["IDBeneficiario"].ToString()),
                                                                     long.Parse(dr["CUIL"].ToString()),
                                                                     dr["Documento"].ToString(),
                                                                     dr["ApellidoNombre"].ToString());

                        unaNovedad.UnBeneficiario.TipoOrigenBeneficiario = new TipoOrigenBeneficiario(Convert.ToBoolean(dr["esPNC"]));
                        unaNovedad.UnTipoTarjeta = dr["idTipoTarjeta"].Equals(DBNull.Value) ? (enum_TipoTarjeta?)null : (enum_TipoTarjeta)Convert.ToInt32(dr["idTipoTarjeta"].ToString());
                        unaNovedad.GeneraNominada = dr["GeneraNominada"].ToString();
                        unaNovedad.GeneraCompImpedimentoFirma = dr["generaCompImpedidoFirmar"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["generaCompImpedidoFirmar"].ToString());
                        unaNovedad.FEstimadaEntrega = dr["fEstimadaEntrega"].Equals(DBNull.Value) ? (DateTime?)null : DateTime.Parse(dr["fEstimadaEntrega"].ToString());
                        unaNovedad.OficinaDestino = dr["oficinaDestino"].ToString();
                        unaNovedad.DescOficinaDestino = dr["denominacion"].ToString();

                        Cuota cuota = new Cuota(Convert.ToInt32(dr["nrocuota"]),
                                                Convert.ToDouble(dr["importeCuota"]),
                                                 dr["MensualCuota"].ToString());
                        cuota.Intereses = dr["importeinteres"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dr["importeinteres"]);
                        cuota.Amortizacion = dr["Amortizacion"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dr["Amortizacion"]);
                        cuota.Gasto_Adm = dr["gastoAdmMensualCalc"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dr["gastoAdmMensualCalc"]);
                        cuota.Gasto_Adm_Tarjeta = dr["gastoAdminTarjeta"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dr["gastoAdminTarjeta"]);
                        cuota.Seguro_Vida = dr["seguroVida"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dr["seguroVida"]);
                        cuota.Interes_Cuota_0 = dr["interesCuotaCero"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dr["interesCuotaCero"]);
                        unaNovedad.IdDomicilioBeneficiario = dr["IdDomicilioBeneficiario"].Equals(DBNull.Value) ? (long?)null : long.Parse(dr["IdDomicilioBeneficiario"].ToString());
                        lst_Cuota.Add(cuota);
                    }

                    if (unaNovedad != null)
                    {
                        unaNovedad.unaLista_Cuotas = lst_Cuota;
                        lstNovedades.Add(unaNovedad);
                        lst_Cuota = new List<Cuota>();
                    }
                }

                return lstNovedades;

            }
            catch (SqlException ErrSQL)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));

                if (ErrSQL.Number == 1205 || ErrSQL.Number == 1204)
                {
                    throw new ApplicationException("Interbloqueo");
                }
                else
                {
                    throw ErrSQL;
                }
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
        #endregion

        #region Novedades Traer X ID Toda Cuotas

        public static List<Novedad> Novedades_TraerXId_TodaCuotas(long idNovedad)
        {
            string sql = "Novedades_TxINovedad_TCuotas";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad> lstNovedades = new List<Novedad>();
            List<Cuota> lst_Cuota = new List<Cuota>();
            Novedad unaNovedad = null;

            try
            {
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, idNovedad);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        unaNovedad = new Novedad(long.Parse(dr["IdNovedad"].ToString()),
                                                 DateTime.Parse(dr["FecNov"].ToString() + " " + dr["HoraNov"].ToString()),
                                                 double.Parse(dr.GetValue("ImporteTotal").ToString()),
                                                 byte.Parse(dr.GetValue("CantCuotas").ToString()),
                                                 Single.Parse(dr["Porcentaje"].ToString()),
                                                 dr["NroComprobante"].ToString(),
                                                 dr["MAC"].ToString(), null,
                                                 dr["PrimerMensual"].ToString(),
                                                 false, null);

                        unaNovedad.Otro = dr["otro"].ToString();
                        unaNovedad.CBU = dr["cbu"].ToString();
                        unaNovedad.Nro_Ticket = dr["nroTicket"].ToString();
                        unaNovedad.MontoPrestamo = dr["MontoPrestamo"].Equals(DBNull.Value) ? new double() : double.Parse(dr["MontoPrestamo"].ToString());
                        unaNovedad.MensualCuota = dr["cuotatotalmensual"].ToString();
                        unaNovedad.TNA = dr["TNA"].Equals(DBNull.Value) ? new double() : double.Parse(dr["TNA"].ToString());
                        unaNovedad.CFTEAReal = dr["CFTEAReal"].Equals(DBNull.Value) ? new double() : double.Parse(dr["CFTEAReal"].ToString());
                        unaNovedad.MontoPrestamo = dr["MontoPrestamo"].Equals(DBNull.Value) ? new double() : double.Parse(dr["MontoPrestamo"].ToString());
                        unaNovedad.Gasto_Otorgamiento = dr["gastootorgamiento"].Equals(DBNull.Value) ? new double() : double.Parse(dr["gastootorgamiento"].ToString());
                        unaNovedad.Gasto_Adm_Mensual = dr["gastoAdmMensual"].Equals(DBNull.Value) ? new double() : double.Parse(dr["gastoAdmMensual"].ToString());
                        unaNovedad.Cuota_Social = dr["cuotasocial"].Equals(DBNull.Value) ? new double() : double.Parse(dr["cuotasocial"].ToString());
                        unaNovedad.Nro_Tarjeta = dr["nroTarjeta"].ToString();
                        unaNovedad.Nro_Sucursal = dr["nroSucursal"].ToString();

                        unaNovedad.UnTipoConcepto = new TipoConcepto(Byte.Parse(dr["TipoConcepto"].ToString()),
                                                                     dr.GetString("DescTipoConcepto"));
                        unaNovedad.UnPrestador = new Prestador(long.Parse(dr["IdPrestador"].ToString()),
                                                               dr.GetString("RazonSocial"), 0);

                        unaNovedad.UnCodMovimiento = new CodigoMovimiento(byte.Parse(dr["CodMovimiento"].ToString()),
                                                                          dr.GetString("DescMovimiento"));

                        unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                                   dr.GetString("DescConceptoLiq"));
                        unaNovedad.UnEstadoReg = new Estado(int.Parse(dr["IdEstadoReg"].ToString()),
                                                            dr.GetString("DescripcionEstadoReg"));
                        unaNovedad.UnAuditoria = new Auditoria(dr["Usuario"].ToString(), dr.GetString("IP"), null);

                        unaNovedad.UnBeneficiario = new Beneficiario(long.Parse(dr["IDBeneficiario"].ToString()),
                                                                     long.Parse(dr["CUIL"].ToString()),
                                                                     dr["Documento"].ToString(),
                                                                     dr["ApellidoNombre"].ToString());

                        unaNovedad.UnTipoTarjeta = dr["idTipoTarjeta"].Equals(DBNull.Value) ? (enum_TipoTarjeta?)null : (enum_TipoTarjeta)Convert.ToInt32(dr["idTipoTarjeta"].ToString());
                        unaNovedad.GeneraNominada = dr["GeneraNominada"].ToString();
                        unaNovedad.FEstimadaEntrega = dr["fEstimadaEntrega"].Equals(DBNull.Value) ? (DateTime?)null : DateTime.Parse(dr["fEstimadaEntrega"].ToString());
                        unaNovedad.OficinaDestino = dr["oficinaDestino"].ToString();
                        unaNovedad.DescOficinaDestino = dr["denominacion"].ToString();
                        unaNovedad.CodigoBanco = DBNull.Value.Equals(dr["nda_codbanco"]) ? string.Empty : dr["nda_codbanco"].ToString();
                        unaNovedad.DescripcionBanco = DBNull.Value.Equals(dr["denominacionBanco"]) ? string.Empty : dr["denominacionBanco"].ToString();
                        unaNovedad.CodigoAgencia = DBNull.Value.Equals(dr["nda_codagencia"]) ? string.Empty : dr["nda_codagencia"].ToString();
                        unaNovedad.DescripcionAgencia =  DBNull.Value.Equals(dr["denominacionAgencia"]) ?  string.Empty : dr["denominacionAgencia"].ToString();

                        Cuota cuota = new Cuota(dr["nrocuota"].Equals(DBNull.Value) ? 0 : Convert.ToInt32(dr["nrocuota"]),
                                                dr["importeCuota"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dr["importeCuota"]),
                                                 dr["MensualCuota"].ToString());
                        cuota.Intereses = dr["importeinteres"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dr["importeinteres"]);
                        cuota.Amortizacion = dr["Amortizacion"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dr["Amortizacion"]);
                        cuota.Gasto_Adm = dr["gastoAdmMensualCalc"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dr["gastoAdmMensualCalc"]);
                        cuota.Gasto_Adm_Tarjeta = dr["gastoAdminTarjeta"].Equals(DBNull.Value) ? new double() : Convert.ToDouble(dr["gastoAdminTarjeta"]);
                        cuota.Seguro_Vida = (dr["seguroVida"].Equals(DBNull.Value) ? new double() : Convert.ToDouble(dr["seguroVida"]));
                        cuota.Interes_Cuota_0 = (dr["interesCuotaCero"].Equals(DBNull.Value) ? new double() : Convert.ToDouble(dr["interesCuotaCero"]));
                        
                        unaNovedad.IdDomicilioBeneficiario = dr["IdDomicilioBeneficiario"].Equals(DBNull.Value) ? (long?)null : long.Parse(dr["IdDomicilioBeneficiario"].ToString());
                        unaNovedad.GeneraCompImpedimentoFirma = dr["generaCompImpedidoFirmar"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["generaCompImpedidoFirmar"].ToString());                        
                        lst_Cuota.Add(cuota);
                    }

                    if (unaNovedad != null)
                    {
                        unaNovedad.unaLista_Cuotas = lst_Cuota;
                        lstNovedades.Add(unaNovedad);
                        lst_Cuota = new List<Cuota>();
                    }
                }

                return lstNovedades;

            }
            catch (SqlException ErrSQL)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));

                if (ErrSQL.Number == 1205 || ErrSQL.Number == 1204)
                {
                    throw new ApplicationException("Interbloqueo");
                }
                else
                {
                    throw ErrSQL;
                }
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
        #endregion

        #region Novedades TxIdNovedad SinLiquidar

        public static List<Novedad> Novedades_TxIdNovedad_Sliq(long idNovedad)
        {
            string sql = "Novedades_TxIdNovedad_Sliq";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad> lstNovedades = new List<Novedad>();
            Novedad unaNovedad;

            try
            {
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, idNovedad);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {


                    dr.Read();
                    //NroCuota 
                    //ImporteCuota 
                    //MensualCuota 
                    unaNovedad = new Novedad(dr.GetInt64("IdNovedad"),
                                             DateTime.Parse(dr["FecNov"].ToString()),
                                             double.Parse(dr.GetValue("ImporteTotal").ToString()),
                                             dr.GetByte("CantCuotas"),
                                             Single.Parse(dr["Porcentaje"].ToString()),
                                             dr.GetString("NroComprobante"),
                                             dr.GetString("MAC"),
                                             dr["FecImportacion"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecImportacion"].ToString()),
                        //dr.GetNullableDateTime("FecImportacion"),
                                             dr.GetString("PrimerMensual"),
                                             dr.GetBoolean("Stock"), null);

                    unaNovedad.UnTipoConcepto = new TipoConcepto(Byte.Parse(dr["TipoConcepto"].ToString()),
                                                                 dr.GetString("DescTipoConcepto"));
                    unaNovedad.UnPrestador = new Prestador(long.Parse(dr["IdPrestador"].ToString()),
                                                           dr.GetString("RazonSocial"), 0);
                    unaNovedad.UnBeneficiario = new Beneficiario(long.Parse(dr["IdBeneficiario"].ToString()),
                                                                 long.Parse(dr["Cuil"].ToString()),
                                                                 dr.GetString("ApellidoNombre"));
                    unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(dr.GetInt32("CodConceptoLiq"),
                                                                               dr.GetString("DescConceptoLiq"));

                    //unaNovedad.UnEstadoReg = new Estado(dr.GetInt32("IdEstadoReg"),
                    //                                    dr.GetString("DescripcionEstadoReg"));
                    unaNovedad.UnEstadoReg = new Estado(int.Parse(dr["IdEstadoReg"].ToString()),
                                                      dr.GetString("DescripcionEstadoReg"));

                    unaNovedad.UnAuditoria = new Auditoria(string.Empty, dr.GetString("IP"), null);
                    unaNovedad.UnConceptoLiquidacion.EsAfiliacion = dr.GetBoolean("EsAfiliacion");

                    unaNovedad.UnCodMovimiento = new CodigoMovimiento(byte.Parse(dr["CodMovimiento"].ToString()), dr["DescMovimiento"].ToString());

                    List<Cuota> unaListaCuotas = new List<Cuota>();

                    Cuota unaCuota = new Cuota(dr["NroCuota"].Equals(DBNull.Value) ? new int() : int.Parse(dr["NroCuota"].ToString()),
                                               dr["ImporteCuota"].Equals(DBNull.Value) ? new double() : double.Parse(dr["ImporteCuota"].ToString()),
                                               dr["MensualCuota"].ToString());
                    unaListaCuotas.Add(unaCuota);

                    while (dr.Read())
                    {
                        unaCuota = new Cuota(int.Parse(dr["NroCuota"].ToString()), double.Parse(dr["ImporteCuota"].ToString()), dr["MensualCuota"].ToString());
                        unaListaCuotas.Add(unaCuota);
                    }
                    unaNovedad.unaLista_Cuotas = unaListaCuotas;

                    lstNovedades.Add(unaNovedad);

                }

                return lstNovedades;
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
        #endregion

        #region Novedades Trae Creditos Activos

        public static List<Novedad> Novedades_Trae_Creditos_Activos(long idPrestador, long idBeneficiario)
        {
            string sql = "Novedades_Tipo3_Vigentes";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad> lstNovedades = new List<Novedad>();
            Novedad unaNovedad;

            try
            {
                db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@Beneficiario", DbType.Int64, idBeneficiario);

                DataSet ds = db.ExecuteDataSet(dbCommand);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRowView dr in ds.Tables[0].DefaultView)
                    {
                        unaNovedad = new Novedad();
                        unaNovedad.IdNovedad = long.Parse(dr["IdNovedad"].ToString());
                        unaNovedad.FechaNovedad = DateTime.Parse(dr["FecNov"].ToString());
                        unaNovedad.ImporteCuota = double.Parse(dr["ImporteCuota"].ToString());
                        unaNovedad.ImporteTotal = double.Parse(dr["ImporteTotal"].ToString());
                        unaNovedad.CantidadCuotas = byte.Parse(dr["CantCuotas"].ToString());
                        unaNovedad.CantidadCuotasRestantes = byte.Parse(dr["CantCuotasImpagas"].ToString());
                        unaNovedad.Comprobante = dr["NroComprobante"].ToString();

                        unaNovedad.UnPrestador = new Prestador(long.Parse(dr["IdPrestador"].ToString()), string.Empty, 0);
                        unaNovedad.UnBeneficiario = new Beneficiario(long.Parse(dr["IdBeneficiario"].ToString()), 0, string.Empty);
                        unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()), dr["DescConceptoLiq"].ToString());


                        DataTable _dt = new DataTable();
                        _dt = ds.Tables[1];
                        _dt.DefaultView.RowFilter = "IDNovedad = " + unaNovedad.IdNovedad;
                        _dt.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;

                        List<Cuota> unaListaCuotas = new List<Cuota>();

                        foreach (DataRowView drC in _dt.DefaultView)
                        {
                            Cuota unaCuota = new Cuota(int.Parse(drC["NroCuota"].ToString()), double.Parse(drC["ImporteCuota"].ToString()),
                                                            drC["MensualCuota"].ToString());

                            unaListaCuotas.Add(unaCuota);
                        }

                        _dt.Dispose();

                        unaNovedad.unaLista_Cuotas = unaListaCuotas;

                        lstNovedades.Add(unaNovedad);

                    }
                }
                ds.Dispose();
                return lstNovedades;

                //CODIGO A NTERIOR
                //using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                //{
                //    while (dr.Read())
                //    {
                //        //unaNovedad = new Novedad(long.Parse(dr["IdNovedad"].ToString()),
                //        //                         DateTime.Parse(dr["FecNov"].ToString()),
                //        //                         dr["FecImportacion"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecImportacion"].ToString()),
                //        //                         double.Parse(dr["ImporteCuota"].ToString()),
                //        //                         double.Parse(dr.GetValue("ImporteTotal").ToString()),
                //        //                         0, 0,
                //        //                         byte.Parse(dr["CantCuotas"].ToString()),
                //        //                         0,
                //        //                         dr["NroComprobante"].ToString(),
                //        //                         dr["PrimerMensual"].ToString(),
                //        //                         dr["MensualCuota"].ToString(),
                //        //                         0,
                //        //                         dr["MAC"].ToString(),
                //        //                         false, 0, 0,
                //        //                         int.Parse(dr["NroCuota"].ToString()),
                //        //                         null);


                //        lstNovedades.Add(unaNovedad);
                //    }
                //}

            }
            catch (SqlException ErrSQL)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));

                if (ErrSQL.Number == 1205 || ErrSQL.Number == 1204)
                {
                    throw new ApplicationException("Interbloqueo");
                }
                else
                {
                    throw ErrSQL;
                }
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
        #endregion

        #region Novedades T1o6 Trae

        public static List<Novedad> Novedades_T1o6_Trae(long idPrestador, byte idTipoConcepto)
        {

            string sql = "Novedades_T1o6_Trae";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad> listNovedad = new List<Novedad>();
            Novedad unaNovedad;

            try
            {
                if (idTipoConcepto == 1 || idTipoConcepto == 6)
                {
                    db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, idPrestador);
                    db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Int16, idTipoConcepto);

                    using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                    {
                        while (dr.Read())
                        {

                            unaNovedad = new Novedad(long.Parse(dr["IdNovedad"].ToString()),
                                                     DateTime.Parse(dr["FecNov"].ToString()),
                                                     dr["FecImportacion"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecImportacion"].ToString()),
                                                     double.Parse(dr["ImporteCuota"].ToString()),
                                                     double.Parse(dr.GetValue("ImporteTotal").ToString()),
                                                     0, 0,
                                                     byte.Parse(dr.GetValue("CantCuotas").ToString()),
                                                     Single.Parse(dr["Porcentaje"].ToString()),
                                                     dr["NroComprobante"].ToString(),
                                                     dr["PrimerMensual"].ToString(),
                                                     dr["MensualCuota"].ToString(),
                                                     0,
                                                     dr["MAC"].ToString(),
                                                     false, 0, 0,
                                                     int.Parse(dr["NroCuota"].ToString()),
                                                     null);

                            unaNovedad.UnTipoConcepto = new TipoConcepto(Byte.Parse(dr["TipoConcepto"].ToString()),
                                                                         dr.GetString("DescTipoConcepto"));
                            unaNovedad.UnPrestador = new Prestador(long.Parse(dr["IdPrestador"].ToString()),
                                                                   dr.GetString("RazonSocial"),
                                                                   long.Parse(dr["CUIT_Prestador"].ToString()));
                            unaNovedad.UnBeneficiario = new Beneficiario(long.Parse(dr["IdBeneficiario"].ToString()),
                                                                         long.Parse(dr["Cuil"].ToString()),
                                                                         dr.GetString("ApellidoNombre"));
                            unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                                       dr.GetString("DescConceptoLiq"));
                            listNovedad.Add(unaNovedad);
                        }
                    }

                }

                return listNovedad;
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
        #endregion

        #region Novedades Trae Xa ABM codConcepto

        public static List<Novedad> Novedades_Trae_Xa_ABM_codConcepto(long idPrestador, long idBeneficiario, long codConceptoLiq)
        {
            string sql = "Novedades_TT_Xa_ABM_codConcepto";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad> listNovedad = new List<Novedad>();
            Novedad unaNovedad;

            try
            {
                db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@Beneficiario", DbType.String, idBeneficiario);
                db.AddInParameter(dbCommand, "@codConceptoLiq", DbType.Int64, codConceptoLiq);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        unaNovedad = new Novedad();
                        unaNovedad.IdNovedad = long.Parse(dr["IdNovedad"].ToString());
                        unaNovedad.FechaNovedad = DateTime.Parse(dr["FecNov"].ToString());
                        unaNovedad.FechaImportacion = dr["FecImportacion"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecImportacion"].ToString());
                        unaNovedad.ImporteTotal = double.Parse(dr.GetValue("ImporteTotal").ToString());
                        unaNovedad.CantidadCuotas = byte.Parse(dr.GetValue("CantCuotas").ToString());
                        unaNovedad.Porcentaje = Single.Parse(dr["Porcentaje"].ToString());
                        unaNovedad.Comprobante = dr["NroComprobante"].ToString();
                        unaNovedad.MAC = dr["MAC"].ToString();
                        unaNovedad.Nro_Tarjeta = dr["nroTarjeta"].Equals(DBNull.Value) ? null : dr["nroTarjeta"].ToString();
                        unaNovedad.FReposicion = dr["fReposicion"].Equals(DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["fReposicion"]);

                        unaNovedad.UnTipoConcepto = new TipoConcepto(Byte.Parse(dr["TipoConcepto"].ToString()),
                                                                     dr.GetString("DescTipoConcepto"));

                        unaNovedad.UnBeneficiario = new Beneficiario();
                        unaNovedad.UnBeneficiario.IdBeneficiario = long.Parse(dr["IdBeneficiario"].ToString());
                        unaNovedad.UnBeneficiario.ApellidoNombre = dr.GetString("ApellidoNombre");

                        unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                                   dr.GetString("DescConceptoLiq"));
                        listNovedad.Add(unaNovedad);
                    }
                }

                return listNovedad;
            }

            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion

        #region Novedades Trae Xa ABM

        public static List<Novedad> Novedades_Trae_Xa_ABM(long idPrestador, long idBeneficiario)
        {
            string sql = "Novedades_TT_Xa_ABM";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad> listNovedad = new List<Novedad>();
            Novedad unaNovedad;

            try
            {
                db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@Beneficiario", DbType.String, idBeneficiario);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        //unaNovedad = new Novedad(long.Parse(dr["IdNovedad"].ToString()),
                        //                         DateTime.Parse(dr["FecNov"].ToString()),
                        //                         dr["FecImportacion"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecImportacion"].ToString()),
                        //                         double.Parse(dr["ImporteCuota"].ToString()),
                        //                         double.Parse(dr.GetValue("ImporteTotal").ToString()),
                        //                         0, 0,
                        //                         byte.Parse(dr.GetValue("CantCuotas").ToString()),
                        //                         Single.Parse(dr["Porcentaje"].ToString()),
                        //                         dr["NroComprobante"].ToString(),
                        //                         dr["PrimerMensual"].ToString(),
                        //                         dr["MensualCuota"].ToString(),
                        //                         0,
                        //                         dr["MAC"].ToString(),
                        //                         false, 0, 0,
                        //                         int.Parse(dr["NroCuota"].ToString()),
                        //                         null);

                        //unaNovedad.UnTipoConcepto = new TipoConcepto(Byte.Parse(dr["TipoConcepto"].ToString()),
                        //                                             dr.GetString("DescTipoConcepto"));
                        //unaNovedad.UnBeneficiario = new Beneficiario(long.Parse(dr["IdBeneficiario"].ToString()),
                        //                                             long.Parse(dr["Cuil"].ToString()),
                        //                                             dr.GetString("ApellidoNombre"));
                        //unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                        //                                                           dr.GetString("DescConceptoLiq"));

                        unaNovedad = new Novedad();
                        unaNovedad.IdNovedad = long.Parse(dr["IdNovedad"].ToString());
                        unaNovedad.FechaNovedad = DateTime.Parse(dr["FecNov"].ToString());
                        unaNovedad.FechaImportacion = dr["FecImportacion"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecImportacion"].ToString());
                        unaNovedad.ImporteTotal = double.Parse(dr.GetValue("ImporteTotal").ToString());
                        unaNovedad.CantidadCuotas = byte.Parse(dr.GetValue("CantCuotas").ToString());
                        unaNovedad.Porcentaje = Single.Parse(dr["Porcentaje"].ToString());
                        unaNovedad.Comprobante = dr["NroComprobante"].ToString();
                        unaNovedad.MAC = dr["MAC"].ToString();

                        unaNovedad.UnTipoConcepto = new TipoConcepto(Byte.Parse(dr["TipoConcepto"].ToString()),
                                                                     dr.GetString("DescTipoConcepto"));

                        unaNovedad.UnBeneficiario = new Beneficiario();
                        unaNovedad.UnBeneficiario.IdBeneficiario = long.Parse(dr["IdBeneficiario"].ToString());
                        unaNovedad.UnBeneficiario.ApellidoNombre = dr.GetString("ApellidoNombre");

                        unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                                   dr.GetString("DescConceptoLiq"));
                        unaNovedad.HabilitaBaja = Convert.ToBoolean(dr["HabilitaBaja"]);

                        listNovedad.Add(unaNovedad);
                    }
                }

                return listNovedad;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion

        #region Novedades Suspensión

        public static Novedad Novedades_ParaSuspender_Traer(long idNovedad, out List<Novedades_Suspension> listaSuspension)
        {
            string sql = "Novedades_ParaSuspender";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            listaSuspension = new List<Novedades_Suspension>();
            Novedad unaNovedad = null;

            try
            {
                db.AddInParameter(dbCommand, "@idNovedad", DbType.Int64, idNovedad);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        unaNovedad = new Novedad();
                        unaNovedad.IdNovedad = long.Parse(dr["IdNovedad"].ToString());
                        unaNovedad.UnEstadoReg = new Estado(Int32.Parse(dr["IdEstadoReg"].ToString()), dr["DescripcionEstadoReg"].ToString());
                        unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                                   dr.GetString("DescConceptoLiq"));
                        unaNovedad.UnPrestador = new Prestador(dr.GetInt64("IdPrestador"), dr.GetString("RazonSocial"), 0);
                        unaNovedad.UnBeneficiario = new Beneficiario();
                        unaNovedad.UnBeneficiario.IdBeneficiario = long.Parse(dr["IdBeneficiario"].ToString());
                        unaNovedad.UnBeneficiario.Cuil = long.Parse(dr["Cuil"].ToString());
                        unaNovedad.UnBeneficiario.ApellidoNombre = dr.GetString("ApellidoNombre");
                        unaNovedad.ImporteTotal = double.Parse(dr.GetValue("ImporteTotal").ToString());
                        unaNovedad.CantidadCuotas = byte.Parse(dr.GetValue("CantCuotas").ToString());
                        unaNovedad.MontoPrestamo = Double.Parse(dr["montoPrestamo"].ToString());
                        unaNovedad.CantCuotasSinLiquidar = int.Parse(dr["CantCuotasSinLiquidar"].ToString());
                        unaNovedad.ProximoMensualAliq = dr["ProximoMensualAliq"].ToString();
                        unaNovedad.Nro_Tarjeta = dr["nroTarjeta"].ToString();
                        unaNovedad.UnTipoConcepto = new TipoConcepto(Int16.Parse(dr["TipoConcepto"].ToString()), dr["DescTipoConcepto"].ToString());
                        unaNovedad.unTipoEstado_SC = new TipoEstado_SC(int.Parse(dr["idEstadoSC"].ToString()), dr["DescripcionEstadoSC"].ToString());
                    }

                    dr.NextResult();

                    while (dr.Read())
                    {
                        Novedades_Suspension ns = new Novedades_Suspension(
                                                                           long.Parse(dr["IdNovedad"].ToString()),
                                                                           long.Parse(dr["idBeneficiario"].ToString()),
                                                                           DateTime.Parse(dr["FSuspension"].ToString()),
                                                                           dr["FReactivacion"].Equals(DBNull.Value) ? (DateTime?)null : DateTime.Parse(dr["FReactivacion"].ToString()),
                                                                           dr["NroExpediente"].ToString(),
                                                                           dr["MotivoSuspension"].ToString(),
                                                                           new Usuario(dr["UsuarioSuspension"].ToString(),
                                                                                       dr["OficinaSuspension"].ToString(), dr["IPSuspension"].ToString()),
                                                                           new Usuario(dr["UsuarioReactivacion"].ToString(), dr["OficinaReactivacion"].ToString(), dr["IPReactivacion"].ToString()),
                                                                           DateTime.Parse(dr["fultmodificacion"].ToString()),
                                                                           dr["mensualSuspension"].Equals(DBNull.Value) ? 0 : int.Parse(dr["mensualSuspension"].ToString()),
                                                                           dr["mensualReactivacion"].Equals(DBNull.Value) ? 0 : int.Parse(dr["mensualReactivacion"].ToString()),
                                                                           dr["MotivoReactivacion"].ToString()
                                                                           );

                        listaSuspension.Add(ns);
                    }

                    dr.Close();
                }

                return unaNovedad;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }


        #endregion

        public static String Novedades_Suspension_AB(Novedades_Suspension unaNovSuspension)
        {
            string sql = "Novedades_Suspension_AB";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            String mensaje = String.Empty;
            try
            {
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, unaNovSuspension.IdNovedad);
                db.AddInParameter(dbCommand, "@idBeneficiario", DbType.Int64, unaNovSuspension.IdBeneficiario);
                db.AddInParameter(dbCommand, "@FSuspension", DbType.DateTime, unaNovSuspension.FSuspension);
                db.AddInParameter(dbCommand, "@mensualSuspension", DbType.Int64, unaNovSuspension.MensualSuspension);
                db.AddInParameter(dbCommand, "@FReactivacion", DbType.DateTime, unaNovSuspension.FReactivacion != null ? unaNovSuspension.FReactivacion : null);
                db.AddInParameter(dbCommand, "@NroExpediente", DbType.String, unaNovSuspension.NroExpediente);
                db.AddInParameter(dbCommand, "@MotivoSuspension", DbType.String, unaNovSuspension.MotivoSuspension);
                db.AddInParameter(dbCommand, "@UsuarioSuspension", DbType.String, unaNovSuspension.UsuarioSuspension.Legajo);
                db.AddInParameter(dbCommand, "@OficinaSuspension", DbType.String, unaNovSuspension.UsuarioSuspension.OficinaCodigo);
                db.AddInParameter(dbCommand, "@IPSuspension", DbType.String, unaNovSuspension.UsuarioSuspension.Ip);
                db.AddInParameter(dbCommand, "@MotivoReactivacion", DbType.String, unaNovSuspension.MotivoReactivacion);
                db.AddInParameter(dbCommand, "@mensualReactivacion", DbType.Int64, unaNovSuspension.MensualReactivacion);
                db.AddInParameter(dbCommand, "@UsuarioReactivacion", DbType.String, unaNovSuspension.UsuarioReactivacion.Legajo);
                db.AddInParameter(dbCommand, "@OficinaReactivacion", DbType.String, unaNovSuspension.UsuarioReactivacion.OficinaCodigo);
                db.AddInParameter(dbCommand, "@IPReactivacion", DbType.String, unaNovSuspension.UsuarioReactivacion.Ip);
                db.AddOutParameter(dbCommand, "@mensaje", DbType.String, 100);

                using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    dbParametros = dbCommand.Parameters;
                    db.ExecuteNonQuery(dbCommand);
                    oTransactionScope.Complete();
                    mensaje = dbParametros[15].Value.ToString();
                }

                return mensaje;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }

        #region Novedades_Traer_Pendientes

        public static List<Novedad> Novedades_Traer_Pendientes(long prestador, string oficina, string cuil, short idEstado, DateTime? fechaDesde,
                                                               DateTime? fechaHasta, long idNovedad, out int total, out int totalACerrar)
        {
            string sql = "Novedades_TTxEstado";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad> listNovedades = new List<Novedad>();
            DbParameterCollection dbParametros = null;
            total = totalACerrar = 0;

            try
            {
                db.AddInParameter(dbCommand, "@prestador", DbType.Decimal, prestador);
                db.AddInParameter(dbCommand, "@oficina", DbType.String, oficina);
                db.AddInParameter(dbCommand, "@cuil", DbType.String, cuil);
                db.AddInParameter(dbCommand, "@idestadoreg", DbType.Int16, idEstado);
                db.AddInParameter(dbCommand, "@fechaDesde", DbType.DateTime, fechaDesde != DateTime.MinValue ? fechaDesde : null);
                db.AddInParameter(dbCommand, "@fechaHasta", DbType.DateTime, fechaHasta != DateTime.MinValue ? fechaHasta : null);
                db.AddInParameter(dbCommand, "@idNovedad", DbType.Int64, idNovedad);
                db.AddOutParameter(dbCommand, "@total", DbType.Int32, total);
                db.AddOutParameter(dbCommand, "@totalACerrar", DbType.Int32, totalACerrar);

                dbParametros = dbCommand.Parameters;

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        Novedad unaNovedad = new Novedad();

                        unaNovedad.IdNovedad = Int64.Parse(dr["IdNovedad"].ToString());
                        unaNovedad.FechaNovedad = dr["FecNov"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecNov"].ToString());
                        unaNovedad.FechaImportacion = dr["FecImportacion"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecImportacion"].ToString());
                        unaNovedad.ImporteTotal = double.Parse(dr["ImporteTotal"].ToString());
                        unaNovedad.MontoPrestamo = double.Parse(dr["montoPrestamo"].ToString());
                        unaNovedad.CantidadCuotas = byte.Parse(dr["CantCuotas"].ToString());
                        unaNovedad.Porcentaje = float.Parse(dr["Porcentaje"].ToString());
                        unaNovedad.Comprobante = dr["NroComprobante"].Equals(DBNull.Value) ? "" : dr["NroComprobante"].ToString();
                        unaNovedad.PrimerMensual = dr["PrimerMensual"].ToString();
                        unaNovedad.MAC = dr["MAC"].ToString();
                        unaNovedad.UnBeneficiario = new Beneficiario(Int64.Parse(dr["IdBeneficiario"].ToString()), Int64.Parse(dr["Cuil"].ToString()), int.Parse(dr["tipodoc"].ToString()),
                                                                    dr["NroDoc"].ToString(), dr["ApellidoNombre"].ToString(), dr["sexo"].ToString(), dr["CodTipoDoc"].ToString());
                        unaNovedad.UnEstadoReg = new Estado(Int32.Parse(dr["IdEstadoReg"].ToString()));
                        unaNovedad.UnCodMovimiento = new CodigoMovimiento(byte.Parse(dr["CodMovimiento"].ToString()), string.Empty);
                        unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()), dr["DescConceptoLiq"].ToString());
                        unaNovedad.UnTipoConcepto = new TipoConcepto(Int16.Parse(dr["TipoConcepto"].ToString()), string.Empty);
                        unaNovedad.UnAuditoria = new Auditoria(dr["Usuario"].ToString());
                        unaNovedad.Nro_Tarjeta = dr["nroTarjeta"].ToString();
                        unaNovedad.Nro_Sucursal = dr["nroSucursal"].ToString();
                        unaNovedad.CodMotivoAlta = int.Parse(dr["CodMotivoAlta"].ToString());
                        unaNovedad.CodOperacion = int.Parse(dr["codOperacion"].ToString());
                        unaNovedad.idItem = int.Parse(dr["idItem"].ToString());

                        unaNovedad.unContacto = new Contacto(dr["telediscado1"].ToString(), dr["telefono1"].ToString(),
                                                              Boolean.Parse(dr["esCelular1"].ToString()),
                                                              dr["telediscado2"].ToString(), dr["telefono2"].ToString(),
                                                              Boolean.Parse(dr["esCelular1"].ToString()), dr["mail"].ToString());
                        unaNovedad.UnPrestador = new Prestador(long.Parse(dr["IdPrestador"].ToString()));

                        listNovedades.Add(unaNovedad);
                    }
                }
                total = int.Parse(db.GetParameterValue(dbCommand, "@total").ToString());
                totalACerrar = int.Parse(db.GetParameterValue(dbCommand, "@totalACerrar").ToString());
                return listNovedades;
            }
            catch (SqlException ErrSQL)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));

                if (ErrSQL.Number == 1205 || ErrSQL.Number == 1204)
                {
                    throw new ApplicationException("Interbloqueo");
                }
                else
                {
                    throw ErrSQL;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en Novedades_TTxEstado", err);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region Novedades_Traer_PendientesAprobacion_Agrupada

        public static List<Novedad_SinAprobar> Novedades_Traer_PendientesAprobacion_Agrupada(long? prestador, string oficina, short idEstadoReg,
                                                                                             DateTime? fechaDesde,
                                                                                             DateTime? fechaHasta,
                                                                                             bool entregaDocumentacionEnFGS,
                                                                                             out int total)
        {
            string sql = "Novedades_PendientesAprobacion_Agrupadas";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad_SinAprobar> listNovedades = new List<Novedad_SinAprobar>();
            DbParameterCollection dbParametros = null;
            total = 0;

            try
            {
                db.AddInParameter(dbCommand, "@prestador", DbType.Decimal, prestador);
                db.AddInParameter(dbCommand, "@oficina", DbType.String, oficina);
                db.AddInParameter(dbCommand, "@idestadoreg", DbType.Int16, idEstadoReg);
                db.AddInParameter(dbCommand, "@fechaDesde", DbType.DateTime, fechaDesde != DateTime.MinValue ? fechaDesde : null);
                db.AddInParameter(dbCommand, "@fechaHasta", DbType.DateTime, fechaHasta != DateTime.MinValue ? fechaHasta : null);
                db.AddInParameter(dbCommand, "@entregaDocumentacionEnFGS", DbType.Boolean, entregaDocumentacionEnFGS);
                db.AddOutParameter(dbCommand, "@total", DbType.Int32, total);

                dbParametros = dbCommand.Parameters;

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        Novedad_SinAprobar unaNovedad = new Novedad_SinAprobar(
                                                                               Int64.Parse(dr["idprestador"].ToString()),
                                                                               dr["RazonSocial"].ToString(),
                                                                               dr["nroSucursal"].ToString(),
                                                                               dr["denominacion"].ToString()
                                                                               , int.Parse(dr["cantidadSinAprobar"].ToString()),
                                                                               DateTime.Parse(dr["minimaFecNovedad"].ToString()),
                                                                               DateTime.Parse(dr["maxFecNovedad"].ToString())
                                                                               );
                        listNovedades.Add(unaNovedad);
                    }
                }
                total = int.Parse(dbParametros["@total"].Value.ToString());
                return listNovedades;
            }
            catch (SqlException ErrSQL)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));

                if (ErrSQL.Number == 1205 || ErrSQL.Number == 1204)
                {
                    throw new ApplicationException("Interbloqueo");
                }
                else
                {
                    throw ErrSQL;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en Novedades_PendientesAprobacion_Agrupadas", err);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        #endregion

        #region Novedades_Traer_AfilicianesXPrestador

        public static List<Novedad_Afiliaciones> Novedades_Traer_AfilicianesXPrestador(long? prestador, int codConceptoLiq, int tipoConcepto)
        {
            string sql = "Novedades_CantAfiliaciones_X_Prestador";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad_Afiliaciones> listNovedades = new List<Novedad_Afiliaciones>();
            DbParameterCollection dbParametros = null;

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, prestador);
                db.AddInParameter(dbCommand, "@codConceptoLiq", DbType.Int32, codConceptoLiq);
                db.AddInParameter(dbCommand, "@tipoConcepto", DbType.Int32, tipoConcepto);
                dbParametros = dbCommand.Parameters;

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        Novedad_Afiliaciones unaNovedad = new Novedad_Afiliaciones(Int64.Parse(dr["Idprestador"].ToString()),
                                                                                   dr["RazonSocial"].ToString(),
                                                                                   dr["CodSistema"].ToString(),
                                                                                   new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()), dr["DescConceptoLiq"].ToString()),
                                                                                   new TipoConcepto(short.Parse(dr["tipoConcepto"].ToString()), dr["DescTipoConcepto"].ToString()),
                                                                                   decimal.Parse(dr["importetotal"].ToString()),
                                                                                   decimal.Parse(dr["porcentaje"].ToString()),
                                                                                   decimal.Parse(dr["minPrimerMensual"].ToString()),
                                                                                   decimal.Parse(dr["maxPrimerMensual"].ToString()),
                                                                                   Int32.Parse(dr["Cantidad"].ToString()));
                        listNovedades.Add(unaNovedad);
                    }
                }

                return listNovedades;
            }
            catch (SqlException ErrSQL)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));

                if (ErrSQL.Number == 1205 || ErrSQL.Number == 1204)
                {
                    throw new ApplicationException("Interbloqueo");
                }
                else
                {
                    throw ErrSQL;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en Novedades_Traer_AfilicianesXPrestador", err);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        #endregion

        #region Novedades Baja TxIDNovedad FecBaja

        public static List<Novedad> Novedades_BajaTxIDNov_FecBaja(long idNovedad, string FechaBaja)
        {
            string sql = "Novedades_BajaTxIdNov_FecBaja";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad> listNovedad = new List<Novedad>();
            Novedad unaNovedad;

            try
            {
                db.AddInParameter(dbCommand, "@idNovedad", DbType.Int64, idNovedad);
                db.AddInParameter(dbCommand, "@FechaBaja", DbType.String, FechaBaja);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    dr.Read();

                    unaNovedad = new Novedad(long.Parse(dr["IdNovedad"].ToString()),
                                             DateTime.Parse(dr["FecNov"].ToString() + " " + dr["HoraNov"].ToString()),
                                             double.Parse(dr.GetValue("ImporteTotal").ToString()),
                                             byte.Parse(dr.GetValue("CantCuotas").ToString()),
                                             Single.Parse(dr["Porcentaje"].ToString()),
                                             dr["nrocomprobante"].ToString(),
                                             dr["MAC"].ToString());

                    unaNovedad.MontoPrestamo = dr["montoPrestamo"].Equals(DBNull.Value) ? 0 : double.Parse(dr.GetValue("montoPrestamo").ToString());
                    unaNovedad.CFTEAReal = dr["CFTEAReal"].Equals(DBNull.Value) ? 0 : double.Parse(dr.GetValue("CFTEAReal").ToString());
                    unaNovedad.TNA = dr["TNA"].Equals(DBNull.Value) ? 0 : double.Parse(dr.GetValue("TNA").ToString());

                    unaNovedad.UnPrestador.RazonSocial = dr["RazonSocial"].ToString();
                    unaNovedad.UnPrestador.Cuit = long.Parse(dr["CUIT"].ToString());

                    unaNovedad.UnBeneficiario = new Beneficiario(long.Parse(dr["IdBeneficiario"].ToString()),
                                                                 !Utilidades.esNumerico(dr["Cuil"].ToString()) ? 0 : long.Parse(dr["Cuil"].ToString()),
                                                                 dr["Documento"].ToString(),
                                                                 dr.GetString("ApellidoNombre"));
                    unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                                dr.GetString("DescConceptoLiq"));

                    unaNovedad.UnTipoConcepto.IdTipoConcepto = Byte.Parse(dr["TipoConcepto"].ToString());
                    unaNovedad.UnTipoConcepto.DescTipoConcepto = dr["DescTipoConcepto"].ToString();

                    unaNovedad.FechaBaja = dr["FechaEliminacion"].Equals(DBNull.Value) ? (DateTime?)null : DateTime.ParseExact(dr["FechaEliminacion"].ToString() + " " + dr["HoraEliminacion"].ToString(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    unaNovedad.UnAuditoria = new Auditoria(dr["Usuario"].ToString());
                    unaNovedad.UnEstadoReg = new Estado(dr["IdEstadoReg"].Equals(DBNull.Value) ? 0 : int.Parse(dr["IdEstadoReg"].ToString()), dr["DescripcionEstadoReg"].ToString());
                    unaNovedad.unaLista_Cuotas = mapeaCuotaBajaTxIdNov_FecBaja(dr);

                    listNovedad.Add(unaNovedad);
                }
                return listNovedad;
            }

            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }

        public static Novedad Novedades_BajaTxIdNovedad(long idNovedad)
        {
            string sql = "Novedades_BajaTxIdNovedad";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            Novedad unaNovedad;

            try
            {
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, idNovedad);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    dr.Read();

                    unaNovedad = new Novedad(long.Parse(dr["IdNovedad"].ToString()),
                                             DateTime.Parse(dr["FecNov"].ToString() + " " + dr["HoraNov"].ToString()),
                                             dr["ImporteTotal"].Equals(DBNull.Value) ? 0 : double.Parse(dr.GetValue("ImporteTotal").ToString()),
                                             dr["CantCuotas"].Equals(DBNull.Value) ? byte.Parse("0") : byte.Parse(dr.GetValue("CantCuotas").ToString()),
                                             Single.Parse(dr["Porcentaje"].ToString()),
                                             dr["nrocomprobante"].ToString(),
                                             dr["MAC"].ToString());

                    unaNovedad.MontoPrestamo = dr["montoPrestamo"].Equals(DBNull.Value) ? 0 : double.Parse(dr.GetValue("montoPrestamo").ToString());
                    unaNovedad.CFTEAReal = dr["CFTEAReal"].Equals(DBNull.Value) ? 0 : double.Parse(dr.GetValue("CFTEAReal").ToString());
                    unaNovedad.TNA = dr["TNA"].Equals(DBNull.Value) ? 0 : double.Parse(dr.GetValue("TNA").ToString());

                    unaNovedad.FechaInforme = dr["fechaInforme"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["fechaInforme"].ToString());
                   
                    unaNovedad.UnPrestador.RazonSocial = dr["RazonSocial"].ToString();
                    unaNovedad.UnPrestador.Cuit = long.Parse(dr["CUIT"].ToString());

                    unaNovedad.UnBeneficiario = new Beneficiario(long.Parse(dr["IdBeneficiario"].ToString()),
                                                                 !Utilidades.esNumerico(dr["Cuil"].ToString()) ? 0 : long.Parse(dr["Cuil"].ToString()),
                                                                 dr.GetString("ApellidoNombre"));
                    unaNovedad.UnBeneficiario.FFallecimiento = dr["ffallecimiento"].Equals(DBNull.Value) ? (DateTime?)null : DateTime.Parse(dr["ffallecimiento"].ToString());

                    unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                                dr.GetString("DescConceptoLiq"));

                    unaNovedad.UnTipoConcepto.IdTipoConcepto = Byte.Parse(dr["TipoConcepto"].ToString());
                    unaNovedad.UnTipoConcepto.DescTipoConcepto = dr["DescTipoConcepto"].ToString();

                    unaNovedad.IdEstadoSC = dr["idEstadoSC"].Equals(DBNull.Value) ? 0 : Int32.Parse(dr["idEstadoSC"].ToString());
                    unaNovedad.Descripcion = dr["descripcion"].Equals(DBNull.Value) ? string.Empty : dr["descripcion"].ToString();
                    unaNovedad.SaldoAmortizacion = dr["SaldoAmortizacionTotal"].Equals(DBNull.Value) ? 0 : decimal.Parse(dr["SaldoAmortizacionTotal"].ToString());
                    unaNovedad.EstadosNovedad = new List<EstadoNovedad>();
                    unaNovedad.EstadosNovedad.Add(CrearEstadoNovedad(dr));  
                    unaNovedad.UnTipoPolizaSeguro = new TipoPolizaSeguro(dr["idPolizaSeguro"].Equals(DBNull.Value) ? 0 : Int32.Parse(dr["idPolizaSeguro"].ToString()),
                                                                         dr["descripcionPolizaSeguro"].ToString());
                    unaNovedad.unaLista_Cuotas = mapeaCuotaBajaTxIdNovedad(dr, unaNovedad);

                    dr.NextResult();
                    while (dr.Read())
                    {
                        CancelacionAnticipada cancelacionAnticipada = new CancelacionAnticipada();
                        cancelacionAnticipada.FechaCobroFGS = DateTime.Parse(dr["fCobroFGS"].ToString());
                        cancelacionAnticipada.Importe = decimal.Parse(dr["importeCancelacion"].ToString());
                        unaNovedad.CancelacionAnticipada.Add(cancelacionAnticipada);

                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        SiniestroCobrado siniestroCobrado = new SiniestroCobrado();
                        siniestroCobrado.FechaCobroFGS = dr["fCobroFGS"].Equals(DBNull.Value) ? null : (DateTime?)DateTime.Parse(dr["fCobroFGS"].ToString());
                        siniestroCobrado.Importe = dr["importeSiniestro"].Equals(DBNull.Value) ? 0 : decimal.Parse(dr["importeSiniestro"].ToString());
                        siniestroCobrado.fSolicitudCobro = dr["fSolicitudCobro"].Equals(DBNull.Value) ? null : (DateTime?)DateTime.Parse(dr["fSolicitudCobro"].ToString());
                        siniestroCobrado.idLote = dr["idLote"].Equals(DBNull.Value) ? 0 : int.Parse(dr["idLote"].ToString());
                        siniestroCobrado.idNovedad = long.Parse(dr["idNovedad"].ToString());
                        siniestroCobrado.idSiniestro = long.Parse(dr["idSiniestro"].ToString());
                        unaNovedad.SiniestroCobrado.Add(siniestroCobrado);
                    }
                }

                return unaNovedad;
            }

            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }

        private static EstadoNovedad CrearEstadoNovedad(NullableDataReader dr)
        {
            EstadoNovedad estadoNovedad = new EstadoNovedad();
            estadoNovedad.Descripcion = dr["descripcion"].Equals(DBNull.Value) ? string.Empty : dr["descripcion"].ToString();
            estadoNovedad.IdEstadoSC = dr["idEstadoSC"].Equals(DBNull.Value) ? 0 : Int32.Parse(dr["idEstadoSC"].ToString());
            estadoNovedad.SaldoAmortizacionTotal = dr["SaldoAmortizacionTotal"].Equals(DBNull.Value) ? 0 : decimal.Parse(dr["SaldoAmortizacionTotal"].ToString());
            return estadoNovedad;
        }

        private static List<Cuota> mapeaCuotaBajaTxIdNov_FecBaja(NullableDataReader dr)
        {
            List<Cuota> lista = new List<Cuota>();

            do
            {
                Cuota c = new Cuota(long.Parse(dr["IdNovedad"].ToString()),
                                   dr["Mensualcuota"].ToString(),
                                   dr["NroCuota"].Equals(DBNull.Value) ? 0 : int.Parse(dr["NroCuota"].ToString()),
                                   dr["ImporteCuota"].Equals(DBNull.Value) ? 0 : double.Parse(dr["ImporteCuota"].ToString()),
                                   dr["amortizacion"].Equals(DBNull.Value) ? 0 : double.Parse(dr["amortizacion"].ToString()),
                                   dr["importeInteres"].Equals(DBNull.Value) ? 0 : double.Parse(dr["importeInteres"].ToString()),
                                   dr["gastoAdminTarjeta"].Equals(DBNull.Value) ? 0 : double.Parse(dr["gastoAdminTarjeta"].ToString()),
                                   dr["gastoAdmMensualCalc"].Equals(DBNull.Value) ? 0 : double.Parse(dr["gastoAdmMensualCalc"].ToString()),
                                   dr["seguroVida"].Equals(DBNull.Value) ? 0 : double.Parse(dr["seguroVida"].ToString()));

                lista.Add(c);
            }
            while (dr.Read());
            return lista;
        }

        private static List<Cuota> mapeaCuotaBajaTxIdNovedad(NullableDataReader dr, Novedad unaNovedad)
        {
            try
            {
                List<Cuota> lista = new List<Cuota>();

                do
                {
                    Cuota c = new Cuota(long.Parse(dr["IdNovedad"].ToString()),
                                       dr["Mensualcuota"].ToString(),
                                       dr["NroCuota"].Equals(DBNull.Value) ? 0 : int.Parse(dr["NroCuota"].ToString()),
                                       dr["ImporteCuota"].Equals(DBNull.Value) ? 0 : double.Parse(dr["ImporteCuota"].ToString()),
                                       dr["amortizacion"].Equals(DBNull.Value) ? 0 : double.Parse(dr["amortizacion"].ToString()),
                                       dr["importeInteres"].Equals(DBNull.Value) ? 0 : double.Parse(dr["importeInteres"].ToString()),
                                       dr["gastoAdminTarjeta"].Equals(DBNull.Value) ? 0 : double.Parse(dr["gastoAdminTarjeta"].ToString()),
                                       dr["gastoAdmMensualCalc"].Equals(DBNull.Value) ? 0 : double.Parse(dr["gastoAdmMensualCalc"].ToString()),
                                       dr["seguroVida"].Equals(DBNull.Value) ? 0 : double.Parse(dr["seguroVida"].ToString()),
                                       !dr["fbaja"].Equals(DBNull.Value) ? enum_enviadoLiquidar.B : (enum_enviadoLiquidar)Enum.Parse(typeof(enum_enviadoLiquidar), dr["fueLiquidado"].ToString()),
                                       dr["idMensaje"].ToString(),
                                       dr["Mensaje"].ToString(),
                                       dr["mensualEmision"].Equals(DBNull.Value) ? 0 : int.Parse(dr["mensualEmision"].ToString()),
                                       dr["tipoLiq"].Equals(DBNull.Value) ? String.Empty : dr["tipoLiq"].ToString(),
                                       dr["SaldoAmortizacion"].Equals(DBNull.Value) ? 0 : double.Parse(dr["SaldoAmortizacion"].ToString()),
                                       dr["ImporteCuotaLiq"].Equals(DBNull.Value) ? 0 : double.Parse(dr["ImporteCuotaLiq"].ToString()),
                                       long.Parse(dr["idbeneficiario"].ToString()),
                                       dr["CodConceptoLiq"].Equals(DBNull.Value) ? 0 : int.Parse(dr["CodConceptoLiq"].ToString()),
                                       dr["DescripcionIdentPago"].Equals(DBNull.Value) ? String.Empty : dr["DescripcionIdentPago"].ToString(),
                                       dr["identpago"].Equals(DBNull.Value) ? string.Empty : dr["identpago"].ToString(),
                                       dr["idEstadoRub"].Equals(DBNull.Value) ? 0 : int.Parse(dr["idEstadoRub"].ToString()),
                                       dr["descEstadoRub"].Equals(DBNull.Value) ? string.Empty : dr["descEstadoRub"].ToString(),
                                       dr["daEstadoRub"].Equals(DBNull.Value) ? string.Empty : dr["daEstadoRub"].ToString(),
                                       dr["amortizacionDescontadaCuota"].Equals(DBNull.Value) ? 0 : decimal.Parse(dr["amortizacionDescontadaCuota"].ToString()),
                                       dr["FecCierreCuota"].Equals(DBNull.Value) ? DateTime.MinValue : DateTime.Parse(dr["FecCierreCuota"].ToString()),
                                       dr["FecCierreUltEmision"].Equals(DBNull.Value) ? DateTime.MinValue : DateTime.Parse(dr["FecCierreUltEmision"].ToString()),
                                       dr["interesCuotaCero"].Equals(DBNull.Value) ? 0 : double.Parse(dr["interesCuotaCero"].ToString())
                                       );

                    if (unaNovedad.FechaBaja == null && !dr["fbaja"].Equals(DBNull.Value))
                    {
                        unaNovedad.FechaBaja = (dr["fbaja"].Equals(DBNull.Value) ? (DateTime?)null : DateTime.Parse(dr["fbaja"].ToString()));
                        unaNovedad.UnAuditoria = new Auditoria(dr["usuarioBaja"].ToString());
                        unaNovedad.UnEstadoReg = new Estado(dr["IdEstadoReg"].Equals(DBNull.Value) ? 0 : int.Parse(dr["IdEstadoReg"].ToString()), dr["DescripcionEstadoReg"].ToString());//DescripcionEstadoReg
                    }
                    if (!unaNovedad.EstadosNovedad.Exists(x => x.IdEstadoSC == Int32.Parse(dr["idEstadoSC"].ToString())))
                    {
                        unaNovedad.EstadosNovedad.Add(CrearEstadoNovedad(dr));
                    }
                    lista.Add(c);
                }
                while (dr.Read());
                return lista;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw;
            }
        }
        #endregion

        #region Novedades de Baja Traer Agrupada

        public static List<Novedad> Novedades_BajaTraerAgrupadaConsulta(long idPrestador, byte OpcionBusqueda, long BenefCuil,
                                                                         byte TipoConc, int ConcOpp, string MesAplica)
        {
            string sql = "Novedades_BajaT_Agrupadas";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad> listNovedad = new List<Novedad>();
            Novedad unaNovedad;

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@Opcion", DbType.Byte, OpcionBusqueda);
                db.AddInParameter(dbCommand, "@BenefCuil", DbType.Int64, BenefCuil);
                db.AddInParameter(dbCommand, "@TipoConc", DbType.Byte, TipoConc);
                db.AddInParameter(dbCommand, "@Conc", DbType.Int64, ConcOpp);
                db.AddInParameter(dbCommand, "@Conc", DbType.String, MesAplica);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        //HoraNov                     
                        //NroCuota 
                        //ImporteCuota
                        //Mensualcuota 
                        //PrimerMensual 
                        //Usuario 
                        //FechaEliminacion 
                        //FechaEliminacion2 
                        //HoraEliminacion
                        unaNovedad = new Novedad(dr.GetInt64("IdNovedad"),
                                                 DateTime.Parse(dr.GetString("FecNov")),
                                                 double.Parse(dr.GetValue("ImporteTotal").ToString()),
                                                 dr.GetByte("CantCuotas"),
                                                 Single.Parse(dr["Porcentaje"].ToString()),
                                                 dr.GetString("NroComprobante"),
                                                 dr.GetString("MAC"),
                                                 dr.GetNullableDateTime("FecImportacion"),
                                                 dr.GetString("PrimerMensual"),
                                                 dr.GetBoolean("Stock"), null);


                        unaNovedad.MAC = dr.GetString("MAC");
                        unaNovedad.FechaImportacion = dr.GetNullableDateTime("FecImportacion");

                        unaNovedad.UnTipoConcepto = new TipoConcepto(Byte.Parse(dr["TipoConcepto"].ToString()),
                                                                     dr.GetString("DescTipoConcepto"));
                        unaNovedad.UnPrestador = new Prestador(0, string.Empty, 0);
                        unaNovedad.UnBeneficiario = new Beneficiario(long.Parse(dr["IdBeneficiario"].ToString()),
                                                                     long.Parse(dr["Cuil"].ToString()),
                                                                     dr.GetString("ApellidoNombre"));
                        unaNovedad.UnBeneficiario.NroDoc = dr.GetString("Documento");
                        unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                                   dr.GetString("DescConceptoLiq"));
                        unaNovedad.UnEstadoReg = new Estado(dr.GetInt32("IdEstadoReg"),
                                                            dr.GetString("DescripcionEstadoReg"));

                        listNovedad.Add(unaNovedad);
                    }
                }
                return listNovedad;
            }
            catch (SqlException ErrSQL)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));

                if (ErrSQL.Number == 1205 || ErrSQL.Number == 1204)
                {
                    throw new ApplicationException("Interbloqueo");
                }
                else
                {
                    throw ErrSQL;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }

        public static List<Novedad> Novedades_BajaTraerAgrupada(long idPrestador, byte opcionBusqueda,
                                                                long benefCuil, byte tipoConc, int concopp,
                                                                DateTime fdesde, DateTime fhasta, bool GeneraArchivo,
                                                                string mensual, bool generadoAdmin, out string rutaArchivoSal)
        {
            string rutaArchivo = string.Empty;
            string nombreArchivo = string.Empty;
            rutaArchivoSal = string.Empty;
            string msgRta = string.Empty;
                 
            ConsultaBatch consultaBatch = new ConsultaBatch();
            consultaBatch.NombreConsulta = ConsultaBatch.enum_ConsultaBatch_NombreConsulta.NOVEDADES_BAJAT_AGRUPADAS;
            consultaBatch.IDPrestador = idPrestador;
            consultaBatch.OpcionBusqueda = opcionBusqueda;
            consultaBatch.PeriodoCons = mensual;
            consultaBatch.UnConceptoLiquidacion = new ConceptoLiquidacion(concopp, string.Empty, new TipoConcepto(tipoConc, string.Empty));
            consultaBatch.NroBeneficio = benefCuil;
            consultaBatch.GeneradoAdmin = generadoAdmin;
            consultaBatch.FechaDesde = fdesde;
            consultaBatch.FechaHasta = fhasta;

            try
            {
                if (opcionBusqueda != 1 || GeneraArchivo == true)
                {
                    msgRta = ConsultasBatchDAO.ExisteConsulta(consultaBatch);
                    if (!string.IsNullOrEmpty(msgRta))
                    {
                        throw new ApplicationException("MSG_ERROR" + msgRta + "FIN_MSG_ERROR");
                    }
                }

                List<Novedad> listNovedades = Novedades_Trae_NoAplicadasConsulta(opcionBusqueda, idPrestador, benefCuil,
                                                                                 tipoConc, concopp, fdesde, fhasta);

                if ((listNovedades.Count > 0) && (opcionBusqueda != 1 || GeneraArchivo == true))
                {
                    int maxCantidad = Settings.MaxCantidadRegistros();

                    if (listNovedades.Count >= maxCantidad || GeneraArchivo == true)
                    {
                        nombreArchivo = Utilidades.GeneraNombreArchivo(consultaBatch.NombreConsulta.ToString(), idPrestador, out rutaArchivo);
                        StreamWriter sw = new StreamWriter(rutaArchivo + nombreArchivo, false, Encoding.UTF8);
                        string separador = Settings.DelimitadorCampo();

                        foreach (Novedad oNovedad in listNovedades)
                        {
                            StringBuilder linea = new StringBuilder();

                            linea.Append(oNovedad.UnBeneficiario.Cuil.ToString() + separador);
                            linea.Append(oNovedad.IdNovedad.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.IdBeneficiario.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.NroDoc + separador);
                            linea.Append(oNovedad.UnBeneficiario.ApellidoNombre + separador);
                            linea.Append(oNovedad.FechaNovedad.ToString("dd/MM/yyyy HH:mm:ss") + separador);
                            linea.Append(oNovedad.UnTipoConcepto.IdTipoConcepto.ToString() + separador);
                            linea.Append(oNovedad.UnTipoConcepto.DescTipoConcepto.ToString() + separador);
                            linea.Append(oNovedad.UnConceptoLiquidacion.CodConceptoLiq.ToString() + separador);
                            linea.Append(oNovedad.UnConceptoLiquidacion.DescConceptoLiq.ToString() + separador);
                            linea.Append(oNovedad.ImporteTotal.ToString().Replace(",", ".") + separador);
                            linea.Append(oNovedad.CantidadCuotas.ToString() + separador);
                            linea.Append(oNovedad.Porcentaje.ToString().Replace(",", ".") + separador);
                            linea.Append(oNovedad.NroCuotaLiquidada.ToString() + separador);
                            linea.Append(oNovedad.MensualCuota.ToString() + separador);
                            linea.Append(oNovedad.Comprobante.ToString() + separador);
                            linea.Append(oNovedad.MAC.ToString() + separador);
                            linea.Append(oNovedad.ImporteCuota.ToString() + separador);
                            linea.Append(oNovedad.ImporteLiquidado.ToString() + separador);
                            linea.Append(oNovedad.UnEstadoReg.IdEstado.ToString() + separador);
                            linea.Append(oNovedad.UnEstadoReg.DescEstado + separador);
                            linea.Append(oNovedad.UnEstadoNovedad.IdEstado.ToString() + separador);
                            linea.Append(oNovedad.Stock.ToString() + separador);
                            linea.Append((!oNovedad.FechaBaja.HasValue ? string.Empty : oNovedad.FechaBaja.Value.ToString("dd/MM/yyyy HH:mm:ss")) + separador);
                            linea.Append(oNovedad.UnCodMovimiento.CodMovimiento.ToString() + separador);
                            linea.Append(oNovedad.UnAuditoria.Usuario.ToString() + separador);

                            sw.WriteLine(linea.ToString());
                        }
                        sw.Close();

                        Utilidades.ComprimirArchivo(rutaArchivo, nombreArchivo);
                        Utilidades.BorrarArchivo(rutaArchivo, nombreArchivo);

                        nombreArchivo = nombreArchivo + ".zip";
                        rutaArchivoSal = rutaArchivo + nombreArchivo;
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
                    }
                }

                return listNovedades;
            }
            catch (SqlException errsql)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), errsql.Source, errsql.Message));

                if (errsql.Number == -2)
                {
                    nombreArchivo = Utilidades.GeneraNombreArchivo(consultaBatch.NombreConsulta.ToString(), idPrestador, out rutaArchivo);
                    consultaBatch.NomArchGenerado = nombreArchivo;
                    consultaBatch.RutaArchGenerado = rutaArchivo;
                    consultaBatch.FechaGenera = DateTime.MinValue;
                    consultaBatch.Vigente = false;

                    msgRta = ConsultasBatchDAO.AltaNuevaConsulta(consultaBatch);

                    throw new ApplicationException("MSG_ERROR Generando el archivo. Reingrese a la consulta en unos minutos.FIN_MSG_ERROR");
                }
                else
                    throw errsql;
            }
            catch (ApplicationException apperr)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), apperr.Source, apperr.Message));
                throw new ApplicationException(apperr.Message);
            }
            catch (Exception err)
            {
                throw err;
            }
        }

        #endregion

        #region Novedades Baja Con Desaf Monto

        public static void Novedades_B_Con_Desaf_Monto(long idNovedad,
                                                       int idEstadoReg,
                                                       string Mac,
                                                       string ip,
                                                       string usuario,
                                                       bool cierre)
        {
            string sql;
            if (cierre)
                sql = "Novedades_B_Con_Desaf_Monto_Al_Cierre";
            else
                sql = "Novedades_B_Con_Desaf_Monto";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, idNovedad);
                db.AddInParameter(dbCommand, "@IdEstadoReg", DbType.Int16, idEstadoReg);
                db.AddInParameter(dbCommand, "@MAC", DbType.String, Mac);
                db.AddInParameter(dbCommand, "@IP", DbType.String, ip);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, usuario);

                using (TransactionScope scope = new TransactionScope())
                {
                    db.ExecuteNonQuery(dbCommand);
                    scope.Complete();
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }

        #endregion

        #region Novedades Baja T3 con Control Vto

        public static void Novedades_BAJA_T3_ControlVencimiento(long idNovedad,
                                                                int MensualDesde,
                                                                enum_tipoestadoNovedad idEstadoReg,
                                                                string Mac,
                                                                string ip,
                                                                string usuario,
                                                                out string mensaje)
        {
            string sql = "Novedades_BAJA_T3_ControlVencimiento";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;

            try
            {
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, idNovedad);
                db.AddInParameter(dbCommand, "@MensualDesde", DbType.Int32, (int)MensualDesde);
                db.AddInParameter(dbCommand, "@IdEstadoReg", DbType.Int16, (int)idEstadoReg);
                db.AddInParameter(dbCommand, "@MAC", DbType.String, Mac);
                db.AddInParameter(dbCommand, "@IP", DbType.String, ip);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, usuario);
                db.AddOutParameter(dbCommand, "@mensaje", DbType.String, 100);

                using (TransactionScope scope = new TransactionScope())
                {
                    dbParametros = dbCommand.Parameters;
                    db.ExecuteNonQuery(dbCommand);
                    scope.Complete();
                    mensaje = dbParametros[06].Value.ToString();
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }

        #endregion


        #region Controles

        #region Novedad Valido Derecho

        public static string ValidoDerecho(long idPrestador, long idBeneficiario, short tipoConcepto,
                                           int codConceptoLiq, double importeTotal, byte cantCuotas,
                                           Single porcentaje, byte codMovimiento, String nroComprobante)
        {

            string mensaje = String.Empty;
            string sql = "Novedad_Valido_Derecho";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;

            try
            {

                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Int16, tipoConcepto);
                db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, codConceptoLiq);
                db.AddInParameter(dbCommand, "@CodMovimiento", DbType.Byte, codMovimiento);
                db.AddInParameter(dbCommand, "@ImporteTotal", DbType.Decimal, importeTotal);
                db.AddInParameter(dbCommand, "@CantCuotas", DbType.Byte, cantCuotas);
                db.AddInParameter(dbCommand, "@Porcentaje", DbType.Decimal, porcentaje);
                db.AddInParameter(dbCommand, "@NroComprobante", DbType.String, nroComprobante);
                db.AddOutParameter(dbCommand, "@Mensaje", DbType.String, 100);
                db.AddOutParameter(dbCommand, "@EsAfiliacion", DbType.Boolean, 1);

                dbParametros = dbCommand.Parameters;
                db.ExecuteNonQuery(dbCommand);

                mensaje = dbParametros[09].Value.ToString() + '|' + dbParametros[10].Value.ToString();

                if (mensaje != String.Empty) { mensaje += "|"; }

                return mensaje;
            }

            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en NovedadDAO.ValidoDerecho", err);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
                dbParametros = null;
            }
        }
        #endregion

        #region CtrolAlcanza

        public static string CtrolAlcanza(long idBeneficiario, double importe, long idPrestador, int codConceptoLiq)
        {
            // controla si alcanza el monto a ingresar - si no alcanza ingresa el monto en rechazados		
            string mensaje = String.Empty;
            string sql = "AlcanzaAfectacion";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                #region parametros
                db.AddInParameter(dbCommand, "@idBeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@monto", DbType.Decimal, importe);
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@ConceptoOPP", DbType.Int32, codConceptoLiq);
                db.AddOutParameter(dbCommand, "@alcanza", DbType.Byte, 4);
                #endregion parametros

                db.ExecuteNonQuery(dbCommand);

                if (Byte.Parse(db.GetParameterValue(dbCommand, "@alcanza").ToString()) == 0)
                {
                    mensaje = "Afectación Disponible Insuficiente";
                }

                return mensaje;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en NovedadDAO.CtrolAlcanza", err);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region Valido Novedad

        #region Creo que nadie lo usa
        //Esto es = que Valido_Novedad
        private static string Valido_Nov(Novedad unaNovedad)
        {

            string mensaje = String.Empty;

            try
            {
                mensaje = Valido_Novedad(unaNovedad.UnPrestador.ID, unaNovedad.UnBeneficiario.IdBeneficiario,
                                        unaNovedad.UnTipoConcepto.IdTipoConcepto, unaNovedad.UnConceptoLiquidacion.CodConceptoLiq,
                                        unaNovedad.ImporteTotal, unaNovedad.CantidadCuotas, unaNovedad.Porcentaje,
                                        unaNovedad.UnCodMovimiento.CodMovimiento, unaNovedad.Comprobante);
                return mensaje;
            }

            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {

            }
        }
        #endregion

        private static string Valido_Novedad(long idPrestador, long idBeneficiario, short tipoConcepto, int codConceptoLiq, double importeTotal,
                                            byte cantCuotas, Single porcentaje, byte codMovimiento, String nroComprobante)
        {
            string sql = "Novedad_Valido_Derecho";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            string mensaje = string.Empty;

            try
            {
                //CONTROLA MAXIMOS INTENTOS 
                mensaje = CtrolIntentos(idPrestador, idBeneficiario);

                //CONTROLA QUE ESTE INFORMADO EL COMPROBANTE
                if ((mensaje == String.Empty) && (nroComprobante.Trim().Length < 4))
                    mensaje = "El nro. de comprobante debe ser mayor a 3 dígitos.";

                //CONTROLA TIPOS DE CAMPOS
                if (mensaje == String.Empty)
                    mensaje = CtrolMontos(tipoConcepto, codConceptoLiq, cantCuotas, porcentaje);

                //VALIDA LA NOVEDAD
                if (mensaje == String.Empty)
                {
                    db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                    db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                    db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Int16, tipoConcepto);
                    db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, codConceptoLiq);
                    db.AddInParameter(dbCommand, "@CodMovimiento", DbType.Int16, codMovimiento);
                    db.AddInParameter(dbCommand, "@ImporteTotal", DbType.Decimal, importeTotal);
                    db.AddInParameter(dbCommand, "@CantCuotas", DbType.Int16, cantCuotas);
                    db.AddInParameter(dbCommand, "@Porcentaje", DbType.Decimal, porcentaje);
                    db.AddInParameter(dbCommand, "@NroComprobante", DbType.String, nroComprobante);
                    db.AddOutParameter(dbCommand, "@Mensaje", DbType.String, 100);
                    db.AddOutParameter(dbCommand, "@EsAfiliacion", DbType.Boolean, 1);
                    dbParametros = dbCommand.Parameters;

                    db.ExecuteNonQuery(dbCommand);

                    mensaje = db.GetParameterValue(dbCommand, "@Mensaje").ToString() + '|' +
                              bool.Parse(db.GetParameterValue(dbCommand, "@EsAfiliacion").ToString());
                }

                if (mensaje != String.Empty)
                    mensaje += "|";

                return mensaje;
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

        #endregion


        #region Ctrol Montos
        private static string CtrolMontos(short tipoConcepto, double impTotal, byte cantCuotas, Single porcentaje)
        {
            string mensajeMontos = String.Empty;
            switch (tipoConcepto)
            {
                case 1:
                case 2:
                    if (impTotal <= 0) mensajeMontos = @"El campo Importe debe ser mayor a 0";
                    break;
                case 3:
                    if (impTotal <= 0) { mensajeMontos = @"El importe resultante de resta la cuota total  y cuota afiliación debe ser mayor a CERO (0)"; }

                    if (mensajeMontos == String.Empty) if (cantCuotas <= 0 || cantCuotas > 240) mensajeMontos = @"El campo Cant. Cuotas debe estar comprendido entre 1 y 240";
                    break;
                case 6:
                    if (porcentaje <= 0 || porcentaje > 100) mensajeMontos = @"El campo porcentaje debe ser mayor que 0 y menor a 100";
                    break;
                default:
                    mensajeMontos = @"Opción no contemplada";
                    break;
            }
            return mensajeMontos;
        }
        #endregion

        #region Ctrol Cantidad Rechazos

        private static int CtrolCantRechazos(long idPrestador, long idBeneficiario)
        {

            string sql = "NovRechazadas_TCant";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@idBeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                db.AddOutParameter(dbCommand, "@CantRech", DbType.Byte, 3);

                db.ExecuteNonQuery(dbCommand);

                return (int.Parse(db.GetParameterValue(dbCommand, "@CantRech").ToString()));
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en NovedadDAO.CtrolCantRechazos", err);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

        }
        #endregion

        #region Ctrol Intentos

        private static string CtrolIntentos(long idPrestador, long idBeneficiario)
        {
            string mensaje = String.Empty;
            try
            {
                int MaxIntentos = int.Parse(ConfigurationManager.AppSettings["DAT_MaxIntentos"].ToString());
                int MaxCantRechazos = CtrolCantRechazos(idPrestador, idBeneficiario);

                mensaje = (MaxCantRechazos >= MaxIntentos) ? "Máxima cantidad de intentos permitidos" : String.Empty;
                return (mensaje);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
        }
        #endregion

        #endregion Controles

        #endregion No transaccional

        #region  Valido_Nov_T3
        public static string Valido_Nov_T3(long IdPrestador, long IdBeneficiario, byte TipoConcepto, int ConceptoOPP, double ImpTotal, byte CantCuotas,
                                    Single Porcentaje, byte CodMovimiento, String NroComprobante, DateTime FecNovedad, string IP, string Usuario, int Mensual,
                                    decimal montoPrestamo, decimal CuotaTotalMensual, decimal TNA, decimal TEM, decimal gastoOtorgamiento, decimal gastoAdmMensual,
                                    decimal cuotaSocial, decimal CFTEA, decimal CFTNAReal, decimal CFTEAReal, decimal gastoAdmMensualReal, decimal TIRReal)
        {

            string mensaje = String.Empty;
            double Importe = 0;

            mensaje = Valido_Novedad(IdPrestador, IdBeneficiario, TipoConcepto, ConceptoOPP, ImpTotal, CantCuotas, Porcentaje, CodMovimiento, NroComprobante);

            mensaje = mensaje.Split(char.Parse("|"))[0].ToString().Trim();

            if (mensaje == String.Empty)
            {
                Importe = ImpTotal / CantCuotas;
                mensaje = CtrolAlcanza(IdBeneficiario, Importe, IdPrestador, ConceptoOPP);

            }

            //09/01/12 - $3b@
            if (mensaje != String.Empty)
            {
                Novedades_Rechazadas_A_ConTasas(IdBeneficiario, IdPrestador, CodMovimiento, TipoConcepto, ConceptoOPP, ImpTotal, CantCuotas,
                                                Porcentaje, NroComprobante, IP, Usuario, FecNovedad, montoPrestamo, CuotaTotalMensual, TNA, TEM,
                                                gastoOtorgamiento, gastoAdmMensual, cuotaSocial, CFTEA, CFTNAReal, CFTEAReal, gastoAdmMensualReal,
                                                TIRReal, mensaje);

            }

            return mensaje;

        }

        #endregion

        #region Novedades_T3_Alta_ConTasa
        public static string Novedades_T3_Alta_ConTasa(long IdPrestador, long IdBeneficiario, DateTime FecNovedad, byte TipoConcepto, int ConceptoOPP,
                                                double ImpTotal, byte CantCuotas, string NroComprobante, string IP, string Usuario, int Mensual, byte IdEstadoReg,
                                                decimal montoPrestamo, decimal CuotaTotalMensual, decimal TNA, decimal TEM,
                                                decimal gastoOtorgamiento, decimal gastoAdmMensual, decimal cuotaSocial, decimal CFTEA,
                                                decimal CFTNAReal, decimal CFTEAReal, decimal gastoAdmMensualReal, decimal TIRReal, string XMLCuotas,
                                                int idItem, string nroFactura, string cbu, string otro, string prestadorServicio, string poliza,
                                                string nroSocio, int idDomicilioBeneficiario, int idDomicilioPrestador)
        {


            try
            {
                String[] alta = new String[2];
                string mensaje = String.Empty;
                string retorno = String.Empty;
                byte CodMovimiento = 6;
                string MAC = string.Empty;

                mensaje = Valido_Nov_T3(IdPrestador, IdBeneficiario, TipoConcepto, ConceptoOPP, ImpTotal, CantCuotas, 0, CodMovimiento, NroComprobante,
                                       FecNovedad, IP, Usuario, Mensual, montoPrestamo, CuotaTotalMensual, TNA, TEM, gastoOtorgamiento, gastoAdmMensual,
                                       cuotaSocial, CFTEA, CFTNAReal, CFTEAReal, gastoAdmMensualReal, TIRReal);


                if (mensaje == String.Empty)
                {
                    double Importe = ImpTotal / CantCuotas;
                    ModificaSaldo(IdPrestador, IdBeneficiario, ConceptoOPP, Importe, Usuario);

                    alta = Novedades_Alta_Fisica_Tipo3_ConTasa(IdBeneficiario, IdPrestador, ConceptoOPP, ImpTotal, montoPrestamo, CantCuotas,
                                                               CuotaTotalMensual, TNA, TEM, gastoOtorgamiento, gastoAdmMensual, cuotaSocial,
                                                               CFTEA, CFTNAReal, CFTEAReal, gastoAdmMensualReal, TIRReal, NroComprobante, IP,
                                                              Usuario, Mensual, XMLCuotas, CodMovimiento, FecNovedad, TipoConcepto, IdEstadoReg,
                                                              idItem, nroFactura, cbu, otro, prestadorServicio, poliza, nroSocio,
                                                              idDomicilioBeneficiario, idDomicilioPrestador);

                    retorno = " |" + alta[0].ToString() + "|" + alta[1].ToString();
                }
                else
                {
                    retorno = mensaje + "|0| ";
                }
                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {

            }
        }
        #endregion

        #region Novedades_Alta_Fisica_Tipo3_ConTasa

        private static String[] Novedades_Alta_Fisica_Tipo3_ConTasa(long IdBeneficiario, long IdPrestador, int CodConceptoLiq, double importeTotal,
                                                            decimal montoPrestamo, byte CantCuotas, decimal CuotaTotalMensual, decimal TNA,
                                                            decimal TEM, decimal gastoOtorgamiento, decimal gastoAdmMensual, decimal cuotaSocial,
                                                            decimal CFTEA, decimal CFTNAReal, decimal CFTEAReal, decimal gastoAdmMensualReal,
                                                            decimal TIRReal, string NroComprobante, string IP, string Usuario, int PrimerMensual,
                                                            string cuotas, byte CodMovimiento, DateTime FecNovedad, byte TipoConcepto, byte IdEstadoReg,
                                                            int idItem, string nroFactura, string cbu, string otro, string prestadorServicio, string poliza,
                                                            string nroSocio, int idDomicilioBeneficiario, int idDomicilioPrestador)
        {

            string dato = Genera_Datos_para_MAC(IdBeneficiario, IdPrestador, FecNovedad, CodMovimiento, CodConceptoLiq, TipoConcepto,
                                                importeTotal, CantCuotas, 0, NroComprobante, IP, Usuario);

            string MAC = Utilidades.Calculo_MAC(dato);

            String[] retorno = new String[2];

            string sql = "Novedades_Tipo3_AltaConTasas";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {

                db.AddInParameter(dbCommand, "@idbeneficiario", DbType.Int64, IdBeneficiario);
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, IdPrestador);
                db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, CodConceptoLiq);
                db.AddInParameter(dbCommand, "@importeTotal", DbType.Double, importeTotal);
                db.AddInParameter(dbCommand, "@montoPrestamo", DbType.Decimal, montoPrestamo);
                db.AddInParameter(dbCommand, "@CantCuotas", DbType.Byte, CantCuotas);
                db.AddInParameter(dbCommand, "@CuotaTotalMensual", DbType.Decimal, CuotaTotalMensual);
                db.AddInParameter(dbCommand, "@TNA", DbType.Decimal, TNA);
                db.AddInParameter(dbCommand, "@TEM", DbType.Decimal, TEM);
                db.AddInParameter(dbCommand, "@gastoOtorgamiento", DbType.Decimal, gastoOtorgamiento);
                db.AddInParameter(dbCommand, "@gastoAdmMensual", DbType.Decimal, gastoAdmMensual);
                db.AddInParameter(dbCommand, "@cuotaSocial", DbType.Decimal, cuotaSocial);
                db.AddInParameter(dbCommand, "@CFTEA", DbType.Decimal, CFTEA);
                db.AddInParameter(dbCommand, "@CFTNAReal", DbType.Decimal, CFTNAReal);
                db.AddInParameter(dbCommand, "@CFTEAReal", DbType.Decimal, CFTEAReal);
                db.AddInParameter(dbCommand, "@gastoAdmMensualReal", DbType.Decimal, gastoAdmMensualReal);
                db.AddInParameter(dbCommand, "@TIRReal", DbType.Decimal, TIRReal);
                db.AddInParameter(dbCommand, "@NroComprobante", DbType.String, NroComprobante);
                db.AddInParameter(dbCommand, "@MAC", DbType.String, MAC);
                db.AddInParameter(dbCommand, "@IP", DbType.String, IP);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, Usuario);
                db.AddInParameter(dbCommand, "@PrimerMensual", DbType.Int32, PrimerMensual);
                db.AddInParameter(dbCommand, "@cuotas", DbType.String, cuotas);
                db.AddInParameter(dbCommand, "@IdEstadoReg", DbType.Byte, IdEstadoReg);
                db.AddInParameter(dbCommand, "@idItem", DbType.Int32, idItem);
                db.AddInParameter(dbCommand, "@nroFactura", DbType.String, nroFactura);
                db.AddInParameter(dbCommand, "@cbu", DbType.String, cbu);
                db.AddInParameter(dbCommand, "@otro", DbType.String, otro);
                db.AddInParameter(dbCommand, "@prestadorServicio", DbType.String, prestadorServicio);
                db.AddInParameter(dbCommand, "@poliza", DbType.String, poliza);
                db.AddInParameter(dbCommand, "@nroSocio", DbType.String, nroSocio);
                db.AddInParameter(dbCommand, "@idDomicilioBeneficiario", DbType.Int32, idDomicilioBeneficiario);
                db.AddInParameter(dbCommand, "@idDomicilioPrestador", DbType.Int32, idDomicilioPrestador);

                //db.AddInParameter(dbCommand, "@CodMovimiento", DbType.Byte, CodMovimiento);
                //db.AddInParameter(dbCommand, "@FecNovedad", DbType.DateTime, FecNovedad);
                //db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Byte, TipoConcepto);


                db.AddOutParameter(dbCommand, "@IdNovedad", DbType.Int64, 8);

                db.ExecuteNonQuery(dbCommand);

                retorno[0] = db.GetParameterValue(dbCommand, "@IdNovedad").ToString();
                retorno[1] = MAC;

                return retorno;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }


        }
        #endregion

        #region Novedades Rechazadas A ConTasas

        public static void Novedades_Rechazadas_A_ConTasas(long IdBeneficiario, long IdPrestador, byte CodMovimiento, byte TipoConcepto,
                                                    int CodConceptoLiq, double ImporteTotal, byte CantCuotas, Single Porcentaje,
                                                    string NroComprobante, string IP, string Usuario, DateTime FecRechazo,
                                                    decimal montoPrestamo, decimal CuotaTotalMensual, decimal TNA, decimal TEM,
                                                    decimal gastoOtorgamiento, decimal gastoAdmMensual, decimal cuotaSocial,
                                                    decimal CFTEA, decimal CFTNAReal, decimal CFTEAReal, decimal gastoAdmMensualReal,
                                                    decimal TIRReal, string mensajeError)
        {

            string sql = "Novedades_Rechazadas_A_ConTasas";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {

                db.AddInParameter(dbCommand, "@idBeneficiario", DbType.Int64, IdBeneficiario);
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, IdPrestador);
                db.AddInParameter(dbCommand, "@CodMovimiento", DbType.Byte, CodMovimiento);
                db.AddInParameter(dbCommand, "@TipoConcepto", DbType.Byte, TipoConcepto);
                db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, CodConceptoLiq);
                db.AddInParameter(dbCommand, "@ImporteTotal", DbType.Double, ImporteTotal);
                db.AddInParameter(dbCommand, "@CantCuotas", DbType.Byte, CantCuotas);
                db.AddInParameter(dbCommand, "@Porcentaje", DbType.Single, Porcentaje);
                db.AddInParameter(dbCommand, "@NroComprobante", DbType.String, NroComprobante);
                db.AddInParameter(dbCommand, "@IP", DbType.String, IP);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, Usuario);
                db.AddInParameter(dbCommand, "@montoPrestamo", DbType.Decimal, montoPrestamo);
                db.AddInParameter(dbCommand, "@CuotaTotalMensual", DbType.Decimal, CuotaTotalMensual);
                db.AddInParameter(dbCommand, "@TNA", DbType.Decimal, TNA);
                db.AddInParameter(dbCommand, "@TEM", DbType.Decimal, TEM);
                db.AddInParameter(dbCommand, "@gastoOtorgamiento", DbType.Decimal, gastoOtorgamiento);
                db.AddInParameter(dbCommand, "@gastoAdmMensual", DbType.Decimal, gastoAdmMensual);
                db.AddInParameter(dbCommand, "@cuotaSocial", DbType.Decimal, cuotaSocial);
                db.AddInParameter(dbCommand, "@CFTEA", DbType.Decimal, CFTEA);
                db.AddInParameter(dbCommand, "@CFTNAReal", DbType.Decimal, CFTNAReal);
                db.AddInParameter(dbCommand, "@CFTEAReal", DbType.Decimal, CFTEAReal);
                db.AddInParameter(dbCommand, "@gastoAdmMensualReal", DbType.Decimal, gastoAdmMensualReal);
                db.AddInParameter(dbCommand, "@TIRReal", DbType.Decimal, TIRReal);
                db.AddInParameter(dbCommand, "@TipoRechazo", DbType.String, mensajeError);

            }

            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion

        #region Trae cuota social por cuil
        
        public static CuotaSocial CuotaSocial_TraeXCuil(long idbeneficiario, long idPrestador)
        {
            string sql = "CuotaSocial_TraeXCuil";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbParameterCollection dbParametros = null;
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            CuotaSocial CS = null;

            try
            {
                db.AddInParameter(dbCommand, "@idbeneficiario", DbType.Int64, idbeneficiario);
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                dbParametros = dbCommand.Parameters;

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        CS = new CuotaSocial(long.Parse(dr["codconceptoliq"].ToString()), int.Parse(dr["idtipoconcepto"].ToString()),
                                               decimal.Parse(dr["valor"].ToString()), Decimal.Parse(dr["porcentaje"].ToString()),
                                               dr["error"].ToString());
                    }
                }
                return CS;

            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                dbParametros = null;
                db = null;
            }
        }

        #endregion

        #region Traer Parametros de Novedad por Prestador Concepto

        public static void Novedad_Parametros_TraerX_Prestador_Concepto(long idPrestador, int codconceptoLiq, short cantCuotas,
                                                                    out double TNA, out double GastoAdministrativo, out bool esPorcentajeGtoAdministrativo,
                                                                    out double SeguroVida, out bool esPorcentajeSegVida,
                                                                    out double GastoAdministrativoTarjeta, out bool esPorcentajeGtoAdministrativoTarjeta,
                                                                    out short TopeEdad)
        {

            TNA = GastoAdministrativo = SeguroVida = GastoAdministrativoTarjeta = TopeEdad = 0;
            esPorcentajeGtoAdministrativo = esPorcentajeSegVida = esPorcentajeGtoAdministrativoTarjeta = false;

            string sql = "TNA_GastoAdm_SeguroVida_X_Concepto";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbParameterCollection dbParametros = null;
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            CuotaSocial CS = new CuotaSocial();

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@codconceptoLiq", DbType.Int32, codconceptoLiq);
                db.AddInParameter(dbCommand, "@cantCuotas", DbType.Int16, cantCuotas);
                dbParametros = dbCommand.Parameters;

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    if (dr.Read())
                    {
                        TNA = Convert.ToDouble(dr["TNA"]);
                        GastoAdministrativo = Convert.ToDouble(dr["GastoAdministrativo"]);
                        SeguroVida = Convert.ToDouble(dr["SeguroVida"]);
                        GastoAdministrativoTarjeta = Convert.ToDouble(dr["GastoAdministrativoTarjeta"]);
                        esPorcentajeGtoAdministrativo = Convert.ToBoolean(dr["esPorcentajeGtoAdministrativo"]);
                        esPorcentajeSegVida = Convert.ToBoolean(dr["esPorcentajeSegVida"]);
                        esPorcentajeGtoAdministrativoTarjeta = Convert.ToBoolean(dr["esPorcentajeGtoAdministrativoTarjeta"]);
                        TopeEdad = short.Parse(dr["TopeEdad"].ToString());
                    }
                }

            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                dbParametros = null;
                db = null;
            }
        }
        #endregion

        #region Trae Informe_NovedadesALiquidar


        public static List<Informe_NovedadesALiquidar> Informe_NovedadesALiquidar(DateTime Fecha_Informe, long id_Prestador, string Nro_Sucursal)
        {


            string sql = "Informe_NovedadesALiquidar_Trae_PDF";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbParameterCollection dbParametros = null;
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            List<Informe_NovedadesALiquidar> lst_NL = new List<Informe_NovedadesALiquidar>();

            try
            {
                db.AddInParameter(dbCommand, "@fechaInforme", DbType.DateTime, Fecha_Informe);
                db.AddInParameter(dbCommand, "@idprestador", DbType.Int64, id_Prestador);
                db.AddInParameter(dbCommand, "@nroSucursal", DbType.AnsiString, Nro_Sucursal);
                dbParametros = dbCommand.Parameters;


                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        lst_NL.Add(new Informe_NovedadesALiquidar(Convert.ToInt64(dr["nroInforme"].ToString()), Convert.ToDateTime(dr["fechaInforme"].ToString()),
                                                                  Convert.ToInt64(dr["idnovedad"].ToString()), Convert.ToInt64(dr["cuil"].ToString()),
                                                                  dr["ApellidoNombre"].ToString(), dr["CBU"].ToString(), Convert.ToDouble(dr["montoPrestamo"].ToString()),
                                                                  Convert.ToInt64(dr["idbeneficiario"].ToString()), dr["LEYENDA"].ToString()));
                    }
                }
                return lst_NL;

            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                dbParametros = null;
                db = null;
            }
        }
        #endregion

        #region  Novedades_TT_SinMigrar_FGS

        public static List<Novedad_FGS> Novedades_TT_SinMigrar_FGS(long idPrestador, int mensual, long idBeneficiario, int CodConceptoLiq,
                                                                DateTime? FechaDesde, DateTime? FechaHasta, string NroSucursal,
                                                                long? idNovedad, string CUIL_Usuario, int idEstado_Documentacion,
                                                                int Tipo_Pago, string Usuario_Logeado)
        {
            string sql = "Novedades_TT_SinMigrar_FGS";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad_FGS> lstNovedades = new List<Novedad_FGS>();

            try
            {
                db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@Mensual", DbType.Int32, mensual);
                db.AddInParameter(dbCommand, "@idBeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, CodConceptoLiq);
                db.AddInParameter(dbCommand, "@FecDesde", DbType.String, !FechaDesde.HasValue ? null : FechaDesde.Value.ToString("yyyyMMdd"));
                db.AddInParameter(dbCommand, "@FecHasta", DbType.String, !FechaHasta.HasValue ? null : FechaHasta.Value.ToString("yyyyMMdd"));
                db.AddInParameter(dbCommand, "@NroSucursal", DbType.String, NroSucursal);
                db.AddInParameter(dbCommand, "@idNovedad", DbType.String, !idNovedad.HasValue ? null : idNovedad.Value.ToString());
                db.AddInParameter(dbCommand, "@usuario", DbType.String, CUIL_Usuario);
                db.AddInParameter(dbCommand, "@idEstadoDocumentacion", DbType.Int32, idEstado_Documentacion);
                db.AddInParameter(dbCommand, "@TipoPago", DbType.Int32, Tipo_Pago);
                db.AddInParameter(dbCommand, "@UsuarioLogeado", DbType.String, Usuario_Logeado);


                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {

                        Novedad_FGS Nov = new Novedad_FGS();

                        Nov.IdNovedad = Int64.Parse(dr["IdNovedad"].ToString());
                        Nov.CantidadCuotas = byte.Parse(dr["CantCuotas"].ToString());
                        Nov.MontoPrestamo = double.Parse(dr["montoPrestamo"].ToString());
                        Nov.Nro_Sucursal = dr["nroSucursal"].ToString();
                        Nov.FechaNovedad = dr["FecNov"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecNov"].ToString());

                        //Nov.Tipo_Cobro = dr["TipoCobro"].ToString();
                        Nov.CBU = dr["cbu"].Equals(DBNull.Value) ? null : dr["cbu"].ToString();
                        Nov.Nro_Tarjeta = dr["nroTarjeta"].Equals(DBNull.Value) ? null : dr["nroTarjeta"].ToString();

                        Nov.Estado_Documentacion = dr["Estadodocumentacion"].ToString();
                        Nov.TNA = Convert.ToDouble(dr["TNA"]);
                        Nov.CFTEAReal = Convert.ToDouble(dr["CFTEAReal"]);


                        Beneficiario Benef = new Beneficiario(Int64.Parse(dr["IdBeneficiario"].ToString()), Convert.ToInt64(dr["Cuil"].ToString()),
                                                                dr["ApellidoNombre"].ToString());
                        Benef.NroDoc = dr["NroDoc"].ToString();
                        Nov.UnBeneficiario = Benef;

                        Domicilio d = new Domicilio();
                        d.Calle = dr["calle"].ToString();
                        d.NumeroCalle = dr["numero"].ToString();
                        d.Piso = dr["piso"].ToString();
                        d.Departamento = dr["depto"].ToString();
                        d.CodigoPostal = dr["codPostal"].ToString();
                        d.Localidad = dr["localidad"].ToString();
                        d.UnaProvincia = new Provincia(Convert.ToInt16(dr["C_PCIA"]), dr["D_PCIA"].ToString());
                        d.EsCelular = Convert.ToBoolean(dr["esCelular1"].ToString().Equals("S") ? true : false);
                        d.PrefijoTel = dr["telediscado1"].ToString();
                        d.NumeroTel = dr["telefono1"].ToString();
                        d.EsCelular2 = Convert.ToBoolean(dr["esCelular2"].ToString().Equals("S") ? true : false);
                        d.PrefijoTel2 = dr["telediscado2"].ToString();
                        d.NumeroTel2 = dr["telediscado2"].ToString();

                        Nov.UnBeneficiario.unDomicilio = d;


                        ConceptoLiquidacion CL = new ConceptoLiquidacion(Int32.Parse(dr["CodConceptoLiq"].ToString()), dr["DescConceptoLiq"].ToString());
                        Nov.UnConceptoLiquidacion = CL;

                        TipoConcepto TC = new TipoConcepto(Convert.ToInt16(dr["TipoConcepto"].ToString()), dr["DescTipoConcepto"].ToString());
                        Nov.UnTipoConcepto = TC;

                        Nov.UnAuditoria = new Auditoria(dr["Usuario"].ToString());

                        lstNovedades.Add(Nov);
                    }
                }

                return lstNovedades;
            }
            catch (SqlException ErrSQL)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));
                if (ErrSQL.Number == 1205 || ErrSQL.Number == 1204)
                    throw new ApplicationException("Interbloqueo");
                else
                    throw ErrSQL;
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

        #endregion

        #region Novedades_TT_SinMigrar_FGS_Operador

        public static List<Novedad_FGS> Novedades_TT_SinMigrar_FGS_Operador(long idPrestador, int mensual, long idBeneficiario, int CodConceptoLiq,
                                                                DateTime? FechaDesde, DateTime? FechaHasta, string NroSucursal,
                                                                long? idNovedad, string CUIL_Usuario, int idEstado_Documentacion,
                                                                int Tipo_Pago, string Usuario_Logeado)
        {
            string sql = "Novedades_TT_SinMigrar_FGS_OPERADOR";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad_FGS> lstNovedades = new List<Novedad_FGS>();

            try
            {
                db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@Mensual", DbType.Int32, mensual);
                db.AddInParameter(dbCommand, "@idBeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, CodConceptoLiq);
                db.AddInParameter(dbCommand, "@FecDesde", DbType.String, !FechaDesde.HasValue ? null : FechaDesde.Value.ToString("yyyyMMdd"));
                db.AddInParameter(dbCommand, "@FecHasta", DbType.String, !FechaHasta.HasValue ? null : FechaHasta.Value.ToString("yyyyMMdd"));
                db.AddInParameter(dbCommand, "@NroSucursal", DbType.String, NroSucursal);
                db.AddInParameter(dbCommand, "@idNovedad", DbType.String, !idNovedad.HasValue ? null : idNovedad.Value.ToString());
                db.AddInParameter(dbCommand, "@usuario", DbType.String, CUIL_Usuario);
                db.AddInParameter(dbCommand, "@idEstadoDocumentacion", DbType.Int32, idEstado_Documentacion);
                db.AddInParameter(dbCommand, "@TipoPago", DbType.Int32, Tipo_Pago);
                db.AddInParameter(dbCommand, "@UsuarioLogeado", DbType.String, Usuario_Logeado);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {

                        Novedad_FGS Nov = new Novedad_FGS();

                        Nov.IdNovedad = Int64.Parse(dr["IdNovedad"].ToString());
                        //Nov.CantidadCuotas = byte.Parse(dr["CantCuotas"].ToString());
                        // Nov.MontoPrestamo = double.Parse(dr["montoPrestamo"].ToString());
                        Nov.Nro_Sucursal = dr["nroSucursal"].ToString();
                        Nov.FechaNovedad = dr["FecNov"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecNov"].ToString());
                        Nov.CBU = dr["cbu"].Equals(DBNull.Value) ? null : dr["cbu"].ToString();
                        Nov.Nro_Tarjeta = dr["nroTarjeta"].Equals(DBNull.Value) ? null : dr["nroTarjeta"].ToString();
                        Nov.Estado_Documentacion = dr["Estadodocumentacion"].ToString();

                        //Beneficiario Benef = new Beneficiario(Int64.Parse(dr["IdBeneficiario"].ToString()), Convert.ToInt64(dr["Cuil"].ToString()),
                        //                                         dr["ApellidoNombre"].ToString());
                        //Benef.NroDoc = dr["NroDoc"].ToString();

                        //Nov.UnBeneficiario = Benef;

                        ConceptoLiquidacion CL = new ConceptoLiquidacion(Int32.Parse(dr["CodConceptoLiq"].ToString()), dr["DescConceptoLiq"].ToString());
                        Nov.UnConceptoLiquidacion = CL;

                        //TipoConcepto TC = new TipoConcepto(Convert.ToInt16(dr["TipoConcepto"].ToString()), dr["DescTipoConcepto"].ToString());
                        //Nov.UnTipoConcepto = TC;

                        Nov.UnAuditoria = new Auditoria(dr["Usuario"].ToString());

                        lstNovedades.Add(Nov);
                    }
                }

                return lstNovedades;
            }
            catch (SqlException ErrSQL)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));
                if (ErrSQL.Number == 1205 || ErrSQL.Number == 1204)
                    throw new ApplicationException("Interbloqueo");
                else
                    throw ErrSQL;
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

        #endregion

        #region Novedades_Traer_FGS

        public static List<Novedad_FGS> Novedades_Traer_FGS(long idPrestador, int mensual, long idBeneficiario, int CodConceptoLiq,
                                                                DateTime? FechaDesde, DateTime? FechaHasta, string NroSucursal,
                                                                long? idNovedad, string CUIL_Usuario, int idEstado_Documentacion,
                                                                int Tipo_Pago, bool generaArchivo, Boolean generadoAdmin, out string rutaArchivoSal,
                                                                int? NroReporte, DateTime? Fecha_Presentacion, string Nro_Sucursal,
                                                                string Usuario_Logeado, string Perfil)
        {
            string rutaArchivo = string.Empty;
            string nombreArchivo = string.Empty;
            rutaArchivoSal = string.Empty;
            string msgRta = string.Empty;
            ConsultaBatch consultaBatch = new ConsultaBatch();
            consultaBatch.IDPrestador = idPrestador;
            consultaBatch.NombreConsulta = ConsultaBatch.enum_ConsultaBatch_NombreConsulta.NOVEDADES_INGRESADAS_FGS;
            consultaBatch.PeriodoCons = mensual.ToString();
            consultaBatch.NroBeneficio = idBeneficiario;
            consultaBatch.UnConceptoLiquidacion = new ConceptoLiquidacion(CodConceptoLiq, string.Empty);
            consultaBatch.FechaDesde = FechaDesde;
            consultaBatch.FechaHasta = FechaHasta;
            consultaBatch.GeneradoAdmin = generadoAdmin;
            consultaBatch.NroReporte = NroReporte;
            consultaBatch.Fecha_Presentacion = Fecha_Presentacion;
            consultaBatch.Nro_Sucursal = Nro_Sucursal;
            consultaBatch.Tipo_Pago = Tipo_Pago;
            consultaBatch.CUIL_Usuario = CUIL_Usuario;
            consultaBatch.IdEstado_Documentacion = idEstado_Documentacion;
            consultaBatch.Usuario_Logeado = Usuario_Logeado;
            consultaBatch.Perfil = Perfil;
            consultaBatch.Idnovedad = idNovedad;

            try
            {
                if (generaArchivo == true)
                {
                    msgRta = ConsultasBatchDAO.ExisteConsulta(consultaBatch);

                    if (!string.IsNullOrEmpty(msgRta))
                        throw new ApplicationException("MSG_ERROR" + msgRta + "FIN_MSG_ERROR");
                }

                List<Novedad_FGS> listNovedades = Novedades_TT_SinMigrar_FGS(idPrestador, mensual, idBeneficiario, CodConceptoLiq, FechaDesde,
                                                                        FechaHasta, NroSucursal, idNovedad, CUIL_Usuario, idEstado_Documentacion, Tipo_Pago, Usuario_Logeado);

                if (listNovedades.Count > 0 && generaArchivo == true)
                {
                    int maxCantidad = Settings.MaxCantidadRegistros();

                    if (listNovedades.Count >= maxCantidad || generaArchivo == true)
                    {
                        nombreArchivo = Utilidades.GeneraNombreArchivo(consultaBatch.NombreConsulta.ToString(), idPrestador, out rutaArchivo);
                        rutaArchivoSal = Path.Combine(rutaArchivo, nombreArchivo);
                        StreamWriter sw = new StreamWriter(rutaArchivoSal, false, Encoding.UTF8);
                        string separador = Settings.DelimitadorCampo();

                        foreach (Novedad_FGS oNovedad in listNovedades)
                        {
                            StringBuilder linea = new StringBuilder();

                            linea.Append(oNovedad.IdNovedad.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.IdBeneficiario.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.Cuil.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.NroDoc.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.ApellidoNombre.ToString() + separador);
                            linea.Append(oNovedad.FechaNovedad.ToString("dd/MM/yyyy HH:mm:ss") + separador);
                            linea.Append(oNovedad.UnConceptoLiquidacion.CodConceptoLiq.ToString() + separador);
                            linea.Append(oNovedad.UnConceptoLiquidacion.DescConceptoLiq.ToString() + separador);
                            linea.Append(oNovedad.UnTipoConcepto.IdTipoConcepto + separador);
                            linea.Append(oNovedad.UnTipoConcepto.DescTipoConcepto + separador);
                            linea.Append(oNovedad.MontoPrestamo.ToString() + separador);
                            linea.Append(oNovedad.CantidadCuotas.ToString() + separador);
                            linea.Append(oNovedad.Nro_Sucursal.ToString() + separador);
                            linea.Append(oNovedad.UnAuditoria.Usuario.ToString() + separador);
                            linea.Append((oNovedad.Nro_Tarjeta == null ? string.Empty : oNovedad.Nro_Tarjeta.ToString()) + separador);
                            linea.Append((oNovedad.CBU == null ? string.Empty : oNovedad.CBU.ToString()) + separador);
                            linea.Append(oNovedad.Estado_Documentacion.ToString() + separador);
                            linea.Append(oNovedad.TNA.ToString() + separador);
                            linea.Append(oNovedad.CFTEAReal.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.unDomicilio.Calle.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.unDomicilio.NumeroCalle.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.unDomicilio.Piso.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.unDomicilio.Departamento.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.unDomicilio.CodigoPostal.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.unDomicilio.Localidad.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.unDomicilio.UnaProvincia.CodProvincia.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.unDomicilio.UnaProvincia.DescripcionProvincia.ToString() + separador);
                            linea.Append((oNovedad.UnBeneficiario.unDomicilio.EsCelular ? "S" : "N") + separador);
                            linea.Append(oNovedad.UnBeneficiario.unDomicilio.PrefijoTel.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.unDomicilio.NumeroTel.ToString() + separador);
                            linea.Append((oNovedad.UnBeneficiario.unDomicilio.EsCelular2 ? "S" : "N") + separador);
                            linea.Append(oNovedad.UnBeneficiario.unDomicilio.PrefijoTel2.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.unDomicilio.NumeroTel2.ToString() + separador);

                            #region
                            // linea.Append(oNovedad.IdNovedad.ToString() + separador);
                            // linea.Append(oNovedad.CantidadCuotas.ToString() + separador);
                            // linea.Append(oNovedad.MontoPrestamo.ToString() + separador);
                            // linea.Append(oNovedad.TNA.ToString() + separador);
                            // linea.Append(oNovedad.CFTEAReal.ToString() + separador);
                            // linea.Append(oNovedad.Nro_Sucursal.ToString() + separador);
                            // linea.Append(oNovedad.FechaNovedad.ToString("dd/MM/yyyy HH:mm:ss") + separador);                            
                            // linea.Append(oNovedad.Estado_Documentacion.ToString() + separador);                            
                            // linea.Append(oNovedad.UnConceptoLiquidacion.CodConceptoLiq.ToString() + separador);
                            // linea.Append(oNovedad.UnConceptoLiquidacion.DescConceptoLiq.ToString() + separador);
                            // linea.Append(oNovedad.UnTipoConcepto.DescTipoConcepto + separador);
                            // linea.Append((oNovedad.CBU == null ? string.Empty : oNovedad.CBU.ToString()) + separador);
                            //  linea.Append((oNovedad.Nro_Tarjeta == null ? string.Empty : oNovedad.Nro_Tarjeta.ToString()) + separador);
                            // linea.Append(oNovedad.UnAuditoria.Usuario.ToString() + separador);
                            // linea.Append(oNovedad.UnBeneficiario.IdBeneficiario.ToString() + separador);
                            //linea.Append(oNovedad.UnBeneficiario.Cuil.ToString() + separador);
                            // linea.Append(oNovedad.UnBeneficiario.ApellidoNombre.ToString() + separador);
                            // linea.Append(oNovedad.UnBeneficiario.NroDoc.ToString() + separador);

                            //linea.Append(oNovedad.UnBeneficiario.unDomicilio.Calle.ToString() + separador);
                            //linea.Append(oNovedad.UnBeneficiario.unDomicilio.NumeroCalle.ToString() + separador);
                            //linea.Append(oNovedad.UnBeneficiario.unDomicilio.Piso.ToString() + separador);
                            //linea.Append(oNovedad.UnBeneficiario.unDomicilio.Departamento.ToString() + separador);
                            //linea.Append(oNovedad.UnBeneficiario.unDomicilio.CodigoPostal.ToString() + separador);
                            //linea.Append(oNovedad.UnBeneficiario.unDomicilio.Localidad.ToString() + separador);
                            //linea.Append(oNovedad.UnBeneficiario.unDomicilio.UnaProvincia.DescripcionProvincia.ToString() + separador);
                            //linea.Append((oNovedad.UnBeneficiario.unDomicilio.EsCelular?"S":"N") + separador);
                            //linea.Append(oNovedad.UnBeneficiario.unDomicilio.PrefijoTel.ToString() + separador);
                            //linea.Append(oNovedad.UnBeneficiario.unDomicilio.NumeroTel.ToString() + separador);
                            //linea.Append((oNovedad.UnBeneficiario.unDomicilio.EsCelular2?"S":"N") + separador);
                            //linea.Append(oNovedad.UnBeneficiario.unDomicilio.PrefijoTel2.ToString() + separador);
                            //linea.Append(oNovedad.UnBeneficiario.unDomicilio.NumeroTel2.ToString() + separador);                           
                            #endregion

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
                        listNovedades = new List<Novedad_FGS>();
                    }
                }

                return listNovedades;
            }
            catch (SqlException errsql)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), errsql.Source, errsql.Message));

                if (errsql.Number == -2)
                {
                    nombreArchivo = Utilidades.GeneraNombreArchivo(consultaBatch.NombreConsulta.ToString(), idPrestador, out rutaArchivo);
                    consultaBatch.NomArchGenerado = nombreArchivo;
                    consultaBatch.RutaArchGenerado = rutaArchivo;
                    consultaBatch.FechaGenera = DateTime.MinValue;
                    consultaBatch.Vigente = false;

                    msgRta = ConsultasBatchDAO.AltaNuevaConsulta(consultaBatch);

                    throw new ApplicationException("MSG_ERROR Generando el archivo. Reingrese a la consulta en unos minutos.FIN_MSG_ERROR");
                }
                else
                    throw errsql;
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

        #endregion

        #region Novedades_Traer_FGS_Operador

        public static List<Novedad_FGS> Novedades_Traer_FGS_Operador(long idPrestador, int mensual, long idBeneficiario, int CodConceptoLiq,
                                                                DateTime? FechaDesde, DateTime? FechaHasta, string NroSucursal,
                                                                long? idNovedad, string CUIL_Usuario, int idEstado_Documentacion,
                                                                int Tipo_Pago, bool generaArchivo, Boolean generadoAdmin, out string rutaArchivoSal,
                                                                int? NroReporte, DateTime? Fecha_Presentacion, string Nro_Sucursal,
                                                                string Usuario_Logeado, string Perfil)
        {
            string rutaArchivo = string.Empty;
            string nombreArchivo = string.Empty;
            rutaArchivoSal = string.Empty;
            string msgRta = string.Empty;
  
            ConsultaBatch consultaBatch = new ConsultaBatch();
            consultaBatch.IDPrestador = idPrestador;
            consultaBatch.NombreConsulta = ConsultaBatch.enum_ConsultaBatch_NombreConsulta.NOVEDADES_INGRESADAS_FGS_OPERADOR;
            consultaBatch.PeriodoCons = mensual.ToString();
            consultaBatch.NroBeneficio = idBeneficiario;
            consultaBatch.UnConceptoLiquidacion = new ConceptoLiquidacion(CodConceptoLiq, string.Empty);
            consultaBatch.FechaDesde = FechaDesde;
            consultaBatch.FechaHasta = FechaHasta;
            consultaBatch.Nro_Sucursal = Nro_Sucursal;
            consultaBatch.Idnovedad = idNovedad;
            consultaBatch.CUIL_Usuario = CUIL_Usuario;
            consultaBatch.IdEstado_Documentacion = idEstado_Documentacion;
            consultaBatch.Tipo_Pago = Tipo_Pago;
            consultaBatch.GeneradoAdmin = generadoAdmin;
            consultaBatch.NroReporte = NroReporte;
            consultaBatch.Fecha_Presentacion = Fecha_Presentacion;  
            consultaBatch.Usuario_Logeado = Usuario_Logeado;
            consultaBatch.Perfil = Perfil;

            try
            {
                if (generaArchivo == true)
                {
                    msgRta = ConsultasBatchDAO.ExisteConsulta(consultaBatch);

                    if (!string.IsNullOrEmpty(msgRta))
                        throw new ApplicationException("MSG_ERROR" + msgRta + "FIN_MSG_ERROR");
                }

                List<Novedad_FGS> listNovedades = Novedades_TT_SinMigrar_FGS_Operador(idPrestador, mensual, idBeneficiario, CodConceptoLiq, FechaDesde,
                                                                                      FechaHasta, NroSucursal, idNovedad, CUIL_Usuario, idEstado_Documentacion, Tipo_Pago, Usuario_Logeado);

                if (listNovedades.Count > 0 && generaArchivo == true)
                {
                    int maxCantidad = Settings.MaxCantidadRegistros();

                    if (listNovedades.Count >= maxCantidad || generaArchivo == true)
                    {
                        nombreArchivo = Utilidades.GeneraNombreArchivo(consultaBatch.NombreConsulta.ToString(), idPrestador, out rutaArchivo);
                        rutaArchivoSal = Path.Combine(rutaArchivo, nombreArchivo);
                        StreamWriter sw = new StreamWriter(rutaArchivoSal, false, Encoding.UTF8);
                        string separador = Settings.DelimitadorCampo();

                        foreach (Novedad_FGS oNovedad in listNovedades)
                        {
                            StringBuilder linea = new StringBuilder();


                            linea.Append(oNovedad.IdNovedad.ToString() + separador);
                            linea.Append(oNovedad.UnConceptoLiquidacion.CodConceptoLiq.ToString() + separador);
                            linea.Append(oNovedad.UnConceptoLiquidacion.DescConceptoLiq.ToString() + separador);
                            linea.Append(oNovedad.FechaNovedad.ToString("dd/MM/yyyy HH:mm:ss") + separador);
                            linea.Append(oNovedad.Nro_Sucursal.ToString() + separador);
                            linea.Append(oNovedad.UnAuditoria.Usuario.ToString() + separador);
                            linea.Append((oNovedad.CBU == null ? string.Empty : oNovedad.CBU.ToString()) + separador);
                            linea.Append((oNovedad.Nro_Tarjeta == null ? string.Empty : oNovedad.Nro_Tarjeta.ToString()) + separador);
                            linea.Append(oNovedad.Estado_Documentacion.ToString() + separador);
                            #region

                            //linea.Append(oNovedad.IdNovedad.ToString() + separador);
                            //linea.Append(oNovedad.Nro_Sucursal.ToString() + separador);
                            //linea.Append(oNovedad.FechaNovedad.ToString("dd/MM/yyyy HH:mm:ss") + separador);                            
                            //linea.Append(oNovedad.Estado_Documentacion.ToString() + separador);
                            //linea.Append(oNovedad.UnConceptoLiquidacion.CodConceptoLiq.ToString() + separador);
                            //linea.Append(oNovedad.UnConceptoLiquidacion.DescConceptoLiq.ToString() + separador);
                            //linea.Append((oNovedad.CBU == null ? string.Empty : oNovedad.CBU.ToString()) + separador);
                            //linea.Append((oNovedad.Nro_Tarjeta == null ? string.Empty : oNovedad.Nro_Tarjeta.ToString()) + separador);
                            #endregion


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
                        listNovedades = new List<Novedad_FGS>();
                    }
                }

                return listNovedades;
            }
            catch (SqlException errsql)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), errsql.Source, errsql.Message));

                if (errsql.Number == -2)
                {
                    nombreArchivo = Utilidades.GeneraNombreArchivo(consultaBatch.NombreConsulta.ToString(), idPrestador, out rutaArchivo);
                    consultaBatch.NomArchGenerado = nombreArchivo;
                    consultaBatch.RutaArchGenerado = rutaArchivo;
                    consultaBatch.FechaGenera = DateTime.MinValue;
                    consultaBatch.Vigente = false;

                    msgRta = ConsultasBatchDAO.AltaNuevaConsulta(consultaBatch);

                    throw new ApplicationException("MSG_ERROR Generando el archivo. Reingrese a la consulta en unos minutos.FIN_MSG_ERROR");
                }
                else
                    throw errsql;
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

        #endregion

        #region  Novedades_AprobarCredito

        public static string Novedades_AprobarCredito(long id_Novedad, int id_EstadoReg, string Usuario)
        {
            string sql = "Novedades_AprobarCredito";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;

            try
            {
                db.AddInParameter(dbCommand, "@idnovedad", DbType.Int64, id_Novedad);
                db.AddInParameter(dbCommand, "@idestadoreg", DbType.Int64, id_EstadoReg);
                db.AddInParameter(dbCommand, "@usuario", DbType.String, Usuario);

                db.AddOutParameter(dbCommand, "@error", DbType.String, 100);

                dbParametros = dbCommand.Parameters;
                db.ExecuteNonQuery(dbCommand);

                return dbParametros[03].Value.ToString();
            }
            catch (SqlException sqlex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlex.Source, sqlex.Message));
                throw sqlex;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en NovedadDAO.Novedades_AprobarCredito", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        #endregion

        public static void Informe_NovedadesALiquidar_Alta(long idPrestador, long idNovedad, int codConceptoliq, int idItem, int codRtaTS,
                                                           string MensajeTS, DateTime fprocesoRtaTS, long? nroLote, long nroTransaccion, string oficina)
        {
            string sql = "Informe_NovedadesALiquidar_A";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;

            try
            {
                Usuario usuario = Utilidades.GetUsuario();
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@idNovedad", DbType.Int64, idNovedad);
                db.AddInParameter(dbCommand, "@codConceptoliq", DbType.Int32, codConceptoliq);
                db.AddInParameter(dbCommand, "@idItem", DbType.Int32, idItem);
                db.AddInParameter(dbCommand, "@Respuesta", DbType.Int32, codRtaTS);
                db.AddInParameter(dbCommand, "@MensajeTS", DbType.String, MensajeTS);
                db.AddInParameter(dbCommand, "@fprocesoRtaTS", DbType.DateTime, fprocesoRtaTS);
                db.AddInParameter(dbCommand, "@nroLote", DbType.Int64, nroLote);
                db.AddInParameter(dbCommand, "@nroTransaccion", DbType.Int64, nroTransaccion);
                db.AddInParameter(dbCommand, "@usuario", DbType.String, usuario.Legajo);
                db.AddInParameter(dbCommand, "@oficina", DbType.String, oficina);
                db.AddInParameter(dbCommand, "@ip", DbType.String, usuario.Ip);

                dbParametros = dbCommand.Parameters;
                db.ExecuteNonQuery(dbCommand);

            }
            catch (SqlException sqlex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlex.Source, sqlex.Message));

                throw new ApplicationException("MSG_ERROR 003 |" + MensajeTS + sqlex.Message);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("MSG_ERROR 003 en  NovedadDAO.Informe_NovedadesALiquidar_Alta", ex);
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }


        }

        #region Novedades_Aprobacion

        public static List<KeyValue<long, string>> Novedades_Aprobacion(List<long> listNovedadesAAprobar, int idEstadoReg, string usuario)
        {
            try
            {
                string retorno = string.Empty;
                List<KeyValue<long, string>> listNovedadesNoAprobadas = new List<KeyValue<long, string>>();

                foreach (long idNovedad in listNovedadesAAprobar)
                {
                    try
                    {
                        retorno = Novedades_AprobarCredito(idNovedad, idEstadoReg, usuario);

                        if (!retorno.Equals(string.Empty))
                        {
                            listNovedadesNoAprobadas.Add(new KeyValue<long, string>(idNovedad, retorno));
                        }
                        else
                        {
                            listNovedadesNoAprobadas.Add(new KeyValue<long, string>(idNovedad, "OK"));
                        }
                    }
                    catch (Exception err)
                    {
                        listNovedadesNoAprobadas.Add(new KeyValue<long, string>(idNovedad, err.Message));
                    }
                }
                return listNovedadesNoAprobadas;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en NovedadDAO.Novedades_Aprobacion", err);
            }
        }

        #endregion

        #region  Novedades_Confirmacion

        public static string Novedades_Confirmacion(long idNovedad, int idEstadoReg, string ip, string usuario, string oficina)
        {
            string sql = "Novedades_ConfirmaCallCenter";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            string rdo = string.Empty;

            try
            {
                db.AddInParameter(dbCommand, "@IdNovedad", DbType.Int64, idNovedad);
                db.AddInParameter(dbCommand, "@IdEstadoReg", DbType.Int16, idEstadoReg);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, usuario);
                db.AddInParameter(dbCommand, "@IP", DbType.String, ip);
                db.AddInParameter(dbCommand, "@Oficina", DbType.String, oficina);

                dbParametros = dbCommand.Parameters;
                db.ExecuteNonQuery(dbCommand);

                return rdo;
            }
            catch (DbException sqlErr)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlErr.Source, sqlErr.Message));
                if (((System.Data.SqlClient.SqlException)(sqlErr)).Number >= 50000)
                    return ((System.Exception)(sqlErr)).Message;
                else
                    throw sqlErr;
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

        #region Novedades_TT_Xa_REPOSICION

        public static List<Novedad> Novedades_TT_Xa_REPOSICION(string cuil)
        {
            string sql = "Novedades_TT_Xa_REPOSICION";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad> listNovedad = new List<Novedad>();
            Novedad unaNovedad;

            try
            {
                db.AddInParameter(dbCommand, "@Cuil", DbType.String, cuil);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        unaNovedad = new Novedad();
                        unaNovedad.IdNovedad = long.Parse(dr["IdNovedad"].ToString());
                        unaNovedad.FechaNovedad = DateTime.Parse(dr["FecNov"].ToString());
                        unaNovedad.FechaImportacion = dr["FecImportacion"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecImportacion"].ToString());
                        unaNovedad.ImporteTotal = double.Parse(dr.GetValue("ImporteTotal").ToString());
                        unaNovedad.CantidadCuotas = byte.Parse(dr.GetValue("CantCuotas").ToString());
                        unaNovedad.Porcentaje = Single.Parse(dr["Porcentaje"].ToString());
                        unaNovedad.Comprobante = dr["NroComprobante"].ToString();
                        unaNovedad.MAC = dr["MAC"].ToString();
                        unaNovedad.Nro_Tarjeta = dr["nroTarjeta"].Equals(DBNull.Value) ? null : dr["nroTarjeta"].ToString();
                        unaNovedad.FReposicion = dr["fReposicion"].Equals(DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dr["fReposicion"]);
                        unaNovedad.IdEstadoReg = byte.Parse(dr["IdEstadoReg"].ToString());

                        unaNovedad.UnTipoConcepto = new TipoConcepto(Byte.Parse(dr["TipoConcepto"].ToString()),
                                                                     dr.GetString("DescTipoConcepto"));

                        unaNovedad.UnBeneficiario = new Beneficiario();
                        unaNovedad.UnBeneficiario.IdBeneficiario = long.Parse(dr["IdBeneficiario"].ToString());
                        unaNovedad.UnBeneficiario.ApellidoNombre = dr.GetString("ApellidoNombre");
                        unaNovedad.UnBeneficiario.Cuil = long.Parse(cuil);

                        unaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                                   dr.GetString("DescConceptoLiq"));
                        listNovedad.Add(unaNovedad);
                    }
                }

                return listNovedad;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }

        #endregion

        #region traer_NovedadInfoAmpliada

        public static NovedadInfoAmpliada traer_NovedadInfoAmpliada(long idNovedad)
        {
            string sql = "Novedades_InfoAmpliada_XID";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            NovedadInfoAmpliada unNovedadInfoAmpliada = new NovedadInfoAmpliada();
            try
            {
                db.AddInParameter(dbCommand, "@idnovedad", DbType.Int64, idNovedad);
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    //SP retorna 4 select por eso se realiza  varios dr.Read()
                    #region 1° - Noveda Info
                    while (dr.Read())
                    {
                        unNovedadInfoAmpliada.unNovedad_Info = new Novedad_Info(long.Parse(dr["IdNovedad"].ToString()),
                                                                                dr["RazonSocial"].ToString(),
                                                                                DateTime.Parse(dr["FecNov"].ToString()),
                                                                                Byte.Parse(dr["IdEstadoReg"].ToString()),
                                                                                dr["DescripcionEstadoReg"].ToString(),
                                                                                Int32.Parse(dr["CodMovimiento"].ToString()),
                                                                                dr["DescMovimiento"].ToString(),
                                                                                Int32.Parse(dr["CodConceptoLiq"].ToString()),
                                                                                dr["DescConceptoLiq"].ToString(),
                                                                                new TipoConcepto(short.Parse(dr["TipoConcepto"].ToString()), dr["DescTipoConcepto"].ToString()),
                                                                                Double.Parse(dr["ImporteTotal"].ToString()),
                                                                                Int16.Parse(dr["CantCuotas"].ToString()),
                                                                                float.Parse(dr["Porcentaje"].ToString()),
                                                                                dr["NroComprobante"].ToString(),
                                                                                dr["MAC"].ToString(),
                                                                                dr["usuarioAlta"].ToString(),
                                                                                dr["PrimerMensual"].ToString(),
                                                                                dr["MensualReenvio"] == DBNull.Value ? (int?)null : int.Parse(dr["MensualReenvio"].ToString()),
                                                                                dr["montoPrestamo"] == DBNull.Value ? 0 : Double.Parse(dr["montoPrestamo"].ToString()),
                                                                                dr["CuotaTotalMensual"] == DBNull.Value ? (Double?)null : Double.Parse(dr["CuotaTotalMensual"].ToString()),
                                                                                dr["TNA"] == DBNull.Value ? 0 : Double.Parse(dr["TNA"].ToString()),
                                                                                dr["TEM"] == DBNull.Value ? 0 : Double.Parse(dr["TEM"].ToString()),
                                                                                dr["gastoOtorgamiento"] == DBNull.Value ? (Double?)null : Double.Parse(dr["gastoOtorgamiento"].ToString()),
                                                                                dr["gastoAdmMensual"] == DBNull.Value ? (Double?)null : Double.Parse(dr["gastoAdmMensual"].ToString()),
                                                                                dr["cuotaSocial"] == DBNull.Value ? 0 : Double.Parse(dr["cuotaSocial"].ToString()),
                                                                                dr["CFTEA"] == DBNull.Value ? 0 : Double.Parse(dr["CFTEA"].ToString()),
                                                                                dr["CFTNAReal"] == DBNull.Value ? 0 : Double.Parse(dr["CFTNAReal"].ToString()),
                                                                                dr["gastoAdmMensualReal"] == DBNull.Value ? 0 : Double.Parse(dr["gastoAdmMensualReal"].ToString()),
                                                                                dr["idItem"] == DBNull.Value ? (int?)null : int.Parse(dr["idItem"].ToString()),
                                                                                dr["descripcionItem"] == DBNull.Value ? "" : dr["descripcionItem"].ToString(),
                                                                                dr["nroFactura"] == DBNull.Value ? "" : dr["nroFactura"].ToString(),
                                                                                dr["cbu"] == DBNull.Value ? "" : dr["cbu"].ToString(),
                                                                                dr["otro"] == DBNull.Value ? "" : dr["otro"].ToString(),
                                                                                dr["nroSocio"] == DBNull.Value ? "" : dr["poliza"].ToString(),
                                                                                dr["nroSocio"] == DBNull.Value ? "" : dr["nroSocio"].ToString(),
                                                                                dr["otroServicioPrestado"] == DBNull.Value ? "" : dr["otroServicioPrestado"].ToString(),
                                                                                dr["nroSucursal"] == DBNull.Value ? "" : dr["nroSucursal"].ToString(),
                                                                                dr["nroTarjeta"] == DBNull.Value ? "" : dr["nroTarjeta"].ToString(),
                                                                                dr["nroTicket"] == DBNull.Value ? "" : dr["nroTicket"].ToString()
                                                                                );

                    }
                    #endregion

                    unNovedadInfoAmpliada.Cuotas = new List<Cuota>();
                    dr.NextResult();

                    #region 2°- Cuota
                    while (dr.Read())
                    {
                        Cuota unCuota = new Cuota(long.Parse(dr["IdNovedad"].ToString()),
                                                  dr["Mensual"].ToString(),
                                                  int.Parse(dr["NroCuota"].ToString()),
                                                  Double.Parse(dr["Importe"].ToString()),
                                                  dr["amortizacion"] == DBNull.Value ? 0 : Double.Parse(dr["amortizacion"].ToString()),
                                                  dr["importeInteres"] == DBNull.Value ? 0 : Double.Parse(dr["importeInteres"].ToString()),
                                                  dr["gastoAdmMensualCalc"] == DBNull.Value ? 0 : Double.Parse(dr["gastoAdmMensualCalc"].ToString()),
                                                  dr["gastoAdminTarjeta"] == DBNull.Value ? 0 : Double.Parse(dr["gastoAdminTarjeta"].ToString()),
                                                  dr["seguroVida"] == DBNull.Value ? 0 : Double.Parse(dr["seguroVida"].ToString()));

                        unNovedadInfoAmpliada.Cuotas.Add(unCuota);
                    }
                    #endregion

                    dr.NextResult();
                    unNovedadInfoAmpliada.NovedadedLiquidadas = new List<NovedadLiquidada>();

                    #region 3°- NovedadLiquidada

                    while (dr.Read())
                    {
                        NovedadLiquidada unNovLiq = new NovedadLiquidada(long.Parse(dr["IdNovedad"].ToString()),
                                                                         int.Parse(dr["PeriodoLiq"].ToString()),
                                                                         dr["ImporteCuota"] == DBNull.Value ? (Double?)null : Double.Parse(dr["ImporteCuota"].ToString()),
                                                                         dr["NroCuotaLiq"] == DBNull.Value ? (Int16?)null : Convert.ToInt16(dr["NroCuotaLiq"]),
                                                                         dr["IdEstadoNov"] == DBNull.Value ? (Int16?)null : Convert.ToInt16(dr["IdEstadoNov"]),
                                                                         dr["ImporteALiq"] == DBNull.Value ? (Double?)null : Double.Parse(dr["ImporteALiq"].ToString()),
                                                                         dr["ImporteLiq"] == DBNull.Value ? (Double?)null : Double.Parse(dr["ImporteLiq"].ToString()),
                                                                         Boolean.Parse(dr["PorcentajeNoCalc"].ToString()),
                                                                         dr["IdMensaje"].ToString(),
                                                                         dr["Mensaje"].ToString(),
                                                                         dr["importeInteres"] == DBNull.Value ? (int?)null : int.Parse(dr["importeInteres"].ToString()),
                                                                         dr["amortizacion"] == DBNull.Value ? (int?)null : int.Parse(dr["amortizacion"].ToString()),
                                                                         dr["gastoAdmMensualCalc"] == DBNull.Value ? (int?)null : int.Parse(dr["gastoAdmMensualCalc"].ToString()),
                                                                         dr["montoCuotaTotal"] == DBNull.Value ? (int?)null : int.Parse(dr["montoCuotaTotal"].ToString()),
                                                                         dr["totalAmortizado"] == DBNull.Value ? (int?)null : int.Parse(dr["totalAmortizado"].ToString()),
                                                                         dr["seguroVida"] == DBNull.Value ? (int?)null : int.Parse(dr["seguroVida"].ToString()),
                                                                         dr["gastoAdminTarjeta"] == DBNull.Value ? (int?)null : int.Parse(dr["gastoAdminTarjeta"].ToString()),
                                                                         dr["mensualEmision"] == DBNull.Value ? (int?)null : int.Parse(dr["mensualEmision"].ToString()),
                                                                         dr["tipoLiq"].ToString(),
                                                                         dr["amortizacionPagado"] == DBNull.Value ? (Double?)null : Double.Parse(dr["amortizacionPagado"].ToString()),
                                                                         dr["importeInteresPagado"] == DBNull.Value ? (Double?)null : Double.Parse(dr["importeInteresPagado"].ToString()),
                                                                         dr["seguroVidaPagado"] == DBNull.Value ? (Double?)null : Double.Parse(dr["seguroVidaPagado"].ToString()),
                                                                         dr["gastoAdmMensualPagado"] == DBNull.Value ? (Double?)null : Double.Parse(dr["gastoAdmMensualPagado"].ToString()),
                                                                         dr["gastoAdminTarjetaPagado"] == DBNull.Value ? (Double?)null : Double.Parse(dr["gastoAdminTarjetaPagado"].ToString()),
                                                                         dr["identPago"].ToString()
                                                                         );



                        unNovedadInfoAmpliada.NovedadedLiquidadas.Add(unNovLiq);

                    }
                    #endregion

                    dr.NextResult();
                    unNovedadInfoAmpliada.NovedadHistoricas = new List<NovedadHistorica>();

                    #region 4° - NovedadHistorica
                    while (dr.Read())
                    {
                        NovedadHistorica unNovedadHistorica = new NovedadHistorica(long.Parse(dr["IdNovedad"].ToString()),
                                                                                   Int16.Parse(dr["idEstadoReg"].ToString()),
                                                                                   dr["DescripcionEstadoReg"].ToString(),
                                                                                   Int16.Parse(dr["CodMovimiento"].ToString()),
                                                                                   dr["DescMovimiento"].ToString(),
                                                                                   dr["Mensual"] == DBNull.Value ? (int?)null : int.Parse(dr["Mensual"].ToString()),
                                                                                   dr["ImporteCuota"] == DBNull.Value ? (Double?)null : Double.Parse(dr["ImporteCuota"].ToString()),
                                                                                   dr["NroCuotaLiq"] == DBNull.Value ? (int?)null : int.Parse(dr["NroCuotaLiq"].ToString()),
                                                                                   Convert.ToDateTime(dr["FecUltModificacion"]),
                                                                                   dr["UsuarioBaja"].ToString(),
                                                                                   dr["importeInteres"] == DBNull.Value ? (Double?)null : Double.Parse(dr["importeInteres"].ToString()),
                                                                                   dr["amortizacion"] == DBNull.Value ? (Double?)null : Double.Parse(dr["amortizacion"].ToString()),
                                                                                   dr["gastoAdmMensualCalc"] == DBNull.Value ? (Double?)null : Double.Parse(dr["gastoAdmMensualCalc"].ToString()),
                                                                                   dr["montoCuotaTotal"] == DBNull.Value ? (Double?)null : Double.Parse(dr["montoCuotaTotal"].ToString()),
                                                                                   dr["totalAmortizado"] == DBNull.Value ? (Double?)null : Double.Parse(dr["totalAmortizado"].ToString()),
                                                                                   dr["seguroVida"] == DBNull.Value ? (Double?)null : Double.Parse(dr["seguroVida"].ToString()),
                                                                                   dr["gastoAdminTarjeta"] == DBNull.Value ? (Double?)null : Double.Parse(dr["gastoAdminTarjeta"].ToString()));




                        unNovedadInfoAmpliada.NovedadHistoricas.Add(unNovedadHistorica);
                    }
                    #endregion

                    dr.Close();

                }
                return unNovedadInfoAmpliada;

            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }

        }

        #endregion
        
        public static List<NovedadesLiq_RepImp_Historico> Novedadesliquidadas_RepagoImpagos_T_Historico(long idBeneficiario, int codConceptoLiq, int periodoliq)
        {
            string sql = "novedadesliquidadas_RepagoImpagos_T_Historico";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbParameterCollection dbParametros = null;
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<NovedadesLiq_RepImp_Historico> listaRdo = new List<NovedadesLiq_RepImp_Historico>();

            try
            {
                db.AddInParameter(dbCommand, "@idbeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, codConceptoLiq);
                db.AddInParameter(dbCommand, "@periodoliq", DbType.Int32, periodoliq);
                dbParametros = dbCommand.Parameters;

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        listaRdo.Add(new NovedadesLiq_RepImp_Historico(int.Parse(dr["PeriodoLiq"].ToString()),
                                                                        int.Parse(dr["MensualEmision"].ToString()),
                                                                       dr["tipoLiq"].ToString(),
                                                                       dr["identPago"].Equals(DBNull.Value) ? "SI": dr["identPago"].ToString(),
                                                                       dr["DescIdentPago"].Equals(DBNull.Value) ? "Sin Información": dr["DescIdentPago"].ToString(),
                                                                       int.Parse(dr["idEstadoRub"].Equals(DBNull.Value) ? "0" : dr["idEstadoRub"].ToString()),
                                                                       dr["daEstadoRub"].Equals(DBNull.Value) ? "SI": dr["daEstadoRub"].ToString(),
                                                                       dr["descEstadoRub"].Equals(DBNull.Value) ? "Sin información":  dr["descEstadoRub"].ToString()
                                                                      ));

                    }
                }
                return listaRdo;

            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }


        }

        public static List<NovedadNoInformadaXBanco> Novedades_NoInformadasXelBanco()
        {
            string sql = "Novedades_NoInformadasXelBanco";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");          
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<NovedadNoInformadaXBanco> listaRdo = new List<NovedadNoInformadaXBanco>();

            try
            {              
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        listaRdo.Add(new NovedadNoInformadaXBanco(dr["fechaInforme"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["fechaInforme"].ToString()),
                                                                  int.Parse(dr["cantidadCierre"].ToString()),
                                                                  int.Parse(dr["cantSinInformar"].ToString()),
                                                                  int.Parse(dr["cantInfOk"].ToString()),
                                                                  int.Parse(dr["cantInfRechazos"].ToString())));

                    }
                }
                return listaRdo;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        
        public static List<NovedadMontoPrestamoTotal> Novedades_CalculoMontoPrestamoTotal(long idBeneficiario, long idPrestador, int codConceptoLiq)
        {
            string sql = "Calculo_MontoPrestamoTotal";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            List<NovedadMontoPrestamoTotal> listaRdo = new List<NovedadMontoPrestamoTotal>();
            
            try
            {
                db.AddInParameter(dbCommand, "@idbeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@idprestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@codconceptoliq", DbType.Int32, codConceptoLiq);
                dbParametros = dbCommand.Parameters;

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        listaRdo.Add(new NovedadMontoPrestamoTotal(dr["importePrimerCuota"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dr["importePrimerCuota"].ToString()),
                                                                   dr["montoPrestamo"].Equals(DBNull.Value) ? 0 : Convert.ToDouble(dr["montoPrestamo"].ToString()),
                                                                   dr["cantcuotas"].Equals(DBNull.Value) ? Convert.ToByte("0") : Convert.ToByte(dr["cantcuotas"].ToString()),  
                                                                   dr["mensaje"].ToString()));

                    }
                }
                return listaRdo;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
    }
}
