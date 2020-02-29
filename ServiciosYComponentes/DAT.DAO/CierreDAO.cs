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
    public class CierreDAO : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CierreDAO).Name);
        
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

        ~CierreDAO()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        //private DbParameterCollection dbParametros = null;
        private string sql = string.Empty;

        #region Trae cierres anteriores
        public List<Cierre> TraerCierresAnteriores()
        {
            sql = "Cierres_TT";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                List<Cierre> lstCierre = new List<Cierre>();
                                
                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        lstCierre.Add(new Cierre(
                                        ds.GetString("FecCierre"),
                                        ds.GetString("Mensual"),
                                        ds.GetNullableDateTime("FecAplicacionPagos")));                                            
                    }

                    ds.Close();
                    ds.Dispose();
                }
                return lstCierre;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en CierresDAO.TraerCierresAnteriores", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region Traer fecha Proximo Cierre
        public static Cierre TraerFechaCierreProx()
        {
            string sql = "Cierres_TProxFecCierre";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                Cierre cierre = new Cierre();

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        cierre.FecCierre = ds["FecCierre"].ToString();
                        cierre.Mensual = ds["Mensual"].ToString();
                    }

                    ds.Close();
                    ds.Dispose();
                }
                return cierre;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en CierresDAO.TraerFechaCierreProx", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region Trae fecha cierre anterior
        public Cierre TraerFechaCierreAnterior()
        {
            sql = "Cierres_TFecCierreAnt";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                Cierre cierre = new Cierre();

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {

                    while (ds.Read())
                    {

                        cierre.FecCierre = ds["FecCierre"].ToString(); 
                        cierre.Mensual = ds["Mensual"].ToString();
                    }
                    ds.Close();
                    ds.Dispose();
                }
                return cierre;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en CierresDAO.TraerFechaCierreAnterior", ex);
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