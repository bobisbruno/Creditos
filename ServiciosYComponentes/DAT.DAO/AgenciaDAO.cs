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
    public class AgenciaDAO
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AgenciaDAO).Name);
       
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

        ~AgenciaDAO()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        public static List<Agencia> TraerAgencias(Agencia unaAgencia)
        {
            string sql = "VamosDePaseo_Agencia_Traer";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            Agencia oAge = null;
            try
            {
                List<Agencia> lstAgencia = new List<Agencia>();

                db.AddInParameter(dbCommand, "@habilitada", DbType.Boolean, unaAgencia.Habilitada);
                db.AddInParameter(dbCommand, "@esMayorista", DbType.Boolean, unaAgencia.EsMayorista);
                if (unaAgencia.NroLegajo > 0)
                    db.AddInParameter(dbCommand, "@nroLegajo", DbType.Int32, unaAgencia.NroLegajo);

                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        oAge = new Agencia();
                        oAge.IdAgencia = int.Parse(ds["IdAgencia"].ToString());
                        oAge.Descripcion = ds["Descripcion"].ToString();
                        oAge.Habilitada = bool.Parse(ds["habilitada"].ToString());
                        oAge.EsMayorista = bool.Parse(ds["Esmayorista"].ToString());
                        oAge.NroCuenta = ds["NroCuenta"].ToString();
                        oAge.NroLegajo = int.Parse(ds["NroLegajo"].ToString());
                        oAge.Cuit = ds["Cuit"].ToString();
                        lstAgencia.Add(oAge);
                        oAge = null;
                    }
                }
                return lstAgencia;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en AgenciaDAO.TraerAgencias", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }

    }
}
