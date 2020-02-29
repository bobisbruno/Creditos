using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using NullableReaders;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class TipoEstado_SCDAO
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TipoEstado_SCDAO).Name);

        public static List<TipoEstado_SC> TipoEstado_SC_TT()
        {
            string sql = "TipoEstado_SC_TT";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<TipoEstado_SC> result = new List<TipoEstado_SC>();

            try
            {
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        result.Add(new TipoEstado_SC(int.Parse(dr["idEstadoSC"].ToString()),
                                                                  dr["descripcion"].ToString(),
                                                                  bool.Parse(dr["novedadEstaVigente"].ToString())));
                    }

                    return result;
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
        }
    }
}
