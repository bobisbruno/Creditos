using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NullableReaders;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class SolicitudDAO : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SolicitudDAO).Name);  
        
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

        ~SolicitudDAO()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        #region Sucursal

        public static string Traer_Sucursal(string idSucursal, long idPrestador, int codConceptoLiq, out Sucursal suc)
        {
            string sql = "SucursalCorreo_HorizonteBaja_trae";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            suc = null;
            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@idsucursal", DbType.String, idSucursal);
                db.AddInParameter(dbCommand, "@CodConceptoLiq", DbType.Int32, codConceptoLiq);

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    if (ds.Read())
                    {

                        suc = new Sucursal(idPrestador,
                                            idSucursal,
                                             ds["denominacion"].ToString(),
                                             Convert.ToDateTime(ds["fdesde"]),
                                             ds["fhasta"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(ds["fhasta"]),
                                             Convert.ToInt16(ds["cantDiasHasta"]));
                        return string.Empty;
                    }
                }
                return null;
            }
            catch (System.Data.SqlClient.SqlException e)
			{
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), e.Source, e.Message));
				//Si es una excepcion especifica guardo el error y continuo
				if (e.Number == 50000)
				{
                    return e.Message;
				}
				else
					throw;
		    }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en SolicitudDAO.Traer_Sucursal", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
      
        public static List<Sucursal> SucursalCorreo_TXPrestador(long idPrestador)
        {
            string sql = "SucursalCorreo_TXPrestador";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Sucursal> lista = new List<Sucursal>();
            
            try
            {
                db.AddInParameter(dbCommand, "@idPrestador", DbType.Int64, idPrestador);
               
                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                 {
                    while(ds.Read())
                    {
                        lista.Add(new Sucursal(long.Parse(ds["idprestador"].ToString()),
                                                           ds["idsucursal"].ToString(),
                                                          ds["denominacion"].ToString()));                       
                    }
                }
                return lista;
            }            
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex ;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
                lista = null;
            }
        
        }

        public static List<Sucursal> SucursalCorreo_Traer( )
        {
            string sql = "SucursalCorreo_TT";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Sucursal> lista = new List<Sucursal>();

            try
            {
                
              using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        lista.Add(new Sucursal(long.Parse(ds["idprestador"].ToString()),
                                                           ds["idsucursal"].ToString(),
                                                          ds["denominacion"].ToString()));
                    }
                }
                return lista;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
                lista = null;
            }

        }

        public static void SucursalCorreo_Grabar(Sucursal sucursal) 
        {
            string sql = "SucursalCorreo_Alta";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
       
            try
            {
                db.AddInParameter(dbCommand, "@idprestador", DbType.Int64, sucursal.IdPrestador);
                db.AddInParameter(dbCommand, "@oficina", DbType.String, sucursal.IdSucursal);
                db.AddInParameter(dbCommand, "@denominacion", DbType.String, sucursal.Denominacion);

                db.ExecuteReader(dbCommand);
               
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
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
