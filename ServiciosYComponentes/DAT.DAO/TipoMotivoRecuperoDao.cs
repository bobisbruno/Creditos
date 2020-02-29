using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Data.SqlClient;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    public class TipoMotivoRecuperoDao
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RecuperoDao).Name);
        public List<TipoMotivoRecupero> Ejecutar_TipoMotivoRecupero_TT()
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            var tipoMotivoRecuperoList = new List<TipoMotivoRecupero>();
            try
            {
                command = AdministradorDeConexion.obtenerSqlComand();
                command.CommandText = "TipoMotivoRecupero_TT";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                AdministradorDeConexion.abrirConexion(command);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    tipoMotivoRecuperoList.Add(parseToTipoMotivoRecupero(reader));
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, System.Reflection.MethodBase.GetCurrentMethod(), ex.Source, ex.Message));
                throw;
            }
            finally
            {
                AdministradorDeConexion.CerrarConexion();
            }
            return tipoMotivoRecuperoList;
        }

        private TipoMotivoRecupero parseToTipoMotivoRecupero(SqlDataReader reader)
        {
            return new TipoMotivoRecupero
            {
                Id = (int)reader["idMotivoRecupero"],
                DescripcionMotivoRecupero = reader["descripcionMotivoRecupero"] == DBNull.Value ? null : (string)reader["descripcionMotivoRecupero"],
                Usuario = reader["usuario"] == DBNull.Value ? null : (string)reader["usuario"],
                Habilitado = (bool)reader["habilitado"],
                HabilitadoWEB = (bool)reader["habilitadoWEB"],
                Ip = reader["ip"] == DBNull.Value ? null : (string)reader["ip"],
                FechaUltimaModificacion = (DateTime)reader["fultmodificacion"]
            };
        }
    }
}
