using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Transactions;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    
    public static class PinLinkDAO
    {

        public static string PinLink_Alta(PinLink unPinLink)
        {
            String mjeRdo = String.Empty;
            String sql = "PinLink_A";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            try
            {
                db.AddInParameter(dbCommand, "@nroTarjeta", DbType.Int64, unPinLink.NroTarjeta);
                db.AddInParameter(dbCommand, "@cuil", DbType.Int64, unPinLink.Cuil);
                db.AddInParameter(dbCommand, "@idEstadoPin", DbType.Int16, unPinLink.UnTipoEstadoPin.IdEstadoPin);
                db.AddInParameter(dbCommand, "@oficina", DbType.String, unPinLink.UnAuditoria.IDOficina);
                db.AddInParameter(dbCommand, "@ip", DbType.String, unPinLink.UnAuditoria.IP);
                db.AddInParameter(dbCommand, "@usuario", DbType.String, unPinLink.UnAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, unPinLink.UnPrestador.ID);
                db.AddInParameter(dbCommand, "@idTipoDocumentoPresentado", DbType.Int16, unPinLink.TipoDocPresentado);
                db.AddOutParameter(dbCommand, "@mensaje", DbType.String, 300);

                using (TransactionScope scope = new TransactionScope())
                {
                    db.ExecuteNonQuery(dbCommand);
                    scope.Complete();
                }
               
                mjeRdo = db.GetParameterValue(dbCommand, "@mensaje").ToString();
                                 
            }
            catch (DbException sqlErr)
            {
                
                if (((System.Data.SqlClient.SqlException)(sqlErr)).Number >= 50000)
                    return ((System.Exception)(sqlErr)).Message;
                else
                    throw sqlErr;
            }
            catch (Exception ex)
            {               
               mjeRdo = ex.Message;
               throw ex;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
           return mjeRdo;
        }
        
        public static List<PinLink> PinLink_Trae(Int64 cuil, Int64 nroTarjeta )
        {
            List<PinLink> lista = new List<PinLink>();
            String sql = "PinLink_Trae";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            
            try{
                
                  db.AddInParameter(dbCommand, "@nroTarjeta", DbType.Int64, nroTarjeta);
                  db.AddInParameter(dbCommand,"@cuil",DbType.Int64, cuil);

                  using (IDataReader dr = db.ExecuteReader(dbCommand))
                  { 
                    while(dr.Read())
                    {
                        PinLink unPinLink = new PinLink(Int64.Parse(dr["nroTarjeta"].ToString()),
                                                        Int64.Parse(dr["cuil"].ToString()),
                                                        new TipoEstadoPin(dr["idEstadoPin"].Equals(DBNull.Value) ? 0 : int.Parse(dr["idEstadoPin"].ToString()),
                                                        dr["descripcionEstadoPin"].ToString()),
                                                        dr["nombreArchivo"].ToString(),                                                        
                                                        new Entidad_Prest_Comer(dr["IdPrestador"].Equals(DBNull.Value) ? 0 : Int64.Parse(dr["IdPrestador"].ToString()),
                                                                                dr["RazonSocial"].Equals(DBNull.Value) ? string.Empty : dr["RazonSocial"].ToString()),
                                                        dr["idTipoDocumentoPresentado"].Equals(DBNull.Value) ? Int16.Parse("0") : Int16.Parse(dr["idTipoDocumentoPresentado"].ToString()),
                                                        dr["codigoAValidar"].Equals(DBNull.Value) ? null: 
                                                        new CodigoPreAprobado(Int64.Parse(dr["cuil"].ToString()), dr["codigoAValidar"].ToString()));

                        if (!dr["idEstadoPin"].Equals(DBNull.Value))
                            unPinLink.FechaNovedad = DateTime.Parse(dr["fNovedad"].ToString());
                        lista.Add(unPinLink);
                    }
                  }

                return lista;
            }catch(Exception ex)
            {
                throw ex;
            }finally
            {
                db = null;
                dbCommand.Dispose();
                lista = null;
            }
        }         
    
    }
}
