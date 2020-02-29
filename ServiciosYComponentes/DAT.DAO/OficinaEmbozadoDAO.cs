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
using System.Data.SqlClient;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class OficinaEmbozadoDAO :IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OficinaEmbozadoDAO).Name);
        
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

        ~OficinaEmbozadoDAO()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion
  
       #region OficinaEmbozadoAnses_Habilitda

        public static OficinaEmbozadaExpress OficinaEmbozadaExpress_Traer(int oficina)
        {
            string sql = "OficinaEmbozadaExpress_T";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            OficinaEmbozadaExpress oficinaEmbozadaExpress = null;
       
            try
            {
                db.AddInParameter(dbCommand, "@oficina", DbType.Int32, oficina);  
                db.ExecuteNonQuery(dbCommand);

                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    if (dr.Read())
                    {
                        if (!string.IsNullOrEmpty(dr["idTipoEmbozado"].ToString()))
                        {
                            oficinaEmbozadaExpress = new OficinaEmbozadaExpress(dr["Oficina"].ToString(),
                                                                                dr["OficinaEntrega"].ToString(),
                                                                                dr["HoraCorte"].Equals(DBNull.Value) ? DateTime.MinValue: Convert.ToDateTime(dr["HoraCorte"].ToString()),
                                                                                new TipoEmbozado((enum_TipoEmbozado)Enum.Parse(typeof(enum_TipoEmbozado), dr["IdTipoEmbozado"].ToString()),
                                                                                                 dr["Descripcion"].ToString(),
                                                                                                 Convert.ToInt32(dr["CantDiasParaEntrega"].ToString())));
                        }
                    }

                    dr.Close();
                }
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

            return oficinaEmbozadaExpress;
        }
     
        #endregion          
    }
}
