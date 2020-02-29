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
    public static class PersonaDato
    {
        public static Persona Persona_Guardar(Persona _unaPersona)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter outparam_CodError = cmd.Parameters.Add("@CodError", SqlDbType.Int);
            outparam_CodError.Direction = ParameterDirection.Output;
            SqlParameter outparam_MsgResultado = cmd.Parameters.Add("@MsgResultado", SqlDbType.NVarChar,4000);
            outparam_MsgResultado.Direction = ParameterDirection.Output;
            try
            {
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "GuardarInicial";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@xml", Serialize(_unaPersona)));
                cmd.Parameters.Add(new SqlParameter("@Usuario", Utils.GetUsuario().UsuarioDesc.ToString()));
                cmd.Parameters.Add(new SqlParameter("@Oficina", Utils.GetUsuario().Oficina));
                cmd.Parameters.Add(new SqlParameter("@IP", Utils.GetUsuario().Ip.ToString()));
                cmd.Connection = oCnn;
                cmd.ExecuteNonQuery();
                int CodError = Convert.ToInt16(cmd.Parameters["@CodError"].Value);
                string MsgResultado = Convert.ToString(cmd.Parameters["@MsgResultado"].Value);
                _unaPersona.Errores.Add(new Mensaje(CodError, MsgResultado));
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

        public static bool TieneDeudaEnArgentaC(long cuil)
        {
            bool tieneDeudaArgentaC = false;
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter outparam_CodError = cmd.Parameters.Add("@CodError", SqlDbType.Int);
            outparam_CodError.Direction = ParameterDirection.Output;
            SqlParameter outparam_MsgResultado = cmd.Parameters.Add("@MsgResultado", SqlDbType.NVarChar, 4000);
            outparam_MsgResultado.Direction = ParameterDirection.Output;
            try
            {
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "TieneDeudaEnArgentaC";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@cuil", cuil));
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        tieneDeudaArgentaC = true;
                    }
                }
                return tieneDeudaArgentaC;
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

        public static bool TieneCodPostalValidoEnPim(int codPostal)
        {
            SqlConnection oCnn = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlParameter outparam_Resultado = cmd.Parameters.Add("@Resultado", SqlDbType.Bit);
            outparam_Resultado.Direction = ParameterDirection.Output;
            try
            {
                oCnn = Conexion.ObtenerConnexionSQL();
                cmd.CommandText = "ValidarCPPim";
                oCnn.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@CP", codPostal));
                cmd.Connection = oCnn;
                cmd.ExecuteNonQuery();
                bool resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                return resultado;
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
    }
}
