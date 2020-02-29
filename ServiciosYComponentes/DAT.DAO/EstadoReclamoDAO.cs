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
    public class EstadoReclamoDAO : EstadoDAO
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(EstadoReclamoDAO).Name);

    
        #region Traer_Proximos
        public static List<EstadoReclamo> Traer_Proximos(int idEstado)
        {
            string sql = "EstadoReclamo_Proximo";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            db.AddInParameter(dbCommand, "@idEstado", DbType.Int32, idEstado);
            List<EstadoReclamo> ListEstado = new List<EstadoReclamo>();
            EstadoReclamo oEst = null;
            try
            {
                dbParametros = dbCommand.Parameters;

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        oEst = new EstadoReclamo();
                        oEst.DescEstado = ds["DescEstado"].ToString();
                        oEst.IdEstado = int.Parse(ds["IdEstado"].ToString());

                        oEst.Control = ds.GetNullableString("Control");
                        oEst.ControlTexto = ds.GetNullableString("ControlTexto");
                        oEst.ControlIdModelo = ds.GetNullableInt32("ControlIdModelo") == null ? 0 : ds.GetInt32("ControlIdModelo");
                        oEst.EsFinal = ds.GetNullableBoolean("EsFinal") == null ? false : ds.GetBoolean("EsFinal");
                        oEst.FecManual = ds.GetNullableBoolean("FecManual") == null ? false : ds.GetBoolean("FecManual");
                        oEst.MensajeInfo = ds.GetNullableString("MensajeInfo");
                        oEst.EstadoAnme = ds.GetNullableInt32("EstadoAnme") == null ? 0 : ds.GetInt32("EstadoAnme");
                        oEst.PaseAutomatico = ds.GetNullableBoolean("PaseAutomatico") == null ? false : ds.GetBoolean("PaseAutomatico");
                        oEst.TieneObservacion = ds.GetNullableBoolean("TieneObservacion") == null ? false : ds.GetBoolean("TieneObservacion");

                        ListEstado.Add(oEst);

                    }

                    ds.Close();
                    ds.Dispose();

                }
                return ListEstado;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message)); 
                throw new Exception("Error en EstadoDAO.Trae_Todos", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region Traer_ModeloImpresion
        public static List<ModeloImpresion> ModeloImpresionTraer(int idEstado)
        {
            string sql = "EstadoReclamo_impresion";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            db.AddInParameter(dbCommand, "@IdEstado", DbType.Int64, idEstado);
            List<ModeloImpresion> LstModeloImpresion = new List<ModeloImpresion>();
            ModeloImpresion oModeloImpresion = null;
            try
            {
                dbParametros = dbCommand.Parameters;

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        oModeloImpresion = new ModeloImpresion();
                        oModeloImpresion.IdModelo = ds["idModeloImpresion"].ToString()==null?0:int.Parse(ds["idModeloImpresion"].ToString());
                        oModeloImpresion.Imprime = ds["Imprime"].ToString()==null?false:bool.Parse(ds["Imprime"].ToString());
                        LstModeloImpresion.Add(oModeloImpresion);
                    }

                    ds.Close();
                    ds.Dispose();

                }
                return LstModeloImpresion;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en EstadoDAO.ModeloImpresionTraer", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region Traer_Estado
        public static EstadoReclamo Traer(int idEstado)
        {
            string sql = "EstadoReclamo_Trae_IdEstado";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;
            db.AddInParameter(dbCommand, "@IdEstado", DbType.Int64, idEstado);
            EstadoReclamo oEst = null;
            try
            {
                dbParametros = dbCommand.Parameters;

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        oEst = new EstadoReclamo();
                        oEst.DescEstado = ds["DescEstado"].ToString();
                        oEst.IdEstado = int.Parse(ds["IdEstado"].ToString());

                        oEst.Control = ds.GetNullableString("Control");
                        oEst.ControlTexto = ds.GetNullableString("ControlTexto");
                        oEst.ControlIdModelo = ds.GetNullableInt32("ControlIdModelo") == null ? 0 : ds.GetInt32("ControlIdModelo");
                        oEst.EsFinal = ds.GetNullableBoolean("EsFinal") == null ? false : ds.GetBoolean("EsFinal");
                        oEst.FecManual = ds.GetNullableBoolean("FecManual") == null ? false : ds.GetBoolean("FecManual");
                        oEst.MensajeInfo = ds.GetNullableString("MensajeInfo");
                        oEst.EstadoAnme = ds.GetNullableInt32("EstadoAnme") == null ? 0 : ds.GetInt32("EstadoAnme");
                        oEst.PaseAutomatico = ds.GetNullableBoolean("PaseAutomatico") == null ? false : ds.GetBoolean("PaseAutomatico");
                        oEst.TieneObservacion = ds.GetNullableBoolean("TieneObservacion") == null ? false : ds.GetBoolean("TieneObservacion");
                    }

                    ds.Close();
                    ds.Dispose();

                }
                return oEst;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en EstadoDAO.Traer", ex);
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
