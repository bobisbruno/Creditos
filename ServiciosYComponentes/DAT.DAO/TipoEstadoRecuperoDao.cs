using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Data.SqlClient;
using log4net;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    public class TipoEstadoRecuperoDao
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RecuperoDao).Name);
        public List<TipoEstadoRecupero> Ejecutar_TipoEstadoRecupero_TT()
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            var estadoRecuperoList = new List<TipoEstadoRecupero>();

            try
            {
                command = AdministradorDeConexion.obtenerSqlComand();
                command.CommandText = "TipoEstadoRecupero_TT";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                AdministradorDeConexion.abrirConexion(command);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    estadoRecuperoList.Add(parseToTipoEstadoRecupero(reader));
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
            return estadoRecuperoList;
        }


        private TipoEstadoRecupero parseToTipoEstadoRecupero(SqlDataReader reader)
        {
            return new TipoEstadoRecupero {
                idEstadorecupero = (int)reader["idEstadoRecupero"],
                descripcionEstadoRecupero = reader["descripcionEstadoRecupero"] == DBNull.Value ? string.Empty : (string)reader["descripcionEstadoRecupero"],
                EnDCCyEE = (bool)reader["enDCCyEE"],
                enRegional = (bool)reader["enRegional"],
                FueNotificado = (bool)reader["fueNotificado"],
                Acordado = (bool)reader["acordado"],
                EtapaExtrajudicial = (bool)reader["etapaExtraJudicial"],
                EtapaJudicial = (bool)reader["etapaJudicial"],
                Habilitado = (bool)reader["habilitado"],
                HabilitadoWeb = (bool)reader["habilitadoWEB"],
                Usuario = (string)reader["usuario"],
                Ip = reader["ip"] == DBNull.Value ? null : (string)reader["ip"],
                FechaUltimaModificacion = (DateTime)reader["fultmodificacion"]
            };
        }
    }
}
