using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Data.SqlClient;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    public class ModalidadDePagoDao
    {
        ILog log = LogManager.GetLogger(typeof(ModalidadDePagoDao).Name);
        public List<ModalidadDePago> EjecutarTipoModalidadPago_TT()
        {

            SqlCommand command = null;
            SqlDataReader reader = null;
            var modalidadDePagoList = new List<ModalidadDePago>();
            try
            {
                command = AdministradorDeConexion.obtenerSqlComand();
                command.CommandText = "TipoModalidadPago_TT";
                AdministradorDeConexion.abrirConexion(command);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    modalidadDePagoList.Add(ParsearAModalidadDePago(reader));
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, "EjecutarTipoModalidadPago_TT", ex.Source, ex.Message));
                throw;
            }
            finally
            {
                AdministradorDeConexion.CerrarConexion();
            }

            return modalidadDePagoList;
        }

        private ModalidadDePago ParsearAModalidadDePago(SqlDataReader reader)
        {
            return new ModalidadDePago((int)reader["id"], reader["descripcionModalidadPago"] == null ? string.Empty : (string)reader["descripcionModalidadPago"]);
        }
    }
}
