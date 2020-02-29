using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NullableReaders;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Transactions;
using Ar.Gov.Anses.Microinformatica.AuditoriaLog;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class TasasDAO : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TasasDAO).Name); 
        
        #region Dispose

        private bool disposing;

        public void Dispose()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }

        protected virtual void Dispose(bool b)
        {
            // Si no se esta destruyendo ya…
            if (!disposing)
            {
                // La marco como desechada ó desechandose,
                // de forma que no se puede ejecutar este código
                // dos veces.
                disposing = true;

                // Indico al GC que no llame al destructor
                // de esta clase al recolectarla.
                GC.SuppressFinalize(this);

                // … libero los recursos… 
            }
        }

        ~TasasDAO()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        public static List<Tasa> TasasAplicadas_TxTop20(int TipoTasa, double TasaDesde, double TasaHasta, int CuotaDesde, int CuotaHasta)
        {
            string sql = "TasasAplicadas_TxTop20";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                List<Tasa> lstTasas = new List<Tasa>();

                db.AddInParameter(dbCommand, "@tipoTasa", DbType.Int32, TipoTasa);
                db.AddInParameter(dbCommand, "@PorcentajeDesde", DbType.Double, TasaDesde);
                db.AddInParameter(dbCommand, "@PorcentajeHasta", DbType.Double, TasaHasta);
                db.AddInParameter(dbCommand, "@CantCuotasDesde", DbType.Int16, CuotaDesde);
                db.AddInParameter(dbCommand, "@CantCuotasHasta", DbType.Int16, CuotaHasta);

                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        lstTasas.Add(new Tasa(int.Parse(dr["idTasaAplicada"].ToString()),
                                     dr["FInicio"].Equals(DBNull.Value) ? new DateTime?() : DateTime.Parse(dr["FInicio"].ToString()),
                                     dr["FFin"].Equals(DBNull.Value) ? new DateTime?() : DateTime.Parse(dr["FFin"].ToString()),
                                     dr["FInicioVigencia"].Equals(DBNull.Value) ? new DateTime?() : DateTime.Parse(dr["FInicioVigencia"].ToString()),
                                     dr["FFinVigencia"].Equals(DBNull.Value) ? new DateTime?() : DateTime.Parse(dr["FFinVigencia"].ToString()),
                                     string.IsNullOrEmpty(dr["TNA"].ToString()) ? 0 : Double.Parse(dr["TNA"].ToString()),
                                     string.IsNullOrEmpty(dr["TEA"].ToString()) ? 0 : Double.Parse(dr["TEA"].ToString()),
                                     string.IsNullOrEmpty(dr["GastoAdministrativo"].ToString()) ? 0 : Double.Parse(dr["GastoAdministrativo"].ToString()),
                                     //int.Parse(dr["CantCuotas"].ToString()),
                                     //dr["CantCuotasHasta"].Equals(DBNull.Value) ? new Int16() : Int16.Parse(dr["CantCuotasHasta"].ToString()),
                                     dr["CantCuotas"].Equals(DBNull.Value) ? (int?) null : Int16.Parse(dr["CantCuotas"].ToString()),
                                     dr["CantCuotasHasta"].Equals(DBNull.Value) ? (int?)null : Int16.Parse(dr["CantCuotasHasta"].ToString()),
                                     dr["LCredito"].ToString(),
                                     dr["Observaciones"].ToString(),
                                     null,
                                     bool.Parse(dr["Aprobada"].ToString()),
                                     new Prestador(long.Parse(dr["idPrestador"].ToString()),
                                                   dr["RazonSocialPrestador"].ToString(), dr["cuitPrestador"].Equals(DBNull.Value) ? 0 : long.Parse(dr["cuitPrestador"].ToString())),
                                     new Comercializador(long.Parse(dr["idcomercializador"].ToString()),
                                                         dr["RazonSocialComercializador"].ToString(), dr["CuitComercializador"].Equals(DBNull.Value) ? 0 : long.Parse(dr["CuitComercializador"].ToString()), string.Empty),
                                     new Auditoria()));

                    }
                }
                return lstTasas;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en TasasDAO.TraerTasas_xidComercializador", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        /// <summary>
        /// Trae una lista de Tasas asociadas a un Comercializador
        /// </summary>
        /// <param name="idPrestador">id del Prestador</param>
        /// <returns></returns>
        public static List<Tasa> TraerTasas_xidPrestadorIdComercializador(long idPrestador, long idComercializador)
        {
            string sql = "TasasAplicada_TxPrestador_Comercializador";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Tasa> lstTasas = new List<Tasa>();

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@idComercializador", DbType.Int64, idComercializador);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        lstTasas.Add(new Tasa(int.Parse(dr["idTasaAplicada"].ToString()),
                                              dr["FInicio"].Equals(DBNull.Value) ? new DateTime?() : DateTime.Parse(dr["FInicio"].ToString()),
                                              dr["FFin"].Equals(DBNull.Value) ? new DateTime?() : DateTime.Parse(dr["FFin"].ToString()),
                                              dr["FInicioVigencia"].Equals(DBNull.Value) ? new DateTime?() : DateTime.Parse(dr["FInicioVigencia"].ToString()),
                                              dr["FFinVigencia"].Equals(DBNull.Value) ? new DateTime?() : DateTime.Parse(dr["FFinVigencia"].ToString()),
                                              string.IsNullOrEmpty(dr["TNA"].ToString()) ? 0 : Double.Parse(dr["TNA"].ToString()),
                                              string.IsNullOrEmpty(dr["TEA"].ToString()) ? 0 : Double.Parse(dr["TEA"].ToString()),
                                              dr["GastoAdministrativo"].Equals(DBNull.Value) ? 0 : Double.Parse(dr["GastoAdministrativo"].ToString()),
                                              //dr["CantCuotas"].Equals(DBNull.Value) ? 0 : Int16.Parse(dr["CantCuotas"].ToString()),
                                              //dr["CantCuotasHasta"].Equals(DBNull.Value) ? 0 : Int16.Parse(dr["CantCuotasHasta"].ToString()),
                                              dr["CantCuotas"].Equals(DBNull.Value) ? (int?)null : Int16.Parse(dr["CantCuotas"].ToString()),
                                              dr["CantCuotasHasta"].Equals(DBNull.Value) ? (int?)null : Int16.Parse(dr["CantCuotasHasta"].ToString()),
                                              dr["LCredito"].Equals(DBNull.Value) ? "" : dr["LCredito"].ToString(),
                                              dr["Observaciones"].Equals(DBNull.Value) ? "" : dr["Observaciones"].ToString(),
                                              dr["FAprobacion"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FAprobacion"].ToString()),
                                              bool.Parse(dr["Aprobada"].ToString()),
                                              new Prestador(long.Parse(dr["idPrestador"].ToString()),
                                                            string.Empty, 0),
                                              new Comercializador(long.Parse(dr["idcomercializador"].ToString()),
                                                                  string.Empty, 0, string.Empty),
                                              new Auditoria(dr["UsuarioCarga"].ToString(),
                                                            dr["IPOrigen"].ToString(),
                                                            int.Parse(dr["NroOficina"].ToString()),
                                                            DateTime.Parse(dr["FUltModificacionCarga"].ToString())),
                                              new Auditoria(dr["UsuarioAprobacion"].Equals(DBNull.Value) ? "" : dr["UsuarioAprobacion"].ToString(),
                                                            dr["IPAprobacion"].Equals(DBNull.Value) ? "" : dr["IPAprobacion"].ToString(),
                                                            dr["NroOficinaAprobacion"].Equals(DBNull.Value) ? 0 : int.Parse(dr["NroOficinaAprobacion"].ToString()),
                                                            null)));
                    }
                }
                return lstTasas;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en TasasDAO.TraerTasas_xidComercializador", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        /// <summary>
        /// Trae una lista de Tasas asociadas a un Comercializador
        /// </summary>        
        public static string TasasAplicadasMB(long idPrestador, long idComercializador, Tasa unaTasaAplicada)
        {
            string sql = "TasasAplicadas_MB";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string mensage = string.Empty;

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@idComercializador", DbType.Int64, idComercializador);
                db.AddInParameter(dbCommand, "@idTasaAplicada", DbType.Int32, unaTasaAplicada.ID);
                db.AddInParameter(dbCommand, "@FInicio", DbType.DateTime, unaTasaAplicada.FechaInicio);
                db.AddInParameter(dbCommand, "@FFIn", DbType.DateTime, unaTasaAplicada.FechaFin);
                db.AddInParameter(dbCommand, "@TNA", DbType.Double, unaTasaAplicada.TNA);
                db.AddInParameter(dbCommand, "@TEA", DbType.Double, unaTasaAplicada.TEA);
                db.AddInParameter(dbCommand, "@GastoAdministrativo", DbType.Double, unaTasaAplicada.GastoAdministrativo);
                db.AddInParameter(dbCommand, "@CantCuotas", DbType.Int16, unaTasaAplicada.CantCuotas);
                db.AddInParameter(dbCommand, "@CantCuotasHasta", DbType.Int16, unaTasaAplicada.CantCuotasHasta);
                db.AddInParameter(dbCommand, "@LCredito", DbType.String, unaTasaAplicada.LineaCredito);
                db.AddInParameter(dbCommand, "@Observaciones", DbType.String, unaTasaAplicada.Observaciones);

                db.AddInParameter(dbCommand, "@UsuarioCarga", DbType.String, unaTasaAplicada.UnaAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@IPOrigen", DbType.String, unaTasaAplicada.UnaAuditoria.IP);
                db.AddInParameter(dbCommand, "@NroOficina", DbType.Int32, unaTasaAplicada.UnaAuditoria.IDOficina);

                using (TransactionScope oTransactionScope = new TransactionScope())
                {
                   SeguridadLogDAO.GuardarLog("Tasas", "M", dbCommand.Parameters);
                  
                   db.ExecuteReader(dbCommand);

                    oTransactionScope.Complete();
                }
            }
            catch (SqlException sqlex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlex.Source, sqlex.Message));
                if (sqlex.State == 2)
                {
                    mensage = sqlex.Message;
                    return mensage;
                }
                else
                    throw sqlex;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en TasasDAO.TasasAplicadasAM : ", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return mensage;
        }

        /// <summary>
        /// Trae una lista de Tasas asociadas a un Comercializador
        /// </summary>        
        public static string TasasAplicadas_RelacionComercializadorPresador_B(Int64 idprestador,
                                             Int64 idComercializador,
                                             DateTime FFin_Baja,
                                             Auditoria unaAuditoria)
        {
            List<Tasa> unaListTasas = new List<Tasa>();
            string sql = "TasasAplicadas_RelacionComercializadorPresador_B";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idprestador);
                db.AddInParameter(dbCommand, "@idComercializador", DbType.Int64, idComercializador);
                db.AddInParameter(dbCommand, "@FFin_Baja", DbType.DateTime, FFin_Baja);

                db.AddInParameter(dbCommand, "@UsuarioCarga", DbType.String, unaAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@IPOrigen", DbType.String, unaAuditoria.IP);
                db.AddInParameter(dbCommand, "@NroOficina", DbType.String, unaAuditoria.IDOficina);

                using (TransactionScope oTransactionScope = new TransactionScope())
                {
                   SeguridadLogDAO.GuardarLog("Tasas", "M", dbCommand.Parameters);

                   //SeguridadLogDAO.AuditarOnlineLog(idprestador.ToString(), dbCommand.Parameters, " ", LoggingAnses.Servicio.Entidad.TipoAction.ACTUALIZAR);

                    db.ExecuteReader(dbCommand);

                    oTransactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en TasasDAO.TasasAplicadasB : ", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return string.Empty;
        }

        public static string TasasAplicadas_RelacionComercializadorPresador_BFisica(Int64 idprestador,
                                     Int64 idComercializador,
                                     DateTime FFin_Baja,
                                     Auditoria unaAuditoria)
        {
            List<Tasa> unaListTasas = new List<Tasa>();
            string sql = "TasasAplicadas_RelacionComercializadorPresador_BFisica";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idprestador);
                db.AddInParameter(dbCommand, "@idComercializador", DbType.Int64, idComercializador);
                db.AddInParameter(dbCommand, "@FFin_Baja", DbType.DateTime, FFin_Baja);

                db.AddInParameter(dbCommand, "@UsuarioCarga", DbType.String, unaAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@IPOrigen", DbType.String, unaAuditoria.IP);
                db.AddInParameter(dbCommand, "@NroOficina", DbType.String, unaAuditoria.IDOficina);

                using (TransactionScope oTransactionScope = new TransactionScope())
                {
                    SeguridadLogDAO.GuardarLog("Tasas", "B", dbCommand.Parameters);

                    db.ExecuteReader(dbCommand);

                    oTransactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en TasasDAO.TasasAplicadasB : ", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return string.Empty;
        }


        /// <summary>
        /// Guarda un historico de Tasas Aplicadas
        /// </summary>        
        public static string TasasAplicadasHistorica_A(Int64 idprestador,
                                                       Int64 idComercializador,
                                                       DateTime FFin_Baja)
        {
            List<Tasa> unaListTasas = new List<Tasa>();
            string sql = "TasasAplicadasHistorica_A";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idprestador);
                db.AddInParameter(dbCommand, "@idComercializador", DbType.Int64, idComercializador);
                db.AddInParameter(dbCommand, "@FFin_Baja", DbType.DateTime, FFin_Baja);

                //db.AddInParameter(dbCommand, "@UsuarioCarga", DbType.String, unaAuditoria.Usuario);
                //db.AddInParameter(dbCommand, "@IPOrigen", DbType.String, unaAuditoria.IP);
                //db.AddInParameter(dbCommand, "@NroOficina", DbType.String, unaAuditoria.IDOficina);

                db.ExecuteReader(dbCommand);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en TasasDAO.TasasAplicadasHistorica_A : ", ex);
            }
            finally
            {
                 
                db = null;
                dbCommand.Dispose();
            }

            return string.Empty;
        }

        public static string TasasAplicadasA(long idPrestador, long idComercializador, Tasa unaTasaAplicada)
        {
            string sql = "TasasAplicadas_A";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string mensage = string.Empty;

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@idComercializador", DbType.Int64, idComercializador);
                //db.AddInParameter(dbCommand, "@idTasaAplicada", DbType.Int32, unaTasaAplicada.ID);
                db.AddInParameter(dbCommand, "@FInicio", DbType.DateTime, unaTasaAplicada.FechaInicio);
                db.AddInParameter(dbCommand, "@FInicioVigencia", DbType.DateTime, unaTasaAplicada.FechaInicioVigencia);
                db.AddInParameter(dbCommand, "@FFin", DbType.DateTime, unaTasaAplicada.FechaFin);
                db.AddInParameter(dbCommand, "@TNA", DbType.Double, unaTasaAplicada.TNA);
                db.AddInParameter(dbCommand, "@TEA", DbType.Double, unaTasaAplicada.TEA);
                db.AddInParameter(dbCommand, "@GastoAdministrativo", DbType.Double, unaTasaAplicada.GastoAdministrativo);
                db.AddInParameter(dbCommand, "@CantCuotas", DbType.Int16, unaTasaAplicada.CantCuotas);
                db.AddInParameter(dbCommand, "@CantCuotasHasta", DbType.Int16, unaTasaAplicada.CantCuotasHasta);
                db.AddInParameter(dbCommand, "@LCredito", DbType.String, unaTasaAplicada.LineaCredito);
                db.AddInParameter(dbCommand, "@Observaciones", DbType.String, unaTasaAplicada.Observaciones);
                db.AddInParameter(dbCommand, "@UsuarioCarga", DbType.String, unaTasaAplicada.UnaAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@IPOrigen", DbType.String, unaTasaAplicada.UnaAuditoria.IP);
                db.AddInParameter(dbCommand, "@NroOficina", DbType.Int32, unaTasaAplicada.UnaAuditoria.IDOficina);

                db.ExecuteReader(dbCommand);
            }
            catch (SqlException sqlex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlex.Source, sqlex.Message));
                
                if (sqlex.State == 2)
                {
                    mensage = sqlex.Message;
                    return mensage;
                }
                else
                    throw sqlex;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en TasasDAO.TasasAplicadasAM : ", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return mensage;
        }

        //public static string TasasAplicadasHistoricaA(long idPrestador, long idComercializador, Tasa unaTasaAplicada)
        //{
        //    string sql = "TasasAplicadas_A";
        //    Database db = DatabaseFactory.CreateDatabase("DAT_V01");
        //    DbCommand dbCommand = db.GetStoredProcCommand(sql);
        //    string mensage = string.Empty;

        //    try
        //    {
        //        db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
        //        db.AddInParameter(dbCommand, "@idComercializador", DbType.Int64, idComercializador);
        //        //db.AddInParameter(dbCommand, "@idTasaAplicada", DbType.Int32, unaTasaAplicada.ID);
        //        db.AddInParameter(dbCommand, "@FInicio", DbType.DateTime, unaTasaAplicada.FechaInicio);
        //        db.AddInParameter(dbCommand, "@FInicioVigencia", DbType.DateTime, unaTasaAplicada.FechaInicioVigencia);
        //        //db.AddInParameter(dbCommand, "@FFin", DbType.DateTime, unaTasaAplicada.FechaFin);
        //        db.AddInParameter(dbCommand, "@TNA", DbType.Double, unaTasaAplicada.TNA);
        //        db.AddInParameter(dbCommand, "@TEA", DbType.Double, unaTasaAplicada.TEA);
        //        db.AddInParameter(dbCommand, "@GastoAdministrativo", DbType.Double, unaTasaAplicada.GastoAdministrativo);
        //        db.AddInParameter(dbCommand, "@CantCuotas", DbType.Int16, unaTasaAplicada.CantCuotas);
        //        db.AddInParameter(dbCommand, "@CantCuotasHasta", DbType.Int16, unaTasaAplicada.CantCuotasHasta);
        //        db.AddInParameter(dbCommand, "@LCredito", DbType.String, unaTasaAplicada.LineaCredito);
        //        db.AddInParameter(dbCommand, "@Observaciones", DbType.String, unaTasaAplicada.Observaciones);
        //        db.AddInParameter(dbCommand, "@UsuarioCarga", DbType.String, unaTasaAplicada.UnaAuditoria.Usuario);
        //        db.AddInParameter(dbCommand, "@IPOrigen", DbType.String, unaTasaAplicada.UnaAuditoria.IP);
        //        db.AddInParameter(dbCommand, "@NroOficina", DbType.Int32, unaTasaAplicada.UnaAuditoria.IDOficina);

        //        db.ExecuteReader(dbCommand);
        //    }
        //    catch (SqlException sqlex)
        //    {
        //        if (sqlex.State == 2)
        //        {
        //            mensage = sqlex.Message;
        //            return mensage;
        //        }
        //        else
        //            throw sqlex;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error en TasasDAO.TasasAplicadasAM : ", ex);
        //    }
        //    finally
        //    {
        //        db = null;
        //        dbCommand.Dispose();
        //    }

        //    return mensage;
        //}

        /// <summary>
        /// Trae una lista de Tasas asociadas a un Prestador por RazonSocial o fecha inicio fecha fin.
        /// </summary>        
        public static List<Tasa> TasasAplicadasParaGestionUCATxTop50(string codigoSistema, string razonSocial, DateTime fechaDesde, DateTime fechaHasta, bool habilita)
        {
            string sql = "TasasAplicadas_ParaAprobacionTxTop50";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                List<Tasa> lstTasas = new List<Tasa>();

                db.AddInParameter(dbCommand, "@codigoSistema", DbType.String, codigoSistema);
                db.AddInParameter(dbCommand, "@razonSocial", DbType.String, razonSocial);
                db.AddInParameter(dbCommand, "@fDesde", DbType.DateTime, (fechaDesde.Equals(new DateTime()) ? null : (DateTime?)fechaDesde));
                db.AddInParameter(dbCommand, "@fHasta", DbType.DateTime, (fechaHasta.Equals(new DateTime()) ? null : (DateTime?)fechaHasta));
                db.AddInParameter(dbCommand, "@habilita", DbType.Boolean, habilita);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        lstTasas.Add(new Tasa(int.Parse(dr["idTasaAplicada"].ToString()),
                                               dr["FInicio"].Equals(DBNull.Value) ? null : dr.GetNullableDateTime("FInicio"),
                                               dr["FFin"].Equals(DBNull.Value) ? null : dr.GetNullableDateTime("FFin"),
                                               dr["FInicioVigencia"].Equals(DBNull.Value) ? null : dr.GetNullableDateTime("FInicioVigencia"),
                                               dr["FFinVigencia"].Equals(DBNull.Value) ? new DateTime?() : DateTime.Parse(dr["FFinVigencia"].ToString()),
                                               string.IsNullOrEmpty(dr["TNA"].ToString()) ? 0 : Double.Parse(dr["TNA"].ToString()),
                                               string.IsNullOrEmpty(dr["TEA"].ToString()) ? 0 : Double.Parse(dr["TEA"].ToString()),
                                               dr["GastoAdministrativo"].Equals(DBNull.Value) ? 0 : Double.Parse(dr["GastoAdministrativo"].ToString()),
                                               //dr["CantCuotas"].Equals(DBNull.Value) ? 0 : Int16.Parse(dr["CantCuotas"].ToString()),
                                               //dr["CantCuotasHasta"].Equals(DBNull.Value) ? 0 : Int16.Parse(dr["CantCuotasHasta"].ToString()),
                                               dr["CantCuotas"].Equals(DBNull.Value) ? (int?)null : Int16.Parse(dr["CantCuotas"].ToString()),
                                               dr["CantCuotasHasta"].Equals(DBNull.Value) ? (int?)null : Int16.Parse(dr["CantCuotasHasta"].ToString()),
                                               dr["LCredito"].Equals(DBNull.Value) ? "" : dr["LCredito"].ToString(),
                                               dr["Observaciones"].Equals(DBNull.Value) ? "" : dr["Observaciones"].ToString(),
                                               dr["FAprobacion"].Equals(DBNull.Value) ? null : dr.GetNullableDateTime("FAprobacion"),
                                               bool.Parse(dr["Aprobada"].ToString()),
                                               new Prestador(long.Parse(dr["idPrestador"].ToString()),
                                                             dr["RazonSocialPrestador"].ToString(),
                                                             long.Parse(dr["CUIT"].ToString())),
                                               new Comercializador(long.Parse(dr["idcomercializador"].ToString()),
                                                                   dr["RazonSocialComercializador"].ToString(), 0, string.Empty),
                                               new Auditoria(dr["UsuarioCarga"].ToString(),
                                                             dr["IPOrigen"].ToString(),
                                                             int.Parse(dr["NroOficina"].ToString()),
                                                             DateTime.Parse(dr["FUltModificacionCarga"].ToString())),
                                               new Auditoria(dr["UsuarioAprobacion"].Equals(DBNull.Value) ? "" : dr["UsuarioAprobacion"].ToString(),
                                                             dr["IPAprobacion"].Equals(DBNull.Value) ? "" : dr["IPAprobacion"].ToString(),
                                                             dr["NroOficinaAprobacion"].Equals(DBNull.Value) ? 0 : int.Parse(dr["NroOficinaAprobacion"].ToString()),
                                                             null)));
                    }
                }
                return lstTasas;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en TasasDAO.TraerTasas_xidComercializador", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        public static string TasasAplicadasHabilitaDeshabilita(Tasa[] TasasAplicadas, bool habilita)
        {
            string sql = "TasasAplicadas_Habilita_Deshabilita";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            string mensage = string.Empty;
            string xmlTasasAplicadas = Utilidades.SerializeObject(TasasAplicadas);
            xmlTasasAplicadas = xmlTasasAplicadas.Remove(0, 1);

            try
            {
                db.AddInParameter(dbCommand, "@TasasAprobacion", DbType.String, xmlTasasAplicadas);
                db.AddInParameter(dbCommand, "@Habilita", DbType.Boolean, habilita);
                db.ExecuteNonQuery(dbCommand);
            }
            catch (SqlException sqlex)
            {
                if (sqlex.State == 2)
                {
                    mensage = sqlex.Message;
                    return mensage;
                }
                else
                    throw sqlex;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en TasasDAO.TasasAplicadasAprobacion : ", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
            return mensage;
        }

        //public static string TasasAplicadasHabilitaDeshabilita(List<Tasa> TasasAplicadas, bool habilita)
        //{
        //    string sql = "TasasAplicadas_Habilita_Deshabilita";
        //    SqlDatabase db = (SqlDatabase)DatabaseFactory.CreateDatabase("DAT_V01");
        //    DbCommand dbCommand = db.GetStoredProcCommand(sql);
        //    DataTable dtTasasAplicadas = new DataTable();
        //    string mensage = string.Empty;

        //    dtTasasAplicadas.Columns.Add("idTasaAplicada", typeof(int));
        //    dtTasasAplicadas.Columns.Add("FAprobacion", typeof(DateTime));            
        //    dtTasasAplicadas.Columns.Add("UsuarioAprobacion", typeof(string));
        //    dtTasasAplicadas.Columns.Add("IPAprobacion", typeof(string));            
        //    dtTasasAplicadas.Columns.Add("NroOficinaAprobacion", typeof(int));
        //    dtTasasAplicadas.PrimaryKey = new DataColumn[] { dtTasasAplicadas.Columns["idTasaAplicada"] };

        //    foreach (Tasa  itemTasa in TasasAplicadas)
        //    {
        //        DataRow dr = dtTasasAplicadas.NewRow();
        //        dr["idTasaAplicada"] = itemTasa.ID;
        //        if (habilita)
        //        dr["FAprobacion"] = DateTime.Now ;
        //        else
        //        dr["FAprobacion"] = DBNull.Value;

        //        dr["IPAprobacion"] = itemTasa.UnaAudAprobacion.IP;
        //        dr["NroOficinaAprobacion"] = itemTasa.UnaAudAprobacion.IDOficina;
        //        dr["UsuarioAprobacion"] = itemTasa.UnaAudAprobacion.Usuario;  
        //        dtTasasAplicadas.Rows.Add(dr);
        //    }

        //    try
        //    {
        //        db.AddInParameter(dbCommand, "@TasasAprobacion", SqlDbType.Structured, dtTasasAplicadas);
        //        db.AddInParameter(dbCommand, "@Habilita", SqlDbType.Bit, habilita);
        //        db.ExecuteNonQuery(dbCommand);
        //    }
        //    catch (SqlException sqlex)
        //    {
        //        if (sqlex.State == 2)
        //        {
        //            mensage = sqlex.Message;
        //            return mensage;
        //        }
        //        else
        //            throw sqlex; 
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error en TasasDAO.TasasAplicadasAprobacion : " , ex );
        //    }
        //    finally
        //    {
        //        db = null;
        //        dbCommand.Dispose();
        //    }
        //    return mensage;
        //}
    }
}
