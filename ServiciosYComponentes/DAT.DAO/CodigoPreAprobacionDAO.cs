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
    public class CodigoPreAprobacionDAO : IDisposable
    {
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

        ~CodigoPreAprobacionDAO()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        private static readonly ILog log = LogManager.GetLogger(typeof(CodigoPreAprobacionDAO).Name);

        #region ALTA 

        public static string Novedades_CodigoPreAprobacion_Alta(Int64 Cuil, string Ip, string Usuario)
        {
            string sql = "Novedades_CodigoPreAprobacion_A";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            
            try
            {
                db.AddInParameter(dbCommand, "@Cuil", DbType.Int64, Cuil);
                db.AddInParameter(dbCommand, "@ip", DbType.String, Ip);
                db.AddInParameter(dbCommand, "@usuario", DbType.String, Usuario);
                db.AddOutParameter(dbCommand,"@mensaje", DbType.String, 100);

                using (TransactionScope scope = new TransactionScope())
                {
                    db.ExecuteNonQuery(dbCommand);
                    scope.Complete();
               
                }
                return db.GetParameterValue(dbCommand, "@mensaje").ToString();
                 
            }
            catch (DbException ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));

                if (((System.Data.SqlClient.SqlException)(ex)).Number >= 50000)
                    return ((System.Exception)(ex)).Message;              
                else throw ex;                    
            }
            finally
            {
                db = null;
                dbCommand.Dispose();

            }
         }

        #endregion

        #region Modificacion X Cuil, CodigoAValidar

        public static string Novedades_CodigoPreAprobacion_Modificacion(CodigoPreAprobado unCodigoPreAprobado)
        {

            string sql = "Novedades_CodigoPreAprobacion_M";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string mensaje = string.Empty;
            try
            {
                db.AddInParameter(dbCommand, "@Cuil", DbType.Int64, unCodigoPreAprobado.Cuil);
                db.AddInParameter(dbCommand, "@codigoAValidar", DbType.String, unCodigoPreAprobado.CodigoAValidar);
                db.AddInParameter(dbCommand, "@idnovedad", DbType.Int64, unCodigoPreAprobado.IdNovedad);
                db.AddInParameter(dbCommand, "@esUtilizada", DbType.String, unCodigoPreAprobado.unTipoUso.ToString().ToUpper());
                db.AddInParameter(dbCommand, "@ip", DbType.String, unCodigoPreAprobado.UnAuditoria.IP);
                db.AddInParameter(dbCommand, "@usuario", DbType.String, unCodigoPreAprobado.UnAuditoria.Usuario);

                db.ExecuteNonQuery(dbCommand);                                     

                return mensaje;
            }
            catch (DbException sqlErr)
            {
                if (((System.Data.SqlClient.SqlException)(sqlErr)).Number >= 50000)
                    return ((System.Exception)(sqlErr)).Message;
                else
                    throw sqlErr;
            }
            catch (Exception ex)
            {
                throw new Exception("Error en CodigoPreAprobacionDAO.Novedades_CodigoPreAprobacion_Modificacion", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region VALIDA X Cuil, CodigoAValidar
        public static string Novedades_CodigoPreAprobacion_Valida(CodigoPreAprobado unCodigoPreAprobado)
        {
            string sql = "Novedades_CodigoPreAprobacion_Valida";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string mensaje = string.Empty;

            try
            {
                db.AddInParameter(dbCommand, "@Cuil", DbType.Int64, unCodigoPreAprobado.Cuil);
                db.AddInParameter(dbCommand, "@codigoAValidar", DbType.String, unCodigoPreAprobado.CodigoAValidar);
                                
                db.ExecuteNonQuery(dbCommand);

                return mensaje;
            }
            catch (DbException sqlErr)
            {
                if (((System.Data.SqlClient.SqlException)(sqlErr)).Number >= 50000)
                    return ((System.Exception)(sqlErr)).Message;
                else
                    throw sqlErr;
            }
            catch (Exception ex)
            {

                throw new Exception("Error en CodigoPreAprobacionDAO.Novedades_CodigoPreAprobacion_Valida", ex);
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
