using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ar.Gov.Anses.Microinformatica.DAT.Entidades;
using System.Data.SqlClient;
using log4net;
using System.Data;

namespace Ar.Gov.Anses.Microinformatica.DAT.DAO
{
    public class RecuperoDao
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RecuperoDao).Name);

        public GestionRecuperoForm Ejecutar_Recupero_T(FiltroDeRecuperos filtro)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            var recuperoList = new List<Recupero>();
            SqlParameter cantidadDeRegistros = null;
            try
            {
                command = AdministradorDeConexion.obtenerSqlComand();
                command.CommandText = "Recupero_T";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@cuil", filtro.Cuil);
                command.Parameters.AddWithValue("@idMotivoRecupero", filtro.Motivo.Id);
                command.Parameters.AddWithValue("@idEstadoRecupero", filtro.Estado.Id);
                command.Parameters.AddWithValue("@valorResidualDesde", filtro.ValorResidualDesde);
                command.Parameters.AddWithValue("@valorResidualHasta", filtro.ValorResidualHasta);
                cantidadDeRegistros = new SqlParameter("@totalRecuperos", SqlDbType.Int);
                cantidadDeRegistros.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(cantidadDeRegistros);
                AdministradorDeConexion.abrirConexion(command);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    recuperoList.Add(ConvertirARecupero(reader));
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, "Ejecutar_Recupero_T", ex.Source, ex.Message));
                throw;
            }
            finally
            {
                AdministradorDeConexion.CerrarConexion();
            }
            int registros = int.Parse(cantidadDeRegistros.Value.ToString());
            return new GestionRecuperoForm { CantidadTotalDeRegistros = registros, RecuperosList = recuperoList };
        }

        public decimal Ejecutar_Recupero_ValorMinimo_T(int idPrestador)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            SqlParameter cantidadDeRegistros = null;
            decimal valorMinimoDeRecupero = decimal.Zero;
            try
            {
                command = AdministradorDeConexion.obtenerSqlComand();
                command.CommandText = "Recupero_ValorMinimo_T";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@idPrestador", idPrestador);
                cantidadDeRegistros = new SqlParameter("@totalRecuperos", SqlDbType.Int);
                cantidadDeRegistros.Direction = System.Data.ParameterDirection.Output;
                command.Parameters.Add(cantidadDeRegistros);
                AdministradorDeConexion.abrirConexion(command);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    valorMinimoDeRecupero = (decimal)reader["ValorMinimo"];
                }
                return valorMinimoDeRecupero;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, "Ejecutar_Recupero_ValorMinimo_T", ex.Source, ex.Message));
                throw;
            }
            finally
            {
                AdministradorDeConexion.CerrarConexion();
            }
        }

        public RecuperoDetalleForm Ejecutar_Recupero_TXId(decimal idRecupero, out decimal valorResidualTotal)
        {
            SqlCommand command = null;
            SqlDataReader reader = null;
            var novedadesList = new List<DatosDeNovedadDeRecupero>();
            var beneficioDisponibleList = new List<BeneficioDisponible>();
            valorResidualTotal = 0;

            try
            {
                command = AdministradorDeConexion.obtenerSqlComand();
                command.CommandText = "Recupero_TXId";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@idRecupero", idRecupero);
                command.Parameters.AddWithValue("@valorResidualTotal", valorResidualTotal);
                AdministradorDeConexion.abrirConexion(command);
                reader = command.ExecuteReader();
                reader.NextResult();
                while (reader.Read())
                {
                    novedadesList.Add(ConvertirADatosDeNovedadDeRecupero(reader));
                }

                reader.NextResult();
                while (reader.Read())
                {
                    beneficioDisponibleList.Add(ConvertirABeneficioDisponible(reader));
                }
            }
            catch (Exception ex)
            {
                log.Error(string.Format("{0}->{1}-> - Error:{2}->{3}", DateTime.Now, "Ejecutar_Recupero_TXId", ex.Source, ex.Message));
                throw;
            }
            finally
            {
                AdministradorDeConexion.CerrarConexion();
            }

            return new RecuperoDetalleForm(novedadesList, beneficioDisponibleList);
        }

        private Recupero ConvertirARecupero(SqlDataReader reader)
        {
            return new Recupero
            {
                Cuil = (decimal)reader["Cuil"],
                ApellidoYNombre = (string)reader["apellidoyNombre"],
                ValorResidual = (Decimal)reader["ValorResidual"],
                IdMotivoRecupero = (int)reader["idMotivoRecupero"],
                DescripcionMotivoDeRecupero = (string)reader["descripcionMotivoRecupero"],
                IdEstadoDeRecupero = (int)reader["idEstadoRecupero"],
                DescripcionEstadoDeRecupero = (string)reader["descripcionEstadoRecupero"],
                FechaDeEstadoDeRecupero = (DateTime)reader["FEstadoRecupero"],
                CantidadDeCreditos = (int)reader["CantCreditos"],
                IdDeRecupero = (decimal)reader["IdRecupero"]
            };
        }

        private DatosDeNovedadDeRecupero ConvertirADatosDeNovedadDeRecupero(SqlDataReader reader)
        {
          return  new DatosDeNovedadDeRecupero
            {
                IdNovedad = (long)reader["idNovedad"],
                VinculacionNovedadRecupero = (DateTime)reader["fVinculacionNovRecupero"],
                FechaDeNovedad = reader["nda_fecnov"] != DBNull.Value ? (DateTime?)reader["nda_fecnov"] : null,
                ValorResidual = reader["valorResidual"] != DBNull.Value ? (Decimal?)reader["valorResidual"] : null,
                CodigoConceptoLiquidacion = (int)reader["nda_codconceptoliq"],
                RazonSocial = (string)reader["RazonSocial"],
                MontoDelPrestamo = reader["nda_montoPrestamo"] != DBNull.Value ? (Decimal?)reader["nda_montoPrestamo"] : null,
                CantidadDeCuotas = reader["nda_cantcuotas"] != DBNull.Value ? (int?)reader["nda_cantcuotas"] : null,
                IdBeneficiario = reader["nda_idBeneficiario"] != DBNull.Value ? (long?)reader["nda_idBeneficiario"] : null,
                PeriodoBajaBeneficiario = reader["periodoBajaBeneficiario"] != DBNull.Value ? (int?)reader["periodoBajaBeneficiario"] : null,
                IdMotivoBajaBeneficiario = reader["idMotivoBajaBeneficiario"] != DBNull.Value ? (int?)reader["idMotivoBajaBeneficiario"] : null,
                MotivoBajaBeneficiario = (string)reader["MotivoBajaBeneficiario"],
                OficinaDeBaja = (string)reader["oficinaBaja"],
                PeriodoDeReactivacion = reader["periodoReactivacion"] !=  DBNull.Value ? (int?)reader["periodoReactivacion"] : null,
                IdPrestador = (long)reader["idPrestador"],
                RecuperaSobreConcepto = reader["recuperaSobreConcepto"] != DBNull.Value ? (int?)reader["recuperaSobreConcepto"] : null               
            };
        }

        private BeneficioDisponible ConvertirABeneficioDisponible(SqlDataReader reader)
        {
            return new BeneficioDisponible((long)reader["IdBeneficiario"], (bool)reader["AfectacionDisponible"]);
        }

        private RecuperoDetalle ConvertirARecuperoDetalle(SqlDataReader reader)
        {
            return new RecuperoDetalle();
        }      

        private TipoEstadoRecupero parseToTipoEstadoRecupero(SqlDataReader reader)
        {
            return new TipoEstadoRecupero
            {
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
