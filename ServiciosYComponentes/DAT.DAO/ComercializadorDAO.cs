using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NullableReaders;
using System.Data.SqlClient;
using System.Transactions;
using Ar.Gov.Anses.Microinformatica.AuditoriaLog;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class ComercializadorDAO : IDisposable
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(ComercializadorDAO).Name);
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

        ~ComercializadorDAO()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        /// <summary>
        /// Trae una lista de comercializadoras asociadas a un prestador
        /// </summary>
        /// <param name="idPrestador">id del Prestador</param>
        /// <returns></returns>
        public static List<Comercializador> TraerComericalizadoras_xidPrestador(long idPrestador)
        {
            string sql = "Comercializador_TXPrestador";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                List<Comercializador> lstComercializadoras = new List<Comercializador>();
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        lstComercializadoras.Add(
                             new Comercializador(long.Parse(dr["idComercializador"].ToString()),
                                                 dr["RazonSocial"].ToString(),
                                                 long.Parse(dr["CuitComercializador"].ToString()),
                                                 dr["Observaciones"].Equals(DBNull.Value) ? "" : dr["Observaciones"].ToString(),
                                                 new Estado(int.Parse(dr["idEstado"].ToString())),
                                                 new Auditoria(dr["Usuario"].ToString(),
                                                               null,//dr["IPOrigen"].Equals(DBNull.Value) ? string.Empty : dr["IP"].ToString(),
                                                               dr["FecUltModificacion"].Equals(DBNull.Value) ? new DateTime?() : DateTime.Parse(dr["FecUltModificacion"].ToString())),
                                                 dr["NombreFantasia"].Equals(DBNull.Value) ? string.Empty : dr["NombreFantasia"].ToString(),
                                                 dr["FInicio"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FInicio"].ToString()),
                                                 dr["FFin"].Equals(DBNull.Value) ? new DateTime?() : DateTime.Parse(dr["FFin"].ToString())));
                    }
                }
                return lstComercializadoras;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en ComercializadorDAO.TraerComericalizadoras_xCuitPrestador", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        /// <summary>
        /// Trae una lista de comercializadoras asociadas a un prestador
        /// </summary>
        /// <param name="idPrestador">id del Prestador</param>
        /// <returns></returns>
        public static Comercializador TraerComericalizadoras_xCuit(string cuit)
        {
            string sql = "Comercializador_TXCuit";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                Comercializador unaComercializadora = new Comercializador();
                db.AddInParameter(dbCommand, "@cuit", DbType.Int64, long.Parse(cuit));

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        //Auditoria unAuditoria = new Auditoria(dr["Usuario"].ToString(), null, DateTime.Parse(dr["FecUltModificacion"].ToString()));

                        unaComercializadora = new Comercializador(long.Parse(dr["idComercializador"].ToString()),
                                                 dr.GetString("RazonSocial"),
                                                 long.Parse(dr["CuitComercializador"].ToString()),
                                                 "",
                                                 new Estado(int.Parse(dr.GetByte("idEstado").ToString())),
                                                 new Auditoria(dr["Usuario"].ToString(), null, DateTime.Parse(dr["FecUltModificacion"].ToString())),
                                                 dr.GetString("NombreFantasia"),
                                                 new DateTime(1900, 1, 1),
                                                 new DateTime(1900, 1, 1));
                    }
                }
                return unaComercializadora;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en ComercializadorDAO.TraerComericalizadoras_xCuitPrestador", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        /// <summary>
        /// Trae una lista de comercializadoras asociadas a un prestador
        /// </summary>
        /// <param name="idPrestador">id del Prestador</param>
        /// <param name="idComercializador">id del Comercializador</param>
        /// <returns></returns>
        public static List<Comercializador> TraerDomiciliosComercializador_T_PrestadorComercializador(long idPrestador, long idComercializador)
        {
            string sql = "DomicilioComercializador_T_PrestadorComercializador";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                List<Comercializador> lstComercializadoras = new List<Comercializador>();
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@idComercializador", DbType.Int64, idComercializador);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    List<Domicilio> listDomicilios = new List<Domicilio>();
                    while (dr.Read())
                    {
                        lstComercializadoras.Add(new Comercializador(long.Parse(dr["idComercializador"].ToString()),
                                                 dr.GetString("RazonSocial"),
                                                 long.Parse(dr["CuitComercializador"].ToString()), "",
                                                 new Estado(),
                                                 new Auditoria(dr["Usuario"].ToString(), null, new DateTime()),
                                                 dr.GetString("NombreFantasia"),
                                                 new DateTime(),
                                                 new DateTime?(),
                                                 new Domicilio(Int64.Parse(dr["idDomicilioComercializador"].ToString()),
                                                               dr["Calle"].Equals(DBNull.Value) ? "" : dr["Calle"].ToString(),
                                                               dr["Nro"].Equals(DBNull.Value) ? "" : dr["Nro"].ToString(),
                                                               dr["Piso"].Equals(DBNull.Value) ? "" : dr["Piso"].ToString(),
                                                               dr["Dpto"].Equals(DBNull.Value) ? "" : dr["Dpto"].ToString(),
                                                               dr["CodPostal"].Equals(DBNull.Value) ? "" : dr["CodPostal"].ToString(),
                                                               false,
                                                               dr["Tel_Prefijo"].Equals(DBNull.Value) ? "" : dr["Tel_Prefijo"].ToString(),
                                                               dr["Tel_Nro"].Equals(DBNull.Value) ? "" : dr["Tel_Nro"].ToString(),
                                                               false,string.Empty, string.Empty,
                                                               dr["Fax"].Equals(DBNull.Value) ? "" : dr["Fax"].ToString(),
                                                               dr["Localidad"].Equals(DBNull.Value) ? "" : dr["Localidad"].ToString(),
                                                               new Provincia(short.Parse(dr["C_Pcia"].ToString()),
                                                                             dr["D_PCIA"].Equals(DBNull.Value) ? "" : dr["D_PCIA"].ToString().Trim()),
                                                               DateTime.Parse(dr["FInicio"].ToString()),
                                                               dr["FFin"].Equals(DBNull.Value) ? new DateTime?() : DateTime.Parse(dr["FFin"].ToString()),
                                                               dr["Observaciones"].Equals(DBNull.Value) ? "" : dr["Observaciones"].ToString(),
                                                               bool.Parse(dr["EsSucursal"].ToString()),
                                                               (dr["Mail"].Equals(DBNull.Value) ? "" : dr["Mail"].ToString()),
                                                               new TipoDomicilio(short.Parse(dr["IdTipoDomicilio"].ToString()),
                                                                                 dr["Descripcion"].Equals(DBNull.Value) ? "" : dr["Descripcion"].ToString()))));
                    }
                }
                return lstComercializadoras;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en ComercializadorDAO.TraerDomiciliosComercializador_T_PrestadorComercializador", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        /// <summary>
        /// Trae una lista de comercializadoras de distinto prestador
        /// </summary>
        /// <param name="idPrestador">id del Prestador</param>
        /// <param name="idComercializador">id del Comercializador</param>
        /// <returns></returns>
        public static List<Comercializador> TraerDomicilioComercializador_T_ComercializadorDistintoIDPrestador(long idPrestador, long idComercializador)
        {
            string sql = "DomicilioComercializador_T_ComercializadorDistintoIDPrestador";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                List<Comercializador> lstComercializadoras = new List<Comercializador>();

                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@idComercializador", DbType.Int64, idComercializador);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    List<Domicilio> listDomicilios = new List<Domicilio>();
                    while (dr.Read())
                    {
                        lstComercializadoras.Add(new Comercializador(long.Parse(dr["idComercializador"].ToString()),
                                                 dr.GetString("RazonSocial"),
                                                 long.Parse(dr["CuitComercializador"].ToString()),
                                                 string.Empty,
                                                 new Estado(),
                                                 new Auditoria(dr["Usuario"].ToString(), null, new DateTime()),
                                                 dr.GetString("NombreFantasia"),
                                                 new DateTime(),
                                                 new DateTime?(),
                                                 new Domicilio(Int64.Parse(dr["idDomicilioComercializador"].ToString()),
                                                               dr["Calle"].Equals(DBNull.Value) ? "" : dr["Calle"].ToString(),
                                                               dr["Nro"].Equals(DBNull.Value) ? "" : dr["Nro"].ToString(),
                                                               dr["Piso"].Equals(DBNull.Value) ? "" : dr["Piso"].ToString(),
                                                               dr["Dpto"].Equals(DBNull.Value) ? "" : dr["Dpto"].ToString(),
                                                               dr["CodPostal"].Equals(DBNull.Value) ? "" : dr["CodPostal"].ToString(),
                                                               false,
                                                               dr["Tel_Prefijo"].Equals(DBNull.Value) ? "" : dr["Tel_Prefijo"].ToString(),
                                                               dr["Tel_Nro"].Equals(DBNull.Value) ? "" : dr["Tel_Nro"].ToString(),
                                                               false, string.Empty,string.Empty,
                                                               dr["Fax"].Equals(DBNull.Value) ? "" : dr["Fax"].ToString(),
                                                               dr["Localidad"].Equals(DBNull.Value) ? "" : dr["Localidad"].ToString(),
                                                               new Provincia(short.Parse(dr["C_Pcia"].ToString()),
                                                                             dr["D_PCIA"].Equals(DBNull.Value) ? "" : dr["D_PCIA"].ToString().Trim()),
                                                               dr["FInicio"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FInicio"].ToString()),
                                                               dr["FFin"].Equals(DBNull.Value) ? new DateTime?() : DateTime.Parse(dr["FFin"].ToString()),
                                                               dr["Observaciones"].Equals(DBNull.Value) ? "" : dr["Observaciones"].ToString(),
                                                               bool.Parse(dr["EsSucursal"].ToString()),
                                                               (dr["Mail"].Equals(DBNull.Value) ? "" : dr["Mail"].ToString()),
                                                               new TipoDomicilio(short.Parse(dr["IdTipoDomicilio"].ToString()),
                                                                                 dr["Descripcion"].Equals(DBNull.Value) ? "" : dr["Descripcion"].ToString()))));
                    }
                }
                return lstComercializadoras;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en ComercializadorDAO.TraerDomiciliosComercializador_T_PrestadorComercializador", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        public static bool DomicilioComercializador_BuscarComercializadorDistintoIDPrestador(long idPrestador, long idDomicilioComercializador)
        {
            string sql = "DomicilioComercializador_T_DomicilioRelacionadoOtroPrestador";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string mensage = string.Empty;

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@idDomicilioComercializador", DbType.Int64, idDomicilioComercializador);

                return ((System.Data.SqlClient.SqlDataReader)db.ExecuteReader(dbCommand)).HasRows;

            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en ComercializadorDAO.BuscarDomicilioComercializador_T_ComercializadorDistintoIDPrestador ", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        
        public static bool DomicilioComercializador_BuscarIgual(string calle, string nro, string piso,
                                                                string dPto, string codPostal)
        {
            string sql = "DomicilioComercializador_BuscarIgual";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string mensage = string.Empty;

            try
            {
                db.AddInParameter(dbCommand, "@calle", DbType.String, calle);
                db.AddInParameter(dbCommand, "@nro", DbType.String, nro);
                db.AddInParameter(dbCommand, "@piso", DbType.String, piso);
                db.AddInParameter(dbCommand, "@dpto", DbType.String, dPto);
                db.AddInParameter(dbCommand, "@codpost", DbType.String, codPostal);

                return ((System.Data.SqlClient.SqlDataReader)db.ExecuteReader(dbCommand)).HasRows;

                //return (db.ExecuteReader(dbCommand).FieldCount > 0);
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en ComercializadorDAO.DomicilioComercializador_BurcarIgual ", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        /// <summary>
        /// Relacionar/Asignar comercializadora a un prestador
        /// </summary>
        /// <param name="idPrestador">id del Prestador</param>
        /// <param name="idPrestador">Objeto Comercializador</param>
        /// <returns></returns>
        public static string Relacion_ComercializadorPrestador_A(long idPrestador, Comercializador unComercializador)
        {
            string mensage = null;
            try
            {

                using (TransactionScope oTransactionScope = new TransactionScope())
                {
                    if (unComercializador.ID == 0)
                        unComercializador.ID = ComercializadorA(idPrestador, unComercializador);

                    if (unComercializador.ID != 0)
                    {
                        Relacion_ComercializadorPrestador_Asignar(idPrestador, unComercializador);

                        //if (string.IsNullOrEmpty(mensage))
                        oTransactionScope.Complete();
                    }
                }
            }
            catch (SqlException sqlex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlex.Source, sqlex.Message));
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
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en ComercializadorDAO.Relacion_ComercializadorPrestador_A ", ex);
            }

            return mensage;
        }

        /// <summary>
        /// Relacionar/Asignar comercializadora a un prestador
        /// </summary>
        /// <param name="idPrestador">id del Prestador</param>
        /// <param name="idPrestador">Objeto Comercializador</param>
        /// <returns></returns>
        public static string Relacion_ComercializadorPrestador_Asignar(long idPrestador, Comercializador unComercializador)
        {
            string sql = "RelacionComercializadorPrestador_Asignar";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string mensage = string.Empty;

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@idComercializador", DbType.Int64, unComercializador.ID);
                db.AddInParameter(dbCommand, "@FInicio", DbType.DateTime, unComercializador.FechaInicio);
                db.AddInParameter(dbCommand, "@FFin", DbType.DateTime, unComercializador.FechaFin);
                db.AddInParameter(dbCommand, "@Observaciones", DbType.String, unComercializador.Observaciones);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, unComercializador.UnAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@IPOrigen", DbType.String, unComercializador.UnAuditoria.IP);
                db.AddInParameter(dbCommand, "@NroOficina", DbType.Int32, unComercializador.UnAuditoria.IDOficina);
                db.ExecuteNonQuery(dbCommand);
            }
            catch (SqlException sqlex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlex.Source, sqlex.Message));
                throw sqlex;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en ComercializadorDAO.Relacion_ComercializadorPrestador_Asignar ", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return mensage;
        }

        /// <summary>
        /// Alta comercializadora
        /// </summary>
        /// <param name="idPrestador">id del Prestador</param>
        /// <param name="idPrestador">Objeto Comercializador</param>
        /// <returns></returns>
        public static Int64 ComercializadorA(long idPrestador, Comercializador unComercializador)
        {
            Int64 idComercializador = 0;
            string sql = "Comercializador_A";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@Cuit", DbType.String, unComercializador.Cuit);
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@FInicio", DbType.DateTime, unComercializador.FechaInicio);
                db.AddInParameter(dbCommand, "@FFin", DbType.DateTime, unComercializador.FechaFin);
                db.AddInParameter(dbCommand, "@RazonSocial", DbType.String, unComercializador.RazonSocial);
                db.AddInParameter(dbCommand, "@NombreFantasia", DbType.String, (string.IsNullOrEmpty(unComercializador.NombreFantasia) ? null : unComercializador.NombreFantasia));
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, unComercializador.UnAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@IPOrigen", DbType.String, unComercializador.UnAuditoria.IP);
                db.AddInParameter(dbCommand, "@NroOficina", DbType.Int32, unComercializador.UnAuditoria.IDOficina);

                idComercializador = Int64.Parse(db.ExecuteScalar(dbCommand).ToString());
            }
            catch (SqlException sqlex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlex.Source, sqlex.Message));
                throw sqlex;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message)); 
                throw new Exception("Error en ComercializadorA ", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return idComercializador;
        }

        /// <summary>
        /// Modifica/Baja una relacion comercializadora / prestador
        /// </summary>
        /// <param name="idPrestador">id del Prestador</param>
        /// <param name="idPrestador">Objeto Comercializador</param>
        /// <returns></returns>
        public static string Relacion_ComercializadorPrestadorMB(long idPrestador, Comercializador unComercializador)
        {
            string sql = "Relacion_ComercializadorPrestador_MB";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string mensage = string.Empty;

            try
            {
                //ComercializadorM(idPrestador, unComercializador);

                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@idComercializador", DbType.Int64, unComercializador.ID);
                db.AddInParameter(dbCommand, "@FInicio", DbType.DateTime, unComercializador.FechaInicio);
                db.AddInParameter(dbCommand, "@FFin", DbType.DateTime, unComercializador.FechaFin);
                db.AddInParameter(dbCommand, "@Observaciones", DbType.String, unComercializador.Observaciones);

                db.AddInParameter(dbCommand, "@Usuario", DbType.String, unComercializador.UnAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@IPOrigen", DbType.String, unComercializador.UnAuditoria.IP);
                db.AddInParameter(dbCommand, "@NroOficina", DbType.Int32, unComercializador.UnAuditoria.IDOficina);

                using (TransactionScope oTransactionScope = new TransactionScope())
                {
                    SeguridadLogDAO.GuardarLog("RelacionComercializadorPrestador", "M", dbCommand.Parameters);
                    
                    db.ExecuteReader(dbCommand);

                    oTransactionScope.Complete();
                }
            }
            catch (SqlException sqlex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlex.Source, sqlex.Message));
               
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
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en ComercializadorDAO.Relacion_ComercializadorPrestadorMB ", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return mensage;
        }

        /// <summary>
        /// Modifica comercializadora
        /// </summary>
        /// <param name="idPrestador">id del Prestador</param>
        /// <param name="idPrestador">Objeto Comercializador</param>
        /// <returns></returns>
       
        public static string ComercializadorM(long idPrestador, Comercializador unComercializador)
        {
            string sql = "Comercializador_M";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string mensage = string.Empty;

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@idComercializador", DbType.Int64, unComercializador.ID);
                db.AddInParameter(dbCommand, "@FInicio", DbType.DateTime, unComercializador.FechaInicio);
                db.AddInParameter(dbCommand, "@FFin", DbType.DateTime, !unComercializador.FechaFin.HasValue ? new DateTime?() : unComercializador.FechaFin);
                db.AddInParameter(dbCommand, "@NombreFantasia", DbType.String, (string.IsNullOrEmpty(unComercializador.NombreFantasia) ? null : unComercializador.NombreFantasia));

                db.AddInParameter(dbCommand, "@Usuario", DbType.String, unComercializador.UnAuditoria.CodigoUsuario);
                db.AddInParameter(dbCommand, "@IPOrigen", DbType.String, unComercializador.UnAuditoria.IP);
                db.AddInParameter(dbCommand, "@NroOficina", DbType.Int32, unComercializador.UnAuditoria.IDOficina);

                db.ExecuteNonQuery(dbCommand);
            }
            catch (SqlException sqlex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlex.Source, sqlex.Message));

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
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en ComercializadorDAO.ComercializadorM ", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return mensage;
        }

        public static string RelacionComercializadorPrestador_B(long idPrestador, Comercializador unComercializador)
        {
            string sql = "RelacionComercializadorPrestador_B";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string mensage = string.Empty;

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@idComercializador", DbType.Int64, unComercializador.ID);
                db.AddInParameter(dbCommand, "@FechaInicio", DbType.DateTime, unComercializador.FechaInicio);
                db.AddInParameter(dbCommand, "@FFin_Baja", DbType.DateTime, !unComercializador.FechaFin.HasValue ? new DateTime?() : unComercializador.FechaFin);

                db.AddInParameter(dbCommand, "@Usuario", DbType.String, unComercializador.UnAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@IPOrigen", DbType.String, unComercializador.UnAuditoria.IP);
                db.AddInParameter(dbCommand, "@NroOficina", DbType.Int32, unComercializador.UnAuditoria.IDOficina);

                using (TransactionScope oTransactionScope = new TransactionScope())
                {
                    SeguridadLogDAO.GuardarLog("RelacionComercializadorPrestador", "M", dbCommand.Parameters);

                    db.ExecuteNonQuery(dbCommand);

                    oTransactionScope.Complete(); 
                }
            }
            catch (SqlException sqlex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlex.Source, sqlex.Message));
               
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
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en ComercializadorDAO.ComercializadorM ", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return mensage;
        }

        //public static string RelacionComercializadorPrestador_BFisica(long idPrestador, Comercializador unComercializador)
        //{
        //    string sql = "RelacionComercializadorPrestador_BFisica";
        //    Database db = DatabaseFactory.CreateDatabase("DAT_V01");
        //    DbCommand dbCommand = db.GetStoredProcCommand(sql);
        //    string mensage = string.Empty;

        //    try
        //    {
        //        db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
        //        db.AddInParameter(dbCommand, "@idComercializador", DbType.Int64, unComercializador.ID);
        //        db.AddInParameter(dbCommand, "@FFin_Baja", DbType.DateTime, !unComercializador.FechaFin.HasValue ? new DateTime?() : unComercializador.FechaFin);

        //        db.AddInParameter(dbCommand, "@Usuario", DbType.String, unComercializador.UnAuditoria.Usuario);
        //        db.AddInParameter(dbCommand, "@IPOrigen", DbType.String, unComercializador.UnAuditoria.IP);
        //        db.AddInParameter(dbCommand, "@NroOficina", DbType.Int32, unComercializador.UnAuditoria.IDOficina);

        //        using (TransactionScope oTransactionScope = new TransactionScope())
        //        {
        //            SeguridadLogDAO.GuardarLog("RelacionComercializadorPrestador", "B", dbCommand.Parameters);

        //            db.ExecuteReader(dbCommand);

        //            oTransactionScope.Complete();
        //        }
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
        //        throw new Exception("Error en ComercializadorDAO.ComercializadorM ", ex);
        //    }
        //    finally
        //    {
        //        db = null;
        //        dbCommand.Dispose();
        //    }

        //    return mensage;
        //}

        public static string DomicilioComercializador_RelacionDC_A(long idPrestador, Comercializador unComercializador)
        {
            string mensage = null;
            try
            {
                using (TransactionScope oTransactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (unComercializador.UnDomicilio.IdDomicilio == 0)
                        unComercializador.UnDomicilio.IdDomicilio = DomicilioComercializadorA(idPrestador, unComercializador);

                    if (unComercializador.UnDomicilio.IdDomicilio != 0)
                    {
                        Relacion_DomicilioComercializadorPrestadorA(idPrestador, unComercializador);

                        //if (string.IsNullOrEmpty(mensage))
                        oTransactionScope.Complete();
                    }
                }
            }
            catch (SqlException sqlex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlex.Source, sqlex.Message));

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
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en ComercializadorDAO.DomicilioComercializador_RelacionDC_A ", ex);
            }

            return mensage;
        }

        /// <summary>
        /// Alta/Relacion domicilio comercializado/prestador
        /// </summary>
        public static long DomicilioComercializadorA(long idPrestador, Comercializador unComercializador)
        {
            long idDomicilioComercializador = 0;
            string sql = "DomicilioComercializador_A";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                //db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@IdComercializador", DbType.Int64, unComercializador.ID);
                db.AddInParameter(dbCommand, "@Calle", DbType.String, unComercializador.UnDomicilio.Calle);
                db.AddInParameter(dbCommand, "@Nro", DbType.Int32, unComercializador.UnDomicilio.NumeroCalle);
                db.AddInParameter(dbCommand, "@Piso", DbType.String, unComercializador.UnDomicilio.Piso);
                db.AddInParameter(dbCommand, "@Dpto", DbType.String, unComercializador.UnDomicilio.Departamento);
                db.AddInParameter(dbCommand, "@C_Pcia", DbType.Int16, unComercializador.UnDomicilio.UnaProvincia.CodProvincia);
                db.AddInParameter(dbCommand, "@Localidad", DbType.String, unComercializador.UnDomicilio.Localidad);
                db.AddInParameter(dbCommand, "@CodPostal", DbType.String, unComercializador.UnDomicilio.CodigoPostal);
                db.AddInParameter(dbCommand, "@idTipoDomicilio", DbType.Int16, unComercializador.UnDomicilio.UnTipoDomicilio.IdTipoDomicilio);
                //db.AddInParameter(dbCommand, "@FInicio", DbType.DateTime, unComercializador.UnDomicilio.FechaInicio);
                db.AddInParameter(dbCommand, "@EsSucursal", DbType.Boolean, unComercializador.UnDomicilio.EsSucursal);
                //db.AddInParameter(dbCommand, "@Mail", DbType.String, unComercializador.UnDomicilio.Mail);
                //db.AddInParameter(dbCommand, "@Observaciones", DbType.String, unComercializador.UnDomicilio.Observaciones);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, unComercializador.UnAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@IPOrigen", DbType.String, unComercializador.UnAuditoria.IP);
                db.AddInParameter(dbCommand, "@NroOficina", DbType.Int32, unComercializador.UnAuditoria.IDOficina);

                idDomicilioComercializador = long.Parse(db.ExecuteScalar(dbCommand).ToString());
            }
            catch (SqlException sqlex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlex.Source, sqlex.Message));
                throw sqlex;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return idDomicilioComercializador;
        }

        /// <summary>
        /// Alta/Relacion domicilio comercializado/prestador
        /// </summary>
        public static string Relacion_DomicilioComercializadorPrestadorA(long idPrestador, Comercializador unComercializador)
        {
            //long idDomicilioComercializador = 0;
            string sql = "Relacion_Domicilio_ComercializadorPrestador_A";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string mensage = string.Empty;

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@IdDomicilioComercializador", DbType.Int64, unComercializador.UnDomicilio.IdDomicilio);
                db.AddInParameter(dbCommand, "@FInicio", DbType.DateTime, unComercializador.UnDomicilio.FechaInicio);
                db.AddInParameter(dbCommand, "@FFin", DbType.DateTime, unComercializador.UnDomicilio.FechaFin);
                db.AddInParameter(dbCommand, "@Tel_Prefijo", DbType.String, unComercializador.UnDomicilio.PrefijoTel);
                db.AddInParameter(dbCommand, "@Tel_Nro", DbType.String, unComercializador.UnDomicilio.NumeroTel);
                db.AddInParameter(dbCommand, "@Fax", DbType.String, unComercializador.UnDomicilio.Fax);
                db.AddInParameter(dbCommand, "@Observaciones", DbType.String, unComercializador.UnDomicilio.Observaciones);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, unComercializador.UnAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@IPOrigen", DbType.String, unComercializador.UnAuditoria.IP);
                db.AddInParameter(dbCommand, "@NroOficina", DbType.Int32, unComercializador.UnAuditoria.IDOficina);
                db.AddInParameter(dbCommand, "@Mail", DbType.String, unComercializador.UnDomicilio.Mail);

                db.ExecuteNonQuery(dbCommand);
            }
            catch (SqlException sqlex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlex.Source, sqlex.Message));
                throw sqlex;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en ComercializadorDAO.Relacion_DomicilioComercializadorPrestadorA ", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return mensage;
        }

        /// <summary>
        /// Modificacion/Baja domicilio comercializado/prestador
        /// </summary>
        public static string Relacion_ComercializadorPrestadorDomicilioMB(Int64 idPrestador, long idDomicilioComercializador, Comercializador unComercializador)
        {
            string sql = "Relacion_ComercializadorDomicilio_MB";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string mensage = string.Empty;

            try
            {   

                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@idDomicilioComercializador", DbType.Int64, idDomicilioComercializador);
                db.AddInParameter(dbCommand, "@Tel_Prefijo", DbType.String, unComercializador.UnDomicilio.PrefijoTel);
                db.AddInParameter(dbCommand, "@Tel_Nro", DbType.String, unComercializador.UnDomicilio.NumeroTel);
                db.AddInParameter(dbCommand, "@Fax", DbType.String, unComercializador.UnDomicilio.Fax);
                db.AddInParameter(dbCommand, "@FInicio", DbType.DateTime, unComercializador.UnDomicilio.FechaInicio);
                db.AddInParameter(dbCommand, "@FFin", DbType.DateTime, unComercializador.UnDomicilio.FechaFin.Equals(new DateTime()) ? new DateTime?() : unComercializador.UnDomicilio.FechaFin);
                db.AddInParameter(dbCommand, "@Observaciones", DbType.String, unComercializador.UnDomicilio.Observaciones);
                db.AddInParameter(dbCommand, "@Mail", DbType.String, unComercializador.UnDomicilio.Mail);

                db.AddInParameter(dbCommand, "@Usuario", DbType.String, unComercializador.UnAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@IPOrigen", DbType.String, unComercializador.UnAuditoria.IP);
                db.AddInParameter(dbCommand, "@NroOficina", DbType.Int32, unComercializador.UnAuditoria.IDOficina);

                using (TransactionScope oTransactionScope = new TransactionScope())
                {
                    //if (unComercializador.UnDomicilio.FechaFin.Equals(new DateTime()))
                    DomicilioComercializadorM(idDomicilioComercializador, unComercializador);

                    SeguridadLogDAO.GuardarLog("RelacionComercializadorDomicilio", "M", dbCommand.Parameters);

                    db.ExecuteReader(dbCommand);

                    oTransactionScope.Complete();
                }
            }
            catch (SqlException sqlex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlex.Source, sqlex.Message));
              
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
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en ComercializadorDAO.Relacion_ComercializadorDomicilioMB ", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return mensage;
        }

        /// <summary>
        /// Modificacion domicilio comercializador
        /// </summary>
        public static string DomicilioComercializadorM(long idDomicilioComercializador, Comercializador unComercializador)
        {
            string sql = "DomicilioComercializador_M";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string mensage = string.Empty;

            try
            {
                db.AddInParameter(dbCommand, "@idDomicilioComercializador", DbType.Int64, idDomicilioComercializador);
                db.AddInParameter(dbCommand, "@Calle", DbType.String, unComercializador.UnDomicilio.Calle);
                db.AddInParameter(dbCommand, "@Nro", DbType.Int32, unComercializador.UnDomicilio.NumeroCalle);
                db.AddInParameter(dbCommand, "@Piso", DbType.String, unComercializador.UnDomicilio.Piso);
                db.AddInParameter(dbCommand, "@Dpto", DbType.String, unComercializador.UnDomicilio.Departamento);
                db.AddInParameter(dbCommand, "@C_Pcia", DbType.Int16, unComercializador.UnDomicilio.UnaProvincia.CodProvincia);
                db.AddInParameter(dbCommand, "@Localidad", DbType.String, unComercializador.UnDomicilio.Localidad);
                db.AddInParameter(dbCommand, "@CodPostal", DbType.String, unComercializador.UnDomicilio.CodigoPostal);
                db.AddInParameter(dbCommand, "@idTipoDomicilio", DbType.Int16, unComercializador.UnDomicilio.UnTipoDomicilio.IdTipoDomicilio);
                db.AddInParameter(dbCommand, "@EsSucursal", DbType.Boolean, unComercializador.UnDomicilio.EsSucursal);

                db.AddInParameter(dbCommand, "@Usuario", DbType.String, unComercializador.UnAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@IPOrigen", DbType.String, unComercializador.UnAuditoria.IP);
                db.AddInParameter(dbCommand, "@NroOficina", DbType.Int32, unComercializador.UnAuditoria.IDOficina);

                using (TransactionScope oTransactionScope = new TransactionScope())
                {
                    SeguridadLogDAO.GuardarLog("DomicilioComercializador", "M", dbCommand.Parameters);

                    db.ExecuteReader(dbCommand);

                    oTransactionScope.Complete();
                }
            }
            catch (SqlException sqlex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlex.Source, sqlex.Message));
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
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en ComercializadorDAO.DomicilioComercializador_M ", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return mensage;
        }

        /// <summary>
        /// Baja logica del domicilio comercializado/prestador
        /// </summary>
        public static string Relacion_ComercializadorPrestadorDomicilioB(Int64 idPrestador,
                                                                         Int64 idComercializador,
                                                                         DateTime FFin_Baja,
                                                                         Auditoria unaAuditoria)
        {
            string sql = "Relacion_ComercializadorPrestadorDomicilio_B";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string mensage = string.Empty;

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@idComercializador", DbType.Int64, idComercializador);
                db.AddInParameter(dbCommand, "@FFin_Baja", DbType.DateTime, FFin_Baja);                

                db.AddInParameter(dbCommand, "@UsuarioCarga", DbType.String, unaAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@IPOrigen", DbType.String, unaAuditoria.IP);
                db.AddInParameter(dbCommand, "@NroOficina", DbType.String, unaAuditoria.IDOficina);


                using (TransactionScope oTransactionScope = new TransactionScope())
                {
                    SeguridadLogDAO.GuardarLog("RelacionComercializadorPrestadorDomicilio", "M", dbCommand.Parameters);
                    //throw new Exception();
                    db.ExecuteReader(dbCommand);

                    oTransactionScope.Complete();
                }

            }
            catch (SqlException sqlex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlex.Source, sqlex.Message));

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
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en ComercializadorDAO.Relacion_ComercializadorPrestadorDomicilioB ", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return mensage;
        }

        /// <summary>
        /// Baja fisica del domicilio comercializado/prestador
        /// </summary>
        public static string Relacion_ComercializadorPrestadorDomicilioBFisica(Int64 idPrestador,
                                                                         Int64 idComercializador,
                                                                         DateTime FFin_Baja,
                                                                         Auditoria unaAuditoria)
        {
            string sql = "Relacion_ComercializadorPrestadorDomicilio_BFisica";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string mensage = string.Empty;

            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@idComercializador", DbType.Int64, idComercializador);
                db.AddInParameter(dbCommand, "@FFin_Baja", DbType.DateTime, FFin_Baja);

                db.AddInParameter(dbCommand, "@UsuarioCarga", DbType.String, unaAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@IPOrigen", DbType.String, unaAuditoria.IP);
                db.AddInParameter(dbCommand, "@NroOficina", DbType.String, unaAuditoria.IDOficina);

                using (TransactionScope oTransactionScope = new TransactionScope())
                {
                    SeguridadLogDAO.GuardarLog("RelacionComercializadorPrestadorDomicilio", "B", dbCommand.Parameters);

                    db.ExecuteReader(dbCommand);

                    oTransactionScope.Complete();
                }
            }
            catch (SqlException sqlex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), sqlex.Source, sqlex.Message));
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
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en ComercializadorDAO.Relacion_ComercializadorPrestadorDomicilioB ", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

            return mensage;
        }  

        public static string Relacion_ComercializadorPrestador_Domicilio_Tasas_B(long idPrestador,
                                                                                 long idComercializador,
                                                                                 DateTime FechaInicio,
                                                                                 DateTime FFin_Baja)
        {
            try
            {
                Auditoria unaAuditoria = new Auditoria(); 
                using (TransactionScope oTransactionScope = new TransactionScope())
                {                    
                    //doy de baja logicamente las tasas actuales no vigentes.
                    TasasDAO.TasasAplicadas_RelacionComercializadorPresador_B(idPrestador,
                                                                              idComercializador,
                                                                              FFin_Baja,
                                                                              unaAuditoria);
                    //guardo un historico del las tasas cargadas a futuras no vigentes,
                    //doy de baja fisicamente las tasas cargadas a futuro no vigentes.
                    TasasDAO.TasasAplicadasHistorica_A(idPrestador,
                                                       idComercializador,
                                                       FFin_Baja);
                    TasasDAO.TasasAplicadas_RelacionComercializadorPresador_BFisica(idPrestador,
                                                                                    idComercializador,
                                                                                    FFin_Baja,
                                                                                    unaAuditoria);
                    //doy de baja logicamente los domicilios actuales.
                    Relacion_ComercializadorPrestadorDomicilioB(idPrestador,
                                                                idComercializador,
                                                                FFin_Baja,
                                                                unaAuditoria);
                    //doy de baja fisicamente los domicilios cargados a futuro.
                    Relacion_ComercializadorPrestadorDomicilioBFisica(idPrestador,
                                                                      idComercializador,
                                                                      FFin_Baja,
                                                                      unaAuditoria);

                    Comercializador unComercializador = new Comercializador();
                    unComercializador.ID = idComercializador;
                    unComercializador.FechaInicio = FechaInicio;
                    unComercializador.FechaFin = FFin_Baja;                     
                    unComercializador.UnAuditoria = unaAuditoria;
                    
                    //doy de baja logicamente la relacion del prestador con el comercializador.
                    RelacionComercializadorPrestador_B(idPrestador, unComercializador);

                    oTransactionScope.Complete();
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en TasasDAO.Relacion_ComercializadorPrestador_Domicilio_Tasas_B : ", ex);
            }

            return string.Empty;
        }   

    }
}
