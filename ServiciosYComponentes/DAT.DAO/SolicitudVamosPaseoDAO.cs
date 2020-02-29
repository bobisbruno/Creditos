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
    public class SolicitudVamosPaseoDAO
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SolicitudVamosPaseoDAO).Name); 

        private DbParameterCollection dbParametros = null;
        private string sql = string.Empty;

        #region Solicitud_Alta
        public long Solicitud_Alta(SolicitudVamosPaseo oSol)
        {
            sql = "VamosDePaseo_solicitud_Alta";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            try
            {
                dbParametros = dbCommand.Parameters;
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, oSol.UnBeneficiario.IdBeneficiario);
                db.AddInParameter(dbCommand, "@fIngresoSolicitud", DbType.DateTime, oSol.FIngresoSolicitud);
                db.AddInParameter(dbCommand, "@MensualSolicitud", DbType.Int32, oSol.MensualSolicitud);
                db.AddInParameter(dbCommand, "@importeTotal", DbType.Decimal, oSol.ImporteTotal);
                db.AddInParameter(dbCommand, "@cantCuotas", DbType.Byte, oSol.CantCuotas);
                db.AddInParameter(dbCommand, "@idAgenciaMayorista", DbType.Int32, oSol.AgenciaMayorista.IdAgencia);
                db.AddInParameter(dbCommand, "@destino", DbType.String, oSol.Destino);
                if (oSol.FDesde == DateTime.MinValue)
                    db.AddInParameter(dbCommand, "@fDesde", DbType.DateTime, null);
                else
                    db.AddInParameter(dbCommand, "@fDesde", DbType.DateTime, oSol.FDesde);
                if (oSol.FHasta == DateTime.MinValue)
                    db.AddInParameter(dbCommand, "@fHasta", DbType.DateTime, null);
                else
                    db.AddInParameter(dbCommand, "@fHasta", DbType.DateTime, oSol.FHasta);

                db.AddInParameter(dbCommand, "@idEstado", DbType.Int16, oSol.UnEstado.IdEstado);
                db.AddInParameter(dbCommand, "@nroExpediente", DbType.String, oSol.NroExpediente);
                db.AddInParameter(dbCommand, "@usuario", DbType.String, oSol.UnaAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@ip", DbType.String, oSol.UnaAuditoria.IP);

                db.AddInParameter(dbCommand, "@importeFinanciado", DbType.Decimal, oSol.ImporteFinanciado);
                db.AddInParameter(dbCommand, "@idAgenciaMinorista", DbType.Int32, oSol.AgenciaMinorista.IdAgencia);
                db.AddInParameter(dbCommand, "@ViajaSolo", DbType.Boolean, oSol.ViajaSolo);
                db.AddInParameter(dbCommand, "@Oficina", DbType.String, oSol.Oficina);
                db.AddOutParameter(dbCommand, "@idSolicitud", DbType.Int64, 1);
                db.ExecuteNonQuery(dbCommand);

                long idSol = 0;

                long.TryParse(db.GetParameterValue(dbCommand, "@idSolicitud").ToString(), out idSol);
                return idSol;
                
                //ds.Close();
                //ds.Dispose();
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message)); 
                throw new Exception("Error en SolicitudDAO.Solicitud_Alta", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion

        #region Solicitud_TraerXBeneficiario
        public List<SolicitudVamosPaseo> Solicitud_TraerXBeneficiario(long idBeneficiario)
        {
            sql = "VamosDePaseo_solicitud_Trae_Beneficiario";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<SolicitudVamosPaseo> lstSol = new List<SolicitudVamosPaseo>();

            db.AddInParameter(dbCommand, "@idBeneficiario", DbType.Int64, idBeneficiario);
            dbParametros = dbCommand.Parameters;

            try
            {
                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (ds.Read())
                    {
                        SolicitudVamosPaseo oSol = new SolicitudVamosPaseo();
                        oSol.IdSolicitud = ds.GetInt64("IdSolicitud");
                        oSol.CantCuotas = ds.GetByte("CantCuotas");
                        oSol.FIngresoSolicitud = ds["fIngresoSolicitud"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(ds["fIngresoSolicitud"].ToString());
                        oSol.MensualSolicitud = ds.GetInt32("MensualSolicitud");
                        oSol.ImporteTotal = ds.GetDecimal("ImporteTotal");
                        oSol.ImporteFinanciado = ds.GetDecimal("ImporteFinanciado");
                        oSol.FDesde = ds["fDesde"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse( ds["fDesde"].ToString());
                        oSol.FHasta = ds["fHasta"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(ds["fHasta"].ToString());
                        oSol.Destino = ds.GetString("Destino");
                        oSol.Oficina = ds.GetString("oficina");
                        oSol.Usuario = ds.GetString("usuario");

                        oSol.UnaAuditoria = new Auditoria();
                        oSol.UnaAuditoria.Usuario = ds.GetString("usuario");
                        oSol.UnaAuditoria.IP = ds.GetString("ip");
         
                        
                        oSol.UnBeneficiario = new Beneficiario();
                        oSol.UnBeneficiario.ApellidoNombre = ds.GetString("ApellidoNombre");
                        oSol.UnBeneficiario.Cuil = long.Parse(ds.GetString("cuil"));
                        oSol.UnBeneficiario.IdBeneficiario = ds.GetInt64("IdBeneficiario");
                        
                        oSol.UnEstado = new Estado();
                        oSol.UnEstado.IdEstado = ds.GetByte("IdEstado");
                        
                        oSol.AgenciaMayorista = new Agencia();
                        oSol.AgenciaMayorista.IdAgencia = ds.GetInt32("idAgencia");
                        oSol.AgenciaMayorista.Descripcion = ds.GetString("descripcion");
                        oSol.AgenciaMayorista.Cuit = ds.GetString("cuit");
                        oSol.AgenciaMayorista.NroCuenta = ds.GetString("NroCuenta");
                        oSol.AgenciaMayorista.EsMayorista = true;
                        
                        oSol.AgenciaMinorista = new Agencia();            
                        oSol.AgenciaMinorista.IdAgencia = ds.GetInt32("idAgenciaAMin");
                        oSol.AgenciaMinorista.NroLegajo = ds.GetInt32("NroLegajoAMin");                        
                        oSol.AgenciaMinorista.NroCuenta = ds.GetString("NroCuentaAMin");                        
                        oSol.AgenciaMinorista.Cuit = ds.GetString("cuitAMin");
                        oSol.AgenciaMinorista.Descripcion = ds.GetString("descripcionAMin");
                        oSol.AgenciaMinorista.EsMayorista = false;

                        lstSol.Add(oSol);
                    }
                }

                return lstSol;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw new Exception("Error en SolicitudVamosPaseoDAO.Solicitud_TraerXBeneficiario", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion 

        #region Solicitud_TraerXBeneficiario_NroComprobante
        public SolicitudVamosPaseo Solicitud_TraerXBeneficiario_NroComprobante(long idBeneficiario, int nroComprobante)
        {
            sql = "VamosDePaseo_solicitud_Trae_Beneficiario_NroComprobante";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            SolicitudVamosPaseo oSol = new SolicitudVamosPaseo();

            db.AddInParameter(dbCommand, "@idBeneficiario", DbType.Int64, idBeneficiario);
            db.AddInParameter(dbCommand, "@nroComprobante", DbType.Int32, nroComprobante);
            dbParametros = dbCommand.Parameters;

            try
            {
                using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    if (ds.Read())
                    {
                        oSol.IdSolicitud = ds.GetInt64("IdSolicitud");
                        oSol.CantCuotas = ds.GetByte("CantCuotas");
                        oSol.FIngresoSolicitud = ds["fIngresoSolicitud"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(ds["fIngresoSolicitud"].ToString());
                        oSol.MensualSolicitud = ds.GetInt32("MensualSolicitud");
                        oSol.ImporteTotal = ds.GetDecimal("ImporteTotal");
                        oSol.ImporteFinanciado = ds.GetDecimal("ImporteFinanciado");
                        oSol.FDesde = ds["fDesde"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(ds["fDesde"].ToString());
                        oSol.FHasta = ds["fHasta"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(ds["fHasta"].ToString());
                        oSol.Destino = ds.GetString("Destino");
                        oSol.Oficina = ds.GetString("oficina");
                        oSol.Usuario = ds.GetString("usuario");

                        oSol.UnaAuditoria = new Auditoria();
                        oSol.UnaAuditoria.Usuario = ds.GetString("usuario");
                        oSol.UnaAuditoria.IP = ds.GetString("ip");

                        oSol.UnBeneficiario = new Beneficiario();
                        oSol.UnBeneficiario.ApellidoNombre = ds.GetString("ApellidoNombre");
                        oSol.UnBeneficiario.Cuil = long.Parse(ds.GetString("cuil"));
                        oSol.UnBeneficiario.IdBeneficiario = ds.GetInt64("IdBeneficiario");

                        oSol.UnEstado = new Estado();
                        oSol.UnEstado.IdEstado = ds.GetByte("IdEstado");

                        oSol.AgenciaMayorista = new Agencia();
                        oSol.AgenciaMayorista.IdAgencia = ds.GetInt32("idAgencia");
                        oSol.AgenciaMayorista.Descripcion = ds.GetString("descripcion");
                        oSol.AgenciaMayorista.Cuit = ds.GetString("cuit");
                        oSol.AgenciaMayorista.NroCuenta = ds.GetString("NroCuenta");                        
                        oSol.AgenciaMayorista.EsMayorista = true;

                        oSol.AgenciaMinorista = new Agencia();
                        oSol.AgenciaMinorista.IdAgencia = ds.GetInt32("idAgenciaAMin");
                        oSol.AgenciaMinorista.NroLegajo = ds.GetInt32("NroLegajoAMin");
                        oSol.AgenciaMinorista.NroCuenta = ds.GetString("NroCuentaAMin");
                        oSol.AgenciaMinorista.Cuit = ds.GetString("cuitAMin");
                        oSol.AgenciaMinorista.Descripcion = ds.GetString("descripcionAMin");
                        oSol.AgenciaMinorista.EsMayorista = false;                        
                    }
                }

                return oSol;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message)); 
                throw new Exception("Error en SolicitudVamosPaseoDAO.Solicitud_TraerXBeneficiario", ex);
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
