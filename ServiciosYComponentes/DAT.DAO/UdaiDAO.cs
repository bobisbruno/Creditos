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
    public class UdaiDAO : IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UdaiDAO).Name);

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

        ~UdaiDAO()
        {
            // Llamo al método que contiene la lógica
            // para liberar los recursos de esta clase.
            Dispose(true);
        }
        #endregion

        #region UdaiExterno_TXProvincia_CodPostal

        public static List<UDAI> Traer_UdaiExterno_TXProvincia_CodPostal(Int16 idProvincia, Int32 CodigoPostal, out Int32 UdaiCercanaDomicilio)
        {
            string sql = "UdaiExterno_TXProvincia_CodPostal";
            Database db = DatabaseFactory.CreateDatabase("UDAT_WEB_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<UDAI> listaRdo = new List<UDAI>();
            try
            {
                db.AddInParameter(dbCommand, "@idprovincia", DbType.Int64, idProvincia);
                db.AddInParameter(dbCommand, "@codPostal", DbType.String, CodigoPostal);
                db.AddOutParameter(dbCommand, "@UdaiCercanaDomicilio", DbType.Int32, int.MaxValue);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        listaRdo.Add(new UDAI(long.Parse(dr["IdUdaiExterno"].ToString()),dr["UdaiDescripcion"].ToString(), dr["Domicilio"].ToString(), dr["CodigoPostal"].ToString(),
                                              (int)dr["IdRegional"],(string)dr["Regional"],(string)dr["DomicilioRegional"],(string)dr["CodigoPostalRegional"],(string)dr["ProvinciaRegional"])
                                    );
                    }
                }


                UdaiCercanaDomicilio = db.GetParameterValue(dbCommand, "@UdaiCercanaDomicilio").ToString() == "" ? 0 : Int32.Parse(db.GetParameterValue(dbCommand, "@UdaiCercanaDomicilio").ToString());
                return listaRdo;
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
        #endregion

        #region UdaiExternoRegional_Traer

        public static List<Regional> RegionalesUdaiExterno_Traer()
        {
            string sql = "RegionalUdaiExterno_TT";
            Database db = DatabaseFactory.CreateDatabase("UDAT_WEB_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Regional> listaRdo = new List<Regional>();


            try
            {
                using (DbDataReader dr = (DbDataReader)db.ExecuteReader(dbCommand))
                {
                    dr.Read();
                    bool existe = dr.HasRows;

                    while (existe)
                    {
                        listaRdo.Add(new Regional(int.Parse(dr["idRegional"].ToString()),
                                                  dr["Descripcion"].ToString(),
                                                  mapListaUdai(dr, ref existe)));
                    }
                }

                return listaRdo;
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

        private static List<UDAI> mapListaUdai(DbDataReader dr, ref bool existe)
        {
            List<UDAI> listaUDAI = new List<UDAI>();
            int indice, idRegional = 0;

            indice = idRegional = Int32.Parse(dr["idRegional"].ToString());

            while (existe && indice == idRegional)
            {
                listaUDAI.Add(new UDAI(long.Parse(dr["IdUdaiExterno"].ToString()),
                                       dr["UdaiDescripcion"].ToString(),
                                       dr["Domicilio"].ToString(),
                                       dr["CodigoPostal"].ToString(),
                                       int.Parse(dr["idRegional"].ToString())));

                if (dr.Read())
                    indice = Int32.Parse(dr["idRegional"].ToString());
                else
                    existe = false;

            }

            return listaUDAI;
        }

        #endregion

    }
}
