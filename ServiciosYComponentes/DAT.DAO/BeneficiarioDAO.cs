
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NullableReaders;
using System.Transactions;
using Ar.Gov.Anses.Microinformatica.AuditoriaLog;
using System.Data.SqlClient;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class BeneficiarioDAO : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BeneficiarioDAO).Name);

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

        ~BeneficiarioDAO()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        #region Trae Apellido y Nombre

        public static string TraerApellNom(long idBeneficiario)
        {
            string sql = "Beneficiarios_TApenombre";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);

                string apenom = db.ExecuteScalar(dbCommand).ToString();
                return apenom;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> IdBeneficiario:{2} - Error:{3}->{4}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), idBeneficiario, err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion Trae Apellido y Nombre

        #region Trae Beneficiarios

        public static List<Beneficiario> TraerPorIdBenefCuil(string idBeneficiario, string cuil)
        {
            string sql = "Admin_Beneficiarios_Traer";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Beneficiario> lstBeneficiario = new List<Beneficiario>();

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.String, idBeneficiario);
                db.AddInParameter(dbCommand, "@Cuil", DbType.String, cuil);

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        /* Any - comento pq el sp No trae estos datos, produce error. verificar si se necesitan*/
                        //Auditoria unAuditoria = new Auditoria(ds["Usuario"].ToString(),
                        //                                    string.Empty,
                        //                                    DateTime.Parse(ds["FecUltModificacion"].ToString()));

                        lstBeneficiario.Add(new Beneficiario(
                                             long.Parse(ds["IdBeneficiario"].ToString()),
                                             long.Parse(ds["Cuil"].ToString()),
                                             int.Parse(ds["TipoDoc"].ToString()),
                                             (ds.GetNullableInt64("NroDoc") == null ? "" : ds.GetInt64("NroDoc").ToString()),
                                             ds["ApellidoNombre"].ToString(),
                                             double.Parse(ds["SueldoBruto"].ToString()),
                                             double.Parse(ds["SueldoParaOblig"].ToString()),
                                             double.Parse(ds["AfectacionDisponible"].ToString()),
                                             double.Parse(ds["TotObligatoria"].ToString()),
                                             double.Parse(ds["TotNovedad"].ToString()),
                                             int.Parse(ds["CantOcurrenciasDisp"].ToString()),
                                             new Auditoria(), 0,
                                             int.Parse(ds["IdEstado"].ToString()),
                                             ds["DescripcionEstado"].ToString()                                           
                                             ));
                    }

                }
                return lstBeneficiario;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> IdBeneficiario:{2} -Cuil {3}- Error:{4}->{5}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), idBeneficiario, cuil, err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion Trae Beneficiarios

        #region Trae Novedades por Ingresadas del Beneficiario

        public static List<Beneficiario> NovedadesTraer(long idBeneficiario)
        {
            string sql = "Admin_Novedades_TT_X_Beneficiario";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Beneficiario> lstBeneficiario = new List<Beneficiario>();

            try
            {
                db.AddInParameter(dbCommand, "@Beneficiario", DbType.Int64, idBeneficiario);

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        lstBeneficiario.Add(new Beneficiario(long.Parse(ds["IdBeneficiario"].ToString()),
                                                             0, 0, string.Empty,
                                                             ds["ApellidoNombre"].ToString(),
                                                             0, 0, 0, 0, 0, 0, null, 0, 0, ""));
                    }
                }

                return lstBeneficiario;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> IdBeneficiario:{2} - Error:{3}->{4}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), idBeneficiario, err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion Trae Novedades ingresadas del Beneficiario

        #region Trae todas las Novedades del Beneficiario

        public static List<Beneficiario> NovedadesTraerTodas(long idBeneficiario)
        {
            string sql = "Admin_Novedades_TraeTodas_X_Beneficiario";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Beneficiario> lstBeneficiario = new List<Beneficiario>();

            try
            {
                return lstBeneficiario;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> IdBeneficiario:{2} - Error:{3}->{4}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), idBeneficiario, err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion Trae todas las Novedades del Beneficiario

        #region Trae Novedades Rechazadas por Beneficiario

        public static List<Beneficiario> NovedadesRechazadasTraer(long idBeneficiario)
        {
            string sql = "Admin_Novedades_TTRechazadas_X_Beneficiario";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Beneficiario> lstBeneficiario = new List<Beneficiario>();

            try
            {
                db.AddInParameter(dbCommand, "@Beneficiario", DbType.String, idBeneficiario);

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        lstBeneficiario.Add(new Beneficiario(long.Parse(ds["IdBeneficiario"].ToString()),
                                                             0, 0, string.Empty,
                                                             ds["ApellidoNombre"].ToString(),
                                                             0, 0, 0, 0, 0, 0, null, 0, 0, ""));
                    }
                }

                return lstBeneficiario;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> IdBeneficiario:{2} - Error:{3}->{4}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), idBeneficiario, err.Source, err.Message));
                throw err;
            }
            finally
            {
                dbCommand.Dispose();
                db = null;
            }
        }
        #endregion Trae Novedades Rechazadas por Beneficiario

        #region Trae Datos Completos del Beneficio
        public static TodoDelBeneficio TraerTodoDelBeneficio(string Beneficiario)
        {
            string sql = "Admin_Beneficio_TraeTodo_V02";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            TodoDelBeneficio unTodoDelBeneficio = null;

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, Beneficiario);

                List<Inhibiciones> inhibiciones = new List<Inhibiciones>();
                Beneficiario unBeneficiario = new Beneficiario();
                BeneficioBloqueado unBeneficioBloqueado = new BeneficioBloqueado();
                List<ConceptoAplicado> conceptoAplicados = new List<ConceptoAplicado>();

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    unTodoDelBeneficio = new TodoDelBeneficio();
                    //Sp trae varios select por eso se realiza varioa dr.Read()

                    #region 1° - Beneficiario
                    while (dr.Read())
                    {
                        //Estos Datos vienen seguro, es el primer select de la consulta
                        if (dr.GetNullableInt64("IdBeneficiario") != null)
                        {
                            unTodoDelBeneficio.unBeneficiario = new Beneficiario(
                                                 long.Parse(dr["IdBeneficiario"].ToString()),
                                                 long.Parse(dr["Cuil"].ToString()),
                                                 dr["ApellidoNombre"].ToString(),
                                                 double.Parse(dr["SueldoBruto"].ToString()),
                                                 double.Parse(dr["SueldoParaOblig"].ToString()),
                                                 double.Parse(dr["AfectacionDisponible"].ToString()),
                                                 double.Parse(dr["TotObligatoria"].ToString()),
                                                 double.Parse(dr["TotNovedad"].ToString()),
                                                 int.Parse(dr["CantOcurrenciasDisp"].ToString()),
                                                 obtenerEstado(dr),
                                                 int.Parse(dr["TipoDoc"].ToString()),
                                                 dr["codalfa"].ToString(),
                                                 (dr.GetNullableInt64("NroDoc") == null ? "" : dr.GetInt64("NroDoc").ToString()),
                                                 dr["cbu"] == DBNull.Value ? "" : dr["cbu"].ToString()
                                                 );
                        }
                    }
                    #endregion

                    dr.NextResult();
                    #region 2° - ConceptoAplicado
                    unTodoDelBeneficio.conceptoAplicados = new List<ConceptoAplicado>();
                    while (dr.Read())
                    {
                        ConceptoAplicado unConceptoAplicado = new ConceptoAplicado();
                        unConceptoAplicado = obtenerConceptoAplicado(dr);
                        unTodoDelBeneficio.conceptoAplicados.Add(unConceptoAplicado);
                    }
                    #endregion

                    dr.NextResult();
                    #region 3° -  BeneficioBloqueado
                    unTodoDelBeneficio.unBeneficioBloqueado = new BeneficioBloqueado();
                    while (dr.Read())
                    {
                        unTodoDelBeneficio.unBeneficioBloqueado = obtenerBeneficioBloqueado(dr);
                    }
                    #endregion

                    dr.NextResult();
                    #region 4° - Inhibiciones

                    unTodoDelBeneficio.inhibiciones = new List<Inhibiciones>();
                    while (dr.Read())
                    {
                        Inhibiciones unInhibicion = new Inhibiciones();
                        unInhibicion = obtenerInhibiciones(dr);
                        unTodoDelBeneficio.inhibiciones.Add(unInhibicion);
                    }
                    #endregion

                    dr.Close();
                }

                return unTodoDelBeneficio;

            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Beneficiario:{2} - Error:{3}->{4}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), Beneficiario, err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        private static Estado obtenerEstado(NullableDataReader dr)
        {
            return new Estado(Int32.Parse(string.IsNullOrEmpty(dr["IdEstado"].ToString()) ? "0" : dr["IdEstado"].ToString()),
                                dr.IsDBNull(dr.GetOrdinal("DescripcionEstado")) ? "" : dr.GetString(dr.GetOrdinal("DescripcionEstado")));

        }

        private static BeneficioBloqueado obtenerBeneficioBloqueado(NullableDataReader dr)
        {
            return new BeneficioBloqueado(long.Parse(dr["IdBeneficiario_B"].ToString()),
                                          DateTime.Parse(dr["FecInicio"].ToString()),
                                          dr["FecFin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FecFin"]),
                                          dr["D_PCIA"].ToString(),
                                          dr["Origen"].ToString(),
                                          dr["Causa"].Equals(DBNull.Value) ? "" : dr["Causa"].ToString(),
                                          dr["Juez"].Equals(DBNull.Value) ? "" : dr["Juez"].ToString(),
                                          dr["Secretaria"].Equals(DBNull.Value) ? "" : dr["Secretaria"].ToString(),
                                          dr["Actuacion"].ToString(),
                                          DateTime.Parse(dr["FecNotificacion"].ToString()),
                                          dr["Observaciones"].Equals(DBNull.Value) ? "" : dr["Observaciones"].ToString(),
                                          dr["EntradaCAP"].ToString(),
                                          dr["NroNota"].ToString(),
                                          dr["Firmante"].ToString());
        }

        private static Inhibiciones obtenerInhibiciones(NullableDataReader dr)
        {
            return new Inhibiciones(long.Parse(dr["IdBeneficiario_I"].ToString()),
                                    DateTime.Parse(dr["FecInicio"].ToString()),
                                    dr["FecFin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FecFin"]),
                                    Int64.Parse(dr["CodConceptoLiq"].ToString()),
                                    dr["DescConceptoLiq"].ToString(),
                                    dr["RazonSocial"].Equals(DBNull.Value) ? "" : dr["RazonSocial"].ToString(),
                                    long.Parse(dr["CUIT"].ToString()),
                                    dr["CodSistema"].ToString(),
                                    dr["D_PCIA"].ToString(),
                                    dr["Origen"].ToString(),
                                    dr["Causa"].Equals(DBNull.Value) ? "" : dr["Causa"].ToString(),
                                    dr["Juez"].Equals(DBNull.Value) ? "" : dr["Juez"].ToString(),
                                    dr["Secretaria"].Equals(DBNull.Value) ? "" : dr["Secretaria"].ToString(),
                                    dr["Actuacion"].ToString(),
                                    DateTime.Parse(dr["FecNotificacion"].ToString()),
                                    dr["Observaciones"].Equals(DBNull.Value) ? "" : dr["Observaciones"].ToString(),
                                    dr["EntradaCAP"].ToString(),
                                    dr["NroNota"].ToString(),
                                    dr["Firmante"].ToString()
                                    );

        }

        private static ConceptoAplicado obtenerConceptoAplicado(NullableDataReader dr)
        {

            return new ConceptoAplicado(long.Parse(dr["IdNovedad"].ToString()),
                                         long.Parse(dr["CA_IdBeneficiario"].ToString()),
                                         Int64.Parse(dr["CA_CodConceptoLiq"].ToString()),
                                         dr["CA_DescConceptoLiq"].ToString(),
                                         dr["CA_RazonSocial"].Equals(DBNull.Value) ? "" : dr["CA_RazonSocial"].ToString(),
                                         long.Parse(dr["CA_CUIT"].ToString()),
                                         dr["CA_CodSistema"].ToString(),
                                         dr["ImporteTotal"].Equals(DBNull.Value) ? 0 : Double.Parse(dr["ImporteTotal"].ToString()),
                //dr["CantCuotas"].Equals(DBNull.Value) ? 0 : dr["CantCuotas"].ToString(),
                                          Int16.Parse(dr["CantCuotas"].ToString()),
                                         dr["Porcentaje"].Equals(DBNull.Value) ? 0 : Double.Parse(dr["Porcentaje"].ToString()),
                                         dr["montoPrestamo"].Equals(DBNull.Value) ? 0 : Double.Parse(dr["montoPrestamo"].ToString()),
                                         new TipoConcepto(Int16.Parse(dr["TipoConcepto"].ToString()), dr["DescTipoConcepto"].ToString())
                                        );
        }

        #endregion Trae Datos Completos del Beneficio

        #region Traer
        /// <summary>
        /// Trae una lista de los beneficios que tiene un benficiario
        /// </summary>
        /// <param name="Cuil"></param>
        /// <returns>lista de beneficiario</returns>
        public static List<Beneficiario> Traer(long idBeneficiario, string cuil)
        {
            string sql = "Beneficiarios_Traer";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.String, idBeneficiario.ToString());
                db.AddInParameter(dbCommand, "@Cuil", DbType.String, cuil);

                List<Beneficiario> lstBeneficiario = new List<Beneficiario>();
                string cbuVacio  = "0000000000000000000000";
                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {  
                        lstBeneficiario.Add(new Beneficiario(long.Parse(ds["IdBeneficiario"].ToString()),
                                            long.Parse(ds["Cuil"].ToString()),
                                            int.Parse(ds["TipoDoc"].ToString()),
                                            ds["NroDoc"].ToString(),
                                            ds["ApellidoNombre"].ToString(),
                                            ds["sexo"].ToString(),
                                            double.Parse(ds["SueldoBruto"].ToString()),
                                            double.Parse(ds["SueldoParaOblig"].ToString()),
                                            double.Parse(ds["AfectacionDisponible"].ToString()),
                                            double.Parse(ds["TotObligatoria"].ToString()),
                                            double.Parse(ds["TotNovedad"].ToString()),
                                            int.Parse(ds["CantOcurrenciasDisp"].ToString()),
                                            new Auditoria(ds["Usuario"].ToString(),
                                                          string.Empty,
                                                          DateTime.Parse(ds["FecUltModificacion"].ToString())),
                                            int.Parse(ds["ExAfjp"].ToString()),
                                            int.Parse(ds["IdEstado"].ToString()),
                                            ds["DescripcionEstado"].ToString(),
                                            ds["fVencimiento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(ds["fVencimiento"]),
                                            ds["cbu"] == cbuVacio ? " - " : ds["cbu"].ToString(),
                                            Convert.ToBoolean(ds["esCbuSocial"]),                                           
                                            ds["jubilado_pensionado"] == DBNull.Value ? (char?)null : Convert.ToChar(ds["jubilado_pensionado"]),
                                            ds["leyAplicada"].ToString(),
                                            ds["exCaja"].ToString(),
                                            new TipoOrigenBeneficiario(int.Parse(ds["idOrigenBeneficiario"].ToString()),Convert.ToBoolean(ds["esPNC"])),
                                            new TipoEvaluacionRiesgo(int.Parse(ds["codEvaluacionRiesgo"].ToString()), 
                                                                     Convert.ToBoolean(ds["habilitaCargaCredito"]),
                                                                     ds["mensajeMostrar"].ToString())));    
                    }
                }
                return lstBeneficiario;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> IdBeneficiario:{2} - Cuil: {3}- Error:{4}->{5}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), idBeneficiario, cuil, err.Source, err.Message));
                throw new Exception("Error en BeneficiarioDAO.Traer", err);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        public static List<Beneficiario> Traer(long idBeneficiario, string cuil, bool Reclamos, bool Novedades)
        {
            string sql = "Beneficiarios_Traer";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;

            try
            {
                List<Beneficiario> lstBeneficiario = new List<Beneficiario>();
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.String, idBeneficiario.ToString());
                db.AddInParameter(dbCommand, "@Cuil", DbType.String, cuil);
                dbParametros = dbCommand.Parameters;

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {

                    while (ds.Read())
                    {
                        Beneficiario unBeneficiario = new Beneficiario(long.Parse(ds["IdBeneficiario"].ToString()),
                                                          long.Parse(ds["Cuil"].ToString()),
                                                          int.Parse(ds["TipoDoc"].ToString()),
                                                          ds["NroDoc"].ToString(),
                                                          ds["ApellidoNombre"].ToString(),
                                                          double.Parse(ds["SueldoBruto"].ToString()),
                                                          double.Parse(ds["SueldoParaOblig"].ToString()),
                                                          double.Parse(ds["AfectacionDisponible"].ToString()),
                                                          double.Parse(ds["TotObligatoria"].ToString()),
                                                          double.Parse(ds["TotNovedad"].ToString()),
                                                          int.Parse(ds["CantOcurrenciasDisp"].ToString()),
                                                          new Auditoria(ds["Usuario"].ToString(),
                                                                        string.Empty,
                                                                        DateTime.Parse(ds["FecUltModificacion"].ToString())),
                                                          int.Parse(ds["ExAfjp"].ToString()),
                                                          int.Parse(ds["IdEstado"].ToString()),
                                                          ds["DescripcionEstado"].ToString());

                        if (Reclamos)
                            unBeneficiario.Lista_Reclamos = ReclamoDAO.Reclamo_Traer(long.Parse(ds["IdBeneficiario"].ToString()), 0, 0, 0, DateTime.MinValue, DateTime.MinValue, "");
                        if (Novedades)
                            unBeneficiario.Lista_Novedades = NovedadDAO.Traer_Novedades_xa_Reclamos(long.Parse(ds["IdBeneficiario"].ToString()));
                        lstBeneficiario.Add(unBeneficiario);

                    }
                }
                return lstBeneficiario;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> IdBeneficiario:{2} - Cuil: {3}- Error:{4}->{5}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), idBeneficiario, cuil, ex.Source, ex.Message));
                throw new Exception("Error en BeneficiarioDAO.Traer", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }


        public static List<Beneficiario> Beneficiarios_Traer_XA_Reclamos(long idBeneficiario, string cuil)
        {
            string sql = "Beneficiarios_Traer_XA_Reclamos";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.String, idBeneficiario.ToString());
                db.AddInParameter(dbCommand, "@Cuil", DbType.String, cuil);

                List<Beneficiario> lstBeneficiario = new List<Beneficiario>();

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        lstBeneficiario.Add(new Beneficiario(long.Parse(ds["IdBeneficiario"].ToString()),
                                            long.Parse(ds["Cuil"].ToString()),
                                            int.Parse(ds["TipoDoc"].ToString()),
                                            ds["NroDoc"].ToString(),
                                            ds["ApellidoNombre"].ToString(),
                                            ds["sexo"].ToString(),
                                            double.Parse(ds["SueldoBruto"].ToString()),
                                            double.Parse(ds["SueldoParaOblig"].ToString()),
                                            double.Parse(ds["AfectacionDisponible"].ToString()),
                                            double.Parse(ds["TotObligatoria"].ToString()),
                                            double.Parse(ds["TotNovedad"].ToString()),
                                            int.Parse(ds["CantOcurrenciasDisp"].ToString()),
                                            new Auditoria(ds["Usuario"].ToString(),
                                                          string.Empty,
                                                          DateTime.Parse(ds["FecUltModificacion"].ToString())),
                                            int.Parse(ds["ExAfjp"].ToString()),
                                            int.Parse(ds["IdEstado"].ToString()),
                                            ds["DescripcionEstado"].ToString()
                                           , ds["fVencimiento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(ds["fVencimiento"]),
                                           new TipoOrigenBeneficiario(int.Parse(ds["idOrigenBeneficiario"].ToString()),Convert.ToBoolean(ds["esPNC"]))
                                           ));
                    }
                }
                return lstBeneficiario;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> IdBeneficiario:{2} - Cuil: {3}- Error:{4}->{5}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), idBeneficiario, cuil, err.Source, err.Message));
                throw new Exception("Error en BeneficiarioDAO.Traer", err);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        

        #endregion Traer

        #region Domicilio
        public static Domicilio TraerDomicilio(string cuil, long? idDomicilio)
        {
            string sql = "Domicilio_Beneficiario_TraerUltimo";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@Cuil", DbType.String, cuil);
                db.AddInParameter(dbCommand, "@idDomicilio", DbType.Int64, idDomicilio);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    if (dr.Read())
                    {
                        return new Domicilio(Int64.Parse(dr["idDomicilio"].ToString()),
                                            dr["calle"].Equals(DBNull.Value) ? "" : dr["calle"].ToString(),
                                            dr["numero"].Equals(DBNull.Value) ? "" : dr["numero"].ToString(),
                                            dr["piso"].Equals(DBNull.Value) ? "" : dr["piso"].ToString(),
                                            dr["depto"].Equals(DBNull.Value) ? "" : dr["depto"].ToString(),
                                            dr["codPostal"].Equals(DBNull.Value) ? "" : dr["codPostal"].ToString(),
                                            dr["esCelular1"].Equals(DBNull.Value)? false: Convert.ToBoolean(dr["esCelular1"]),
                                            dr["telediscado1"].Equals(DBNull.Value) ? "" : dr["telediscado1"].ToString(),
                                            dr["telefono1"].Equals(DBNull.Value) ? "" : dr["telefono1"].ToString(),
                                            dr["esCelular2"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["esCelular2"]),
                                            dr["telediscado2"].Equals(DBNull.Value) ? "" : dr["telediscado2"].ToString(),
                                            dr["telefono2"].Equals(DBNull.Value) ? "" : dr["telefono2"].ToString(),
                                            "",
                                            dr["Localidad"].Equals(DBNull.Value) ? "" : dr["Localidad"].ToString(),
                                            new Provincia((dr["codProvincia"].Equals(DBNull.Value) ? short.Parse("0") : short.Parse(dr["codProvincia"].ToString())),
                                                          ""),
                                            DateTime.MinValue,
                                            DateTime.MinValue,
                                            "",
                                            false,
                                            (dr["Mail"].Equals(DBNull.Value) ? "" : dr["Mail"].ToString()),
                                            null);
                    }
                }
                return null;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> IdDomicilio:{2} - Cuil: {3}- Error:{4}->{5}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), idDomicilio, cuil, err.Source, err.Message));
                throw new Exception("Error en BeneficiarioDAO.TraerUltimoDomicilio", err);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }


        public static Int64 GuardarDomicilio(string cuil, Domicilio domicilio)
        {
            string sql = "Domicilio_Beneficiario_Guardar";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@cuil", DbType.String, cuil);
                db.AddInParameter(dbCommand, "@calle", DbType.String, domicilio.Calle);
                db.AddInParameter(dbCommand, "@numero", DbType.String, domicilio.NumeroCalle);
                db.AddInParameter(dbCommand, "@piso", DbType.String, domicilio.Piso);
                db.AddInParameter(dbCommand, "@depto", DbType.String, domicilio.Departamento);
                db.AddInParameter(dbCommand, "@codPostal", DbType.Int16, Int16.Parse(domicilio.CodigoPostal));
                db.AddInParameter(dbCommand, "@localidad", DbType.String, domicilio.Localidad);
                db.AddInParameter(dbCommand, "@codProvincia", DbType.Int16, domicilio.UnaProvincia.CodProvincia);
                db.AddInParameter(dbCommand, "@telediscado1", DbType.String, domicilio.PrefijoTel);
                db.AddInParameter(dbCommand, "@esCelular1", DbType.Boolean, domicilio.EsCelular);
                db.AddInParameter(dbCommand, "@telefono1", DbType.String, domicilio.NumeroTel);
                db.AddInParameter(dbCommand, "@telediscado2", DbType.String, domicilio.PrefijoTel2);
                db.AddInParameter(dbCommand, "@esCelular2", DbType.Boolean, domicilio.EsCelular2);
                db.AddInParameter(dbCommand, "@telefono2", DbType.String, domicilio.NumeroTel2);
                db.AddInParameter(dbCommand, "@mail", DbType.String, domicilio.Mail);
                db.AddInParameter(dbCommand, "@usuario", DbType.String, "");
                db.AddInParameter(dbCommand, "@oficinaCarga", DbType.String, "");
                db.AddInParameter(dbCommand, "@ip", DbType.String, "");
                db.AddInParameter(dbCommand, "@sexo", DbType.String, domicilio.Sexo);
                db.AddInParameter(dbCommand, "@fNacimiento", DbType.DateTime, domicilio.FechaNacimiento);
                db.AddInParameter(dbCommand, "@nacionalidad", DbType.Int16, domicilio.Nacionalidad);

                return Convert.ToInt64(db.ExecuteScalar(dbCommand));
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> Cuil: {2}- Error:{3}->{4}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), cuil, err.Source, err.Message));
                throw new Exception("Error en BeneficiarioDAO.GuardarDomicilio", err);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        #endregion Domicilio

        #region BeneficioBloqueado

        public static String GuardarBeneficioBloqueado(BeneficioBloqueado unBeneficioBloqueado, enum_TipoOperacion accion)
        {

            string sql = "Admin_BloqueoBeneficio_Alta";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            String error = "";
            try
            {
                db.AddInParameter(dbCommand, "@idbeneficiario", DbType.Int64, unBeneficioBloqueado.IdBeneficiario);
                db.AddInParameter(dbCommand, "@fechainicio", DbType.DateTime, unBeneficioBloqueado.FecInicio);

                if (unBeneficioBloqueado.FecFin == null)
                    db.AddInParameter(dbCommand, "@fechafin", DbType.DateTime, DBNull.Value);
                else
                    db.AddInParameter(dbCommand, "@fechafin", DbType.DateTime, unBeneficioBloqueado.FecFin);

                db.AddInParameter(dbCommand, "@codprovincia", DbType.Int16, unBeneficioBloqueado.C_Pcia);
                db.AddInParameter(dbCommand, "@origen", DbType.String, unBeneficioBloqueado.Origen);
                db.AddInParameter(dbCommand, "@causa", DbType.String, unBeneficioBloqueado.Causa);
                db.AddInParameter(dbCommand, "@juez", DbType.String, unBeneficioBloqueado.Juez);
                db.AddInParameter(dbCommand, "@secretaria", DbType.String, unBeneficioBloqueado.Secretaria);
                db.AddInParameter(dbCommand, "@actuacion", DbType.String, unBeneficioBloqueado.Actuacion);

                if (unBeneficioBloqueado.FecNotificacion == null)
                    db.AddInParameter(dbCommand, "@fechanotificacion", DbType.DateTime, DBNull.Value);
                else db.AddInParameter(dbCommand, "@fechanotificacion", DbType.DateTime, unBeneficioBloqueado.FecNotificacion);

                db.AddInParameter(dbCommand, "@observaciones", DbType.String, unBeneficioBloqueado.Observaciones);
                db.AddInParameter(dbCommand, "@ip", DbType.String, unBeneficioBloqueado.IP);
                db.AddInParameter(dbCommand, "@usuario", DbType.String, unBeneficioBloqueado.Usuario);
                db.AddInParameter(dbCommand, "@entradacap", DbType.String, unBeneficioBloqueado.EntradaCAP);
                db.AddInParameter(dbCommand, "@nronota", DbType.String, unBeneficioBloqueado.NroNota);
                db.AddInParameter(dbCommand, "@firmante", DbType.String, unBeneficioBloqueado.Firmante);
                db.AddInParameter(dbCommand, "@oficina", DbType.String, unBeneficioBloqueado.Oficina);
                db.AddInParameter(dbCommand, "@nroNotaBajaBloqueo", DbType.String, unBeneficioBloqueado.NroNotaBajaBloqueo);
                db.AddInParameter(dbCommand, "@usuarioBajaBloqueo", DbType.String, unBeneficioBloqueado.UsuarioBajaBloqueo);
                db.AddInParameter(dbCommand, "@oficinaBajaBloqueo", DbType.String, unBeneficioBloqueado.OficinaBajaBloqueo);

                if (unBeneficioBloqueado.FProcesoBajaBloqueo == null)
                    db.AddInParameter(dbCommand, "@FProcesoBajaBloqueo", DbType.DateTime, DBNull.Value);
                else db.AddInParameter(dbCommand, "@FProcesoBajaBloqueo", DbType.DateTime, unBeneficioBloqueado.FProcesoBajaBloqueo);


                db.AddInParameter(dbCommand, "@ipcierreBajaBloqueo", DbType.String, unBeneficioBloqueado.IpcierreBajaBloqueo);
                db.AddInParameter(dbCommand, "@nroexpedientebajabloqueo", DbType.String, unBeneficioBloqueado.NroExpedienteBajaBloqueo);
                db.AddInParameter(dbCommand, "@accion", DbType.Int16, accion);

                db.AddOutParameter(dbCommand, "@error", DbType.String, 50);

               using (TransactionScope scope = new TransactionScope())
               {
                   if (accion == enum_TipoOperacion.modificacion || accion == enum_TipoOperacion.cierre)
                   {
                        SeguridadLogDAO.AuditarOnlineLog(unBeneficioBloqueado.IdBeneficiario.ToString(), unBeneficioBloqueado, "Beneficios_Bloqueados", LoggingAnses.Servicio.Entidad.TipoAction.ACTUALIZAR);
                   }
                   else if (accion == enum_TipoOperacion.nuevo)
                    {
                        SeguridadLogDAO.AuditarOnlineLog(unBeneficioBloqueado.IdBeneficiario.ToString(), unBeneficioBloqueado, "Beneficios_Bloqueados", LoggingAnses.Servicio.Entidad.TipoAction.AGREGAR);
                    }

                    db.ExecuteNonQuery(dbCommand);
                    scope.Complete();
                }

                return error = db.GetParameterValue(dbCommand, "@error").ToString();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en BeneficiarioDAO.GuardarBeneficioBloqueado", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }


        public static List<BeneficioBloqueado> BeneficioBloqueado_Traer(Int64 IdBeneficiario)
        {
            string sql = "Beneficios_Bloqueados_Trae";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<BeneficioBloqueado> listaRdo = new List<BeneficioBloqueado>();

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, IdBeneficiario);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {                        
                        BeneficioBloqueado unBeneficioBloqueado = new BeneficioBloqueado(long.Parse(dr["IdBeneficiario"].ToString()),
                                                                                         long.Parse(dr["Cuil"].ToString()),
                                                                                         dr["ApellidoNombre"].Equals(DBNull.Value) ? "" : dr["ApellidoNombre"].ToString(),
                                                                                         DateTime.Parse(dr["FecInicio"].ToString()),
                                                                                         dr["FecFin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FecFin"]),
                                                                                         dr["Origen"].Equals(DBNull.Value) ? "" : dr["Origen"].ToString(),
                                                                                         dr["EntradaCAP"].Equals(DBNull.Value) ? "": dr["EntradaCAP"].ToString(),
                                                                                         dr["C_Pcia"].Equals(DBNull.Value) ? (Int16) 0 : Int16.Parse(dr["C_Pcia"].ToString()),
                                                                                         dr["Causa"].Equals(DBNull.Value) ? "" : dr["Causa"].ToString(),
                                                                                         dr["Juez"].Equals(DBNull.Value) ? "" : dr["Juez"].ToString(),
                                                                                         dr["Secretaria"].Equals(DBNull.Value) ? "" : dr["Secretaria"].ToString(),
                                                                                         dr["Actuacion"].Equals(DBNull.Value) ? "" : dr["Actuacion"].ToString(),
                                                                                         dr["FecNotificacion"].Equals(DBNull.Value) ? DateTime.MinValue : DateTime.Parse(dr["FecNotificacion"].ToString()),
                                                                                         dr["NroNota"].Equals(DBNull.Value) ? "": dr["NroNota"].ToString(),
                                                                                         dr["Firmante"].Equals(DBNull.Value) ? "": dr["Firmante"].ToString(),
                                                                                         dr["Observaciones"].Equals(DBNull.Value) ? "" : dr["Observaciones"].ToString(),
                                                                                         dr["Usuario"].Equals(DBNull.Value) ? "" :  dr["Usuario"].ToString(),
                                                                                         dr["FecUltModificación"].Equals(DBNull.Value) ? DateTime.MinValue : DateTime.Parse(dr["FecUltModificación"].ToString()),
                                                                                         dr["ip"].Equals(DBNull.Value) ? "" : dr["ip"].ToString(),
                                                                                         dr["oficina"].Equals(DBNull.Value) ? "" :  dr["oficina"].ToString(),
                                                                                         dr["nroNotaBajaBloqueo"].Equals(DBNull.Value) ? "" : dr["nroNotaBajaBloqueo"].ToString(),
                                                                                         dr["usuarioBajaBloqueo"].Equals(DBNull.Value) ? "" : dr["usuarioBajaBloqueo"].ToString(),
                                                                                         dr["oficinaBajaBloqueo"].Equals(DBNull.Value) ? "" : dr["oficinaBajaBloqueo"].ToString(),
                                                                                         dr["FProcesoBajaBloqueo"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FProcesoBajaBloqueo"]),
                                                                                         dr["ipcierreBajaBloqueo"].Equals(DBNull.Value) ? "" : dr["ipcierreBajaBloqueo"].ToString(),
                                                                                         dr["nroexpedientebajabloqueo"].Equals(DBNull.Value) ? "" : dr["nroexpedientebajabloqueo"].ToString()
                                                                                         );
                        listaRdo.Add(unBeneficioBloqueado);
                    }
                }
                return listaRdo;

            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception(" Error en BeneficioBloqueado_Buscar", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region Inhibiciones


        private static String GuardarBeneficioInhibicion(Inhibiciones unInhibiciones, enum_TipoOperacion accion)
        {
            string sql = "Admin_Inhibicion_Alta";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@idprestador", DbType.Int64, unInhibiciones.IdPrestador);
                db.AddInParameter(dbCommand, "@idbeneficiario", DbType.Int64, unInhibiciones.IdBeneficiario);
                db.AddInParameter(dbCommand, "@codconceptoliq", DbType.Int32, unInhibiciones.CodConceptoLiq);
                db.AddInParameter(dbCommand, "@fechaInicio", DbType.DateTime, unInhibiciones.FecInicio);

                if (unInhibiciones.FecFin == null)
                    db.AddInParameter(dbCommand, "@fechafin", DbType.DateTime, DBNull.Value);
                else db.AddInParameter(dbCommand, "@fechafin", DbType.DateTime, unInhibiciones.FecFin);

                db.AddInParameter(dbCommand, "@codprovincia", DbType.Int16, unInhibiciones.C_Pcia);
                db.AddInParameter(dbCommand, "@origen", DbType.String, unInhibiciones.Origen);
                db.AddInParameter(dbCommand, "@causa", DbType.String, unInhibiciones.Causa);
                db.AddInParameter(dbCommand, "@juez", DbType.String, unInhibiciones.Juez);
                db.AddInParameter(dbCommand, "@secretaria", DbType.String, unInhibiciones.Secretaria);
                db.AddInParameter(dbCommand, "@actuacion", DbType.String, unInhibiciones.Actuacion);

                if (unInhibiciones.FecNotificacion == null)
                    db.AddInParameter(dbCommand, "@fechanotificacion", DbType.DateTime, DBNull.Value);
                else db.AddInParameter(dbCommand, "@fechanotificacion", DbType.DateTime, unInhibiciones.FecNotificacion);

                db.AddInParameter(dbCommand, "@observaciones", DbType.String, unInhibiciones.Observaciones);
                db.AddInParameter(dbCommand, "@ip", DbType.String, unInhibiciones.IP);
                db.AddInParameter(dbCommand, "@usuario", DbType.String, unInhibiciones.Usuario);
                db.AddInParameter(dbCommand, "@entradacap", DbType.String, unInhibiciones.EntradaCAP);
                db.AddInParameter(dbCommand, "@nronota", DbType.String, unInhibiciones.NroNota);
                db.AddInParameter(dbCommand, "@firmante", DbType.String, unInhibiciones.Firmante);
                db.AddInParameter(dbCommand, "@oficina", DbType.String, unInhibiciones.Oficina);
                db.AddInParameter(dbCommand, "@nroNotaBajaIn", DbType.String, unInhibiciones.NroNotaBajaIn);
                db.AddInParameter(dbCommand, "@usuarioBajaIn", DbType.String, unInhibiciones.UsuarioBajaIn);
                db.AddInParameter(dbCommand, "@oficinaBajaIn", DbType.String, unInhibiciones.OficinaBajaIn);

                if (unInhibiciones.FProcesoBajaIn == null)
                    db.AddInParameter(dbCommand, "@FProcesoBajaIn", DbType.DateTime, DBNull.Value);
                else db.AddInParameter(dbCommand, "@FProcesoBajaIn", DbType.DateTime, unInhibiciones.FProcesoBajaIn);

                db.AddInParameter(dbCommand, "@ipcierreBajaIn", DbType.String, unInhibiciones.IpcierreBajaIn);
                db.AddInParameter(dbCommand, "@nroexpedientebajaIn", DbType.String, unInhibiciones.NroExpedienteBajaIn);
                db.AddInParameter(dbCommand, "@accion", DbType.Int16, accion);
                db.AddOutParameter(dbCommand, "@error", DbType.String, 50);

               if (accion == enum_TipoOperacion.modificacion || accion == enum_TipoOperacion.cierre)
               {
                   //SeguridadLogDAO.GuardarLog("Inhibiciones", "M", dbCommand.Parameters);
                   SeguridadLogDAO.AuditarOnlineLog(unInhibiciones.IdBeneficiario.ToString(), unInhibiciones, "Inhibiciones", LoggingAnses.Servicio.Entidad.TipoAction.ACTUALIZAR);
               }
               else if (accion == enum_TipoOperacion.nuevo)
               {
                    SeguridadLogDAO.AuditarOnlineLog(unInhibiciones.IdBeneficiario.ToString(), unInhibiciones, "Inhibiciones", LoggingAnses.Servicio.Entidad.TipoAction.AGREGAR);
               }

                db.ExecuteNonQuery(dbCommand);

                return db.GetParameterValue(dbCommand, "@error").ToString();

            }
            catch (DbException sqlErr)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlErr.Source, sqlErr.Message));

                if (((System.Data.SqlClient.SqlException)(sqlErr)).Number >= 50000)
                    return ((System.Exception)(sqlErr)).Message;
                else throw sqlErr;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        public static string AltaInhibiciones(List<Inhibiciones> listaInhibiciones, enum_TipoOperacion accion)
        {
            string error = String.Empty;

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (Inhibiciones i in listaInhibiciones)
                    {
                        error = GuardarBeneficioInhibicion(i, accion);

                        if (!error.Equals(""))
                        {
                            return error;
                        }
                        else error = string.Empty;

                    }
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return error;
        }

        public static List<Inhibiciones> Inhibiciones_Traer(Int64 IdBeneficiario)
        {
            string sql = "Inhibiciones_Trae";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Inhibiciones> listaRdo = new List<Inhibiciones>();

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, IdBeneficiario);
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        Inhibiciones unInhibiciones = new Inhibiciones(long.Parse(dr["IdBeneficiario"].ToString()),
                                                                       long.Parse(dr["IdPrestador"].ToString()),
                                                                       dr["RazonSocial"].Equals(DBNull.Value) ? "" : dr["RazonSocial"].ToString(),
                                                                       Int64.Parse(dr["CodConceptoLiq"].ToString()),
                                                                       long.Parse(dr["Cuil"].ToString()),
                                                                       dr["ApellidoNombre"].Equals(DBNull.Value) ? "" : dr["ApellidoNombre"].ToString(),
                                                                       DateTime.Parse(dr["FecInicio"].ToString()),
                                                                       dr["FecFin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FecFin"]),
                                                                       dr["Origen"].ToString(),
                                                                       dr["EntradaCAP"].ToString(),
                                                                       short.Parse(dr["C_Pcia"].ToString()),
                                                                       dr["D_PCIA"].Equals(DBNull.Value) ? "" : dr["D_PCIA"].ToString(),
                                                                       dr["Causa"].Equals(DBNull.Value) ? "" : dr["Causa"].ToString(),
                                                                       dr["Juez"].Equals(DBNull.Value) ? "" : dr["Juez"].ToString(),
                                                                       dr["Secretaria"].Equals(DBNull.Value) ? "" : dr["Secretaria"].ToString(),
                                                                       dr["Actuacion"].Equals(DBNull.Value) ? "" : dr["Actuacion"].ToString(),
                                                                       DateTime.Parse(dr["FecNotificacion"].ToString()),
                                                                       dr["NroNota"].ToString(),
                                                                       dr["Firmante"].ToString(),
                                                                       dr["Observaciones"].Equals(DBNull.Value) ? "" : dr["Observaciones"].ToString(),
                                                                       dr["Usuario"].ToString(),
                                                                       DateTime.Parse(dr["FecUltModificacion"].ToString()),
                                                                       dr["ip"].ToString(),
                                                                       dr["oficina"].ToString(),
                                                                       dr["nroNotaBajaIn"].Equals(DBNull.Value) ? "" : dr["nroNotaBajaIn"].ToString(),
                                                                       dr["usuarioBajaIn"].Equals(DBNull.Value) ? "" : dr["usuarioBajaIn"].ToString(),
                                                                       dr["oficinaBajaIn"].Equals(DBNull.Value) ? "" : dr["oficinaBajaIn"].ToString(),
                                                                       dr["FProcesoBajaIn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["FProcesoBajaIn"]),
                                                                       dr["ipcierreBajaIn"].Equals(DBNull.Value) ? "" : dr["ipcierreBajaIn"].ToString(),
                                                                       dr["nroexpedienteBajaIn"].Equals(DBNull.Value) ? "" : dr["nroexpedienteBajaIn"].ToString()
                                                                       );

                        listaRdo.Add(unInhibiciones);
                    }
                }
                return listaRdo;

            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> IdBeneficiario: {2}- Error:{3}->{4}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), IdBeneficiario, ex.Source, ex.Message));
                throw new Exception(" Error en BeneficioBloqueado_Buscar", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        public static List<Beneficiario> TraerBeneficiario_HabXaTarjetas(string Cuil)
        {
            string sql = "Beneficiarios_Traer_HabXaTarjetas";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Beneficiario> lstBeneficiario = new List<Beneficiario>();
           try
            {
               
                db.AddInParameter(dbCommand, "@Cuil", DbType.String, Cuil);
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        Beneficiario b = new Beneficiario(long.Parse(dr["IdBeneficiario"].ToString()), long.Parse(dr["Cuil"].ToString()), dr["ApellidoNombre"].ToString(),
                                                          double.Parse(dr["SueldoBruto"].ToString()), double.Parse(dr["SueldoParaOblig"].ToString()),
                                                          double.Parse(dr["AfectacionDisponible"].ToString()), double.Parse(dr["TotObligatoria"].ToString()),
                                                          double.Parse(dr["TotNovedad"].ToString()), int.Parse(dr["CantOcurrenciasDisp"].ToString()),
                                                          new Estado(int.Parse(dr["IdEstado"].ToString()), dr["DescripcionEstado"].ToString()),
                                                          int.Parse(dr["TipoDoc"].ToString()), dr["NroDoc"].ToString(),
                                                          new Auditoria(dr["Usuario"].ToString(), string.Empty, DateTime.Parse(dr["FecUltModificacion"].ToString())),
                                                          int.Parse(dr["ExAfjp"].ToString()), 
                                                          dr["cbu"].ToString(), bool.Parse(dr["esCbuSocial"].ToString()),
                                                          bool.Parse(dr["HabilitadoSolicitarTarjeta"].ToString()));

                        b.unDomicilio = int.Parse(dr["iddomicilio"].ToString()) == 0 ? null : TraerDomicilio(b.Cuil.ToString(), long.Parse(dr["iddomicilio"].ToString()));
                        lstBeneficiario.Add(b);                    
                    }
                }
           
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> CUIL:{2} - Error:{3}->{4}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), Cuil, err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
            return lstBeneficiario;

        }

        # region Datos Banco

        public static List<BeneficiarioCBU> Benefeciarios_CBU_XCuil(Int64 cuil, out string mensaje)
        {
            mensaje = string.Empty;
            string sql = "Benefeciarios_CBU_XCuil";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<BeneficiarioCBU> lstBeneficiario = new List<BeneficiarioCBU>();
           try
            {

                db.AddInParameter(dbCommand, "@Cuil", DbType.Int64, cuil);
                db.AddOutParameter(dbCommand, "@mensaje", DbType.String,500);

                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        BeneficiarioCBU b = new BeneficiarioCBU();
                        b.IdBeneficiario = long.Parse(dr["IdBeneficiario"].ToString());
                        b.Cuil = long.Parse(dr["cuil"].ToString());
                        b.CBU = dr["cbu"].ToString();
                        b.codBanco = dr["codbanco"].ToString();
                        b.denominacionBanco = dr["denominacionBanco"].ToString();
                        b.codAgencia = dr["codAgencia"].ToString();
                        b.denominacionAgencia = dr["denominacionAgencia"].ToString();
                        b.calle = dr["calle"].ToString();
                        b.localidad = dr["localidad"].ToString();
                        b.codpostal = dr["codpostal"].ToString();
                        b.idprovincia =   string.IsNullOrEmpty(dr["idprovincia"].ToString()) ? 0 : int.Parse(dr["idprovincia"].ToString());
                        b.descripcionProvincia = dr["descripcionProvincia"].ToString();
                            
                        lstBeneficiario.Add(b);                    
                    }
                }
                mensaje = db.GetParameterValue(dbCommand, "@mensaje").ToString();
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> CUIL:{2} - Error:{3}->{4}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), cuil, err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
            return lstBeneficiario;

        }   
        
        #endregion
    }
}
