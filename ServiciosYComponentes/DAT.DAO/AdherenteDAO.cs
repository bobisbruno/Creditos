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
    public class AdherenteDAO : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AdherenteDAO).Name);

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

        ~AdherenteDAO()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        public static List<Adherente> Traer_Adherentes(long idNovedad)
        {
            string sql = "Adherente_Trae";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            Adherente oAdhe = null;

            try
            {
                List<Adherente> lstAdherentes = new List<Adherente>();

                db.AddInParameter(dbCommand, "@idNovedad", DbType.Int64, idNovedad);
               
                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        oAdhe = new Adherente(long.Parse(ds["cuilAdherente"].ToString()), ds["apellidoNombre"].ToString(),
                                                        ds["ip"].ToString(), DateTime.Parse(ds["fecUltModificacion"].ToString()),
                                                        ds["usuario"].ToString());
                        
                        lstAdherentes.Add(oAdhe);
                        oAdhe = null;
                    }
                }
                return lstAdherentes;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en AdherenteDAO.Traer_Adherentes", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }

        }

    }
}
