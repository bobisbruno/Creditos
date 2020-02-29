using System;
using System.Data;
using System.Reflection;
using System.Configuration;
using System.EnterpriseServices;
using System.Diagnostics;
using System.Data.SqlClient;

using System.Runtime;
using System.IO;
using System.Text;
using System.Collections;

using System.Data.Common;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using NullableReaders;
using System.Collections.Generic;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    [Serializable]
	public class NovedadHistoricaDAO
	{
        private static readonly ILog log = LogManager.GetLogger(typeof(NovedadHistoricaDAO).Name);        
        
        public NovedadHistoricaDAO()
		{
		}

		#region NovedadHistorica Trae Consulta
        
        public static List<Novedad> NovedadHistorica_Trae_Consulta(byte criterio, byte opcion,
                                                                   long idPrestador, long benefCuil,
                                                                   byte tipoConc, int concOpp, string fecCierre)
        {
            string sql = "Novedades_Historica_Trae_V2";
            Database db = DatabaseFactory.CreateDatabase("DAT_V01");
            DbCommand dbCommand = db.GetStoredProcCommand(sql);
            List<Novedad> lstNovedades = new List<Novedad>();

            try
            {
                /*Criterio: 
                *		1 Descontado
                *		2 Descuento Parcial
                *		3 No Descontado
                * */

                db.AddInParameter(dbCommand, "@Criterio", DbType.Int16, criterio);
                db.AddInParameter(dbCommand, "@Opcion", DbType.Int16, opcion);
                db.AddInParameter(dbCommand, "@Prestador", DbType.Int64, idPrestador);
                db.AddInParameter(dbCommand, "@BenefCuil", DbType.Int64, benefCuil);
                db.AddInParameter(dbCommand, "@TipoConc", DbType.Int16, tipoConc);
                db.AddInParameter(dbCommand, "@Conc", DbType.Int32, concOpp);
                db.AddInParameter(dbCommand, "@FechaCierre", DbType.String, fecCierre);

                using (NullableDataReader dr = new NullableDataReader(db.ExecuteReader(dbCommand)))
                {
                    while (dr.Read())
                    {
                        //Int64.Parse(dr["IdNovedad"].ToString()),
                        //Int64.Parse(dr["IdBeneficiario"].ToString()),
                        //dr["ApellidoNombre"].ToString(),
                        //dr["Cuil"].Equals(DBNull.Value) ? "" : dr["Cuil"].ToString(),
                        //DateTime.Parse(dr["FecNov"].ToString()),
                        //dr["FecImportacion"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecImportacion"].ToString()),
                        
                        //byte.Parse(dr["TipoConcepto"].ToString()),
                        //dr["DescTipoConcepto"].ToString(),
                        
                        //int.Parse(dr["CodConceptoLiq"].ToString()),
                        //dr["DescConceptoLiq"].ToString(),

                        //byte.Parse(dr["CantCuotas"].ToString()),
                        //dr["NroCuotaLiq"].Equals(DBNull.Value) ? 0 : byte.Parse(dr["NroCuotaLiq"].ToString()),
                        //int.Parse(dr["PeriodoLiq"].ToString()),
                        //decimal.Parse(dr["ImporteTotal"].ToString()),
                        //decimal.Parse(dr["Porcentaje"].ToString()),
                        //dr["ImporteCuota"].Equals(DBNull.Value) ? 0 : decimal.Parse(dr["ImporteCuota"].ToString()),
                        //dr["ImporteALiq"].Equals(DBNull.Value) ? 0 : decimal.Parse(dr["ImporteALiq"].ToString()),
                        //dr["ImporteLiq"].Equals(DBNull.Value) ? 0 : decimal.Parse(dr["ImporteLiq"].ToString()),
                        //dr["NroComprobante"].Equals(DBNull.Value) ? "" : dr["NroComprobante"].ToString(),
                        //dr["MAC"].ToString(),
                        //dr["Usuario"].ToString(),
                        //dr["Stock"].ToString(),

                        lstNovedades.Add(new Novedad(Int64.Parse(dr["IdNovedad"].ToString()),
                                         dr["FecNov"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecNov"].ToString()),
                                         dr["FecImportacion"].Equals(DBNull.Value) ? new DateTime() : DateTime.Parse(dr["FecImportacion"].ToString()),
                                         dr["ImporteCuota"].Equals(DBNull.Value) ? 0 : double.Parse(dr["ImporteCuota"].ToString()),
                                         double.Parse(dr["ImporteTotal"].ToString()),
                                         dr["ImporteALiq"].Equals(DBNull.Value) ? 0 : double.Parse(dr["ImporteALiq"].ToString()),
                                         dr["ImporteLiq"].Equals(DBNull.Value) ? 0 : double.Parse(dr["ImporteLiq"].ToString()),
                                         byte.Parse(dr["CantCuotas"].ToString()),
                                         float.Parse(dr["Porcentaje"].ToString()),
                                         dr["NroComprobante"].Equals(DBNull.Value) ? "" : dr["NroComprobante"].ToString(),
                                         dr["PeriodoLiq"].ToString(), 
                                         string.Empty,0,
                                         dr["MAC"].ToString(),
                                         bool.Parse(dr["Stock"].ToString()),
                                         0, 0,
                                         dr["NroCuotaLiq"].Equals(DBNull.Value) ? (byte)0 : byte.Parse(dr["NroCuotaLiq"].ToString()),
                                         new Beneficiario(long.Parse(dr["IdBeneficiario"].ToString()),
                                                          long.Parse(dr["Cuil"].Equals(DBNull.Value) ? "0" : dr["Cuil"].ToString()),
                                                          dr.GetString("ApellidoNombre")),
                                         new Prestador(),
                                         new Estado(),
                                         new CodigoMovimiento(),
                                         new ConceptoLiquidacion(int.Parse(dr["CodConceptoLiq"].ToString()),
                                                                           dr.GetString("DescConceptoLiq")),
                                         new TipoConcepto(short.Parse(dr["TipoConcepto"].ToString()),
                                                                      dr.GetString("DescTipoConcepto"), true),
                                         new ModeloPC(),
                                         new Auditoria(dr["Usuario"].ToString()))
                            );
                    }
                }

                return lstNovedades;
            }
            catch (SqlException ErrSQL)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ErrSQL.Source, ErrSQL.Message));
               
                if (ErrSQL.Number == 1205 || ErrSQL.Number == 1204)
                {
                    throw new ApplicationException("Interbloqueo");
                }
                else
                {
                    throw ErrSQL;
                }
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }            
        }
		#endregion
		
		#region NovedadHistorica Trae

        public static List<Novedad> NovedadHistorica_Trae(byte opcionBusqueda, byte opcion, long prestador,
                                                          long benefCuil, byte tipoConc, int conCopp, string fecCierre,
                                                          bool generaArchivo, bool generadoAdmin, out string rutaArchivoSal)
        {
            string rutaArchivo = string.Empty;
            string nombreArchivo = string.Empty;
            rutaArchivoSal = string.Empty;
            string msgRta = string.Empty;
 
            ConsultaBatch consultaBatch = new ConsultaBatch();
            consultaBatch.NombreConsulta = ConsultaBatch.enum_ConsultaBatch_NombreConsulta. NOVEDADESLIQUIDADAS;
            consultaBatch.IDPrestador = prestador;
            consultaBatch.CriterioBusqueda = opcionBusqueda;
            consultaBatch.OpcionBusqueda = opcion;
            consultaBatch.PeriodoCons = fecCierre.ToString();         
            consultaBatch.UnConceptoLiquidacion = new ConceptoLiquidacion(conCopp, string.Empty, new TipoConcepto(tipoConc, string.Empty));
            consultaBatch.NroBeneficio = benefCuil;
            consultaBatch.GeneradoAdmin = generadoAdmin;
            consultaBatch.FechaDesde = consultaBatch.FechaHasta = null;

            try
            {
                if (opcion != 1 || generaArchivo == true)
                {
                    msgRta = ConsultasBatchDAO.ExisteConsulta(consultaBatch);
                    if (!string.IsNullOrEmpty(msgRta))
                    {
                        throw new ApplicationException("MSG_ERROR" + msgRta + "FIN_MSG_ERROR");
                    }
                }

                List<Novedad> listNovedades = NovedadHistorica_Trae_Consulta(opcionBusqueda, opcion, prestador, 
                                                                             benefCuil, tipoConc, conCopp, fecCierre);
                
                //if ((ds.Tables[0].Rows.Count != 0) && (opcion != 1 || GeneraArchivo == true))
                if (listNovedades.Count > 0 && (opcion != 1 || generaArchivo == true))
                {
                    int maxCantidad = Settings.MaxCantidadRegistros();

                    if (listNovedades.Count >= maxCantidad || generaArchivo == true)
                    {
                        nombreArchivo = Utilidades.GeneraNombreArchivo(consultaBatch.NombreConsulta.ToString(), prestador, out rutaArchivo);
                        rutaArchivoSal = Path.Combine(rutaArchivo, nombreArchivo);

                        StreamWriter sw = new StreamWriter(rutaArchivoSal, false, System.Text.Encoding.UTF8);
                        string separador = Settings.DelimitadorCampo();
                        
                        foreach (Novedad oNovedad in listNovedades)
                        {
                            StringBuilder linea = new StringBuilder();

                            linea.Append(oNovedad.IdNovedad.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.IdBeneficiario.ToString() + separador);
                            linea.Append(oNovedad.UnBeneficiario.ApellidoNombre.ToString() + separador);
                            linea.Append(oNovedad.FechaNovedad.ToString("dd/MM/yyyy HH:mm:ss") + separador);
                            linea.Append(oNovedad.UnTipoConcepto.IdTipoConcepto.ToString() + separador);
                            linea.Append(oNovedad.UnTipoConcepto.DescTipoConcepto.ToString() + separador);
                            linea.Append(oNovedad.UnConceptoLiquidacion.CodConceptoLiq.ToString() + separador);
                            linea.Append(oNovedad.UnConceptoLiquidacion.DescConceptoLiq.ToString() + separador);
                            linea.Append(oNovedad.ImporteTotal.ToString().Replace(",", ".") + separador);
                            linea.Append(oNovedad.CantidadCuotas.ToString() + separador);
                            linea.Append(oNovedad.Porcentaje.ToString().Replace(",", ".") + separador);
                            linea.Append(oNovedad.ImporteCuota.ToString().Replace(",", ".") + separador);
                            linea.Append(oNovedad.NroCuotaLiquidada.ToString() + separador);
                            linea.Append(oNovedad.ImporteALiquidar.ToString() + separador);                            
                            linea.Append(oNovedad.ImporteLiquidado.ToString() + separador);
                            linea.Append(oNovedad.Comprobante.ToString() + separador);
                            linea.Append(oNovedad.MAC.ToString() + separador);
                            linea.Append(oNovedad.UnAuditoria.Usuario.ToString());
                            linea.Append(oNovedad.Stock.ToString());

                            sw.WriteLine(linea.ToString());
                        }
                        sw.Close();

                        Utilidades.ComprimirArchivo(rutaArchivo, nombreArchivo);
                        Utilidades.BorrarArchivo(rutaArchivoSal);
                        nombreArchivo = nombreArchivo + ".zip";
                        consultaBatch.RutaArchGenerado = rutaArchivo;
                        consultaBatch.NomArchGenerado = nombreArchivo;
                        consultaBatch.FechaGenera = DateTime.Now;
                        consultaBatch.Vigente = true;

                        msgRta = ConsultasBatchDAO.AltaNuevaConsulta(consultaBatch);

                        if (!string.IsNullOrEmpty(msgRta))
                        {
                            msgRta = "MSG_ERROR" + msgRta + "FIN_MSG_ERROR";
                            throw new ApplicationException(msgRta);
                        }

                        /* Se instacia el objeto para que no muestre los 
                        * registros y pueda ver solo el archivo generado. */
                        listNovedades = new List<Novedad>(); 
                    }
                }

                return listNovedades;
            }
            catch (SqlException errsql)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), errsql.Source, errsql.Message));
 
                if (errsql.Number == -2)
                {
                    nombreArchivo = Utilidades.GeneraNombreArchivo(consultaBatch.NombreConsulta.ToString(), prestador, out rutaArchivo);
                    consultaBatch.NomArchGenerado = nombreArchivo;
                    consultaBatch.RutaArchGenerado = rutaArchivo;
                    consultaBatch.FechaGenera = DateTime.MinValue;
                    consultaBatch.Vigente = false;

                    msgRta = ConsultasBatchDAO.AltaNuevaConsulta(consultaBatch);

                    throw new ApplicationException("MSG_ERROR Generando el archivo. Reingrese a la consulta en unos minutos.FIN_MSG_ERROR");
                }
                else
                {
                    throw errsql;
                }
            }
            catch (ApplicationException apperr)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), apperr.Source, apperr.Message));
                throw new ApplicationException(apperr.Message);
            }
            catch (Exception err)
            {
                log.Error(string.Format("{0}->{1}->Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), err.Source, err.Message));
                throw err;
            }
            finally
            {
            }
        }

		#endregion				
	
	}	
}