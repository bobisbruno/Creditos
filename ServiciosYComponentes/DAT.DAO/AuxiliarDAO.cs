using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Configuration;
using System.EnterpriseServices;
using System.Diagnostics;
using System.Data.Common;
using NullableReaders;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
	public class AuxiliarDAO
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(AuxiliarDAO).Name);
        public AuxiliarDAO(){}
		
		#region Traer Tipos de Concepto

        public static List<TipoConcepto> TraerTiposConceptos()
		{         
            string sql = "Admin_TipoConc_Trae";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<TipoConcepto> oListTipoConcepto = new List<TipoConcepto>();

            try
            {
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        oListTipoConcepto.Add(new TipoConcepto(
                                              Int16.Parse(dr["TipoConcepto"].ToString()),
                                              dr["DescTipoConcepto"].ToString()));
                    }
                }
                return oListTipoConcepto;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
		}
		#endregion Traer Tipos de Concepto
		
		#region Trae Variables DTS

		public DataSet TraerDTSVariables()
		{
			DataSet _ds = new DataSet();
			//Conexion objCnn = new Conexion();
			
			try
			{
				//_ds=SqlHelper.ExecuteDataset( objCnn.Conectar(), CommandType.StoredProcedure, "Admin_DTSTrae");
				return _ds;
			} 
			
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err ;
			}
			finally
			{

			}
		}
		#endregion Trae Variables DTS

		#region Trae Tipos Domicilio		

        public static List<TipoDomicilio> TraerTiposDomicilio()
		{
            string sql = "Admin_TiposDomicilio_Traer";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<TipoDomicilio> oListTipoDomicilio = new List<TipoDomicilio>();

            try
            {
                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {                        
                        oListTipoDomicilio.Add(new TipoDomicilio(Int16.Parse(dr["IDTipoDomicilio"].ToString()),
                                               dr["Descripcion"].ToString()));
                    }
                }
                return oListTipoDomicilio;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();
            }
		}
		#endregion Trae Tipos Domicilio

		#region Trae Entes Reguladores
	
  	    public DataSet TraerEntesReguladores()
		{
			DataSet _ds=new DataSet();
			//Conexion objCnn = new Conexion();
			
			try
			{
				//_ds=SqlHelper.ExecuteDataset( objCnn.Conectar(), CommandType.StoredProcedure, "Admin_EntesReguladores_Traer");
				return _ds;
			} 
			
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err ;
			}
			finally
			{
			
			}
		}
		#endregion Trae Entes Reguladores

		#region Trae Estados de Convenios

		public DataSet TraerEstadosConvenios()
		{
			DataSet _ds=new DataSet();
			//Conexion objCnn = new Conexion();
			
			try
			{
				//_ds=SqlHelper.ExecuteDataset( objCnn.Conectar(), CommandType.StoredProcedure, "Admin_EstadosConvenios_Traer");
				return _ds;
			} 
			
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err ;
			}
			finally
			{
				
			}
		}
		#endregion Trae Estados de Convenios
		
		#region Trae Bloqueos y / o inhibiciones

		public static void TraerBloqueosInhibiciones(int filtro, long idBeneficiario, string fechas)
		{
            string sql = "Admin_EstadosRegistro_Traer";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Estado> oListEstados = new List<Estado>();

            try
            {
                db.AddInParameter(dbCommand, "@Filtro", DbType.Int16, filtro);
                db.AddInParameter(dbCommand, "@IdBeneficiario", DbType.Int64, idBeneficiario);
                db.AddInParameter(dbCommand, "@Fechas", DbType.String, fechas);
                db.AddOutParameter(dbCommand, "@bloqueado", DbType.Boolean, 1);
                db.AddOutParameter(dbCommand, "@inhibido", DbType.Boolean, 1);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                            //Int64.Parse(dr["IdBeneficiario"].ToString()),
                            //dr["Cuil"].Equals(DBNull.Value) ? "" : dr["Cuil"].ToString(),
                            //dr["ApellidoNombre"].Equals(DBNull.Value) ? "" : dr["ApellidoNombre"].ToString(),
                            //DateTime.Parse(dr["FecInicio"].ToString()),
                            //dr["Origen"].Equals(DBNull.Value) ? "" : dr["Origen"].ToString(),
                            //dr["EntradaCAP"].Equals(DBNull.Value) ? "" : dr["EntradaCAP"].ToString(),
                            //dr["D_PCIA"].Equals(DBNull.Value) ? "" : dr["D_PCIA"].ToString(),
                            //dr["Causa"].Equals(DBNull.Value) ? "" : dr["Causa"].ToString(),
                            //dr["Juez"].Equals(DBNull.Value) ? "" : dr["Juez"].ToString(),
                            //dr["Secretaria"].Equals(DBNull.Value) ? "" : dr["Secretaria"].ToString(),
                            //dr["Actuacion"].Equals(DBNull.Value) ? "" : dr["Actuacion"].ToString(),
                            //dr["FecNotificacion"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecNotificacion"].ToString()),
                            //dr["NroNota"].Equals(DBNull.Value) ? "" : dr["NroNota"].ToString(),
                            //dr["Firmante"].Equals(DBNull.Value) ? "" : dr["Firmante"].ToString(),
                            //dr["Observaciones"].Equals(DBNull.Value) ? "" : dr["Observaciones"].ToString(),
                    }
                }
                //return oListEstados;
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
                db = null;
                dbCommand.Dispose();  
            }		
		}
		#endregion #region Trae Bloqueos y / o inhibiciones

		#region Trae Estados de Registro
        public static List<Estado> TraerEstadosRegBajaPorPerfil(string perfil, bool esBaja)
        {
            string sql = "RelacionTipoPerfilEstadoReg_TxPerfil";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Estado> oListEstados = new List<Estado>();

            try
            {
                db.AddInParameter(dbCommand, "@perfil", DbType.String, perfil);
                db.AddInParameter(dbCommand, "@esBaja", DbType.Boolean, esBaja);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        oListEstados.Add(new Estado(dr.GetByte("IdEstadoReg"),
                                                    dr.GetString("DescripcionEstadoReg"),
                                                    dr.GetBoolean("EsBaja")));
                    }
                }
                return oListEstados;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw ex;
            }
            finally
            {

            }
        }

		public static List<Estado> TraerEstadosReg(bool bajas)
		{
            string sql = "Admin_EstadosRegistro_Traer";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Estado> oListEstados = new List<Estado>();  

			try
            {
                Int16 selection = 0;
                if (!bajas)
                    selection = 1;
                db.AddInParameter(dbCommand, "@soloBajas", DbType.Int16, selection);

				using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        oListEstados.Add(new Estado(dr.GetByte("IdEstadoReg"),
                                                    dr.GetString("DescripcionEstadoReg"),
                                                    dr.GetBoolean("EsBaja")));
                                                    //dr.GetInt32("Opera"),                                                    
                    }
                }
                return oListEstados;
			} 			
			catch(Exception err)
			{
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err ;
			}
			finally
			{
				
			}
		}
		#endregion Trae Estados de Registro
	}
}
