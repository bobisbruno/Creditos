using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Transactions;
using System.ComponentModel;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
	public static class TipoDocumentoPresentadoDAO
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(TipoDocumentoPresentadoDAO).Name);        
        
        
        public static List<TipoDocumentoPresentado> TipoDocumentoPresentado_Traer()
		{
            string sql = "TipoDocumentoPresentado_Traer";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            List<TipoDocumentoPresentado> rdo = new List<TipoDocumentoPresentado>();

            try
            {
                
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        TipoDocumentoPresentado tipoDocPresentado = new TipoDocumentoPresentado(Int16.Parse(dr["TipoDocumento"].ToString()),
                                                                                                dr["DescTipoDocumento"].ToString(),
                                                                                                Boolean.Parse(dr["Habilitado"].ToString()),
                                                                                                Boolean.Parse(dr["HabilitadoWeb"].ToString()),
                                                                                                Boolean.Parse(dr["HabilitadoPreAprobado"].ToString()),
                                                                                                Boolean.Parse(dr["HabilitadoComercio"].ToString()),
                                                                                                Boolean.Parse(dr["HabilitadoMayor75"].ToString()),
                                                                                                Boolean.Parse(dr["HabilitadoMenor75"].ToString()));
                        rdo.Add(tipoDocPresentado);
                    }
                }

                return rdo;
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
                rdo = null;
            }
		}
	}
}