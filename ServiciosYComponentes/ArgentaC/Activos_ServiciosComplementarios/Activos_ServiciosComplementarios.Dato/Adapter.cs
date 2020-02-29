using Activos_ServiciosComplementarios.Entidades;
using IBM.Data.DB2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Activos_ServiciosComplementarios.Dato
{
    public class Adapter
    {
        public static DatosDeConsultaAltaTemprana toConsultaAltaTempranaResponse(DB2DataReader reader)
        {

            return new DatosDeConsultaAltaTemprana()
            {
                Cuit = (decimal)reader["CUIT"],
                Cuil = (decimal)reader["CUIL"],
                FechaAltaTemprana = (DateTime)reader["F_ALTA_TEMPRANA"],
                DescripcionAbreviadaMovimiento = (string)reader["DA_MOVIMIENTO"],
                FechaFinRelacionLaboral = reader["F_FIN_RLABORAL"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(reader["F_FIN_RLABORAL"])
            };

        }

        public static DatosDeConsultaCondenadoProcesado toConsultaCondenadoProcesadoResponse(DB2DataReader reader)
        {

            return new DatosDeConsultaCondenadoProcesado()
            {
                Cuil = (decimal)reader["CUIL"],
                FechaCarga = (DateTime)reader["F_CARGA"],
                CodigoEstado = (short)reader["C_ESTADO"],
                PeriodoLiquidacion = (decimal)reader["PE_LIQUIDACION"],
                TipoInterno = (string)reader["TIPO_INTERNO"],
                Cuit = reader["CUIT"] == DBNull.Value ? null : (decimal?)reader["CUIT"]

            };
        }
    }
}
