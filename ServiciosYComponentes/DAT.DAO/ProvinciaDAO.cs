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
    public class ProvinciaDAO 
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProvinciaDAO).Name); 
       
        public ProvinciaDAO() { }

        #region TraerProvincias

        public static List<Provincia> TraerProvincias()
        {
           
            string sql = "Provincias_Traer";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            //DbParameterCollection dbParametros = null;
            List<Provincia> lstProvincias = new List<Provincia>();
            try
            {
                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        lstProvincias.Add(new Provincia(
                            ds.GetByte("C_PCIA"),
                            ds.GetString("D_PCIA").Trim()));
                    }
                }

                return lstProvincias;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));   
                throw new Exception("Error en ProvinciaDAO.Provincias_Traer", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

        #endregion

        #region TraerProvincia_xID
        public static string TraerProvincia_xID(int idProvincia)
        {

            string sql = "Provincia_xID";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            string strProv = "";
            try
            {
                db.AddInParameter(dbCommand,"@c_pcia",DbType.Int16,idProvincia); 
                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    if (ds.Read())
                        strProv = ds.GetString("D_PCIA").Trim();                  
                }
                return strProv;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));  
                throw new Exception("Error en ProvinciaDAO.TraerProvincia_xID", ex);
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
