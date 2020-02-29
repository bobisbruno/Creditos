using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using NullableReaders;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Configuration;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
    public class ReclamoDAO 
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ReclamoDAO).Name); 

        #region  ReclamoEstadoGrabar
        public static ResultadoUnico<string, int> Estado_Grabar(Reclamo oReclamo)
       
        {
            ResultadoUnico<string, int> oResultado = new ResultadoUnico<string, int>();
            string sql = "Reclamo_estado_grabar";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;

            try
            {
                db.AddInParameter(dbCommand, "@IdReclamo", DbType.Int64, oReclamo.IdReclamo);
                db.AddInParameter(dbCommand, "@IdEstadoReclamo", DbType.Int32, oReclamo.UnEstadoReclamo.IdEstado);
                db.AddInParameter(dbCommand, "@fecCambio", DbType.DateTime, oReclamo.UnEstadoReclamo.FecCambio);
                db.AddInParameter(dbCommand, "@observacion", DbType.String, oReclamo.UnEstadoReclamo.observacion);
                db.AddInParameter(dbCommand, "@idOficina", DbType.String, oReclamo.UnaAuditoria.IDOficina);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, oReclamo.UnaAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@ip", DbType.String, oReclamo.UnaAuditoria.IP);

                dbParametros = dbCommand.Parameters;
                db.ExecuteNonQuery(dbCommand);

                oResultado.DatoUnico2 = 0;
                return oResultado;
            }
            catch (System.Data.SqlClient.SqlException err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));   
                oResultado.Error = new Error();
                oResultado.Error.Descripcion = err.Message;
                return oResultado;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));   
                oResultado.Error = new Error();
                oResultado.Error.Descripcion = err.Message;
                return oResultado;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();                
            }
        }
        #endregion
       
        #region  AddReclamo
        public static ResultadoUnico<string, int>AddReclamo(Reclamo unReclamo)
       // public static int AddReclamo(Reclamo unReclamo)
        {
            
            ResultadoUnico<string, int> oResultado = new ResultadoUnico<string, int>();
            string sql = "Reclamo_grabar";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;

            try
            {
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, unReclamo.UnaNovedad.UnBeneficiario.IdBeneficiario);
                db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int32, unReclamo.UnaNovedad.UnPrestador.ID);
                db.AddInParameter(dbCommand, "@codConceptoLiq", DbType.Int64, unReclamo.UnaNovedad.UnConceptoLiquidacion.CodConceptoLiq);
                db.AddInParameter(dbCommand, "@DescripcionReclamo", DbType.String, unReclamo.DescripcionReclamo);
                db.AddInParameter(dbCommand, "@IdTipoReclamo", DbType.Int32, unReclamo.UnTipoReclamo.IdTipoReclamo);
                db.AddInParameter(dbCommand, "@expediente", DbType.String, unReclamo.Expediente);
                db.AddInParameter(dbCommand, "@Observacion", DbType.String, unReclamo.UnEstadoReclamo.observacion);
                db.AddInParameter(dbCommand, "@PresentoDoc", DbType.Boolean, unReclamo.PresentoDoc);
                db.AddInParameter(dbCommand, "@IdEstadoReclamo", DbType.Int32, unReclamo.UnEstadoReclamo.IdEstado);
                db.AddInParameter(dbCommand, "@idOficina", DbType.String, unReclamo.UnaAuditoria.IDOficina);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, unReclamo.UnaAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@ip", DbType.String, unReclamo.UnaAuditoria.IP);
                db.AddInParameter(dbCommand, "@IdReclamo", DbType.Int64, unReclamo.IdReclamo);
                db.AddInParameter(dbCommand, "@solicitaReintegro", DbType.Boolean, unReclamo.SolicitaReintegro);
                db.AddOutParameter(dbCommand, "@IdReclamoOUT", DbType.Int64, 20);
                
                dbParametros = dbCommand.Parameters;
                db.ExecuteNonQuery(dbCommand);

                oResultado.DatoUnico2 = Convert.ToInt32(db.GetParameterValue(dbCommand, "IdReclamoOUT"));
                return oResultado;
            }
            catch (System.Data.SqlClient.SqlException err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));   
                oResultado.Error = new Error();
                oResultado.Error.Descripcion = err.Message;
                return oResultado;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));   
                oResultado.Error = new Error();
                oResultado.Error.Descripcion = err.Message;
                return oResultado;
            
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
               

            }
        }
        #endregion

        #region Reclamo_Traer
        public static List<Reclamo> Reclamo_Traer(long idBeneficiario, long idPrestador, long idReclamo, int idEstado, DateTime FechaDesde, DateTime FechaHasta, string cuil)
        {
                List<Reclamo> listaReclamos = new List<Reclamo>();
                string sql = "Reclamo_Trae_Filtro";
                Database db = DatabaseFactory.CreateDatabase("DAT_V01");
                DbCommand dbCommand = db.GetStoredProcCommand(sql);
                DbParameterCollection dbParametros = null;
                Reclamo unReclamo;

            try
                {
                    if (idBeneficiario != 0)
                    {
                        db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                    }
                    else
                    {
                        db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, null);
                    }

                    if (idPrestador != 0)
                    {
                        db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, idPrestador);
                    }
                    else
                    {
                        db.AddInParameter(dbCommand, "@IdPrestador", DbType.Int64, null);
                    }

                    if (idReclamo != 0)
                    {
                        db.AddInParameter(dbCommand, "@IdReclamo", DbType.Int64, idReclamo);
                    }
                    else
                    {
                        db.AddInParameter(dbCommand, "@IdReclamo", DbType.Int64, null);
                    }

                    if (idEstado != 0)
                    {
                        db.AddInParameter(dbCommand, "@idEstado", DbType.Int32, idEstado);
                    }
                    else
                    {
                        db.AddInParameter(dbCommand, "@idEstado", DbType.Int64, null);
                    }

                    if (FechaDesde != DateTime.MinValue)
                        db.AddInParameter(dbCommand, "@FechaDesde", DbType.DateTime, FechaDesde);
                    else
                        db.AddInParameter(dbCommand, "@FechaDesde", DbType.DateTime, null);

                    if (FechaHasta != DateTime.MinValue)
                        db.AddInParameter(dbCommand, "@FechaHasta", DbType.DateTime, FechaHasta);
                    else
                        db.AddInParameter(dbCommand, "@FechaHasta", DbType.DateTime, null);
                    if (!string.IsNullOrEmpty(cuil))
                        db.AddInParameter(dbCommand, "@Cuil", DbType.String, cuil);

                    dbParametros = dbCommand.Parameters;
                    using (NullableDataReader ds = new NullableDataReader(db.ExecuteReader(dbCommand)))
                    {
                        while (ds.Read())
                        {
                            Fill(out unReclamo, ds);
                            listaReclamos.Add(unReclamo);
                        }
                        ds.Close();
                    }

                    return listaReclamos;
                }
          
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));   
                throw new Exception("Error en ReclamoDAO.FiltroReclamo", ex);
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion
       
        #region Fill
        public static void Fill(out Reclamo unReclamo, NullableDataReader dr)
        {
                 unReclamo = new Reclamo();
                 unReclamo.IdReclamo= long.Parse(dr["idReclamo"].ToString());
                 unReclamo.FechaAltaReclamo= DateTime.Parse (dr["FAltaReclamo"].ToString());
                 unReclamo.DescripcionReclamo= dr["DescripcionReclamo"].ToString();
                 unReclamo.Expediente=dr["Expediente"].ToString();
                 unReclamo.PresentoDoc =Boolean.Parse(dr["PresentoDoc"].ToString());
                 unReclamo.SolicitaReintegro = Boolean.Parse(dr["SolicitaReintegro"].ToString());
                 unReclamo.UnaNovedad = new Novedad();

                unReclamo.UnaNovedad.UnBeneficiario = new Beneficiario (
                    long.Parse(dr["idBeneficiario"].ToString()),
                    long.Parse(dr["Cuil"].ToString()),
                    dr["ApellidoNombre"].ToString()
                    );
            
                unReclamo.UnaNovedad.UnPrestador = new Prestador(
                        long.Parse(dr["idPrestador"].ToString()),
                        dr["RazonSocial"].ToString(),     
                        long.Parse(dr["cuitPrestador"].ToString())
                    );

                unReclamo.UnaNovedad.UnConceptoLiquidacion = new ConceptoLiquidacion(
                    int.Parse(dr["codConceptoLiq"].ToString()), 
                        dr["DescConceptoLiq"].ToString());                       
                
                unReclamo.UnTipoReclamo = new TipoReclamo(
                    int.Parse(dr["idTipoReclamo"].ToString()), 
                        dr["DescTipoReclamo"].ToString());      
                
               
                
                bool bVencido = false;
                if (int.Parse(dr["IdEstadoReclamo"].ToString()) == 7)
                {
                    int CantDiasHabiles = ParametroDAO.DiasHabiles();
                    if (SumaDiasHabiles(dr.GetDateTime("FecCambio"), CantDiasHabiles) < DateTime.Today)
                    { 
                        /*Reclamo Vencido*/
                        bVencido = true;
                        unReclamo.UnEstadoReclamo = EstadoReclamoDAO.Traer(14);
                    }
                }
                if (!bVencido)
                {
                    unReclamo.UnEstadoReclamo = new EstadoReclamo();
                    unReclamo.UnEstadoReclamo.IdEstado = int.Parse(dr["IdEstadoReclamo"].ToString());
                    unReclamo.UnEstadoReclamo.DescEstado = dr["DescEstado"].ToString();
                }
                unReclamo.UnEstadoReclamo.observacion = dr["observacion"].ToString();
                unReclamo.UnEstadoReclamo.FecCambio = dr.GetDateTime("FecCambio");
                unReclamo.UnEstadoReclamo.UnAuditoria = new Auditoria();
                unReclamo.UnEstadoReclamo.UnAuditoria.IP = dr["IPRespuesta"].ToString();
                unReclamo.UnEstadoReclamo.UnAuditoria.Usuario = dr["UsuarioRespuesta"].ToString();
                unReclamo.UnEstadoReclamo.UnAuditoria.IDOficina = string.IsNullOrEmpty(dr["IdOficinaRespuesta"].ToString()) ? 0 : int.Parse(dr["IdOficinaRespuesta"].ToString());

                unReclamo.UnaAuditoria = new Auditoria(
                dr["UsuarioCarga"].ToString(),
                dr["IP"].ToString(), 
                dr.GetNullableDateTime("FecUltModificacion"));     

		

                int IdOficina=0;

                unReclamo.UnaAuditoriaRespuesta = new Auditoria();

                if (!string.IsNullOrEmpty(dr["UsuarioRespuesta"].ToString()))
                    unReclamo.UnaAuditoriaRespuesta.Usuario = dr["UsuarioRespuesta"].ToString();

                if (int.TryParse(dr["idOficinaRespuesta"].ToString(), out IdOficina))
                    unReclamo.UnaAuditoriaRespuesta.IDOficina = IdOficina;
                if (!string.IsNullOrEmpty(dr["IPRespuesta"].ToString()))
                    unReclamo.UnaAuditoriaRespuesta.IP = dr["IPRespuesta"].ToString();
            

                //unReclamo.UnaAuditoriaRespuesta.IdOficina = int.Parse(dr["IdOficinaRespuesta"].ToString()); 
        }
        #endregion
   
        #region  ImpresionGrabar
        public static ResultadoUnico<string, int> Impresion_Historia_Grabar(ModeloImpresion oModelo)
        // public static int AddReclamo(Reclamo unReclamo)
        {

            ResultadoUnico<string, int> oResultado = new ResultadoUnico<string, int>();
            string sql = "Reclamo_Impresion_Historia_Grabar";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            DbParameterCollection dbParametros = null;

            try
            {
                db.AddInParameter(dbCommand, "@IdReclamo", DbType.Int64, oModelo.IdReclamo);
                db.AddInParameter(dbCommand, "@IdModelo", DbType.Int32, oModelo.IdModelo);
                db.AddInParameter(dbCommand, "@Usuario", DbType.String, oModelo.unaAuditoria.Usuario);
                db.AddInParameter(dbCommand, "@idOficina", DbType.Int32, oModelo.unaAuditoria.IDOficina);
                db.AddInParameter(dbCommand, "@IP", DbType.String, oModelo.unaAuditoria.IP);

                dbParametros = dbCommand.Parameters;
                db.ExecuteNonQuery(dbCommand);

                oResultado.DatoUnico2 = 0;
                return oResultado;
            }
            catch (System.Data.SqlClient.SqlException err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));   
                oResultado.Error = new Error();
                oResultado.Error.Descripcion = err.Message;
                return oResultado;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));   
                oResultado.Error = new Error();
                oResultado.Error.Descripcion = err.Message;
                return oResultado;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
        }
        #endregion
        private static  DateTime SumaDiasHabiles(DateTime fecha, int Totaldías)
        {
            int incremento = 0;
            bool ok = false;
            while (incremento < Totaldías)
            {
                fecha = fecha.AddDays(1);
                ok = (fecha.DayOfWeek != DayOfWeek.Saturday);
                if (ok)
                    ok = (fecha.DayOfWeek != DayOfWeek.Sunday);
                //if (ok)
                //{
                //    DateTime d = (LstFeriados.Find(delegate(DateTime date) { return (date.Day == fecha.Day && date.Month == fecha.Month); }));
                //    ok = (DateTime.MinValue == d);
                //}
                if (ok)
                    incremento += 1;

            }
            return fecha;
        }
    }
}
