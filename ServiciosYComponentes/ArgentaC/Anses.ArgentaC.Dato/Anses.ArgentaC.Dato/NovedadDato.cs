using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Anses.ArgentaC.Contrato;
using Anses.ArgentaC.Comun.Domain;

namespace Anses.ArgentaC.Dato
{
    public static class NovedadDato
    {
        public static Persona ObtenerNovedades(Persona _persona, enum_TipoEstadoNovedad estado)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter outparam_CodError = cmd.Parameters.Add("@CodError", SqlDbType.Int);
            outparam_CodError.Direction = ParameterDirection.Output;
            SqlParameter outparam_MsgResultado = cmd.Parameters.Add("@MsgResultado", SqlDbType.NVarChar, 4000);
            outparam_MsgResultado.Direction = ParameterDirection.Output;
            try
            {
                List<Novedad> lstNovedades = new List<Novedad>();
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "ObtenerNovedades ";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@cuil", (_persona == null) ? 0 : _persona.Cuil));
                //no existe estado indeterminado, el SP recibe null
                if (estado == enum_TipoEstadoNovedad.Indeterminado)
                    cmd.Parameters.Add(new SqlParameter("@idEstadoNovedad ", null));
                else
                    cmd.Parameters.Add(new SqlParameter("@idEstadoNovedad ", (int)estado));
                cmd.Connection = oCnn;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lstNovedades.Add(obtenerEntidad(dr));
                    }
                }
                List<Novedad> listaAgrupadaPorID = new List<Novedad>();
                listaAgrupadaPorID = lstNovedades.GroupBy(n => n.IdNovedad).Select(g => g.First()).ToList();
                int CodError = Convert.ToInt16(cmd.Parameters["@CodError"].Value);
                string MsgResultado = Convert.ToString(cmd.Parameters["@MsgResultado"].Value);
                _persona.Errores.Add(new Mensaje(CodError, MsgResultado));
                _persona.Novedades = listaAgrupadaPorID;
                return _persona;
            }
            catch (Exception err)

            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                cmd.Dispose();
            }
        }

        public static Novedad CambiarEstado(Novedad _novedad, enum_TipoEstadoNovedad _idEstadoDestino, long _idProducto, decimal _montoSolicitado)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter outparam_CodError = cmd.Parameters.Add("@CodError", SqlDbType.Int);
            outparam_CodError.Direction = ParameterDirection.Output;
            SqlParameter outparam_MsgResultado = cmd.Parameters.Add("@MsgResultado", SqlDbType.NVarChar, 4000);
            outparam_MsgResultado.Direction = ParameterDirection.Output;
            string idProd = (_idProducto == 0) ? null : _idProducto.ToString();
            string montoS = (_montoSolicitado == 0) ? null : _montoSolicitado.ToString();
            try
            {
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "NovedadCambiarEstado";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdNovedad", _novedad.IdNovedad));
                cmd.Parameters.Add(new SqlParameter("@idEstadoNovedadDestino", (int)_idEstadoDestino));
                cmd.Parameters.Add(new SqlParameter("@idProducto", idProd));
                cmd.Parameters.Add(new SqlParameter("@MontoSolicitado", montoS));
                cmd.Parameters.Add(new SqlParameter("@Usuario", Utils.GetUsuario().UsuarioDesc.ToString()));
                cmd.Parameters.Add(new SqlParameter("@Oficina", Utils.GetUsuario().Oficina));
                cmd.Parameters.Add(new SqlParameter("@IP", Utils.GetUsuario().Ip.ToString()));
                cmd.Parameters.Add(new SqlParameter("@ImposibilidadFirma", _novedad.ImposibilidadFirma));
                cmd.Connection = oCnn;
                cmd.ExecuteNonQuery();
                int CodError = Convert.ToInt16(cmd.Parameters["@CodError"].Value);
                string MsgResultado = Convert.ToString(cmd.Parameters["@MsgResultado"].Value);
                int IdNovedad = Convert.ToInt32(cmd.Parameters["@IdNovedad"].Value);
                //analizar los mensajes de error para saber qué devolvemos de la función
                _novedad.Mensaje = new Mensaje(CodError, MsgResultado);
                _novedad.IdProducto = _idProducto;
                _novedad.ImporteTotal = _montoSolicitado;
                _novedad.Estado = (enum_TipoEstadoNovedad)_idEstadoDestino;
                return _novedad;
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                cmd.Dispose();
            }
        }

        public static NovedadesPendientesDeAprobacionAgrupada Novedades_PendientesAprobacion_Agrupadas(string _oficina, DateTime? _fechaDesde, DateTime? _fechaHasta, out Int32 _total)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            NovedadesPendientesDeAprobacionAgrupada NovedadesSinAprobar = new NovedadesPendientesDeAprobacionAgrupada();
            _total = 0;

            SqlParameter outparam_Total = cmd.Parameters.Add("@total", SqlDbType.Int);
            outparam_Total.Direction = ParameterDirection.Output;

            try
            {
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "Novedades_PendientesAprobacion_Agrupadas";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@oficina", (string.IsNullOrEmpty(_oficina) ? null : _oficina)));
                cmd.Parameters.Add(new SqlParameter("@fechaDesde", _fechaDesde));
                cmd.Parameters.Add(new SqlParameter("@fechaHasta", _fechaHasta));
                cmd.Connection = oCnn;

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        NovedadesSinAprobar = ObtenerNovedadesPendientesDeAprobacion(dr);
                    }
                }

                _total = Convert.ToInt16(cmd.Parameters["@total"].Value);
                return NovedadesSinAprobar;
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                cmd.Dispose();
            }
        }

        public static List<DatosDeConsultaDeNovedad> NovedadesConsulta(int? idNovedad, long? cuil, int? estado, DateTime? desde, DateTime? hasta)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                List<DatosDeConsultaDeNovedad> lstNovedades = new List<DatosDeConsultaDeNovedad>();
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "NovedadesConsulta";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@idNovedad", (idNovedad == 0) ? null : idNovedad));
                cmd.Parameters.Add(new SqlParameter("@cuil", (cuil == 0) ? null : cuil));
                cmd.Parameters.Add(new SqlParameter("@idEstadoNovedad", (estado == 0) ? null : estado));
                cmd.Parameters.Add(new SqlParameter("@desde", (desde == DateTime.MinValue) ? null : desde));
                cmd.Parameters.Add(new SqlParameter("@hasta", (hasta == DateTime.MinValue) ? null : hasta));
                if (cuil != 0 || idNovedad != 0)
                    cmd.Parameters.Add(new SqlParameter("@oficina", null));
                else
                    cmd.Parameters.Add(new SqlParameter("@oficina", Utils.GetUsuario().Oficina));
                cmd.Connection = oCnn;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lstNovedades.Add(obtenerEntidadConsulta(dr));
                    }
                }
                return lstNovedades;
            }
            catch (Exception err)

            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                cmd.Dispose();
            }
        }

        private static NovedadesPendientesDeAprobacionAgrupada ObtenerNovedadesPendientesDeAprobacion(SqlDataReader dataReader)
        {
            var cantidadSinAprobar = (Int32)dataReader["cantidadSinAprobar"];

            if (cantidadSinAprobar > 0)
            {
                var minimaFecNovedad = (DateTime)dataReader["minimaFecNovedad"];
                var maxFecNovedad = (DateTime)dataReader["maxFecNovedad"];
                return new NovedadesPendientesDeAprobacionAgrupada(cantidadSinAprobar, minimaFecNovedad, maxFecNovedad);
            }
            else
            {
                return new NovedadesPendientesDeAprobacionAgrupada();
            }

        }

        public static Novedad[] ObtenerNominaNovedades(long? cuil, enum_TipoEstadoNovedad? estado, int? oficina)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter outparam_CodError = cmd.Parameters.Add("@CodError", SqlDbType.Int);
            outparam_CodError.Direction = ParameterDirection.Output;
            SqlParameter outparam_MsgResultado = cmd.Parameters.Add("@MsgResultado", SqlDbType.NVarChar, 4000);
            outparam_MsgResultado.Direction = ParameterDirection.Output;
            try
            {
                List<Novedad> lstNovedades = new List<Novedad>();
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "ObtenerNominaNovedades ";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@cuil", (cuil == null) ? null : cuil));
                cmd.Parameters.Add(new SqlParameter("@idEstadoNovedad", (int?)estado));
                cmd.Parameters.Add(new SqlParameter("@Oficina", oficina));
                cmd.Connection = oCnn;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lstNovedades.Add(obtenerEntidadAprobacion(dr));
                    }
                }
                int CodError = Convert.ToInt16(cmd.Parameters["@CodError"].Value);
                string MsgResultado = Convert.ToString(cmd.Parameters["@MsgResultado"].Value);
                return lstNovedades.ToArray();
            }
            catch (Exception err)

            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                cmd.Dispose();
            }
        }

        public static bool ConfirmarCreditosGenerados(List<Novedad> listaNovedades)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter outparam_CodError = cmd.Parameters.Add("@CodError", SqlDbType.Int);
            outparam_CodError.Direction = ParameterDirection.Output;
            SqlParameter outparam_MsgResultado = cmd.Parameters.Add("@MsgResultado", SqlDbType.NVarChar, 4000);
            outparam_MsgResultado.Direction = ParameterDirection.Output;
            try
            {
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "GenerarCreditoDesdeAcuerdo ";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@idNovedad", null));
                cmd.Parameters.Add(new SqlParameter("@Usuario", Utils.GetUsuario().UsuarioDesc));
                cmd.Parameters.Add(new SqlParameter("@Oficina", (Utils.GetUsuario().Oficina)));
                cmd.Parameters.Add(new SqlParameter("@IP", Utils.GetUsuario().Ip));
                cmd.Parameters.Add(new SqlParameter("@xml", Serialize(listaNovedades)));
                cmd.Connection = oCnn;
                cmd.ExecuteNonQuery();
                int CodError = Convert.ToInt16(cmd.Parameters["@CodError"].Value);
                string MsgResultado = Convert.ToString(cmd.Parameters["@MsgResultado"].Value);
                if (CodError > 0)
                    return false;
                else
                    return true;
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                cmd.Dispose();
            }
        }

        private static Novedad obtenerEntidad(SqlDataReader dr)
        {
            return new Novedad(long.Parse(string.IsNullOrEmpty(dr["IdNovedad"].ToString()) ? "0" : dr["IdNovedad"].ToString()),
                                long.Parse(string.IsNullOrEmpty(dr["cuil"].ToString()) ? "0" : dr["cuil"].ToString()),
                                dr["Nombre"].ToString(),
                                long.Parse(string.IsNullOrEmpty(dr["idProducto"].ToString()) ? "0" : dr["idProducto"].ToString()),
                                decimal.Parse(string.IsNullOrEmpty(dr["ImporteTotal"].ToString()) ? "0" : dr["ImporteTotal"].ToString()),
                                Int32.Parse(string.IsNullOrEmpty(dr["CantCuotas"].ToString()) ? "0" : dr["CantCuotas"].ToString()),
                                decimal.Parse(string.IsNullOrEmpty(dr["Importe"].ToString()) ? "0" : dr["Importe"].ToString()),
                                string.IsNullOrEmpty(dr["idEstadoNovedad"].ToString()) ? enum_TipoEstadoNovedad.Indeterminado : (enum_TipoEstadoNovedad)Convert.ToInt16(dr["idEstadoNovedad"]),
                                DateTime.Parse(dr["FecNovedad"].ToString()),
                                dr["CodConceptoLiq"].ToString()
                                );
        }

        private static Novedad obtenerEntidadAprobacion(SqlDataReader dr)
        {
            return new Novedad(long.Parse(string.IsNullOrEmpty(dr["IdNovedad"].ToString()) ? "0" : dr["IdNovedad"].ToString()),
                                long.Parse(string.IsNullOrEmpty(dr["cuil"].ToString()) ? "0" : dr["cuil"].ToString()),
                                dr["Nombre"].ToString(),
                                decimal.Parse(string.IsNullOrEmpty(dr["ImporteTotal"].ToString()) ? "0" : dr["ImporteTotal"].ToString()),
                                Int32.Parse(string.IsNullOrEmpty(dr["CantCuotas"].ToString()) ? "0" : dr["CantCuotas"].ToString()),
                                string.IsNullOrEmpty(dr["idEstadoNovedad"].ToString()) ? enum_TipoEstadoNovedad.Indeterminado : (enum_TipoEstadoNovedad)Convert.ToInt16(dr["idEstadoNovedad"]),
                                DateTime.Parse(dr["FecNovedad"].ToString()),
                                dr["CodConceptoLiq"].ToString()
                                );
        }

        private static DatosDeConsultaDeNovedad obtenerEntidadConsulta(SqlDataReader dr)
        {
            return new DatosDeConsultaDeNovedad(
                long.Parse(string.IsNullOrEmpty(dr["IdNovedad"].ToString()) ? "0" : dr["IdNovedad"].ToString()),
                long.Parse(string.IsNullOrEmpty(dr["cuilTomador"].ToString()) ? "0" : dr["cuilTomador"].ToString()),
                DateTime.Parse(dr["FecNovedad"].ToString()),
                dr.IsDBNull(dr.GetOrdinal("FecAprobacion")) ? DateTime.MinValue : DateTime.Parse(dr["FecAprobacion"].ToString()),
                decimal.Parse(string.IsNullOrEmpty(dr["ImporteTotal"].ToString()) ? "0" : dr["ImporteTotal"].ToString()),
                string.IsNullOrEmpty(dr["PrimerMensual"].ToString()) ? "" : dr["PrimerMensual"].ToString(),
                new Tipo(dr["idEstadoNovedad"].ToString(), dr["Descripcion"].ToString()),
                Int32.Parse(string.IsNullOrEmpty(dr["idProducto"].ToString()) ? "0" : dr["idProducto"].ToString()),
                string.IsNullOrEmpty(dr["OFICINA_CARGA"].ToString()) ? "" : dr["OFICINA_CARGA"].ToString(),
                dr.IsDBNull(dr.GetOrdinal("NombreyApellido")) ? "" : dr["NombreyApellido"].ToString(),
                dr.IsDBNull(dr.GetOrdinal("CBU")) ? "" : dr["CBU"].ToString(),
                dr.IsDBNull(dr.GetOrdinal("Banco")) ? "" : dr["Banco"].ToString(),
                dr.IsDBNull(dr.GetOrdinal("Agencia")) ? "" : dr["Agencia"].ToString(),
                Int32.Parse(string.IsNullOrEmpty(dr["CantCuotas"].ToString()) ? "0" : dr["CantCuotas"].ToString()),
                string.IsNullOrEmpty(dr["UsuarioAlta"].ToString()) ? "" : dr["UsuarioAlta"].ToString(),
                string.IsNullOrEmpty(dr["UsuarioSupervision"].ToString()) ? "" : dr["UsuarioSupervision"].ToString(),
                dr.IsDBNull(dr.GetOrdinal("FechaUltimaModificacion")) ? DateTime.MinValue : DateTime.Parse(dr["FechaUltimaModificacion"].ToString()),
                string.IsNullOrEmpty(dr["CodConceptoLiq"].ToString()) ? "" : dr["CodConceptoLiq"].ToString()
                );
        }

        public static List<Tipo> ObtenerSistemasHabilitados()
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                List<Tipo> lstSistemas = new List<Tipo>();
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "SistemasHabilitadosObtener";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = oCnn;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lstSistemas.Add(obtenerEntidadSistema(dr));
                    }
                }
                return lstSistemas;
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                cmd.Dispose();
            }
        }

        private static Tipo obtenerEntidadSistema(SqlDataReader dr)
        {
            return new Tipo(
                                dr["idSistema"].ToString(),
                                dr["Descripcion"].ToString()
                                );
        }

        public static string Serialize(object obj)
        {
            try
            {
                String stringXML = null;
                MemoryStream oMemoryStream = new MemoryStream();

                XmlSerializer oXmlSerializer = new XmlSerializer(obj.GetType());
                XmlTextWriter oXmlTextWriter = new XmlTextWriter(oMemoryStream, Encoding.Unicode);

                XmlSerializerNamespaces oXmlSerializerNamespaces = new XmlSerializerNamespaces();
                oXmlSerializerNamespaces.Add(string.Empty, string.Empty);

                oXmlSerializer.Serialize(oXmlTextWriter, obj, oXmlSerializerNamespaces);

                oMemoryStream = (MemoryStream)oXmlTextWriter.BaseStream;

                UnicodeEncoding oUnicode = new UnicodeEncoding();
                stringXML = oUnicode.GetString(oMemoryStream.ToArray());

                return stringXML;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region Bajas y suspensiones
        public static List<enum_TipoEstadoNovedad> MotivoBaja_traer()
        {
            List<enum_TipoEstadoNovedad> oSalida = new List<enum_TipoEstadoNovedad>();

            SqlConnection oCnn = Conexion.ObtenerConnexionSQL();
            SqlCommand cmd = new SqlCommand("EstadosNovedad_Traer", oCnn);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@idEstadoNovedadDestino", null));
            cmd.Parameters.Add(new SqlParameter("@idPropositoEstados", 1));

            oCnn.Open();
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    oSalida.Add((enum_TipoEstadoNovedad)Convert.ToInt16(dr["idEstadoNovedad"]));
                }
            }

            if (oCnn.State != ConnectionState.Closed)
            {
                oCnn.Close();
            }
            oCnn.Dispose();
            cmd.Dispose();

            return oSalida;
        }

        public static ONovedadBSRPre[] ObtenerNovedadesParaBaja(long? cuil, int? idNovedad)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            //SqlParameter outparam_CodError = cmd.Parameters.Add("@CodError", SqlDbType.Int);
            //outparam_CodError.Direction = ParameterDirection.Output;
            SqlParameter outparam_MsgResultado = cmd.Parameters.Add("@MsgResultado", SqlDbType.NVarChar, 4000);
            outparam_MsgResultado.Direction = ParameterDirection.Output;
            try
            {
                List<ONovedadBSRPre> lstNovedades = new List<ONovedadBSRPre>();
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "ObtenerNovedadesParaBajaSuspension";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@cuil", (cuil == null) ? null : cuil));
                cmd.Parameters.Add(new SqlParameter("@idNovedad", (idNovedad == null) ? null : idNovedad));


                cmd.Connection = oCnn;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lstNovedades.Add(obtenerNovedadBSRPre(dr));
                    }
                }
                //int CodError = Convert.ToInt16(cmd.Parameters["@CodError"].Value);
                string MsgResultado = Convert.ToString(cmd.Parameters["@MsgResultado"].Value);
                return lstNovedades.ToArray();
            }
            catch (Exception err)

            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                cmd.Dispose();
            }
        }

        public static ONovedadBSRPre[] ObtenerNovedadesParaSuspender(long? cuil, int? idNovedad)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            //SqlParameter outparam_CodError = cmd.Parameters.Add("@CodError", SqlDbType.Int);
            //outparam_CodError.Direction = ParameterDirection.Output;
            SqlParameter outparam_MsgResultado = cmd.Parameters.Add("@MsgResultado", SqlDbType.NVarChar, 4000);
            outparam_MsgResultado.Direction = ParameterDirection.Output;
            try
            {
                List<ONovedadBSRPre> lstNovedades = new List<ONovedadBSRPre>();
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "ObtenerNovedadesParaBajaSuspension";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@cuil", (cuil == null) ? null : cuil));
                cmd.Parameters.Add(new SqlParameter("@idNovedad", (idNovedad == null) ? null : idNovedad));


                cmd.Connection = oCnn;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lstNovedades.Add(obtenerNovedadBSRPre(dr));
                    }
                }
                //int CodError = Convert.ToInt16(cmd.Parameters["@CodError"].Value);
                string MsgResultado = Convert.ToString(cmd.Parameters["@MsgResultado"].Value);
                return lstNovedades.ToArray();
            }
            catch (Exception err)

            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                cmd.Dispose();
            }
        }

        public static ONovedadBSRPre[] ObtenerNovedadesParaRehabilitar(long? cuil, int? idNovedad)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            //SqlParameter outparam_CodError = cmd.Parameters.Add("@CodError", SqlDbType.Int);
            //outparam_CodError.Direction = ParameterDirection.Output;
            SqlParameter outparam_MsgResultado = cmd.Parameters.Add("@MsgResultado", SqlDbType.NVarChar, 4000);
            outparam_MsgResultado.Direction = ParameterDirection.Output;
            try
            {
                List<ONovedadBSRPre> lstNovedades = new List<ONovedadBSRPre>();
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "ObtenerNovedadesParaBajaSuspension";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@cuil", (cuil == null) ? null : cuil));
                cmd.Parameters.Add(new SqlParameter("@idNovedad", (idNovedad == null) ? null : idNovedad));


                cmd.Connection = oCnn;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lstNovedades.Add(obtenerNovedadBSRPre(dr));
                    }
                }
                //int CodError = Convert.ToInt16(cmd.Parameters["@CodError"].Value);
                string MsgResultado = Convert.ToString(cmd.Parameters["@MsgResultado"].Value);
                return lstNovedades.ToArray();
            }
            catch (Exception err)

            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                cmd.Dispose();
            }
        }

        private static ONovedadBSRPre obtenerNovedadBSRPre(SqlDataReader dr)
        {
            return new ONovedadBSRPre(int.Parse(string.IsNullOrEmpty(dr["IdNovedad"].ToString()) ? "0" : dr["IdNovedad"].ToString()),
                DateTime.Parse(dr["FecAprobacion"].ToString()),
                long.Parse(string.IsNullOrEmpty(dr["cuilTomador"].ToString())? "0" : dr["cuilTomador"].ToString()),
                dr["NombreyApellido"].ToString(),
                dr["CodigoDescuento"].ToString(),
                Int32.Parse(string.IsNullOrEmpty(dr["idEstadoNovedad"].ToString()) ? "0" : dr["idEstadoNovedad"].ToString()),
                dr["EstadoNovedad"].ToString(),
                decimal.Parse(string.IsNullOrEmpty(dr["ImporteTotal"].ToString()) ? "0" : dr["ImporteTotal"].ToString()),
                Int32.Parse(string.IsNullOrEmpty(dr["CantidadCuotas"].ToString()) ? "0" : dr["CantidadCuotas"].ToString()),
                decimal.Parse(string.IsNullOrEmpty(dr["MontoPrestamo"].ToString()) ? "0" : dr["MontoPrestamo"].ToString())
            );
        }

        private static Cuota_Baja_Suspension obtenerCuotasBajaSuspension(SqlDataReader dr)
        {
            return new Cuota_Baja_Suspension(
                (int)dr["idCuotas"],
                (int)dr["idNovedad"],
                (int)dr["IdNovedadDetalle"],
                (long)dr["CuilRelacionado"],
                string.IsNullOrEmpty(dr["EstadoCuota"].ToString()) ? "" : dr["EstadoCuota"].ToString(),
                (byte)dr["NroCuota"],
                (int)dr["Mensual"],
                decimal.Parse(string.IsNullOrEmpty(dr["montoCuotaTotal"].ToString()) ? "0" : dr["montoCuotaTotal"].ToString())
            );
        }

        private static ONovedadHistoEstados obtenerHistoricosEstadosNovedad(SqlDataReader dr)
        {
            return new ONovedadHistoEstados(
                (int)dr["Tramite"],
                (int)dr["idEstadoNovedad"],
                dr["DescEstado"].ToString(),
                (long)dr["CuilTomador"],
                (int)dr["NroCredito"],
                (short)dr["CuotasPendiente"],
                (short)dr["CuotasPaga"],
                (short)dr["CuotasImpaga"],
                (short)dr["CuotasEnviadaLiq"],
                (DateTime?)dr["Fecha"] == null ? "" : dr["Fecha"].ToString(),
                string.IsNullOrEmpty(dr["expediente"].ToString()) ? "" : dr["expediente"].ToString(),
                (int)dr["Mensual"], 
                string.IsNullOrEmpty(dr["Motivo"].ToString()) ? "" : dr["Motivo"].ToString(),
                string.IsNullOrEmpty(dr["UsuarioGeneral"].ToString()) ? "" : dr["UsuarioGeneral"].ToString(),
                string.IsNullOrEmpty(dr["OficinaGeneral"].ToString()) ? "" : dr["OficinaGeneral"].ToString(),
                string.IsNullOrEmpty(dr["IP"].ToString()) ? "" : dr["IP"].ToString()
            );
        }

        private static ONovedadBSRPost obtenerNovedadBSRPost(SqlDataReader dr)
        {
            return new Contrato.ONovedadBSRPost(
                (int)dr["idNovedadBaja"],
                (int)dr["idNovedad"],
                (int)dr["idEstadoNovedad"],
                (string)dr["DEstadoNovedad"],
                (long)dr["CuilTomador"],
                string.IsNullOrEmpty(dr["NombreyApellido"].ToString()) ? "" : dr["NombreyApellido"].ToString(),
                (short)dr["CuotasPendiente"],
                (short)dr["CuotasPaga"],
                (short)dr["CuotasImpaga"],
                (short)dr["CuotasEnviadaLiq"],

                (DateTime?)dr["FechaCancelacion"] == null ? "" : dr["FechaCancelacion"].ToString(),
                string.IsNullOrEmpty(dr["usuarioBaja"].ToString()) ? "" : dr["usuarioBaja"].ToString(),
                string.IsNullOrEmpty(dr["oficinaBaja"].ToString()) ? "" : dr["oficinaBaja"].ToString()
            );
            
        }

        public static bool NovedadCambiarEstado(INovedadBSR iParams, out int codError, out string msgResultado)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter ioparam_idNovedad = new SqlParameter("@IdNovedad", SqlDbType.Int);
            ioparam_idNovedad.Value = iParams.idNovedad;
            ioparam_idNovedad.Direction = ParameterDirection.InputOutput;
            SqlParameter outparam_CodError = cmd.Parameters.Add("@CodError", SqlDbType.Int);
            outparam_CodError.Direction = ParameterDirection.Output;
            SqlParameter outparam_MsgResultado = cmd.Parameters.Add("@MsgResultado", SqlDbType.NVarChar, 4000);
            outparam_MsgResultado.Direction = ParameterDirection.Output;
            codError = 0;
            msgResultado = string.Empty;

            //StringBuilder armadoXml = new StringBuilder();
            //if (lcuotas != null)
            //{
            //    armadoXml.Append("<ArrayOfCuotas>");
            //    foreach (long i in lcuotas)
            //    {
            //        armadoXml.Append("<idCuota>");
            //        armadoXml.Append(i.ToString());
            //        armadoXml.Append("</idCuota>");
            //    }
            //    armadoXml.Append("</ArrayOfCuotas>");
            //}

            try
            {
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "NovedadCambiarEstado";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(ioparam_idNovedad);

                cmd.Parameters.Add(new SqlParameter("@idEstadoNovedadOrigen", (iParams.idEstadoOrigen == null) ? null : iParams.idEstadoOrigen));
                cmd.Parameters.Add(new SqlParameter("@idEstadoNovedadDestino", iParams.idEstadoDestino));
                cmd.Parameters.Add(new SqlParameter("@idProducto", null));
                cmd.Parameters.Add(new SqlParameter("@MontoSolicitado", iParams.Monto));
                cmd.Parameters.Add(new SqlParameter("@Usuario", (Utils.GetUsuario().UsuarioDesc)));
                cmd.Parameters.Add(new SqlParameter("@Oficina", (Utils.GetUsuario().Oficina)));
                cmd.Parameters.Add(new SqlParameter("@IP", Utils.GetUsuario().Ip));
                cmd.Parameters.Add(new SqlParameter("@ImposibilidadFirma", iParams.imposibilidadFirma));
                cmd.Parameters.Add(new SqlParameter("@xml", null));
                cmd.Parameters.Add(new SqlParameter("@expediente", string.IsNullOrEmpty(iParams.expediente) ? null : iParams.expediente));
                cmd.Parameters.Add(new SqlParameter("@motivo", string.IsNullOrEmpty(iParams.motivoSuspension) ? null : iParams.motivoSuspension));

                //cmd.Parameters.Add(new SqlParameter("@xml", (lcuotas==null) ? null : Serialize(lcuotas)));
                //cmd.Parameters.Add(new SqlParameter("@xml", (lcuotas == null) ? null : armadoXml.ToString()));

                cmd.Connection = oCnn;
                cmd.ExecuteNonQuery();
                codError = Convert.ToInt16(cmd.Parameters["@CodError"].Value);
                msgResultado = Convert.ToString(cmd.Parameters["@MsgResultado"].Value);
                if (codError > 0)
                    return false;
                else
                    return true;
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                cmd.Dispose();
            }
        }

        public static Cuota_Baja_Suspension[] ObtenerCuotasNovedadBaja(int idNovedad, int idEstadoNovedadMotivoDeBaja)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                List<Cuota_Baja_Suspension> lstCuotasBaja = new List<Cuota_Baja_Suspension>();
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "ObtenerCuotasNovedadBaja";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@idNovedad", idNovedad));
                cmd.Parameters.Add(new SqlParameter("@idEstadoNovedadMotivoDeBaja", idEstadoNovedadMotivoDeBaja));


                cmd.Connection = oCnn;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lstCuotasBaja.Add(obtenerCuotasBajaSuspension(dr));
                    }
                }
                return lstCuotasBaja.ToArray();
            }
            catch (Exception err)

            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                cmd.Dispose();
            }
        }

        public static ONovedadBSRPost ObtenerNovedadBSR(int idNovedad)
        {

            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            try
            {
                ONovedadBSRPost NBaja = null;
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "NovedadBajaComprobante_traer";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@IdNovedad", idNovedad));

                cmd.Connection = oCnn;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read() != false)
                    {
                        NBaja = obtenerNovedadBSRPost(dr);
                    }
                }

                return NBaja;
            }
            catch (Exception err)

            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                cmd.Dispose();
            }
        }

        public static List<ONovedadBSRPost> ObtenerNovedadesBSR()
        {

            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();

            List<ONovedadBSRPost> oResultado = new List<ONovedadBSRPost>();

            try
            {    
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "NovedadBajaComprobante_traer";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@IdNovedad", null));

                cmd.Connection = oCnn;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oResultado.Add(obtenerNovedadBSRPost(dr));
                    }
                }

                return oResultado;
            }
            catch (Exception err)

            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                cmd.Dispose();
            }
        }

        public static List<ONovedadHistoEstados> ObtenerNovedadHistoricoEstados(long idNovedad)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            
            try
            {
                List<ONovedadHistoEstados> lsthistoricos = new List<ONovedadHistoEstados>();

                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "ObtenerNovedadesHistoBS";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@IdNovedad", idNovedad));

                cmd.Connection = oCnn;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lsthistoricos.Add(obtenerHistoricosEstadosNovedad(dr));
                    }
                }
                return lsthistoricos;
            }
                
            catch (Exception err)

            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                cmd.Dispose();
            }
        }

        public static List<NovedadSuspension> ObtenerSuspensionesHabilitacionesDeNovedad(long idNovedad)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                List<NovedadSuspension> lstSuspensionesHabilitaciones = new List<NovedadSuspension>();
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "NovedadSuspension_Detalle";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@idNovedad", idNovedad));
                cmd.Connection = oCnn;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lstSuspensionesHabilitaciones.Add(obtenerEntidadSuspensionesHabilitaciones(dr));
                    }
                }
                return lstSuspensionesHabilitaciones;
            }
            catch (Exception err)

            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                cmd.Dispose();
            }
        }

        public static NovedadSuspension obtenerEntidadSuspensionesHabilitaciones(SqlDataReader dr)
        {
            return new NovedadSuspension(
                Int32.Parse(string.IsNullOrEmpty(dr["Orden"].ToString()) ? "0" : dr["Orden"].ToString()),
                Int32.Parse(string.IsNullOrEmpty(dr["idNovedad"].ToString()) ? "0" : dr["idNovedad"].ToString()),
                Int32.Parse(string.IsNullOrEmpty(dr["idNovedadSuspension"].ToString()) ? "0" : dr["idNovedadSuspension"].ToString()),
                Int32.Parse(string.IsNullOrEmpty(dr["idNovedadReactivacion"].ToString()) ? "0" : dr["idNovedadReactivacion"].ToString()),
                Int32.Parse(string.IsNullOrEmpty(dr["idEstadoNovedadSuspension"].ToString()) ? "0" : dr["idEstadoNovedadSuspension"].ToString()),
                dr["EstadoNovedadSuspensionDescripcion"].ToString(),
                long.Parse(string.IsNullOrEmpty(dr["cuilTomador"].ToString()) ? "0" : dr["cuilTomador"].ToString()),
                decimal.Parse(string.IsNullOrEmpty(dr["MontoPrestamo"].ToString()) ? "0" : dr["MontoPrestamo"].ToString()),
                Int32.Parse(string.IsNullOrEmpty(dr["idEstadoNovedadReactivacion"].ToString()) ? "0" : dr["idEstadoNovedadReactivacion"].ToString()),
                string.IsNullOrEmpty(dr["EstadoNovedadReactivacionDescripcion"].ToString()) ? "" : dr["EstadoNovedadReactivacionDescripcion"].ToString(),
                dr["Expediente"].ToString(),
                DateTime.Parse(dr["FechaSuspension"].ToString()),
                dr["MensualSuspension"].ToString(),
                dr["MotivoSuspension"].ToString(),
                dr["UsuarioSuspension"].ToString(),
                dr["OficinaSuspension"].ToString(),
                DateTime.Parse(string.IsNullOrEmpty(dr["FechaReactivacion"].ToString()) ? DateTime.MinValue.ToString() : dr["FechaReactivacion"].ToString()),
                string.IsNullOrEmpty(dr["MensualReactivacion"].ToString()) ? "" : dr["MensualReactivacion"].ToString(),
                string.IsNullOrEmpty(dr["MotivoReactivacion"].ToString()) ? "" : dr["MotivoReactivacion"].ToString(),
                string.IsNullOrEmpty(dr["UsuarioReactivacion"].ToString()) ? "" : dr["UsuarioReactivacion"].ToString(),
                string.IsNullOrEmpty(dr["OficinaReactivacion"].ToString()) ? "" : dr["OficinaReactivacion"].ToString());
        }

        public static NovedadSuspension obtenerEntidadSuspensionHabilitacion(SqlDataReader dr)
        {
            return new NovedadSuspension(
                Int32.Parse(string.IsNullOrEmpty(dr["idNovedad"].ToString()) ? "0" : dr["idNovedad"].ToString()),
                Int32.Parse(string.IsNullOrEmpty(dr["idNovedadSuspension"].ToString()) ? "0" : dr["idNovedadSuspension"].ToString()),
                Int32.Parse(string.IsNullOrEmpty(dr["idNovedadReactivacion"].ToString()) ? "0" : dr["idNovedadReactivacion"].ToString()),
                Int32.Parse(string.IsNullOrEmpty(dr["idEstadoNovedadSuspension"].ToString()) ? "0" : dr["idEstadoNovedadSuspension"].ToString()),
                dr["EstadoNovedadSuspensionDescripcion"].ToString(),
                long.Parse(string.IsNullOrEmpty(dr["cuilTomador"].ToString()) ? "0" : dr["cuilTomador"].ToString()),
                decimal.Parse(string.IsNullOrEmpty(dr["MontoPrestamo"].ToString()) ? "0" : dr["MontoPrestamo"].ToString()),
                Int32.Parse(string.IsNullOrEmpty(dr["idEstadoNovedadReactivacion"].ToString()) ? "0" : dr["idEstadoNovedadReactivacion"].ToString()),
                string.IsNullOrEmpty(dr["EstadoNovedadReactivacionDescripcion"].ToString()) ? "" : dr["EstadoNovedadReactivacionDescripcion"].ToString(),
                dr["Expediente"].ToString(),
                DateTime.Parse(dr["FechaSuspension"].ToString()),
                dr["MensualSuspension"].ToString(),
                dr["MotivoSuspension"].ToString(),
                dr["UsuarioSuspension"].ToString(),
                dr["OficinaSuspension"].ToString(),
                DateTime.Parse(string.IsNullOrEmpty(dr["FechaReactivacion"].ToString()) ? DateTime.MinValue.ToString() : dr["FechaReactivacion"].ToString()),
                string.IsNullOrEmpty(dr["MensualReactivacion"].ToString()) ? "" : dr["MensualReactivacion"].ToString(),
                string.IsNullOrEmpty(dr["MotivoReactivacion"].ToString()) ? "" : dr["MotivoReactivacion"].ToString(),
                string.IsNullOrEmpty(dr["UsuarioReactivacion"].ToString()) ? "" : dr["UsuarioReactivacion"].ToString(),
                string.IsNullOrEmpty(dr["OficinaReactivacion"].ToString()) ? "" : dr["OficinaReactivacion"].ToString());
        }
        public static NovedadSuspension ObtenerSuspensionReactivacionDeNovedad(int? idSuspension, int? idReactivacion)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            try
            {
                NovedadSuspension salida = new NovedadSuspension();
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "ObtenerSuspensionReactivacionDeNovedad";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@idNovedadSuspension", (idSuspension == 0) ? null : idSuspension));
                cmd.Parameters.Add(new SqlParameter("@idNovedadReactivacion", (idReactivacion == 0) ? null : idReactivacion));
                cmd.Connection = oCnn;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        salida = obtenerEntidadSuspensionHabilitacion(dr);
                    }
                }
                return salida;
            }
            catch (Exception err)

            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                cmd.Dispose();
            }
        }

        public static bool NovedadSuspensionModificar(NovedadSuspension n, enum_TipoBSR e, out int CodError, out string MsgResultado)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter outparam_CodError = cmd.Parameters.Add("@CodError", SqlDbType.Int);
            outparam_CodError.Direction = ParameterDirection.Output;
            SqlParameter outparam_MsgResultado = cmd.Parameters.Add("@MsgResultado", SqlDbType.NVarChar, 4000);
            outparam_MsgResultado.Direction = ParameterDirection.Output;
            try
            {
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "NovedadSuspensionModificar";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@IdNovedadSuspension", (e == enum_TipoBSR.Suspension)? n.IdEstadoNovedadSuspension : n.IdNovedadReactivacion));
                cmd.Parameters.Add(new SqlParameter("@Expediente", n.Expediente));
                cmd.Parameters.Add(new SqlParameter("@Motivo", (e == enum_TipoBSR.Suspension) ? n.MotivoSuspension : n.MotivoReactivacion));
                cmd.Parameters.Add(new SqlParameter("@Usuario", Utils.GetUsuario().UsuarioDesc.ToString()));
                cmd.Parameters.Add(new SqlParameter("@Oficina", Utils.GetUsuario().Oficina));
                cmd.Parameters.Add(new SqlParameter("@IP", Utils.GetUsuario().Ip.ToString()));
                cmd.Connection = oCnn;
                cmd.ExecuteNonQuery();
                CodError = Convert.ToInt16(cmd.Parameters["@CodError"].Value);
                MsgResultado = Convert.ToString(cmd.Parameters["@MsgResultado"].Value);
                return (CodError == 0);
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (oCnn.State != ConnectionState.Closed)
                {
                    oCnn.Close();
                }
                oCnn.Dispose();
                cmd.Dispose();
            }
        }

        #endregion Bajas y suspensiones
    }
}
