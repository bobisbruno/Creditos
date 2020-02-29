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
    public class EstadoDAO : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EstadoDAO).Name);	
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

        ~EstadoDAO()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        #region Traer_Todos
        public static List<Estado> Traer_Todos()
        {
            string sql = "EstadoReclamo_Trae_Todos";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;

            List<Estado> ListEstado = new List<Estado>();
            Estado oEst = null;
            try
            {
               dbParametros = dbCommand.Parameters;
           
               using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        oEst = new Estado();
                        oEst.DescEstado = ds["DescEstado"].ToString();
                        oEst.IdEstado = int.Parse(ds["IdEstado"].ToString());
                        ListEstado.Add(oEst);
                    }
                    ds.Close();
                    ds.Dispose();
                }
                return ListEstado;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en EstadoDAO.Trae_Todos", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

     
        #region Traer_Estado
        public static Estado Traer(int idEstado)
        {
            string sql = "EstadoReclamo_Trae_IdEstado";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            db.AddInParameter(dbCommand, "@IdEstado", DbType.Int64, idEstado);
            Estado oEst = null;
            try
            {
                dbParametros = dbCommand.Parameters;

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        oEst = new Estado();
                        oEst.DescEstado = ds["DescEstado"].ToString();
                        oEst.IdEstado = int.Parse(ds["IdEstado"].ToString());
                    }

                    ds.Close();
                    ds.Dispose();
                }
                return oEst;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en EstadoDAO.Traer", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion
        
        #region Tipos_EstadosDocumentacion_Trae        
        public static List<EstadoDocumentacion> Tipos_EstadosDocumentacion_Trae()
        {
            string sql = "TipoEstadoDocumentacion_Trae";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;

            List<EstadoDocumentacion> ListEstado = new List<EstadoDocumentacion>();
         
            try
            {
                dbParametros = dbCommand.Parameters;

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        ListEstado.Add(new EstadoDocumentacion(int.Parse(ds["idEstadodocumentacion"].ToString()),
                                                       ds["descripcion"].ToString(),true,
                                                       Convert.ToBoolean(ds["verOnlineCarga"]),
                                                       Convert.ToBoolean(ds["debeIngresarCaja"]),
                                                       Convert.ToBoolean(ds["apruebaNovedad"])));
                    }

                    ds.Close();
                    ds.Dispose();

                }
                return ListEstado;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en EstadoDAO.Tipos_EstadosDocumentacion_Trae", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region Tipos_EstadosCaratulacion_Trae
        public static List<EstadoCaratulacion> Tipos_EstadosCaratulacion_Trae()
        {
            string sql = "EstadoNovCaratulacion_Traer";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;

            List<EstadoCaratulacion> ListEstado = new List<EstadoCaratulacion>();
            try
            {
                dbParametros = dbCommand.Parameters;

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        ListEstado.Add(new EstadoCaratulacion(int.Parse(ds["IdEstado"].ToString()),
                                                            ds["DescEstadoNov"].ToString(), true,
                                                            short.Parse(ds["idEstadoExpediente"].ToString())));
                    }

                    ds.Close();
                    ds.Dispose();

                }
                return ListEstado;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en EstadoDAO.Tipos_EstadosCaratulacion_Trae", ex);
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
