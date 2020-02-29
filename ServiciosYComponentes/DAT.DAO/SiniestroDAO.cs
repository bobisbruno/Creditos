using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Data.SqlClient;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    public class SiniestroDAO
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SiniestroDAO).Name);

        public static List<TipoEstadoSiniestro> TipoEstadoSiniestro_Traer()
        {
            string sql = "TipoEstadoSiniestro_Trae";           
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            List<TipoEstadoSiniestro> rdo = new List<TipoEstadoSiniestro>();

            try
            {
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        TipoEstadoSiniestro tipo = new TipoEstadoSiniestro(int.Parse(dr["idEstadoSiniestro"].ToString()),
                                                                           dr["descripcionEstadoSiniestro"].ToString(),
                                                                           Boolean.Parse(dr["solAsignacion"].ToString()),
                                                                           Boolean.Parse(dr["solImpresion"].ToString()),
                                                                           Boolean.Parse(dr["solResumen"].ToString()),
                                                                           Boolean.Parse(dr["fueImpreso"].ToString()),
                                                                           Boolean.Parse(dr["fuePresentado"].ToString()),
                                                                           Boolean.Parse(dr["fueCobrado"].ToString()),
                                                                           Boolean.Parse(dr["esBaja"].ToString()),                                                                          
                                                                           Boolean.Parse(dr["filtraXOperador"].ToString()),
                                                                           Boolean.Parse(dr["habilitado"].ToString()),
                                                                           Boolean.Parse(dr["habilitadoWEB"].ToString()),
                                                                           Boolean.Parse(dr["debeTenerIdResumen"].ToString()));
                        rdo.Add(tipo);
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

        public static List<TipoPolizaSeguro> TipoPolizaSeguro_Traer()
        {
            string sql = "TipoPolizaSeguro_T";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            List<TipoPolizaSeguro> rdo = new List<TipoPolizaSeguro>();

            try
            {
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {                        
                        rdo.Add(new TipoPolizaSeguro(int.Parse(dr["idPolizaSeguro"].ToString()),
                                                     dr["descripcionPolizaSeguro"].ToString()));
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

        public static List<Usuario> OperadorSiniestro_Traer(string idOperador)
        {
            string sql = "OperadorSiniestro_Trae";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            List<Usuario> rdo = new List<Usuario>();

            try
            {
                db.AddInParameter(dbCommand, "@idOperador", DbType.String, string.IsNullOrEmpty(idOperador)? null : idOperador);

                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        Usuario tipo = new Usuario(dr["idOperador"].ToString(),
                                                   dr["apellidoNombreOperador"].ToString());
                        rdo.Add(tipo);
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

        public static List<NovedadSiniestro> NovedadSiniestrosCobrado_Traer(int? idEstado,int? idPolizaSeguro,bool? esGraciable, string operador, string usuario, long idNovedad, string cuil, int idResumen,
                                                                            DateTime? fFallecimientoDesde, DateTime? fFallecimientoHasta, int nroPagina, out int cantTotal, out int idUltimoResumen, 
                                                                            out int cantUltimoResumen,  out int cantPaginas)
        {
            string sql = "Novedades_SiniestrosCobrado_T";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);

            List<NovedadSiniestro> rdo = new List<NovedadSiniestro>();
            cantTotal = idUltimoResumen = cantUltimoResumen = cantPaginas =  0;

            try
            {
                db.AddInParameter(dbCommand, "@idEstado", DbType.Int32, idEstado);
                db.AddInParameter(dbCommand, "@idPolizaSeguro", DbType.Int32, idPolizaSeguro);
                db.AddInParameter(dbCommand, "@esGraciable", DbType.Boolean, esGraciable);
                db.AddInParameter(dbCommand, "@idOperadorAsignado", DbType.String, operador);
                db.AddInParameter(dbCommand, "@idNovedad", DbType.Int64, idNovedad);
                db.AddInParameter(dbCommand, "@cuil", DbType.String, cuil);
                db.AddInParameter(dbCommand, "@usuario", DbType.String, usuario);
                db.AddInParameter(dbCommand, "@FechaFallecimientoDesde", DbType.String, fFallecimientoDesde.HasValue ? fFallecimientoDesde.Value.ToString("yyyyMMdd") : null);
                db.AddInParameter(dbCommand, "@FechaFallecimientoHasta", DbType.String, fFallecimientoHasta.HasValue ? fFallecimientoHasta.Value.ToString("yyyyMMdd") : null);
                db.AddInParameter(dbCommand, "@idResumen", DbType.Int32, idResumen);
                db.AddInParameter(dbCommand, "@NroPagina", DbType.Int32, nroPagina);
                db.AddOutParameter(dbCommand, "@canttotal", DbType.Int32,cantTotal);
                db.AddOutParameter(dbCommand, "@idUltimoResumen", DbType.Int32, idUltimoResumen);
                db.AddOutParameter(dbCommand, "@cantidadUltimoResumen", DbType.Int32, cantUltimoResumen);
                db.AddOutParameter(dbCommand, "@cantPaginas", DbType.Int32, cantPaginas);

                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read()) 
                    {                       
                       NovedadSiniestro novedad = new NovedadSiniestro(long.Parse(dr["idSiniestro"].ToString()),                                                                     
                                                                        long.Parse(dr["idNovedad"].ToString()),
                                                                        long.Parse(dr["Cuil"].ToString()),
                                                                        dr["ApellidoNombre"].ToString(),
                                                                        dr["ffallecimiento"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["ffallecimiento"].ToString()),
                                                                        dr["esGraciable"].Equals(DBNull.Value)? false: Boolean.Parse(dr["esGraciable"].ToString()),
                                                                        dr["nda_fecnov"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["nda_fecnov"].ToString()),
                                                                        dr["nda_montoPrestamo"].Equals(DBNull.Value) ? 0 : double.Parse(dr["nda_montoPrestamo"].ToString()),
                                                                        dr["ImporteSolicitado"].Equals(DBNull.Value) ? 0 : double.Parse(dr["ImporteSolicitado"].ToString()),
                                                                        new TipoEstadoSiniestro(int.Parse(dr["idEstadoSiniestro"].ToString()), 
                                                                                                dr["descripcionEstadoSiniestro"].ToString(),
                                                                                                bool.Parse(dr["solAsignacion"].ToString()),
                                                                                                bool.Parse(dr["solImpresion"].ToString()),
                                                                                                bool.Parse(dr["solResumen"].ToString()),
                                                                                                bool.Parse(dr["fueImpreso"].ToString()),
                                                                                                bool.Parse(dr["fuePresentado"].ToString()),
                                                                                                bool.Parse(dr["fueCobrado"].ToString()),
                                                                                                bool.Parse(dr["esBaja"].ToString())),
                                                                        dr["IdLote"].Equals(DBNull.Value) ? 0 : int.Parse(dr["IdLote"].ToString()),
                                                                        dr["fSolicitudCobro"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["fSolicitudCobro"].ToString()),                                                                        
                                                                        dr["importeCobrado"].Equals(DBNull.Value) ? 0 : double.Parse(dr["importeCobrado"].ToString()),
                                                                        dr["fCobroFGS"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["fCobroFGS"].ToString()),
                                                                        dr["idOperadorAsignado"].ToString(),
                                                                        new Usuario(dr["usuario"].ToString(), 
                                                                                    dr["apellidoNombreOperador"].ToString())); 
                        
                        if(!dr["idResumen"].Equals(DBNull.Value))
                        {
                            novedad.Resumen = new NovedadSiniestroResumen(dr["idResumen"].Equals(DBNull.Value) ? 0 :int.Parse(dr["idResumen"].ToString()),
                                                                          dr["fResumen"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["fResumen"].ToString()),
                                                                          dr["cantidadSiniestros"].Equals(DBNull.Value) ? 0 : int.Parse(dr["cantidadSiniestros"].ToString()),
                                                                          new TipoPolizaSeguro(int.Parse(dr["idPolizaSeguro"].ToString()),
                                                                                               dr["descripcionPolizaSeguro"].ToString()),
                                                                          new Usuario(dr["usuarioResumen"].ToString()));
                        }
                        
                       rdo.Add(novedad);
                    }

                    dr.Close();
                    dr.Dispose();
                }
               
                cantTotal = string.IsNullOrEmpty(db.GetParameterValue(dbCommand, "@canttotal").ToString()) ? cantTotal : Int32.Parse(db.GetParameterValue(dbCommand, "@canttotal").ToString());
                idUltimoResumen = string.IsNullOrEmpty(db.GetParameterValue(dbCommand, "@idUltimoResumen").ToString()) ? idUltimoResumen : Int32.Parse(db.GetParameterValue(dbCommand, "@idUltimoResumen").ToString());
                cantUltimoResumen = string.IsNullOrEmpty(db.GetParameterValue(dbCommand, "@cantidadUltimoResumen").ToString()) ? cantUltimoResumen : Int32.Parse(db.GetParameterValue(dbCommand, "@cantidadUltimoResumen").ToString());
                cantPaginas = string.IsNullOrEmpty(db.GetParameterValue(dbCommand, "@cantPaginas").ToString()) ? cantPaginas : Int32.Parse(db.GetParameterValue(dbCommand, "@cantPaginas").ToString());
                
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

        public static void NovedadSiniestrosCobrado_CambioEstado(string novAsignar, int idEstado,string idOperadorAsignado, Usuario usuario)
        {
            string sql = "Novedades_SiniestrosCobrado_CambioEstado";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);                   
           
            try
            {
                db.AddInParameter(dbCommand, "@novedades", DbType.AnsiString, novAsignar);
                db.AddInParameter(dbCommand, "@idEstado", DbType.Int32, idEstado);
                db.AddInParameter(dbCommand, "@idOperadorAsignado", DbType.String, idOperadorAsignado);
                db.AddInParameter(dbCommand, "@usuario", DbType.String, usuario.Legajo);
                db.AddInParameter(dbCommand, "@ip", DbType.String, usuario.Ip);
                db.AddInParameter(dbCommand, "@oficina", DbType.String, usuario.OficinaCodigo);
              
                db.ExecuteNonQuery(dbCommand);               
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw new Exception("Error en NovedadSiniestrosCobrado_CambioEstado - ERROR: " + err.Message + " - SRC: " + err.Source);
            }            
        }

        public static void NovedadSiniestrosResumen_Alta(List<NovedadSiniestro> novedades, string idOperador, Usuario usuario, int? idPolizaSeguro, bool? esGraciable, int idResumenAgregar, out int idResumen, out string mensaje)
        {
            string sql = "Novedades_SiniestrosResumen_A";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
           
            try
            {   
                db.AddInParameter(dbCommand, "@novedades", DbType.AnsiString, obtenerXML(novedades));
                db.AddInParameter(dbCommand, "@idOperador", DbType.String, idOperador);
                db.AddInParameter(dbCommand, "@ip", DbType.String, usuario.Ip);
                db.AddInParameter(dbCommand, "@usuario", DbType.String, usuario.Legajo);
                db.AddInParameter(dbCommand, "@oficina", DbType.String, usuario.OficinaCodigo);  
                db.AddInParameter(dbCommand, "@idPolizaSeguro", DbType.Int32, idPolizaSeguro);
                db.AddInParameter(dbCommand, "@esGraciable", DbType.Boolean, esGraciable); 
                db.AddInParameter(dbCommand, "@idResumenAgregar", DbType.Int32, idResumenAgregar);
                db.AddOutParameter(dbCommand, "@idresumen", DbType.Int32, 0);
                db.AddOutParameter(dbCommand, "@mensaje", DbType.String, 200);
            
                 db.ExecuteNonQuery(dbCommand);

                 idResumen = string.IsNullOrEmpty(db.GetParameterValue(dbCommand, "@idresumen").ToString()) ? 0 : Convert.ToInt32(db.GetParameterValue(dbCommand, "@idresumen").ToString());
                 mensaje = db.GetParameterValue(dbCommand, "@mensaje").ToString();
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

        public static List<NovedadSiniestroResumen> NovedadSiniestrosResumen_Traer(int idResumen)
        {
            string sql = "Novedades_SiniestrosResumen_T";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<NovedadSiniestroResumen> rdo = new List<NovedadSiniestroResumen>();
            NovedadSiniestroResumen novedad= null;
            
            try
            {
                db.AddInParameter(dbCommand, "@idresumen", DbType.Int32, idResumen);
                
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                { 
                    while (dr.Read())
                    {
                        Usuario oUsuario = new Usuario(dr["usuarioResumen"].ToString(), dr["usuario"].ToString());
                        oUsuario.Ip = dr["ip"].ToString();

                        novedad= new NovedadSiniestroResumen(int.Parse(dr["idresumen"].ToString()),
                                                             int.Parse(dr["idOrden"].ToString()),
                                                             int.Parse(dr["idSiniestro"].ToString()),
                                                             long.Parse(dr["cuil"].ToString()),
                                                             dr["ffallecimiento"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["ffallecimiento"].ToString()),
                                                             dr["ApellidoNombre"].ToString(),
                                                             dr["fNacimiento"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["fNacimiento"].ToString()),
                                                             dr["Sexo"].ToString(),
                                                             long.Parse(dr["nda_idBeneficiario"].ToString()),
                                                             long.Parse(dr["idNovedad"].ToString()),
                                                             dr["nda_fecnov"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["nda_fecnov"].ToString()),
                                                             dr["nda_montoPrestamo"].Equals(DBNull.Value) ? 0 : double.Parse(dr["nda_montoPrestamo"].ToString()),
                                                             dr["nda_cantcuotas"].Equals(DBNull.Value) ? 0 : int.Parse(dr["nda_cantcuotas"].ToString()),
                                                             dr["ImporteSolicitado"].Equals(DBNull.Value) ? 0 : double.Parse(dr["ImporteSolicitado"].ToString()),
                                                             oUsuario);

                        novedad.FechaResumen =  dr["fResumen"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["fResumen"].ToString());
                        novedad.CantidadSiniestros = dr["cantidadSiniestros"].Equals(DBNull.Value) ? 0 : int.Parse(dr["cantidadSiniestros"].ToString());
                        novedad.TipoPolizaSeguro =  new TipoPolizaSeguro(int.Parse(dr["idPolizaSeguro"].ToString()),
                                                                         dr["descripcionPolizaSeguro"].ToString());                                                                         
 
                        rdo.Add(novedad);                 
                    }

                    dr.Close();
                    dr.Dispose();
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
            }
        }

        public static List<string> NovedadSiniestroResumen_TraerTXT(int idResumen)
        {
            string sql = "Novedades_SiniestrosResumen_TArchivo";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<string> rdo = new List<string>();

            try
            {
                db.AddInParameter(dbCommand, "@idresumen", DbType.Int32, idResumen);

                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    while (dr.Read())
                    {
                        rdo.Add(dr["dato"].ToString());
                    }

                    dr.Close();
                    dr.Dispose();
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
            }
        }

        public static TipoCuentaBancariaSiniestro TipoCuentaBancariaSiniestro_Traer()
        {
            string sql = "TipoCuentaBancariaSiniestro_T";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            TipoCuentaBancariaSiniestro rdo= null;

            try
            { 
                using (IDataReader dr = db.ExecuteReader(dbCommand))
                {
                    if (dr.Read())
                    {
                        rdo = new TipoCuentaBancariaSiniestro(dr["tipoCuenta"].ToString(),
                                                              dr["cbu"].ToString(),
                                                              dr["nroCuenta"].ToString(),
                                                              dr["banco"].ToString(),
                                                              dr["fdesde"].Equals(DBNull.Value) ? DateTime.MinValue : DateTime.Parse(dr["fdesde"].ToString()),
                                                              dr["fHasta"].Equals(DBNull.Value) ? DateTime.MinValue : DateTime.Parse(dr["fHasta"].ToString()));
                    }

                    dr.Close();
                    dr.Dispose();
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
            }
        }

        public static void NovedadSiniestroImpresion_Alta(NovedadSiniestroImpresion novedad)
        {
            string sql = "Novedades_SiniestrosImpresion_A";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
     
            try
            {
                db.AddInParameter(dbCommand, "@idSiniestro", DbType.Int64, novedad.IdSiniestro);
                db.AddInParameter(dbCommand, "@idResumen", DbType.Int32, novedad.IdResumen);
                db.AddInParameter(dbCommand, "@idDocumentoImpreso", DbType.Int32, novedad.IdDocumentoImpreso);
                db.AddInParameter(dbCommand, "@usuario", DbType.String, novedad.Usuario.Legajo);
                db.AddInParameter(dbCommand, "@ip", DbType.String, novedad.Usuario.Ip);
                db.AddInParameter(dbCommand, "@oficina", DbType.String, novedad.Usuario.OficinaCodigo);
 
                 db.ExecuteNonQuery(dbCommand);
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

        private static string obtenerXML(List<NovedadSiniestro> novedades)
       {
            string xml = "<Siniestros>";

            foreach (NovedadSiniestro item in novedades)
            {
                 xml += ("<Siniestro>" +
                         "<IdSiniestro>" + item.IdSiniestro.ToString() +"</IdSiniestro>" +
                         "<Cuil>" + item.Cuil.ToString() + "</Cuil>" +
                         "<FechaFallecimiento>" + item.FechaFallecimiento.ToString("yyyyMMdd") + "</FechaFallecimiento>" +
                         "<ApellidoNombre>" + item.ApellidoNombre + "</ApellidoNombre>" +
                         "<FechaNacimiento>" + item.FechaNacimiento.ToString("yyyyMMdd") + "</FechaNacimiento>" +
                         "<Sexo>" + (string.IsNullOrEmpty(item.Sexo) ? string.Empty : item.Sexo.ToString()) + "</Sexo>" +
                         "<IdBeneficiario>" + item.IdBeneficiario.ToString() + "</IdBeneficiario>" +
                         "<IdNovedad>" + item.IdNovedad.ToString() + "</IdNovedad>" +
                         "<MontoPrestamo>" + item.MontoPrestamo.ToString().Replace(',', '.') + "</MontoPrestamo>" +
                         "<CantCuotas>" + item.CantCuotas.ToString() + "</CantCuotas>" +
                         "<ImporteAReclamar>" + item.ImporteSolicitado.ToString().Replace(',', '.') + "</ImporteAReclamar>" +                        
                         "</Siniestro>");
            }

            xml += "</Siniestros>";
            return xml;
        }
    }
}
