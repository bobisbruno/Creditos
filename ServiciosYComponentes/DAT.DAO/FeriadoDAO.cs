using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Transactions;
using NullableReaders;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class FeriadoDAO 
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(FeriadoDAO).Name);

        public FeriadoDAO() { }

        #region TraerFeriados

        public static List<Feriado> FeriadosTraer(DateTime? fecha)
        {
            string sql = "Feriados_Traer";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Feriado> lstFeriados = new List<Feriado>();

            try
            {
                dbCommand = db.GetStoredProcCommand(sql);
                db.AddInParameter(dbCommand, "@FecFeriado", DbType.DateTime, fecha);
                
                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        lstFeriados.Add(new Feriado(ds.GetDateTime("FecFeriado"),
                                                    ds.GetString("Usuario"),
                                                    ds.GetString("IP"),
                                                    ds.GetString("Oficina"),
                                                    ds.GetDateTime("FecUltModificacion")));
                    }
                }

                return lstFeriados;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en FeriadoDAO.TraerFeriados", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        #endregion   
    
        #region FeriadosABM
        public static string FeriadosABM(Feriado unFeriado, Boolean esBaja)
        {
            String sql = "Feriados_ABM";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                db.AddInParameter(dbCommand, "@FecFeriado", DbType.DateTime, unFeriado.Fecha);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, unFeriado.Usuario);
                db.AddInParameter(dbCommand, "@IP", DbType.String, unFeriado.IP);
                db.AddInParameter(dbCommand, "@Oficina", DbType.String, unFeriado.Oficina);
                db.AddInParameter(dbCommand, "@EsBaja", DbType.Boolean, esBaja);

                using (TransactionScope scope = new TransactionScope())
                {
                    db.ExecuteNonQuery(dbCommand);
                    scope.Complete();
                }

                return string.Empty;
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
    }
}
