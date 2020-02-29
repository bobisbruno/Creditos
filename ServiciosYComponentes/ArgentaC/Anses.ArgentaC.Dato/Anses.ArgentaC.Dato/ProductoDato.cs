using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;
using Anses.ArgentaC.Contrato;
using Anses.ArgentaC.Comun.Domain;

namespace Anses.ArgentaC.Dato
{
    public static class ProductoDato
    {
        public static Persona obtenerOfertaCredito(Persona _unaPersona)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter outparam_CodError = cmd.Parameters.Add("@CodError", SqlDbType.Int);
            outparam_CodError.Direction = ParameterDirection.Output;
            SqlParameter outparam_MsgResultado = cmd.Parameters.Add("@MsgResultado", SqlDbType.NVarChar, 4000);
            outparam_MsgResultado.Direction = ParameterDirection.Output;
            try
            {
                List<Producto> lstProd = new List<Producto>();
                List<MensajeBDD> lstMsjBDD = new List<MensajeBDD>();
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "obtenerOfertaCredito";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Cuil", _unaPersona.Cuil));
                cmd.Parameters.Add(new SqlParameter("@oficina", Utils.GetUsuario().Oficina));
                cmd.Parameters.Add(new SqlParameter("@IdSistema", _unaPersona.IdSistema));
                cmd.Connection = oCnn;
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lstProd.Add(obtenerEntidad(dr, false));
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        lstMsjBDD.Add(obtenerEntidadMensajesBDD(dr));
                    }
                    dr.NextResult();
                    while (dr.Read())
                    {
                        lstProd.Add(obtenerEntidad(dr, true));
                    }
                }
                int CodError = Convert.ToInt16(cmd.Parameters["@CodError"].Value);
                string MsgResultado = Convert.ToString(cmd.Parameters["@MsgResultado"].Value);
                _unaPersona.Errores.Add(new Mensaje(CodError, MsgResultado));
                _unaPersona.Productos = lstProd;
                _unaPersona.MensajesBDD = lstMsjBDD;
                return _unaPersona;
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
        
        public static Persona Preacuerdo_Guardar(Persona _persona, Novedad _novedad)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter outparam_CodError = cmd.Parameters.Add("@CodError", SqlDbType.Int);
            outparam_CodError.Direction = ParameterDirection.Output;
            SqlParameter outparam_MsgResultado = cmd.Parameters.Add("@MsgResultado", SqlDbType.NVarChar, 4000);
            outparam_MsgResultado.Direction = ParameterDirection.Output;
            SqlParameter outparam_idNovedad = cmd.Parameters.Add("@idNovedad", SqlDbType.Int);
            outparam_idNovedad.Direction = ParameterDirection.Output;
            try
            {
                List<Producto> lstProd = new List<Producto>();
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "GenerarPreAcuerdo";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@cuil", _persona.Cuil));
                cmd.Parameters.Add(new SqlParameter("@idSistema", _persona.IdSistema));
                cmd.Parameters.Add(new SqlParameter("@idProducto", _novedad.IdProducto));
                cmd.Parameters.Add(new SqlParameter("@MontoSolicitado", _novedad.ImporteTotal));
                cmd.Parameters.Add(new SqlParameter("@IdTurno", _novedad.Turno.Id));
                cmd.Parameters.Add(new SqlParameter("@idVersionMutuo", _novedad.VersionMutuo));
                cmd.Parameters.Add(new SqlParameter("@OficinaTurno", _novedad.Turno.IdOficina.ToString()));
                cmd.Parameters.Add(new SqlParameter("@FechaTurno", _novedad.Turno.Fecha));
                cmd.Parameters.Add(new SqlParameter("@Usuario", Utils.GetUsuario().UsuarioDesc.ToString()));
                cmd.Parameters.Add(new SqlParameter("@Oficina", Utils.GetUsuario().Oficina));
                cmd.Parameters.Add(new SqlParameter("@IP", Utils.GetUsuario().Ip.ToString()));
                cmd.Connection = oCnn;
                cmd.ExecuteNonQuery();
                int CodError = Convert.ToInt16(cmd.Parameters["@CodError"].Value);
                string MsgResultado = Convert.ToString(cmd.Parameters["@MsgResultado"].Value);
                if (CodError == 0) //le doy a la novedad ingresada el id y estado correspondiente
                {
                    _novedad.Estado = enum_TipoEstadoNovedad.Pre_Acuerdo;
                    _novedad.IdNovedad = Convert.ToInt32(cmd.Parameters["@idNovedad"].Value); ;
                    _persona.SalidaPreacuerdoOK = true;
                }
                else
                    _persona.SalidaPreacuerdoOK = false;

                _persona.Errores.Add(new Mensaje(CodError, MsgResultado));
                _persona.Novedades.Add(_novedad);
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

        private static Producto obtenerEntidad(SqlDataReader dr, bool esRestrictivo)
        {
            return new Producto(Int32.Parse(string.IsNullOrEmpty(dr["idProducto"].ToString()) ? "0" : dr["idProducto"].ToString()),
                                Int32.Parse(string.IsNullOrEmpty(dr["CantCuotas"].ToString()) ? "0" : dr["CantCuotas"].ToString()),
                                decimal.Parse(string.IsNullOrEmpty(dr["Desde"].ToString()) ? "0" : dr["Desde"].ToString()),
                                decimal.Parse(string.IsNullOrEmpty(dr["Hasta"].ToString()) ? "0" : dr["Hasta"].ToString()),
                                decimal.Parse(string.IsNullOrEmpty(dr["TNA"].ToString()) ? "0" : dr["TNA"].ToString()),
                                decimal.Parse(string.IsNullOrEmpty(dr["CFTEA"].ToString()) ? "0" : dr["CFTEA"].ToString()),
                                decimal.Parse(string.IsNullOrEmpty(dr["CFTNA"].ToString()) ? "0" : dr["CFTNA"].ToString()),
                                new List<Cuota>(),
                                esRestrictivo);
        }

        private static MensajeBDD obtenerEntidadMensajesBDD(SqlDataReader dr)
        {
            return new MensajeBDD(Int64.Parse(dr["cuil"].ToString()), dr["Mensaje"].ToString(), bool.Parse(dr["Salvable"].ToString()));
        }
    }
}
