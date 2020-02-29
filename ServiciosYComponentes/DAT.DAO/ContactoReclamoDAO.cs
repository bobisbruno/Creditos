using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NullableReaders;
using log4net;
          
namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class ContactoReclamoDAO : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ContactoReclamoDAO).Name);

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

        ~ContactoReclamoDAO()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        #region Traer
        public static ContactoReclamo Traer(string cuil)
        {
            string sql = "[ContactoReclamo_TxCuil]";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            try
            {
                ContactoReclamo oContacto=new ContactoReclamo();

                db.AddInParameter(dbCommand, "@cuil", DbType.String, cuil);

                dbParametros = dbCommand.Parameters;

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        Auditoria unAuditoria = new Auditoria(ds["Usuario"].ToString(), ds["Usuario"].ToString(), DateTime.Parse(ds["FecUltModificacion"].ToString()));
                        oContacto.Cuil=ds["cuil"].ToString();
                        oContacto.Telediscado=ds["Telediscado"].ToString();
                        oContacto.Telefono=ds["Telefono"].ToString();
                        oContacto.Celular=bool.Parse(ds["Celular"].ToString());
                        oContacto.Mail=ds["Mail"].ToString();
                    }
                }
                return oContacto;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en ContactoReclamoDAO.Traer", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

        }
        #endregion

        #region Grabar
        public static bool Grabar(ContactoReclamo oContacto)
        {
            string sql = "ContactoReclamo_AM";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;

            try
            {
                db.AddInParameter(dbCommand, "@cuil", DbType.String, oContacto.Cuil);
                db.AddInParameter(dbCommand, "@Telediscado", DbType.String, oContacto.Telediscado);
                db.AddInParameter(dbCommand, "@Telefono", DbType.String, oContacto.Telefono);
                db.AddInParameter(dbCommand, "@Celular", DbType.Boolean, oContacto.Celular);
                db.AddInParameter(dbCommand, "@Mail", DbType.String, oContacto.Mail);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, oContacto.UnaAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@IP", DbType.String, oContacto.UnaAuditoria.IP);
                dbParametros = dbCommand.Parameters;

                db.ExecuteNonQuery(dbCommand);

                return true;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en ContactoReclamoDAO.Grabar", ex);
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
